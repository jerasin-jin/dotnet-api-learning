using RestApiSample.Models;

namespace RestApiSample.Mocks
{
    public class WareHouseMock : BaseMock<List<WareHouse>>
    {

        public WareHouseMock()
        {
            _value = new List<WareHouse>();
            _value.Add(new WareHouse
            {
                ProductId = 1,
                Amount = 1000,
                CreatedBy = "admin"
            });
            _value.Add(new WareHouse
            {
                ProductId = 2,
                Amount = 1000,
                CreatedBy = "admin"
            });
        }
    }
}