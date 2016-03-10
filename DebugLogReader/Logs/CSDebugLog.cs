using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class CSDebugLog : DebugLog
    {
        public CSDebugLog(IFileWrapper fileWrapper, int cameraNumber, List<DebugLogFilter> filters) : base(fileWrapper, cameraNumber, filters)
        {
            m_summaryHeader = "CS log";
        }

        protected override void InitialiseRegex()
        {
            m_rowRegex = frmDebugLogReader.m_csRegex;
        }

        protected override DebugLogRow ParseLine(int cameraNumber, String line, Regex rowRegex, Regex wroteDataRegex, DateTime previousTimestamp)
        {
            DebugLogRow newRow = new DebugLogCSRow(cameraNumber, line, rowRegex, wroteDataRegex, previousTimestamp);
            return newRow;
        }
    }
}
