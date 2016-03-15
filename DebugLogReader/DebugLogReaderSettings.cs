using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    [ProtoContract]
    public class DebugLogReaderSettings
    {
        [ProtoMember(1)]
        public String LogDirectory
        {
            set
            {
                m_logDirectory = value;
            }
            get
            {
                return m_logDirectory;
            }
        }

        String m_logDirectory;
    }
}
