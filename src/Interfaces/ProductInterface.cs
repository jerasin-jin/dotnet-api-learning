using System.ComponentModel.DataAnnotations;
using RestApiSample.Models;
using RestApiSample.Services;

namespace RestApiSample.Interfaces
{
    public class ProductDto
    {

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int Price { get; set; }

        public IFormFile? Img { get; set; }

    }

    public class UpdateProductDto
    {

        public string? Name { get; set; } = null!;

        public int? Price { get; set; }

        public IFormFile? Img { get; set; }

    }


    public interface IProductService
    {

        public Task initProduct(List<Product> product);

        public Task<FormatResponseService> createProduct(string email, ProductDto productDto);

        public Task<int> saveProduct(Product product);

        public Task<int> saveAsyncProducts(List<Product> product);

        public int saveProducts(List<Product> product);

        public FormatResponseService getProducts();

        public Task<FormatResponseService> getProduct(int id);

        public Task<FormatResponseService> updateProduct(int id, string email, UpdateProductDto updateProductDto);

        public Task<FormatResponseService> deleteProduct(int id);

        public string? uploadImgProduct(ProductDto productDto);

        public string? uploadImgProduct(UpdateProductDto updateProductDto);

        public void deleteImgProduct(string imgName);
    }
}