namespace MicroSocialPlatform.Models;

// This model is used by the search bar.
// The search bar sends a get request to the given route
// to implement searching in a page.
public class SearchModel
{
    public string Route;

    public SearchModel(string route)
    {
        Route = route;
    }
}