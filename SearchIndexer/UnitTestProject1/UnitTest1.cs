using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchIndexer;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            SampleDataFileReader reader = new SampleDataFileReader();
            IEnumerable<SampleDataFileRow> rows= reader.ReadAllRows();
            LuceneService service = new LuceneService();
            service.BuildIndex(rows);
            rows=service.Search("job");
        }
    }
}
