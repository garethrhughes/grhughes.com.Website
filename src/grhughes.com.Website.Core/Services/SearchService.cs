namespace grhughes.com.Website.Core.Services
{
  using System.Collections.Generic;
  using System.IO;
  using System.Text.RegularExpressions;
  using System.Web.Hosting;
  using Interfaces;
  using Lucene.Net.Analysis.Snowball;
  using Lucene.Net.Documents;
  using Lucene.Net.Index;
  using Lucene.Net.QueryParsers;
  using Lucene.Net.Search;
  using Lucene.Net.Store;
  using Lucene.Net.Util;
  using Model;
  using Newtonsoft.Json;
  using Directory = System.IO.Directory;

  public class SearchService : ISearchService
  {
    private static readonly SimpleFSLockFactory LockFactory = new SimpleFSLockFactory();
    private static bool Expired = true;
    private readonly IBlogService blogService;

    public SearchService(IBlogService blogService)
    {
      this.blogService = blogService;
    }

    private static IndexSearcher Searcher { get; set; }

    public void ReIndex()
    {
      if (Directory.Exists(IndexDirectory())) return;

      var fsDirectory = FsDirectory();
      var writer = new IndexWriter(fsDirectory, new SnowballAnalyzer(Version.LUCENE_30, "English"), true,
                                   IndexWriter.MaxFieldLength.UNLIMITED);


      var posts = blogService.LoadAll();
      foreach (var blogPost in posts)
      {
        writer.AddDocument(CreateDocument(blogPost));
      }

      writer.Optimize();
      writer.Dispose();
    }

    public SearchResults Search(string query, int page = 0, int limit = 10)
    {
      if (string.IsNullOrWhiteSpace(query))
        return new SearchResults();

      var searcher = IndexSearcher();

      var luceneQuery =
        new QueryParser(Version.LUCENE_30, "searchable", new SnowballAnalyzer(Version.LUCENE_30, "English")).Parse(query);

      var booleanQuery = new BooleanQuery {{luceneQuery, Occur.MUST}};

      if (limit <= 0) limit = 10;

      var skipRecords = (page*limit);
      var hits = searcher.Search(booleanQuery, null, skipRecords + limit);
      var docs = new List<BlogPost>();

      for (var i = skipRecords; i < hits.TotalHits; i++)
      {
        if (i > skipRecords + (limit - 1)) break;

        var currentDoc = searcher.Doc(hits.ScoreDocs[i].Doc);
        docs.Add(JsonConvert.DeserializeObject<BlogPost>(currentDoc.GetField("blogpost").StringValue));
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
      using (var writer = CreateIndexWriter())
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

    private static Document CreateDocument(BlogPost blogPost)
    {
      var doc = new Document();
      var contentregex = new Regex("<[^>]*>", RegexOptions.Multiline | RegexOptions.Compiled);

      doc.Add(new Field("id", blogPost.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
      doc.Add(new Field("blogpost", JsonConvert.SerializeObject(blogPost), Field.Store.YES,
                        Field.Index.NOT_ANALYZED_NO_NORMS));
      doc.Add(new Field("searchable",
                        string.Format("{0} {1}", blogPost.Title, contentregex.Replace(blogPost.Content, " ")),
                        Field.Store.YES, Field.Index.ANALYZED));

      return doc;
    }

    private static IndexWriter CreateIndexWriter()
    {
      return new IndexWriter(FsDirectory(), new SnowballAnalyzer(Version.LUCENE_30, "English"), false,
                             IndexWriter.MaxFieldLength.UNLIMITED);
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
      return HostingEnvironment.MapPath("~/App_Data/searchindex");
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