﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class RealFileWrapper : IFileWrapper
    {
        public String[] LoadFromFile(String filename)
        {
            return File.ReadAllLines(filename);
        }

        public void Save(List<DebugLogRow> rows, String filename)
        {
            StreamWriter sw = new StreamWriter(filename);
            foreach (DebugLogRow row in rows)
            {
                sw.WriteLine(row.ToString());
            }

            sw.Close();
        }
    }
}
