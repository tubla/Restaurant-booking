using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantBookingApp.Core.ViewModels;
using RestaurantBookingApp.Service;
using StackExchange.Redis;

namespace RestaurantTableBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IDatabase _cache;

        public RestaurantController(IRestaurantService restaurantService, IConfiguration configuration)
        {
            _restaurantService = restaurantService;

            var lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                var redisCacheConnectionString = KeyVaultSecretReader.GetConnectionString(configuration, "RedisCacheConnectionString");
                var cacheConnectionString = configuration.GetConnectionString(redisCacheConnectionString);
                return ConnectionMultiplexer.Connect(cacheConnectionString!);
            });

            _cache = lazyConnection.Value.GetDatabase();
        }

        [HttpGet("restaurants")]
        [ProducesResponseType(200, Type = typeof(List<RestaurantModel>))]
        public async Task<ActionResult> GetAllRestaurantAsync()
        {
            //NOTE : For put/delete simple delete the key from cache, next time during get query cache will be refilled.
            /*
                    var keyName = $"restaurantUpdate-{restaurantId}";
                    var cachedData = cache.StringGet(keyName);
                    if(cachedData.HasValue)
                    {
                        await _cache.KeyDeleteAsync(keyName);
                    }             
             */


            IEnumerable<RestaurantModel> restaurantModels = new List<RestaurantModel>();
            var keyName = "getAllRestaurent";
            var cachedData = _cache.StringGet(keyName);
            if (cachedData.HasValue)
            {
                restaurantModels = JsonConvert.DeserializeObject<List<RestaurantModel>>(cachedData.ToString());
            }
            else
            {
                restaurantModels = await _restaurantService.GetAllRestaurantAsync();
                if (restaurantModels == null || !restaurantModels.Any())
                {
                    return NotFound(); // this returns 404 http status code
                }

                if (!cachedData.HasValue)
                {
                    _cache.StringSet(keyName, JsonConvert.SerializeObject(restaurantModels));
                }

            }
            return Ok(restaurantModels);
        }


        [HttpGet("branches/{restaurantId}")]
        [ProducesResponseType(200, Type = typeof(List<RestaurantBranchModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<RestaurantBranchModel>>> GetRestaurantBrancesByRestaurantIdAsync(int restaurantId)
        {
            IEnumerable<RestaurantBranchModel> restaurantBranchModels = new List<RestaurantBranchModel>();
            var keyName = $"getBranchesByRestaurantId-{restaurantId}";
            var cachedData = _cache.StringGet(keyName);
            if (cachedData.HasValue)
            {
                restaurantBranchModels = JsonConvert.DeserializeObject<IEnumerable<RestaurantBranchModel>>(cachedData.ToString());
            }
            else
            {
                restaurantBranchModels = await _restaurantService.GetAllRestaurantBranchesByRestaurantIdAsync(restaurantId);
                if (restaurantBranchModels == null || !restaurantBranchModels.Any())
                {
                    return NotFound(); // this returns 404 http status code
                }

                if (!cachedData.HasValue)
                {
                    _cache.StringSet(keyName, JsonConvert.SerializeObject(restaurantBranchModels));
                }
            }
            return Ok(restaurantBranchModels);
        }

        [HttpGet("diningtables/{branchId}")]
        [ProducesResponseType(200, Type = typeof(List<DiningTableWithTimeSlotstModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<DiningTableWithTimeSlotstModel>>> GetDiningTableByBranchAsync(int branchId)
        {
            var diningTables = await _restaurantService.GetDiningTablesByBranchIdAsync(branchId);
            if (diningTables == null || !diningTables.Any())
            {
                return NotFound(); // this returns 404 http status code
            }
            return Ok(diningTables);
        }

        [HttpGet("diningtables/{branchId}/{date}")]
        [ProducesResponseType(200, Type = typeof(List<DiningTableWithTimeSlotstModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<DiningTableWithTimeSlotstModel>>> GetDiningTableByBranchAndDateAsync(int branchId, DateTime date)
        {
            var diningTables = await _restaurantService.GetDiningTablesByBranchAsync(branchId, date);
            if (diningTables == null || !diningTables.Any())
            {
                return NotFound(); // this returns 404 http status code
            }
            return Ok(diningTables);
        }
    }
}
