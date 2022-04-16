
namespace Accounts.ServiceClients.Cloudinary
{
    /// <summary>
    /// Contract for ImageHandler class. Used for dependency injection.
    /// </summary>
    public interface IImageHandler
    {
        string PostProfileImg(string image, string username);
        string PostProductImg(string image, string username, string itemName);
    }
}