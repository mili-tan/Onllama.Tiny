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
            label4 = new AntdUI.Label();
            inputTemplate = new AntdUI.Input();
            inputParameters = new AntdUI.Input();
            label7 = new AntdUI.Label();
            label2 = new AntdUI.Label();
            badgeContext = new AntdUI.Badge();
            badgeEmbedding = new AntdUI.Badge();
            label1 = new AntdUI.Label();
            label3 = new AntdUI.Label();
            inputLicense = new AntdUI.Input();
            panel2 = new AntdUI.Panel();
            visionTag = new AntdUI.Tag();
            toolTag = new AntdUI.Tag();
            panel1 = new AntdUI.Panel();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(label4, 0, 5);
            tableLayoutPanel1.Controls.Add(inputTemplate, 0, 5);
            tableLayoutPanel1.Controls.Add(inputParameters, 1, 4);
            tableLayoutPanel1.Controls.Add(label7, 0, 4);
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(badgeContext, 1, 0);
            tableLayoutPanel1.Controls.Add(badgeEmbedding, 1, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label3, 0, 3);
            tableLayoutPanel1.Controls.Add(inputLicense, 1, 3);
            tableLayoutPanel1.Controls.Add(panel2, 1, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(15, 15);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(9);
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Size = new Size(525, 382);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            label4.Dock = DockStyle.Top;
            label4.Location = new Point(12, 301);
            label4.Name = "label4";
            label4.Size = new Size(139, 29);
            label4.TabIndex = 15;
            label4.Text = "template";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // inputTemplate
            // 
            inputTemplate.Dock = DockStyle.Fill;
            inputTemplate.Location = new Point(157, 301);
            inputTemplate.Multiline = true;
            inputTemplate.Name = "inputTemplate";
            inputTemplate.ReadOnly = true;
            inputTemplate.Size = new Size(356, 69);
            inputTemplate.TabIndex = 14;
            inputTemplate.Text = "input3";
            // 
            // inputParameters
            // 
            inputParameters.Dock = DockStyle.Fill;
            inputParameters.Location = new Point(157, 227);
            inputParameters.Multiline = true;
            inputParameters.Name = "inputParameters";
            inputParameters.ReadOnly = true;
            inputParameters.Size = new Size(356, 68);
            inputParameters.TabIndex = 13;
            inputParameters.Text = "input2";
            // 
            // label7
            // 
            label7.Dock = DockStyle.Top;
            label7.Location = new Point(12, 227);
            label7.Name = "label7";
            label7.Size = new Size(139, 29);
            label7.TabIndex = 9;
            label7.Text = "inference_params";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label2.Location = new Point(12, 30);
            label2.Name = "label2";
            label2.Size = new Size(139, 12);
            label2.TabIndex = 3;
            label2.Text = "embedding_length";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // badgeContext
            // 
            badgeContext.Location = new Point(157, 12);
            badgeContext.Name = "badgeContext";
            badgeContext.Size = new Size(217, 12);
            badgeContext.State = AntdUI.TState.Primary;
            badgeContext.TabIndex = 0;
            badgeContext.Text = "badge1";
            // 
            // badgeEmbedding
            // 
            badgeEmbedding.Location = new Point(157, 30);
            badgeEmbedding.Name = "badgeEmbedding";
            badgeEmbedding.Size = new Size(217, 12);
            badgeEmbedding.State = AntdUI.TState.Success;
            badgeEmbedding.TabIndex = 1;
            badgeEmbedding.Text = "badge2";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label1.Location = new Point(43, 12);
            label1.Name = "label1";
            label1.Size = new Size(108, 12);
            label1.TabIndex = 2;
            label1.Text = "context_length";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Top;
            label3.Location = new Point(12, 78);
            label3.Name = "label3";
            label3.Size = new Size(139, 25);
            label3.TabIndex = 4;
            label3.Text = "license";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // inputLicense
            // 
            inputLicense.Dock = DockStyle.Fill;
            inputLicense.Location = new Point(157, 78);
            inputLicense.Multiline = true;
            inputLicense.Name = "inputLicense";
            inputLicense.ReadOnly = true;
            inputLicense.Size = new Size(356, 143);
            inputLicense.TabIndex = 10;
            inputLicense.Text = "input1";
            // 
            // panel2
            // 
            panel2.Controls.Add(visionTag);
            panel2.Controls.Add(toolTag);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(157, 48);
            panel2.Name = "panel2";
            panel2.Size = new Size(356, 24);
            panel2.TabIndex = 16;
            panel2.Text = "panel2";
            // 
            // visionTag
            // 
            visionTag.Location = new Point(59, 0);
            visionTag.Name = "visionTag";
            visionTag.Size = new Size(50, 20);
            visionTag.TabIndex = 1;
            visionTag.Text = "Vision";
            visionTag.Visible = false;
            // 
            // toolTag
            // 
            toolTag.Location = new Point(3, 0);
            toolTag.Name = "toolTag";
            toolTag.Size = new Size(50, 20);
            toolTag.TabIndex = 0;
            toolTag.Text = "Tool";
            toolTag.Visible = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(15);
            panel1.Size = new Size(555, 412);
            panel1.TabIndex = 1;
            panel1.Text = "panel1";
            // 
            // FormInfo
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(555, 412);
            Controls.Add(panel1);
            Name = "FormInfo";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "model_info";
            Load += FormInfo_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel2.ResumeLayout(false);
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
        private AntdUI.Input inputLicense;
        private AntdUI.Panel panel1;
        private AntdUI.Input inputParameters;
        private AntdUI.Input inputTemplate;
        private AntdUI.Label label4;
        private AntdUI.Panel panel2;
        private AntdUI.Tag toolTag;
        private AntdUI.Tag visionTag;
    }
}