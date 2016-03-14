using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace DebugLogReader
{
    public class DebugLog
    {
        public DebugLog(IFileWrapper fileWrapper)
        {
            m_fileWrapper = fileWrapper;
            m_cameraNumber = -1;
            m_rows = new List<DebugLogRow>();
            m_filters = null;
        }

        public DebugLog(IFileWrapper fileWrapper, int cameraNumber, List<DebugLogFilter> filters)
        {
            m_fileWrapper = fileWrapper;
            m_cameraNumber = cameraNumber;
            m_rows = new List<DebugLogRow>();
            m_filters = filters;
        }

        // Only wrote data rows hold coldstore id info, but we want them in all rows for the popping
        protected virtual void SetColdstoreInfo(DebugLogRow newRow, DebugLogRow oldRow)
        {
        }

        // Only frame data rows hold RTSP error count info
        protected virtual void SetRTSPErrorCountInfo(DebugLogRow newRow, DebugLogRow oldRow)
        {
                }

        public void Load(String filename)
        {
            DebugLogRow newRow = null;
            DebugLogRow previousRow = null;
            DateTime previousTimestamp = DateTime.MinValue;
            DateTime lastWroteDataTimestamp = DateTime.MinValue;
            int dataWritten = 0;
            bool nullFrameDetectedPreviously = false;

            // Check filters for the debug log before we even read file
            if (CheckDebugLogFilters(m_filters))
            {
                String[] debugLogText = m_fileWrapper.LoadFromFile(filename);

                if (debugLogText.Length > 0)
                {
                    foreach (String line in debugLogText)
                    {
                        if (!String.IsNullOrEmpty(line) && (!String.IsNullOrWhiteSpace(line)))
                        {
                            newRow = ParseLine(m_cameraNumber, line, previousTimestamp);
                            if (newRow.NullFrameDetected)
                            {
                                // Null frame stops recording so clear the data written progress
                                dataWritten = 0;
                            }
                            else
                        {
                            dataWritten = dataWritten + newRow.DataPopped;
                            }
                            nullFrameDetectedPreviously = newRow.NullFrameDetected || nullFrameDetectedPreviously;

                            SetWroteDataInfo(newRow, ref dataWritten, ref lastWroteDataTimestamp, ref nullFrameDetectedPreviously);
                            SetColdstoreInfo(newRow, previousRow);
                            SetRTSPErrorCountInfo(newRow, previousRow);
                            AddRow(newRow, m_filters);

                            if (newRow != null)
                            {
                                previousTimestamp = newRow.Timestamp;
                            }
                        }
                        previousRow = newRow;
                    }
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

        protected virtual void SetWroteDataInfo(DebugLogRow row, ref int dataWritten, ref DateTime lastTime, ref bool nullFrameDetectedPreviously)
        {
            nullFrameDetectedPreviously = false;
        }

        protected virtual DebugLogRow ParseLine(int cameraNumber, String line, DateTime previousTimestamp)
        {
            throw new Exception("Not implemented");
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
            m_rows.Sort(delegate (DebugLogRow row1, DebugLogRow row2)
            {
                int timestampComp = row1.Timestamp.CompareTo(row2.Timestamp);

                if (timestampComp == 0)
                {
                    return row1.SortingText.CompareTo(row2.SortingText);
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
                while (i >= 0)
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
            m_fileWrapper.Save(m_rows, filename);
        }

        public String SummaryText()
        {
            if (String.IsNullOrEmpty(m_filterMessage))
            {
                String lineSummary = "no lines";
                String timeSummary = "";
                String durationSummary = "";
                if (m_rows != null)
                {
                    lineSummary = $"{m_rows.Count} lines";
                    if (m_rows.Count > 0)
                    {
                DateTime startTime = GetStartTime();
                TimeSpan duration = GetEndime() - startTime;
                        if (duration.TotalSeconds > 1.0f)
                        {
                            durationSummary = $", {(int)duration.TotalSeconds} secs";
                        }
                        else if (duration.TotalSeconds > 0.0f)
                        {
                            durationSummary = $", {(int)duration.TotalMilliseconds} ms";
                        }

                        timeSummary = $"{startTime.ToString("HH:mm:ss")}";
                    }
                }

                return $"{m_summaryHeader} {lineSummary} ({timeSummary}{durationSummary})";
            }
            else
            {
                return $"{m_summaryHeader} {m_filterMessage}";
            }
        }

        public override string ToString()
        {
            return $"{m_summaryHeader} Cam {m_cameraNumber}";
        }

        protected String m_summaryHeader;
        protected int m_cameraNumber;
        protected List<DebugLogFilter> m_filters;
        protected String m_filterMessage;
        protected List<DebugLogRow> m_rows;

        IFileWrapper m_fileWrapper;

        protected Regex m_rowRegex;
        protected Regex m_wroteDataRegex;
    }
}
