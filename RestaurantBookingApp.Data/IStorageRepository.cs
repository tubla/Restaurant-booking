using Microsoft.AspNetCore.Http;
using RestaurantBookingApp.Core.ViewModels;

namespace RestaurantBookingApp.Data
{
    public interface IStorageRepository
    {
        Task<bool> UploadBlobContainer(IFormFile file, string azureStorage);
        Task<List<BlobDownloadModel>> GetBlob(string azureStorageConnectionString, string containerName);
    }
}