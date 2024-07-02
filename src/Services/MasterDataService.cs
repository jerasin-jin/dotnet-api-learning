using Microsoft.EntityFrameworkCore;
using RestApiSample.Interfaces;
using RestApiSample.Models;

namespace RestApiSample.Services
{
    public class MasterDataService
    {
        private readonly ApiDbContext _dbContext;
        private readonly FormatResponseService _formatResponseService;

        public MasterDataService(ApiDbContext dbContext, FormatResponseService formatResponseService)
        {
            _dbContext = dbContext;
            _formatResponseService = formatResponseService;
        }

        public async Task initMasterData(List<MasterData> masterDataList)
        {
            foreach (var masterData in masterDataList)
            {
                var getMasterType = _dbContext.MasterData.FirstOrDefault(u => u.Name == masterData.Name);

                if (getMasterType is null)
                {
                    await createTransactionRunningNumber(masterData);
                }
            }

            Console.WriteLine("initMasterData is Running...");
        }

        public async Task<IFormatResponseService> createTransactionRunningNumber(MasterData masterData)
        {
            _dbContext.Add(masterData);
            var createMasterData = await _dbContext.SaveChangesAsync();
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = createMasterData;
            return _formatResponseService;
        }

        public async Task<IFormatResponseService> getMasterDataByName(string name)
        {
            var result = await _dbContext.MasterData.FirstOrDefaultAsync(u => u.Name == name);

            if (result is null)
            {
                _formatResponseService._status = DefaultStatus.NotFound;
                _formatResponseService._value = null;
                return _formatResponseService;
            }

            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = result;
            return _formatResponseService;
        }

        public async Task<IFormatResponseService> updateMasterData(MasterData masterData)
        {
            var result = _dbContext.MasterData.AsQueryable().FirstOrDefault(item => item.Name == masterData.Name);

            if (result is null)
            {
                _formatResponseService._status = DefaultStatus.NotFound;
                _formatResponseService._value = null;
                return _formatResponseService;
            }

            result.Value = masterData.Value;
            await _dbContext.SaveChangesAsync();
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = result;
            return _formatResponseService;
        }

        public IFo555r geneRateTransactionId()
        {
            var getTransactionNo = await _masterDataService.getMasterDataByName(MasterType.TransactionNo.ToString());

            if (getTransactionNo.getObject().status != DefaultStatus.Success.ToString())
            {
                DefaultStatus defaultStatus = (DefaultStatus)System.Enum.Parse(typeof(DefaultStatus), getTransactionNo.getObject().status);

                _formatResponseService._status = defaultStatus;
                _formatResponseService._value = null;
                return _formatResponseService;
            }
        }
    }


}