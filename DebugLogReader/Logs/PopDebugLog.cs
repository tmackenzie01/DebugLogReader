using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class PopDebugLog : DebugLog
    {
        public PopDebugLog(int cameraNumber, List<DebugLogFilter> filters) : base(cameraNumber, filters)
        {
            m_summaryHeader = "pop log";
            m_coldstoreIds = new List<int>();
        }

        protected override void InitialiseRegex()
        {
            m_rowRegex = frmDebugLogReader.m_poppedRegex;
            m_wroteDataRegex = frmDebugLogReader.m_wroteDataRegex;
        }

        protected override DebugLogRow ParseLine(int cameraNumber, String line, Regex rowRegex, DateTime previousTimestamp)
        {
            DebugLogRow newRow = new DebugLogPopRow(cameraNumber, line, rowRegex, m_wroteDataRegex, previousTimestamp);
            return newRow;
        }

        protected override void SetWroteDataInfo(DebugLogRow baseRow, ref int dataWritten, ref DateTime lastTime, ref bool nullFrameDetectedPreviously)
        {
            DebugLogPopRow row = (DebugLogPopRow)baseRow;
            if (row.WroteData)
            {
                if (nullFrameDetectedPreviously)
                {
                    // If we have wrote data then we can clear the nullFrameDetected
                    nullFrameDetectedPreviously = false;
                    row.NewWroteData = true;
                }
                else
                {
                    row.SetWroteDataWritten(dataWritten);
                }
                if (!lastTime.Equals(DateTime.MinValue))
                {
                    row.SetWroteDataElapsed(row.Timestamp - lastTime);
                }
                dataWritten = 0;
                lastTime = row.Timestamp;
            }
        }

        protected override void SetColdstoreInfo(DebugLogRow baseRow, DebugLogRow baseOldRow)
        {
            DebugLogPopRow newRow = (DebugLogPopRow)baseRow;
            DebugLogPopRow oldRow = (DebugLogPopRow)baseOldRow;

            if (oldRow != null)
            {
                if (oldRow.ColdstoreInformationDetected)
                {
                    // Keep track of all the coldstore ids for this log
                    if (!m_coldstoreIds.Contains(oldRow.ColdstoreId))
                    {
                        m_coldstoreIds.Add(oldRow.ColdstoreId);
                    }

                    if (!newRow.ColdstoreInformationDetected)
                    {
                        newRow.SetColdstoreId(oldRow.ColdstoreId);
                    }
                }
            }
        }

        public List<int> ColdstoreIds
        {
            get
            {
                return m_coldstoreIds;
            }
        }

        List<int> m_coldstoreIds;
        Regex m_wroteDataRegex;
    }
}
