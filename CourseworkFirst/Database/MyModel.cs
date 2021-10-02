using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CourseworkFirst
{
    public partial class MyModel : DbContext
    {
        public MyModel()
            : base(@"data source=COMPUTER\MSSQLSERVER1;
                    initial catalog=PartsAndProducts;
                    integrated security=True;
                    MultipleActiveResultSets=True;
                    App=EntityFramework")
        {
        }

        public virtual DbSet<Part> Parts { get; set; }
        public virtual DbSet<ProductPart> ProductParts { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Part>()
                .HasMany(e => e.ProductParts)
                .WithRequired(e => e.Part)
                .HasForeignKey(e => e.PartId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductParts)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.ProductId)
                .WillCascadeOnDelete(false);
        }
    }
}
