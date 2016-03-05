using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class PushDebugLog : DebugLog
    {
        public PushDebugLog(int cameraNumber, List<DebugLogFilter> filters) : base(cameraNumber, filters)
        {
        }

        protected override void InitialiseRegex()
        {
            m_rowRegex = frmDebugLogReader.m_pushedRegex;
        }
    }
}
