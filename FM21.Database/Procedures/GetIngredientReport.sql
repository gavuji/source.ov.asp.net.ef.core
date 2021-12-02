CREATE PROCEDURE GetIngredientReport
(
	@reportType VARCHAR(50),
	@siteIDs VARCHAR(50) = '',
	@productTypeID VARCHAR(50) = ''
)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @query VARCHAR(MAX);
	DECLARE @selectStatement VARCHAR(MAX);
	DECLARE @fromStatement VARCHAR(MAX);
	DECLARE @whereStatement VARCHAR(MAX);
	DECLARE @columns NVARCHAR(MAX) = '', @pivColumn NVARCHAR(MAX) = '', @displayColumn NVARCHAR(MAX) = '';
	DECLARE @Nutrient VARCHAR(1000) = 'Total Fat,Saturated Fat,Total Carbohydrate,Carb Kcal Factor,Soluble fiber,Not Approved Fiber,Sugar Alcohol (incl glycerine),Added Sugars,Protein,Moisture,Ash,Non-Proximate Nutrient';
	DECLARE @nutrientCol TABLE(NutrientId INT, ItemValue VARCHAR(100));
	DECLARE @finalSiteIDs VARCHAR(50) = '';

	SET @selectStatement = 'SELECT IM.RMDescription, IM.JDECode';
	SET @fromStatement = 'FROM IngredientMaster IM';
	SET @whereStatement = 'WHERE (IM.IsActive = 1 AND IM.IsDeleted = 0)';

	IF(@siteIDs = '0')
	BEGIN
		SELECT @finalSiteIDs = STRING_AGG(SiteID, ',') FROM SiteMaster WHERE IsActive = 1;
	END
	ELSE
	BEGIN
		SET @finalSiteIDs = @siteIDs;
	END

	IF(@siteIDs != '0')
	BEGIN

		SET @fromStatement += CHAR(13) + 'INNER JOIN FormulaDetailMapping FDM ON FDM.ReferenceID = IM.IngredientID AND FDM.ReferenceType = 2
								INNER JOIN FormulaMaster FM ON FM.FormulaID = FDM.FormulaID
								INNER JOIN SiteProductTypeMapping SPTM ON SPTM.SiteProductMapID = FM.SiteProductMapID';

		SET @whereStatement += CHAR(13) + 'AND SPTM.SiteID IN (' + @finalSiteIDs + ')';

	END
	
	SELECT
		@fromStatement += (CASE WHEN SM.SiteID != 1 THEN CHAR(13) + 'LEFT JOIN IngredientSitePartMapping ISPM_' + SM.SiteCode + ' ON ISPM_' + SM.SiteCode + '.IngredientID = IM.IngredientID AND ISPM_' + SM.SiteCode + '.SiteID = ' + CAST(SM.SiteID AS VARCHAR(10)) ELSE '' END +
						 CHAR(13) + 'LEFT JOIN IngredientSupplierMapping ISM_' + SM.SiteCode + ' ON ISM_' + SM.SiteCode + '.IngredientID = IM.IngredientID AND ISM_' + SM.SiteCode + '.SiteID = ' + CAST(SM.SiteID AS VARCHAR(10)) +
						 CHAR(13) + 'LEFT JOIN SupplierMaster SM_' + SM.SiteCode + ' ON SM_' + SM.SiteCode + '.SupplierID = ISM_' + SM.SiteCode + '.ManufactureID'),
		@selectStatement += CASE WHEN SM.SiteID != 1 THEN ', ISPM_' + SM.SiteCode + '.PartNumber AS ' + SM.SiteCode + '_PartCode' ELSE '' END + 
							', ISM_' + SM.SiteCode + '.ManufactureDescription AS ' + SM.SiteCode + '_ManufactureDesc, 
							SM_' + SM.SiteCode + '.SupplierName AS ' + SM.SiteCode + '_Manufacture'
	FROM SiteMaster SM
	INNER JOIN dbo.Split(@finalSiteIDs, ',') SD ON SD.ItemValue = SM.SiteID;

	IF(UPPER(@reportType) = 'ALLERGEN')
	BEGIN

		SET @selectStatement += ', IM.IsAllergenPendingDocument, IM.HighRiskCrossContSpecies AS XC_Allergen, IM.AllergenNote, IM.GeneralNote, IM.IngredientList, Allergen.Allergen';
		SET @fromStatement += CHAR(13) + 'OUTER APPLY (
			SELECT STRING_AGG(AM.AllergenName, '', '') AS Allergen
			FROM AllergenMaster AM
			INNER JOIN IngredientAllergenMapping IAM ON IAM.AllergenID = AM.AllergenID
			WHERE IAM.IngredientID = IM.IngredientID
		) AS Allergen';
		
	END
	ELSE IF(UPPER(@reportType) = 'ORGANIC/GMO')
	BEGIN
	
		SET @selectStatement += ', SM_O.RMStatus AS OrganicStatus, IM.GMOStatus, IM.OrganicIssueDate, IM.OrganicExpireDate'; 
		SET @fromStatement += CHAR(13) + 'LEFT JOIN RMStatusMaster SM_O ON SM_O.RMStatusMasterID = IM.OrganicStatusID';
		
	END
	ELSE IF(UPPER(@reportType) = 'VALIDATION')
	BEGIN

		SET @selectStatement += ', ICM.IngredientCategoryCode, IM.NutrientLink, IM.IngredientUsed AS WU, IM.IngredientBreakDown, IM.IngredientList, 
		IM.HighRiskCrossContSpecies AS XC_Allergen, IM.AllergenNote, IM.AlertReviewDate, IM.UpdatedOn, IM.DataSourceNote, 
		Allergen.Allergen'; 
		SET @fromStatement += CHAR(13) + 'INNER JOIN IngredientCategoryMaster ICM ON ICM.IngredientCategoryID = IM.IngredientCategoryID
		OUTER APPLY ( 
			SELECT STRING_AGG(AM.AllergenName, '', '') AS Allergen
			FROM AllergenMaster AM
			INNER JOIN IngredientAllergenMapping IAM ON IAM.AllergenID = AM.AllergenID
			WHERE IAM.IngredientID = IM.IngredientID
		) AS Allergen';

	END
	ELSE IF(UPPER(@reportType) = 'NUTR MACROS')
	BEGIN
		
		DECLARE @colName VARCHAR(100);

		INSERT INTO @nutrientCol 
		SELECT NM.NutrientID, funRes.ItemValue 
		FROM dbo.Split(@Nutrient, ',') AS funRes 
		INNER JOIN NutrientMaster NM ON NM.Name = funRes.ItemValue;

		SELECT
		   @colName = QUOTENAME(REPLACE(REPLACE(REPLACE(REPLACE(ItemValue, ' ', '_'), '-', '_'), '(', ''), ')', '')),
		   @columns += ', ' + QUOTENAME(NutrientID),
		   @pivColumn += ', MIN(' + QUOTENAME(NutrientID) + ') AS ' + @colName,
		   @displayColumn += ', ' + @colName 
		FROM
		   @nutrientCol;

		SET @selectStatement = ';WITH CTE  
			 AS 
				  (
					 SELECT
						IngredientID,
						' + STUFF(@columns, 1, 2, '') + ' 
					 FROM
						IngredientNutrientMapping AS PN PIVOT (MIN(NutrientValue) FOR NutrientID IN 
						(
						   ' + STUFF(@columns, 1, 2, '') + '
						)
				) AS p 
			)
			' + @selectStatement + ' ,ICM.IngredientCategoryCode, IM.NutrientLink, IM.IngredientUsed AS WU ' + @displayColumn +
			',IM.IngredientList,IM.UsageAlert, IM.AlertCustomerAbbr,
			CASE WHEN CHARINDEX(''CC'', IM.ExclusivityAlert) > 0 THEN ''Yes'' ELSE ''No'' END AS Contracted,  
			CASE WHEN CHARINDEX(''IP'', IM.ExclusivityAlert) > 0 THEN ''Yes'' ELSE ''No'' END AS IP_Proprietary,  
			CASE WHEN CHARINDEX(''CS'', IM.ExclusivityAlert) > 0 THEN ''Yes'' ELSE ''No'' END AS Customer_Supplied,  
			CASE WHEN CHARINDEX(''CP'', IM.ExclusivityAlert) > 0 THEN ''Yes'' ELSE ''No'' END AS Copyright ';
 
		SET @fromStatement += CHAR(13) + 'LEFT JOIN (SELECT IngredientID' + @pivColumn + ' 
		   FROM CTE 
		   GROUP BY IngredientID
		) AS INM 
		ON INM.IngredientID = IM.IngredientID INNER JOIN IngredientCategoryMaster ICM ON ICM.IngredientCategoryID = IM.IngredientCategoryID'; 
		
	END
	ELSE IF(UPPER(@reportType) = 'EXCLUSIVITY')
	BEGIN

		SET @selectStatement += ', ICM.IngredientCategoryCode, IM.NutrientLink, IM.IngredientUsed AS WU, IM.UsageAlert, IM.AlertCustomerAbbr, 
		CASE WHEN CHARINDEX(''CC'', IM.ExclusivityAlert) > 0 THEN ''Yes'' ELSE ''No'' END AS Contracted,
		CASE WHEN CHARINDEX(''IP'', IM.ExclusivityAlert) > 0 THEN ''Yes'' ELSE ''No'' END AS IP_Proprietary,
		CASE WHEN CHARINDEX(''CS'', IM.ExclusivityAlert) > 0 THEN ''Yes'' ELSE ''No'' END AS Customer_Supplied,
		CASE WHEN CHARINDEX(''CP'', IM.ExclusivityAlert) > 0 THEN ''Yes'' ELSE ''No'' END AS Copyright'; 
		SET @fromStatement += CHAR(13) + 'INNER JOIN IngredientCategoryMaster ICM ON ICM.IngredientCategoryID = IM.IngredientCategoryID';
		
	END
	ELSE IF(UPPER(@reportType) = 'FOOD SAFETY')
	BEGIN

		SET @fromStatement += CHAR(13) + 'INNER JOIN IngredientCategoryMaster ICM ON ICM.IngredientCategoryID = IM.IngredientCategoryID
									 LEFT JOIN RMStatusMaster RM on IM.SterilizationMethodID = RM.RMStatusMasterID'; 

		SET @selectStatement +=', ICM.IngredientCategoryCode, IM.NutrientLink, IM.FSApproval, IM.TGApproval, IM.AlertCustomerAbbr, IM.PreConditionCode, 
							IM.OptimumStorageCondition, CASE WHEN CHARINDEX(''CS'', IM.ExclusivityAlert) > 0 THEN ''Yes'' ELSE ''No'' END AS Customer_Supplied,
							IM.Biological, IM.ControlMechanismBiological AS BioControl, IM.Chemical, IM.ControlMachanismChemical AS ChemicalControl, IM.Physical, 
							IM.ControlMechanismPhysical AS PhysicalControl, IM.Cadmium, IM.Lead, IM.Arsenic, IM.Mercury, RM.RMStatus AS Lethality, IM.Micro, IM.ForeignMatter ' 
		
	END
	ELSE IF(UPPER(@reportType) = 'COO')
	BEGIN

		SET @selectStatement += ', IM.NAFTAExpireDate AS USMCA_Exp, IM.OriginCountry'; 
		
	END
	ELSE IF(UPPER(@reportType) = 'HALAL')
	BEGIN

		DECLARE @nutrientID INT = ISNULL(( SELECT TOP 1 NutrientID FROM NutrientMaster WHERE [Name] = 'Alcohol' ), 0);

		SET @selectStatement += ', SM_H.RMStatus AS HalalStatus, IM.IsHalalStatement AS H_Questionnaire, INM.NutrientValue AS Alcohol, IM.GeneralNote';
		SET @fromStatement += CHAR(13) + 'LEFT JOIN RMStatusMaster SM_H ON SM_H.RMStatusMasterID = IM.HalalStatusID' + 
						CHAR(13) + 'LEFT JOIN IngredientNutrientMapping INM ON INM.IngredientID = IM.IngredientID AND INM.NutrientID = ' + CAST(@nutrientID AS VARCHAR(20))
		
	END
	ELSE IF(UPPER(@reportType) = 'WADA')
	BEGIN

		SET @selectStatement += ', IM.IsWADA AS WADA, IM.WADAYear, IM.GeneralNote, IM.IngredientList'; 
		
	END

	SET @query = @selectStatement + CHAR(13) + @fromStatement + CHAR(13) + @whereStatement;

	EXEC(@query);

	SET NOCOUNT OFF;

END