using RestApiSample.Models;

namespace RestApiSample.Mocks
{
    public class SaleOrderMock : BaseMock<List<SaleOrder>>
    {

        public SaleOrderMock()
        {
            _value = new List<SaleOrder>();
            _value.Add(new SaleOrder
            {
                Amount = 1,
                CreatedBy = "admin",
                ProductId = 1
            });

            _value.Add(new SaleOrder
            {
                Amount = 1,
                CreatedBy = "admin",
                ProductId = 2
            });
        }
    }
}