CREATE FUNCTION GetColumnByParams
(
	 @selectAttributes VARCHAR(50),
	 @attributeType VARCHAR(50),
	 @callingSource TINYINT = 0 --1 For IngredientSearch, 2 For CustomReport
)
RETURNS VARCHAR(MAX)
AS
BEGIN

	DECLARE @returnValue VARCHAR(MAX);
 
	IF(@attributeType = 'IngredientMaster')
	BEGIN 

		SET @returnValue = CASE @selectAttributes
			WHEN 'Contracted' THEN 'CASE WHEN CHARINDEX(''CC'', IM.ExclusivityAlert) > 0 THEN ''Yes'' ELSE ''No'' END AS Contracted'
			WHEN 'IP_Proprietary' THEN 'CASE WHEN CHARINDEX(''IP'', IM.ExclusivityAlert) > 0 THEN ''Yes'' ELSE ''No'' END AS IP_Proprietary'  
			WHEN 'Customer_Supplied' THEN 'CASE WHEN CHARINDEX(''CS'', IM.ExclusivityAlert) > 0 THEN ''Yes'' ELSE ''No'' END AS Customer_Supplied'  
			WHEN 'CustAbbrev' THEN 'IM.AlertCustomerAbbr AS CustAbbrev'
			WHEN 'Copyright' THEN 'CASE WHEN CHARINDEX(''CP'', IM.ExclusivityAlert) > 0 THEN ''Yes'' ELSE ''No'' END AS Copyright' 
			WHEN 'PrimaryUnitWeight' THEN (CASE @callingSource WHEN 1 THEN '(CAST(IM.PrimaryUnitWeight AS VARCHAR(20))+'' ''+ISNULL(UM.MeasurementUnit,'''')) AS PrimaryUnitWeight'
															WHEN 2 THEN 'IM.PrimaryUnitWeight, UM.MeasurementUnit AS UOM' END)
			WHEN 'Exclusivity' THEN 'IM.ExclusivityAlert AS Exclusivity' 
			WHEN 'StorageInformation' THEN 'CAST(IM.StorageInformation AS VARCHAR(MAX)) AS StorageInformation'
			WHEN 'GeneralNote' THEN 'CAST(IM.GeneralNote AS VARCHAR(MAX)) AS GeneralNote'
			WHEN 'Category' THEN 'ICM.IngredientCategoryCode AS Category'  
			WHEN 'CategoryDescrp' THEN 'ICM.IngredientCategoryDescription AS CategoryDescrp' 
			WHEN 'WU' THEN 'IM.IngredientUsed AS WU' 
			WHEN 'ReviewStatus' THEN 'IM.AlertReview AS ReviewStatus' 
			WHEN 'ReviewAlert' THEN 'IM.AlertReview AS ReviewAlert' 
			WHEN 'NutrientsValidation' THEN 'CASE WHEN IM.IsDataReviewedNutrient = 1 THEN ''Yes'' ELSE ''No'' END AS NutrientsValidation'  
			WHEN 'AttributesValidation' THEN 'CASE WHEN IM.IsDataReviewedAllergen = 1 THEN ''Yes'' ELSE ''No'' END AS AttributesValidation'  
			WHEN 'SupplierNote' THEN 'CAST(IM.SupplierNote AS VARCHAR(MAX)) AS SupplierNote'
			WHEN 'CreatedDate' THEN 'CONVERT(VARCHAR, IM.CreatedOn, 1) AS CreatedDate' 
			WHEN 'UpdatedDate' THEN 'CONVERT(VARCHAR, IM.UpdatedOn, 1) AS UpdatedDate'
			WHEN 'CreatedBy' THEN 'UMCB.DisplayName AS CreatedBy'
			WHEN 'UpdatedBy' THEN 'UMUB.DisplayName AS UpdatedBy'
			WHEN 'IsRBST' THEN 'CASE WHEN IsRBST =  1 THEN ''Yes'' ELSE ''No'' END AS IsRBST'
			WHEN 'IsSynthitic' THEN 'CASE WHEN IsSynthitic =  1 THEN ''Yes'' ELSE ''No'' END AS IsSynthitic'
			WHEN 'IsRSPO' THEN 'CASE WHEN IsRSPO = 1 THEN ''Yes'' ELSE ''No'' END AS IsRSPO'
			WHEN 'DataSourceNote' THEN 'CAST(IM.DataSourceNote AS VARCHAR(MAX)) AS DataSourceNote'
			ELSE 'IM.'+ @selectAttributes END
 
	END

	RETURN @returnValue;

END