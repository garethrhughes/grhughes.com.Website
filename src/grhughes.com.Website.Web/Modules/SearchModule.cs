namespace grhughes.com.Website.Web.Modules
{
  using Core.Model;
  using Core.Services.Interfaces;
  using Nancy.ModelBinding;

  public class SearchModule : BaseModule
  {
    public SearchModule (ISearchService<BlogPost> searchService)
    {
      Get["/search"] = p =>
                         {
                           var query = this.Bind<SearchQuery>();
                           var result = searchService.Search(query.Query, query.Page, 5);

                           ViewBag.SearchQuery = query.Query;
                           
                           return View["Index", result];
                         };
    }
  }
}