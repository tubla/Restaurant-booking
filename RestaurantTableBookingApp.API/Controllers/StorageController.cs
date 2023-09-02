using Microsoft.AspNetCore.Mvc;
using RestaurantBookingApp.Core.ViewModels;
using RestaurantBookingApp.Service;

namespace RestaurantTableBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {

        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;

        public StorageController(IStorageService storageService, IConfiguration configuration)
        {
            _storageService = storageService;
            _configuration = configuration;
        }

        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var azureStorageConnectionString = KeyVaultSecretReader.GetConnectionString(_configuration, "StorageKeyVault");
            var isSuccess = await _storageService.UploadBlobContainer(file, azureStorageConnectionString);
            return Ok(isSuccess);
        }

        [HttpGet("download")]
        [ProducesResponseType(typeof(List<BlobDownloadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DownloadBlobs(string azureStorageContainerName = "rr-restaurant-storage-protected")
        {
            var azureStorageConnectionString = KeyVaultSecretReader.GetConnectionString(_configuration, "StorageKeyVault");
            var blobUrlsWithSas = await _storageService.GetBlob(azureStorageConnectionString, azureStorageContainerName);
            return Ok(blobUrlsWithSas);
        }
    }
}
