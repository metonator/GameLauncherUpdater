namespace GameLauncherUpdater
{
    partial class Updater
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Updater));
            this.DownloadProgress = new System.Windows.Forms.ProgressBar();
            this.Information = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // DownloadProgress
            // 
            this.DownloadProgress.Location = new System.Drawing.Point(12, 41);
            this.DownloadProgress.Name = "DownloadProgress";
            this.DownloadProgress.Size = new System.Drawing.Size(331, 10);
            this.DownloadProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.DownloadProgress.TabIndex = 0;
            // 
            // Information
            // 
            this.Information.AutoEllipsis = true;
            this.Information.BackColor = System.Drawing.Color.Transparent;
            this.Information.ForeColor = System.Drawing.Color.Snow;
            this.Information.Location = new System.Drawing.Point(12, 1);
            this.Information.Name = "Information";
            this.Information.Size = new System.Drawing.Size(331, 37);
            this.Information.TabIndex = 1;
            this.Information.Text = "Checking for latest update";
            this.Information.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Updater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::GameLauncherUpdater.Properties.Resources.Background;
            this.ClientSize = new System.Drawing.Size(355, 63);
            this.Controls.Add(this.Information);
            this.Controls.Add(this.DownloadProgress);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Updater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar DownloadProgress;
        private System.Windows.Forms.Label Information;
    }
}

