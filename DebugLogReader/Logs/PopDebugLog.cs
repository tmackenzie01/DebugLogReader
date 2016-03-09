using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        protected override void SetColdstoreInfo(DebugLogRow newRow, DebugLogRow oldRow)
        {
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
    }
}
