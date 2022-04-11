
namespace Accounts.ServiceClients.Cloudinary
{
    public interface IImageHandler
    {
        string PostImage(string image, string username, string location);
    }
}