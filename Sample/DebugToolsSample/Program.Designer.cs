namespace DD.Sample.DebugToolsSample {
    partial class DebugToolsSampleProgram {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugToolsSampleProgram));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.終了ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.デバッグToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.デバッグツールの起動ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 16;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem,
            this.デバッグToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.終了ToolStripMenuItem});
            this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            this.ファイルToolStripMenuItem.Size = new System.Drawing.Size(68, 22);
            this.ファイルToolStripMenuItem.Text = "ファイル";
            // 
            // 終了ToolStripMenuItem
            // 
            this.終了ToolStripMenuItem.Name = "終了ToolStripMenuItem";
            this.終了ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.終了ToolStripMenuItem.Text = "終了";
            // 
            // デバッグToolStripMenuItem
            // 
            this.デバッグToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.デバッグツールの起動ToolStripMenuItem});
            this.デバッグToolStripMenuItem.Name = "デバッグToolStripMenuItem";
            this.デバッグToolStripMenuItem.Size = new System.Drawing.Size(68, 22);
            this.デバッグToolStripMenuItem.Text = "デバッグ";
            // 
            // デバッグツールの起動ToolStripMenuItem
            // 
            this.デバッグツールの起動ToolStripMenuItem.Name = "デバッグツールの起動ToolStripMenuItem";
            this.デバッグツールの起動ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.デバッグツールの起動ToolStripMenuItem.Text = "デバッグツールの起動";
            this.デバッグツールの起動ToolStripMenuItem.Click += new System.EventHandler(this.デバッグツールの起動ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 651);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 26);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "デバッグ ツールの起動";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 600);
            this.panel1.TabIndex = 3;
            // 
            // DebugToolsSampleProgram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 673);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DebugToolsSampleProgram";
            this.Text = "Debug Tools Sample";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Program_FormClosing);
            this.Load += new System.EventHandler(this.Program_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 終了ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem デバッグToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem デバッグツールの起動ToolStripMenuItem;

    }
}