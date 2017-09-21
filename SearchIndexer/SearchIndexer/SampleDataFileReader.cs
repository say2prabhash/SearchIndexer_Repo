using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SearchIndexer
{
    public class SampleDataFileReader
    {
        public IEnumerable<SampleDataFileRow> ReadAllRows()
        {
            FileInfo assFile = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string file = string.Format(@"D:\Lucene\SampleDataFile.txt", assFile.Directory.FullName);
            string[] lines = File.ReadAllLines(file);
            for (int i = 0; i < lines.Length; i++)
            {
                yield return new SampleDataFileRow
                {
                    LineNumber = i + 1,
                    LineText = lines[i]
                };
            }
        }
    }
}
