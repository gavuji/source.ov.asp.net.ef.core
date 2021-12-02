using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class InstructionMasterConfiguration : IEntityTypeConfiguration<InstructionMaster>
    {
        public void Configure(EntityTypeBuilder<InstructionMaster> builder)
        {
            builder.ToTable("InstructionMaster");
            builder.HasKey(k => k.InstructionMasterID);

            builder.Property(e => e.DescriptionEn)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode(false);

            builder.Property(e => e.DescriptionEs).HasMaxLength(150);
            builder.Property(e => e.DescriptionFr).HasMaxLength(150);


            builder.Property(e => e.IsActive).HasDefaultValueSql("(1)");
            builder.Property(e => e.IsDeleted).HasDefaultValueSql("(0)");

            builder.Property(e => e.CreatedOn)
               .HasColumnType("datetime")
               .HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.SiteProductMap)
                   .WithMany(p => p.InstructionMaster)
                   .HasForeignKey(d => d.SiteProductMapID)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_InstructionMasterSiteProductTypeMapping");

            builder.HasOne(d => d.InstructionCategory)
                    .WithMany(p => p.InstructionMaster)
                    .HasForeignKey(d => d.InstructionCategoryID)
                    .HasConstraintName("FK_InstructionMasterInstructionCategory");

            builder.HasOne(d => d.InstructionGroup)
                .WithMany(p => p.InstructionMaster)
                .HasForeignKey(d => d.InstructionGroupID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InstructionMasterInstructionGroupMaster");
        }
    }
}
