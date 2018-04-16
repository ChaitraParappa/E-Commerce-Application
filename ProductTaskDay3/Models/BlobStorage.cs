using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProductTaskDay3.Models
{
    public class BlobStorage
    {
        static string account = CloudConfigurationManager.GetSetting("StorageAccountName");
        static string key = CloudConfigurationManager.GetSetting("StorageAccountKey");
        public static class ConnectionString
        {
           
            public static CloudStorageAccount GetConnectionString()
            {
                string connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", account, key);
                return CloudStorageAccount.Parse(connectionString);
            }
        }

        public async Task<string> UploadImageAsync(HttpPostedFileBase imageToUpload)
        {
            string imageFullPath = null;
            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return null;
            }
            try
            {
                CloudStorageAccount cloudStorageAccount = ConnectionString.GetConnectionString();
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("orginalimages");

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                        );
                }

                string imageName = Guid.NewGuid().ToString() + "-" + Path.GetExtension(imageToUpload.FileName);

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                await cloudBlockBlob.UploadFromStreamAsync(imageToUpload.InputStream);

                imageFullPath = cloudBlockBlob.Uri.ToString();


                return imageFullPath;
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }

        public string UploadImageAsyncasByte(byte[] image, string filename)
        {
            try{



                StorageCredentials storageCredentials = new StorageCredentials(account,key);
                CloudStorageAccount acc = new CloudStorageAccount(storageCredentials, useHttps: true);
                CloudBlobClient client = acc.CreateCloudBlobClient();
                CloudBlobContainer cont = client.GetContainerReference("thumbnalimages");
                cont.CreateIfNotExists();
                cont.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                string imageName = Guid.NewGuid().ToString() + "-" + Path.GetExtension(filename);

                CloudBlockBlob cloudBlockBlob = cont.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = "image/" + filename;
                cloudBlockBlob.UploadFromByteArray(image, 0, image.Length);
                string blobURL = cloudBlockBlob.Uri.AbsoluteUri;
                if(blobURL != null)
                    return blobURL;
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}