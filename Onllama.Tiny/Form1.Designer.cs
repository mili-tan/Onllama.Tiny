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
            table1 = new AntdUI.Table();
            panel1 = new AntdUI.Panel();
            flowLayoutPanel1 = new AntdUI.In.FlowLayoutPanel();
            select1 = new AntdUI.Select();
            button1 = new AntdUI.Button();
            dropdown1 = new AntdUI.Dropdown();
            progress1 = new AntdUI.Progress();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // table1
            // 
            table1.Dock = DockStyle.Fill;
            table1.Location = new Point(0, 0);
            table1.Name = "table1";
            table1.Size = new Size(684, 381);
            table1.TabIndex = 0;
            table1.Text = "table1";
            table1.CellButtonClick += table1_CellButtonClick;
            // 
            // panel1
            // 
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 336);
            panel1.Name = "panel1";
            panel1.Shadow = 5;
            panel1.Size = new Size(684, 45);
            panel1.TabIndex = 1;
            panel1.Text = "panel1";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.BackColor = Color.Transparent;
            flowLayoutPanel1.Controls.Add(select1);
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Controls.Add(dropdown1);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(5, 5);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(674, 35);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.SizeChanged += flowLayoutPanel1_SizeChanged;
            // 
            // select1
            // 
            select1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            select1.Items.AddRange(new object[] { "qwen:4b", "qwen:7b", "qwen:14b", "qwen:32b", "yi:6b", "yi:34b", "llama3:8b", "phi3:mini", "command-r:35b", "gemma:7b", "deepseek-llm:7b", "llama2-chinese:7b", "llama2-chinese:13b" });
            select1.Location = new Point(3, 3);
            select1.Name = "select1";
            select1.SelectedIndex = 0;
            select1.SelectedValue = "qwen:4b";
            select1.Size = new Size(560, 30);
            select1.TabIndex = 0;
            select1.Text = "qwen:4b";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            button1.BorderWidth = 1F;
            button1.Ghost = true;
            button1.Location = new Point(569, 3);
            button1.Name = "button1";
            button1.Size = new Size(65, 30);
            button1.TabIndex = 1;
            button1.Text = "下载";
            button1.Click += button1_Click;
            // 
            // dropdown1
            // 
            dropdown1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            dropdown1.BorderWidth = 1F;
            dropdown1.Ghost = true;
            dropdown1.ImageSvg = Properties.Resources.settingSvg;
            dropdown1.Items.AddRange(new object[] { "导入模型", "Ollama 设置", "刷新模型列表" });
            dropdown1.Location = new Point(640, 3);
            dropdown1.Name = "dropdown1";
            dropdown1.Placement = AntdUI.TAlignFrom.TR;
            dropdown1.Size = new Size(30, 30);
            dropdown1.TabIndex = 2;
            dropdown1.SelectedValueChanged += dropdown1_SelectedValueChanged;
            // 
            // progress1
            // 
            progress1.Dock = DockStyle.Top;
            progress1.Location = new Point(0, 0);
            progress1.Name = "progress1";
            progress1.Radius = 0;
            progress1.Size = new Size(684, 2);
            progress1.TabIndex = 2;
            progress1.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(684, 381);
            Controls.Add(progress1);
            Controls.Add(panel1);
            Controls.Add(table1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2, 3, 2, 3);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Onllama - Models";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Table table1;
        private AntdUI.Panel panel1;
        private AntdUI.Select select1;
        private AntdUI.Button button1;
        private AntdUI.Progress progress1;
        private AntdUI.Dropdown dropdown1;
        private AntdUI.In.FlowLayoutPanel flowLayoutPanel1;
    }
}
