using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

            txtLogDirectory.Text = @"C:\Users\tmackenzie01\Documents\Recorder testing\20160302\DebugLogs2";

            // Move the Regex out
            m_pushedRegex = new Regex("Pushed...(?<timestamp>[0-9]+.[0-9]+.[0-9]+.[0-9]+.[0-9]+.[0-9]+.[0-9]+).(\\-\\-\\-...[0-9]+.[0-9]+.seconds..)*Q.[0-9]+.F..?[0-9]+,.[0-9]+,.[0-9]+$",
                RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);
            m_poppedRegex = new Regex("Popped...(?<timestamp>[0-9]+.[0-9]+.[0-9]+.[0-9]+.[0-9]+.[0-9]+.[0-9]+).(\\-\\-\\-..[0-9]+.[0-9]+.seconds..)*Q.[0-9]+.F..?[0-9]+,.[0-9]+,.[0-9]+$",
                RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);
        }

        private Regex GetPushedLogRegex()
        {
            return m_pushedRegex;
        }

        private Regex GetPoppedLogRegex()
        {
            return m_poppedRegex;
        }

        private void btnReadLogs_Click(object sender, EventArgs e)
        {
            StartLogReads();
        }

        private void StartLogReads()
        {
            lstProgress.Items.Clear();
            btnReadLogs.Enabled = false;

            DebugLogRow pushRow1 = new DebugLogRow(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0", GetPushedLogRegex());
            DebugLogRow pushRow2 = new DebugLogRow(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0", GetPushedLogRegex());
            DebugLogRow pushRow3 = new DebugLogRow(1, "Pushed - 02/03/2016 17:36:15.325 ---  (76.810 seconds) Q:0 F:1242, 5806, 0", GetPushedLogRegex());
            DebugLogRow pushRow4 = new DebugLogRow(1, "Pushed - 02/03/2016 17:08:47.343 ---  (5.010 seconds) Q:0 F:-1, 0, 4", GetPushedLogRegex());
            DebugLogRow pushRow5 = new DebugLogRow(1, "Pushed - 02/03/2016 17:08:47.343 ---  (0.000 seconds) Q:0 F:Null", GetPushedLogRegex());
            DebugLogRow pushRow6 = new DebugLogRow(1, "Pushed - 02/03/2016 17:09:37.973 Q:0 F: 0, 0, 0", GetPushedLogRegex());
            DebugLogRow popRow1 = new DebugLogRow(1, "Popped - 02/03/2016 17:01:48.412 --- (0.000 seconds) Q:1 F:745, 5234, 0", GetPoppedLogRegex());
            DebugLogRow popRow2 = new DebugLogRow(1, "Wrote data", GetPoppedLogRegex());
            DebugLogRow popRow3 = new DebugLogRow(1, "Popped - 02/03/2016 17:09:37.973 Q:1 F:0, 0, 0", GetPoppedLogRegex());
            List<int> cameraNumbers = GetCameraNumbers();

            cameraNumbers.Sort();

            if (cameraNumbers.Count > 0)
            {
                m_logs = new List<DebugLog>();

                foreach (int cameraNumber in cameraNumbers)
                {
                    BackgroundWorker bgReadLog = new BackgroundWorker();
                    bgReadLog.DoWork += ReadLogs_DoWork;
                    bgReadLog.ProgressChanged += ReadLogs_ProgressChanged;
                    bgReadLog.RunWorkerCompleted += ReadLogs_RunWorkerCompleted;
                    bgReadLog.RunWorkerAsync(new DebugLogReaderArgs(txtLogDirectory.Text, cameraNumber));
                    m_readLogsInProgress++;
                }
            }

            prgFiles.Maximum = cameraNumbers.Count;
        }

        private List<int> GetCameraNumbers()
        {
            String[] logDirectories = Directory.GetDirectories(txtLogDirectory.Text);
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

            //return cameraNumbers;
            // Returning only one camera id here for testing
            return new List<int> { 1 };
        }

        private void ReadLogs_DoWork(object sender, DoWorkEventArgs e)
        {
            DebugLogReaderArgs args = (DebugLogReaderArgs)e.Argument;

            String[] logFiles = Directory.GetFiles(args.LogDirectory());
            String pushFile = "";
            String popFile = "";
            DebugLog pushLog = null;
            DebugLog popLog = null;
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
            }

            if (fileFoundCount == 2)
            {
                if (!String.IsNullOrEmpty(pushFile))
                {
                    pushLog = new DebugLog(args.CameraNumber, File.ReadAllLines(pushFile), m_pushedRegex);
                }
                if (!String.IsNullOrEmpty(popFile))
                {
                    popLog = new DebugLog(args.CameraNumber, File.ReadAllLines(popFile), m_poppedRegex);
                }

                pushLog.AddLog(popLog);

                e.Result = new DebugLogReadResult(args.CameraNumber, pushLog, popLog);
            }
            else
            {
                e.Result = new DebugLogReadResult(args.CameraNumber);
            }
        }

        private void ReadLogs_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void ReadLogs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DebugLogReadResult result = (DebugLogReadResult)e.Result;
            bool combineLogs = false;

            ListViewItem newItem = new ListViewItem(result.ToString());

            lstProgress.Items.Add(newItem);
            prgFiles.Value++;

            lock (m_logs)
            {
                m_logs.Add(result.PushLog);
                m_logs.Add(result.PopLog);
                m_readLogsInProgress--;

                combineLogs = (m_readLogsInProgress == 0);
            }

            if (combineLogs)
            {
                // Start the log combining
            }
        }

        Regex m_pushedRegex;
        Regex m_poppedRegex;

        int m_readLogsInProgress;
        List<DebugLog> m_logs;
    }
}
