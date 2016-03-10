using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class PushDebugLog : DebugLog
    {
        public PushDebugLog(IFileWrapper fileWrapper, int cameraNumber, List<DebugLogFilter> filters) : base(fileWrapper, cameraNumber, filters)
        {
            m_summaryHeader = "push log";
        }

        protected override void InitialiseRegex()
        {
            m_rowRegex = frmDebugLogReader.m_pushedRegex;
        }
    }
}
