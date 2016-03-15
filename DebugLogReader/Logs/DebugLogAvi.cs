using System;
using System.Collections.Generic;

namespace DebugLogReader
{
    public class DebugLogAvi : DebugLogBase
    {
        public DebugLogAvi(IFileWrapper fileWrapper, int cameraNumber, List<DebugLogFilter> filters) : base(fileWrapper, cameraNumber, filters)
        {
            m_summaryHeader = "Avi log";
        }

        protected override DebugLogRowBase ParseLine(int cameraNumber, String line, DateTime previousTimestamp)
        {
            DebugLogRowBase newRow = new DebugLogRowAvi(cameraNumber, line, previousTimestamp);
            return newRow;
        }
    }
}
