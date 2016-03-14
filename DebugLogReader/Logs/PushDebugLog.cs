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
        public PushDebugLog(IFileWrapper fileWrapper, int cameraNumber, List<DebugLogFilter> filters) : base(fileWrapper, cameraNumber, filters)
        {
            m_summaryHeader = "push log";
        }

        protected override DebugLogRow ParseLine(int cameraNumber, String line, DateTime previousTimestamp)
        {
            DebugLogRow newRow = new DebugLogPushRow(cameraNumber, line, previousTimestamp);
            return newRow;
        }
    }
}
