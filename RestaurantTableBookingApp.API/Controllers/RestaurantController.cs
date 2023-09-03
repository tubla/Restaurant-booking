using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantBookingApp.Core.ViewModels;
using RestaurantBookingApp.Service;

namespace RestaurantTableBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IRedisCacheService _redisCacheService;

        //private readonly IDatabase _cache;

        public RestaurantController(IRestaurantService restaurantService, IConfiguration configuration, IRedisCacheService redisCacheService)
        {
            _restaurantService = restaurantService;
            _redisCacheService = redisCacheService;
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
            restaurantModels = _redisCacheService.GetDeserializedData<IEnumerable<RestaurantModel>>("getAllRestaurent")!;
            if (restaurantModels == default(IEnumerable<RestaurantModel>))
            {
                restaurantModels = await _restaurantService.GetAllRestaurantAsync();
                if (restaurantModels == null || !restaurantModels.Any())
                {
                    return NotFound(); // this returns 404 http status code
                }
                _redisCacheService.CacheData(keyName, JsonConvert.SerializeObject(restaurantModels));
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
            restaurantBranchModels = _redisCacheService.GetDeserializedData<IEnumerable<RestaurantBranchModel>>(keyName)!;
            if (restaurantBranchModels == default(IEnumerable<RestaurantBranchModel>))
            {
                restaurantBranchModels = await _restaurantService.GetAllRestaurantBranchesByRestaurantIdAsync(restaurantId);
                if (restaurantBranchModels == null || !restaurantBranchModels.Any())
                {
                    return NotFound(); // this returns 404 http status code
                }

                _redisCacheService.CacheData(keyName, JsonConvert.SerializeObject(restaurantBranchModels));
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
