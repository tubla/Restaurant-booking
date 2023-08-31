using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Core.ViewModels;

namespace RestaurantBookingApp.Data
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantBookingDBContext _dbContext;

        public RestaurantRepository(RestaurantBookingDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<RestaurantModel>> GetAllRestaurantAsync()
        {
            return await _dbContext.Restaurants
                        .OrderByDescending(r => r.Name)
                        .Select(r => new RestaurantModel
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Address = r.Address,
                            Email = r.Email,
                            Phone = r.Phone,
                            ImageUrl = r.ImageUrl
                        }).ToListAsync();
        }

        public async Task<IEnumerable<RestaurantBranchModel>> GetAllRestaurantBranchesByRestaurantIdAsync(int restaurantId)
        {
            return await _dbContext.RestaurantBranches.Where(b => b.RestaurantId == restaurantId)
                        .OrderByDescending(b => b.Name)
                        .Select(b => new RestaurantBranchModel
                        {
                            Id = b.Id,
                            Name = b.Name,
                            Address = b.Address,
                            RestaurantId = b.RestaurantId,
                            Email = b.Email,
                            Phone = b.Phone,
                            ImageUrl = b.ImageUrl

                        }).ToListAsync();
        }

        public async Task<IEnumerable<DiningTableWithTimeSlotstModel>> GetDiningTablesByBranchAsync(int branchId, DateTime date)
        {
            var diningTables = await _dbContext.DiningTables
                               .Where(dt => dt.RestaurantBranchId == branchId)
                               .SelectMany(dt => dt.TimeSlots, (dt, ts) => new
                               {
                                   dt.RestaurantBranchId,
                                   dt.TableName,
                                   dt.Capacity,
                                   ts.ReservationDay,
                                   ts.MealType,
                                   ts.TableStatus,
                                   ts.Id
                               })
                               .Where(ts => ts.ReservationDay == date)
                               .OrderBy(ts => ts.Id)
                               .ThenBy(ts => ts.MealType)
                               .ToListAsync();

            return diningTables.Select(dt => new DiningTableWithTimeSlotstModel
            {
                BranchId = dt.RestaurantBranchId,
                Capacity = dt.Capacity,
                MealType = dt.MealType,
                ResevationDay = dt.ReservationDay,
                TableName = dt.TableName,
                TableStatus = dt.TableStatus,
                TimeSlotId = dt.Id
            });
        }

        public async Task<IEnumerable<DiningTableWithTimeSlotstModel>> GetDiningTablesByBranchIdAsync(int branchId)
        {
            var data = await (from dt in _dbContext.DiningTables
                              join ts in _dbContext.TimeSlots on dt.Id equals ts.DiningTableId
                              where dt.RestaurantBranchId == branchId && ts.ReservationDay >= DateTime.Now.Date
                              orderby ts.Id, ts.MealType
                              select new DiningTableWithTimeSlotstModel()
                              {
                                  BranchId = branchId,
                                  Capacity = dt.Capacity,
                                  TableName = dt.TableName,
                                  MealType = ts.MealType,
                                  ResevationDay = ts.ReservationDay,
                                  TableStatus = ts.TableStatus,
                                  TimeSlotId = ts.Id
                              }).ToListAsync();
            return data;

        }
    }
}
