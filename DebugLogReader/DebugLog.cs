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

        public DebugLog(int cameraNumber, List<DebugLogRowFilter> filters)
        {
            m_cameraNumber = cameraNumber;
            m_rows = new List<DebugLogRow>();
            m_filters = filters;
            InitialiseRegex();
        }

        protected virtual void InitialiseRegex()
        {
        }

        public void Load(String filename)
        {
            DebugLogRow newRow = null;
            DateTime previousTimestamp = DateTime.MinValue;
            int rowCount = 0;

            String[] debugLogText = File.ReadAllLines(filename);

            foreach (String line in debugLogText)
            {
                newRow = new DebugLogRow(m_cameraNumber, line, m_rowRegex, previousTimestamp);
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

        private void AddRow(DebugLogRow newRow, List<DebugLogRowFilter> filters)
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
                    conditionsMet = filters[0].MeetsConditions(newRow);

                    for (int i = 1; i < filters.Count; i++)
                    {
                        conditionsMet = conditionsMet && filters[i].MeetsConditions(newRow);
                    }
                }
            }

            if (conditionsMet)
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
            m_rows.Sort(delegate (DebugLogRow log1, DebugLogRow log2) { return log1.Timestamp.CompareTo(log2.Timestamp); });
        }

        public DateTime GetStartTime()
        {
            foreach(DebugLogRow row in m_rows)
            {
                if (row.Timestamp > DateTime.MinValue)
                {
                    return row.Timestamp;
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

        public void Filter(DebugLogRowFilter filter)
        {
            // Store old, create new log
            List<DebugLogRow> oldRows = m_rows;
            m_rows = new List<DebugLogRow>();
            
            foreach(DebugLogRow row in oldRows)
            {
                AddRow(row, new List<DebugLogRowFilter>() { filter });
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

        int m_cameraNumber;
        List<DebugLogRowFilter> m_filters;
        List<DebugLogRow> m_rows;

        protected Regex m_rowRegex;
    }
}
