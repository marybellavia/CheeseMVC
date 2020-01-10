using CheeseMVC.Models;
using Microsoft.EntityFrameworkCore;
namespace CheeseMVC.Data
{
    public class CheeseDbContext : DbContext
    {
        // tables in the database
        public DbSet<Cheese> Cheeses { get; set; }
        public DbSet<CheeseCategory> Categories { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<CheeseMenu> CheeseMenus { get; set; }


        /* this is the thing that does the join table and links the primary keys
         * to be foreign keys in the table
         */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CheeseMenu>()
                .HasKey(c => new { c.CheeseID, c.MenuID });
        }

        // code to configure tables on a Mac
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseSqlite("Data Source=CheeseMVC.db");

        // this would be the code if I was on a PC
        //public CheeseDbContext(DbContextOptions<CheeseDbContext> options)
        //    : base(options)
        //{ }
    }
}