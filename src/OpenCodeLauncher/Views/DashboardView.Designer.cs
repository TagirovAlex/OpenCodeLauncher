namespace OpenCodeLauncher.Views;

partial class DashboardView
{
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.TableLayoutPanel rootLayout;
    private System.Windows.Forms.FlowLayoutPanel topBarFlow;
    private System.Windows.Forms.Label statusLabel;
    private System.Windows.Forms.Label urlLabel;
    private System.Windows.Forms.Button refreshButton;
    private System.Windows.Forms.CheckBox autoRefreshCheckBox;
    private System.Windows.Forms.DataGridView sessionsGrid;
    private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn titleColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn createdAtColumn;
    private System.Windows.Forms.FlowLayoutPanel statsFlow;
    private System.Windows.Forms.Label statsTokensLabel;
    private System.Windows.Forms.Label statsCostLabel;
    private System.Windows.Forms.Label statusLineLabel;

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
        this.rootLayout = new System.Windows.Forms.TableLayoutPanel();
        this.topBarFlow = new System.Windows.Forms.FlowLayoutPanel();
        this.statusLabel = new System.Windows.Forms.Label();
        this.urlLabel = new System.Windows.Forms.Label();
        this.refreshButton = new System.Windows.Forms.Button();
        this.autoRefreshCheckBox = new System.Windows.Forms.CheckBox();
        this.sessionsGrid = new System.Windows.Forms.DataGridView();
        this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.titleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.createdAtColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.statsFlow = new System.Windows.Forms.FlowLayoutPanel();
        this.statsTokensLabel = new System.Windows.Forms.Label();
        this.statsCostLabel = new System.Windows.Forms.Label();
        this.statusLineLabel = new System.Windows.Forms.Label();
        this.rootLayout.SuspendLayout();
        this.topBarFlow.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.sessionsGrid)).BeginInit();
        this.statsFlow.SuspendLayout();
        this.SuspendLayout();
        // 
        // rootLayout
        // 
        this.rootLayout.ColumnCount = 1;
        this.rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.rootLayout.Controls.Add(this.topBarFlow, 0, 0);
        this.rootLayout.Controls.Add(this.sessionsGrid, 0, 1);
        this.rootLayout.Controls.Add(this.statsFlow, 0, 2);
        this.rootLayout.Controls.Add(this.statusLineLabel, 0, 3);
        this.rootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        this.rootLayout.Location = new System.Drawing.Point(0, 0);
        this.rootLayout.Name = "rootLayout";
        this.rootLayout.RowCount = 4;
        this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
        this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
        this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
        this.rootLayout.Size = new System.Drawing.Size(776, 473);
        this.rootLayout.TabIndex = 0;
        // 
        // topBarFlow
        // 
        this.topBarFlow.Controls.Add(this.statusLabel);
        this.topBarFlow.Controls.Add(this.urlLabel);
        this.topBarFlow.Controls.Add(this.refreshButton);
        this.topBarFlow.Controls.Add(this.autoRefreshCheckBox);
        this.topBarFlow.Dock = System.Windows.Forms.DockStyle.Fill;
        this.topBarFlow.Location = new System.Drawing.Point(0, 0);
        this.topBarFlow.Name = "topBarFlow";
        this.topBarFlow.Padding = new System.Windows.Forms.Padding(4, 5, 4, 4);
        this.topBarFlow.Size = new System.Drawing.Size(776, 34);
        this.topBarFlow.TabIndex = 0;
        // 
        // statusLabel
        // 
        this.statusLabel.AutoSize = true;
        this.statusLabel.Location = new System.Drawing.Point(7, 8);
        this.statusLabel.Name = "statusLabel";
        this.statusLabel.Size = new System.Drawing.Size(66, 15);
        this.statusLabel.TabIndex = 0;
        this.statusLabel.Text = "Статус: —";
        // 
        // urlLabel
        // 
        this.urlLabel.AutoSize = true;
        this.urlLabel.Location = new System.Drawing.Point(79, 8);
        this.urlLabel.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
        this.urlLabel.Name = "urlLabel";
        this.urlLabel.Size = new System.Drawing.Size(48, 15);
        this.urlLabel.TabIndex = 1;
        this.urlLabel.Text = "URL: —";
        // 
        // refreshButton
        // 
        this.refreshButton.Location = new System.Drawing.Point(136, 5);
        this.refreshButton.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
        this.refreshButton.Name = "refreshButton";
        this.refreshButton.Size = new System.Drawing.Size(90, 25);
        this.refreshButton.TabIndex = 2;
        this.refreshButton.Text = "Обновить";
        this.refreshButton.UseVisualStyleBackColor = true;
        // 
        // autoRefreshCheckBox
        // 
        this.autoRefreshCheckBox.AutoSize = true;
        this.autoRefreshCheckBox.Location = new System.Drawing.Point(235, 8);
        this.autoRefreshCheckBox.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
        this.autoRefreshCheckBox.Name = "autoRefreshCheckBox";
        this.autoRefreshCheckBox.Size = new System.Drawing.Size(188, 19);
        this.autoRefreshCheckBox.TabIndex = 3;
        this.autoRefreshCheckBox.Text = "Автообновление каждые 10 с";
        this.autoRefreshCheckBox.UseVisualStyleBackColor = true;
        // 
        // sessionsGrid
        // 
        this.sessionsGrid.AllowUserToAddRows = false;
        this.sessionsGrid.AllowUserToDeleteRows = false;
        this.sessionsGrid.AllowUserToOrderColumns = false;
        this.sessionsGrid.AllowUserToResizeRows = false;
        this.sessionsGrid.AutoGenerateColumns = false;
        this.sessionsGrid.BackgroundColor = System.Drawing.SystemColors.Window;
        this.sessionsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.sessionsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idColumn,
            this.titleColumn,
            this.createdAtColumn});
        this.sessionsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
        this.sessionsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
        this.sessionsGrid.Location = new System.Drawing.Point(3, 37);
        this.sessionsGrid.MultiSelect = false;
        this.sessionsGrid.Name = "sessionsGrid";
        this.sessionsGrid.ReadOnly = true;
        this.sessionsGrid.RowHeadersVisible = false;
        this.sessionsGrid.RowTemplate.Height = 25;
        this.sessionsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.sessionsGrid.Size = new System.Drawing.Size(770, 369);
        this.sessionsGrid.TabIndex = 1;
        // 
        // idColumn
        // 
        this.idColumn.HeaderText = "Id";
        this.idColumn.Name = "idColumn";
        this.idColumn.ReadOnly = true;
        this.idColumn.Width = 130;
        // 
        // titleColumn
        // 
        this.titleColumn.HeaderText = "Название";
        this.titleColumn.Name = "titleColumn";
        this.titleColumn.ReadOnly = true;
        this.titleColumn.Width = 420;
        // 
        // createdAtColumn
        // 
        this.createdAtColumn.HeaderText = "Создана";
        this.createdAtColumn.Name = "createdAtColumn";
        this.createdAtColumn.ReadOnly = true;
        this.createdAtColumn.Width = 160;
        // 
        // statsFlow
        // 
        this.statsFlow.Controls.Add(this.statsTokensLabel);
        this.statsFlow.Controls.Add(this.statsCostLabel);
        this.statsFlow.Dock = System.Windows.Forms.DockStyle.Fill;
        this.statsFlow.Location = new System.Drawing.Point(0, 409);
        this.statsFlow.Name = "statsFlow";
        this.statsFlow.Padding = new System.Windows.Forms.Padding(4, 8, 4, 4);
        this.statsFlow.Size = new System.Drawing.Size(776, 40);
        this.statsFlow.TabIndex = 2;
        // 
        // statsTokensLabel
        // 
        this.statsTokensLabel.AutoSize = true;
        this.statsTokensLabel.Location = new System.Drawing.Point(7, 11);
        this.statsTokensLabel.Name = "statsTokensLabel";
        this.statsTokensLabel.Size = new System.Drawing.Size(106, 15);
        this.statsTokensLabel.TabIndex = 0;
        this.statsTokensLabel.Text = "Токенов в/из: —";
        // 
        // statsCostLabel
        // 
        this.statsCostLabel.AutoSize = true;
        this.statsCostLabel.Location = new System.Drawing.Point(119, 11);
        this.statsCostLabel.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
        this.statsCostLabel.Name = "statsCostLabel";
        this.statsCostLabel.Size = new System.Drawing.Size(72, 15);
        this.statsCostLabel.TabIndex = 1;
        this.statsCostLabel.Text = "Стоимость: —";
        // 
        // statusLineLabel
        // 
        this.statusLineLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.statusLineLabel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.statusLineLabel.Location = new System.Drawing.Point(0, 449);
        this.statusLineLabel.Name = "statusLineLabel";
        this.statusLineLabel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.statusLineLabel.Size = new System.Drawing.Size(776, 24);
        this.statusLineLabel.TabIndex = 3;
        this.statusLineLabel.Text = "Готово";
        this.statusLineLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // DashboardView
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Controls.Add(this.rootLayout);
        this.Name = "DashboardView";
        this.Size = new System.Drawing.Size(776, 473);
        this.rootLayout.ResumeLayout(false);
        this.topBarFlow.ResumeLayout(false);
        this.topBarFlow.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.sessionsGrid)).EndInit();
        this.statsFlow.ResumeLayout(false);
        this.statsFlow.PerformLayout();
        this.ResumeLayout(false);
    }
}