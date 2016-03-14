using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class FrameDebugLog : DebugLog
    {
        public FrameDebugLog(IFileWrapper fileWrapper, int cameraNumber, List<DebugLogFilter> filters) : base(fileWrapper, cameraNumber, filters)
        {
            m_summaryHeader = "frame log";
        }

        protected override DebugLogRow ParseLine(int cameraNumber, String line, DateTime previousTimestamp)
        {
            DebugLogRow newRow = new DebugLogFrameRow(cameraNumber, line, previousTimestamp);
            return newRow;
        }

        protected override void SetRTSPErrorCountInfo(DebugLogRow baseRow, DebugLogRow baseOldRow)
        {
            DebugLogFrameRow newRow = (DebugLogFrameRow)baseRow;
            DebugLogFrameRow oldRow = (DebugLogFrameRow)baseOldRow;

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
