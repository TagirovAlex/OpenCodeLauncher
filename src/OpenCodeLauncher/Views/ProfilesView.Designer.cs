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
        this.components = new System.ComponentModel.Container();
        this.profilesListView = new ListView();
        this.columnHeaderName = new ColumnHeader();
        this.columnHeaderProject = new ColumnHeader();
        this.columnHeaderHost = new ColumnHeader();
        this.columnHeaderPort = new ColumnHeader();
        this.labelName = new Label();
        this.nameTextBox = new TextBox();
        this.labelProject = new Label();
        this.projectPathTextBox = new TextBox();
        this.buttonBrowse = new Button();
        this.labelHost = new Label();
        this.hostComboBox = new ComboBox();
        this.labelPort = new Label();
        this.portNumericUpDown = new NumericUpDown();
        this.labelModel = new Label();
        this.modelTextBox = new TextBox();
        this.continueSessionCheckBox = new CheckBox();
        this.labelSessionId = new Label();
        this.sessionIdTextBox = new TextBox();
        this.labelExtraArgs = new Label();
        this.extraArgsTextBox = new TextBox();
        this.labelServerUsername = new Label();
        this.serverUsernameTextBox = new TextBox();
        this.labelServerPassword = new Label();
        this.serverPasswordTextBox = new TextBox();
        this.buttonAdd = new Button();
        this.buttonSave = new Button();
        this.buttonDelete = new Button();
        this.buttonLaunch = new Button();
        this.buttonStop = new Button();
        this.labelStatusCaption = new Label();
        this.statusLabel = new Label();
        this.labelActionCaption = new Label();
        this.actionStatusLabel = new Label();
        this.folderBrowserDialog = new FolderBrowserDialog();
        this.SuspendLayout();
        // 
        // profilesListView
        // 
        this.profilesListView.Columns.AddRange(new ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderProject,
            this.columnHeaderHost,
            this.columnHeaderPort});
        this.profilesListView.FullRowSelect = true;
        this.profilesListView.Location = new Point(12, 12);
        this.profilesListView.MultiSelect = false;
        this.profilesListView.Name = "profilesListView";
        this.profilesListView.Size = new Size(420, 445);
        this.profilesListView.TabIndex = 0;
        this.profilesListView.UseCompatibleStateImageBehavior = false;
        this.profilesListView.View = View.Details;
        this.profilesListView.SelectedIndexChanged += new EventHandler(this.profilesListView_SelectedIndexChanged);
        // 
        // columnHeaderName
        // 
        this.columnHeaderName.Text = "Имя";
        this.columnHeaderName.Width = 100;
        // 
        // columnHeaderProject
        // 
        this.columnHeaderProject.Text = "Проект";
        this.columnHeaderProject.Width = 160;
        // 
        // columnHeaderHost
        // 
        this.columnHeaderHost.Text = "Хост";
        this.columnHeaderHost.Width = 100;
        // 
        // columnHeaderPort
        // 
        this.columnHeaderPort.Text = "Порт";
        this.columnHeaderPort.Width = 55;
        // 
        // labelName
        // 
        this.labelName.AutoSize = true;
        this.labelName.Location = new Point(445, 15);
        this.labelName.Name = "labelName";
        this.labelName.Size = new Size(30, 15);
        this.labelName.TabIndex = 1;
        this.labelName.Text = "Имя";
        // 
        // nameTextBox
        // 
        this.nameTextBox.Location = new Point(520, 12);
        this.nameTextBox.Name = "nameTextBox";
        this.nameTextBox.Size = new Size(280, 21);
        this.nameTextBox.TabIndex = 2;
        // 
        // labelProject
        // 
        this.labelProject.AutoSize = true;
        this.labelProject.Location = new Point(445, 45);
        this.labelProject.Name = "labelProject";
        this.labelProject.Size = new Size(43, 15);
        this.labelProject.TabIndex = 3;
        this.labelProject.Text = "Проект";
        // 
        // projectPathTextBox
        // 
        this.projectPathTextBox.Location = new Point(520, 42);
        this.projectPathTextBox.Name = "projectPathTextBox";
        this.projectPathTextBox.Size = new Size(200, 21);
        this.projectPathTextBox.TabIndex = 4;
        // 
        // buttonBrowse
        // 
        this.buttonBrowse.Location = new Point(725, 41);
        this.buttonBrowse.Name = "buttonBrowse";
        this.buttonBrowse.Size = new Size(75, 23);
        this.buttonBrowse.TabIndex = 5;
        this.buttonBrowse.Text = "Обзор…";
        this.buttonBrowse.Click += new EventHandler(this.buttonBrowse_Click);
        // 
        // labelHost
        // 
        this.labelHost.AutoSize = true;
        this.labelHost.Location = new Point(445, 75);
        this.labelHost.Name = "labelHost";
        this.labelHost.Size = new Size(30, 15);
        this.labelHost.TabIndex = 6;
        this.labelHost.Text = "Хост";
        // 
        // hostComboBox
        // 
        this.hostComboBox.Location = new Point(520, 72);
        this.hostComboBox.Name = "hostComboBox";
        this.hostComboBox.Size = new Size(280, 21);
        this.hostComboBox.TabIndex = 7;
        // 
        // labelPort
        // 
        this.labelPort.AutoSize = true;
        this.labelPort.Location = new Point(445, 105);
        this.labelPort.Name = "labelPort";
        this.labelPort.Size = new Size(30, 15);
        this.labelPort.TabIndex = 8;
        this.labelPort.Text = "Порт";
        // 
        // portNumericUpDown
        // 
        this.portNumericUpDown.Location = new Point(520, 102);
        this.portNumericUpDown.Maximum = 65535;
        this.portNumericUpDown.Minimum = 1;
        this.portNumericUpDown.Name = "portNumericUpDown";
        this.portNumericUpDown.Size = new Size(70, 21);
        this.portNumericUpDown.TabIndex = 9;
        this.portNumericUpDown.Value = 4000;
        // 
        // labelModel
        // 
        this.labelModel.AutoSize = true;
        this.labelModel.Location = new Point(445, 135);
        this.labelModel.Name = "labelModel";
        this.labelModel.Size = new Size(46, 15);
        this.labelModel.TabIndex = 10;
        this.labelModel.Text = "Модель";
        // 
        // modelTextBox
        // 
        this.modelTextBox.Location = new Point(520, 132);
        this.modelTextBox.Name = "modelTextBox";
        this.modelTextBox.Size = new Size(280, 21);
        this.modelTextBox.TabIndex = 11;
        // 
        // continueSessionCheckBox
        // 
        this.continueSessionCheckBox.AutoSize = true;
        this.continueSessionCheckBox.Location = new Point(520, 162);
        this.continueSessionCheckBox.Name = "continueSessionCheckBox";
        this.continueSessionCheckBox.Size = new Size(280, 19);
        this.continueSessionCheckBox.TabIndex = 12;
        this.continueSessionCheckBox.Text = "Продолжить последнюю сессию";
        this.continueSessionCheckBox.UseVisualStyleBackColor = true;
        // 
        // labelSessionId
        // 
        this.labelSessionId.AutoSize = true;
        this.labelSessionId.Location = new Point(445, 195);
        this.labelSessionId.Name = "labelSessionId";
        this.labelSessionId.Size = new Size(55, 15);
        this.labelSessionId.TabIndex = 13;
        this.labelSessionId.Text = "Session id";
        // 
        // sessionIdTextBox
        // 
        this.sessionIdTextBox.Location = new Point(520, 192);
        this.sessionIdTextBox.Name = "sessionIdTextBox";
        this.sessionIdTextBox.Size = new Size(280, 21);
        this.sessionIdTextBox.TabIndex = 14;
        // 
        // labelExtraArgs
        // 
        this.labelExtraArgs.AutoSize = true;
        this.labelExtraArgs.Location = new Point(445, 225);
        this.labelExtraArgs.Name = "labelExtraArgs";
        this.labelExtraArgs.Size = new Size(91, 15);
        this.labelExtraArgs.TabIndex = 15;
        this.labelExtraArgs.Text = "Доп. аргументы";
        // 
        // extraArgsTextBox
        // 
        this.extraArgsTextBox.Location = new Point(520, 222);
        this.extraArgsTextBox.Name = "extraArgsTextBox";
        this.extraArgsTextBox.Size = new Size(280, 21);
        this.extraArgsTextBox.TabIndex = 16;
        // 
        // labelServerUsername
        // 
        this.labelServerUsername.AutoSize = true;
        this.labelServerUsername.Location = new Point(445, 255);
        this.labelServerUsername.Name = "labelServerUsername";
        this.labelServerUsername.Size = new Size(43, 15);
        this.labelServerUsername.TabIndex = 26;
        this.labelServerUsername.Text = "Логин:";
        // 
        // serverUsernameTextBox
        // 
        this.serverUsernameTextBox.Location = new Point(520, 252);
        this.serverUsernameTextBox.Name = "serverUsernameTextBox";
        this.serverUsernameTextBox.Size = new Size(280, 23);
        this.serverUsernameTextBox.TabIndex = 27;
        // 
        // labelServerPassword
        // 
        this.labelServerPassword.AutoSize = true;
        this.labelServerPassword.Location = new Point(445, 285);
        this.labelServerPassword.Name = "labelServerPassword";
        this.labelServerPassword.Size = new Size(48, 15);
        this.labelServerPassword.TabIndex = 28;
        this.labelServerPassword.Text = "Пароль:";
        // 
        // serverPasswordTextBox
        // 
        this.serverPasswordTextBox.Location = new Point(520, 282);
        this.serverPasswordTextBox.Name = "serverPasswordTextBox";
        this.serverPasswordTextBox.Size = new Size(280, 23);
        this.serverPasswordTextBox.TabIndex = 29;
        this.serverPasswordTextBox.UseSystemPasswordChar = true;
        // 
        // buttonAdd
        // 
        this.buttonAdd.Location = new Point(445, 315);
        this.buttonAdd.Name = "buttonAdd";
        this.buttonAdd.Size = new Size(100, 25);
        this.buttonAdd.TabIndex = 17;
        this.buttonAdd.Text = "Добавить";
        this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
        // 
        // buttonSave
        // 
        this.buttonSave.Location = new Point(550, 315);
        this.buttonSave.Name = "buttonSave";
        this.buttonSave.Size = new Size(130, 25);
        this.buttonSave.TabIndex = 18;
        this.buttonSave.Text = "Сохранить изменения";
        this.buttonSave.Click += new EventHandler(this.buttonSave_Click);
        // 
        // buttonDelete
        // 
        this.buttonDelete.Location = new Point(685, 315);
        this.buttonDelete.Name = "buttonDelete";
        this.buttonDelete.Size = new Size(85, 25);
        this.buttonDelete.TabIndex = 19;
        this.buttonDelete.Text = "Удалить";
        this.buttonDelete.Click += new EventHandler(this.buttonDelete_Click);
        // 
        // buttonLaunch
        // 
        this.buttonLaunch.Location = new Point(445, 345);
        this.buttonLaunch.Name = "buttonLaunch";
        this.buttonLaunch.Size = new Size(100, 25);
        this.buttonLaunch.TabIndex = 20;
        this.buttonLaunch.Text = "Запустить";
        this.buttonLaunch.Click += new EventHandler(this.buttonLaunch_Click);
        // 
        // buttonStop
        // 
        this.buttonStop.Enabled = false;
        this.buttonStop.Location = new Point(550, 345);
        this.buttonStop.Name = "buttonStop";
        this.buttonStop.Size = new Size(110, 25);
        this.buttonStop.TabIndex = 21;
        this.buttonStop.Text = "Остановить";
        this.buttonStop.Click += new EventHandler(this.buttonStop_Click);
        // 
        // labelStatusCaption
        // 
        this.labelStatusCaption.AutoSize = true;
        this.labelStatusCaption.Location = new Point(445, 385);
        this.labelStatusCaption.Name = "labelStatusCaption";
        this.labelStatusCaption.Size = new Size(45, 15);
        this.labelStatusCaption.TabIndex = 22;
        this.labelStatusCaption.Text = "Статус:";
        // 
        // statusLabel
        // 
        this.statusLabel.AutoSize = true;
        this.statusLabel.Location = new Point(505, 385);
        this.statusLabel.Name = "statusLabel";
        this.statusLabel.Size = new Size(250, 15);
        this.statusLabel.TabIndex = 23;
        this.statusLabel.Text = "Остановлен";
        // 
        // labelActionCaption
        // 
        this.labelActionCaption.AutoSize = true;
        this.labelActionCaption.Location = new Point(445, 410);
        this.labelActionCaption.Name = "labelActionCaption";
        this.labelActionCaption.Size = new Size(51, 15);
        this.labelActionCaption.TabIndex = 24;
        this.labelActionCaption.Text = "Действие:";
        // 
        // actionStatusLabel
        // 
        this.actionStatusLabel.AutoSize = true;
        this.actionStatusLabel.Location = new Point(505, 410);
        this.actionStatusLabel.Name = "actionStatusLabel";
        this.actionStatusLabel.Size = new Size(250, 15);
        this.actionStatusLabel.TabIndex = 25;
        this.actionStatusLabel.Text = "—";
        // 
        // ProfilesView
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.Controls.Add(this.profilesListView);
        this.Controls.Add(this.labelName);
        this.Controls.Add(this.nameTextBox);
        this.Controls.Add(this.labelProject);
        this.Controls.Add(this.projectPathTextBox);
        this.Controls.Add(this.buttonBrowse);
        this.Controls.Add(this.labelHost);
        this.Controls.Add(this.hostComboBox);
        this.Controls.Add(this.labelPort);
        this.Controls.Add(this.portNumericUpDown);
        this.Controls.Add(this.labelModel);
        this.Controls.Add(this.modelTextBox);
        this.Controls.Add(this.continueSessionCheckBox);
        this.Controls.Add(this.labelSessionId);
        this.Controls.Add(this.sessionIdTextBox);
        this.Controls.Add(this.labelExtraArgs);
        this.Controls.Add(this.extraArgsTextBox);
        this.Controls.Add(this.labelServerUsername);
        this.Controls.Add(this.serverUsernameTextBox);
        this.Controls.Add(this.labelServerPassword);
        this.Controls.Add(this.serverPasswordTextBox);
        this.Controls.Add(this.buttonAdd);
        this.Controls.Add(this.buttonSave);
        this.Controls.Add(this.buttonDelete);
        this.Controls.Add(this.buttonLaunch);
        this.Controls.Add(this.buttonStop);
        this.Controls.Add(this.labelStatusCaption);
        this.Controls.Add(this.statusLabel);
        this.Controls.Add(this.labelActionCaption);
        this.Controls.Add(this.actionStatusLabel);
        this.Dock = DockStyle.Fill;
        this.Name = "ProfilesView";
        this.Size = new Size(960, 470);
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}