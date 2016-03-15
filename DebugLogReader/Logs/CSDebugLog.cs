using System;
using System.Collections.Generic;

namespace DebugLogReader
{
    public class CSDebugLog : DebugLog
    {
        public CSDebugLog(IFileWrapper fileWrapper, int cameraNumber, List<DebugLogFilter> filters) : base(fileWrapper, cameraNumber, filters)
        {
            m_summaryHeader = "CS log";
        }

        protected override DebugLogRow ParseLine(int cameraNumber, String line, DateTime previousTimestamp)
        {
            DebugLogRow newRow = new DebugLogCSRow(cameraNumber, line, previousTimestamp);
            return newRow;
        }
    }
}
