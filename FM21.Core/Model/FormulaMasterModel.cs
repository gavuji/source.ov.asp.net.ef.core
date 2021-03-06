namespace FM21.Core.Model
{
    public class FormulaMasterModel
    {
        public int FormulaID { get; set; }
        public int SiteProductMapID { get; set; }
        public int SiteID { get; set; }
        public int ProductTypeID { get; set; }
        public string FormulaChangeNote { get; set; }
        public string TextureAppearanceNote { get; set; }
        public string ProjectInfoNote { get; set; }
        public string PackagingInfoForPowderNote { get; set; }
        public string DatasheetNote { get; set; }
        public string FormulaNote { get; set; }
        public string FormulaCode { get; set; }
        public string FormulaDescription { get; set; }
        public string FormulaTag { get; set; }
        public decimal? CoreWeight { get; set; }
        public decimal? CoatingWeight { get; set; }
        public decimal? ToppingWeight { get; set; }       
        public string FormulaStatusCode { get; set; }
        public string FormulaReference { get; set; }
        public decimal? SubGroupCoreWeight { get; set; }
        public decimal? SubGroupCoatingWeight { get; set; }
        public decimal? SubGroupToppingWeight { get; set; }
        public int? FormulaTypeID { get; set; }
        public string AltCode { get; set; }
        public int? RegulatoryCategoryID { get; set; }
        public string ProductDescription { get; set; }
        public string FlavourDescription { get; set; }
        public string Qualifier { get; set; }
        public int? FormulaProjectID { get; set; }
        public string FormulaProjectCode { get; set; }
        public int? FormulaCustomerID { get; set; }
        public string CustomerCode { get; set; }
        public int? NutrientFormatID { get; set; }
        public string NutrientFormatDesc { get; set; }
        public string AllergenCode { get; set; }
        public string Allergen { get; set; }
        public int? ServingUnit { get; set; }
        public decimal? CoatedRework { get; set; }
        public decimal? TrimRework { get; set; }
        public string OpsNote { get; set; }
        public string Preconditioning { get; set; }
        public int? DryIceUsage { get; set; }
        public bool? SyrupBinderHeated { get; set; }
        public bool? SyrupBinderShear { get; set; }
        public decimal? CookTemperature { get; set; }
        public decimal? TankTemperature { get; set; }
        public decimal? PipeTemperature { get; set; }
        public decimal? MixerTemperature { get; set; }
        public decimal? MaximumHoldingTime { get; set; }
        public string BrixRange { get; set; }
        public string SyrupAw { get; set; }
        public string SyrupPh { get; set; }
        public decimal? TransferTemperature { get; set; }
        public decimal? UsageTemperature { get; set; }
        public string FinalDoughTemperature { get; set; }
        public decimal? DoughWaterActivity { get; set; }
        public decimal? DoughDensity { get; set; }
        public string ReleaseAgent { get; set; }
        public string PKOPercentage { get; set; }
        public decimal? ActualWaterPercentage { get; set; }
        public decimal? WaterLoss { get; set; }
        public bool? IsRework { get; set; }
        public decimal? SlitterWidth { get; set; }
        public decimal? USMav { get; set; }
        public string USMavMin { get; set; }
        public string USMavMax { get; set; }
        public decimal? CanadaMav { get; set; }
        public string CanadaMavMin { get; set; }
        public string CanadaMavMax { get; set; }
        public decimal? DimensionsCoreLength { get; set; }
        public decimal? DimensionsCoreWidth { get; set; }
        public decimal? DimensionsCoreHeight { get; set; }
        public decimal? DimensionsCore { get; set; }
        public decimal? DimensionsLayer { get; set; }
        public decimal? DimensionsTopping { get; set; }
        public decimal? DimensionsOverrideLength { get; set; }
        public decimal? DimensionsOverrideWidth { get; set; }
        public decimal? DimensionsOverrideHeight { get; set; }
        public decimal? DimensionsOverrideCore { get; set; }
        public decimal? DimensionsOverrideLayer { get; set; }
        public decimal? DimensionsOverrideTopping { get; set; }
        public decimal? DimensionsFinishLength { get; set; }
        public decimal? DimensionsFinishWidth { get; set; }
        public decimal? DimensionsFinishHeight { get; set; }
        public decimal? DimensionsLengthLimit { get; set; }
        public decimal? DimensionsWidthLimit { get; set; }
        public decimal? DimensionsHeightLimit { get; set; }
        public decimal? CircleDiameter { get; set; }
        public decimal? CircleLineFactor { get; set; }
        public decimal? CircleCore { get; set; }
        public decimal? CircleLayer { get; set; }
        public decimal? CircleTopping { get; set; }
        public decimal? CircleLowerLimit { get; set; }
        public decimal? CircleUpperLimit { get; set; }
        public string QcSpecificationNote { get; set; }
        public string BarformatEquipment { get; set; }
        public string BarFormatForm { get; set; }
        public string BarFormatCore { get; set; }
        public string BarFormatTop { get; set; }
        public string BarFormatDescription { get; set; }
        public string BarFormatTopping { get; set; }
        public string BarFormatLocation { get; set; }
        public string BarFormatCoating { get; set; }
        public string BarFormatDrizzle { get; set; }
        public string BarFormatWoody { get; set; }
        public string PowderReconstitution { get; set; }
        public int? PowderLiquidAmountID { get; set; }
        public decimal? PowderBatchSize { get; set; }
        public int? PowderBlenderID { get; set; }
        public decimal? PowderBulkDensity { get; set; }
        public int? PowderFillWidth { get; set; }
        public int? PowderUnitID { get; set; }
        public decimal? S10CoreDoughYield { get; set; }
        public decimal? S65SyrupYield { get; set; }
        public decimal? S68_1LiquidBlendYield { get; set; }
        public decimal? S68_2LiquidBlendYield { get; set; }
        public decimal? S40CoreIngredientYield { get; set; }
        public decimal? S40ToppingIngredientYield { get; set; }
        public decimal? S60DryBlendYield { get; set; }
        public decimal? S63DryBlendYield { get; set; }
        public decimal? S20LayerDoughYield { get; set; }
        public decimal? S30CoatingYield { get; set; }
        public decimal? S10CoreDoughYieldLoss { get; set; }
        public decimal? S65SyrupYieldLoss { get; set; }
        public decimal? S68_1LiquidBlendYieldLoss { get; set; }
        public decimal? S68_2LiquidBlendYieldLoss { get; set; }
        public decimal? S40CoreIngredientYieldLoss { get; set; }
        public decimal? S40ToppingIngredientYieldLoss { get; set; }
        public decimal? S60DryBlendYieldLoss { get; set; }
        public decimal? S63DryBlendYieldLoss { get; set; }
        public decimal? S20LayerDoughYieldLoss { get; set; }
        public decimal? S30CoatingYieldLoss { get; set; }
        public string OPSNoteDescription { get; set; }
        public string DieNumber { get; set; }
        public decimal? KcalPer100g { get; set; }
        public decimal? PricePer100g { get; set; }
        public decimal? TotalFatInPercent { get; set; }
        public decimal? TotalCarbInPercent { get; set; }
        public decimal? TotalFiberInPercent { get; set; }
        public decimal? TotalSugerInPercent { get; set; }
        public decimal? TotalProteinInPercent { get; set; }
        public int? PrimaryProductionLineID { get; set; }

        public string RegulatoryCategoryDescription { get; set; }
        public string CustomerName { get; set; }
        public string LastChangedBy { get; set; }

        public decimal? CookieCutterCore { get; set; }
        public decimal? CookieCutterBottomLayer { get; set; }
        public decimal? CookieCutterTopLayer { get; set; }
        public decimal? CookieCutterMin { get; set; }
        public decimal? CookieCutterMax { get; set; }
        public decimal? CookieCutterOuterCore { get; set; }
        public decimal? CookieCutterInnerFilling { get; set; }
        public bool IsAllClaimVerify { get; set; }
    }
}