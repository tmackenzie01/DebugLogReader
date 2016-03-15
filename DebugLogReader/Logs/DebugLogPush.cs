using System;
using System.Collections.Generic;

namespace DebugLogReader
{
    public class DebugLogPush : DebugLogBase
    {
        public DebugLogPush(IFileWrapper fileWrapper, int cameraNumber, List<DebugLogFilter> filters) : base(fileWrapper, cameraNumber, filters)
        {
            m_summaryHeader = "push log";
        }

        protected override DebugLogRowBase ParseLine(int cameraNumber, String line, DateTime previousTimestamp)
        {
            DebugLogRowBase newRow = new DebugLogRowPush(cameraNumber, line, previousTimestamp);
            return newRow;
        }
    }
}
