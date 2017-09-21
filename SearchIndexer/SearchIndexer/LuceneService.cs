using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene;
using Lucene.Net.Analysis;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using static Lucene.Net.Index.IndexWriter;
using Lucene.Net.Analysis.Standard;

namespace SearchIndexer
{
    public class LuceneService
    {
            // Note there are many different types of Analyzer that may be used with Lucene, the exact one you use
            // will depend on your requirements
            private Analyzer analyzer = new WhitespaceAnalyzer();
            private Directory luceneIndexDirectory;
            private IndexWriter writer;
            private string indexPath = @"d:\LuceneIndex";

            public LuceneService()
            {
                InitialiseLucene();
            }

            private void InitialiseLucene()
            {
                if (System.IO.Directory.Exists(indexPath))
                {
                    System.IO.Directory.Delete(indexPath, true);
                }

            luceneIndexDirectory = FSDirectory.Open(indexPath);
                writer = new IndexWriter(luceneIndexDirectory, analyzer, true,MaxFieldLength.UNLIMITED);
            }

            public void BuildIndex(IEnumerable<SampleDataFileRow> dataToIndex)
            {
                foreach (var sampleDataFileRow in dataToIndex)
                {
                    Document doc = new Document();
                    doc.Add(new Field("LineNumber",
                    sampleDataFileRow.LineNumber.ToString(),
                    Field.Store.YES,
                    Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("LineText",
                    sampleDataFileRow.LineText,
                    Field.Store.YES,
                    Field.Index.ANALYZED));
                    writer.AddDocument(doc);
                }
                writer.Optimize();
                //writer.Flush();
                writer.Close();
                //luceneIndexDirectory.Close();
            }
        public IEnumerable<SampleDataFileRow> Search(string searchTerm)
        {
            IndexSearcher searcher = new IndexSearcher(luceneIndexDirectory);
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_CURRENT,"LineText", analyzer);

            Query query = parser.Parse(searchTerm);
            TopDocs resultDocs = searcher.Search(query,4);
            List<SampleDataFileRow> results = new List<SampleDataFileRow>();
            SampleDataFileRow sampleDataFileRow = null;

            foreach (var scoreDoc in resultDocs.ScoreDocs)
            {
                Lucene.Net.Documents.Document doc = searcher.Doc(scoreDoc.Doc);
                Field field = doc.GetField("LineText");
                sampleDataFileRow.LineText = field.ToString();
                results.Add(sampleDataFileRow);
            }

            return results.OrderByDescending(x => x.Score).ToList();
        }

    }
}
