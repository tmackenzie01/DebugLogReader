﻿namespace DebugLogReader
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
            this.txtEndTime = new System.Windows.Forms.TextBox();
            this.txtStartTime = new System.Windows.Forms.TextBox();
            this.txtCameras = new System.Windows.Forms.TextBox();
            this.txtTotalFrameProcessing = new System.Windows.Forms.TextBox();
            this.txtLastWroteElapsedAbove = new System.Windows.Forms.TextBox();
            this.txtColdstoreId = new System.Windows.Forms.TextBox();
            this.txtQueueAbove = new System.Windows.Forms.TextBox();
            this.chkEndTime = new System.Windows.Forms.CheckBox();
            this.chkStartTime = new System.Windows.Forms.CheckBox();
            this.chkCamerSelect = new System.Windows.Forms.CheckBox();
            this.chkRTSPErrorCountChanged = new System.Windows.Forms.CheckBox();
            this.chkTotalFrameProcessing = new System.Windows.Forms.CheckBox();
            this.chkLastWroteElapsedAbove = new System.Windows.Forms.CheckBox();
            this.chkColdstoreId = new System.Windows.Forms.CheckBox();
            this.lblLogsDirectory = new System.Windows.Forms.Label();
            this.btnOpenCombinedLog = new System.Windows.Forms.Button();
            this.lblCombinedLog = new System.Windows.Forms.Label();
            this.txtCombinedLog = new System.Windows.Forms.TextBox();
            this.grpFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReadLogs
            // 
            this.btnReadLogs.Location = new System.Drawing.Point(560, 39);
            this.btnReadLogs.Name = "btnReadLogs";
            this.btnReadLogs.Size = new System.Drawing.Size(75, 23);
            this.btnReadLogs.TabIndex = 17;
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
            this.lstProgress.Location = new System.Drawing.Point(3, 159);
            this.lstProgress.Name = "lstProgress";
            this.lstProgress.Size = new System.Drawing.Size(885, 315);
            this.lstProgress.TabIndex = 18;
            this.lstProgress.UseCompatibleStateImageBehavior = false;
            this.lstProgress.View = System.Windows.Forms.View.Details;
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            this.colMessage.Width = 878;
            // 
            // txtLogDirectory
            // 
            this.txtLogDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogDirectory.Location = new System.Drawing.Point(84, 7);
            this.txtLogDirectory.Name = "txtLogDirectory";
            this.txtLogDirectory.Size = new System.Drawing.Size(804, 20);
            this.txtLogDirectory.TabIndex = 0;
            // 
            // prgFiles
            // 
            this.prgFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgFiles.Location = new System.Drawing.Point(3, 506);
            this.prgFiles.Name = "prgFiles";
            this.prgFiles.Size = new System.Drawing.Size(885, 23);
            this.prgFiles.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgFiles.TabIndex = 3;
            // 
            // chkQueueFilter
            // 
            this.chkQueueFilter.AutoSize = true;
            this.chkQueueFilter.Location = new System.Drawing.Point(9, 19);
            this.chkQueueFilter.Name = "chkQueueFilter";
            this.chkQueueFilter.Size = new System.Drawing.Size(91, 17);
            this.chkQueueFilter.TabIndex = 1;
            this.chkQueueFilter.Text = "Queue above";
            this.chkQueueFilter.UseVisualStyleBackColor = true;
            // 
            // grpFilters
            // 
            this.grpFilters.Controls.Add(this.chkStartAtSameTime);
            this.grpFilters.Controls.Add(this.txtEndTime);
            this.grpFilters.Controls.Add(this.txtStartTime);
            this.grpFilters.Controls.Add(this.txtCameras);
            this.grpFilters.Controls.Add(this.txtTotalFrameProcessing);
            this.grpFilters.Controls.Add(this.txtLastWroteElapsedAbove);
            this.grpFilters.Controls.Add(this.txtColdstoreId);
            this.grpFilters.Controls.Add(this.txtQueueAbove);
            this.grpFilters.Controls.Add(this.chkEndTime);
            this.grpFilters.Controls.Add(this.chkStartTime);
            this.grpFilters.Controls.Add(this.chkCamerSelect);
            this.grpFilters.Controls.Add(this.chkRTSPErrorCountChanged);
            this.grpFilters.Controls.Add(this.chkTotalFrameProcessing);
            this.grpFilters.Controls.Add(this.chkLastWroteElapsedAbove);
            this.grpFilters.Controls.Add(this.chkColdstoreId);
            this.grpFilters.Controls.Add(this.chkQueueFilter);
            this.grpFilters.Location = new System.Drawing.Point(3, 33);
            this.grpFilters.Name = "grpFilters";
            this.grpFilters.Size = new System.Drawing.Size(551, 120);
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
            this.chkStartAtSameTime.TabIndex = 5;
            this.chkStartAtSameTime.Text = "Start at same time";
            this.chkStartAtSameTime.UseVisualStyleBackColor = true;
            // 
            // txtEndTime
            // 
            this.txtEndTime.Location = new System.Drawing.Point(392, 40);
            this.txtEndTime.Name = "txtEndTime";
            this.txtEndTime.Size = new System.Drawing.Size(148, 20);
            this.txtEndTime.TabIndex = 9;
            this.txtEndTime.TextChanged += new System.EventHandler(this.txtEndTime_TextChanged);
            // 
            // txtStartTime
            // 
            this.txtStartTime.Location = new System.Drawing.Point(192, 40);
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.Size = new System.Drawing.Size(129, 20);
            this.txtStartTime.TabIndex = 7;
            this.txtStartTime.TextChanged += new System.EventHandler(this.txtStartTime_TextChanged);
            // 
            // txtCameras
            // 
            this.txtCameras.Location = new System.Drawing.Point(306, 16);
            this.txtCameras.Name = "txtCameras";
            this.txtCameras.Size = new System.Drawing.Size(100, 20);
            this.txtCameras.TabIndex = 4;
            this.txtCameras.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtCameras_MouseClick);
            // 
            // txtTotalFrameProcessing
            // 
            this.txtTotalFrameProcessing.Location = new System.Drawing.Point(182, 88);
            this.txtTotalFrameProcessing.Name = "txtTotalFrameProcessing";
            this.txtTotalFrameProcessing.Size = new System.Drawing.Size(100, 20);
            this.txtTotalFrameProcessing.TabIndex = 15;
            // 
            // txtLastWroteElapsedAbove
            // 
            this.txtLastWroteElapsedAbove.Location = new System.Drawing.Point(163, 65);
            this.txtLastWroteElapsedAbove.Name = "txtLastWroteElapsedAbove";
            this.txtLastWroteElapsedAbove.Size = new System.Drawing.Size(100, 20);
            this.txtLastWroteElapsedAbove.TabIndex = 11;
            // 
            // txtColdstoreId
            // 
            this.txtColdstoreId.Location = new System.Drawing.Point(440, 65);
            this.txtColdstoreId.Name = "txtColdstoreId";
            this.txtColdstoreId.Size = new System.Drawing.Size(100, 20);
            this.txtColdstoreId.TabIndex = 13;
            // 
            // txtQueueAbove
            // 
            this.txtQueueAbove.Location = new System.Drawing.Point(96, 17);
            this.txtQueueAbove.Name = "txtQueueAbove";
            this.txtQueueAbove.Size = new System.Drawing.Size(100, 20);
            this.txtQueueAbove.TabIndex = 2;
            // 
            // chkEndTime
            // 
            this.chkEndTime.AutoSize = true;
            this.chkEndTime.Location = new System.Drawing.Point(327, 42);
            this.chkEndTime.Name = "chkEndTime";
            this.chkEndTime.Size = new System.Drawing.Size(67, 17);
            this.chkEndTime.TabIndex = 8;
            this.chkEndTime.Text = "End time";
            this.chkEndTime.UseVisualStyleBackColor = true;
            // 
            // chkStartTime
            // 
            this.chkStartTime.AutoSize = true;
            this.chkStartTime.Location = new System.Drawing.Point(126, 43);
            this.chkStartTime.Name = "chkStartTime";
            this.chkStartTime.Size = new System.Drawing.Size(70, 17);
            this.chkStartTime.TabIndex = 6;
            this.chkStartTime.Text = "Start time";
            this.chkStartTime.UseVisualStyleBackColor = true;
            // 
            // chkCamerSelect
            // 
            this.chkCamerSelect.AutoSize = true;
            this.chkCamerSelect.Location = new System.Drawing.Point(239, 19);
            this.chkCamerSelect.Name = "chkCamerSelect";
            this.chkCamerSelect.Size = new System.Drawing.Size(67, 17);
            this.chkCamerSelect.TabIndex = 3;
            this.chkCamerSelect.Text = "Cameras";
            this.chkCamerSelect.UseVisualStyleBackColor = true;
            // 
            // chkRTSPErrorCountChanged
            // 
            this.chkRTSPErrorCountChanged.AutoSize = true;
            this.chkRTSPErrorCountChanged.Location = new System.Drawing.Point(353, 91);
            this.chkRTSPErrorCountChanged.Name = "chkRTSPErrorCountChanged";
            this.chkRTSPErrorCountChanged.Size = new System.Drawing.Size(154, 17);
            this.chkRTSPErrorCountChanged.TabIndex = 16;
            this.chkRTSPErrorCountChanged.Text = "RTSP error count changed";
            this.chkRTSPErrorCountChanged.UseVisualStyleBackColor = true;
            // 
            // chkTotalFrameProcessing
            // 
            this.chkTotalFrameProcessing.AutoSize = true;
            this.chkTotalFrameProcessing.Location = new System.Drawing.Point(9, 91);
            this.chkTotalFrameProcessing.Name = "chkTotalFrameProcessing";
            this.chkTotalFrameProcessing.Size = new System.Drawing.Size(166, 17);
            this.chkTotalFrameProcessing.TabIndex = 14;
            this.chkTotalFrameProcessing.Text = "Total frame processing above";
            this.chkTotalFrameProcessing.UseVisualStyleBackColor = true;
            // 
            // chkLastWroteElapsedAbove
            // 
            this.chkLastWroteElapsedAbove.AutoSize = true;
            this.chkLastWroteElapsedAbove.Location = new System.Drawing.Point(9, 68);
            this.chkLastWroteElapsedAbove.Name = "chkLastWroteElapsedAbove";
            this.chkLastWroteElapsedAbove.Size = new System.Drawing.Size(148, 17);
            this.chkLastWroteElapsedAbove.TabIndex = 10;
            this.chkLastWroteElapsedAbove.Text = "Last wrote elapsed above";
            this.chkLastWroteElapsedAbove.UseVisualStyleBackColor = true;
            // 
            // chkColdstoreId
            // 
            this.chkColdstoreId.AutoSize = true;
            this.chkColdstoreId.Location = new System.Drawing.Point(353, 67);
            this.chkColdstoreId.Name = "chkColdstoreId";
            this.chkColdstoreId.Size = new System.Drawing.Size(82, 17);
            this.chkColdstoreId.TabIndex = 12;
            this.chkColdstoreId.Text = "Coldstore Id";
            this.chkColdstoreId.UseVisualStyleBackColor = true;
            // 
            // lblLogsDirectory
            // 
            this.lblLogsDirectory.AutoSize = true;
            this.lblLogsDirectory.Location = new System.Drawing.Point(9, 10);
            this.lblLogsDirectory.Name = "lblLogsDirectory";
            this.lblLogsDirectory.Size = new System.Drawing.Size(73, 13);
            this.lblLogsDirectory.TabIndex = 6;
            this.lblLogsDirectory.Text = "Logs directory";
            // 
            // btnOpenCombinedLog
            // 
            this.btnOpenCombinedLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenCombinedLog.Enabled = false;
            this.btnOpenCombinedLog.Location = new System.Drawing.Point(813, 480);
            this.btnOpenCombinedLog.Name = "btnOpenCombinedLog";
            this.btnOpenCombinedLog.Size = new System.Drawing.Size(75, 23);
            this.btnOpenCombinedLog.TabIndex = 20;
            this.btnOpenCombinedLog.Text = "Open";
            this.btnOpenCombinedLog.UseVisualStyleBackColor = true;
            this.btnOpenCombinedLog.Click += new System.EventHandler(this.btnCombinedLog_Click);
            // 
            // lblCombinedLog
            // 
            this.lblCombinedLog.AutoSize = true;
            this.lblCombinedLog.Location = new System.Drawing.Point(9, 483);
            this.lblCombinedLog.Name = "lblCombinedLog";
            this.lblCombinedLog.Size = new System.Drawing.Size(71, 13);
            this.lblCombinedLog.TabIndex = 6;
            this.lblCombinedLog.Text = "Combined log";
            // 
            // txtCombinedLog
            // 
            this.txtCombinedLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCombinedLog.Location = new System.Drawing.Point(84, 480);
            this.txtCombinedLog.Name = "txtCombinedLog";
            this.txtCombinedLog.Size = new System.Drawing.Size(723, 20);
            this.txtCombinedLog.TabIndex = 19;
            // 
            // frmDebugLogReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 532);
            this.Controls.Add(this.lblCombinedLog);
            this.Controls.Add(this.lblLogsDirectory);
            this.Controls.Add(this.grpFilters);
            this.Controls.Add(this.prgFiles);
            this.Controls.Add(this.txtCombinedLog);
            this.Controls.Add(this.txtLogDirectory);
            this.Controls.Add(this.lstProgress);
            this.Controls.Add(this.btnOpenCombinedLog);
            this.Controls.Add(this.btnReadLogs);
            this.Name = "frmDebugLogReader";
            this.Text = "Debug Log Reader";
            this.Load += new System.EventHandler(this.frmDebugLogReader_Load);
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
        private System.Windows.Forms.TextBox txtEndTime;
        private System.Windows.Forms.TextBox txtStartTime;
        private System.Windows.Forms.CheckBox chkEndTime;
        private System.Windows.Forms.CheckBox chkStartTime;
        private System.Windows.Forms.TextBox txtLastWroteElapsedAbove;
        private System.Windows.Forms.CheckBox chkLastWroteElapsedAbove;
        private System.Windows.Forms.Label lblLogsDirectory;
        private System.Windows.Forms.Button btnOpenCombinedLog;
        private System.Windows.Forms.Label lblCombinedLog;
        private System.Windows.Forms.TextBox txtCombinedLog;
        private System.Windows.Forms.TextBox txtColdstoreId;
        private System.Windows.Forms.CheckBox chkColdstoreId;
        private System.Windows.Forms.TextBox txtTotalFrameProcessing;
        private System.Windows.Forms.CheckBox chkTotalFrameProcessing;
        private System.Windows.Forms.CheckBox chkRTSPErrorCountChanged;
    }
}

