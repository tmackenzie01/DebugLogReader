using System;
using System.Collections.Generic;

namespace DebugLogReader
{
    public class DebugLogFrame : DebugLogBase
    {
        public DebugLogFrame(IFileWrapper fileWrapper, int cameraNumber, List<DebugLogFilter> filters) : base(fileWrapper, cameraNumber, filters)
        {
            m_summaryHeader = "frame log";
        }

        protected override DebugLogRowBase ParseLine(int cameraNumber, String line, DateTime previousTimestamp)
        {
            DebugLogRowBase newRow = new DebugLogRowFrame(cameraNumber, line, previousTimestamp);
            return newRow;
        }

        protected override void SetRTSPErrorCountInfo(DebugLogRowBase baseRow, DebugLogRowBase baseOldRow)
        {
            DebugLogRowFrame newRow = (DebugLogRowFrame)baseRow;
            DebugLogRowFrame oldRow = (DebugLogRowFrame)baseOldRow;

            if (oldRow != null)
            {
                if (oldRow.RTSPErrorCount != newRow.RTSPErrorCount)
                {
                    newRow.SetRTSPErrorCountChanged(true);
                }
            }
        }
    }
}
