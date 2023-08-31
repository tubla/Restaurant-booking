using RestaurantBookingApp.Core.ViewModels;
using RestaurantBookingApp.Data;

namespace RestaurantBookingApp.Service
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }
        public async Task<IEnumerable<RestaurantModel>> GetAllRestaurantAsync()
        {
            return await _restaurantRepository.GetAllRestaurantAsync();
        }

        public async Task<IEnumerable<RestaurantBranchModel>> GetAllRestaurantBranchesByRestaurantIdAsync(int restaurantId)
        {
            return await _restaurantRepository.GetAllRestaurantBranchesByRestaurantIdAsync(restaurantId);
        }

        public async Task<IEnumerable<DiningTableWithTimeSlotstModel>> GetDiningTablesByBranchAsync(int branchId, DateTime date)
        {
            return await _restaurantRepository.GetDiningTablesByBranchAsync(branchId, date);
        }

        public async Task<IEnumerable<DiningTableWithTimeSlotstModel>> GetDiningTablesByBranchIdAsync(int branchId)
        {
            return await _restaurantRepository.GetDiningTablesByBranchIdAsync(branchId);
        }
    }
}
