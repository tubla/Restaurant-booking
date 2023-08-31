using Microsoft.AspNetCore.Mvc;
using RestaurantBookingApp.Core.ViewModels;
using RestaurantBookingApp.Service;

namespace RestaurantTableBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet("restaurants")]
        [ProducesResponseType(200, Type = typeof(List<RestaurantModel>))]
        public async Task<ActionResult> GetAllRestaurantAsync()
        {
            return Ok(await _restaurantService.GetAllRestaurantAsync());
        }


        [HttpGet("branches/{restaurantId}")]
        [ProducesResponseType(200, Type = typeof(List<RestaurantBranchModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<RestaurantBranchModel>>> GetRestaurantBrancesByRestaurantIdAsync(int restaurantId)
        {
            var brances = await _restaurantService.GetAllRestaurantBranchesByRestaurantIdAsync(restaurantId);
            if (brances == null || !brances.Any())
            {
                return NotFound(); // this returns 404 http status code
            }
            return Ok(brances);
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
