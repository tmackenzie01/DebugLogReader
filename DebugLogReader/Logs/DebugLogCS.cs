using System;
using System.Collections.Generic;

namespace DebugLogReader
{
    public class DebugLogCS : DebugLogBase
    {
        public DebugLogCS(IFileWrapper fileWrapper, int cameraNumber, List<DebugLogFilter> filters) : base(fileWrapper, cameraNumber, filters)
        {
            m_summaryHeader = "CS log";
        }

        protected override DebugLogRowBase ParseLine(int cameraNumber, String line, DateTime previousTimestamp)
        {
            DebugLogRowBase newRow = new DebugLogRowCS(cameraNumber, line, previousTimestamp);
            return newRow;
        }
    }
}
