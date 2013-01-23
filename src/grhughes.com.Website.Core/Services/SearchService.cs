namespace grhughes.com.Website.Core.Services
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Web;
  using Interfaces;
  using Lucene.Net.Analysis.Standard;
  using Lucene.Net.Documents;
  using Lucene.Net.Index;
  using Lucene.Net.QueryParsers;
  using Lucene.Net.Search;
  using Lucene.Net.Store;
  using Model;
  using Directory = System.IO.Directory;
  using Version = Lucene.Net.Util.Version;

  public class SearchService : ISearchService
  {
    private static readonly SimpleFSLockFactory LockFactory = new SimpleFSLockFactory();
    private static bool Expired = true;
    private static IndexSearcher Searcher { get; set; }

    public void ReIndex()
    {
      FSDirectory fsDirectory = FsDirectory();

      if (Directory.Exists(IndexDirectory()))
        Directory.Delete(IndexDirectory(), true);

      var writer = new IndexWriter(fsDirectory, new StandardAnalyzer(Version.LUCENE_30), true,
                                   IndexWriter.MaxFieldLength.UNLIMITED);
      /*var nodes = library.GetXmlNodeByXPath("/root//*[@id]");

      while (nodes.MoveNext())
      {
        var current = nodes.Current;
        if (current != null)
        {
          var node = new Node(int.Parse(current.GetAttribute("id", "")));
          writer.AddDocument(Document(node));
        }
      }*/

      writer.Optimize();
      writer.Close();
    }

    public SearchResults Search(string query, int page = 0, int limit = 10)
    {
      if (string.IsNullOrWhiteSpace(query))
        return new SearchResults();

      IndexSearcher searcher = IndexSearcher();

      Query luceneQuery =
        new QueryParser(Version.LUCENE_29, "searchable", new StandardAnalyzer(Version.LUCENE_29)).Parse(query);

      var booleanQuery = new BooleanQuery();
      booleanQuery.Add(luceneQuery, Occur.MUST);

      if (limit <= 0) limit = 10;

      int skipRecords = (page*limit);
      TopDocs hits = searcher.Search(booleanQuery, null, skipRecords + limit);
      var docs = new List<SearchResult>();

      for (int i = skipRecords; i < hits.TotalHits; i++)
      {
        if (i > skipRecords + (limit - 1)) break;

        Document currentDoc = searcher.Doc(hits.ScoreDocs[i].Doc);
        //var node = currentDoc.GetField("node").StringValue();

        //docs.Add(new JavaScriptSerializer().Deserialize<Models.Node>(node));
      }

      searcher.Dispose();

      return new SearchResults
               {
                 Page = page,
                 TotalResults = hits.TotalHits,
                 Results = docs,
                 ResultsPerPage = limit
               };
    }

    public void Add(BlogPost blogPost)
    {
      Expired = true;
      throw new NotImplementedException();
    }

    public void Delete(BlogPost blogPost)
    {
      Expired = true;
      throw new NotImplementedException();
    }

    public void Update(BlogPost blogPost)
    {
      Expired = true;
      throw new NotImplementedException();
    }

    private static FSDirectory FsDirectory()
    {
      string indexDirectory = IndexDirectory();

      if (!Directory.Exists(indexDirectory))
        Directory.CreateDirectory(indexDirectory);

      return FSDirectory.Open(new DirectoryInfo(indexDirectory), LockFactory);
    }

    private static string IndexDirectory()
    {
      return string.Format("{0}/searchindex", HttpContext.Current.Server.MapPath("~/App_Data"));
    }

    private static IndexSearcher IndexSearcher()
    {
      if (Expired)
      {
        Expired = false;
        Searcher = new IndexSearcher(IndexReader.Open(FsDirectory(), true));
      }

      return Searcher;
    }
  }
}