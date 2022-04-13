using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.ServiceClients.Cloudinary
{
    public class ImageHandler : IImageHandler
    {
        private readonly Account _account;
        public ImageHandler()
        {

            _account = new Account
            {
                Cloud = "sovran-merch",
                ApiKey = "228678378674647",
                ApiSecret = "khwW-o2cq6SmVssVY1K5B7t5U8s"
            };
        }

        public string PostImage(string image, string username, string location)
        {
            CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(_account);
            cloudinary.Api.Secure = true;

            var uploadParams = new ImageUploadParams()
            {
                PublicId = $"merchants/{username}/{location}",
                Transformation = new Transformation().Width(400).Height(400).Crop("limit"),
                File = new FileDescription(image)
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            var result = uploadResult.Url;
            return result.AbsoluteUri;
        }
    }
}
