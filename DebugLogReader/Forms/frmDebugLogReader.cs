using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DebugLogReader
{
    public partial class frmDebugLogReader : Form
    {
        public frmDebugLogReader()
        {
            InitializeComponent();

            m_stpLogsProcessing = new Stopwatch();
            m_fileWrapper = new RealFileWrapper();
        }

        private void frmDebugLogReader_Load(object sender, EventArgs e)
        {
            m_settings = new DebugLogReaderSettings();
            m_settings.LogDirectory = @"Recorder testing\20160302\DebugLogs22";

            LoadSettings();

            String logsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), m_settings.LogDirectory);
            txtLogDirectory.Text = logsDir;

            frmFilters filters = new frmFilters();
            filters.Show();
            filters.BringToFront();
        }

        private String GetSettingsFolder()
        {
            String folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            folder = Path.Combine(folder, Application.ProductName);

            return folder;
        }

        private String GetSettingsFile()
        {
            return Path.Combine(GetSettingsFolder(), "settings.bin");
        }

        private void LoadSettings()
        {
            String folder = GetSettingsFolder();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (Directory.Exists(folder))
            {
                String settingsFile = GetSettingsFile();
                if (!File.Exists(settingsFile))
                {
                    using (FileStream fs = File.Create(settingsFile))
                    {
                        Serializer.Serialize(fs, m_settings);
                    }
                    AddMessage($"Settings created in {folder}");
                }
                else
                {

                    using (FileStream fs = File.OpenRead(settingsFile))
                    {
                        m_settings = Serializer.Deserialize<DebugLogReaderSettings>(fs);
                    }
                    AddMessage($"Settings read from {folder}");
                }
            }
            else
            {
                AddMessage($"Failed to load settings from {folder}");
            }
        }

        private void SaveSettings()
        {
            String folder = GetSettingsFolder();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (Directory.Exists(folder))
            {
                String settingsFile = GetSettingsFile();
                using (FileStream fs = File.Create(settingsFile))
                {
                    Serializer.Serialize(fs, m_settings);
                }
                AddMessage($"Settings saved in {folder}");
            }
        }

        private void CaptureSettingsChanges()
        {
            String logDirectory = txtLogDirectory.Text;

            m_settings.LogDirectory = logDirectory;
        }

        private void btnReadLogs_Click(object sender, EventArgs e)
        {
            bool checkSuccess = false;

            txtCombinedLog.Text = "";
            btnOpenCombinedLog.Enabled = false;

            // Confirm log directory exists
            if (Directory.Exists(txtLogDirectory.Text))
            {
                checkSuccess = true;
            }
            else
            {
                AddMessage($"Invalid log directory {txtLogDirectory.Text}");
            }

            if (checkSuccess)
            {
                CaptureSettingsChanges();
                SaveSettings();
                StartLogReads();
            }
        }

        private void btnCombinedLog_Click(object sernder, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtCombinedLog.Text))
            {
                String nppExe = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
                ProcessStartInfo nppInfo = new ProcessStartInfo(nppExe, txtCombinedLog.Text);
                Process npp = Process.Start(nppInfo);
            }
        }

        private void StartLogReads()
        {
            m_stpLogsProcessing.Restart();
            lstProgress.Items.Clear();
            btnReadLogs.Enabled = false;
            prgFiles.Value = 0;

            List<CameraDirectory> cameraNumbers = GetCameras(txtLogDirectory.Text);
            List<DebugLogFilter> filters = GetFilters();

            //cameraNumbers.Sort();

            if (cameraNumbers.Count > 0)
            {
                m_logs = new List<DebugLogBase>();

                foreach (CameraDirectory camera in cameraNumbers)
                {
                    DebugLogReaderArgs args = new DebugLogReaderArgs(txtLogDirectory.Text, camera);
                    args.AddFilters(filters);
                    BackgroundWorker bgReadLog = new BackgroundWorker();
                    bgReadLog.DoWork += ReadLogs_DoWork;
                    bgReadLog.RunWorkerCompleted += ReadLogs_RunWorkerCompleted;
                    bgReadLog.RunWorkerAsync(args);
                    m_readLogsInProgress++;
                }
            }

            prgFiles.Maximum = cameraNumbers.Count;
        }

        public static List<CameraDirectory> GetCameras(String parentLogDirectory)
        {
            String[] logDirectories = Directory.GetDirectories(parentLogDirectory);
            List<CameraDirectory> cameras = new List<CameraDirectory>();
            int cameraNumber = -1;

            foreach (String logDirectory in logDirectories)
            {
                String directoryOnly = Path.GetFileName(logDirectory);

                Regex r = new Regex("(?<cameraName>.+)_(?<cameraNumber>[0-9]+)$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

                if (r.IsMatch(directoryOnly))
                {
                    Match match = r.Match(directoryOnly);
                    String strCamName = match.Groups["cameraName"].Value;
                    String strCamNumber = match.Groups["cameraNumber"].Value;

                    if (Int32.TryParse(strCamNumber, out cameraNumber))
                    {
                        cameras.Add(new CameraDirectory(cameraNumber, strCamName));
                    }
                }
            }

            //cameras.Sort();

            return cameras;
        }

        List<DebugLogFilter> GetFilters()
        {
            List<DebugLogFilter> filters = new List<DebugLogFilter>();
            StringBuilder filterDescription = new StringBuilder();

            // Queue above
            if (chkQueueFilter.Checked)
            {
                if (!String.IsNullOrEmpty(txtQueueAbove.Text))
                {
                    int queueAbove = 0;
                    if (Int32.TryParse(txtQueueAbove.Text, out queueAbove))
                    {
                        DebugLogFilter filter = new DebugLogFilter("QueueCount", eFilterComparision.GreaterThan, queueAbove);
                        filterDescription.Append($"QueueCount{eFilterComparision.GreaterThan}{queueAbove}");
                        filters.Add(filter);
                    }
                }
            }

            // Cameras
            if (chkCamerSelect.Checked)
            {
                if (!String.IsNullOrEmpty(txtCameras.Text))
                {
                    List<int> cameras = frmCameraSelection.CameraCSVToList(txtCameras.Text);
                    DebugLogFilter filter = new DebugLogFilter("CameraNumber", eFilterComparision.MemberOf, cameras);
                    filterDescription.Append($"CameraNumber{eFilterComparision.MemberOf}{frmCameraSelection.CameraListToCSV(cameras)}");
                    filters.Add(filter);
                }
            }

            // Start at same time
            if (chkStartAtSameTime.Checked)
            {
                // We cannot create the filter as we need to have the times for all the logs first
                // we do this just before the sort
                filterDescription.Append("_StartAtSameTime");
            }

            // Start time
            if (chkStartTime.Checked)
            {
                if (!String.IsNullOrEmpty(txtStartTime.Text))
                {
                    if ((bool)txtStartTime.Tag)
                    {
                        DateTime startTime = DateTime.ParseExact(txtStartTime.Text, @"dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        DebugLogFilter filter = new DebugLogFilter("Timestamp", eFilterComparision.GreaterThan, startTime);
                        filterDescription.Append($"Timestamp{eFilterComparision.GreaterThan}{startTime.ToString("HHmmss")}");
                        filters.Add(filter);
                    }
                }
            }

            // End time
            if (chkEndTime.Checked)
            {
                if (!String.IsNullOrEmpty(txtEndTime.Text))
                {
                    if ((bool)txtEndTime.Tag)
                    {
                        DateTime endTime = DateTime.ParseExact(txtEndTime.Text, @"dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        DebugLogFilter filter = new DebugLogFilter("Timestamp", eFilterComparision.LessThan, endTime);
                        filterDescription.Append($"Timestamp{eFilterComparision.LessThan}{endTime.ToString("HHmmss")}");
                        filters.Add(filter);
                    }
                }
            }

            // Last wrote elapsed time
            if (chkLastWroteElapsedAbove.Checked)
            {
                if (!String.IsNullOrEmpty(txtLastWroteElapsedAbove.Text))
                {
                    int lastWroteElapsedAbove = 0;
                    if (Int32.TryParse(txtLastWroteElapsedAbove.Text, out lastWroteElapsedAbove))
                    {
                        TimeSpan lastWroteElapsed = new TimeSpan(0, 0, lastWroteElapsedAbove);
                        DebugLogFilter filter = new DebugLogFilter("LastWroteDataElapsed", eFilterComparision.GreaterThan, lastWroteElapsed);
                        filterDescription.Append($"LastWroteDataElapsed{eFilterComparision.GreaterThan}{lastWroteElapsed.TotalSeconds}");
                        filters.Add(filter);
                    }
                }
            }

            // Coldstore Id
            if (chkColdstoreId.Checked)
            {
                if (!String.IsNullOrEmpty(txtColdstoreId.Text))
                {
                    int coldstoreId = 0;
                    if (Int32.TryParse(txtColdstoreId.Text, out coldstoreId))
                    {
                        DebugLogFilter filter = new DebugLogFilter("ColdstoreId", eFilterComparision.EqualTo, coldstoreId);
                        filterDescription.Append($"ColdstoreId{eFilterComparision.EqualTo}{coldstoreId}");
                        filters.Add(filter);
                    }
                }
            }

            // Total frame processing time
            if (chkTotalFrameProcessing.Checked)
            {
                if (!String.IsNullOrEmpty(txtTotalFrameProcessing.Text))
                {
                    int totalFrameProcessing = 0;
                    if (Int32.TryParse(txtTotalFrameProcessing.Text, out totalFrameProcessing))
                    {
                        TimeSpan totalFrameProcessingElapsed = new TimeSpan(0, 0, totalFrameProcessing);
                        DebugLogFilter filter = new DebugLogFilter("TotalFrameProcessing", eFilterComparision.GreaterThan, totalFrameProcessingElapsed);
                        filterDescription.Append($"TotalFrameProcessing{eFilterComparision.GreaterThan}{totalFrameProcessingElapsed.TotalSeconds}");
                        filters.Add(filter);
                    }
                }
            }

            // RTSP error count changed
            if (chkRTSPErrorCountChanged.Checked)
            {
                DebugLogFilter filter = new DebugLogFilter("RTSPErrorCountChanged", eFilterComparision.EqualTo, true);
                filterDescription.Append($"RTSPErrorCountChanged{eFilterComparision.EqualTo}true");
                filters.Add(filter);
            }

            // If we have not created any filters then clear the list - we use this to determine there are no filters
            if (filters.Count == 0)
            {
                filters = null;
            }

            m_filterDescription = filterDescription.ToString();
            return filters;
        }

        private void ReadLogs_DoWork(object sender, DoWorkEventArgs e)
        {
            DebugLogReaderArgs args = (DebugLogReaderArgs)e.Argument;

            String[] logFiles = Directory.GetFiles(args.LogDirectory());
            List<DebugLogBase> logs = new List<DebugLogBase>();
            DebugLogBase newLog = null;
            foreach (String logFile in logFiles)
            {
                newLog = null;
                if (CreateDebugLog(logFile, args.Camera.CameraNumber, args.Filters, ref newLog))
                {
                    logs.Add(newLog);
                }
            }

            e.Result = new DebugLogReaderResult(args.Camera.CameraNumber, logs);
        }

        private bool CreateDebugLog(String filename, int cameraNumber, List<DebugLogFilter> filters, ref DebugLogBase log)
        {
            // Try to parse the CSWrite file
            if (!String.IsNullOrEmpty(filename))
            {
                if (filename.EndsWith("_Push.txt"))
                {
                    log = new DebugLogPush(m_fileWrapper, cameraNumber, filters);
                    log.Load(filename);
                }
                else if (filename.EndsWith("_Pop.txt"))
                {
                    log = new DebugLogPop(m_fileWrapper, cameraNumber, filters);
                    log.Load(filename);
                }
                else if (filename.EndsWith("_CSWrite.txt"))
                {
                    log = new DebugLogCS(m_fileWrapper, cameraNumber, filters);
                    log.Load(filename);
                }
                else if (filename.EndsWith("_FrameWrite.txt"))
                {
                    log = new DebugLogFrame(m_fileWrapper, cameraNumber, filters);
                    log.Load(filename);
                }
                else if (filename.EndsWith("_AviFile.txt"))
                {
                    log = new DebugLogAvi(m_fileWrapper, cameraNumber, filters);
                    log.Load(filename);
                }
            }

            return (log != null);
        }

        private void AddMessage(String text)
        {
            ListViewItem newItem = new ListViewItem(text);
            lstProgress.Items.Add(newItem);
            lstProgress.Items[lstProgress.Items.Count - 1].EnsureVisible();
        }

        private void ReadLogs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DebugLogReaderResult result = (DebugLogReaderResult)e.Result;
            bool combineLogs = false;

            AddMessage(result.ToString());
            prgFiles.Value++;

            lock (m_logs)
            {
                if (result.Logs != null)
                {
                    if (result.Logs.Count > 0)
                    {
                        m_logs.AddRange(result.Logs);
                    }
                }
                m_readLogsInProgress--;

                combineLogs = (m_readLogsInProgress == 0);
            }

            if (combineLogs)
            {
                GetColdstoreIdLogs();
                StartLogCombines();
            }
        }

        private void GetColdstoreIdLogs()
        {
            Dictionary<int, List<int>> coldstoreIdLogs = new Dictionary<int, List<int>>();

            foreach (DebugLogBase log in m_logs)
            {
                if (log is DebugLogPop)
                {
                    DebugLogPop popLog = (DebugLogPop)log;

                    foreach (int coldstoreId in popLog.ColdstoreIds)
                    {
                        if (!coldstoreIdLogs.ContainsKey(coldstoreId))
                        {
                            // Create new list
                            coldstoreIdLogs[coldstoreId] = new List<int> { popLog.CameraNumber };
                        }
                        else
                        {
                            // Add to list
                            if (!coldstoreIdLogs[coldstoreId].Contains(popLog.CameraNumber))
                            {
                                coldstoreIdLogs[coldstoreId].Add(popLog.CameraNumber);
                            }
                        }
                    }
                }
            }

            List<String> messages = new List<String>();
            foreach (KeyValuePair<int, List<int>> kvp in coldstoreIdLogs)
            {
                int coldstoreId = kvp.Key;
                List<int> cameraNumbers = kvp.Value;
                if (cameraNumbers.Count == 0)
                {
                    messages.Add($"Coldstore id {coldstoreId} used by no cameras");
                }
                else
                {
                    cameraNumbers.Sort();
                    StringBuilder cameras = new StringBuilder();
                    cameras.Append($"{cameraNumbers[0]}");
                    for (int i = 1; i < cameraNumbers.Count; i++)
                    {
                        cameras.Append($",{cameraNumbers[i]}");
                    }

                    messages.Add($"Coldstore id {coldstoreId} used by {cameraNumbers.Count} cameras {cameras.ToString()}");
                }
            }

            messages.Sort();
            foreach (String message in messages)
            {
                AddMessage(message);
            }
        }

        private void StartLogCombines()
        {
            prgFiles.Maximum = 100;
            prgFiles.Value = 0;

            BackgroundWorker bgCombineLogs = new BackgroundWorker();
            bgCombineLogs.WorkerReportsProgress = true;
            bgCombineLogs.DoWork += CombineLogs_DoWork;
            bgCombineLogs.ProgressChanged += CombineLogs_ProgressChanged;
            bgCombineLogs.RunWorkerCompleted += CombineLogs_RunWorkerCompleted;
            bgCombineLogs.RunWorkerAsync(m_logs);
        }

        private void CombineLogs_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            List<DebugLogBase> logs = (List<DebugLogBase>)e.Argument;
            DebugLogBase giantLog = new DebugLogBase(m_fileWrapper);
            int progressCount = 0;
            int progressFinish = logs.Count + 2; // One for the sort and one for the saving (done in another background worker)
            DateTime latestStartTime = DateTime.MinValue;

            // Sort the logs before we combine them as they will be in a different order each time
            // This may affect the results of rows with the same time (easier to compare correct results after changes)
            m_logs.Sort(delegate (DebugLogBase log1, DebugLogBase log2)
            {
                return log1.CameraNumber.CompareTo(log2.CameraNumber);
            });


            foreach (DebugLogBase log in logs)
            {
                if (latestStartTime < log.GetStartTime())
                {
                    latestStartTime = log.GetStartTime();
                }
                giantLog.AddLog(log);
                progressCount++;

                worker.ReportProgress((progressCount * 100) / progressFinish, log);
            }

            if (chkStartAtSameTime.Checked)
            {
                giantLog.Filter(new DebugLogFilter("Timestamp", eFilterComparision.GreaterThan, latestStartTime));
            }
            giantLog.Sort();
            progressCount++;

            worker.ReportProgress((progressCount * 100) / progressFinish, null);
            e.Result = giantLog;
        }

        private void CombineLogs_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DebugLogBase debugLog = (DebugLogBase)e.UserState;
            if (debugLog != null)
            {
                AddMessage($"{debugLog} log combined");
            }
            prgFiles.Value = e.ProgressPercentage;
        }

        private void CombineLogs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AddMessage("Logs combined");
            DebugLogBase giantLog = (DebugLogBase)e.Result;
            bool saveFile = true;
            String errorMessage = "";
            String giantLogFilename = Path.Combine(txtLogDirectory.Text, $"giantLog{m_filterDescription}.txt");

            // Don't count the saving as part of the processing time
            m_stpLogsProcessing.Stop();

            if (giantLog.Count == 0)
            {
                saveFile = false;
                errorMessage = "Empty file";
            }
            else
            {
                if (File.Exists(giantLogFilename))
                {
                    if (MessageBox.Show($"{giantLogFilename} exists\r\nYou want to overwrite it?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Delete(giantLogFilename);
                    }
                    else
                    {
                        saveFile = false;
                        errorMessage = "existing file not overwritten";
                    }
                }
            }

            if (saveFile)
            {
                Tuple<DebugLogBase, String> writeLogArgs = new Tuple<DebugLogBase, String>(giantLog, giantLogFilename);

                BackgroundWorker bgWriteLog = new BackgroundWorker();
                bgWriteLog.WorkerReportsProgress = true;
                bgWriteLog.DoWork += WriteLogs_DoWork;
                bgWriteLog.ProgressChanged += WriteLogs_ProgressChanged;
                bgWriteLog.RunWorkerCompleted += WriteLogs_RunWorkerCompleted;
                bgWriteLog.RunWorkerAsync(writeLogArgs);
            }
            else
            {
                LogsComplete(false, "", errorMessage);
            }
        }

        private void WriteLogs_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            Tuple<DebugLogBase, String> args = (Tuple<DebugLogBase, String>)e.Argument;
            DebugLogBase debugLog = args.Item1;
            String logFilename = args.Item2;
            bool fileWritten = false;
            Stopwatch saveStopwatch = new Stopwatch();
            saveStopwatch.Start();
            try
            {
                debugLog.Save(logFilename);
                saveStopwatch.Stop();
                fileWritten = true;
            }
            catch
            {
                saveStopwatch.Stop();
            }
            worker.ReportProgress(100);

            e.Result = new Tuple<bool, String, TimeSpan>(fileWritten, logFilename, saveStopwatch.Elapsed);
        }

        private void WriteLogs_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            prgFiles.Value = e.ProgressPercentage;
        }

        private void WriteLogs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Tuple<bool, String, TimeSpan> args = (Tuple<bool, String, TimeSpan>)e.Result;
            LogsComplete(args.Item1, args.Item2, $" {args.Item3.TotalSeconds.ToString("f3")} seconds");
        }

        private void LogsComplete(bool fileWritten, String logFilename, String saveInformation)
        {
            prgFiles.Value = 100;
            if (fileWritten)
            {
                AddMessage($"File created {logFilename} {saveInformation}");
            }
            else
            {
                AddMessage($"Failed to create log {saveInformation}");
            }
            AddMessage($"Total time: {m_stpLogsProcessing.Elapsed.TotalSeconds.ToString("f3")} seconds");

            txtCombinedLog.Text = logFilename;
            btnReadLogs.Enabled = true;
            btnOpenCombinedLog.Enabled = !String.IsNullOrEmpty(logFilename);

        }
        private void txtCameras_MouseClick(object sender, MouseEventArgs e)
        {
            frmCameraSelection frmCam = new frmCameraSelection(txtLogDirectory.Text, txtCameras.Text);
            if (frmCam.ShowDialog() == DialogResult.OK)
            {
                chkCamerSelect.Checked = !String.IsNullOrEmpty(frmCam.SelectedCameraCSV);
                txtCameras.Text = frmCam.SelectedCameraCSV;
            }
        }

        private void txtStartTime_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            CheckTimeFormat(txt, chkStartTime);
        }

        private void txtEndTime_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            CheckTimeFormat(txt, chkEndTime);
        }

        private void CheckTimeFormat(TextBox txt, CheckBox chk)
        {
            bool formatOk = false;

            try
            {
                DateTime startTime = DateTime.ParseExact(txt.Text, @"dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                formatOk = true;
            }
            catch
            {
                formatOk = false;
            }

            // If we want to support just the time use this
            //if (!formatOk)
            //{
            //    try
            //    {
            //        DateTime startTime = DateTime.ParseExact(txt.Text, @"HH:mm:ss", CultureInfo.InvariantCulture);
            //        formatOk = true;
            //    }
            //    catch
            //    {
            //        formatOk = false;
            //    }
            //}

            if (formatOk)
            {
                txt.Tag = true;
                txt.ForeColor = Color.Black;
                chk.Enabled = true;
            }
            else
            {
                txt.ForeColor = Color.Red;
                chk.Checked = false;
                chk.Enabled = false;
            }
        }

        int m_readLogsInProgress;
        List<DebugLogBase> m_logs;
        String m_filterDescription;
        Stopwatch m_stpLogsProcessing;

        IFileWrapper m_fileWrapper;

        DebugLogReaderSettings m_settings;
    }
}
