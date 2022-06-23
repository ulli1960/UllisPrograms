namespace WinDirSize
{
    partial class WinDirSize
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinDirSize));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsLabelDirectories = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLabelThreads = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLabeDir = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLabelError = new System.Windows.Forms.ToolStripStatusLabel();
            this.lvResult = new System.Windows.Forms.ListView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnStart = new System.Windows.Forms.ToolStripButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Arial", 12F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLabelDirectories,
            this.tsLabelThreads,
            this.tsLabeDir,
            this.tsLabelError});
            this.statusStrip1.Location = new System.Drawing.Point(0, 597);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1192, 23);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsLabelDirectories
            // 
            this.tsLabelDirectories.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsLabelDirectories.Name = "tsLabelDirectories";
            this.tsLabelDirectories.Size = new System.Drawing.Size(136, 18);
            this.tsLabelDirectories.Text = "tsLabelDirectories";
            // 
            // tsLabelThreads
            // 
            this.tsLabelThreads.Font = new System.Drawing.Font("Arial", 12F);
            this.tsLabelThreads.Name = "tsLabelThreads";
            this.tsLabelThreads.Size = new System.Drawing.Size(116, 18);
            this.tsLabelThreads.Text = "tsLabelThreads";
            // 
            // tsLabeDir
            // 
            this.tsLabeDir.Name = "tsLabeDir";
            this.tsLabeDir.Size = new System.Drawing.Size(77, 18);
            this.tsLabeDir.Text = "tsLabeDir";
            // 
            // tsLabelError
            // 
            this.tsLabelError.Name = "tsLabelError";
            this.tsLabelError.Size = new System.Drawing.Size(848, 18);
            this.tsLabelError.Spring = true;
            this.tsLabelError.Text = "tsLabelError";
            // 
            // lvResult
            // 
            this.lvResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lvResult.Location = new System.Drawing.Point(0, 32);
            this.lvResult.Name = "lvResult";
            this.lvResult.Size = new System.Drawing.Size(1192, 559);
            this.lvResult.TabIndex = 3;
            this.lvResult.UseCompatibleStateImageBehavior = false;
            this.lvResult.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.lvResult_ItemMouseHover);
            this.lvResult.Click += new System.EventHandler(this.lvResult_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStart});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1192, 31);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnStart
            // 
            this.btnStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnStart.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(65, 28);
            this.btnStart.Text = "&Start!";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 620);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lvResult);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsLabelDirectories;
        private System.Windows.Forms.ToolStripStatusLabel tsLabelThreads;
        private System.Windows.Forms.ListView lvResult;
        private System.Windows.Forms.ToolStripStatusLabel tsLabeDir;
        private System.Windows.Forms.ToolStripStatusLabel tsLabelError;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnStart;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

