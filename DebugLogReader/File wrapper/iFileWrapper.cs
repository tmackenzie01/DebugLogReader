using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public interface IFileWrapper
    {
        String[] LoadFromFile(String filename);
        void Save(List<DebugLogRowBase> rows, String filename);
    }
}
