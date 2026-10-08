namespace OpenCodeLauncher.Views;

partial class LaunchView
{
    private System.ComponentModel.IContainer components = null;

    private Label labelProject;
    private TextBox projectPathTextBox;
    private Button buttonBrowse;
    private Label labelFavorites;
    private ComboBox comboBoxFavorites;
    private Button buttonAddFavorite;
    private Button buttonRemoveFavorite;
    private Label labelInterface;
    private ComboBox comboBoxInterface;
    private Label labelPort;
    private NumericUpDown portNumericUpDown;
    private Label labelServerUsername;
    private TextBox serverUsernameTextBox;
    private Label labelServerPassword;
    private TextBox serverPasswordTextBox;
    private Button buttonStart;
    private Button buttonStop;
    private Button buttonOpen;
    private Label labelStatusCaption;
    private Label statusLabel;
    private TextBox logTextBox;
    private FolderBrowserDialog folderBrowserDialog;

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
        this.labelProject = new Label();
        this.projectPathTextBox = new TextBox();
        this.buttonBrowse = new Button();
        this.labelFavorites = new Label();
        this.comboBoxFavorites = new ComboBox();
        this.buttonAddFavorite = new Button();
        this.buttonRemoveFavorite = new Button();
        this.labelInterface = new Label();
        this.comboBoxInterface = new ComboBox();
        this.labelPort = new Label();
        this.portNumericUpDown = new NumericUpDown();
        this.labelServerUsername = new Label();
        this.serverUsernameTextBox = new TextBox();
        this.labelServerPassword = new Label();
        this.serverPasswordTextBox = new TextBox();
        this.buttonStart = new Button();
        this.buttonStop = new Button();
        this.buttonOpen = new Button();
        this.labelStatusCaption = new Label();
        this.statusLabel = new Label();
        this.logTextBox = new TextBox();
        this.folderBrowserDialog = new FolderBrowserDialog();
        this.SuspendLayout();
        // 
        // labelProject
        // 
        this.labelProject.AutoSize = true;
        this.labelProject.Location = new Point(12, 15);
        this.labelProject.Name = "labelProject";
        this.labelProject.Size = new Size(55, 15);
        this.labelProject.TabIndex = 0;
        this.labelProject.Text = "Проект";
        // 
        // projectPathTextBox
        // 
        this.projectPathTextBox.Location = new Point(60, 12);
        this.projectPathTextBox.Name = "projectPathTextBox";
        this.projectPathTextBox.Size = new Size(500, 21);
        this.projectPathTextBox.TabIndex = 1;
        // 
        // buttonBrowse
        // 
        this.buttonBrowse.Location = new Point(565, 11);
        this.buttonBrowse.Name = "buttonBrowse";
        this.buttonBrowse.Size = new Size(75, 23);
        this.buttonBrowse.TabIndex = 2;
        this.buttonBrowse.Text = "Обзор…";
        this.buttonBrowse.Click += new EventHandler(this.buttonBrowse_Click);
        // 
        // labelFavorites
        // 
        this.labelFavorites.AutoSize = true;
        this.labelFavorites.Location = new Point(12, 45);
        this.labelFavorites.Name = "labelFavorites";
        this.labelFavorites.Size = new Size(70, 15);
        this.labelFavorites.TabIndex = 3;
        this.labelFavorites.Text = "Избранное";
        // 
        // comboBoxFavorites
        // 
        this.comboBoxFavorites.Location = new Point(90, 42);
        this.comboBoxFavorites.Name = "comboBoxFavorites";
        this.comboBoxFavorites.Size = new Size(380, 21);
        this.comboBoxFavorites.TabIndex = 4;
        this.comboBoxFavorites.SelectedIndexChanged += new EventHandler(this.comboBoxFavorites_SelectedIndexChanged);
        // 
        // buttonAddFavorite
        // 
        this.buttonAddFavorite.Location = new Point(475, 41);
        this.buttonAddFavorite.Name = "buttonAddFavorite";
        this.buttonAddFavorite.Size = new Size(100, 23);
        this.buttonAddFavorite.TabIndex = 5;
        this.buttonAddFavorite.Text = "В избранное";
        this.buttonAddFavorite.Click += new EventHandler(this.buttonAddFavorite_Click);
        // 
        // buttonRemoveFavorite
        // 
        this.buttonRemoveFavorite.Location = new Point(580, 41);
        this.buttonRemoveFavorite.Name = "buttonRemoveFavorite";
        this.buttonRemoveFavorite.Size = new Size(120, 23);
        this.buttonRemoveFavorite.TabIndex = 6;
        this.buttonRemoveFavorite.Text = "Убрать из избранного";
        this.buttonRemoveFavorite.Click += new EventHandler(this.buttonRemoveFavorite_Click);
        // 
        // labelInterface
        // 
        this.labelInterface.AutoSize = true;
        this.labelInterface.Location = new Point(12, 75);
        this.labelInterface.Name = "labelInterface";
        this.labelInterface.Size = new Size(115, 15);
        this.labelInterface.TabIndex = 7;
        this.labelInterface.Text = "Сетевой интерфейс";
        // 
        // comboBoxInterface
        // 
        this.comboBoxInterface.Location = new Point(135, 72);
        this.comboBoxInterface.Name = "comboBoxInterface";
        this.comboBoxInterface.Size = new Size(250, 21);
        this.comboBoxInterface.TabIndex = 8;
        // 
        // labelPort
        // 
        this.labelPort.AutoSize = true;
        this.labelPort.Location = new Point(400, 75);
        this.labelPort.Name = "labelPort";
        this.labelPort.Size = new Size(30, 15);
        this.labelPort.TabIndex = 9;
        this.labelPort.Text = "Порт";
        // 
        // portNumericUpDown
        // 
        this.portNumericUpDown.Location = new Point(440, 72);
        this.portNumericUpDown.Maximum = 65535;
        this.portNumericUpDown.Minimum = 1;
        this.portNumericUpDown.Name = "portNumericUpDown";
        this.portNumericUpDown.Size = new Size(70, 21);
        this.portNumericUpDown.TabIndex = 10;
        this.portNumericUpDown.Value = 4000;
        // 
        // labelServerUsername
        // 
        this.labelServerUsername.AutoSize = true;
        this.labelServerUsername.Location = new Point(12, 100);
        this.labelServerUsername.Name = "labelServerUsername";
        this.labelServerUsername.Size = new Size(43, 15);
        this.labelServerUsername.TabIndex = 11;
        this.labelServerUsername.Text = "Логин:";
        // 
        // serverUsernameTextBox
        // 
        this.serverUsernameTextBox.Location = new Point(58, 97);
        this.serverUsernameTextBox.Name = "serverUsernameTextBox";
        this.serverUsernameTextBox.Size = new Size(140, 23);
        this.serverUsernameTextBox.TabIndex = 12;
        // 
        // labelServerPassword
        // 
        this.labelServerPassword.AutoSize = true;
        this.labelServerPassword.Location = new Point(210, 100);
        this.labelServerPassword.Name = "labelServerPassword";
        this.labelServerPassword.Size = new Size(48, 15);
        this.labelServerPassword.TabIndex = 13;
        this.labelServerPassword.Text = "Пароль:";
        // 
        // serverPasswordTextBox
        // 
        this.serverPasswordTextBox.Location = new Point(261, 97);
        this.serverPasswordTextBox.Name = "serverPasswordTextBox";
        this.serverPasswordTextBox.Size = new Size(140, 23);
        this.serverPasswordTextBox.TabIndex = 14;
        this.serverPasswordTextBox.UseSystemPasswordChar = true;
        // 
        // buttonStart
        // 
        this.buttonStart.Location = new Point(12, 128);
        this.buttonStart.Name = "buttonStart";
        this.buttonStart.Size = new Size(130, 25);
        this.buttonStart.TabIndex = 11;
        this.buttonStart.Text = "Запустить сервер";
        this.buttonStart.Click += new EventHandler(this.buttonStart_Click);
        // 
        // buttonStop
        // 
        this.buttonStop.Enabled = false;
        this.buttonStop.Location = new Point(147, 128);
        this.buttonStop.Name = "buttonStop";
        this.buttonStop.Size = new Size(100, 25);
        this.buttonStop.TabIndex = 12;
        this.buttonStop.Text = "Остановить";
        this.buttonStop.Click += new EventHandler(this.buttonStop_Click);
        // 
        // buttonOpen
        // 
        this.buttonOpen.Location = new Point(252, 128);
        this.buttonOpen.Name = "buttonOpen";
        this.buttonOpen.Size = new Size(150, 25);
        this.buttonOpen.TabIndex = 13;
        this.buttonOpen.Text = "Открыть в браузере";
        this.buttonOpen.Click += new EventHandler(this.buttonOpen_Click);
        // 
        // labelStatusCaption
        // 
        this.labelStatusCaption.AutoSize = true;
        this.labelStatusCaption.Location = new Point(12, 160);
        this.labelStatusCaption.Name = "labelStatusCaption";
        this.labelStatusCaption.Size = new Size(45, 15);
        this.labelStatusCaption.TabIndex = 14;
        this.labelStatusCaption.Text = "Статус:";
        // 
        // statusLabel
        // 
        this.statusLabel.AutoSize = true;
        this.statusLabel.Location = new Point(65, 160);
        this.statusLabel.Name = "statusLabel";
        this.statusLabel.Size = new Size(300, 15);
        this.statusLabel.TabIndex = 15;
        this.statusLabel.Text = "Остановлен";
        // 
        // logTextBox
        // 
        this.logTextBox.Location = new Point(12, 185);
        this.logTextBox.Multiline = true;
        this.logTextBox.Name = "logTextBox";
        this.logTextBox.ReadOnly = true;
        this.logTextBox.Size = new Size(752, 272);
        this.logTextBox.TabIndex = 16;
        // 
        // LaunchView
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.Controls.Add(this.labelProject);
        this.Controls.Add(this.projectPathTextBox);
        this.Controls.Add(this.buttonBrowse);
        this.Controls.Add(this.labelFavorites);
        this.Controls.Add(this.comboBoxFavorites);
        this.Controls.Add(this.buttonAddFavorite);
        this.Controls.Add(this.buttonRemoveFavorite);
        this.Controls.Add(this.labelInterface);
        this.Controls.Add(this.comboBoxInterface);
        this.Controls.Add(this.labelPort);
        this.Controls.Add(this.portNumericUpDown);
        this.Controls.Add(this.labelServerUsername);
        this.Controls.Add(this.serverUsernameTextBox);
        this.Controls.Add(this.labelServerPassword);
        this.Controls.Add(this.serverPasswordTextBox);
        this.Controls.Add(this.buttonStart);
        this.Controls.Add(this.buttonStop);
        this.Controls.Add(this.buttonOpen);
        this.Controls.Add(this.labelStatusCaption);
        this.Controls.Add(this.statusLabel);
        this.Controls.Add(this.logTextBox);
        this.Dock = DockStyle.Fill;
        this.Name = "LaunchView";
        this.Size = new Size(776, 473);
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}