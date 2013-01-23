namespace grhughes.com.Website.Web.Modules
{
  using Core.Model;
  using Core.Services.Interfaces;
  using Nancy.ModelBinding;

  public class SearchModule : BaseModule
  {
    public SearchModule (ISearchService searchService)
    {
      Get["/search"] = p =>
                         {
                           var query = this.Bind<SearchQuery>();
                           ViewBag.SearchQuery = query.Query;
                           
                           return View["Index", searchService.Search(query.Query).Results];
                         };
    }
  }
}