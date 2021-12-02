using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class NutrientMasterConfiguration : IEntityTypeConfiguration<NutrientMaster>
    {
        public void Configure(EntityTypeBuilder<NutrientMaster> builder)
        {
            builder.ToTable("NutrientMaster");
            builder.HasKey(e => e.NutrientID);

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.NutrientTypeMaster)
                .WithMany(p => p.NutrientMaster)
                .HasForeignKey(d => d.NutrientTypeID)
                .HasConstraintName("FK_NutrientTypeMaster");
        }
    }
}