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
    private readonly IBlogService blogService;
    private static IndexSearcher Searcher { get; set; }

    public SearchService(IBlogService blogService)
    {
      this.blogService = blogService;
    }

    public void ReIndex()
    {
      var fsDirectory = FsDirectory();

      if (Directory.Exists(IndexDirectory()))
        Directory.Delete(IndexDirectory(), true);

      var writer = new IndexWriter(fsDirectory, new StandardAnalyzer(Version.LUCENE_30), true,
                                   IndexWriter.MaxFieldLength.UNLIMITED);


      var posts = blogService.LoadAll();
      foreach (var blogPost in posts)
      {
        writer.AddDocument(CreateDocument(blogPost));
      }

      writer.Optimize();
      writer.Dispose();
    }

    private Document CreateDocument(BlogPost blogPost)
    {
      throw new NotImplementedException();
    }

    public SearchResults Search(string query, int page = 0, int limit = 10)
    {
      if (string.IsNullOrWhiteSpace(query))
        return new SearchResults();

      var searcher = IndexSearcher();

      var luceneQuery =
        new QueryParser(Version.LUCENE_29, "searchable", new StandardAnalyzer(Version.LUCENE_30)).Parse(query);

      var booleanQuery = new BooleanQuery {{luceneQuery, Occur.MUST}};

      if (limit <= 0) limit = 10;

      var skipRecords = (page*limit);
      var hits = searcher.Search(booleanQuery, null, skipRecords + limit);
      var docs = new List<SearchResult>();

      for (var i = skipRecords; i < hits.TotalHits; i++)
      {
        if (i > skipRecords + (limit - 1)) break;

        var currentDoc = searcher.Doc(hits.ScoreDocs[i].Doc);
        //var node = currentDoc.GetField("node").StringValue();

        //docs.Add(new JavaScriptSerializer().Deserialize<Models.Node>(node));

        //TODO: Load from index
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
      using(var writer = CreateIndexWriter())
      {
        writer.AddDocument(CreateDocument(blogPost));
        writer.Commit();
      }

      Expired = true;
    }

    public void Delete(BlogPost blogPost)
    {
      using (var writer = CreateIndexWriter())
      {
        var query = new BooleanQuery {{new TermQuery(new Term("id", blogPost.Id.ToString())), Occur.MUST}};
        writer.DeleteDocuments(query);
        writer.Commit();
      }

      Expired = true;
    }

    public void Update(BlogPost blogPost)
    {
      using (var writer = CreateIndexWriter())
      {
        var query = new BooleanQuery {{new TermQuery(new Term("id", blogPost.Id.ToString())), Occur.MUST}};
        writer.DeleteDocuments(query);
        writer.AddDocument(CreateDocument(blogPost));
        writer.Commit();
      }

      Expired = true;
    }

    private static IndexWriter CreateIndexWriter()
    {
      return new IndexWriter(FsDirectory(), new StandardAnalyzer(Version.LUCENE_30), false, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);
    }

    private static FSDirectory FsDirectory()
    {
      var indexDirectory = IndexDirectory();

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