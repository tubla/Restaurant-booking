using Microsoft.AspNetCore.Http;
using RestaurantBookingApp.Core.ViewModels;
using RestaurantBookingApp.Data;

namespace RestaurantBookingApp.Service
{
    public class StorageService : IStorageService
    {
        private readonly IStorageRepository _storageRepository;

        public StorageService(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public async Task<bool> UploadBlobContainer(IFormFile file, string azureStorage)
        {
            return await _storageRepository.UploadBlobContainer(file, azureStorage);
        }

        public async Task<List<BlobDownloadModel>> GetBlob(string azureStorageConnectionString, string containerName)
        {
            return await _storageRepository.GetBlob(azureStorageConnectionString, containerName);
        }
    }
}
