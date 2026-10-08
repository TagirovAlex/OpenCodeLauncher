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
        this.components = new System.ComponentModel.Container();
        this.tabControl = new TabControl();
        this.tabPageLaunch = new TabPage();
        this.tabPageDashboard = new TabPage();
        this.tabPageProfiles = new TabPage();
        this.statusStrip = new StatusStrip();
        this.statusLabel = new ToolStripStatusLabel();
        this.trayMenu = new ContextMenuStrip(this.components);
        this.trayOpenToolStripMenuItem = new ToolStripMenuItem();
        this.traySeparator1 = new ToolStripSeparator();
        this.trayStartToolStripMenuItem = new ToolStripMenuItem();
        this.trayStopToolStripMenuItem = new ToolStripMenuItem();
        this.traySeparator2 = new ToolStripSeparator();
        this.trayAutostartToolStripMenuItem = new ToolStripMenuItem();
        this.traySeparator3 = new ToolStripSeparator();
        this.trayExitToolStripMenuItem = new ToolStripMenuItem();
        this.notifyIcon = new NotifyIcon(this.components);
        this.tabControl.SuspendLayout();
        this.statusStrip.SuspendLayout();
        this.trayMenu.SuspendLayout();
        this.SuspendLayout();
        // 
        // tabControl
        // 
        this.tabControl.Controls.Add(this.tabPageLaunch);
        this.tabControl.Controls.Add(this.tabPageDashboard);
        this.tabControl.Controls.Add(this.tabPageProfiles);
        this.tabControl.Dock = DockStyle.Fill;
        this.tabControl.Location = new Point(0, 0);
        this.tabControl.Name = "tabControl";
        this.tabControl.SelectedIndex = 0;
        this.tabControl.Size = new Size(784, 501);
        this.tabControl.TabIndex = 0;
        // 
        // tabPageLaunch
        // 
        this.tabPageLaunch.Location = new Point(4, 24);
        this.tabPageLaunch.Name = "tabPageLaunch";
        this.tabPageLaunch.Padding = new Padding(3);
        this.tabPageLaunch.Size = new Size(776, 473);
        this.tabPageLaunch.TabIndex = 0;
        this.tabPageLaunch.Text = "Запуск";
        this.tabPageLaunch.UseVisualStyleBackColor = true;
        // 
        // tabPageDashboard
        // 
        this.tabPageDashboard.Location = new Point(4, 24);
        this.tabPageDashboard.Name = "tabPageDashboard";
        this.tabPageDashboard.Padding = new Padding(3);
        this.tabPageDashboard.Size = new Size(776, 473);
        this.tabPageDashboard.TabIndex = 2;
        this.tabPageDashboard.Text = "Дашборд";
        this.tabPageDashboard.UseVisualStyleBackColor = true;
        // 
        // tabPageProfiles
        // 
        this.tabPageProfiles.Location = new Point(4, 24);
        this.tabPageProfiles.Name = "tabPageProfiles";
        this.tabPageProfiles.Padding = new Padding(3);
        this.tabPageProfiles.Size = new Size(776, 473);
        this.tabPageProfiles.TabIndex = 3;
        this.tabPageProfiles.Text = "Профили";
        this.tabPageProfiles.UseVisualStyleBackColor = true;
        // 
        // statusStrip
        // 
        this.statusStrip.Items.AddRange(new ToolStripItem[] { this.statusLabel });
        this.statusStrip.Location = new Point(0, 501);
        this.statusStrip.Name = "statusStrip";
        this.statusStrip.Size = new Size(784, 22);
        this.statusStrip.TabIndex = 1;
        this.statusStrip.Text = "statusStrip";
        // 
        // statusLabel
        // 
        this.statusLabel.Name = "statusLabel";
        this.statusLabel.Size = new Size(39, 17);
        this.statusLabel.Text = "Готово";
        // 
        // trayMenu
        // 
        this.trayMenu.Items.AddRange(new ToolStripItem[] {
        this.trayOpenToolStripMenuItem,
        this.traySeparator1,
        this.trayStartToolStripMenuItem,
        this.trayStopToolStripMenuItem,
        this.traySeparator2,
        this.trayAutostartToolStripMenuItem,
        this.traySeparator3,
        this.trayExitToolStripMenuItem});
        this.trayMenu.Name = "trayMenu";
        this.trayMenu.Size = new Size(221, 150);
        // 
        // trayOpenToolStripMenuItem
        // 
        this.trayOpenToolStripMenuItem.Name = "trayOpenToolStripMenuItem";
        this.trayOpenToolStripMenuItem.Size = new Size(220, 22);
        this.trayOpenToolStripMenuItem.Text = "Открыть OpenCodeLauncher";
        this.trayOpenToolStripMenuItem.Click += new EventHandler(this.trayOpen_Click);
        // 
        // traySeparator1
        // 
        this.traySeparator1.Name = "traySeparator1";
        this.traySeparator1.Size = new Size(217, 6);
        // 
        // trayStartToolStripMenuItem
        // 
        this.trayStartToolStripMenuItem.Name = "trayStartToolStripMenuItem";
        this.trayStartToolStripMenuItem.Size = new Size(220, 22);
        this.trayStartToolStripMenuItem.Text = "Запустить сервер";
        this.trayStartToolStripMenuItem.Click += new EventHandler(this.trayStart_Click);
        // 
        // trayStopToolStripMenuItem
        // 
        this.trayStopToolStripMenuItem.Name = "trayStopToolStripMenuItem";
        this.trayStopToolStripMenuItem.Size = new Size(220, 22);
        this.trayStopToolStripMenuItem.Text = "Остановить сервер";
        this.trayStopToolStripMenuItem.Click += new EventHandler(this.trayStop_Click);
        // 
        // traySeparator2
        // 
        this.traySeparator2.Name = "traySeparator2";
        this.traySeparator2.Size = new Size(217, 6);
        // 
        // trayAutostartToolStripMenuItem
        // 
        this.trayAutostartToolStripMenuItem.CheckOnClick = true;
        this.trayAutostartToolStripMenuItem.Name = "trayAutostartToolStripMenuItem";
        this.trayAutostartToolStripMenuItem.Size = new Size(220, 22);
        this.trayAutostartToolStripMenuItem.Text = "Автозапуск с Windows";
        this.trayAutostartToolStripMenuItem.Click += new EventHandler(this.trayAutostart_Click);
        // 
        // traySeparator3
        // 
        this.traySeparator3.Name = "traySeparator3";
        this.traySeparator3.Size = new Size(217, 6);
        // 
        // trayExitToolStripMenuItem
        // 
        this.trayExitToolStripMenuItem.Name = "trayExitToolStripMenuItem";
        this.trayExitToolStripMenuItem.Size = new Size(220, 22);
        this.trayExitToolStripMenuItem.Text = "Выход";
        this.trayExitToolStripMenuItem.Click += new EventHandler(this.trayExit_Click);
        // 
        // notifyIcon
        // 
        this.notifyIcon.ContextMenuStrip = this.trayMenu;
        this.notifyIcon.Icon = SystemIcons.Application;
        this.notifyIcon.Text = "OpenCodeLauncher";
        this.notifyIcon.Visible = true;
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(784, 523);
        this.Controls.Add(this.tabControl);
        this.Controls.Add(this.statusStrip);
        this.Name = "MainForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "OpenCodeLauncher";
        this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
        this.tabControl.ResumeLayout(false);
        this.statusStrip.ResumeLayout(false);
        this.statusStrip.PerformLayout();
        this.trayMenu.ResumeLayout(false);
        this.trayMenu.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}