using FM21.Core;
using FM21.Entities;
using Microsoft.EntityFrameworkCore;

namespace FM21.Data
{
    public class AppEntities : DbContext, IAppEntities
    {
        #region Constructor
        public AppEntities(DbContextOptions options)
        {
        }
        #endregion

        #region DB Properties
        public virtual DbSet<AlertMaster> AlertMaster { get; set; }
        public virtual DbSet<AllergenMaster> AllergenMaster { get; set; }
        public virtual DbSet<AutoGenerateCode> AutoGenerateCode { get; set; }
        public virtual DbSet<BarFormatCodeMaster> BarFormatCodeMaster { get; set; }
        public virtual DbSet<BarFormatMaster> BarFormatMaster { get; set; }
        public virtual DbSet<BrokerMaster> BrokerMaster { get; set; }
        public virtual DbSet<ClaimMaster> ClaimMaster { get; set; }
        public virtual DbSet<CountryMaster> CountryMaster { get; set; }
        public virtual DbSet<CriteriaMaster> CriteriaMaster { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<DatasheetFormatMaster> DatasheetFormatMaster { get; set; }
        public virtual DbSet<ExtrusionDieMaster> ExtrusionDieMaster { get; set; }
        public virtual DbSet<FormulaClaimMapping> FormulaClaimMapping { get; set; }
        public virtual DbSet<FormulaCriteriaMapping> FormulaCriteriaMapping { get; set; }
        public virtual DbSet<FormulaDatasheetMapping> FormulaDatasheetMapping { get; set; }
        public virtual DbSet<FormulaDetailMapping> FormulaDetailMapping { get; set; }
        public virtual DbSet<FormulaMaster> FormulaMaster { get; set; }
        public virtual DbSet<FormulaRegulatoryCategoryMaster> FormulaRegulatoryCategoryMaster { get; set; }
        public virtual DbSet<FormulaSearchHistory> FormulaSearchHistory { get; set; }
        public virtual DbSet<FormulaStatusMaster> FormulaStatusMaster { get; set; }
        public virtual DbSet<FormulaTypeMaster> FormulaTypeMaster { get; set; }
        public virtual DbSet<FormulaTypeProductMapping> FormulaTypeProductMapping { get; set; }
        public virtual DbSet<HACCPMaster> HACCPMaster { get; set; }
        public virtual DbSet<IngredientAllergenMapping> IngredientAllergenMapping { get; set; }
        public virtual DbSet<IngredientCategoryMaster> IngredientCategoryMaster { get; set; }
        public virtual DbSet<IngredientMaster> IngredientMaster { get; set; }
        public virtual DbSet<IngredientNutrientMapping> IngredientNutrientMapping { get; set; }
        public virtual DbSet<IngredientSearchHistory> IngredientSearchHistory { get; set; }
        public virtual DbSet<IngredientSitePartMapping> IngredientSitePartMapping { get; set; }
        public virtual DbSet<IngredientSupplierMapping> IngredientSupplierMapping { get; set; }
        public virtual DbSet<InstructionCategoryMaster> InstructionCategoryMaster { get; set; }
        public virtual DbSet<InstructionGroupMaster> InstructionGroupMaster { get; set; }
        public virtual DbSet<InstructionMaster> InstructionMaster { get; set; }
        public virtual DbSet<KosherCodeMaster> KosherCodeMaster { get; set; }
        public virtual DbSet<NutrientMaster> NutrientMaster { get; set; }
        public virtual DbSet<NutrientTypeMaster> NutrientTypeMaster { get; set; }
        public virtual DbSet<PermissionMaster> PermissionMaster { get; set; }
        public virtual DbSet<PkoPercentageMaster> PkoPercentageMaster { get; set; }
        public virtual DbSet<ProductionLineMaster> ProductionLineMaster { get; set; }
        public virtual DbSet<ProductionLineMixerMapping> ProductionLineMixerMapping { get; set; }
        public virtual DbSet<ProductionMixerMaster> ProductionMixerMaster { get; set; }
        public virtual DbSet<PowderBlenderMaster> PowderBlenderMaster { get; set; }
        public virtual DbSet<PowderBlenderSiteMapping> PowderBlenderSiteMapping { get; set; }
        public virtual DbSet<PowderLiquidMaster> PowderLiquidMaster { get; set; }
        public virtual DbSet<UnitServingMaster> UnitServingMaster { get; set; }
        public virtual DbSet<PowderUnitServingSiteMapping> PowderUnitServingSiteMapping { get; set; }
        public virtual DbSet<ProductTypeMaster> ProductTypeMaster { get; set; }
        public virtual DbSet<ProjectMaster> ProjectMaster { get; set; }
        public virtual DbSet<ReleaseAgentMaster> ReleaseAgentMaster { get; set; }
        public virtual DbSet<RMStatusMaster> RMStatusMaster { get; set; }
        public virtual DbSet<ReconstitutionMaster> ReconstitutionMaster { get; set; }
        public virtual DbSet<RegulatoryMaster> RegulatoryMaster { get; set; }
        public virtual DbSet<RoleMaster> RoleMaster { get; set; }
        public virtual DbSet<RolePermissionMapping> RolePermissionMapping { get; set; }
        public virtual DbSet<SiteInstructionCategoryMapping> SiteInstructionCategoryMapping { get; set; }
        public virtual DbSet<SiteMaster> SiteMaster { get; set; }
        public virtual DbSet<SiteProductionLineMapping> SiteProductionLineMapping { get; set; }
        public virtual DbSet<SiteProductTypeMapping> SiteProductTypeMapping { get; set; }
        public virtual DbSet<SitterWidthMaster> SitterWidthMaster { get; set; }
        public virtual DbSet<StorageConditionMaster> StorageConditionMaster { get; set; }
        public virtual DbSet<SupplierMaster> SupplierMaster { get; set; }
        public virtual DbSet<UnitOfMeasurementMaster> UnitOfMeasurementMaster { get; set; }
        public virtual DbSet<UserMaster> UserMaster { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        public virtual DbSet<InternalQCMAVLookUpMaster> InternalQCMAVLookUpMaster { get; set; }
        #endregion

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(SecurityProvider.Decrypt(ApplicationConstants.DbConnectionString),
                                       // sqlServerOptions => sqlServerOptions.CommandTimeout(ApplicationConstants.SQLServerTimeOut));
            optionsBuilder.UseSqlServer(ApplicationConstants.DbConnectionString,
                                        sqlServerOptions => sqlServerOptions.CommandTimeout(ApplicationConstants.SQLServerTimeOut));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AlertMasterConfiguration());
            modelBuilder.ApplyConfiguration(new AllergenMasterConfiguration());
            modelBuilder.ApplyConfiguration(new BrokerMasterConfiguration());
            modelBuilder.ApplyConfiguration(new ClaimMasterConfiguration());
            modelBuilder.ApplyConfiguration(new CountryMasterConfiguration());
            modelBuilder.ApplyConfiguration(new CriteriaMasterConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new FormulaChangeCodeConfiguration());
            modelBuilder.ApplyConfiguration(new FormulaClaimMappingConfiguration());
            modelBuilder.ApplyConfiguration(new FormulaCriteriaMappingConfiguration());
            modelBuilder.ApplyConfiguration(new FormulaDetailMappingConfiguration());
            modelBuilder.ApplyConfiguration(new FormulaDatasheetMappingConfiguration());
            modelBuilder.ApplyConfiguration(new FormulaMasterConfiguration());
            modelBuilder.ApplyConfiguration(new FormulaProductionLineMappingConfiguration());
            modelBuilder.ApplyConfiguration(new FormulaRevisionConfiguration());
            modelBuilder.ApplyConfiguration(new FormulaTypeProductMappingConfiguration());
            modelBuilder.ApplyConfiguration(new HACCPMasterConfiguration());
            modelBuilder.ApplyConfiguration(new IngredientAllergenMappingConfiguration());
            modelBuilder.ApplyConfiguration(new IngredientCategoryMasterConfiguration());
            modelBuilder.ApplyConfiguration(new IngredientMasterConfiguration());
            modelBuilder.ApplyConfiguration(new IngredientNutrientMappingConfiguration());
            modelBuilder.ApplyConfiguration(new IngredientSitePartMappingConfiguration());
            modelBuilder.ApplyConfiguration(new IngredientSupplierMappingConfiguration());
            modelBuilder.ApplyConfiguration(new InstructionGroupMasterConfiguration());
            modelBuilder.ApplyConfiguration(new InstructionMasterConfiguration());
            modelBuilder.ApplyConfiguration(new KosherCodeMasterConfiguration());
            modelBuilder.ApplyConfiguration(new NutrientMasterConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionMasterConfiguration());
            modelBuilder.ApplyConfiguration(new PowderBlenderSiteMappingConfiguration());
            modelBuilder.ApplyConfiguration(new PowderUnitServingSiteMappingConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectMasterConfiguration());
            modelBuilder.ApplyConfiguration(new ProductionLineMixerMappingConfiguration());
            modelBuilder.ApplyConfiguration(new RMStatusMasterConfiguration());
            modelBuilder.ApplyConfiguration(new RegulatoryMasterConfiguration());
            modelBuilder.ApplyConfiguration(new RoleMasterConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionMappingConfiguration());
            modelBuilder.ApplyConfiguration(new SiteInstructionCategoryMappingConfiguration());
            modelBuilder.ApplyConfiguration(new SiteMasterConfiguration());
            modelBuilder.ApplyConfiguration(new SiteProcessCodeConfiguration());
            modelBuilder.ApplyConfiguration(new SiteProductTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new SiteProductionLineMappingConfiguration());
            modelBuilder.ApplyConfiguration(new SitterWidthMasterConfiguration());
            modelBuilder.ApplyConfiguration(new StorageConditionMasterConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierMasterConfiguration());
            modelBuilder.ApplyConfiguration(new UnitOfMeasurementMasterConfiguration());
            modelBuilder.ApplyConfiguration(new UserMasterConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}