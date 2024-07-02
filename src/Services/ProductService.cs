using RestApiSample.Interfaces;
using RestApiSample.Models;

namespace RestApiSample.Services
{
    public class ProductService : IProductService
    {

        private readonly ApiDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly FormatResponseService _formatResponseService;
        private readonly AuthCustomService _authCustomService;


        public ProductService(ApiDbContext dbContext, IWebHostEnvironment environment, FormatResponseService formatResponseService, AuthCustomService authCustomService)
        {
            _dbContext = dbContext;
            _environment = environment;
            _formatResponseService = formatResponseService;
            _authCustomService = authCustomService;

        }

        public async Task initProduct(List<Product> products)
        {


            foreach (var product in products)
            {
                var getProduct = _dbContext.Product.FirstOrDefault(u => u.Name == product.Name);

                Console.WriteLine("getProduct = {0}", getProduct);

                if (getProduct is null)
                {

                    Console.WriteLine("getProduct is null");
                    await saveProduct(product);
                }

            }

            Console.WriteLine("initProduct is Running...");
        }


        public async Task<FormatResponseService> createProduct(string email, ProductDto productDto)
        {
            string? imgName = null;
            if (productDto?.Img?.Length > 0)
            {
                Console.WriteLine("uploadImgProduct");
                imgName = uploadImgProduct(productDto);
            }

            var product = new Product
            {
                Name = productDto!.Name,
                Price = productDto.Price,
                CreatedBy = email
            };

            if (imgName != null)
            {
                product.Img = imgName;
            }

            var newProduct = await saveProduct(product);
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = newProduct;
            return _formatResponseService;
        }

        public async Task<int> saveProduct(Product product)
        {
            _dbContext.Add(product);
            var saveProduct = await _dbContext.SaveChangesAsync();
            return saveProduct;
        }

        public async Task<int> saveAsyncProducts(List<Product> product)
        {
            _dbContext.AddRange(product);
            var saveProduct = await _dbContext.SaveChangesAsync();
            return saveProduct;
        }

        public int saveProducts(List<Product> product)
        {
            _dbContext.AddRange(product);
            var saveProduct = _dbContext.SaveChanges();
            return saveProduct;
        }

        public string? uploadImgProduct(ProductDto productDto)
        {
            var imgName = productDto.Name + ".png";

            if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "src/wwwroot/upload/");
            }

            var rootPathImage = _environment.WebRootPath;
            Console.WriteLine("rootPathImage = {0}", rootPathImage);

            try
            {
                if (productDto?.Img?.Length > 0)
                {
                    if (!Directory.Exists(rootPathImage))
                    {


                        Directory.CreateDirectory(rootPathImage);
                    }

                    Console.WriteLine(
                        "_rootPathImage + imgName = {0}", rootPathImage + imgName
                    );
                    using (FileStream fileStream = System.IO.File.Create(rootPathImage + imgName))
                    {
                        productDto.Img.CopyTo(fileStream);
                        fileStream.Flush();

                    }

                    return imgName;
                }

                return null;

            }
            catch (Exception err)
            {
                throw err;
            }

        }

        public string? uploadImgProduct(UpdateProductDto updateProductDto)
        {
            var imgName = updateProductDto.Name + ".png";

            if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "src/wwwroot/upload/");
            }

            var rootPathImage = _environment.WebRootPath;
            Console.WriteLine("rootPathImage = {0}", rootPathImage);

            try
            {
                if (updateProductDto?.Img?.Length > 0)
                {
                    if (!Directory.Exists(rootPathImage))
                    {


                        Directory.CreateDirectory(rootPathImage);
                    }

                    Console.WriteLine(
                        "_rootPathImage + imgName = {0}", rootPathImage + imgName
                    );
                    using (FileStream fileStream = System.IO.File.Create(rootPathImage + imgName))
                    {
                        updateProductDto.Img.CopyTo(fileStream);
                        fileStream.Flush();

                    }

                    return imgName;
                }

                return null;

            }
            catch (Exception err)
            {
                throw err;
            }

        }


        public void deleteImgProduct(string imgName)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
                {
                    _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "src/wwwroot/upload/");
                }

                var rootPathImage = _environment.WebRootPath;

                var path = rootPathImage + imgName;


                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception error)
            {
                throw new IOException("Failed to delete file: '{0}'.", error);
            }
        }


        public FormatResponseService getProducts()
        {

            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = _dbContext.Product.ToList();

            Console.WriteLine("_status = {0}", _formatResponseService._status);


            return _formatResponseService;
        }

        public async Task<FormatResponseService> getProduct(int id)
        {
            var product = await _dbContext.Product.FindAsync(id);

            if (product is null)
            {
                _formatResponseService._status = DefaultStatus.NotFound;
                _formatResponseService._value = null;
                return _formatResponseService;
            }

            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = product;
            return _formatResponseService;
        }

        public async Task<FormatResponseService> updateProduct(int id, string email, UpdateProductDto updateProductDto)
        {

            var result = _dbContext.Product.AsQueryable().FirstOrDefault(product => product.Id == id);

            if (result is null)
            {
                _formatResponseService._status = DefaultStatus.NotFound;
                _formatResponseService._value = null;
                return _formatResponseService;
            }


            result.Id = result.Id;
            result.Name = updateProductDto.Name ?? result.Name;
            result.Price = updateProductDto.Price ?? result.Price;
            result.CreatedBy = result.CreatedBy;
            result.UpdatedBy = email;
            result.UpdatedAt = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");


            string? imgName = null;
            if (updateProductDto?.Img?.Length > 0)
            {
                updateProductDto.Name = updateProductDto.Name ?? result.Name;
                imgName = uploadImgProduct(updateProductDto);

                if (imgName != null)
                {
                    result.Img = imgName;
                }

            }

            await _dbContext.SaveChangesAsync();
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = result;
            return _formatResponseService;
        }

        public async Task<FormatResponseService> deleteProduct(int id)
        {
            var product = _dbContext.Product.FirstOrDefault(product => product.Id == id);

            if (product is null)
            {
                _formatResponseService._status = DefaultStatus.NotFound;
                _formatResponseService._value = null;
                return _formatResponseService;
            }

            _dbContext.Remove(product);
            var result = await _dbContext.SaveChangesAsync();

            if (product.Img != null)
            {
                deleteImgProduct(product.Img);
            }

            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = result;
            return _formatResponseService;
        }

    }
}