using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DebugLogReader
{
    public partial class frmDebugLogReader : Form
    {
        public frmDebugLogReader()
        {
            InitializeComponent();

            String logsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                @"Recorder testing\20160302\DebugLogs16");

            txtLogDirectory.Text = logsDir;
            m_stpLogsProcessing = new Stopwatch();
        }

        private void btnReadLogs_Click(object sender, EventArgs e)
        {
            txtCombinedLog.Text = "";
            btnOpenCombinedLog.Enabled = false;
            StartLogReads();
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

            List<int> cameraNumbers = GetCameraNumbers(txtLogDirectory.Text);
            List<DebugLogFilter> filters = GetFilters();

            cameraNumbers.Sort();

            if (cameraNumbers.Count > 0)
            {
                m_logs = new List<DebugLog>();

                foreach (int cameraNumber in cameraNumbers)
                {
                    DebugLogReaderArgs args = new DebugLogReaderArgs(txtLogDirectory.Text, cameraNumber);
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

        public static List<int> GetCameraNumbers(String parentLogDirectory)
        {
            String[] logDirectories = Directory.GetDirectories(parentLogDirectory);
            List<int> cameraNumbers = new List<int>();
            int cameraNumber = -1;

            foreach (String logDirectory in logDirectories)
            {
                Regex r = new Regex("Cam.(?<cameraName>[0-9]+)_(?<cameraNumber>[0-9]+)$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

                if (r.IsMatch(logDirectory))
                {
                    Match match = r.Match(logDirectory);
                    String strCamName = match.Groups["cameraName"].Value;
                    String strCamNumber = match.Groups["cameraNumber"].Value;

                    if (strCamName.Equals(strCamNumber))
                    {
                        if (Int32.TryParse(strCamNumber, out cameraNumber))
                        {
                            cameraNumbers.Add(cameraNumber);
                        }
                    }
                }
            }

            cameraNumbers.Sort();

            return cameraNumbers;
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
                        DebugLogFilter filter = new DebugLogFilter(eFilterBy.QueueCount, eFilterComparision.GreaterThan, queueAbove);
                        filterDescription.Append(filter.ToString());
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
                    DebugLogFilter filter = new DebugLogFilter(eFilterBy.CameraNumber, eFilterComparision.MemberOf, cameras);
                    filterDescription.Append(filter.ToString());
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
                        DebugLogFilter filter = new DebugLogFilter(eFilterBy.StartTime, eFilterComparision.GreaterThan, startTime);
                        filterDescription.Append(filter.ToString());
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
                        DebugLogFilter filter = new DebugLogFilter(eFilterBy.EndTime, eFilterComparision.LessThan, endTime);
                        filterDescription.Append(filter.ToString());
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
                        DebugLogFilter filter = new DebugLogFilter(eFilterBy.LastWroteElapsed, eFilterComparision.GreaterThan, lastWroteElapsed);
                        filterDescription.Append(filter.ToString());
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
                        DebugLogFilter filter = new DebugLogFilter(eFilterBy.ColdstoreId, eFilterComparision.EqualTo, coldstoreId);
                        filterDescription.Append(filter.ToString());
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
                        DebugLogFilter filter = new DebugLogFilter(eFilterBy.TotalFrameProcessing, eFilterComparision.GreaterThan, totalFrameProcessingElapsed);
                        filterDescription.Append(filter.ToString());
                        filters.Add(filter);
                    }
                }
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
            String pushFile = "";
            String popFile = "";
            String csFile = "";
            String frameFile = "";
            DebugLog pushLog = null;
            DebugLog popLog = null;
            DebugLog csLog = null;
            DebugLog frameLog = null;
            List<DebugLog> logs = new List<DebugLog>();
            int fileFoundCount = 0;

            foreach (String logFile in logFiles)
            {
                if (logFile.EndsWith("_Push.txt"))
                {
                    pushFile = logFile;
                    fileFoundCount++;
                }
                if (logFile.EndsWith("_Pop.txt"))
                {
                    popFile = logFile;
                    fileFoundCount++;
                }
                if (logFile.EndsWith("_CSWrite.txt"))
                {
                    csFile = logFile;
                    fileFoundCount++;
                }
                if (logFile.EndsWith("_FrameWrite.txt"))
                {
                    frameFile = logFile;
                    fileFoundCount++;
                }
            }

            // Try to parse the CSWrite file
            if (!String.IsNullOrEmpty(csFile))
            {
                csLog = new CSDebugLog(args.CameraNumber, args.Filters);
                csLog.Load(csFile);
                logs.Add(csLog);
            }

            if (!String.IsNullOrEmpty(pushFile))
            {
                pushLog = new PushDebugLog(args.CameraNumber, args.Filters);
                pushLog.Load(pushFile);
                logs.Add(pushLog);
            }

            if (!String.IsNullOrEmpty(popFile))
            {
                popLog = new PopDebugLog(args.CameraNumber, args.Filters);
                popLog.Load(popFile);
                logs.Add(popLog);
            }

            if (!String.IsNullOrEmpty(frameFile))
            {
                frameLog = new FrameDebugLog(args.CameraNumber, args.Filters);
                frameLog.Load(frameFile);
                logs.Add(frameLog);
            }

            e.Result = new DebugLogReaderResult(args.CameraNumber, logs);
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

            foreach (DebugLog log in m_logs)
            {
                if (log is PopDebugLog)
                {
                    PopDebugLog popLog = (PopDebugLog)log;

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
            List<DebugLog> logs = (List<DebugLog>)e.Argument;
            DebugLog giantLog = new DebugLog();
            int progressCount = 0;
            int progressFinish = logs.Count + 2; // One for the sort and one for the saving (done in another background worker)
            DateTime latestStartTime = DateTime.MinValue;

            // Sort the logs before we combine them as they will be in a different order each time
            // This may affect the results of rows with the same time (easier to compare correct results after changes)
            m_logs.Sort(delegate (DebugLog log1, DebugLog log2)
            {
                return log1.CameraNumber.CompareTo(log2.CameraNumber);
            });


            foreach (DebugLog log in logs)
            {
                if (latestStartTime < log.GetStartTime())
                {
                    latestStartTime = log.GetStartTime();
                }
                giantLog.AddLog(log);
                progressCount++;

                worker.ReportProgress((progressCount * 100) / progressFinish, log.CameraNumber);
            }

            if (chkStartAtSameTime.Checked)
            {
                giantLog.Filter(new DebugLogFilter(eFilterBy.StartTime, eFilterComparision.GreaterThan, latestStartTime));
            }
            giantLog.Sort();
            progressCount++;

            worker.ReportProgress((progressCount * 100) / progressFinish, -1);
            e.Result = giantLog;
        }

        private void CombineLogs_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int cameraNumber = (int)e.UserState;
            if (cameraNumber >= 0)
            {
                AddMessage($"Camera {cameraNumber} logs combined");
            }
            prgFiles.Value = e.ProgressPercentage;
        }

        private void CombineLogs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AddMessage("Logs combined");
            DebugLog giantLog = (DebugLog)e.Result;
            bool saveFile = true;
            String errorMessage = "";
            String giantLogFilename = Path.Combine(txtLogDirectory.Text, $"giantLog{m_filterDescription}.txt");

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
                Tuple<DebugLog, String> writeLogArgs = new Tuple<DebugLog, String>(giantLog, giantLogFilename);

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

            Tuple<DebugLog, String> args = (Tuple<DebugLog, String>)e.Argument;
            DebugLog debugLog = args.Item1;
            String logFilename = args.Item2;
            bool fileWritten = false;
            try
            {
                debugLog.Save(logFilename);
                fileWritten = true;
            }
            catch
            {
            }
            worker.ReportProgress(100);

            e.Result = new Tuple<bool, String>(fileWritten, logFilename);
        }

        private void WriteLogs_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            prgFiles.Value = e.ProgressPercentage;
        }

        private void WriteLogs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Tuple<bool, String> args = (Tuple<bool, String>)e.Result;
            LogsComplete(args.Item1, args.Item2, "");
        }

        private void LogsComplete(bool fileWritten, String logFilename, String errorMessage)
        {
            prgFiles.Value = 100;
            if (fileWritten)
            {
                AddMessage($"File created {logFilename}");
            }
            else
            {
                AddMessage($"Failed to create log {errorMessage}");
            }
            m_stpLogsProcessing.Stop();
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
            try
            {
                DateTime startTime = DateTime.ParseExact(txt.Text, @"dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                txt.Tag = true;
                txt.ForeColor = Color.Black;
                chk.Enabled = true;
            }
            catch
            {
                txt.ForeColor = Color.Red;
                chk.Checked = false;
                chk.Enabled = false;
            }
        }

        int m_readLogsInProgress;
        List<DebugLog> m_logs;
        String m_filterDescription;
        Stopwatch m_stpLogsProcessing;

        // Declare and intialise these Regex here as it's costly to keep creating them
        // Need to figure out a better way to do this
        public static Regex m_pushedRegex = new Regex("Pushed..." +
                "(?<timestamp>[0-9]+.[0-9]+.[0-9]+.[0-9]+.[0-9]+.[0-9]+.[0-9]+).(\\-\\-\\-...[0-9]+.[0-9]+.seconds..)*" +
                "Q.(?<queueCount>[0-9]+).F..?([0-9]+|ull)(,.(?<pushedPopped>[0-9]+),.[0-9]+)*$",
        RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

        public static Regex m_poppedRegex = new Regex("Popped..." +
            "(?<timestamp>[0-9]+.[0-9]+.[0-9]+.[0-9]+.[0-9]+.[0-9]+.[0-9]+).(\\-\\-\\-..[0-9]+.[0-9]+.seconds..)*" +
            "Q.(?<queueCount>[0-9]+).F..?((?<frameNo>[0-9]+|ull))(,.(?<pushedPopped>[0-9]+),.[0-9]+)*" +
            "(.T:A.(?<timeA>[0-9]+.[0-9]+.[0-9]+.[0-9]+).(B.(?<timeB>[0-9]+.[0-9]+.[0-9]+.[0-9]+).)*" +
            "C.(?<timeC>[0-9]+.[0-9]+.[0-9]+.[0-9]+).D.(?<timeD>[0-9]+.[0-9]+.[0-9]+.[0-9]+).)*$");

        public static Regex m_csRegex = new Regex("(?<timestamp>[0-9]+.[0-9]+.[0-9]+.[0-9]+).*[0-9]+.[0-9]+.(.)*[0-9]+.[0-9]+$",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

        public static Regex m_wroteDataRegex = new Regex("Wrote data( C.(?<coldstoreId>[0-9]+) P.(?<coldstorePort>[0-9]+))*$",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

        public static Regex m_frameRegex = new Regex("Record.(?<timestamp>[0-9]+.[0-9]+.[0-9]+.[0-9]+)." + "\\(" +
                "(C:(?<cTimestamp1>[0-9]+.[0-9]+).O:(?<oTimestamp>[0-9]+.[0-9]+).)*" +
                "(MPEG4-AA:(?<aaTimestamp>[0-9]+.[0-9]+).)*" + "(.BB:(?<bbTimestamp>[0-9]+.[0-9]+).)*" +
                "(.(CC|CR):(?<ccTimestamp>[0-9]+.[0-9]+).)*" + "(.DD:(?<ddTimestamp>[0-9]+.[0-9]+).)*" +
                "(.EE:(?<eeTimestamp>[0-9]+.[0-9]+).)*" + "(.FF:(?<ffTimestamp>[0-9]+.[0-9]+).)*" +
                "(.GG:(?<ggTimestamp>[0-9]+.[0-9]+).)*" + "(.HH:(?<hhTimestamp>([0-9]+.[0-9]+.)*[0-9]+.[0-9]+).)*" +
                "(RV|C|(?<rvException>RVE)).(?<rvORcTimestamp>[0-9]+.[0-9]+)." + "\\)" + ".TOT.(?<totTimestamp>[0-9]+.[0-9]+).$",
                RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);
    }
}
