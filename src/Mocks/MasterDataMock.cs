using RestApiSample.Interfaces;
using RestApiSample.Models;

namespace RestApiSample.Mocks
{

    public class MasterDataMock : BaseMock<List<MasterData>>
    {

        public MasterDataMock()
        {
            _value = new List<MasterData>();
            _value.Add(new MasterData
            {
                Name = MasterType.TransactionNo.ToString(),
                Value = "SO-1000",
                CreatedBy = "admin",
            });
        }
    }
}

