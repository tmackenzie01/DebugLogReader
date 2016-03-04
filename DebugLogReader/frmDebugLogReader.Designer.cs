namespace DebugLogReader
{
    partial class frmDebugLogReader
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
            this.btnReadLogs = new System.Windows.Forms.Button();
            this.lstProgress = new System.Windows.Forms.ListView();
            this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtLogDirectory = new System.Windows.Forms.TextBox();
            this.prgFiles = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnReadLogs
            // 
            this.btnReadLogs.Location = new System.Drawing.Point(3, 4);
            this.btnReadLogs.Name = "btnReadLogs";
            this.btnReadLogs.Size = new System.Drawing.Size(75, 23);
            this.btnReadLogs.TabIndex = 0;
            this.btnReadLogs.Text = "Read logs";
            this.btnReadLogs.UseVisualStyleBackColor = true;
            this.btnReadLogs.Click += new System.EventHandler(this.btnReadLogs_Click);
            // 
            // lstProgress
            // 
            this.lstProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstProgress.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colMessage});
            this.lstProgress.Location = new System.Drawing.Point(3, 33);
            this.lstProgress.Name = "lstProgress";
            this.lstProgress.Size = new System.Drawing.Size(551, 353);
            this.lstProgress.TabIndex = 1;
            this.lstProgress.UseCompatibleStateImageBehavior = false;
            this.lstProgress.View = System.Windows.Forms.View.Details;
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            this.colMessage.Width = 546;
            // 
            // txtLogDirectory
            // 
            this.txtLogDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogDirectory.Location = new System.Drawing.Point(84, 7);
            this.txtLogDirectory.Name = "txtLogDirectory";
            this.txtLogDirectory.Size = new System.Drawing.Size(470, 20);
            this.txtLogDirectory.TabIndex = 2;
            // 
            // prgFiles
            // 
            this.prgFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgFiles.Location = new System.Drawing.Point(3, 392);
            this.prgFiles.Name = "prgFiles";
            this.prgFiles.Size = new System.Drawing.Size(551, 23);
            this.prgFiles.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgFiles.TabIndex = 3;
            // 
            // frmDebugLogReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 418);
            this.Controls.Add(this.prgFiles);
            this.Controls.Add(this.txtLogDirectory);
            this.Controls.Add(this.lstProgress);
            this.Controls.Add(this.btnReadLogs);
            this.Name = "frmDebugLogReader";
            this.Text = "Debug Log Reader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReadLogs;
        private System.Windows.Forms.ListView lstProgress;
        private System.Windows.Forms.TextBox txtLogDirectory;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.ProgressBar prgFiles;
    }
}

