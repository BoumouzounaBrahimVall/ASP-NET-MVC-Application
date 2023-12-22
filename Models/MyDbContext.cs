using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AmazonCloneMVC.Models
{
    public class MyDbContext : IdentityDbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        { }
        public MyDbContext()
        {

        }

        public DbSet<Produit> Produits { get; set; }
        public DbSet<Categorie> Categories { get; set; }


    }
}
