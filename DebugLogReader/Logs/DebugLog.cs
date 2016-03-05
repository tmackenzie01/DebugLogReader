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
            m_filters = null;
            InitialiseRegex();
        }

        public DebugLog(int cameraNumber, List<DebugLogFilter> filters)
        {
            m_cameraNumber = cameraNumber;
            m_rows = new List<DebugLogRow>();
            m_filters = filters;
            InitialiseRegex();
        }

        protected virtual void InitialiseRegex()
        {
        }

        private void SetWroteDataInfo(DebugLogRow row, ref int dataWritten, ref DateTime lastTime)
        {
            if (row.WroteData)
            {
                row.SetWroteDataWritten(dataWritten);
                if (!lastTime.Equals(DateTime.MinValue))
                {
                    row.SetWroteDataElapsed(row.Timestamp - lastTime);
                }
                dataWritten = 0;
                lastTime = row.Timestamp;
            }
        }

        public void Load(String filename)
        {
            DebugLogRow newRow = null;
            DateTime previousTimestamp = DateTime.MinValue;
            int rowCount = 0;
            DateTime lastWroteDataTimestamp = DateTime.MinValue;
            int dataWritten = 0;

            // Check filters for the debug log before we even read file
            if (CheckDebugLogFilters(m_filters))
            {
                String[] debugLogText = File.ReadAllLines(filename);

                foreach (String line in debugLogText)
                {
                    newRow = new DebugLogRow(m_cameraNumber, line, m_rowRegex, previousTimestamp);
                    newRow.RowCount = rowCount;
                    dataWritten = dataWritten + newRow.DataPopped;
                    SetWroteDataInfo(newRow, ref dataWritten, ref lastWroteDataTimestamp);
                    AddRow(newRow, m_filters);

                    if (newRow != null)
                    {
                        previousTimestamp = newRow.Timestamp;
                    }
                    rowCount++;
                }

                if ((debugLogText.Length != m_rows.Count) && (m_filters == null))
                {
                    throw new Exception("Ooops!");
                }
            }
            else
            {
                m_filterMessage = "filtered out";
            }
        }

        private bool CheckDebugLogFilters(List<DebugLogFilter> filters)
        {
            bool conditionsMet = false;

            if (filters == null)
            {
                conditionsMet = true;
            }
            else
            {
                conditionsMet = (filters.Count == 0);

                if (filters.Count > 0)
                {
                    // Check first condition
                    conditionsMet = filters[0].MeetsConditions(this);

                    for (int i = 1; i < filters.Count; i++)
                    {
                        conditionsMet = conditionsMet && filters[i].MeetsConditions(this);
                    }
                }
            }

            return conditionsMet;
        }

        private bool CheckDebugLogRowFilters(DebugLogRow row, List<DebugLogFilter> filters)
        {
            bool conditionsMet = false;

            if (filters == null)
            {
                conditionsMet = true;
            }
            else
            {
                conditionsMet = (filters.Count == 0);

                if (filters.Count > 0)
                {
                    // Check first condition
                    conditionsMet = filters[0].MeetsConditions(row);

                    for (int i = 1; i < filters.Count; i++)
                    {
                        conditionsMet = conditionsMet && filters[i].MeetsConditions(row);
                    }
                }
            }

            return conditionsMet;
        }

        private void AddRow(DebugLogRow newRow, List<DebugLogFilter> filters)
        {
            if (CheckDebugLogRowFilters(newRow, filters))
            {
                m_rows.Add(newRow);
            }
        }

        public void AddLog(DebugLog newLog)
        {
            m_rows.AddRange(newLog.m_rows);
        }

        public void Sort()
        {
            m_rows.Sort(delegate (DebugLogRow log1, DebugLogRow log2) 
            {
                int timestampComp = log1.Timestamp.CompareTo(log2.Timestamp);

                if (timestampComp == 0)
                {
                    return log1.RowCount.CompareTo(log2.RowCount);
                }

                return timestampComp;
            });
        }

        public DateTime GetStartTime()
        {
            foreach (DebugLogRow row in m_rows)
            {
                if (row.Timestamp > DateTime.MinValue)
                {
                    return row.Timestamp;
                }
            }

            return DateTime.MinValue;
        }

        public DateTime GetEndime()
        {
            if (m_rows.Count > 0)
            {
                int i = m_rows.Count - 1;
                while (i > 0)
                {
                    if (m_rows[i].Timestamp > DateTime.MinValue)
                    {
                        return m_rows[i].Timestamp;
                    }
                }
            }

            return DateTime.MinValue;
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

        public void Filter(DebugLogFilter filter)
        {
            // Store old, create new log
            List<DebugLogRow> oldRows = m_rows;
            m_rows = new List<DebugLogRow>();

            foreach (DebugLogRow row in oldRows)
            {
                AddRow(row, new List<DebugLogFilter>() { filter });
            }
        }

        public void Save(String filename)
        {
            StreamWriter sw = new StreamWriter(filename);
            foreach (DebugLogRow row in m_rows)
            {
                sw.WriteLine(row.ToString());
            }

            sw.Close();
        }

        public String SummaryText()
        {
            if (String.IsNullOrEmpty(m_filterMessage))
            {
                DateTime startTime = GetStartTime();
                TimeSpan duration = GetEndime() - startTime;

                return $"{m_rows?.Count} lines, {startTime.ToString("HH:mm:ss")} ({duration.TotalSeconds} secs)";
            }
            else
            {
                return m_filterMessage;
            }
        }

        int m_cameraNumber;
        List<DebugLogFilter> m_filters;
        String m_filterMessage;
        List<DebugLogRow> m_rows;

        protected Regex m_rowRegex;
    }
}
