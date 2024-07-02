using RestApiSample.Models;
using RestApiSample.Services;

namespace RestApiSample.Interfaces
{
    public class IWareHouse
    {
        public int Amount { get; set; }
        public int ProductId { get; set; }
    }

    public interface IWareHouseService
    {
        public Task initWareHouse(List<WareHouse> wareHouses);

        public Task<int> createWareHouse(WareHouse wareHouse);

        public FormatResponseService getWareHouses();

        public WareHouse? getWareHouse(int id);

        public Task<int?> updateWareHouse(int id, WareHouse wareHouse);

        public Task<int?> deleteWareHouse(int id);

        public IQueryable WareHouseProducts();

        public List<WareHouse>? ContextWareHouseProducts();
    }
}