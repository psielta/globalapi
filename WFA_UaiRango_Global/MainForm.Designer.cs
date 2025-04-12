namespace WFA_UaiRango_Global
{
    partial class MainForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            notifyIcon1 = new NotifyIcon(components);
            buttonMinimizar = new Button();
            buttonFechar = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            textBox1 = new TextBox();
            bindingSource1 = new BindingSource(components);
            richTextBox1 = new RichTextBox();
            checkBox1 = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseClick += notifyIcon1_MouseClick;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // buttonMinimizar
            // 
            buttonMinimizar.Location = new Point(230, 292);
            buttonMinimizar.Name = "buttonMinimizar";
            buttonMinimizar.Size = new Size(75, 23);
            buttonMinimizar.TabIndex = 0;
            buttonMinimizar.TabStop = false;
            buttonMinimizar.Text = "Minimizar";
            buttonMinimizar.UseVisualStyleBackColor = true;
            buttonMinimizar.Click += buttonMinimizar_Click;
            // 
            // buttonFechar
            // 
            buttonFechar.Location = new Point(311, 292);
            buttonFechar.Name = "buttonFechar";
            buttonFechar.Size = new Size(75, 23);
            buttonFechar.TabIndex = 1;
            buttonFechar.TabStop = false;
            buttonFechar.Text = "Fechar";
            buttonFechar.UseVisualStyleBackColor = true;
            buttonFechar.Click += buttonFechar_Click;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.InactiveBorder;
            textBox1.Enabled = false;
            textBox1.Location = new Point(12, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(374, 23);
            textBox1.TabIndex = 2;
            textBox1.TabStop = false;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(12, 41);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(374, 245);
            richTextBox1.TabIndex = 3;
            richTextBox1.Text = "";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(12, 292);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(54, 19);
            checkBox1.TabIndex = 4;
            checkBox1.Text = "Ativo";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(398, 327);
            Controls.Add(checkBox1);
            Controls.Add(richTextBox1);
            Controls.Add(textBox1);
            Controls.Add(buttonFechar);
            Controls.Add(buttonMinimizar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Integrador UAI Rango";
            FormClosing += MainForm_FormClosing;
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private Button buttonMinimizar;
        private Button buttonFechar;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private TextBox textBox1;
        private BindingSource bindingSource1;
        private RichTextBox richTextBox1;
        private CheckBox checkBox1;
    }
}
