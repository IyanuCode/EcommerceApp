using Ecommerce.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data.Data
{
    //EcommerceStoreDbContext inherit from DbContext which is the central class in EF core for interacting with the database
    //It acts as a bridge between my C# code and the database handling query in,saving and tracking changes
    public class EcommerceStoreDbContext : DbContext
    {
        public EcommerceStoreDbContext(DbContextOptions<EcommerceStoreDbContext> options) : base(options)
        {

        }
        //DbSet<T> represent a table in the database where T is the entity type
        //EcommerceStores is the table for Ecommerce entities
        //"=null!;" syntax is a null-forgiving operator that tells the compiler not to worry even though the table is empty at compile time, it will be populated later.

        public DbSet<EcommerceStore> EcommerceStores { get; set; } = null!;        
    }
}