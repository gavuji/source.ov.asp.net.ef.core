using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("IngredientMaster")]
    public class IngredientMaster
    {
        public IngredientMaster()
        {
            IngredientAllergenMapping = new HashSet<IngredientAllergenMapping>();
            IngredientNutrientMapping = new HashSet<IngredientNutrientMapping>();
            IngredientSitePartMapping = new HashSet<IngredientSitePartMapping>();
            IngredientSupplierMapping = new HashSet<IngredientSupplierMapping>();
        }

        [Key]
        public int IngredientID { get; set; }
        public string IRWPart { get; set; }
        public string ANJResearchCode { get; set; }
        public string ONTResearchCode { get; set; }
        public string S30SubAssemblyCode { get; set; }
        public string JDECode { get; set; }
        public string IngredientUsed { get; set; }
        public string RMDescription { get; set; }
        public string OriginCountry { get; set; }
        public string Viscosity { get; set; }
        public string Microns { get; set; }
        public int? HACCPRisk { get; set; }
        public string IngredientBreakDown { get; set; }
        public string IngredientList { get; set; }
        public string InternalXReference { get; set; }
        public string ExternalXReference { get; set; }
        public decimal? Sulfites { get; set; }
        public string OptimumStorageCondition { get; set; }
        public string GeneralNote { get; set; }
        public string AltCode { get; set; }
        public int IngredientCategoryID { get; set; }
        public string NutrientLink { get; set; }
        public string NutrientDescription { get; set; }
        public string DataSourceNote { get; set; }
        public string StorageCode { get; set; }
        public string PreConditionCode { get; set; }
        public string StorageInformation { get; set; }
        public string IPPBagSize { get; set; }
        public string AlertReview { get; set; }
        public bool? IsDataReviewedNutrient { get; set; }
        public bool? IsDataReviewedAllergen { get; set; }
        public DateTime? AlertReviewDate { get; set; }
        public string UsageAlert { get; set; }
        public string ExclusivityAlert { get; set; }
        public string AlertCustomerAbbr { get; set; }
        public string SupplierSeeNotes { get; set; }
        public string AlertNote { get; set; }
        public string Vegetarian { get; set; }
        public string Vegan { get; set; }
        public string PorkPresent { get; set; }
        public bool? IsRBST { get; set; }
        public bool? IsPreservatives { get; set; }
        public bool? IsRSPO { get; set; }
        public DateTime? RSPODate { get; set; }
        public string RSPOCertificateNumber { get; set; }
        public bool? IsSynthitic { get; set; }
        public bool? IsWADA { get; set; }
        public int? WADAYear { get; set; }
        public DateTime? GeneralDate { get; set; }
        public int? RegulatoryStatusID { get; set; }
        public int? SterilizationMethodID { get; set; }
        public int? GlutenStatusID { get; set; }
        public int? OrganicStatusID { get; set; }
        public string GMOStatus { get; set; }
        public DateTime? OrganicIssueDate { get; set; }
        public DateTime? OrganicExpireDate { get; set; }
        public int? HalalStatusID { get; set; }
        public bool? IsHalalStatement { get; set; }
        public string AllergenNote { get; set; }
        public string HighRiskCrossContSpecies { get; set; }
        public decimal? PrimaryUnitWeight { get; set; }
        public int? UnitOfMeasurementID { get; set; }
        public string CFRReferenceNo { get; set; }
        public string GRASNo { get; set; }
        public string HTSNo { get; set; }
        public string ShakleeCode { get; set; }
        public string CASNumber { get; set; }
        public DateTime? NAFTAExpireDate { get; set; }
        public string SupplierNote { get; set; }
        public int? ShelfLife { get; set; }
        public string Biological { get; set; }
        public string Chemical { get; set; }
        public string Physical { get; set; }
        public string ControlMechanismBiological { get; set; }
        public string ControlMechanismPhysical { get; set; }
        public string ControlMachanismChemical { get; set; }
        public string Micro { get; set; }
        public string ForeignMatter { get; set; }
        public DateTime? FSApproval { get; set; }
        public string TGApproval { get; set; }
        public string PackageOption { get; set; }
        public string SupplierPackage { get; set; }
        public string ProcurementDetail { get; set; }
        public string HandSort { get; set; }
        public decimal? Cadmium { get; set; }
        public decimal? Lead { get; set; }
        public decimal? Arsenic { get; set; }
        public decimal? Mercury { get; set; }
        public DateTime? NutrientDataChangeDate { get; set; }
        public string UsageBarLimit { get; set; }
        public string UsagePowderLimit { get; set; }
        public string UsageCanadaLimit { get; set; }
        public string GeneralRMDescription { get; set; }
        public bool IsAllergenPendingDocument { get; set; }
        public string ActiveNutrient { get; set; }
        public string NutrientActivity { get; set; }
        public string ClaimAmountUnit { get; set; }
        public string KosherAgency { get; set; }
        public int? GeneralDataChangedBy { get; set; }
        public string UsageRangeLimit { get; set; }
        public string Pro65Chemical { get; set; }
        public string SortingPart { get; set; }
        public bool? NeedInputForSulfites { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual RMStatusMaster RMStatusMasterGluten { get; set; }
        public virtual RMStatusMaster RMStatusMasterHalal { get; set; }
        public virtual IngredientCategoryMaster IngredientCategory { get; set; }
        public virtual RMStatusMaster RMStatusMasterOrganic { get; set; }
        public virtual RMStatusMaster RMStatusMasterRegulatory { get; set; }
        public virtual RMStatusMaster RMStatusMasterSterilization { get; set; }
        public virtual UnitOfMeasurementMaster UnitOfMeasurement { get; set; }
        public virtual ICollection<IngredientAllergenMapping> IngredientAllergenMapping { get; set; }
        public virtual ICollection<IngredientNutrientMapping> IngredientNutrientMapping { get; set; }
        public virtual ICollection<IngredientSitePartMapping> IngredientSitePartMapping { get; set; }
        public virtual ICollection<IngredientSupplierMapping> IngredientSupplierMapping { get; set; }
    }
}