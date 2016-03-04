using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class DebugLog
    {
        public DebugLog()
        {
            m_cameraNumber = -1;
            m_rows = new List<DebugLogRow>();
        }

        public DebugLog(int cameraNumber, String[] debugLogText, Regex r)
        {
            m_cameraNumber = cameraNumber;
            m_rows = new List<DebugLogRow>();
            DebugLogRow newRow = null;
            DateTime previousTimestamp = DateTime.MinValue;
            int rowCount = 0;

            foreach (String line in debugLogText)
            {
                newRow = new DebugLogRow(cameraNumber, line, r, previousTimestamp);
                m_rows.Add(newRow);

                if (newRow != null)
                {
                    previousTimestamp = newRow.Timestamp;
                }
                rowCount++;
            }

            if (debugLogText.Length != m_rows.Count)
            {
                throw new Exception("Ooops!");
            }
        }

        public void AddLog(DebugLog newLog)
        {
            m_rows.AddRange(newLog.m_rows);

            m_rows.Sort(delegate (DebugLogRow log1, DebugLogRow log2) { return log1.Timestamp.CompareTo(log2.Timestamp); });
        }

        public int Count
        {
            get
            {
                return m_rows.Count;
            }
        }

        public int CameraNumber
        {
            get
            {
                return m_cameraNumber;
            }
        }

        public void Save(String filename)
        {
            StringBuilder text = new StringBuilder();
            foreach(DebugLogRow row in m_rows)
            {
                text.AppendLine(row.ToString());
            }

            File.WriteAllText(filename, text.ToString());
        }

        int m_cameraNumber;
        List<DebugLogRow> m_rows;
    }
}
