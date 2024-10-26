using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplication261024_BooksAPI.Models;

namespace WebApplication261024_BooksAPI.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
