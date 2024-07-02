using RestApiSample.Models;

namespace RestApiSample.Mocks
{
    public class ProductMock : BaseMock<List<Product>>
    {

        public ProductMock()
        {
            _value = new List<Product>();
            _value.Add(new Product
            {
                Id = 1,
                Name = "TV",
                Price = 1500,
                CreatedBy = "admin",
                Img = "TV.png"
            });
            _value.Add(new Product
            {
                Id = 2,
                Name = "IPad",
                Price = 24000,
                CreatedBy = "admin",
            });


        }
    }
}