
namespace ConsoleApp2.utils
{
    public class PathValidationService
    {
        private readonly HashSet<string> approvedPathCatImagesExtensions = new HashSet<string>() { ".jpg", ".png", ".jpeg" };

        public string ValidatePath(string path)
        {
            var extension = Path.GetExtension(path);

            if (string.IsNullOrWhiteSpace(extension))
            {
                path += ".jpg";
            }
            else if (!approvedPathCatImagesExtensions.Contains(extension))
            {
                throw new Exception(".jpg, .png, .jpeg extensions only");
            }

            return path;
        }
    }
}
