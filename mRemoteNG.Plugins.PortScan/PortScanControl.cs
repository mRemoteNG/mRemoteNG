using System.Net;
using System.Windows.Forms;
using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins.PortScan;

internal sealed class PortScanControl : UserControl
{
    private readonly IPluginContext _pluginContext;
    private readonly BindingSource _bindingSource = new();
    private readonly ComboBox _protocolComboBox = new();
    private readonly CheckBox _scanDefaultPortsCheckBox = new();
    private readonly DataGridView _resultsGrid = new();
    private readonly Button _importButton = new();
    private readonly TextBox _startIpTextBox = new();
    private readonly TextBox _endIpTextBox = new();
    private readonly NumericUpDown _startPortUpDown = new();
    private readonly NumericUpDown _endPortUpDown = new();
    private readonly NumericUpDown _timeoutUpDown = new();
    private readonly Button _scanButton = new();
    private readonly ProgressBar _progressBar = new();
    private readonly PortScanService _portScanService = new();
    private CancellationTokenSource? _scanCancellation;
    private bool _scanning;

    public PortScanControl(IPluginContext pluginContext)
    {
        _pluginContext = pluginContext ?? throw new ArgumentNullException(nameof(pluginContext));
        InitializeComponent();
        ApplyLanguage();
    }

    private void InitializeComponent()
    {
        Dock = DockStyle.Fill;

        TableLayoutPanel rootLayout = new()
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
        };
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        TableLayoutPanel inputLayout = new()
        {
            AutoSize = true,
            ColumnCount = 6,
            Dock = DockStyle.Fill,
            Padding = new Padding(8),
        };

        inputLayout.Controls.Add(BuildLabel("FirstIpLabel"), 0, 0);
        inputLayout.Controls.Add(_startIpTextBox, 1, 0);
        inputLayout.Controls.Add(BuildLabel("LastIpLabel"), 2, 0);
        inputLayout.Controls.Add(_endIpTextBox, 3, 0);
        inputLayout.Controls.Add(_scanButton, 4, 0);

        inputLayout.Controls.Add(BuildLabel("FirstPortLabel"), 0, 1);
        inputLayout.Controls.Add(_startPortUpDown, 1, 1);
        inputLayout.Controls.Add(BuildLabel("LastPortLabel"), 2, 1);
        inputLayout.Controls.Add(_endPortUpDown, 3, 1);
        inputLayout.Controls.Add(_scanDefaultPortsCheckBox, 4, 1);

        inputLayout.Controls.Add(BuildLabel("TimeoutLabel"), 0, 2);
        inputLayout.Controls.Add(_timeoutUpDown, 1, 2);
        inputLayout.Controls.Add(_progressBar, 2, 2);
        inputLayout.SetColumnSpan(_progressBar, 3);

        _startIpTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _endIpTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _startPortUpDown.Maximum = 65535;
        _startPortUpDown.Minimum = 1;
        _startPortUpDown.Value = 22;
        _endPortUpDown.Maximum = 65535;
        _endPortUpDown.Minimum = 1;
        _endPortUpDown.Value = 5900;
        _timeoutUpDown.Maximum = 60;
        _timeoutUpDown.Minimum = 1;
        _timeoutUpDown.Value = 5;
        _progressBar.Dock = DockStyle.Fill;
        _scanButton.AutoSize = true;
        _scanButton.Click += ScanButtonOnClick;
        _scanDefaultPortsCheckBox.AutoSize = true;

        _resultsGrid.AllowUserToAddRows = false;
        _resultsGrid.AllowUserToDeleteRows = false;
        _resultsGrid.AutoGenerateColumns = false;
        _resultsGrid.DataSource = _bindingSource;
        _resultsGrid.Dock = DockStyle.Fill;
        _resultsGrid.MultiSelect = true;
        _resultsGrid.ReadOnly = true;
        _resultsGrid.RowHeadersVisible = false;
        _resultsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        ConfigureGridColumns();

        TableLayoutPanel importLayout = new()
        {
            AutoSize = true,
            ColumnCount = 3,
            Dock = DockStyle.Fill,
            Padding = new Padding(8),
        };
        importLayout.Controls.Add(BuildLabel("ProtocolLabel"), 0, 0);
        importLayout.Controls.Add(_protocolComboBox, 1, 0);
        importLayout.Controls.Add(_importButton, 2, 0);

