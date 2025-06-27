namespace Onllama.Tiny
{
    partial class FormImport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImport));
            panel1 = new AntdUI.Panel();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            inputMf = new AntdUI.Input();
            select2 = new AntdUI.Select();
            select1 = new AntdUI.Select();
            inputName = new AntdUI.Input();
            input1 = new AntdUI.Input();
            buttonSave = new AntdUI.Button();
            buttonOpen = new AntdUI.Button();
            labelQuantization = new AntdUI.Label();
            labelType = new AntdUI.Label();
            labelName = new AntdUI.Label();
            labelFile = new AntdUI.Label();
            tabPage2 = new TabPage();
            progressBarHf = new AntdUI.Progress();
            listBoxHfModels = new ListBox();
            buttonHfDownload = new AntdUI.Button();
            buttonHfSearch = new AntdUI.Button();
            inputHfModel = new AntdUI.Input();
            labelHfModel = new AntdUI.Label();
            panel1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(tabControl1);
            panel1.Location = new Point(14, 13);
            panel1.Name = "panel1";
            panel1.Shadow = 5;
            panel1.Size = new Size(559, 323);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(3, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(553, 317);
            tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(inputMf);
            tabPage1.Controls.Add(select2);
            tabPage1.Controls.Add(select1);
            tabPage1.Controls.Add(inputName);
            tabPage1.Controls.Add(input1);
            tabPage1.Controls.Add(buttonSave);
            tabPage1.Controls.Add(buttonOpen);
            tabPage1.Controls.Add(labelQuantization);
            tabPage1.Controls.Add(labelType);
            tabPage1.Controls.Add(labelName);
            tabPage1.Controls.Add(labelFile);
            tabPage1.Location = new Point(4, 28);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(545, 285);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Model File";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // inputMf
            // 
            inputMf.Multiline = true;
            inputMf.Location = new Point(119, 113);
            inputMf.Name = "inputMf";
            inputMf.Size = new Size(339, 121);
            inputMf.TabIndex = 10;
            // 
            // select2
            // 
            select2.Items.AddRange(new object[] { "Q2_K", "Q3_K_L", "Q3_K_M", "Q3_K_S", "Q4_0", "Q4_K_M", "Q4_K_S", "Q5_0", "Q5_K_M", "Q5_K_S", "Q6_K", "Q8_0", "Q8_K" });
            select2.Location = new Point(119, 77);
            select2.Name = "select2";
            select2.Size = new Size(133, 30);
            select2.TabIndex = 9;
            // 
            // select1
            // 
            select1.Items.AddRange(new object[] { "none", "mistral", "llama2", "gemma", "qwen1.5", "qwen2", "baichuan", "yi", "yi-1.5", "phi", "deepseek", "deepseek-v2" });
            select1.Location = new Point(119, 41);
            select1.Name = "select1";
            select1.Size = new Size(133, 30);
            select1.TabIndex = 8;
            select1.SelectedValueChanged += select1_SelectedValueChanged;
            // 
            // inputName
            // 
            inputName.JoinRight = true;
            inputName.Location = new Point(119, 7);
            inputName.Margin = new Padding(6);
            inputName.Name = "inputName";
            inputName.Size = new Size(339, 28);
            inputName.TabIndex = 7;
            // 
            // input1
            // 
            input1.JoinRight = true;
            input1.Location = new Point(119, 240);
            input1.Margin = new Padding(6);
            input1.Name = "input1";
            input1.Size = new Size(339, 28);
            input1.TabIndex = 0;
            // 
            // buttonSave
            // 
            buttonSave.BackColor = Color.Transparent;
            buttonSave.BorderWidth = 1F;
            buttonSave.Location = new Point(464, 240);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 28);
            buttonSave.TabIndex = 6;
            buttonSave.Text = "导入";
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonOpen
            // 
            buttonOpen.BackColor = Color.Transparent;
            buttonOpen.BorderWidth = 1F;
            buttonOpen.Location = new Point(464, 6);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(75, 28);
            buttonOpen.TabIndex = 6;
            buttonOpen.Text = "更改";
            buttonOpen.Click += buttonOpen_Click;
            // 
            // labelQuantization
            // 
            labelQuantization.AutoSize = true;
            labelQuantization.BackColor = Color.Transparent;
            labelQuantization.Location = new Point(7, 77);
            labelQuantization.Name = "labelQuantization";
            labelQuantization.Size = new Size(44, 19);
            labelQuantization.TabIndex = 3;
            labelQuantization.Text = "量化:";
            // 
            // labelType
            // 
            labelType.AutoSize = true;
            labelType.BackColor = Color.Transparent;
            labelType.Location = new Point(7, 41);
            labelType.Name = "labelType";
            labelType.Size = new Size(74, 19);
            labelType.TabIndex = 2;
            labelType.Text = "模型类型:";
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.BackColor = Color.Transparent;
            labelName.Location = new Point(7, 7);
            labelName.Name = "labelName";
            labelName.Size = new Size(74, 19);
            labelName.TabIndex = 1;
            labelName.Text = "模型名称:";
            // 
            // labelFile
            // 
            labelFile.AutoSize = true;
            labelFile.BackColor = Color.Transparent;
            labelFile.Location = new Point(7, 241);
            labelFile.Name = "labelFile";
            labelFile.Size = new Size(74, 19);
            labelFile.TabIndex = 1;
            labelFile.Text = "模型文件:";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(progressBarHf);
            tabPage2.Controls.Add(listBoxHfModels);
            tabPage2.Controls.Add(buttonHfDownload);
            tabPage2.Controls.Add(buttonHfSearch);
            tabPage2.Controls.Add(inputHfModel);
            tabPage2.Controls.Add(labelHfModel);
            tabPage2.Location = new Point(4, 28);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(545, 285);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "HuggingFace";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // progressBarHf
            // 
            progressBarHf.Location = new Point(6, 250);
            progressBarHf.Name = "progressBarHf";
            progressBarHf.Size = new Size(533, 29);
            progressBarHf.TabIndex = 5;
            progressBarHf.Value = 0F;
            progressBarHf.Visible = false;
            // 
            // listBoxHfModels
            // 
            listBoxHfModels.FormattingEnabled = true;
            listBoxHfModels.ItemHeight = 19;
            listBoxHfModels.Location = new Point(6, 50);
            listBoxHfModels.Name = "listBoxHfModels";
            listBoxHfModels.Size = new Size(533, 194);
            listBoxHfModels.TabIndex = 4;
            listBoxHfModels.SelectedIndexChanged += listBoxHfModels_SelectedIndexChanged;
            // 
            // buttonHfDownload
            // 
            buttonHfDownload.BackColor = Color.Transparent;
            buttonHfDownload.BorderWidth = 1F;
            buttonHfDownload.Enabled = false;
            buttonHfDownload.Location = new Point(464, 6);
            buttonHfDownload.Name = "buttonHfDownload";
            buttonHfDownload.Size = new Size(75, 28);
            buttonHfDownload.TabIndex = 3;
            buttonHfDownload.Text = "Download";
            buttonHfDownload.Click += buttonHfDownload_Click;
            // 
            // buttonHfSearch
            // 
            buttonHfSearch.BackColor = Color.Transparent;
            buttonHfSearch.BorderWidth = 1F;
            buttonHfSearch.Location = new Point(383, 6);
            buttonHfSearch.Name = "buttonHfSearch";
            buttonHfSearch.Size = new Size(75, 28);
            buttonHfSearch.TabIndex = 2;
            buttonHfSearch.Text = "Search";
            buttonHfSearch.Click += buttonHfSearch_Click;
            // 
            // inputHfModel
            // 
            inputHfModel.JoinRight = true;
            inputHfModel.Location = new Point(120, 6);
            inputHfModel.Margin = new Padding(6);
            inputHfModel.Name = "inputHfModel";
            inputHfModel.Size = new Size(257, 28);
            inputHfModel.TabIndex = 1;
            // 
            // labelHfModel
            // 
            labelHfModel.AutoSize = true;
            labelHfModel.BackColor = Color.Transparent;
            labelHfModel.Location = new Point(6, 10);
            labelHfModel.Name = "labelHfModel";
            labelHfModel.Size = new Size(111, 19);
            labelHfModel.TabIndex = 0;
            labelHfModel.Text = "HuggingFace ID:";
            // 
            // FormImport
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(585, 348);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormImport";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "导入模型";
            Load += FormImport_Load;
            panel1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panel1;
        private AntdUI.Label labelFile;
        private AntdUI.Button buttonOpen;
        private AntdUI.Input input1;
        private AntdUI.Button buttonSave;
        private AntdUI.Label labelName;
        private AntdUI.Input inputName;
        private AntdUI.Select select1;
        private AntdUI.Select select2;
        private AntdUI.Label labelType;
        private AntdUI.Label labelQuantization;
        private AntdUI.Input inputMf;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private AntdUI.Label labelHfModel;
        private AntdUI.Input inputHfModel;
        private AntdUI.Button buttonHfSearch;
        private AntdUI.Button buttonHfDownload;
        private ListBox listBoxHfModels;
        private AntdUI.Progress progressBarHf;
    }
}