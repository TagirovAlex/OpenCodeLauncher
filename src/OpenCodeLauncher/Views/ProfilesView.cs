using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Core.Settings;
using OpenCodeLauncher.Presenters;

namespace OpenCodeLauncher.Views;

/// <summary>Вкладка «Профили»: управление сохранёнными профилями запуска.</summary>
/// <remarks>
/// Вью «глупое»: вся бизнес-логика — в <see cref="ProfilesPresenter"/>.
/// Вызовы из презентера (в том числе из фоновых потоков) маршалятся в UI-поток через
/// <c>InvokeRequired</c>/<c>BeginInvoke</c>.
/// </remarks>
public partial class ProfilesView : UserControl, IProfilesView
{
    /// <summary>Адреса интерфейсов в порядке пунктов <c>hostComboBox</c>.</summary>
    private readonly List<string> _addresses = new();

    private ProfilesPresenter? _presenter;

    /// <summary>Конструктор для дизайнера Visual Studio.</summary>
    public ProfilesView()
    {
        InitializeComponent();
    }

    /// <summary>Конструктор с презентером (используется кодом приложения).</summary>
    public ProfilesView(ProfilesPresenter presenter) : this()
    {
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        _presenter.Attach(this);
    }

    /// <inheritdoc/>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Dock = DockStyle.Fill;
        _presenter?.OnLoaded();
    }

    /// <inheritdoc/>
    public string? ProfileName
    {
        get
        {
            var name = nameTextBox.Text.Trim();
            return string.IsNullOrEmpty(name) ? null : name;
        }
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
            var index = hostComboBox.SelectedIndex;
            return index >= 0 && index < _addresses.Count ? _addresses[index] : null;
        }
    }

    /// <inheritdoc/>
    public int Port => (int)portNumericUpDown.Value;

    /// <inheritdoc/>
    public string? Model
    {
        get
        {
            var text = modelTextBox.Text.Trim();
            return string.IsNullOrEmpty(text) ? null : text;
        }
    }

    /// <inheritdoc/>
    public bool ContinueLastSession => continueSessionCheckBox.Checked;

    /// <inheritdoc/>
    public string? SessionId
    {
        get
        {
            var text = sessionIdTextBox.Text.Trim();
            return string.IsNullOrEmpty(text) ? null : text;
        }
    }

    /// <inheritdoc/>
    public string? ExtraArgs
    {
        get
        {
            var text = extraArgsTextBox.Text.Trim();
            return string.IsNullOrEmpty(text) ? null : text;
        }
    }

    /// <inheritdoc/>
    public string? ServerUsername
    {
        get
        {
            var text = serverUsernameTextBox.Text.Trim();
            return string.IsNullOrEmpty(text) ? null : text;
        }
    }

    /// <inheritdoc/>
    public string? ServerPassword
    {
        get
        {
            var text = serverPasswordTextBox.Text;
            return string.IsNullOrEmpty(text) ? null : text;
        }
    }

    /// <inheritdoc/>
    public string? SelectedProfileName
    {
        get
        {
            if (profilesListView.SelectedItems.Count == 0)
            {
                return null;
            }

            return profilesListView.SelectedItems[0].Text;
        }
    }

    /// <inheritdoc/>
    public void PopulateProfiles(IReadOnlyList<LaunchProfile> profiles)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => PopulateProfiles(profiles));
            return;
        }

        profilesListView.BeginUpdate();
        try
        {
            profilesListView.Items.Clear();
            foreach (var profile in profiles)
            {
                var item = new ListViewItem(profile.Name);
                item.SubItems.Add(profile.ProjectPath);
                item.SubItems.Add(profile.Hostname);
                item.SubItems.Add(profile.Port.ToString());
                profilesListView.Items.Add(item);
            }
        }
        finally
        {
            profilesListView.EndUpdate();
        }
    }

    /// <inheritdoc/>
    public void PopulateInterfaces(IReadOnlyList<NetworkInterfaceInfo> interfaces, string defaultHostname)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => PopulateInterfaces(interfaces, defaultHostname));
            return;
        }

        hostComboBox.Items.Clear();
        _addresses.Clear();

        foreach (var info in interfaces)
        {
            _addresses.Add(info.IpAddress);
            hostComboBox.Items.Add(FormatInterfaceText(info));
        }

        var defaultIndex = _addresses.IndexOf(defaultHostname);
        hostComboBox.SelectedIndex = defaultIndex >= 0 ? defaultIndex : (_addresses.Count > 0 ? 0 : -1);
    }

    /// <inheritdoc/>
    public void SetFields(LaunchProfile profile)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SetFields(profile));
            return;
        }

        nameTextBox.Text = profile.Name;
        projectPathTextBox.Text = profile.ProjectPath;
        SetHostname(profile.Hostname);
        portNumericUpDown.Value = ClampPort(profile.Port);
        modelTextBox.Text = profile.Model ?? string.Empty;
        continueSessionCheckBox.Checked = profile.ContinueLastSession;
        sessionIdTextBox.Text = profile.SessionId ?? string.Empty;
        extraArgsTextBox.Text = profile.ExtraArgs ?? string.Empty;
        serverUsernameTextBox.Text = profile.ServerUsername ?? string.Empty;
        serverPasswordTextBox.Text = profile.ServerPassword ?? string.Empty;
    }

    /// <inheritdoc/>
    public void ClearFields()
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => ClearFields());
            return;
        }

        nameTextBox.Text = string.Empty;
        projectPathTextBox.Text = string.Empty;
        modelTextBox.Text = string.Empty;
        continueSessionCheckBox.Checked = false;
        sessionIdTextBox.Text = string.Empty;
        extraArgsTextBox.Text = string.Empty;
        serverUsernameTextBox.Text = string.Empty;
        serverPasswordTextBox.Text = string.Empty;
        portNumericUpDown.Value = 4000;

        if (_addresses.Count > 0)
        {
            hostComboBox.SelectedIndex = 0;
        }
    }

    /// <inheritdoc/>
    public void SelectProfile(string name)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SelectProfile(name));
            return;
        }

        foreach (ListViewItem item in profilesListView.Items)
        {
            if (string.Equals(item.Text, name, StringComparison.OrdinalIgnoreCase))
            {
                item.Selected = true;
                item.EnsureVisible();
                return;
            }
        }
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
    public void SetActionText(string text)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SetActionText(text));
            return;
        }

        actionStatusLabel.Text = text;
    }

    /// <inheritdoc/>
    public void SetLaunchEnabled(bool enabled)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SetLaunchEnabled(enabled));
            return;
        }

        buttonLaunch.Enabled = enabled;
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
    public void ShowError(string message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => ShowError(message));
            return;
        }

        MessageBox.Show(message, "OpenCodeLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>Выбирает хост в списке; если адреса нет — добавляет его.</summary>
    private void SetHostname(string hostname)
    {
        if (string.IsNullOrEmpty(hostname))
        {
            return;
        }

        var index = _addresses.IndexOf(hostname);
        if (index >= 0)
        {
            hostComboBox.SelectedIndex = index;
            return;
        }

        _addresses.Add(hostname);
        hostComboBox.Items.Add(hostname);
        hostComboBox.SelectedIndex = _addresses.Count - 1;
    }

    /// <summary>Форматирует текст пункта списка интерфейсов: "IP  (имя[ — Tailscale])".</summary>
    private static string FormatInterfaceText(NetworkInterfaceInfo info)
    {
        var suffix = info.IsTailscale ? " — Tailscale" : string.Empty;
        return $"{info.IpAddress}  ({info.InterfaceName}{suffix})";
    }

    /// <summary>Ограничивает порт диапазоном NumericUpDown.</summary>
    private static decimal ClampPort(int port) => port < 1 ? 1 : port > 65535 ? 65535 : port;

    // --- Обработчики контролов ---

    private void buttonBrowse_Click(object sender, EventArgs e)
    {
        folderBrowserDialog.SelectedPath = projectPathTextBox.Text;
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            projectPathTextBox.Text = folderBrowserDialog.SelectedPath;
        }
    }

    private void buttonAdd_Click(object sender, EventArgs e) => _presenter?.OnAddClicked();

    private void buttonSave_Click(object sender, EventArgs e) => _presenter?.OnSaveClicked();

    private void buttonDelete_Click(object sender, EventArgs e) => _presenter?.OnDeleteClicked();

    private void buttonLaunch_Click(object sender, EventArgs e) => _presenter?.OnLaunchClicked();

    private void buttonStop_Click(object sender, EventArgs e) => _presenter?.OnStopClicked();

    private void profilesListView_SelectedIndexChanged(object sender, EventArgs e) => _presenter?.OnProfileSelected();
}