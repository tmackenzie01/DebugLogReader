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
            this.chkQueueFilter = new System.Windows.Forms.CheckBox();
            this.grpFilters = new System.Windows.Forms.GroupBox();
            this.chkStartAtSameTime = new System.Windows.Forms.CheckBox();
            this.txtQueueAbove = new System.Windows.Forms.TextBox();
            this.chkCamerSelect = new System.Windows.Forms.CheckBox();
            this.txtCameras = new System.Windows.Forms.TextBox();
            this.grpFilters.SuspendLayout();
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
            this.lstProgress.Location = new System.Drawing.Point(3, 111);
            this.lstProgress.Name = "lstProgress";
            this.lstProgress.Size = new System.Drawing.Size(551, 275);
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
            // chkQueueFilter
            // 
            this.chkQueueFilter.AutoSize = true;
            this.chkQueueFilter.Location = new System.Drawing.Point(9, 19);
            this.chkQueueFilter.Name = "chkQueueFilter";
            this.chkQueueFilter.Size = new System.Drawing.Size(91, 17);
            this.chkQueueFilter.TabIndex = 4;
            this.chkQueueFilter.Text = "Queue above";
            this.chkQueueFilter.UseVisualStyleBackColor = true;
            // 
            // grpFilters
            // 
            this.grpFilters.Controls.Add(this.chkStartAtSameTime);
            this.grpFilters.Controls.Add(this.txtCameras);
            this.grpFilters.Controls.Add(this.txtQueueAbove);
            this.grpFilters.Controls.Add(this.chkCamerSelect);
            this.grpFilters.Controls.Add(this.chkQueueFilter);
            this.grpFilters.Location = new System.Drawing.Point(3, 33);
            this.grpFilters.Name = "grpFilters";
            this.grpFilters.Size = new System.Drawing.Size(551, 72);
            this.grpFilters.TabIndex = 5;
            this.grpFilters.TabStop = false;
            this.grpFilters.Text = "Filters";
            // 
            // chkStartAtSameTime
            // 
            this.chkStartAtSameTime.AutoSize = true;
            this.chkStartAtSameTime.Location = new System.Drawing.Point(9, 42);
            this.chkStartAtSameTime.Name = "chkStartAtSameTime";
            this.chkStartAtSameTime.Size = new System.Drawing.Size(110, 17);
            this.chkStartAtSameTime.TabIndex = 6;
            this.chkStartAtSameTime.Text = "Start at same time";
            this.chkStartAtSameTime.UseVisualStyleBackColor = true;
            // 
            // txtQueueAbove
            // 
            this.txtQueueAbove.Location = new System.Drawing.Point(96, 17);
            this.txtQueueAbove.Name = "txtQueueAbove";
            this.txtQueueAbove.Size = new System.Drawing.Size(100, 20);
            this.txtQueueAbove.TabIndex = 5;
            // 
            // chkCamerSelect
            // 
            this.chkCamerSelect.AutoSize = true;
            this.chkCamerSelect.Location = new System.Drawing.Point(239, 19);
            this.chkCamerSelect.Name = "chkCamerSelect";
            this.chkCamerSelect.Size = new System.Drawing.Size(67, 17);
            this.chkCamerSelect.TabIndex = 4;
            this.chkCamerSelect.Text = "Cameras";
            this.chkCamerSelect.UseVisualStyleBackColor = true;
            // 
            // txtCameras
            // 
            this.txtCameras.Location = new System.Drawing.Point(306, 16);
            this.txtCameras.Name = "txtCameras";
            this.txtCameras.Size = new System.Drawing.Size(100, 20);
            this.txtCameras.TabIndex = 5;
            this.txtCameras.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtCameras_MouseClick);
            // 
            // frmDebugLogReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 418);
            this.Controls.Add(this.grpFilters);
            this.Controls.Add(this.prgFiles);
            this.Controls.Add(this.txtLogDirectory);
            this.Controls.Add(this.lstProgress);
            this.Controls.Add(this.btnReadLogs);
            this.Name = "frmDebugLogReader";
            this.Text = "Debug Log Reader";
            this.grpFilters.ResumeLayout(false);
            this.grpFilters.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReadLogs;
        private System.Windows.Forms.ListView lstProgress;
        private System.Windows.Forms.TextBox txtLogDirectory;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.ProgressBar prgFiles;
        private System.Windows.Forms.CheckBox chkQueueFilter;
        private System.Windows.Forms.GroupBox grpFilters;
        private System.Windows.Forms.TextBox txtQueueAbove;
        private System.Windows.Forms.CheckBox chkStartAtSameTime;
        private System.Windows.Forms.TextBox txtCameras;
        private System.Windows.Forms.CheckBox chkCamerSelect;
    }
}

