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
        }

        protected override void InitialiseRegex()
        {
            m_rowRegex = frmDebugLogReader.m_poppedRegex;
            m_wroteDataRegex = frmDebugLogReader.m_wroteDataRegex;
        }
    }
}
