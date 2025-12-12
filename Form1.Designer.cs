namespace RepliDonaCarne
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox textBoxResponse;
        private ContextMenuStrip contextMenuStrip1;
        private NotifyIcon notifyIcon1;
        private ToolStripMenuItem sairToolStripMenuItem;
        private Panel topPanel;
        private Button buttonEnvioInicial;

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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            textBoxResponse = new TextBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            sairToolStripMenuItem = new ToolStripMenuItem();
            notifyIcon1 = new NotifyIcon(components);
            topPanel = new Panel();
            buttonEnvioInicial = new Button();
            contextMenuStrip1.SuspendLayout();
            topPanel.SuspendLayout();
            SuspendLayout();
            // 
            // textBoxResponse
            // 
            textBoxResponse.Dock = DockStyle.Fill;
            textBoxResponse.Location = new Point(0, 38);
            textBoxResponse.Multiline = true;
            textBoxResponse.Name = "textBoxResponse";
            textBoxResponse.ScrollBars = ScrollBars.Vertical;
            textBoxResponse.Size = new Size(224, 160);
            textBoxResponse.TabIndex = 1;
            textBoxResponse.TextChanged += textBoxResponse_TextChanged;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { sairToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(94, 26);
            // 
            // sairToolStripMenuItem
            // 
            sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            sairToolStripMenuItem.Size = new Size(93, 22);
            sairToolStripMenuItem.Text = "Sair";
            sairToolStripMenuItem.Click += SairToolStripMenuItem_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Text = "Replicador Dona Carne";
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += NotifyIcon1_DoubleClick;
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.LightGray;
            topPanel.Controls.Add(buttonEnvioInicial);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(224, 38);
            topPanel.TabIndex = 2;
            // 
            // buttonEnvioInicial
            // 
            buttonEnvioInicial.Location = new Point(3, 3);
            buttonEnvioInicial.Name = "buttonEnvioInicial";
            buttonEnvioInicial.Size = new Size(120, 30);
            buttonEnvioInicial.TabIndex = 0;
            buttonEnvioInicial.Text = "Envio Inicial";
            buttonEnvioInicial.UseVisualStyleBackColor = true;
            buttonEnvioInicial.Click += ButtonEnvioInicial_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(224, 198);
            Controls.Add(textBoxResponse);
            Controls.Add(topPanel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "LSI Gestor";
            Load += Form1_Load;
            Resize += Form1_Resize;
            contextMenuStrip1.ResumeLayout(false);
            topPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        // Precisam existir no seu Form1.cs
        private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void SairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
