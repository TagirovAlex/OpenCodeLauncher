using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Presenters;

namespace OpenCodeLauncher.Views;

/// <summary>Вкладка «Запуск»: выбор проекта, сетевого интерфейса и порта, управление сервером.</summary>
/// <remarks>
/// Вью «глупое»: вся бизнес-логика — в <see cref="LaunchPresenter"/>.
/// Вызовы из презентера (в том числе из фоновых потоков) маршалятся в UI-поток через
/// <c>InvokeRequired</c>/<c>BeginInvoke</c>.
/// </remarks>
public partial class LaunchView : UserControl, ILaunchView
{
    /// <summary>Адреса интерфейсов в порядке пунктов <c>comboBoxInterface</c>.</summary>
    private readonly List<string> _addresses = new();

    private LaunchPresenter? _presenter;

    /// <summary>Конструктор для дизайнера Visual Studio.</summary>
    public LaunchView()
    {
        InitializeComponent();
    }

    /// <summary>Конструктор с презентером (используется кодом приложения).</summary>
    public LaunchView(LaunchPresenter presenter) : this()
    {
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        _presenter.Attach(this);
    }

    /// <inheritdoc/>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        _presenter?.OnLoaded();
    }

    /// <inheritdoc/>
    public string? ProjectPath
    {
        get
        {
            var path = projectPathTextBox.Text.Trim();
            return string.IsNullOrEmpty(path) ? null : path;
        }
    }

    /// <inheritdoc/>
    public string? SelectedHostname
    {
        get
        {
            var index = comboBoxInterface.SelectedIndex;
            return index >= 0 && index < _addresses.Count ? _addresses[index] : null;
        }
    }

    /// <inheritdoc/>
    public int Port
    {
        get
        {
            return (int)portNumericUpDown.Value;
        }
    }

    /// <inheritdoc/>
    public string? SelectedFavorite
    {
        get
        {
            var index = comboBoxFavorites.SelectedIndex;
            if (index < 0)
            {
                return null;
            }

            var path = comboBoxFavorites.Items[index]?.ToString() ?? string.Empty;
            return string.IsNullOrEmpty(path) ? null : path;
        }
    }

    /// <inheritdoc/>
    public string? ServerUsername
    {
        get
        {
            var value = serverUsernameTextBox.Text.Trim();
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }

    /// <inheritdoc/>
    public string? ServerPassword
    {
        get
        {
            var value = serverPasswordTextBox.Text;
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }

    /// <inheritdoc/>
    public void SetServerCredentials(string username, string password)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SetServerCredentials(username, password));
            return;
        }

        serverUsernameTextBox.Text = username;
        serverPasswordTextBox.Text = password;
    }

    /// <inheritdoc/>
    public void SetProjectPath(string projectPath)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SetProjectPath(projectPath));
            return;
        }

        projectPathTextBox.Text = projectPath;
    }

    /// <inheritdoc/>
    public void SetHostname(string hostname)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SetHostname(hostname));
            return;
        }

        if (string.IsNullOrEmpty(hostname))
        {
            return;
        }

        var index = _addresses.IndexOf(hostname);
        if (index >= 0)
        {
            comboBoxInterface.SelectedIndex = index;
            return;
        }

        _addresses.Add(hostname);
        comboBoxInterface.Items.Add(hostname);
        comboBoxInterface.SelectedIndex = _addresses.Count - 1;
    }

    /// <inheritdoc/>
    public void SetPort(int port)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SetPort(port));
            return;
        }

        portNumericUpDown.Value = port < 1 ? 1 : port > 65535 ? 65535 : port;
    }

    /// <inheritdoc/>
    public void SetStatusText(string text)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SetStatusText(text));
            return;
        }

        statusLabel.Text = text;
    }

    /// <inheritdoc/>
    public void AppendLog(string stream, string line)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => AppendLog(stream, line));
            return;
        }

        logTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] [{stream}] {line}{Environment.NewLine}");
    }

    /// <inheritdoc/>
    public void SetStartEnabled(bool enabled)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SetStartEnabled(enabled));
            return;
        }

        buttonStart.Enabled = enabled;
    }

    /// <inheritdoc/>
    public void SetStopEnabled(bool enabled)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SetStopEnabled(enabled));
            return;
        }

        buttonStop.Enabled = enabled;
    }

    /// <inheritdoc/>
    public void PopulateInterfaces(IReadOnlyList<NetworkInterfaceInfo> interfaces, string defaultHostname)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => PopulateInterfaces(interfaces, defaultHostname));
            return;
        }

        comboBoxInterface.Items.Clear();
        _addresses.Clear();

        foreach (var info in interfaces)
        {
            _addresses.Add(info.IpAddress);
            comboBoxInterface.Items.Add(FormatInterfaceText(info));
        }

        var defaultIndex = _addresses.IndexOf(defaultHostname);
        comboBoxInterface.SelectedIndex = defaultIndex >= 0 ? defaultIndex : (_addresses.Count > 0 ? 0 : -1);
    }

    /// <inheritdoc/>
    public void PopulateFavorites(IReadOnlyList<string> favorites)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => PopulateFavorites(favorites));
            return;
        }

        comboBoxFavorites.Items.Clear();

        var count = 0;
        foreach (var path in favorites)
        {
            if (!string.IsNullOrEmpty(path))
            {
                comboBoxFavorites.Items.Add(path);
                count++;
            }
        }

        comboBoxFavorites.SelectedIndex = count > 0 ? 0 : -1;
    }

    /// <inheritdoc/>
    public void ShowError(string message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => ShowError(message));
            return;
        }

        MessageBox.Show(message, "OpenCodeLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    // --- Обработчики контролов ---

    private void buttonBrowse_Click(object sender, EventArgs e)
    {
        folderBrowserDialog.SelectedPath = projectPathTextBox.Text;
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            projectPathTextBox.Text = folderBrowserDialog.SelectedPath;
        }
    }

    private void buttonStart_Click(object sender, EventArgs e) => _presenter?.OnStartClicked();

    private void buttonStop_Click(object sender, EventArgs e) => _presenter?.OnStopClicked();

    private void buttonOpen_Click(object sender, EventArgs e) => _presenter?.OnOpenInBrowserClicked();

    private void buttonAddFavorite_Click(object sender, EventArgs e) => _presenter?.OnAddFavoriteClicked();

    private void buttonRemoveFavorite_Click(object sender, EventArgs e) => _presenter?.OnRemoveFavoriteClicked();

    private void comboBoxFavorites_SelectedIndexChanged(object sender, EventArgs e)
    {
        var index = comboBoxFavorites.SelectedIndex;
        if (index < 0)
        {
            return;
        }

        projectPathTextBox.Text = comboBoxFavorites.Items[index]?.ToString() ?? string.Empty;
    }

    /// <summary>Форматирует текст пункта списка интерфейсов: "IP  (имя[ — Tailscale])".</summary>
    private static string FormatInterfaceText(NetworkInterfaceInfo info)
    {
        var suffix = info.IsTailscale ? " — Tailscale" : string.Empty;
        return $"{info.IpAddress}  ({info.InterfaceName}{suffix})";
    }
}