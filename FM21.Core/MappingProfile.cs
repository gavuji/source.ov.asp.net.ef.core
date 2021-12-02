using AutoMapper;
using FM21.Core.Model;
using FM21.Entities;
using System.Linq;

namespace FM21.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Entity To Model
            CreateMap<RoleMaster, RoleMasterModel>();
            CreateMap<SiteProductTypeMapping, SiteProductTypeModel>()
                .ForMember(d => d.SiteCode, opt => opt.MapFrom(s => s.Site.SiteCode))
                .ForMember(d => d.SiteName, opt => opt.MapFrom(s => s.Site.SiteDescription))
                .ForMember(d => d.ProductType, opt => opt.MapFrom(s => s.ProductType.ProductType))
                .ForMember(d => d.SiteProductType, opt => opt.MapFrom(s => string.Format("{0} {1}", s.Site.SiteCode, s.ProductType.ProductType)));
            CreateMap<InstructionMaster, InstructionMasterModel>()
                .ForMember(d => d.SiteProductMap, opt => opt.MapFrom(s => string.Format("{0} {1}", s.SiteProductMap.Site.SiteCode, s.SiteProductMap.ProductType.ProductType)))
                .ForMember(d => d.InstructionCategory, opt => opt.MapFrom(s => s.InstructionCategory.InstructionCategory))
                .ForMember(d => d.InstructionGroup, opt => opt.MapFrom(s => s.InstructionGroup.InstructionGroupName));
            CreateMap<UserMaster, UserMasterModel>();
            CreateMap<Customer, CustomerModel>();
            CreateMap<SupplierMaster, SupplierMasterModel>();
            CreateMap<RegulatoryMaster, RegulatoryModel>()
                .ForMember(d => d.Nutrient, opt => opt.MapFrom(s => s.NutrientMaster.Name))
                .ForMember(d => d.Unit, opt => opt.MapFrom(s => s.NutrientMaster.UnitOfMeasurement.MeasurementUnit));
            CreateMap<AllergenMaster, AllergenMasterModel>();
            CreateMap<ProjectMaster, ProjectMasterModel>().ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.CustomerMaster.Name));
            CreateMap<FormulaMaster, FormulaMasterModel>()
                .ForMember(d => d.SiteID, opt => opt.MapFrom(s => s.SiteProductMap.SiteID))
                .ForMember(d => d.ProductTypeID, opt => opt.MapFrom(s => s.SiteProductMap.ProductTypeID))
                .ForMember(d => d.FormulaProjectCode, opt => opt.MapFrom(s => (string.IsNullOrEmpty(s.FormulaProject.ProjectCode.ToString()) ? s.FormulaProject.NPICode : s.FormulaProject.ProjectCode.ToString()) + " \\ " + s.FormulaProject.CustomerMaster.Name + " \\ " + s.FormulaProject.ProjectDescription))
                .ForMember(d => d.NutrientFormatDesc, opt => opt.MapFrom(s => s.NutrientFormat.DatasheetDescription))
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.FormulaProject.CustomerMaster.Name))
                .ForMember(d => d.RegulatoryCategoryDescription, opt => opt.MapFrom(s => s.RegulatoryCategory.FormulaRegulatoryCategoryDescription));
            CreateMap<IngredientMaster, IngredientMasterModel>()
                .ForMember(d => d.LACCode, opt => opt.MapFrom(s => s.IngredientSitePartMapping.FirstOrDefault(o => o.SiteID == SiteCode.LAC.GetHashCode()).PartNumber))
                .ForMember(d => d.ANJCode, opt => opt.MapFrom(s => s.IngredientSitePartMapping.FirstOrDefault(o => o.SiteID == SiteCode.ANJ.GetHashCode()).PartNumber))
                .ForMember(d => d.ANACode, opt => opt.MapFrom(s => s.IngredientSitePartMapping.FirstOrDefault(o => o.SiteID == SiteCode.ANA.GetHashCode()).PartNumber))
                .ForMember(d => d.SLCCode, opt => opt.MapFrom(s => s.IngredientSitePartMapping.FirstOrDefault(o => o.SiteID == SiteCode.SLC.GetHashCode()).PartNumber))
                .ForMember(d => d.IngredientCategoryCode, opt => opt.MapFrom(s => s.IngredientCategory.IngredientCategoryCode));
            CreateMap<IngredientNutrientMapping, NutrientModel>();
            CreateMap<NutrientMaster, NutrientModel>();
            CreateMap<FormulaDetailMapping, FormulaDetailsModel>();
            #endregion

            #region Model to Entity
            CreateMap<RoleMasterModel, RoleMaster>();
            CreateMap<InstructionMasterModel, InstructionMaster>()
                .ForMember(dest => dest.GroupDisplayOrder, act => act.Ignore())
                .ForMember(dest => dest.GroupItemDisplayOrder, act => act.Ignore())
                .ForMember(dest => dest.SiteProductMap, act => act.Ignore())
                .ForMember(dest => dest.InstructionCategory, act => act.Ignore())
                .ForMember(dest => dest.InstructionGroup, act => act.Ignore());
            CreateMap<UserMasterModel, UserMaster>()
                .ForMember(dest => dest.UserRole, act => act.Ignore());
            CreateMap<CustomerModel, Customer>()
                .ForMember(dest => dest.CustomerAbbreviation1, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.CustomerAbbreviation1) ? null : src.CustomerAbbreviation1))
                .ForMember(dest => dest.CustomerAbbreviation2, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.CustomerAbbreviation2) ? null : src.CustomerAbbreviation2));
            CreateMap<RolePermissionAccess, RolePermissionMapping>()
                .ForMember(dest => dest.PermissionType, opt => opt.MapFrom(src => src.isAccess ? 1 : 0));
            CreateMap<SupplierMasterModel, SupplierMaster>();
            CreateMap<RegulatoryModel, RegulatoryMaster>();
            CreateMap<AllergenMasterModel, AllergenMaster>();
            CreateMap<ProjectMasterModel, ProjectMaster>()
                .ForMember(dest => dest.NPICode, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.NPICode) ? null : src.NPICode));
            CreateMap<FormulaMasterModel, FormulaMaster>();
            CreateMap<IngredientSupplierModel, IngredientSupplierMapping>();
            CreateMap<IngredientMasterModel, IngredientMaster>();
            CreateMap<NutrientModel, IngredientNutrientMapping>();
            CreateMap<FormulaDetailsModel, FormulaDetailMapping>()
                .ForMember(dest => dest.InstructionDescription, opt => opt.MapFrom(s => s.Description));
            CreateMap<ClaimModel, FormulaClaimMapping>();
            #endregion
        }
    }
}