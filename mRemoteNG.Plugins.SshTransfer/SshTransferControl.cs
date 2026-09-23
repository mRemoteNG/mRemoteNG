using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.PluginContracts;
using Renci.SshNet.Common;

namespace mRemoteNG.Plugins.SshTransfer;

internal sealed class SshTransferControl : UserControl
{
    private readonly IPluginContext _pluginContext;
    private readonly Resources _resources;
    private GroupBox _grpConnection = null!;
    private GroupBox _grpFiles = null!;
    private Label _lblHost = null!;
    private Label _lblPort = null!;
    private Label _lblUser = null!;
    private Label _lblPassword = null!;
    private Label _lblProtocol = null!;
    private Label _lblLocalFile = null!;
    private Label _lblRemoteFile = null!;
    private TextBox _txtHost = null!;
    private TextBox _txtPort = null!;
    private TextBox _txtUser = null!;
    private TextBox _txtPassword = null!;
    private TextBox _txtLocalFile = null!;
    private TextBox _txtRemoteFile = null!;
    private RadioButton _radProtScp = null!;
    private RadioButton _radProtSftp = null!;
    private Button _btnBrowse = null!;
    private Button _btnTransfer = null!;
    private ProgressBar _pbStatus = null!;
    private OpenFileDialog _openFileDialog = null!;
    private SecureTransferService? _transfer;
    private int _maxVal;
    private int _curVal;

    public SshTransferControl(IPluginContext pluginContext)
    {
        _pluginContext = pluginContext ?? throw new ArgumentNullException(nameof(pluginContext));
        _resources = new Resources(_pluginContext);
        InitializeComponent();
        ApplyLanguage();
    }

