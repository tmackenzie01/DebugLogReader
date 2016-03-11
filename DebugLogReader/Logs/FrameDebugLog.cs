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
        public FrameDebugLog(int cameraNumber, List<DebugLogFilter> filters) : base(cameraNumber, filters)
        {
            m_summaryHeader = "frame log";
        }

        protected override void InitialiseRegex()
        {
            m_rowRegex = frmDebugLogReader.m_frameRegex;
        }

        protected override DebugLogRow ParseLine(int cameraNumber, String line, Regex rowRegex, DateTime previousTimestamp)
        {
            DebugLogRow newRow = new DebugLogFrameRow(cameraNumber, line, rowRegex, null, previousTimestamp);
            return newRow;
        }
    }
}
