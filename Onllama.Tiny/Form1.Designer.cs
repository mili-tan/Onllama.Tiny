namespace Onllama.Tiny
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.table1 = new AntdUI.Table();
            this.progress1 = new AntdUI.Progress();
            this.headerPanel = new AntdUI.Panel();
            this.appTitleLabel = new AntdUI.Label();
            this.statusLabel = new AntdUI.Label();
            this.cpuUsageLabel = new AntdUI.Label();
            this.ramUsageLabel = new AntdUI.Label();
            this.gpuUsageLabel = new AntdUI.Label();
            this.settingsButton = new AntdUI.Button();
            this.mainPanel = new AntdUI.Panel();
            this.footerPanel = new AntdUI.Panel();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.copyLogButton = new AntdUI.Button();
            this.clearLogButton = new AntdUI.Button();
            this.tabControlModels = new AntdUI.Tabs();
            this.tabPageLocalModels = new System.Windows.Forms.TabPage();
            this.tabPageOnlineModels = new System.Windows.Forms.TabPage();
            this.selectOnlineModelSource = new AntdUI.Select();
            this.selectOnlineModels = new AntdUI.Select();
            this.buttonPullOnlineModel = new AntdUI.Button();
            this.textBoxSearchOnlineModels = new AntdUI.Input();
            this.headerPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.tabControlModels.SuspendLayout();
            this.tabPageLocalModels.SuspendLayout();
            this.tabPageOnlineModels.SuspendLayout();
            this.SuspendLayout();
            // 
            // table1
            // 
            this.table1.Bordered = true;
            this.table1.Dock = DockStyle.Fill;
            this.table1.Location = new Point(0, 0);
            this.table1.Name = "table1";
            this.table1.Size = new Size(796, 259);
            this.table1.TabIndex = 0;
            this.table1.Text = "table1";
            this.table1.CellButtonClick += Table1OnCellButtonClick;
            // 
            // progress1
            // 
            this.progress1.Dock = DockStyle.Top;
            this.progress1.Location = new Point(0, 30); // Adjusted to be under headerPanel
            this.progress1.Name = "progress1";
            this.progress1.Size = new Size(804, 4);
            this.progress1.TabIndex = 2;
            this.progress1.Text = "";
            this.progress1.Visible = false; // Initially hidden
            //
            // headerPanel
            //
            this.headerPanel.Controls.Add(this.appTitleLabel);
            this.headerPanel.Controls.Add(this.statusLabel);
            this.headerPanel.Controls.Add(this.combinedUsageLabel); // Combined usage label
            this.headerPanel.Controls.Add(this.settingsButton);
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Location = new Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new Padding(5, 0, 5, 0); // Add some padding
            this.headerPanel.Size = new Size(804, 30);
            this.headerPanel.TabIndex = 3;
            //
            // appTitleLabel
            //
            this.appTitleLabel.Dock = DockStyle.Left;
            this.appTitleLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.appTitleLabel.Location = new Point(5, 0); // Adjusted for padding
            this.appTitleLabel.Name = "appTitleLabel";
            this.appTitleLabel.Size = new Size(100, 30);
            this.appTitleLabel.TabIndex = 0;
            this.appTitleLabel.Text = "Onllama";
            this.appTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            //
            // statusLabel
            //
            this.statusLabel.Dock = DockStyle.Fill; // Will fill space between title and usage/settings
            this.statusLabel.Location = new Point(105, 0); // Adjusted for padding
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new Size(434, 30); // Adjusted size
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "Готов";
            this.statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            //
            // combinedUsageLabel
            //
            this.combinedUsageLabel.Dock = DockStyle.Right;
            this.combinedUsageLabel.Location = new Point(539, 0); // Position before settings button
            this.combinedUsageLabel.Name = "combinedUsageLabel";
            this.combinedUsageLabel.Size = new Size(230, 30); // Adjust size to fit C: R: G:
            this.combinedUsageLabel.TabIndex = 2;
            this.combinedUsageLabel.Text = "C: --% R: --/-- GB G: --%";
            this.combinedUsageLabel.TextAlign = ContentAlignment.MiddleRight;
            //
            // settingsButton
            //
            this.settingsButton.Dock = DockStyle.Right;
            this.settingsButton.IconSvg = "<svg viewBox=\"64 64 896 896\" focusable=\"false\" data-icon=\"setting\" width=\"1em\" height=\"1em\" fill=\"currentColor\" aria-hidden=\"true\"><path d=\"M924.8 625.7l-65.5-56c3.1-19 4.7-38.4 4.7-57.8s-1.6-38.8-4.7-57.8l65.5-56a32.03 32.03 0 009.4-34.4l-32-72.4a32.03 32.03 0 00-34.4-9.4L767.2 310c-22.2-17.6-46.3-32.1-71.8-42.5L679 168.1a32.03 32.03 0 00-32.2-29.8h-72.4a32.03 32.03 0 00-32.2 29.8l-16.4 99.3c-25.5 10.4-49.6 24.9-71.8 42.5l-100.9-28.1a32.03 32.03 0 00-34.4 9.4l-32 72.4a32.03 32.03 0 009.4 34.4l65.5 56c-3.1 19-4.7 38.4-4.7 57.8s1.6 38.8 4.7 57.8l-65.5 56a32.03 32.03 0 00-9.4 34.4l32 72.4a32.03 32.03 0 0034.4 9.4l100.9-28.1c22.2 17.6 46.3 32.1 71.8 42.5l16.4 99.3a32.03 32.03 0 0032.2 29.8h72.4a32.03 32.03 0 0032.2-29.8l16.4-99.3c25.5-10.4 49.6-24.9 71.8-42.5l100.9 28.1a32.03 32.03 0 0034.4-9.4l32-72.4a32.03 32.03 0 00-9.4-34.4zM512 656c-80 0-144-64-144-144s64-144 144-144 144 64 144 144-64 144-144 144zm0-224c-44.2 0-80 35.8-80 80s35.8 80 80 80 80-35.8 80-80-35.8-80-80-80z\"></path></svg>";
            this.settingsButton.Location = new Point(769, 0); // Adjusted for padding
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new Size(30, 30);
            this.settingsButton.TabIndex = 5; // Keep TabIndex, or adjust as needed
            this.settingsButton.Type = AntdUI.TTypeMini.Primary;
            this.settingsButton.Shape = AntdUI.TShape.Circle;
            this.settingsButton.Margin = new Padding(3,3,0,3); // Add some margin to the button
            //
            // mainPanel
            //
            this.mainPanel.Controls.Add(this.tabControlModels);
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.Location = new Point(0, 34); // Below headerPanel and progress1
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new Size(804, 347); // Adjusted size
            this.mainPanel.TabIndex = 4;
            //
            // tabControlModels
            //
            this.tabControlModels.Controls.Add(this.tabPageLocalModels);
            this.tabControlModels.Controls.Add(this.tabPageOnlineModels);
            this.tabControlModels.Dock = DockStyle.Fill;
            this.tabControlModels.Location = new Point(2,2);
            this.tabControlModels.Name = "tabControlModels";
            this.tabControlModels.SelectedIndex = 0;
            this.tabControlModels.Size = new Size(800, 343); // Fill mainPanel
            this.tabControlModels.TabIndex = 1; // Changed from 0
            //
            // tabPageLocalModels
            //
            this.tabPageLocalModels.Controls.Add(this.table1);
            this.tabPageLocalModels.Location = new Point(4, 28);
            this.tabPageLocalModels.Name = "tabPageLocalModels";
            this.tabPageLocalModels.Padding = new Padding(3);
            this.tabPageLocalModels.Size = new Size(792, 311);
            this.tabPageLocalModels.TabIndex = 0;
            this.tabPageLocalModels.Text = "Установленные"; // To be localized
            this.tabPageLocalModels.UseVisualStyleBackColor = true;
            //
            // tabPageOnlineModels
            //
            this.tabPageOnlineModels.Padding = new Padding(5); // Reduced padding
            this.tabPageOnlineModels.Location = new Point(4, 28);
            this.tabPageOnlineModels.Name = "tabPageOnlineModels";
            this.tabPageOnlineModels.Size = new Size(792, 311);
            this.tabPageOnlineModels.TabIndex = 1;
            this.tabPageOnlineModels.Text = "Онлайн"; // To be localized
            this.tabPageOnlineModels.UseVisualStyleBackColor = true;
            // Add controls for online models here
            var onlineModelsFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                WrapContents = false,
                Padding = new Padding(0,0,0,5) // Reduced bottom padding
            };
            this.textBoxSearchOnlineModels = new AntdUI.Input { PlaceholderText = "Поиск онлайн моделей...", Width = 200, Margin = new Padding(0,0,5,0) };
            this.selectOnlineModelSource = new AntdUI.Select { Width = 120, Margin = new Padding(0,0,5,0) };
            this.selectOnlineModelSource.Items.AddRange(new object[] { "Ollama", "HuggingFace" });
            this.selectOnlineModelSource.SelectedIndex = 0;
            this.buttonPullOnlineModel = new AntdUI.Button { Text = "Загрузить", Type = AntdUI.TTypeMini.Primary, Margin = new Padding(0,0,5,0) };

            onlineModelsFlowPanel.Controls.Add(this.textBoxSearchOnlineModels);
            onlineModelsFlowPanel.Controls.Add(this.selectOnlineModelSource);
            onlineModelsFlowPanel.Controls.Add(this.buttonPullOnlineModel);

            this.selectOnlineModels = new AntdUI.Select { Dock = DockStyle.Top, PlaceholderText = "Выберите модель для загрузки" };

            this.tabPageOnlineModels.Controls.Add(this.selectOnlineModels);
            this.tabPageOnlineModels.Controls.Add(onlineModelsFlowPanel);
            //
            // footerPanel
            //
            this.footerPanel.Controls.Add(this.logTextBox);
            this.footerPanel.Controls.Add(this.copyLogButton);
            this.footerPanel.Controls.Add(this.clearLogButton);
            this.footerPanel.Dock = DockStyle.Bottom;
            this.footerPanel.Location = new Point(0, 381); // Adjusted position
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Size = new Size(804, 100); // Fixed height for log
            this.footerPanel.TabIndex = 5;
            //
            // logTextBox
            //
            this.logTextBox.Dock = DockStyle.Fill;
            this.logTextBox.Multiline = true;
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = ScrollBars.Vertical;
            this.logTextBox.Location = new Point(0, 0);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new Size(724, 100); // Adjusted width to make space for buttons
            this.logTextBox.TabIndex = 0;
            //
            // copyLogButton
            //
            this.copyLogButton.Dock = DockStyle.Right;
            this.copyLogButton.Location = new Point(724, 0);
            this.copyLogButton.Name = "copyLogButton";
            this.copyLogButton.Size = new Size(40, 100);
            this.copyLogButton.Text = "Копировать"; // To be localized
            this.copyLogButton.IconSvg = "<svg viewBox=\"64 64 896 896\" focusable=\"false\" data-icon=\"copy\" width=\"1em\" height=\"1em\" fill=\"currentColor\" aria-hidden=\"true\"><path d=\"M832 64H296c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h496v688c0 4.4 3.6 8 8 8h56c4.4 0 8-3.6 8-8V96c0-17.7-14.3-32-32-32zM704 192H192c-17.7 0-32 14.3-32 32v592c0 17.7 14.3 32 32 32h512c17.7 0 32-14.3 32-32V224c0-17.7-14.3-32-32-32zm-32 592H224V256h448v528z\"></path></svg>"; // Placeholder
            this.copyLogButton.Type = AntdUI.TTypeMini.Default;
            this.copyLogButton.Tooltip = "Копировать весь лог";
            //
            // clearLogButton
            //
            this.clearLogButton.Dock = DockStyle.Right;
            this.clearLogButton.Location = new Point(764, 0);
            this.clearLogButton.Name = "clearLogButton";
            this.clearLogButton.Size = new Size(40, 100);
            this.clearLogButton.Text = "Очистить"; // To be localized
            this.clearLogButton.IconSvg = "<svg viewBox=\"64 64 896 896\" focusable=\"false\" data-icon=\"delete\" width=\"1em\" height=\"1em\" fill=\"currentColor\" aria-hidden=\"true\"><path d=\"M360 184h-8c4.4 0 8-3.6 8-8v8h304v-8c0 4.4 3.6 8 8 8h-8v72h72v-80c0-35.3-28.7-64-64-64H352c-35.3 0-64 28.7-64 64v80h72v-72zm504 72H160c-17.7 0-32 14.3-32 32v32c0 4.4 3.6 8 8 8h60.4l24.7 523c1.6 34.1 29.8 61 63.9 61h454c34.2 0 62.3-26.8 63.9-61l24.7-523H888c4.4 0 8-3.6 8-8v-32c0-17.7-14.3-32-32-32zM731.3 840H292.7l-24.2-512h487l-24.2 512z\"></path></svg>"; // Placeholder
            this.clearLogButton.Type = AntdUI.TTypeMini.Default;
            this.clearLogButton.Tooltip = "Очистить лог";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(7F, 17F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(804, 481);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.progress1);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.footerPanel);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Margin = new Padding(2, 3, 2, 3);
            this.Name = "Form1";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Onllama"; // Simplified title, will be updated by LocalizationManager
            this.Load += Form1_Load;
            this.headerPanel.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.footerPanel.ResumeLayout(false);
            this.footerPanel.PerformLayout();
            this.tabControlModels.ResumeLayout(false);
            this.tabPageLocalModels.ResumeLayout(false);
            this.tabPageOnlineModels.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private AntdUI.Table table1;
        private AntdUI.Progress progress1;
        private AntdUI.Panel headerPanel;
        private AntdUI.Label appTitleLabel;
        private AntdUI.Label statusLabel;
        private AntdUI.Label cpuUsageLabel;
        private AntdUI.Label ramUsageLabel;
        private AntdUI.Label gpuUsageLabel;
        private AntdUI.Button settingsButton;
        private AntdUI.Panel mainPanel;
        private AntdUI.Panel footerPanel;
        private System.Windows.Forms.TextBox logTextBox;
        private AntdUI.Button copyLogButton;
        private AntdUI.Button clearLogButton;
        private AntdUI.Tabs tabControlModels;
        private System.Windows.Forms.TabPage tabPageLocalModels;
        private System.Windows.Forms.TabPage tabPageOnlineModels;
        private AntdUI.Select selectOnlineModelSource;
        private AntdUI.Select selectOnlineModels;
        private AntdUI.Button buttonPullOnlineModel;
        private AntdUI.Input textBoxSearchOnlineModels;
    }
}
