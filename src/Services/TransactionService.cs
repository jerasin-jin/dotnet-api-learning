using Microsoft.EntityFrameworkCore;
using RestApiSample.Interfaces;
using RestApiSample.Models;

namespace RestApiSample.Services
{
    public class TransactionService
    {
        private readonly ApiDbContext _dbContext;
        private readonly FormatResponseService _formatResponseService;
        private readonly MasterDataService _masterDataService;

        public TransactionService(ApiDbContext apiDbContext, FormatResponseService formatResponseService, MasterDataService masterDataService)
        {
            _dbContext = apiDbContext;
            _formatResponseService = formatResponseService;
            _masterDataService = masterDataService;
        }

        public async Task<IFormatResponseService> initTransaction(Transaction transaction)
        {
            var getTransaction = _dbContext.Transaction.FirstOrDefault(u => u.TransactionId == transaction.TransactionId);

            if (getTransaction is null)
            {

               

                var masterData = getTransactionNo.getObject().value as MasterData;

                if (masterData != null && masterData.Value != null)
                {
                    transaction.TransactionId = masterData.Value;
                    await createTransaction(transaction);

                    string[] subStr = masterData.Value.Split("-");
                    int runningNo = Int32.Parse(subStr[1]);
                    runningNo += 1;
                    masterData.Value = subStr[0] + runningNo.ToString();
                    await _masterDataService.updateMasterData(masterData);

                    Console.WriteLine("transaction = {0}", transaction.TransactionId);
                    _formatResponseService._status = DefaultStatus.Success;
                    _formatResponseService._value = transaction.TransactionId;
                }
                else
                {
                    _formatResponseService._status = DefaultStatus.BadRequest;
                    _formatResponseService._value = null;
                }

                return _formatResponseService;
            }

            Console.WriteLine("initTransaction is Running...");
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = null;
            return _formatResponseService;
        }

        public async Task<IFormatResponseService> createTransaction(Transaction transaction)
        {
            _dbContext.Add(transaction);
            var createTransaction = await _dbContext.SaveChangesAsync();
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = createTransaction;
            return _formatResponseService;
        }


    }
}