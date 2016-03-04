using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class PopDebugLog : DebugLog
    {
        public PopDebugLog(int cameraNumber, String[] debugLogText, List<DebugLogRowFilter> filters) : base(cameraNumber, debugLogText, filters)
        {
        }

        protected override void InitialiseRegex()
        {
            m_rowRegex = frmDebugLogReader.m_poppedRegex;
        }
    }
}
