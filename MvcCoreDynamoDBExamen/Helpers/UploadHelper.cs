using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreDynamoDBExamen.Helpers
{
    public class UploadHelper
    {
        PathProvider pathprovider;

        public UploadHelper(PathProvider pathprovider)
        {
            this.pathprovider = pathprovider;
        }

        public async Task<String> UploadFileAsync(IFormFile formFile
        , Folders folder)
        {
            Random rnd = new Random();
            int suffix = rnd.Next(5, 80);
            String fileName = suffix.ToString() + formFile.FileName;
            
            String path = this.pathprovider.MapPath(fileName, Folders.Images);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            };
            return path;
        }

    }
}
