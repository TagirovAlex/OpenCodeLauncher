namespace OpenCodeLauncher;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    private TabControl tabControl;
    private TabPage tabPageLaunch;
    private TabPage tabPageDashboard;
    private TabPage tabPageProfiles;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabel;
    private ContextMenuStrip trayMenu;
    private ToolStripMenuItem trayOpenToolStripMenuItem;
    private ToolStripSeparator traySeparator1;
    private ToolStripMenuItem trayStartToolStripMenuItem;
    private ToolStripMenuItem trayStopToolStripMenuItem;
    private ToolStripSeparator traySeparator2;
    private ToolStripMenuItem trayAutostartToolStripMenuItem;
    private ToolStripSeparator traySeparator3;
    private ToolStripMenuItem trayExitToolStripMenuItem;
    private NotifyIcon notifyIcon;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        tabControl = new TabControl();
        tabPageLaunch = new TabPage();
        tabPageDashboard = new TabPage();
        tabPageProfiles = new TabPage();
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        trayMenu = new ContextMenuStrip(components);
        trayOpenToolStripMenuItem = new ToolStripMenuItem();
        traySeparator1 = new ToolStripSeparator();
        trayStartToolStripMenuItem = new ToolStripMenuItem();
        trayStopToolStripMenuItem = new ToolStripMenuItem();
        traySeparator2 = new ToolStripSeparator();
        trayAutostartToolStripMenuItem = new ToolStripMenuItem();
        traySeparator3 = new ToolStripSeparator();
        trayExitToolStripMenuItem = new ToolStripMenuItem();
        notifyIcon = new NotifyIcon(components);
        tabControl.SuspendLayout();
        statusStrip.SuspendLayout();
        trayMenu.SuspendLayout();
        SuspendLayout();
        // 
        // tabControl
        // 
        tabControl.Controls.Add(tabPageLaunch);
        tabControl.Controls.Add(tabPageDashboard);
        tabControl.Controls.Add(tabPageProfiles);
        tabControl.Dock = DockStyle.Fill;
        tabControl.Location = new Point(0, 0);
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(784, 501);
        tabControl.TabIndex = 0;
        // 
        // tabPageLaunch
        // 
        tabPageLaunch.Location = new Point(4, 24);
        tabPageLaunch.Name = "tabPageLaunch";
        tabPageLaunch.Padding = new Padding(3);
        tabPageLaunch.Size = new Size(776, 473);
        tabPageLaunch.TabIndex = 0;
        tabPageLaunch.Text = "Запуск";
        tabPageLaunch.UseVisualStyleBackColor = true;
        // 
        // tabPageDashboard
        // 
        tabPageDashboard.Location = new Point(4, 24);
        tabPageDashboard.Name = "tabPageDashboard";
        tabPageDashboard.Padding = new Padding(3);
        tabPageDashboard.Size = new Size(776, 473);
        tabPageDashboard.TabIndex = 2;
        tabPageDashboard.Text = "Дашборд";
        tabPageDashboard.UseVisualStyleBackColor = true;
        // 
        // tabPageProfiles
        // 
        tabPageProfiles.Location = new Point(4, 24);
        tabPageProfiles.Name = "tabPageProfiles";
        tabPageProfiles.Padding = new Padding(3);
        tabPageProfiles.Size = new Size(776, 473);
        tabPageProfiles.TabIndex = 3;
        tabPageProfiles.Text = "Профили";
        tabPageProfiles.UseVisualStyleBackColor = true;
        // 
        // statusStrip
        // 
        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
        statusStrip.Location = new Point(0, 501);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(784, 22);
        statusStrip.TabIndex = 1;
        statusStrip.Text = "statusStrip";
        // 
        // statusLabel
        // 
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(45, 17);
        statusLabel.Text = "Готово";
        // 
        // trayMenu
        // 
        trayMenu.Items.AddRange(new ToolStripItem[] { trayOpenToolStripMenuItem, traySeparator1, trayStartToolStripMenuItem, trayStopToolStripMenuItem, traySeparator2, trayAutostartToolStripMenuItem, traySeparator3, trayExitToolStripMenuItem });
        trayMenu.Name = "trayMenu";
        trayMenu.Size = new Size(231, 132);
        // 
        // trayOpenToolStripMenuItem
        // 
        trayOpenToolStripMenuItem.Name = "trayOpenToolStripMenuItem";
        trayOpenToolStripMenuItem.Size = new Size(230, 22);
        trayOpenToolStripMenuItem.Text = "Открыть OpenCodeLauncher";
        trayOpenToolStripMenuItem.Click += trayOpen_Click;
        // 
        // traySeparator1
        // 
        traySeparator1.Name = "traySeparator1";
        traySeparator1.Size = new Size(227, 6);
        // 
        // trayStartToolStripMenuItem
        // 
        trayStartToolStripMenuItem.Name = "trayStartToolStripMenuItem";
        trayStartToolStripMenuItem.Size = new Size(230, 22);
        trayStartToolStripMenuItem.Text = "Запустить сервер";
        trayStartToolStripMenuItem.Click += trayStart_Click;
        // 
        // trayStopToolStripMenuItem
        // 
        trayStopToolStripMenuItem.Name = "trayStopToolStripMenuItem";
        trayStopToolStripMenuItem.Size = new Size(230, 22);
        trayStopToolStripMenuItem.Text = "Остановить сервер";
        trayStopToolStripMenuItem.Click += trayStop_Click;
        // 
        // traySeparator2
        // 
        traySeparator2.Name = "traySeparator2";
        traySeparator2.Size = new Size(227, 6);
        // 
        // trayAutostartToolStripMenuItem
        // 
        trayAutostartToolStripMenuItem.CheckOnClick = true;
        trayAutostartToolStripMenuItem.Name = "trayAutostartToolStripMenuItem";
        trayAutostartToolStripMenuItem.Size = new Size(230, 22);
        trayAutostartToolStripMenuItem.Text = "Автозапуск с Windows";
        trayAutostartToolStripMenuItem.Click += trayAutostart_Click;
        // 
        // traySeparator3
        // 
        traySeparator3.Name = "traySeparator3";
        traySeparator3.Size = new Size(227, 6);
        // 
        // trayExitToolStripMenuItem
        // 
        trayExitToolStripMenuItem.Name = "trayExitToolStripMenuItem";
        trayExitToolStripMenuItem.Size = new Size(230, 22);
        trayExitToolStripMenuItem.Text = "Выход";
        trayExitToolStripMenuItem.Click += trayExit_Click;
        // 
        // notifyIcon
        // 
        notifyIcon.ContextMenuStrip = trayMenu;
        notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
        notifyIcon.Text = "OpenCodeLauncher";
        notifyIcon.Visible = true;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 523);
        Controls.Add(tabControl);
        Controls.Add(statusStrip);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "OpenCodeLauncher";
        FormClosing += MainForm_FormClosing;
        tabControl.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        trayMenu.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}