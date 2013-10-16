namespace DD.DebugTools {
    partial class DebugToolsForm {
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.終了ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ヘルプToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.バージョンToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ビューToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.シーンToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.メッセージToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ログToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.nullView1 = new DD.DebugTools.NullView();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem,
            this.ヘルプToolStripMenuItem,
            this.ビューToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(624, 26);
            this.menuStrip1.TabIndex = 1;
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
            this.終了ToolStripMenuItem.Click += new System.EventHandler(this.終了ToolStripMenuItem_Click);
            // 
            // ヘルプToolStripMenuItem
            // 
            this.ヘルプToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ヘルプToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.バージョンToolStripMenuItem});
            this.ヘルプToolStripMenuItem.Name = "ヘルプToolStripMenuItem";
            this.ヘルプToolStripMenuItem.Size = new System.Drawing.Size(56, 22);
            this.ヘルプToolStripMenuItem.Text = "ヘルプ";
            // 
            // バージョンToolStripMenuItem
            // 
            this.バージョンToolStripMenuItem.Name = "バージョンToolStripMenuItem";
            this.バージョンToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.バージョンToolStripMenuItem.Text = "バージョン";
            this.バージョンToolStripMenuItem.Click += new System.EventHandler(this.バージョンToolStripMenuItem_Click);
            // 
            // ビューToolStripMenuItem
            // 
            this.ビューToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.シーンToolStripMenuItem,
            this.メッセージToolStripMenuItem,
            this.ログToolStripMenuItem});
            this.ビューToolStripMenuItem.Name = "ビューToolStripMenuItem";
            this.ビューToolStripMenuItem.Size = new System.Drawing.Size(56, 22);
            this.ビューToolStripMenuItem.Text = "ビュー";
            // 
            // シーンToolStripMenuItem
            // 
            this.シーンToolStripMenuItem.Name = "シーンToolStripMenuItem";
            this.シーンToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.シーンToolStripMenuItem.Text = "シーン";
            this.シーンToolStripMenuItem.Click += new System.EventHandler(this.シーンToolStripMenuItem_Click);
            // 
            // メッセージToolStripMenuItem
            // 
            this.メッセージToolStripMenuItem.Name = "メッセージToolStripMenuItem";
            this.メッセージToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.メッセージToolStripMenuItem.Text = "メッセージ";
            this.メッセージToolStripMenuItem.Click += new System.EventHandler(this.メッセージToolStripMenuItem_Click);
            // 
            // ログToolStripMenuItem
            // 
            this.ログToolStripMenuItem.Name = "ログToolStripMenuItem";
            this.ログToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ログToolStripMenuItem.Text = "ログ";
            this.ログToolStripMenuItem.Click += new System.EventHandler(this.ログToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.nullView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 416);
            this.panel1.TabIndex = 2;
            // 
            // nullView1
            // 
            this.nullView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nullView1.Location = new System.Drawing.Point(0, 0);
            this.nullView1.Name = "nullView1";
            this.nullView1.Size = new System.Drawing.Size(624, 416);
            this.nullView1.TabIndex = 0;
            // 
            // DebugToolsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DebugToolsForm";
            this.Text = "DebugTools";
            this.Load += new System.EventHandler(this.DebugToolsForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 終了ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ヘルプToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem バージョンToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ビューToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem シーンToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem メッセージToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ログToolStripMenuItem;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Panel panel1;
        private NullView nullView1;
    }
}