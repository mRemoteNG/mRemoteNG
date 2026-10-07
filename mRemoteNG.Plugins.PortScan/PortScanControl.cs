using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins.PortScan;

internal sealed class PortScanControl : UserControl
{
    private readonly IPluginContext _pluginContext;
    private readonly BindingSource _bindingSource = new();
    private readonly ComboBox _protocolComboBox = new();
    private readonly CheckBox _useIpAddressCheckBox = new();
    private readonly CheckBox _importAllOpenPortsCheckBox = new();
    private readonly DataGridView _resultsGrid = new();
    private readonly Button _importButton = new();
    private readonly TextBox _addressTextBox = new();
    private readonly RadioButton _commonPortsRadio = new();
    private readonly RadioButton _allPortsRadio = new();
    private readonly RadioButton _customPortsRadio = new();
    private readonly TextBox _customPortsTextBox = new();
    private readonly ToolTip _toolTip = new();
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
            ColumnCount = 4,
            Dock = DockStyle.Fill,
            Padding = new Padding(8),
        };

        inputLayout.Controls.Add(BuildLabel("AddressLabel"), 0, 0);
        inputLayout.Controls.Add(_addressTextBox, 1, 0);
        inputLayout.SetColumnSpan(_addressTextBox, 2);
        inputLayout.Controls.Add(_scanButton, 3, 0);

        inputLayout.Controls.Add(BuildLabel("PortsLabel"), 0, 1);
        FlowLayoutPanel portModePanel = new()
        {
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = new Padding(0),
            WrapContents = false,
        };
        portModePanel.Controls.Add(_commonPortsRadio);
        portModePanel.Controls.Add(_allPortsRadio);
        portModePanel.Controls.Add(_customPortsRadio);
        inputLayout.Controls.Add(portModePanel, 1, 1);
        inputLayout.SetColumnSpan(portModePanel, 3);

        inputLayout.Controls.Add(_customPortsTextBox, 1, 2);
        inputLayout.SetColumnSpan(_customPortsTextBox, 3);

        inputLayout.Controls.Add(BuildLabel("TimeoutLabel"), 0, 3);
        inputLayout.Controls.Add(_timeoutUpDown, 1, 3);
        inputLayout.Controls.Add(_progressBar, 2, 3);
        inputLayout.SetColumnSpan(_progressBar, 2);

        _addressTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _customPortsTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _commonPortsRadio.AutoSize = true;
        _commonPortsRadio.Checked = true;
        _allPortsRadio.AutoSize = true;
        _customPortsRadio.AutoSize = true;
        _commonPortsRadio.CheckedChanged += PortModeOnCheckedChanged;
        _allPortsRadio.CheckedChanged += PortModeOnCheckedChanged;
        _customPortsRadio.CheckedChanged += PortModeOnCheckedChanged;
        _timeoutUpDown.Maximum = 60;
        _timeoutUpDown.Minimum = 1;
        _timeoutUpDown.Value = 5;
        _progressBar.Dock = DockStyle.Fill;
        _scanButton.AutoSize = true;
        _scanButton.Click += ScanButtonOnClick;

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
            ColumnCount = 5,
            Dock = DockStyle.Fill,
            Padding = new Padding(8),
        };
        importLayout.Controls.Add(BuildLabel("ProtocolLabel"), 0, 0);
        importLayout.Controls.Add(_protocolComboBox, 1, 0);
        importLayout.Controls.Add(_useIpAddressCheckBox, 2, 0);
        importLayout.Controls.Add(_importAllOpenPortsCheckBox, 3, 0);
        importLayout.Controls.Add(_importButton, 4, 0);

        _protocolComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _protocolComboBox.DisplayMember = nameof(ProtocolOption.DisplayName);
        _protocolComboBox.ValueMember = nameof(ProtocolOption.ProtocolId);
        _protocolComboBox.DataSource = BuildProtocolOptions();
        _importButton.AutoSize = true;
        _importButton.Click += ImportButtonOnClick;
        _useIpAddressCheckBox.AutoSize = true;
        _importAllOpenPortsCheckBox.AutoSize = true;

        rootLayout.Controls.Add(inputLayout, 0, 0);
        rootLayout.Controls.Add(_resultsGrid, 0, 1);
        rootLayout.Controls.Add(importLayout, 0, 2);
        Controls.Add(rootLayout);
    }

    private void ApplyLanguage()
    {
        _scanButton.Text = Resource("_Scan", "Scan");
        _importButton.Text = Resource("_Import", "Import");
        _useIpAddressCheckBox.Text = Resource("UseIpAddressForImport", "Use IP address for imported connections");
        _importAllOpenPortsCheckBox.Text = Resource("ImportAllOpenPorts", "Import all open ports");
        _commonPortsRadio.Text = Resource("PortScanCommonPorts", "Common ports");
        _allPortsRadio.Text = Resource("PortScanAllPorts", "All ports");
        _customPortsRadio.Text = Resource("PortScanCustomPorts", "Custom");
        _addressTextBox.PlaceholderText = Resource("PortScanAddressRangePlaceholder", "192.168.1.0/24");
        _customPortsTextBox.PlaceholderText = Resource("PortScanCustomPortsPlaceholder", "22, 80, 443, 3389, 8000-8100");
        _toolTip.SetToolTip(_addressTextBox, Resource("PortScanAddressRangeHint", IpRangeParser.SyntaxHint));
        _toolTip.SetToolTip(_commonPortsRadio, string.Join(", ", PortScanService.CommonPorts));
        _toolTip.SetToolTip(_customPortsRadio, Resource("PortScanCustomPortsHint", PortListParser.SyntaxHint));
        _toolTip.SetToolTip(_customPortsTextBox, Resource("PortScanCustomPortsHint", PortListParser.SyntaxHint));
        UpdatePortModeControls();
    }

    private void ConfigureGridColumns()
    {
        _resultsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(PortScanHostResult.HostNameDisplay),
            HeaderText = Resource("Hostname", "Hostname"),
        });
        _resultsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(PortScanHostResult.HostIp),
            HeaderText = Resource("IpAddress", "IP Address"),
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
                "AddressLabel" => Resource("PortScanAddressRange", "Scan target"),
                "PortsLabel" => Resource("Ports", "Ports"),
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

        if (!IpRangeParser.TryParse(_addressTextBox.Text, out IPAddress? startAddress, out IPAddress? endAddress,
                                    out string addressError))
        {
            _pluginContext.Messages.Warning(addressError);
            return;
        }

        if (!TryGetSelectedPorts(out List<int> ports, out string portError))
        {
            _pluginContext.Messages.Warning(portError);
            return;
        }

        if (startAddress is null || endAddress is null)
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
                ports,
                (int)_timeoutUpDown.Value * 1000,
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
        catch (ArgumentException ex)
        {
            // Range too large or mixed address families: report the reason instead of logging an exception.
            _pluginContext.Messages.Warning(ex.Message);
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

        List<PluginConnectionRequest> requests = PortScanConnectionRequestBuilder.Build(
            selectedHosts,
            selectedProtocol.ProtocolId,
            _useIpAddressCheckBox.Checked,
            _importAllOpenPortsCheckBox.Checked);

        if (requests.Count == 0)
        {
            _pluginContext.Messages.Warning("The selected hosts do not expose the chosen protocol.", true);
            return;
        }

        _pluginContext.Connections.ImportConnections(requests);
    }

    private void PortModeOnCheckedChanged(object? sender, EventArgs eventArgs)
    {
        UpdatePortModeControls();
    }

    /// <summary>
    /// The custom port list is only editable while the "Custom" option is selected, so the three
    /// port options can never be left in a half-configured state.
    /// </summary>
    private void UpdatePortModeControls()
    {
        _customPortsTextBox.Enabled = _customPortsRadio.Checked;
    }

    /// <summary>
    /// Resolves the ports to probe from the selected port option. Returns false, with a
    /// user-readable reason, when the custom list is empty or malformed.
    /// </summary>
    private bool TryGetSelectedPorts(out List<int> ports, out string error)
    {
        error = string.Empty;

        if (_allPortsRadio.Checked)
        {
            ports = PortListParser.AllPorts();
            return true;
        }

        if (!_customPortsRadio.Checked)
        {
            ports = [.. PortScanService.CommonPorts];
            return true;
        }

        return PortListParser.TryParse(_customPortsTextBox.Text, out ports, out error);
    }

    private string Resource(string name, string fallback)
    {
        return _pluginContext.Resources.GetString(name.Trim(), fallback);
    }

    private sealed record ProtocolOption(string ProtocolId, string DisplayName);
}
