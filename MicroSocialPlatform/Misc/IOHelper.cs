namespace MicroSocialPlatform.Misc;

public static class IOHelper
{
    // Returns the actual path to the file.
    public static string getImageFilePath(IWebHostEnvironment env, string fileName)
    {
        return Path.Combine(env.WebRootPath, "images", fileName);
    }

    // Returns the filename string that will be placed in the database.
    public static string getImageDatabasePath(string fileName)
    {
        return "/images/" + fileName;
    }

    public static async void writeToPath(string path, IFormFile file)
    {
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
    }
}