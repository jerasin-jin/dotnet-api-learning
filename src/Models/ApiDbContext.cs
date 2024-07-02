using Microsoft.EntityFrameworkCore;

namespace RestApiSample.Models
{


    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // builder.Entity<Product>()
            //             .HasOne<WareHouse>(product => product.WareHouse)
            //             .WithOne(wareHouse => wareHouse.Product)
            //             .HasForeignKey<WareHouse>(wareHouse => wareHouse.ProductId);

            // https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/711
            builder.Entity<Transaction>().Property<int>("Id").ValueGeneratedOnAdd();
            builder.Entity<Transaction>().HasAlternateKey("Id");
        }


        public DbSet<User> User { get; set; } = null!;

        public DbSet<Product> Product { get; set; } = null!;

        public DbSet<WareHouse> WareHouse { get; set; } = null!;

        public DbSet<SaleOrder> SaleOrder { get; set; } = null!;

        public DbSet<Transaction> Transaction { get; set; } = null!;

        public DbSet<MasterData> MasterData { get; set; } = null!;
    }
}