        _protocolComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _protocolComboBox.DisplayMember = nameof(ProtocolOption.DisplayName);
        _protocolComboBox.ValueMember = nameof(ProtocolOption.ProtocolId);
        _protocolComboBox.DataSource = BuildProtocolOptions();
        _importButton.AutoSize = true;
        _importButton.Click += ImportButtonOnClick;

        rootLayout.Controls.Add(inputLayout, 0, 0);
        rootLayout.Controls.Add(_resultsGrid, 0, 1);
        rootLayout.Controls.Add(importLayout, 0, 2);
        Controls.Add(rootLayout);
    }

    private void ApplyLanguage()
    {
        _scanButton.Text = Resource("_Scan", "Scan");
        _importButton.Text = Resource("_Import", "Import");
        _scanDefaultPortsCheckBox.Text = Resource("DefaultProtocolPortsOnly", "Default protocol ports only");
    }

    private void ConfigureGridColumns()
    {
        _resultsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(PortScanHostResult.HostDisplayName),
            HeaderText = Resource("HostnameIp", "Hostname / IP"),
        });
        _resultsGrid.Columns.Add(CreateBooleanColumn(nameof(PortScanHostResult.SshDisplay), "SSH"));
        _resultsGrid.Columns.Add(CreateBooleanColumn(nameof(PortScanHostResult.TelnetDisplay), "Telnet"));
        _resultsGrid.Columns.Add(CreateBooleanColumn(nameof(PortScanHostResult.HttpDisplay), "HTTP"));
        _resultsGrid.Columns.Add(CreateBooleanColumn(nameof(PortScanHostResult.HttpsDisplay), "HTTPS"));
        _resultsGrid.Columns.Add(CreateBooleanColumn(nameof(PortScanHostResult.RloginDisplay), "Rlogin"));
        _resultsGrid.Columns.Add(CreateBooleanColumn(nameof(PortScanHostResult.RdpDisplay), "RDP"));
        _resultsGrid.Columns.Add(CreateBooleanColumn(nameof(PortScanHostResult.VncDisplay), "VNC"));
        _resultsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(PortScanHostResult.OpenPortsDisplay),
            HeaderText = Resource("OpenPorts", "Open Ports"),
            Width = 160,
        });
        _resultsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(PortScanHostResult.ClosedPortsDisplay),
            HeaderText = Resource("ClosedPorts", "Closed Ports"),
            Width = 160,
        });
    }

    private DataGridViewTextBoxColumn CreateBooleanColumn(string propertyName, string fallback)
    {
        return new DataGridViewTextBoxColumn
        {
            DataPropertyName = propertyName,
            HeaderText = fallback,
            Width = 60,
        };
    }

    private List<ProtocolOption> BuildProtocolOptions()
    {
        return
        [
            new ProtocolOption(PluginProtocolIds.Ssh2, "SSH"),
            new ProtocolOption(PluginProtocolIds.Telnet, "Telnet"),
            new ProtocolOption(PluginProtocolIds.Http, "HTTP"),
            new ProtocolOption(PluginProtocolIds.Https, "HTTPS"),
            new ProtocolOption(PluginProtocolIds.Rlogin, "Rlogin"),
            new ProtocolOption(PluginProtocolIds.Rdp, "RDP"),
            new ProtocolOption(PluginProtocolIds.Vnc, "VNC"),
            new ProtocolOption(PluginProtocolIds.Ard, "ARD"),
        ];
    }

    private Label BuildLabel(string resourceName)
    {
        return new Label
        {
            AutoSize = true,
            Text = resourceName switch
            {
                "FirstIpLabel" => Resource("FirstIp", "First IP"),
                "LastIpLabel" => Resource("LastIp", "Last IP"),
                "FirstPortLabel" => Resource("FirstPort", "First Port"),
                "LastPortLabel" => Resource("LastPort", "Last Port"),
                "TimeoutLabel" => Resource("TimeoutInSeconds", "Timeout (seconds)"),
                "ProtocolLabel" => Resource("ProtocolToImport", "Protocol to import"),
                _ => resourceName,
            },
            TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
        };
    }

    private async void ScanButtonOnClick(object? sender, EventArgs eventArgs)
    {
        if (_scanning)
        {
            _scanCancellation?.Cancel();
            return;
        }

        if (!TryParseAddressRange(out IPAddress? startAddress, out IPAddress? endAddress))
        {
            _pluginContext.Messages.Warning(Resource("CannotStartPortScan", "Cannot start port scan."));
            return;
        }

        try
        {
            _scanning = true;
            _scanCancellation = new CancellationTokenSource();
            _bindingSource.DataSource = new BindingList<PortScanHostResult>();
            _progressBar.Value = 0;
            _scanButton.Text = Resource("_Stop", "Stop").Trim();

            IReadOnlyList<PortScanHostResult> results = await _portScanService.ScanAsync(
                startAddress,
                endAddress,
                (int)_startPortUpDown.Value,
                (int)_endPortUpDown.Value,
                (int)_timeoutUpDown.Value * 1000,
                _scanDefaultPortsCheckBox.Checked,
                host => _pluginContext.Messages.Info($"Scanning {host}", true),
                AddScannedHost,
                _scanCancellation.Token);

            _bindingSource.DataSource = new BindingList<PortScanHostResult>(results.ToList());
            _pluginContext.Messages.Info(Resource("PortScanComplete", "Port scan complete."));
        }
        catch (OperationCanceledException)
        {
            _pluginContext.Messages.Info(Resource("PortScan", "Port Scan") + " cancelled.", true);
        }
        catch (Exception ex)
        {
            _pluginContext.Messages.Exception("Port scan plugin failed.", ex);
        }
        finally
        {
            _scanCancellation?.Dispose();
            _scanCancellation = null;
            _scanning = false;
            _scanButton.Text = Resource("_Scan", "Scan").Trim();
        }
    }

    private void AddScannedHost(PortScanHostResult result, int scannedCount, int totalCount)
    {
        if (_bindingSource.DataSource is not BindingList<PortScanHostResult> items)
        {
            items = [];
            _bindingSource.DataSource = items;
        }

        items.Add(result);
        _progressBar.Maximum = Math.Max(1, totalCount);
        _progressBar.Value = Math.Min(scannedCount, _progressBar.Maximum);
    }

    private void ImportButtonOnClick(object? sender, EventArgs eventArgs)
    {
        ProtocolOption? selectedProtocol = _protocolComboBox.SelectedItem as ProtocolOption;
        if (selectedProtocol == null)
        {
            return;
        }

        List<PortScanHostResult> selectedHosts = _resultsGrid.SelectedRows
            .Cast<DataGridViewRow>()
            .Select(row => row.DataBoundItem)
            .OfType<PortScanHostResult>()
            .ToList();

        if (selectedHosts.Count == 0)
        {
            _pluginContext.Messages.Warning("No scan results are selected for import.", true);
            return;
        }

        List<PluginConnectionRequest> requests = selectedHosts
            .Where(host => SupportsProtocol(host, selectedProtocol.ProtocolId))
            .Select(host => new PluginConnectionRequest
            {
                Hostname = host.HostDisplayName,
                Name = host.HostNameWithoutDomain,
                ProtocolId = selectedProtocol.ProtocolId,
            })
            .ToList();

        if (requests.Count == 0)
        {
            _pluginContext.Messages.Warning("The selected hosts do not expose the chosen protocol.", true);
            return;
        }

        _pluginContext.Connections.ImportConnections(requests);
    }

    private bool SupportsProtocol(PortScanHostResult host, string protocolId)
    {
        return protocolId switch
        {
            PluginProtocolIds.Ard => host.Vnc,
            PluginProtocolIds.Http => host.Http,
            PluginProtocolIds.Https => host.Https,
            PluginProtocolIds.Rdp => host.Rdp,
            PluginProtocolIds.Rlogin => host.Rlogin,
            PluginProtocolIds.Ssh2 => host.Ssh,
            PluginProtocolIds.Telnet => host.Telnet,
            PluginProtocolIds.Vnc => host.Vnc,
            _ => false,
        };
    }

    private bool TryParseAddressRange(out IPAddress? startAddress, out IPAddress? endAddress)
    {
        bool startParsed = IPAddress.TryParse(_startIpTextBox.Text, out startAddress);
        bool endParsed = IPAddress.TryParse(_endIpTextBox.Text, out endAddress);
        return startParsed && endParsed;
    }

    private string Resource(string name, string fallback)
    {
        return _pluginContext.Resources.GetString(name.Trim(), fallback);
    }

    private sealed record ProtocolOption(string ProtocolId, string DisplayName);
}
