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
        labelProject = new Label();
        projectPathTextBox = new TextBox();
        buttonBrowse = new Button();
        labelFavorites = new Label();
        comboBoxFavorites = new ComboBox();
        buttonAddFavorite = new Button();
        buttonRemoveFavorite = new Button();
        labelInterface = new Label();
        comboBoxInterface = new ComboBox();
        labelPort = new Label();
        portNumericUpDown = new NumericUpDown();
        labelServerUsername = new Label();
        serverUsernameTextBox = new TextBox();
        labelServerPassword = new Label();
        serverPasswordTextBox = new TextBox();
        buttonStart = new Button();
        buttonStop = new Button();
        buttonOpen = new Button();
        labelStatusCaption = new Label();
        statusLabel = new Label();
        logTextBox = new TextBox();
        folderBrowserDialog = new FolderBrowserDialog();
        ((System.ComponentModel.ISupportInitialize)portNumericUpDown).BeginInit();
        SuspendLayout();
        // 
        // labelProject
        // 
        labelProject.AutoSize = true;
        labelProject.Location = new Point(12, 15);
        labelProject.Name = "labelProject";
        labelProject.Size = new Size(47, 15);
        labelProject.TabIndex = 0;
        labelProject.Text = "Проект";
        // 
        // projectPathTextBox
        // 
        projectPathTextBox.Location = new Point(60, 12);
        projectPathTextBox.Name = "projectPathTextBox";
        projectPathTextBox.Size = new Size(500, 23);
        projectPathTextBox.TabIndex = 1;
        // 
        // buttonBrowse
        // 
        buttonBrowse.Location = new Point(565, 11);
        buttonBrowse.Name = "buttonBrowse";
        buttonBrowse.Size = new Size(75, 23);
        buttonBrowse.TabIndex = 2;
        buttonBrowse.Text = "Обзор…";
        buttonBrowse.Click += buttonBrowse_Click;
        // 
        // labelFavorites
        // 
        labelFavorites.AutoSize = true;
        labelFavorites.Location = new Point(12, 45);
        labelFavorites.Name = "labelFavorites";
        labelFavorites.Size = new Size(68, 15);
        labelFavorites.TabIndex = 3;
        labelFavorites.Text = "Избранное";
        // 
        // comboBoxFavorites
        // 
        comboBoxFavorites.Location = new Point(90, 42);
        comboBoxFavorites.Name = "comboBoxFavorites";
        comboBoxFavorites.Size = new Size(380, 23);
        comboBoxFavorites.TabIndex = 4;
        comboBoxFavorites.SelectedIndexChanged += comboBoxFavorites_SelectedIndexChanged;
        // 
        // buttonAddFavorite
        // 
        buttonAddFavorite.Location = new Point(475, 41);
        buttonAddFavorite.Name = "buttonAddFavorite";
        buttonAddFavorite.Size = new Size(100, 23);
        buttonAddFavorite.TabIndex = 5;
        buttonAddFavorite.Text = "В избранное";
        buttonAddFavorite.Click += buttonAddFavorite_Click;
        // 
        // buttonRemoveFavorite
        // 
        buttonRemoveFavorite.Location = new Point(580, 41);
        buttonRemoveFavorite.Name = "buttonRemoveFavorite";
        buttonRemoveFavorite.Size = new Size(120, 23);
        buttonRemoveFavorite.TabIndex = 6;
        buttonRemoveFavorite.Text = "Убрать из избранного";
        buttonRemoveFavorite.Click += buttonRemoveFavorite_Click;
        // 
        // labelInterface
        // 
        labelInterface.AutoSize = true;
        labelInterface.Location = new Point(12, 75);
        labelInterface.Name = "labelInterface";
        labelInterface.Size = new Size(115, 15);
        labelInterface.TabIndex = 7;
        labelInterface.Text = "Сетевой интерфейс";
        // 
        // comboBoxInterface
        // 
        comboBoxInterface.Location = new Point(135, 72);
        comboBoxInterface.Name = "comboBoxInterface";
        comboBoxInterface.Size = new Size(250, 23);
        comboBoxInterface.TabIndex = 8;
        // 
        // labelPort
        // 
        labelPort.AutoSize = true;
        labelPort.Location = new Point(400, 75);
        labelPort.Name = "labelPort";
        labelPort.Size = new Size(35, 15);
        labelPort.TabIndex = 9;
        labelPort.Text = "Порт";
        // 
        // portNumericUpDown
        // 
        portNumericUpDown.Location = new Point(440, 72);
        portNumericUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        portNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        portNumericUpDown.Name = "portNumericUpDown";
        portNumericUpDown.Size = new Size(70, 23);
        portNumericUpDown.TabIndex = 10;
        portNumericUpDown.Value = new decimal(new int[] { 4000, 0, 0, 0 });
        // 
        // labelServerUsername
        // 
        labelServerUsername.AutoSize = true;
        labelServerUsername.Location = new Point(12, 100);
        labelServerUsername.Name = "labelServerUsername";
        labelServerUsername.Size = new Size(44, 15);
        labelServerUsername.TabIndex = 11;
        labelServerUsername.Text = "Логин:";
        // 
        // serverUsernameTextBox
        // 
        serverUsernameTextBox.Location = new Point(58, 97);
        serverUsernameTextBox.Name = "serverUsernameTextBox";
        serverUsernameTextBox.Size = new Size(140, 23);
        serverUsernameTextBox.TabIndex = 12;
        // 
        // labelServerPassword
        // 
        labelServerPassword.AutoSize = true;
        labelServerPassword.Location = new Point(204, 100);
        labelServerPassword.Name = "labelServerPassword";
        labelServerPassword.Size = new Size(52, 15);
        labelServerPassword.TabIndex = 13;
        labelServerPassword.Text = "Пароль:";
        // 
        // serverPasswordTextBox
        // 
        serverPasswordTextBox.Location = new Point(261, 97);
        serverPasswordTextBox.Name = "serverPasswordTextBox";
        serverPasswordTextBox.Size = new Size(140, 23);
        serverPasswordTextBox.TabIndex = 14;
        serverPasswordTextBox.UseSystemPasswordChar = true;
        // 
        // buttonStart
        // 
        buttonStart.Location = new Point(12, 128);
        buttonStart.Name = "buttonStart";
        buttonStart.Size = new Size(130, 25);
        buttonStart.TabIndex = 11;
        buttonStart.Text = "Запустить сервер";
        buttonStart.Click += buttonStart_Click;
        // 
        // buttonStop
        // 
        buttonStop.Enabled = false;
        buttonStop.Location = new Point(147, 128);
        buttonStop.Name = "buttonStop";
        buttonStop.Size = new Size(100, 25);
        buttonStop.TabIndex = 12;
        buttonStop.Text = "Остановить";
        buttonStop.Click += buttonStop_Click;
        // 
        // buttonOpen
        // 
        buttonOpen.Location = new Point(252, 128);
        buttonOpen.Name = "buttonOpen";
        buttonOpen.Size = new Size(150, 25);
        buttonOpen.TabIndex = 13;
        buttonOpen.Text = "Открыть в браузере";
        buttonOpen.Click += buttonOpen_Click;
        // 
        // labelStatusCaption
        // 
        labelStatusCaption.AutoSize = true;
        labelStatusCaption.Location = new Point(12, 160);
        labelStatusCaption.Name = "labelStatusCaption";
        labelStatusCaption.Size = new Size(46, 15);
        labelStatusCaption.TabIndex = 14;
        labelStatusCaption.Text = "Статус:";
        // 
        // statusLabel
        // 
        statusLabel.AutoSize = true;
        statusLabel.Location = new Point(65, 160);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(73, 15);
        statusLabel.TabIndex = 15;
        statusLabel.Text = "Остановлен";
        // 
        // logTextBox
        // 
        logTextBox.Location = new Point(12, 185);
        logTextBox.Multiline = true;
        logTextBox.Name = "logTextBox";
        logTextBox.ReadOnly = true;
        logTextBox.Size = new Size(752, 272);
        logTextBox.TabIndex = 16;
        // 
        // LaunchView
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(labelProject);
        Controls.Add(projectPathTextBox);
        Controls.Add(buttonBrowse);
        Controls.Add(labelFavorites);
        Controls.Add(comboBoxFavorites);
        Controls.Add(buttonAddFavorite);
        Controls.Add(buttonRemoveFavorite);
        Controls.Add(labelInterface);
        Controls.Add(comboBoxInterface);
        Controls.Add(labelPort);
        Controls.Add(portNumericUpDown);
        Controls.Add(labelServerUsername);
        Controls.Add(serverUsernameTextBox);
        Controls.Add(labelServerPassword);
        Controls.Add(serverPasswordTextBox);
        Controls.Add(buttonStart);
        Controls.Add(buttonStop);
        Controls.Add(buttonOpen);
        Controls.Add(labelStatusCaption);
        Controls.Add(statusLabel);
        Controls.Add(logTextBox);
        Name = "LaunchView";
        Size = new Size(1508, 804);
        ((System.ComponentModel.ISupportInitialize)portNumericUpDown).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}