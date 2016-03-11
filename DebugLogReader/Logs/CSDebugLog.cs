using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class CSDebugLog : DebugLog
    {
        public CSDebugLog(int cameraNumber, List<DebugLogFilter> filters) : base(cameraNumber, filters)
        {
            m_summaryHeader = "CS log";
        }

        protected override void InitialiseRegex()
        {
            m_rowRegex = frmDebugLogReader.m_csRegex;
        }

        protected override DebugLogRow ParseLine(int cameraNumber, String line, Regex rowRegex, DateTime previousTimestamp)
        {
            DebugLogRow newRow = new DebugLogCSRow(cameraNumber, line, rowRegex, null, previousTimestamp);
            return newRow;
        }
    }
}
