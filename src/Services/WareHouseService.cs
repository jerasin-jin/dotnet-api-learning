using Microsoft.EntityFrameworkCore;
using RestApiSample.Interfaces;
using RestApiSample.Models;

namespace RestApiSample.Services
{
    public class WareHouseService : IWareHouseService
    {

        private readonly ApiDbContext _dbContext;
        private readonly FormatResponseService _formatResponseService;

        public WareHouseService(ApiDbContext apiDbContext, FormatResponseService formatResponseService)
        {
            _dbContext = apiDbContext;
            _formatResponseService = formatResponseService;
        }

        public async Task initWareHouse(List<WareHouse> wareHouses)
        {
            // var wareHouse = new WareHouse
            // {
            //     ProductId = 1,
            //     Amount = 1000,
            //     CreatedBy = "admin"
            // };

            foreach (var wareHouse in wareHouses)
            {
                Console.WriteLine("wareHouse", wareHouse.ProductId);

                var getWareHouse = _dbContext.WareHouse.FirstOrDefault(u => u.ProductId == wareHouse.ProductId);

                Console.WriteLine("getWareHouse = {0}", getWareHouse);

                if (getWareHouse is null)
                {
                    Console.WriteLine("getWareHouse is null");
                    await createWareHouse(wareHouse);

                }
            }



            Console.WriteLine("initWareHouse is Running...");
        }


        public async Task<int> createWareHouse(WareHouse wareHouse)
        {
            _dbContext.Add(wareHouse);
            var saveWareHouse = await _dbContext.SaveChangesAsync();
            return saveWareHouse;
        }

        public async Task<IFormatResponseService> createWareHouse(string email, IWareHouse wareHouse)
        {
            var product = await _dbContext.Product.FindAsync(wareHouse.ProductId);

            if (product is null)
            {
                _formatResponseService._status = DefaultStatus.NotFound;
                _formatResponseService._value = null;
                return _formatResponseService;
            }

            var createWareHouse = new WareHouse
            {
                Amount = wareHouse.Amount,
                ProductId = wareHouse.ProductId,
                Product = product,
                CreatedBy = email
            };

            _dbContext.Add(createWareHouse);
            var saveWareHouse = await _dbContext.SaveChangesAsync();
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = saveWareHouse;
            return _formatResponseService;
        }


        public FormatResponseService getWareHouses()
        {

            var wareHouse = _dbContext.WareHouse.ToList();
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = wareHouse;
            return _formatResponseService;
        }

        public WareHouse? getWareHouse(int id)
        {
            var result = _dbContext.WareHouse.AsQueryable().FirstOrDefault(wareHouse => wareHouse.Id == id);

            return result;
        }

        public async Task<int?> updateWareHouse(int id, WareHouse wareHouse)
        {
            var result = _dbContext.WareHouse.AsQueryable().FirstOrDefault(wareHouse => wareHouse.Id == id);

            if (result is null)
            {
                return null;
            }

            var props = new WareHouse
            {
                Id = result.Id,
                ProductId = wareHouse.ProductId,
                Amount = wareHouse.Amount,
            };
            _dbContext.Update(props);
            return await _dbContext.SaveChangesAsync();

        }

        public async Task<int?> deleteWareHouse(int id)
        {
            var wareHouse = _dbContext.WareHouse.FirstOrDefault(wareHouse => wareHouse.Id == id);

            if (wareHouse is null)
            {
                return null;
            }

            _dbContext.Remove(wareHouse);
            return await _dbContext.SaveChangesAsync();
        }


        public IQueryable WareHouseProducts()
        {
            var result = from wareHouse in _dbContext.WareHouse
                         join product in _dbContext.Product on wareHouse.ProductId equals product.Id into Products
                         from m in Products.DefaultIfEmpty()
                         select new
                         {
                             Id = wareHouse.Id,
                             ProductId = wareHouse.ProductId,
                             ProductName = m.Name,
                             ProductActive = m.Active,
                             ProductPrice = m.Price,
                             ProductAmount = wareHouse.Amount
                         };

            return result;
        }

        public List<WareHouse>? ContextWareHouseProducts()
        {
            var result = _dbContext.WareHouse.Include(i => i.Product).ToList();

            return result;
        }

    }
}