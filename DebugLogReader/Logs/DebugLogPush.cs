using System;
using System.Collections.Generic;

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
