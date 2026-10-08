namespace OpenCodeLauncher.Views;

partial class ProfilesView
{
    private System.ComponentModel.IContainer components = null;

    private ListView profilesListView;
    private ColumnHeader columnHeaderName;
    private ColumnHeader columnHeaderProject;
    private ColumnHeader columnHeaderHost;
    private ColumnHeader columnHeaderPort;
    private Label labelName;
    private TextBox nameTextBox;
    private Label labelProject;
    private TextBox projectPathTextBox;
    private Button buttonBrowse;
    private Label labelHost;
    private ComboBox hostComboBox;
    private Label labelPort;
    private NumericUpDown portNumericUpDown;
    private Label labelModel;
    private TextBox modelTextBox;
    private CheckBox continueSessionCheckBox;
    private Label labelSessionId;
    private TextBox sessionIdTextBox;
    private Label labelExtraArgs;
    private TextBox extraArgsTextBox;
    private Label labelServerUsername;
    private TextBox serverUsernameTextBox;
    private Label labelServerPassword;
    private TextBox serverPasswordTextBox;
    private Button buttonAdd;
    private Button buttonSave;
    private Button buttonDelete;
    private Button buttonLaunch;
    private Button buttonStop;
    private Label labelStatusCaption;
    private Label statusLabel;
    private Label labelActionCaption;
    private Label actionStatusLabel;
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
        profilesListView = new ListView();
        columnHeaderName = new ColumnHeader();
        columnHeaderProject = new ColumnHeader();
        columnHeaderHost = new ColumnHeader();
        columnHeaderPort = new ColumnHeader();
        labelName = new Label();
        nameTextBox = new TextBox();
        labelProject = new Label();
        projectPathTextBox = new TextBox();
        buttonBrowse = new Button();
        labelHost = new Label();
        hostComboBox = new ComboBox();
        labelPort = new Label();
        portNumericUpDown = new NumericUpDown();
        labelModel = new Label();
        modelTextBox = new TextBox();
        continueSessionCheckBox = new CheckBox();
        labelSessionId = new Label();
        sessionIdTextBox = new TextBox();
        labelExtraArgs = new Label();
        extraArgsTextBox = new TextBox();
        labelServerUsername = new Label();
        serverUsernameTextBox = new TextBox();
        labelServerPassword = new Label();
        serverPasswordTextBox = new TextBox();
        buttonAdd = new Button();
        buttonSave = new Button();
        buttonDelete = new Button();
        buttonLaunch = new Button();
        buttonStop = new Button();
        labelStatusCaption = new Label();
        statusLabel = new Label();
        labelActionCaption = new Label();
        actionStatusLabel = new Label();
        folderBrowserDialog = new FolderBrowserDialog();
        ((System.ComponentModel.ISupportInitialize)portNumericUpDown).BeginInit();
        SuspendLayout();
        // 
        // profilesListView
        // 
        profilesListView.Columns.AddRange(new ColumnHeader[] { columnHeaderName, columnHeaderProject, columnHeaderHost, columnHeaderPort });
        profilesListView.FullRowSelect = true;
        profilesListView.Location = new Point(12, 12);
        profilesListView.MultiSelect = false;
        profilesListView.Name = "profilesListView";
        profilesListView.Size = new Size(420, 445);
        profilesListView.TabIndex = 0;
        profilesListView.UseCompatibleStateImageBehavior = false;
        profilesListView.View = View.Details;
        profilesListView.SelectedIndexChanged += profilesListView_SelectedIndexChanged;
        // 
        // columnHeaderName
        // 
        columnHeaderName.Text = "Имя";
        columnHeaderName.Width = 100;
        // 
        // columnHeaderProject
        // 
        columnHeaderProject.Text = "Проект";
        columnHeaderProject.Width = 160;
        // 
        // columnHeaderHost
        // 
        columnHeaderHost.Text = "Хост";
        columnHeaderHost.Width = 100;
        // 
        // columnHeaderPort
        // 
        columnHeaderPort.Text = "Порт";
        columnHeaderPort.Width = 55;
        // 
        // labelName
        // 
        labelName.AutoSize = true;
        labelName.Location = new Point(445, 15);
        labelName.Name = "labelName";
        labelName.Size = new Size(31, 15);
        labelName.TabIndex = 1;
        labelName.Text = "Имя";
        // 
        // nameTextBox
        // 
        nameTextBox.Location = new Point(550, 12);
        nameTextBox.Name = "nameTextBox";
        nameTextBox.Size = new Size(280, 23);
        nameTextBox.TabIndex = 2;
        // 
        // labelProject
        // 
        labelProject.AutoSize = true;
        labelProject.Location = new Point(445, 45);
        labelProject.Name = "labelProject";
        labelProject.Size = new Size(47, 15);
        labelProject.TabIndex = 3;
        labelProject.Text = "Проект";
        // 
        // projectPathTextBox
        // 
        projectPathTextBox.Location = new Point(550, 42);
        projectPathTextBox.Name = "projectPathTextBox";
        projectPathTextBox.Size = new Size(200, 23);
        projectPathTextBox.TabIndex = 4;
        // 
        // buttonBrowse
        // 
        buttonBrowse.Location = new Point(755, 41);
        buttonBrowse.Name = "buttonBrowse";
        buttonBrowse.Size = new Size(75, 23);
        buttonBrowse.TabIndex = 5;
        buttonBrowse.Text = "Обзор…";
        buttonBrowse.Click += buttonBrowse_Click;
        // 
        // labelHost
        // 
        labelHost.AutoSize = true;
        labelHost.Location = new Point(445, 75);
        labelHost.Name = "labelHost";
        labelHost.Size = new Size(32, 15);
        labelHost.TabIndex = 6;
        labelHost.Text = "Хост";
        // 
        // hostComboBox
        // 
        hostComboBox.Location = new Point(550, 72);
        hostComboBox.Name = "hostComboBox";
        hostComboBox.Size = new Size(280, 23);
        hostComboBox.TabIndex = 7;
        // 
        // labelPort
        // 
        labelPort.AutoSize = true;
        labelPort.Location = new Point(445, 105);
        labelPort.Name = "labelPort";
        labelPort.Size = new Size(35, 15);
        labelPort.TabIndex = 8;
        labelPort.Text = "Порт";
        // 
        // portNumericUpDown
        // 
        portNumericUpDown.Location = new Point(550, 102);
        portNumericUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        portNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        portNumericUpDown.Name = "portNumericUpDown";
        portNumericUpDown.Size = new Size(70, 23);
        portNumericUpDown.TabIndex = 9;
        portNumericUpDown.Value = new decimal(new int[] { 4000, 0, 0, 0 });
        // 
        // labelModel
        // 
        labelModel.AutoSize = true;
        labelModel.Location = new Point(445, 135);
        labelModel.Name = "labelModel";
        labelModel.Size = new Size(50, 15);
        labelModel.TabIndex = 10;
        labelModel.Text = "Модель";
        // 
        // modelTextBox
        // 
        modelTextBox.Location = new Point(550, 132);
        modelTextBox.Name = "modelTextBox";
        modelTextBox.Size = new Size(280, 23);
        modelTextBox.TabIndex = 11;
        // 
        // continueSessionCheckBox
        // 
        continueSessionCheckBox.AutoSize = true;
        continueSessionCheckBox.Location = new Point(550, 162);
        continueSessionCheckBox.Name = "continueSessionCheckBox";
        continueSessionCheckBox.Size = new Size(209, 19);
        continueSessionCheckBox.TabIndex = 12;
        continueSessionCheckBox.Text = "Продолжить последнюю сессию";
        continueSessionCheckBox.UseVisualStyleBackColor = true;
        // 
        // labelSessionId
        // 
        labelSessionId.AutoSize = true;
        labelSessionId.Location = new Point(445, 195);
        labelSessionId.Name = "labelSessionId";
        labelSessionId.Size = new Size(59, 15);
        labelSessionId.TabIndex = 13;
        labelSessionId.Text = "Session id";
        // 
        // sessionIdTextBox
        // 
        sessionIdTextBox.Location = new Point(550, 192);
        sessionIdTextBox.Name = "sessionIdTextBox";
        sessionIdTextBox.Size = new Size(280, 23);
        sessionIdTextBox.TabIndex = 14;
        // 
        // labelExtraArgs
        // 
        labelExtraArgs.AutoSize = true;
        labelExtraArgs.Location = new Point(445, 225);
        labelExtraArgs.Name = "labelExtraArgs";
        labelExtraArgs.Size = new Size(95, 15);
        labelExtraArgs.TabIndex = 15;
        labelExtraArgs.Text = "Доп. аргументы";
        // 
        // extraArgsTextBox
        // 
        extraArgsTextBox.Location = new Point(550, 222);
        extraArgsTextBox.Name = "extraArgsTextBox";
        extraArgsTextBox.Size = new Size(280, 23);
        extraArgsTextBox.TabIndex = 16;
        // 
        // labelServerUsername
        // 
        labelServerUsername.AutoSize = true;
        labelServerUsername.Location = new Point(445, 255);
        labelServerUsername.Name = "labelServerUsername";
        labelServerUsername.Size = new Size(44, 15);
        labelServerUsername.TabIndex = 26;
        labelServerUsername.Text = "Логин:";
        // 
        // serverUsernameTextBox
        // 
        serverUsernameTextBox.Location = new Point(550, 252);
        serverUsernameTextBox.Name = "serverUsernameTextBox";
        serverUsernameTextBox.Size = new Size(280, 23);
        serverUsernameTextBox.TabIndex = 27;
        // 
        // labelServerPassword
        // 
        labelServerPassword.AutoSize = true;
        labelServerPassword.Location = new Point(445, 285);
        labelServerPassword.Name = "labelServerPassword";
        labelServerPassword.Size = new Size(52, 15);
        labelServerPassword.TabIndex = 28;
        labelServerPassword.Text = "Пароль:";
        // 
        // serverPasswordTextBox
        // 
        serverPasswordTextBox.Location = new Point(550, 282);
        serverPasswordTextBox.Name = "serverPasswordTextBox";
        serverPasswordTextBox.Size = new Size(280, 23);
        serverPasswordTextBox.TabIndex = 29;
        serverPasswordTextBox.UseSystemPasswordChar = true;
        // 
        // buttonAdd
        // 
        buttonAdd.Location = new Point(445, 315);
        buttonAdd.Name = "buttonAdd";
        buttonAdd.Size = new Size(100, 25);
        buttonAdd.TabIndex = 17;
        buttonAdd.Text = "Добавить";
        buttonAdd.Click += buttonAdd_Click;
        // 
        // buttonSave
        // 
        buttonSave.Location = new Point(550, 315);
        buttonSave.Name = "buttonSave";
        buttonSave.Size = new Size(130, 25);
        buttonSave.TabIndex = 18;
        buttonSave.Text = "Сохранить изменения";
        buttonSave.Click += buttonSave_Click;
        // 
        // buttonDelete
        // 
        buttonDelete.Location = new Point(685, 315);
        buttonDelete.Name = "buttonDelete";
        buttonDelete.Size = new Size(85, 25);
        buttonDelete.TabIndex = 19;
        buttonDelete.Text = "Удалить";
        buttonDelete.Click += buttonDelete_Click;
        // 
        // buttonLaunch
        // 
        buttonLaunch.Location = new Point(445, 345);
        buttonLaunch.Name = "buttonLaunch";
        buttonLaunch.Size = new Size(100, 25);
        buttonLaunch.TabIndex = 20;
        buttonLaunch.Text = "Запустить";
        buttonLaunch.Click += buttonLaunch_Click;
        // 
        // buttonStop
        // 
        buttonStop.Enabled = false;
        buttonStop.Location = new Point(550, 345);
        buttonStop.Name = "buttonStop";
        buttonStop.Size = new Size(110, 25);
        buttonStop.TabIndex = 21;
        buttonStop.Text = "Остановить";
        buttonStop.Click += buttonStop_Click;
        // 
        // labelStatusCaption
        // 
        labelStatusCaption.AutoSize = true;
        labelStatusCaption.Location = new Point(445, 385);
        labelStatusCaption.Name = "labelStatusCaption";
        labelStatusCaption.Size = new Size(46, 15);
        labelStatusCaption.TabIndex = 22;
        labelStatusCaption.Text = "Статус:";
        // 
        // statusLabel
        // 
        statusLabel.AutoSize = true;
        statusLabel.Location = new Point(505, 385);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(73, 15);
        statusLabel.TabIndex = 23;
        statusLabel.Text = "Остановлен";
        // 
        // labelActionCaption
        // 
        labelActionCaption.AutoSize = true;
        labelActionCaption.Location = new Point(445, 410);
        labelActionCaption.Name = "labelActionCaption";
        labelActionCaption.Size = new Size(61, 15);
        labelActionCaption.TabIndex = 24;
        labelActionCaption.Text = "Действие:";
        // 
        // actionStatusLabel
        // 
        actionStatusLabel.AutoSize = true;
        actionStatusLabel.Location = new Point(505, 410);
        actionStatusLabel.Name = "actionStatusLabel";
        actionStatusLabel.Size = new Size(19, 15);
        actionStatusLabel.TabIndex = 25;
        actionStatusLabel.Text = "—";
        // 
        // ProfilesView
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(profilesListView);
        Controls.Add(labelName);
        Controls.Add(nameTextBox);
        Controls.Add(labelProject);
        Controls.Add(projectPathTextBox);
        Controls.Add(buttonBrowse);
        Controls.Add(labelHost);
        Controls.Add(hostComboBox);
        Controls.Add(labelPort);
        Controls.Add(portNumericUpDown);
        Controls.Add(labelModel);
        Controls.Add(modelTextBox);
        Controls.Add(continueSessionCheckBox);
        Controls.Add(labelSessionId);
        Controls.Add(sessionIdTextBox);
        Controls.Add(labelExtraArgs);
        Controls.Add(extraArgsTextBox);
        Controls.Add(labelServerUsername);
        Controls.Add(serverUsernameTextBox);
        Controls.Add(labelServerPassword);
        Controls.Add(serverPasswordTextBox);
        Controls.Add(buttonAdd);
        Controls.Add(buttonSave);
        Controls.Add(buttonDelete);
        Controls.Add(buttonLaunch);
        Controls.Add(buttonStop);
        Controls.Add(labelStatusCaption);
        Controls.Add(statusLabel);
        Controls.Add(labelActionCaption);
        Controls.Add(actionStatusLabel);
        Name = "ProfilesView";
        Size = new Size(1508, 804);
        ((System.ComponentModel.ISupportInitialize)portNumericUpDown).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}