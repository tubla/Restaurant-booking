using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using RestaurantBookingApp.Core;
using RestaurantBookingApp.Core.ViewModels;

namespace RestaurantBookingApp.Data
{
    public class StorageRepository : IStorageRepository
    {
        private readonly RestaurantBookingDBContext _dbContext;

        public StorageRepository(RestaurantBookingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> UploadBlobContainer(IFormFile file, string azureStorageConnectionString)
        {
            var containerName = "rr-restaurant-storage-protected";  //"rr-restaurant-storage-public" or "rr-restaurant-storage-protected"
            var isUploadSuccess = false;
            var uploadEntity = new CustomerContactUploads
            {
                CreatedBy = "system",
                CreatedDate = DateTime.Now,
                IsProcessed = false
            };

            try
            {
                // Read the uploaded file into memory stream
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                // Set the position of the stream to beginning
                memoryStream.Position = 0;

                // Use the filename to create a unique blob name
                string blobName = $"{DateTime.Now.Ticks}_{file.FileName}";
                uploadEntity.FilePath = $"https://rrrestaurantstorage.blob.core.window.net/{containerName}/{blobName}"; // [rrrestaurantstorage] - name of the storage account from Azure 
                                                                                                                        //  followed by [blob.core.window.net] this is same for all blobs
                var blobClient = new BlobClient(azureStorageConnectionString, containerName, blobName);
                await blobClient.UploadAsync(memoryStream);
                isUploadSuccess = true;


                await _dbContext.CustomerContactUploads.AddAsync(uploadEntity);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                uploadEntity.ErrorMessage = ex.Message;
                if (!isUploadSuccess)
                {
                    await _dbContext.CustomerContactUploads.AddAsync(uploadEntity);
                    await _dbContext.SaveChangesAsync();
                }
            }

            return true;
        }

        public async Task<List<BlobDownloadModel>> GetBlob(string azureStorageConnectionString, string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureStorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(containerName);

            // Retrieve a list of all blobs in the container

            List<IListBlobItem> blobItems = new List<IListBlobItem>();
            BlobContinuationToken continuationToken = null!;

            do
            {
                var response = await cloudBlobContainer.ListBlobsSegmentedAsync(null, continuationToken);
                continuationToken = response.ContinuationToken;
                blobItems.AddRange(response.Results);
            } while (continuationToken != null);

            //Generate a SAS Token for each blob and construct the URLs with the SAS tokens

            List<BlobDownloadModel> blobUrlsWithSas = new List<BlobDownloadModel>();

            foreach (var blobItem in blobItems)
            {
                if (blobItem.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)blobItem;
                    string sasToken = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy
                    {
                        Permissions = SharedAccessBlobPermissions.Read,
                        SharedAccessExpiryTime = DateTime.Now.AddHours(1) // Set the expiry time for SAS token
                    });

                    string blobUrlWithSas = string.Format("{0}{1}", blob.Uri, sasToken);
                    blobUrlsWithSas.Add(new BlobDownloadModel()
                    {
                        Name = blob.Uri.ToString(),
                        DownloadLink = blobUrlWithSas
                    });
                }
            }

            return blobUrlsWithSas;
        }
    }
}
