namespace Onllama.Tiny
{
    partial class FormRegistryInfo
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
            panel1 = new AntdUI.Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            label4 = new AntdUI.Label();
            inputTemplate = new AntdUI.Input();
            inputParameters = new AntdUI.Input();
            label7 = new AntdUI.Label();
            badgeSize = new AntdUI.Badge();
            label1 = new AntdUI.Label();
            label3 = new AntdUI.Label();
            inputLicense = new AntdUI.Input();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Shadow = 15;
            panel1.Size = new Size(584, 461);
            panel1.TabIndex = 2;
            panel1.Text = "panel1";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(label4, 0, 5);
            tableLayoutPanel1.Controls.Add(inputTemplate, 0, 5);
            tableLayoutPanel1.Controls.Add(inputParameters, 1, 4);
            tableLayoutPanel1.Controls.Add(label7, 0, 4);
            tableLayoutPanel1.Controls.Add(badgeSize, 1, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label3, 0, 3);
            tableLayoutPanel1.Controls.Add(inputLicense, 1, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(15, 15);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(10);
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Size = new Size(554, 431);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            label4.Dock = DockStyle.Top;
            label4.Location = new Point(13, 335);
            label4.Name = "label4";
            label4.Size = new Size(124, 32);
            label4.TabIndex = 15;
            label4.Text = "模板：";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // inputTemplate
            // 
            inputTemplate.Dock = DockStyle.Fill;
            inputTemplate.Location = new Point(143, 335);
            inputTemplate.Multiline = true;
            inputTemplate.Name = "inputTemplate";
            inputTemplate.ReadOnly = true;
            inputTemplate.Size = new Size(398, 83);
            inputTemplate.TabIndex = 14;
            // 
            // inputParameters
            // 
            inputParameters.Dock = DockStyle.Fill;
            inputParameters.Location = new Point(143, 248);
            inputParameters.Multiline = true;
            inputParameters.Name = "inputParameters";
            inputParameters.ReadOnly = true;
            inputParameters.Size = new Size(398, 81);
            inputParameters.TabIndex = 13;
            // 
            // label7
            // 
            label7.Dock = DockStyle.Top;
            label7.Location = new Point(13, 248);
            label7.Name = "label7";
            label7.Size = new Size(124, 32);
            label7.TabIndex = 9;
            label7.Text = "推理参数：";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // badgeSize
            // 
            badgeSize.Location = new Point(143, 13);
            badgeSize.Name = "badgeSize";
            badgeSize.Size = new Size(248, 14);
            badgeSize.State = AntdUI.TState.Primary;
            badgeSize.TabIndex = 0;
            badgeSize.Text = "Unknown";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label1.Location = new Point(14, 13);
            label1.Name = "label1";
            label1.Size = new Size(123, 14);
            label1.TabIndex = 2;
            label1.Text = "模型大小：";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Top;
            label3.Location = new Point(13, 73);
            label3.Name = "label3";
            label3.Size = new Size(124, 28);
            label3.TabIndex = 4;
            label3.Text = "许可证：";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // inputLicense
            // 
            inputLicense.Dock = DockStyle.Fill;
            inputLicense.Location = new Point(143, 73);
            inputLicense.Multiline = true;
            inputLicense.Name = "inputLicense";
            inputLicense.ReadOnly = true;
            inputLicense.Size = new Size(398, 169);
            inputLicense.TabIndex = 10;
            // 
            // FormRegistryInfo
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 461);
            Controls.Add(panel1);
            Name = "FormRegistryInfo";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "模型信息";
            Load += FormRegistryInfo_Load;
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Label label4;
        private AntdUI.Input inputTemplate;
        private AntdUI.Input inputParameters;
        private AntdUI.Label label7;
        private AntdUI.Badge badgeSize;
        private AntdUI.Label label1;
        private AntdUI.Label label3;
        private AntdUI.Input inputLicense;
    }
}