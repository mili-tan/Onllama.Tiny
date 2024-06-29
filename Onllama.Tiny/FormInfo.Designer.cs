namespace Onllama.Tiny
{
    partial class FormInfo
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
            tableLayoutPanel1 = new TableLayoutPanel();
            inputParameters = new AntdUI.Input();
            label7 = new AntdUI.Label();
            label2 = new AntdUI.Label();
            badgeContext = new AntdUI.Badge();
            badgeEmbedding = new AntdUI.Badge();
            label1 = new AntdUI.Label();
            label3 = new AntdUI.Label();
            inputLicense = new AntdUI.Input();
            panel1 = new AntdUI.Panel();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(inputParameters, 1, 4);
            tableLayoutPanel1.Controls.Add(label7, 0, 4);
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(badgeContext, 1, 0);
            tableLayoutPanel1.Controls.Add(badgeEmbedding, 1, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label3, 0, 3);
            tableLayoutPanel1.Controls.Add(inputLicense, 1, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(15, 15);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(10);
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(604, 431);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // inputParameters
            // 
            inputParameters.Dock = DockStyle.Fill;
            inputParameters.Location = new Point(143, 248);
            inputParameters.Multiline = true;
            inputParameters.Name = "inputParameters";
            inputParameters.ReadOnly = true;
            inputParameters.Size = new Size(448, 170);
            inputParameters.TabIndex = 13;
            inputParameters.Text = "input4";
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
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label2.Location = new Point(14, 33);
            label2.Name = "label2";
            label2.Size = new Size(123, 14);
            label2.TabIndex = 3;
            label2.Text = "Embedding 长度：";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // badgeContext
            // 
            badgeContext.Location = new Point(143, 13);
            badgeContext.Name = "badgeContext";
            badgeContext.Size = new Size(248, 14);
            badgeContext.State = AntdUI.TState.Primary;
            badgeContext.TabIndex = 0;
            badgeContext.Text = "badge1";
            // 
            // badgeEmbedding
            // 
            badgeEmbedding.Location = new Point(143, 33);
            badgeEmbedding.Name = "badgeEmbedding";
            badgeEmbedding.Size = new Size(248, 14);
            badgeEmbedding.State = AntdUI.TState.Success;
            badgeEmbedding.TabIndex = 1;
            badgeEmbedding.Text = "badge2";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label1.Location = new Point(14, 13);
            label1.Name = "label1";
            label1.Size = new Size(123, 14);
            label1.TabIndex = 2;
            label1.Text = "上下文长度：";
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
            inputLicense.Size = new Size(448, 169);
            inputLicense.TabIndex = 10;
            inputLicense.Text = "input1";
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Shadow = 15;
            panel1.Size = new Size(634, 461);
            panel1.TabIndex = 1;
            panel1.Text = "panel1";
            // 
            // FormInfo
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(634, 461);
            Controls.Add(panel1);
            Name = "FormInfo";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "模型信息";
            Load += FormInfo_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Label label2;
        private AntdUI.Badge badgeContext;
        private AntdUI.Badge badgeEmbedding;
        private AntdUI.Label label1;
        private AntdUI.Label label7;
        private AntdUI.Label label3;
        private AntdUI.Input inputParameters;
        private AntdUI.Input inputLicense;
        private AntdUI.Panel panel1;
    }
}