    public void PopulateFromConnection(IPluginConnection? connection)
    {
        if (connection is null || connection.IsContainer)
        {
            return;
        }

        _txtHost.Text = connection.Hostname;
        _txtUser.Text = BuildUserName(connection);
        _txtPassword.Text = connection.Password;
        _txtPort.Text = connection.Port > 0 ? connection.Port.ToString() : "22";

        string protocolId = connection.ProtocolId;
        if (string.Equals(protocolId, "ssh1", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(protocolId, "ssh2", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(protocolId, "ssh", StringComparison.OrdinalIgnoreCase))
        {
            _radProtScp.Checked = true;
        }
    }

    private static string BuildUserName(IPluginConnection connection)
    {
        if (string.IsNullOrWhiteSpace(connection.Domain))
        {
            return connection.Username;
        }

        if (string.IsNullOrWhiteSpace(connection.Username))
        {
            return connection.Domain;
        }

        return $@"{connection.Domain}\{connection.Username}";
    }

    private void InitializeComponent()
    {
        Dock = DockStyle.Fill;
        _openFileDialog = new OpenFileDialog { Filter = @"All Files (*.*)|*.*", CheckFileExists = true };
        _grpConnection = new GroupBox { Location = new Point(12, 12), Size = new Size(668, 154), FlatStyle = FlatStyle.Flat };
        _grpFiles = new GroupBox { Location = new Point(12, 172), Size = new Size(668, 175), FlatStyle = FlatStyle.Flat };
        _lblHost = new Label { AutoSize = true, Location = new Point(6, 27) };
        _lblPort = new Label { AutoSize = true, Location = new Point(228, 115) };
        _lblUser = new Label { AutoSize = true, Location = new Point(6, 58) };
        _lblPassword = new Label { AutoSize = true, Location = new Point(6, 88) };
        _lblProtocol = new Label { AutoSize = true, Location = new Point(6, 117) };
        _lblLocalFile = new Label { AutoSize = true, Location = new Point(6, 30) };
        _lblRemoteFile = new Label { AutoSize = true, Location = new Point(6, 67) };
        _txtHost = new TextBox { BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 8.25F), Location = new Point(105, 19), Size = new Size(471, 22) };
        _txtPort = new TextBox { BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 8.25F), Location = new Point(271, 110), Size = new Size(40, 22), Text = "22", TextAlign = HorizontalAlignment.Center };
        _txtUser = new TextBox { BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 8.25F), Location = new Point(105, 51), Size = new Size(471, 22) };
        _txtPassword = new TextBox { BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 8.25F), Location = new Point(105, 82), Size = new Size(471, 22), UseSystemPasswordChar = true };
        _radProtScp = new RadioButton { AutoSize = true, FlatStyle = FlatStyle.Flat, Checked = true, Location = new Point(105, 113) };
        _radProtSftp = new RadioButton { AutoSize = true, FlatStyle = FlatStyle.Flat, Location = new Point(164, 113) };
        _txtLocalFile = new TextBox { BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 8.25F), Location = new Point(105, 28), Size = new Size(455, 22) };
        _btnBrowse = new Button { FlatStyle = FlatStyle.Flat, Location = new Point(566, 28), Size = new Size(81, 22) };
        _btnBrowse.Click += BtnBrowse_Click;
        _txtRemoteFile = new TextBox { BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 8.25F), Location = new Point(105, 60), Size = new Size(542, 22) };
        _btnTransfer = new Button { FlatStyle = FlatStyle.Flat, Location = new Point(562, 145), Size = new Size(100, 24) };
        if (_resources.GetImage("SyncArrow_16x") is Image image)
        {
            _btnTransfer.Image = image;
            _btnTransfer.ImageAlign = ContentAlignment.MiddleLeft;
        }
        _btnTransfer.Click += BtnTransfer_Click;
        _pbStatus = new ProgressBar { Location = new Point(12, 357), Size = new Size(668, 23) };

        _grpConnection.Controls.AddRange([_lblHost, _lblPort, _lblUser, _lblPassword, _lblProtocol, _txtHost, _txtPort, _txtUser, _txtPassword, _radProtScp, _radProtSftp]);
        _grpFiles.Controls.AddRange([_lblLocalFile, _txtLocalFile, _btnBrowse, _lblRemoteFile, _txtRemoteFile, _btnTransfer]);
        Controls.AddRange([_grpConnection, _grpFiles, _pbStatus]);
        Size = new Size(692, 392);
    }

    private void ApplyLanguage()
    {
        _grpFiles.Text = _resources.GetString("Files", "Files");
        _lblLocalFile.Text = _resources.GetString("LocalFile", "Local file") + ":";
        _lblRemoteFile.Text = _resources.GetString("RemoteFile", "Remote file") + ":";
        _btnBrowse.Text = _resources.GetString("_Browse", "Browse");
        _grpConnection.Text = _resources.GetString("Connection", "Connection");
        _lblProtocol.Text = _resources.GetString("Protocol", "Protocol") + ":";
        _lblPassword.Text = _resources.GetString("Password", "Password") + ":";
        _lblUser.Text = _resources.GetString("User", "User") + ":";
        _lblPort.Text = _resources.GetString("Port", "Port") + ":";
        _lblHost.Text = _resources.GetString("Host", "Host") + ":";
        _btnTransfer.Text = _resources.GetString("Transfer", "Transfer");
        _radProtScp.Text = "SCP";
        _radProtSftp.Text = "SFTP";
    }

    private void BtnBrowse_Click(object? sender, EventArgs e)
    {
        if (_openFileDialog.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(_openFileDialog.FileName))
        {
            _txtLocalFile.Text = _openFileDialog.FileName;
        }
    }

    private void BtnTransfer_Click(object? sender, EventArgs e)
    {
        StartTransfer(_radProtScp.Checked ? SshTransferProtocol.Scp : SshTransferProtocol.Sftp);
    }

    private void StartTransfer(SshTransferProtocol protocol)
    {
        if (!AllFieldsSet())
        {
            _pluginContext.Messages.Error(_resources.GetString("PleaseFillAllFields", "Please fill all fields."));
            return;
        }

        if (!File.Exists(_txtLocalFile.Text))
        {
            _pluginContext.Messages.Warning(_resources.GetString("LocalFileDoesNotExist", "Local file does not exist."));
            return;
        }

        if (!int.TryParse(_txtPort.Text, out int port))
        {
            _pluginContext.Messages.Error(_resources.GetString("Port", "Port") + " must be a number.");
            return;
        }

        try
        {
            _transfer = new SecureTransferService(_txtHost.Text, _txtUser.Text, _txtPassword.Text, port, protocol, _txtLocalFile.Text, _txtRemoteFile.Text);
            _transfer.Connect();

            if (protocol == SshTransferProtocol.Scp && _transfer.ScpClient is not null)
            {
                _transfer.ScpClient.Uploading += ScpClientOnUploading;
            }
            else
            {
                _transfer.AsyncCallback = _ => _pluginContext.Messages.Info("SFTP AsyncCallback completed.");
            }

            Thread thread = new(StartTransferBackground)
            {
                IsBackground = true,
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
        catch (Exception ex)
        {
            _pluginContext.Messages.Error(_resources.GetString("SshTransferFailed", "SSH transfer failed.") + Environment.NewLine + ex.Message);
            _transfer?.Disconnect();
            _transfer?.Dispose();
        }
    }

    private void StartTransferBackground()
    {
        try
        {
            DisableButtons();
            _pluginContext.Messages.Info($"Transfer of {Path.GetFileName(_transfer!.SourceFile)} started.");
            _transfer.Upload();

            if (_transfer.Protocol == SshTransferProtocol.Sftp && _transfer.AsyncResult is not null)
            {
                FileInfo fileInfo = new(_transfer.SourceFile);
                while (!_transfer.AsyncResult.IsCompleted)
                {
                    int max = fileInfo.Length > int.MaxValue ? Convert.ToInt32(fileInfo.Length / 1024) : Convert.ToInt32(fileInfo.Length);
                    int cur = fileInfo.Length > int.MaxValue ? Convert.ToInt32(_transfer.AsyncResult.UploadedBytes / 1024) : Convert.ToInt32(_transfer.AsyncResult.UploadedBytes);
                    UpdateProgress(cur, max);
                    Thread.Sleep(50);
                }
            }

            _pluginContext.Messages.Info($"Transfer of {Path.GetFileName(_transfer.SourceFile)} completed.");
            _transfer.Disconnect();
            _transfer.Dispose();
            EnableButtons();
        }
        catch (Exception ex)
        {
            _pluginContext.Messages.Error(_resources.GetString("SshBackgroundTransferFailed", "SSH background transfer failed.") + Environment.NewLine + ex.Message);
            _transfer?.Disconnect();
            _transfer?.Dispose();
            EnableButtons();
        }
    }

    private void ScpClientOnUploading(object? sender, ScpUploadEventArgs e)
    {
        int max = e.Size > int.MaxValue ? Convert.ToInt32(e.Size / 1024) : Convert.ToInt32(e.Size);
        int cur = e.Size > int.MaxValue ? Convert.ToInt32(e.Uploaded / 1024) : Convert.ToInt32(e.Uploaded);
        UpdateProgress(cur, max);
    }

    private bool AllFieldsSet()
    {
        if (string.IsNullOrWhiteSpace(_txtHost.Text) || string.IsNullOrWhiteSpace(_txtPort.Text) || string.IsNullOrWhiteSpace(_txtUser.Text) || string.IsNullOrWhiteSpace(_txtLocalFile.Text) || string.IsNullOrWhiteSpace(_txtRemoteFile.Text))
        {
            return false;
        }

        if (string.IsNullOrEmpty(_txtPassword.Text))
        {
            if (MessageBox.Show(this, _resources.GetString("EmptyPasswordContinue", "Password is empty. Continue?"), "Question?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return false;
            }
        }

        if (_txtRemoteFile.Text.EndsWith("/") || _txtRemoteFile.Text.EndsWith(@"\"))
        {
            _txtRemoteFile.Text += Path.GetFileName(_txtLocalFile.Text);
        }

        return true;
    }

    private void UpdateProgress(int transferredBytes, int totalBytes)
    {
        _maxVal = totalBytes;
        _curVal = transferredBytes;
        if (_pbStatus.InvokeRequired)
        {
            _pbStatus.Invoke(new Action(UpdateStatus));
            return;
        }

        UpdateStatus();
    }

    private void UpdateStatus()
    {
        _pbStatus.Maximum = Math.Max(1, _maxVal);
        _pbStatus.Value = Math.Min(_pbStatus.Maximum, Math.Max(0, _curVal));
    }

    private void EnableButtons()
    {
        if (_btnTransfer.InvokeRequired)
        {
            _btnTransfer.Invoke(new Action(EnableButtons));
            return;
        }

        _btnTransfer.Enabled = true;
    }

    private void DisableButtons()
    {
        if (_btnTransfer.InvokeRequired)
        {
            _btnTransfer.Invoke(new Action(DisableButtons));
            return;
        }

        _btnTransfer.Enabled = false;
    }
}
