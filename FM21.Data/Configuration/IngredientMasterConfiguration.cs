using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class IngredientMasterConfiguration : IEntityTypeConfiguration<IngredientMaster>
    {
        public void Configure(EntityTypeBuilder<IngredientMaster> builder)
        {
            builder.ToTable("IngredientMaster");
            builder.HasKey(e => e.IngredientID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("((0))");

            builder.HasOne(d => d.IngredientCategory)
                .WithMany(p => p.IngredientMaster)
                .HasForeignKey(d => d.IngredientCategoryID)
                .HasConstraintName("FK_IngredientMaster_IngredientCategoryMaster");

            builder.HasOne(d => d.RMStatusMasterGluten)
                .WithMany(p => p.IngredientMasterGlutenStatus)
                .HasForeignKey(d => d.GlutenStatusID)
                .HasConstraintName("FK_IngredientMaster_RMStatusMaster_Gluten");

            builder.HasOne(d => d.RMStatusMasterHalal)
                .WithMany(p => p.IngredientMasterHalalStatus)
                .HasForeignKey(d => d.HalalStatusID)
                .HasConstraintName("FK_IngredientMaster_RMStatusMaster_Halal");

            builder.HasOne(d => d.RMStatusMasterOrganic)
                .WithMany(p => p.IngredientMasterOrganicStatus)
                .HasForeignKey(d => d.OrganicStatusID)
                .HasConstraintName("FK_IngredientMaster_RMStatusMaster_Organic");

            builder.HasOne(d => d.RMStatusMasterRegulatory)
                .WithMany(p => p.IngredientMasterRegulatoryStatus)
                .HasForeignKey(d => d.RegulatoryStatusID)
                .HasConstraintName("FK_IngredientMaster_RMStatusMaster_Regulatory");

            builder.HasOne(d => d.RMStatusMasterSterilization)
                .WithMany(p => p.IngredientMasterSterilization)
                .HasForeignKey(d => d.SterilizationMethodID)
                .HasConstraintName("FK_IngredientMaster_RMStatusMaster_Sterilization");

            builder.HasOne(d => d.UnitOfMeasurement)
                .WithMany(p => p.IngredientMaster)
                .HasForeignKey(d => d.UnitOfMeasurementID)
                .HasConstraintName("FK_IngredientMaster_UnitOfMeasurementMaster");
        }
    }
}