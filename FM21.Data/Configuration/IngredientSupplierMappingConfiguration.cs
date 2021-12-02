using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class IngredientSupplierMappingConfiguration : IEntityTypeConfiguration<IngredientSupplierMapping>
    {
        public void Configure(EntityTypeBuilder<IngredientSupplierMapping> builder)
        {
            builder.ToTable("IngredientSupplierMapping");
            builder.HasKey(e => e.IngredientSupplierID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.KosherExpireDate).HasColumnType("datetime");

            builder.Property(e => e.Price).HasColumnType("decimal(18, 4)");

            builder.Property(e => e.QuotedDate).HasColumnType("datetime");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");

            builder.HasOne(d => d.Broker)
                .WithMany(p => p.IngredientSupplierMapping)
                .HasForeignKey(d => d.BrokerID)
                .HasConstraintName("FK_IngredientSupplierMapping_BrokerMaster");

            builder.HasOne(d => d.Ingredient)
                .WithMany(p => p.IngredientSupplierMapping)
                .HasForeignKey(d => d.IngredientID)
                .HasConstraintName("FK_IngredientSupplierMapping_IngredientMaster");

            builder.HasOne(d => d.KosherCode)
                .WithMany(p => p.IngredientSupplierMapping)
                .HasForeignKey(d => d.KosherCodeID)
                .HasConstraintName("FK_IngredientSupplierMapping_KosherCodeMaster");

            builder.HasOne(d => d.Supplier)
                .WithMany(p => p.IngredientSupplierMapping)
                .HasForeignKey(d => d.ManufactureID)
                .HasConstraintName("FK_IngredientSupplierMapping_SupplierMaster");

            builder.HasOne(d => d.Site)
                .WithMany(p => p.IngredientSupplierMapping)
                .HasForeignKey(d => d.SiteID)
                .HasConstraintName("FK_IngredientSupplierMapping_SiteMaster");
        }
    }
}