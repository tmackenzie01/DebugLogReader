using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class AviDebugLog : DebugLog
    {
        public AviDebugLog(int cameraNumber, List<DebugLogFilter> filters) : base(cameraNumber, filters)
        {
            m_summaryHeader = "Avi log";
        }

        protected override void InitialiseRegex()
        {
            m_rowRegex = frmDebugLogReader.m_aviRegex;
        }

        protected override DebugLogRow ParseLine(int cameraNumber, String line, Regex rowRegex, Regex wroteDataRegex, DateTime previousTimestamp)
        {
            DebugLogRow newRow = new DebugLogAviRow(cameraNumber, line, rowRegex, wroteDataRegex, previousTimestamp);
            return newRow;
        }
    }
}
