using System;
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

        public void Save(List<DebugLogRowBase> rows, String filename)
        {
            System.Diagnostics.Stopwatch stp = new System.Diagnostics.Stopwatch();
            stp.Start();
            StreamWriter sw = new StreamWriter(filename);
            foreach (DebugLogRowBase row in rows)
            {
                sw.WriteLine(row.ToString());
            }

            sw.Close();
            stp.Stop();
            System.Diagnostics.Debug.WriteLine($"Save: {stp.Elapsed.TotalSeconds.ToString("f3")} seconds");
        }

        public Task SaveAsync(List<DebugLogRowBase> rows, String filename)
        {
            return Task.Run(() =>
            {
                Save(rows, filename);
            });

            // Tried doing it like below but it was slower
            // Because it is a tight loop or was StreamWriter doing something funny?
            //System.Diagnostics.Stopwatch stp = new System.Diagnostics.Stopwatch();
            //stp.Start();
            //FileStream fsAsync = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true);

            //StreamWriter sw = new StreamWriter(fsAsync);
            //foreach (DebugLogRowBase row in rows)
            //{
            //    await sw.WriteLineAsync(row.ToString());
            //}
            //stp.Stop();
            //System.Diagnostics.Debug.WriteLine($"SaveAsync {stp.Elapsed.TotalSeconds.ToString("f3")} seconds");

            //sw.Close();
        }
    }
}
