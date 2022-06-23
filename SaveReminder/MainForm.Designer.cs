namespace SaveReminder
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.MyHint = new System.Windows.Forms.TextBox();
            this.txtProgress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.IconClick);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IconClick);
            // 
            // MyHint
            // 
            this.MyHint.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MyHint.BackColor = System.Drawing.SystemColors.Highlight;
            this.MyHint.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MyHint.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MyHint.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.MyHint.Location = new System.Drawing.Point(0, 0);
            this.MyHint.Multiline = true;
            this.MyHint.Name = "MyHint";
            this.MyHint.ReadOnly = true;
            this.MyHint.Size = new System.Drawing.Size(586, 310);
            this.MyHint.TabIndex = 0;
            this.MyHint.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtProgress
            // 
            this.txtProgress.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProgress.Location = new System.Drawing.Point(0, 332);
            this.txtProgress.Name = "txtProgress";
            this.txtProgress.Size = new System.Drawing.Size(584, 29);
            this.txtProgress.TabIndex = 5;
            this.txtProgress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.txtProgress);
            this.Controls.Add(this.MyHint);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(434, 292);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormClosed);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        public System.Windows.Forms.TextBox MyHint;
        private System.Windows.Forms.TextBox txtProgress;
    }
}

