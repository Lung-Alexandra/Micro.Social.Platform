namespace MicroSocialPlatform.Misc;

public static class IOHelper
{
    public static string DefaultPostImage = "https://cdn.pixabay.com/photo/2015/04/23/21/59/tree-736877__480.jpg";
    public static string DefaultProfileImage = "https://img.freepik.com/free-icon/user_318-790139.jpg?w=2000";

    // Returns the filename string that will be placed in the database.
    public static string getImageDatabasePath(string fileName)
    {
        return "/images/" + fileName;
    }

    // Saves the given file into the images folder.
    public static async void saveImage(IWebHostEnvironment env, IFormFile file)
    {
        string path = Path.Combine(env.WebRootPath, "images", file.FileName);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
    }

    // If the given string is null, returns the path to the default image, else returns the image.
    public static string imageOrDefault(string? image, string defaultImage)
    {
        if (image == null)
        {
            return defaultImage;
        }

        return image;
    }
}