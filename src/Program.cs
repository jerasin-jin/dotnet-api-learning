using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestApiSample.Models;
using RestApiSample.Services;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.Extensions.FileProviders;
using RestApiSample.Mocks;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


// https://stackoverflow.com/questions/66720614/cannot-convert-from-string-to-microsoft-entityframeworkcore-serverversion
builder.Services.AddDbContext<ApiDbContext>(options => options.UseMySql(configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 11))));

var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddScoped<ProductService, ProductService>();
builder.Services.AddScoped<WareHouseService, WareHouseService>();
builder.Services.AddScoped<AuthCustomService, AuthCustomService>();
builder.Services.AddScoped<FormatResponseService, FormatResponseService>();
builder.Services.AddScoped<TransactionService, TransactionService>();
builder.Services.AddScoped<SaleOrderService, SaleOrderService>();
builder.Services.AddScoped<MasterDataService, MasterDataService>();

Console.WriteLine("configuration [{0}]", configuration["AppSettings:Tokens:Issuer"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["AppSettings:Tokens:Issuer"],
        ValidAudience = configuration["AppSettings:Tokens:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Tokens:Key"]!)),
        ClockSkew = TimeSpan.Zero,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // test connection Mysql
    try
    {
        connection.Open();
        connection.Close();
    }
    catch (MySqlException error)
    {
        Console.WriteLine(error);
    }


    using (var scope = app.Services.CreateScope())
    {

        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApiDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            Console.WriteLine("Database Migrate Running...");
            context.Database.Migrate();

        }
        else
        {
            Console.WriteLine("Not Have Migrate");
        }

        var userService = services.GetService<UserService>()!;
        var productService = services.GetService<ProductService>()!;
        var wareHouseService = services.GetService<WareHouseService>()!;
        var transactionService = services.GetService<TransactionService>()!;
        var saleOrderService = services.GetService<SaleOrderService>()!;
        var masterDataService = services.GetService<MasterDataService>()!;

        try
        {
            var userMock = new UserMock();
            var productMock = new ProductMock();
            var wareHouseMock = new WareHouseMock();
            var transactionMock = new TransactionMock();
            var saleOrderMock = new SaleOrderMock();
            var masterDataMock = new MasterDataMock();

            await masterDataService.initMasterData(masterDataMock._value);
            await userService.initUserAdmin(userMock._value);
            await productService.initProduct(productMock._value);
            await wareHouseService.initWareHouse(wareHouseMock._value);
            var transaction = await transactionService.initTransaction(transactionMock._value);
            var getTransaction = transaction.getObject().value as string;

            Console.WriteLine("getTransaction = {0}", getTransaction);

            if (getTransaction != null)
            {
                await saleOrderService.initSaleOrders(getTransaction, saleOrderMock._value);
            }

        }
        catch (Exception error)
        {
            Console.WriteLine("Error InitUserAdmin = ${0}", error);
        }
    }

}


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "src/wwwroot/upload")),
    RequestPath = "/images"
});

app.Run();


