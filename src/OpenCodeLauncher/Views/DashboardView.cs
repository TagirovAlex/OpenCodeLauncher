using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Presenters;

namespace OpenCodeLauncher.Views;

/// <summary>Вкладка «Дашборд»: статус сервера, сессии и статистика opencode.</summary>
public partial class DashboardView : UserControl, IDashboardView
{
    private readonly DashboardPresenter? _presenter;

    /// <summary>Конструктор для дизайнера Visual Studio.</summary>
    public DashboardView()
    {
        InitializeComponent();
    }

    /// <summary>Основной конструктор: подключает презентер и события.</summary>
    public DashboardView(DashboardPresenter presenter) : this()
    {
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        _presenter.AttachView(this);

        refreshButton.Click += (_, _) => _presenter.OnRefreshClicked();
        autoRefreshCheckBox.CheckedChanged += (_, _) => _presenter.OnAutoRefreshChanged();
    }

    /// <inheritdoc/>
    public string? ProjectPath => _presenter?.ProjectPath;

    /// <inheritdoc/>
    public bool AutoRefresh => autoRefreshCheckBox.Checked;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Dock = DockStyle.Fill;
        _presenter?.OnLoaded();
    }

    /// <inheritdoc/>
    public void SetServerStatus(string text)
    {
        RunOnUi(() => statusLabel.Text = "Статус: " + text);
    }

    /// <inheritdoc/>
    public void SetServerUrl(string url)
    {
        RunOnUi(() => urlLabel.Text = string.IsNullOrEmpty(url) ? "URL: —" : "URL: " + url);
    }

    /// <inheritdoc/>
    public void ShowSessions(IReadOnlyList<OpenCodeSession> sessions)
    {
        RunOnUi(() =>
        {
            sessionsGrid.Rows.Clear();
            foreach (var session in sessions)
            {
                sessionsGrid.Rows.Add(session.Id, session.Title, session.CreatedAt?.ToString("g") ?? string.Empty);
            }
        });
    }

    /// <inheritdoc/>
    public void ShowStats(UsageStats? stats)
    {
        RunOnUi(() =>
        {
            if (stats is null)
            {
                statsTokensLabel.Text = "Токенов в/из: —";
                statsCostLabel.Text = "Стоимость: —";
                return;
            }

            statsTokensLabel.Text = $"Токенов в/из: {stats.TokensIn:N0} / {stats.TokensOut:N0}";
            statsCostLabel.Text = $"Стоимость: {stats.Cost:C}";
        });
    }

    /// <inheritdoc/>
    public void SetStatusText(string text)
    {
        RunOnUi(() => statusLineLabel.Text = text);
    }

    /// <inheritdoc/>
    public void ShowError(string message)
    {
        RunOnUi(() => statusLineLabel.Text = "Ошибка: " + message);
    }

    /// <summary>
    /// Выполняет действие в UI-потоке. Если вызов пришёл из фонового потока —
    /// маршалит через <see cref="Control.BeginInvoke"/>.
    /// </summary>
    private void RunOnUi(Action action)
    {
        if (IsHandleCreated && InvokeRequired)
        {
            BeginInvoke(action);
        }
        else
        {
            action();
        }
    }
}