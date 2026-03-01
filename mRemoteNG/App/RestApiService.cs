using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Tree;

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public sealed class RestApiService : IDisposable
    {
        private HttpListener? _listener;
        private CancellationTokenSource? _cts;
        private Task? _listenTask;
        private bool _disposed;

        public int Port { get; }
        public string ApiKey { get; }
        public bool IsRunning => _listener?.IsListening == true;

        public RestApiService(int port, string apiKey)
        {
            if (port < 1 || port > 65535)
                throw new ArgumentOutOfRangeException(nameof(port));
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key must not be empty.", nameof(apiKey));

            Port = port;
            ApiKey = apiKey;
        }

        public void Start()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (IsRunning) return;

            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{Port}/");
            _cts = new CancellationTokenSource();

            try
            {
                _listener.Start();
            }
            catch (HttpListenerException ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    $"REST API failed to start on port {Port}: {ex.Message}");
                return;
            }

            _listenTask = Task.Run(() => ListenLoop(_cts.Token));
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                $"REST API started on http://localhost:{Port}/");
        }

        public void Stop()
        {
            if (!IsRunning) return;

            _cts?.Cancel();
            _listener?.Stop();

            try
            {
                _listenTask?.Wait(TimeSpan.FromSeconds(5));
            }
            catch (AggregateException)

            {

                _ = 0; // Intentionally empty — task may be cancelled

            }

            _listener?.Close();
            _listener = null;
            _cts?.Dispose();
            _cts = null;
            _listenTask = null;

            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "REST API stopped.");
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            Stop();
        }

        private async Task ListenLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested && _listener?.IsListening == true)
            {
                HttpListenerContext ctx;
                try
                {
                    ctx = await _listener.GetContextAsync().ConfigureAwait(false);
                }
                catch (ObjectDisposedException) { break; }
                catch (HttpListenerException) { break; }

                try
                {
                    await HandleRequest(ctx).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                        $"REST API error: {ex.Message}");
                    try { WriteError(ctx.Response, 500, "Internal server error"); } catch { }
                }
            }
        }

        private async Task HandleRequest(HttpListenerContext ctx)
        {
            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse resp = ctx.Response;

            // CORS headers for local dev tools
            resp.Headers.Add("Access-Control-Allow-Origin", "http://localhost");
            resp.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            resp.Headers.Add("Access-Control-Allow-Headers", "X-API-Key, Content-Type");

            if (string.Equals(req.HttpMethod, "OPTIONS", StringComparison.Ordinal))
            {
                resp.StatusCode = 204;
                resp.Close();
                return;
            }

            // Authenticate
            string? providedKey = req.Headers["X-API-Key"];
            if (!string.Equals(providedKey, ApiKey, StringComparison.Ordinal))
            {
                WriteError(resp, 401, "Invalid or missing API key. Provide X-API-Key header.");
                return;
            }

            string path = req.Url?.AbsolutePath?.TrimEnd('/') ?? "";
            string method = req.HttpMethod;

            if (string.Equals(path, "/api/status", StringComparison.Ordinal) && string.Equals(method, "GET", StringComparison.Ordinal))
            {
                HandleStatus(resp);
            }
            else if (string.Equals(path, "/api/connections", StringComparison.Ordinal) && string.Equals(method, "GET", StringComparison.Ordinal))
            {
                HandleGetConnections(resp);
            }
            else if (path.StartsWith("/api/connections/", StringComparison.Ordinal) && string.Equals(method, "GET", StringComparison.Ordinal))
            {
                string id = path["/api/connections/".Length..];
                HandleGetConnection(resp, id);
            }
            else if (string.Equals(path, "/api/connections", StringComparison.Ordinal) && string.Equals(method, "POST", StringComparison.Ordinal))
            {
                await HandleCreateConnection(req, resp).ConfigureAwait(false);
            }
            else if (path.StartsWith("/api/connections/", StringComparison.Ordinal) && string.Equals(method, "PUT", StringComparison.Ordinal))
            {
                string id = path["/api/connections/".Length..];
                await HandleUpdateConnection(req, resp, id).ConfigureAwait(false);
            }
            else if (path.StartsWith("/api/connections/", StringComparison.Ordinal) && string.Equals(method, "DELETE", StringComparison.Ordinal))
            {
                string id = path["/api/connections/".Length..];
                HandleDeleteConnection(resp, id);
            }
            else if (string.Equals(path, "/api/tree", StringComparison.Ordinal) && string.Equals(method, "GET", StringComparison.Ordinal))
            {
                HandleGetTree(resp);
            }
            else
            {
                WriteError(resp, 404, $"Unknown endpoint: {method} {path}");
            }
        }

        #region Handlers

        private static void HandleStatus(HttpListenerResponse resp)
        {
            var model = Runtime.ConnectionsService.ConnectionTreeModel;
            int count = model?.GetRecursiveChildList().Count ?? 0;

            WriteJson(resp, new
            {
                status = "ok",
                version = Application.ProductVersion ?? "unknown",
                connectionsLoaded = Runtime.ConnectionsService.IsConnectionsFileLoaded,
                connectionCount = count
            });
        }

        private static void HandleGetConnections(HttpListenerResponse resp)
        {
            ConnectionTreeModel? model = Runtime.ConnectionsService.ConnectionTreeModel;
            if (model == null)
            {
                WriteError(resp, 503, "Connections not loaded yet.");
                return;
            }

            var connections = model.GetRecursiveChildList()
                .Where(c => !c.IsContainer)
                .Select(ToDto)
                .ToList();

            WriteJson(resp, connections);
        }

        private static void HandleGetConnection(HttpListenerResponse resp, string id)
        {
            ConnectionTreeModel? model = Runtime.ConnectionsService.ConnectionTreeModel;
            if (model == null)
            {
                WriteError(resp, 503, "Connections not loaded yet.");
                return;
            }

            ConnectionInfo? conn = model.FindConnectionById(id);
            if (conn == null)
            {
                WriteError(resp, 404, $"Connection '{id}' not found.");
                return;
            }

            WriteJson(resp, ToDto(conn));
        }

        private static async Task HandleCreateConnection(HttpListenerRequest req, HttpListenerResponse resp)
        {
            ConnectionTreeModel? model = Runtime.ConnectionsService.ConnectionTreeModel;
            if (model == null || model.RootNodes.Count == 0)
            {
                WriteError(resp, 503, "Connections not loaded yet.");
                return;
            }

            ConnectionDto? dto;
            try
            {
                dto = await ReadBody<ConnectionDto>(req).ConfigureAwait(false);
            }
            catch (JsonException ex)
            {
                WriteError(resp, 400, $"Invalid JSON: {ex.Message}");
                return;
            }

            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
            {
                WriteError(resp, 400, "Name is required.");
                return;
            }

            // Find the parent container (default = first root)
            ContainerInfo parent;
            if (!string.IsNullOrWhiteSpace(dto.ParentId))
            {
                ConnectionInfo? parentNode = model.FindConnectionById(dto.ParentId);
                if (parentNode is ContainerInfo container)
                    parent = container;
                else
                {
                    WriteError(resp, 400, $"Parent '{dto.ParentId}' not found or is not a folder.");
                    return;
                }
            }
            else
            {
                parent = model.RootNodes[0];
            }

            ConnectionInfo newConn = new();
            ApplyDto(newConn, dto);

            // Must add child on UI thread since it fires collection changed events
            InvokeOnUiThread(() => parent.AddChild(newConn));

            Runtime.ConnectionsService.SaveConnectionsAsync();

            resp.StatusCode = 201;
            WriteJson(resp, ToDto(newConn));
        }

        private static async Task HandleUpdateConnection(HttpListenerRequest req, HttpListenerResponse resp, string id)
        {
            ConnectionTreeModel? model = Runtime.ConnectionsService.ConnectionTreeModel;
            if (model == null)
            {
                WriteError(resp, 503, "Connections not loaded yet.");
                return;
            }

            ConnectionInfo? conn = model.FindConnectionById(id);
            if (conn == null)
            {
                WriteError(resp, 404, $"Connection '{id}' not found.");
                return;
            }

            ConnectionDto? dto;
            try
            {
                dto = await ReadBody<ConnectionDto>(req).ConfigureAwait(false);
            }
            catch (JsonException ex)
            {
                WriteError(resp, 400, $"Invalid JSON: {ex.Message}");
                return;
            }

            if (dto == null)
            {
                WriteError(resp, 400, "Request body required.");
                return;
            }

            InvokeOnUiThread(() => ApplyDto(conn, dto));

            Runtime.ConnectionsService.SaveConnectionsAsync();

            WriteJson(resp, ToDto(conn));
        }

        private static void HandleDeleteConnection(HttpListenerResponse resp, string id)
        {
            ConnectionTreeModel? model = Runtime.ConnectionsService.ConnectionTreeModel;
            if (model == null)
            {
                WriteError(resp, 503, "Connections not loaded yet.");
                return;
            }

            ConnectionInfo? conn = model.FindConnectionById(id);
            if (conn == null)
            {
                WriteError(resp, 404, $"Connection '{id}' not found.");
                return;
            }

            InvokeOnUiThread(() => ConnectionTreeModel.DeleteNode(conn));

            Runtime.ConnectionsService.SaveConnectionsAsync();

            WriteJson(resp, new { deleted = true, id });
        }

        private static void HandleGetTree(HttpListenerResponse resp)
        {
            ConnectionTreeModel? model = Runtime.ConnectionsService.ConnectionTreeModel;
            if (model == null)
            {
                WriteError(resp, 503, "Connections not loaded yet.");
                return;
            }

            var tree = model.RootNodes.Select(ToTreeNode).ToList();
            WriteJson(resp, tree);
        }

        #endregion

        #region DTO & Mapping

        private sealed class ConnectionDto
        {
            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("hostname")]
            public string? Hostname { get; set; }

            [JsonPropertyName("port")]
            public int? Port { get; set; }

            [JsonPropertyName("protocol")]
            public string? Protocol { get; set; }

            [JsonPropertyName("username")]
            public string? Username { get; set; }

            [JsonPropertyName("password")]
            public string? Password { get; set; }

            [JsonPropertyName("domain")]
            public string? Domain { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }

            [JsonPropertyName("panel")]
            public string? Panel { get; set; }

            [JsonPropertyName("parentId")]
            public string? ParentId { get; set; }
        }

        private static object ToDto(ConnectionInfo c)
        {
            return new
            {
                id = c.ConstantID,
                name = c.Name,
                hostname = c.Hostname,
                port = c.Port,
                protocol = c.Protocol.ToString(),
                username = c.Username,
                domain = c.Domain,
                description = c.Description,
                panel = c.Panel,
                isContainer = c.IsContainer,
                parentId = c.Parent?.ConstantID ?? ""
            };
        }

        private static object ToTreeNode(ContainerInfo container)
        {
            return new
            {
                id = container.ConstantID,
                name = container.Name,
                isContainer = true,
                children = container.Children.Select(child =>
                    child is ContainerInfo subContainer
                        ? ToTreeNode(subContainer)
                        : ToDto(child)
                ).ToList<object>()
            };
        }

        private static void ApplyDto(ConnectionInfo conn, ConnectionDto dto)
        {
            if (!string.IsNullOrEmpty(dto.Name))
                conn.Name = dto.Name;
            if (dto.Hostname != null)
                conn.Hostname = dto.Hostname;
            if (dto.Port.HasValue)
                conn.Port = dto.Port.Value;
            if (!string.IsNullOrEmpty(dto.Protocol) && Enum.TryParse<ProtocolType>(dto.Protocol, true, out var pt))
                conn.Protocol = pt;
            if (dto.Username != null)
                conn.Username = dto.Username;
            if (dto.Password != null)
                conn.Password = dto.Password;
            if (dto.Domain != null)
                conn.Domain = dto.Domain;
            if (dto.Description != null)
                conn.Description = dto.Description;
            if (dto.Panel != null)
                conn.Panel = dto.Panel;
        }

        #endregion

        #region Helpers

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false
        };

        private static void WriteJson(HttpListenerResponse resp, object data)
        {
            byte[] body = JsonSerializer.SerializeToUtf8Bytes(data, JsonOptions);
            resp.ContentType = "application/json; charset=utf-8";
            resp.ContentLength64 = body.Length;
            resp.OutputStream.Write(body, 0, body.Length);
            resp.Close();
        }

        private static void WriteError(HttpListenerResponse resp, int statusCode, string message)
        {
            resp.StatusCode = statusCode;
            WriteJson(resp, new { error = message });
        }

        private static async Task<T?> ReadBody<T>(HttpListenerRequest req) where T : class
        {
            using StreamReader reader = new(req.InputStream, Encoding.UTF8);
            string json = await reader.ReadToEndAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(json, JsonOptions);
        }

        private static void InvokeOnUiThread(Action action)
        {
            Form? mainForm = Application.OpenForms.Count > 0 ? Application.OpenForms[0] : null;
            if (mainForm != null && mainForm.InvokeRequired)
                mainForm.Invoke(action);
            else
                action();
        }

        public static string GenerateApiKey()
        {
            byte[] bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes).Replace("+", "", StringComparison.Ordinal).Replace("/", "", StringComparison.Ordinal).Replace("=", "", StringComparison.Ordinal)[..32];
        }

        #endregion
    }
}
