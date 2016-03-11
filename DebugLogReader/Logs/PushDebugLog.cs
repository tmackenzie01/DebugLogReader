using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class PushDebugLog : DebugLog
    {
        public PushDebugLog(int cameraNumber, List<DebugLogFilter> filters) : base(cameraNumber, filters)
        {
            m_summaryHeader = "push log";
        }

        protected override void InitialiseRegex()
        {
            m_rowRegex = frmDebugLogReader.m_pushedRegex;
        }

        protected override DebugLogRow ParseLine(int cameraNumber, String line, Regex rowRegex, DateTime previousTimestamp)
        {
            DebugLogRow newRow = new DebugLogPushRow(cameraNumber, line, rowRegex, null, previousTimestamp);
            return newRow;
        }
    }
}
