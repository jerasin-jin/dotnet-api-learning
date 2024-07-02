using RestApiSample.Interfaces;
using RestApiSample.Models;

namespace RestApiSample.Services
{
    public class SaleOrderService
    {
        private readonly ApiDbContext _dbContext;
        private readonly FormatResponseService _formatResponseService;

        public SaleOrderService(ApiDbContext apiDbContext, FormatResponseService formatResponseService)
        {
            _dbContext = apiDbContext;
            _formatResponseService = formatResponseService;
        }

        public async Task initSaleOrders(string? transactionId, List<SaleOrder> saleOrders)
        {
            if (transactionId is null) return;
            await createSaleOrders(transactionId, saleOrders);
            Console.WriteLine("initSaleOrders is Running...");
        }

        public async Task<List<int>> createSaleOrders(string transactionId, List<SaleOrder> saleOrders)
        {
            var getProducts = _dbContext.SaleOrder.Where(u => u.TransactionId == transactionId).ToList();
            var insertIdList = new List<int>();

            foreach (var saleOrder in saleOrders)
            {
                var itemTarget = getProducts.FirstOrDefault(u => u.ProductId == saleOrder.ProductId);

                if (itemTarget == null)
                {
                    saleOrder.TransactionId = transactionId;
                    await createSaleOrder(saleOrder);
                }
                else if (itemTarget != null && itemTarget.Amount != saleOrder.Amount)
                {
                    await updateSaleOrder(saleOrder);
                }

            }

            return insertIdList;
        }

        public async Task<IFormatResponseService> createSaleOrder(SaleOrder saleOrder)
        {
            _dbContext.Add(saleOrder);
            var createSaleOrder = await _dbContext.SaveChangesAsync();
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = createSaleOrder;
            return _formatResponseService;
        }

        public async Task<IFormatResponseService> updateSaleOrder(SaleOrder saleOrder)
        {
            var result = await _dbContext.SaleOrder.FindAsync(saleOrder.Id);

            if (result is null)
            {
                _formatResponseService._status = DefaultStatus.NotFound;
                _formatResponseService._value = null;
                return _formatResponseService;
            }

            result.Amount = saleOrder.Amount;
            await _dbContext.SaveChangesAsync();
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = saleOrder;
            return _formatResponseService;
        }
    }
}