using System;
using System.Collections.Generic;

namespace DebugLogReader
{
    public class AviDebugLog : DebugLog
    {
        public AviDebugLog(IFileWrapper fileWrapper, int cameraNumber, List<DebugLogFilter> filters) : base(fileWrapper, cameraNumber, filters)
        {
            m_summaryHeader = "Avi log";
        }

        protected override DebugLogRow ParseLine(int cameraNumber, String line, DateTime previousTimestamp)
        {
            DebugLogRow newRow = new DebugLogAviRow(cameraNumber, line, previousTimestamp);
            return newRow;
        }
    }
}
