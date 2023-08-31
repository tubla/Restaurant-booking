using RestaurantBookingApp.Core.ViewModels;

namespace RestaurantBookingApp.Data
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<RestaurantModel>> GetAllRestaurantAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        Task<IEnumerable<RestaurantBranchModel>> GetAllRestaurantBranchesByRestaurantIdAsync(int restaurantId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<IEnumerable<DiningTableWithTimeSlotstModel>> GetDiningTablesByBranchAsync(int branchId, DateTime date);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        Task<IEnumerable<DiningTableWithTimeSlotstModel>> GetDiningTablesByBranchIdAsync(int branchId);
    }
}
