namespace MicroSocialPlatform.Models;

public class ErrorView
{
    public string Error { get; set; }

    public ErrorView(string error)
    {
        Error = error;
    }
}