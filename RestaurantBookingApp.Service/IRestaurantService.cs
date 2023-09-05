using RestaurantBookingApp.Core.ViewModels;

namespace RestaurantBookingApp.Service
{
    public interface IRestaurantService
    {
        Task<PagedResponse<RestaurantModel>> GetAllRestaurantAsync(PagingParameters pagingParameters);

        Task<IEnumerable<RestaurantBranchModel>> GetAllRestaurantBranchesByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<DiningTableWithTimeSlotstModel>> GetDiningTablesByBranchAsync(int branchId, DateTime date);
        Task<IEnumerable<DiningTableWithTimeSlotstModel>> GetDiningTablesByBranchIdAsync(int branchId);
    }
}