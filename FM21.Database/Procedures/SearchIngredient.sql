CREATE PROCEDURE SearchIngredient
(
	@searchCol1 VARCHAR(50) = '',
	@searchCol1Value VARCHAR(50) = '',
	@searchCol2 VARCHAR(50) = '',
	@searchCol2Value VARCHAR(50) = '',
	@searchCol2Condition VARCHAR(20) = '',
	@searchCol3 VARCHAR(50) = '',
	@searchCol3Value VARCHAR(50) = '',
	@searchCol3Condition VARCHAR(20) = '',
	@displayColumn VARCHAR(200) = '',
	@nutrientColumn VARCHAR(200) = '',
	@rMSatusColumn VARCHAR(200) = '',
	@ingColumn VARCHAR(200) = '',
	@ingAllergen VARCHAR(200) = '',  
	@supplierColumn VARCHAR(1000) = '',
	@unitMeasurment VARCHAR(100) = '',
	@siteID INT = 0, --0 For All, -1 For All Active, Rest all positive numbers for specific site wise infomation
	@sortColumn VARCHAR(50) = NULL,
	@sortDirection VARCHAR(10) = ''
)  
AS  
BEGIN

	SET NOCOUNT ON;

	DECLARE @query VARCHAR(MAX);
	DECLARE @selectStatement VARCHAR(MAX);
	DECLARE @fromStatement VARCHAR(MAX);
	DECLARE @whereStatement VARCHAR(MAX);
	DECLARE @tblColumns TABLE(ItemID INT, ItemValue VARCHAR(100));
	DECLARE @tblRMStatusColumns TABLE(ItemID INT, ItemValue VARCHAR(100));
	DECLARE @nutrientCol TABLE(ItemID INT, ItemValue VARCHAR(100));  
	DECLARE @tblIngMasterCol TABLE(ItemID INT, ItemValue VARCHAR(100));
	DECLARE @rowIndex INT = 1;
	DECLARE @col VARCHAR(50), @tblAlies VARCHAR(20);
	DECLARE @tblSuplierCol TABLE(ItemID INT, colName VARCHAR(100));

	INSERT INTO @tblSuplierCol
	SELECT * FROM dbo.Split(@supplierColumn, ',')

	DECLARE @retreiveSupplierInfo BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName NOT IN ('PartNumber', 'ResearchCode')),0);
	DECLARE @partNumber BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'PartNumber'),0);
	DECLARE @randDCode BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'ResearchCode'),0);
	DECLARE @mfgr BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'Manufacture'),0);
	DECLARE @mfgrDesc BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'ManufactureDescription'),0);
	DECLARE @mfgrDetail BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'ManufactureDetail'),0);
	DECLARE @mfgrLocaion BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'ManufactureLocation'),0);
	DECLARE @broker BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'Broker'),0);
	DECLARE @brokerDesc BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'BrokerDescription'),0);
	DECLARE @brokerDetail BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'BrokerDetail'),0);
	DECLARE @lbCost BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'Price'),0);
	DECLARE @quoteDate BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'QuotedDate'),0);
	DECLARE @kosherAgency BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'KosherAgency'),0);
	DECLARE @kosherCode BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'KosherCode'),0);
	DECLARE @kosherExp BIT = ISNULL((SELECT TOP 1 1 FROM @tblSuplierCol WHERE colName = 'KosherExpireDate'),0);  

	SET @searchCol1 = ISNULL(@searchCol1, '');
	SET @searchCol2 = ISNULL(@searchCol2, '');
	SET @searchCol3 = ISNULL(@searchCol3, '');
 
	INSERt INTO @tblColumns
	SELECT funRes.ItemID, NM.NutrientID
	FROM dbo.Split(@nutrientColumn,',')  AS funRes
	INNER JOIN NutrientMaster NM ON NM.[Name] = funRes.ItemValue;  
  
	INSERt INTO @tblRMStatusColumns
	SELECT * FROM dbo.Split(@rMSatusColumn,',');

	INSERT INTO @tblIngMasterCol  
	SELECT * FROM dbo.Split(@ingColumn,','); 
 
	SET @selectStatement = 'SELECT DISTINCT COALESCE(IM.JDECode, PN.PartNumber, IM.ONTResearchCode, IM.ANJResearchCode, IM.IRWPart, IM.AltCode, IM.S30SubAssemblyCode) AS PartNumber,
		IM.IngredientID, IM.JDECode, IM.NutrientLink, IM.IngredientUsed, IM.InternalXReference, IM.CreatedOn, IM.UpdatedOn,
		STUFF(CONCAT(''~'', IM.UsageAlert, ''~'', IM.ExclusivityAlert, ''~'', IM.AlertReview, CASE WHEN IM.SupplierSeeNotes = ''1'' THEN ''~SeeNote'' ELSE '''' END, 
		''~'', IM.RMDescription, '' \ '', SM.SupplierName, '' '', ISM.ManufactureDetail, '' \ '', BM.BrokerName, '' '', ISM.BrokerDetail), 1, 1, '''') AS [Description],
		CAST(CASE WHEN CONCAT(IM.UsageAlert, IM.ExclusivityAlert, IM.AlertReview) = '''' THEN 0 ELSE 1 END AS BIT) AS IsAlertInfoExist';
	SET @fromStatement = 'FROM IngredientMaster IM
		INNER JOIN IngredientCategoryMaster ICM ON ICM.IngredientCategoryID = IM.IngredientCategoryID'
		+ CASE WHEN @siteID > 1  THEN CHAR(13) + 'LEFT JOIN IngredientSitePartMapping ISPM ON ISPM.IngredientID = IM.IngredientID AND ISPM.SiteID = ' + CAST(@siteID AS VARCHAR(10)) ELSE '' END + ' 
		OUTER APPLY (SELECT TOP 1 ISPM.PartNumber
			FROM IngredientSitePartMapping ISPM
			WHERE ISPM.IngredientID = IM.IngredientID' + CASE WHEN @siteID > 0 THEN ' AND ISPM.SiteID = ' + CAST(@siteID AS VARCHAR(10)) ELSE '' END + '
		) AS PN
		OUTER APPLY (SELECT TOP 1 * 
			FROM IngredientSupplierMapping ISM
			WHERE ISM.IngredientID = IM.IngredientID' + CASE WHEN @siteID > 0 THEN ' AND ISM.SiteID = ' + CAST(@siteID AS VARCHAR(10)) ELSE '' END + '
		) AS ISM
		LEFT JOIN SupplierMaster SM ON SM.SupplierID = ISM.ManufactureID
		LEFT JOIN BrokerMaster BM ON BM.BrokerID = ISM.BrokerID
		' + CASE WHEN @kosherCode = 1 THEN 'LEFT JOIN KosherCodeMaster KC ON KC.KosherCodeID = ISM.KosherCodeID' ELSE '' END;  
	SET @whereStatement = 'WHERE (IM.IsActive = 1 AND IM.IsDeleted = 0)';

	IF(@siteID = -1 OR @siteID > 0)
	BEGIN

		SET @fromStatement += CHAR(13) + 'INNER JOIN FormulaDetailMapping FDM ON FDM.ReferenceID = IM.IngredientID AND FDM.ReferenceType = 2
								INNER JOIN FormulaMaster FM ON FM.FormulaID = FDM.FormulaID
								INNER JOIN SiteProductTypeMapping SPTM ON SPTM.SiteProductMapID = FM.SiteProductMapID';

		IF(@siteID > 0)
		BEGIN
			SET @whereStatement += CHAR(13) + 'AND SPTM.SiteID = ' + CAST(@siteID AS VARCHAR(10)); 
		END

	END

	WHILE(EXISTS(SELECT TOP 1 1 FROM @tblColumns WHERE ItemID = @rowIndex))
	BEGIN

		SET @col = (SELECT TOP 1 ItemValue FROM @tblColumns WHERE ItemID = @rowIndex AND ItemValue <> '0');
		IF(ISNULL(@col,'') != '')
		BEGIN

			SET @tblAlies = 'INM' + CAST(@rowIndex AS VARCHAR(10));
			SET @fromStatement += CHAR(13) + 'LEFT JOIN IngredientNutrientMapping ' + @tblAlies + ' ON ' + @tblAlies + '.IngredientID = IM.IngredientID AND ' + @tblAlies + '.NutrientID = ' + @col;
			SET @selectStatement += ', ' + @tblAlies + '.NutrientValue AS NutrientValue' + CAST(@rowIndex AS VARCHAR(10));  

		END
		SET @rowIndex += 1;

	END  

	IF(ISNULL(@unitMeasurment,'') != '')  
	BEGIN  
  
		SET @fromStatement += CHAR(13) + 'LEFT JOIN UnitOfMeasurementMaster UM ON UM.UnitOfMeasurementID = IM.UnitOfMeasurementID';  
		SET @selectStatement += ', ' + 'UM.MeasurementUnit AS UOM';  

	END  
  
	SET @rowIndex = 1
	WHILE(EXISTS(SELECT TOP 1 1 FROM @tblRMStatusColumns WHERE ItemID = @rowIndex ))
	BEGIN

		SET @col = (SELECT TOP 1 ItemValue FROM @tblRMStatusColumns WHERE ItemID = @rowIndex);
		IF(ISNULL(@col,'') != '')
		BEGIN

			SET @tblAlies = 'RSM' + CAST(@rowIndex AS VARCHAR(10));
			SET @fromStatement += CHAR(13) + 'LEFT JOIN RMStatusMaster ' + @tblAlies + ' ON ' + @tblAlies + '.RMStatusMasterID = IM.'+ CASE @col
				WHEN 'GlutenStatus' THEN 'GlutenStatusID'  
				WHEN 'OrganicStatus' THEN 'OrganicStatusID'  
				WHEN 'HalalStatus' THEN 'HalalStatusID'  
				WHEN 'RegulatoryStatus' THEN 'RegulatoryStatusID' 
				WHEN 'Lethality' THEN 'SterilizationMethodID' END
			SET @selectStatement += ', ' + @tblAlies + CASE @col
				WHEN 'GlutenStatus' THEN '.RMStatus AS GlutenStatus'  
				WHEN 'OrganicStatus' THEN '.RMStatus AS OrganicStatus'  
				WHEN 'HalalStatus' THEN '.RMStatus AS HalalStatus'
				WHEN 'Lethality' THEN '.RMStatus AS Lethality'  
				WHEN 'RegulatoryStatus' THEN '.RMStatus AS RegulatoryStatus' END

		END
		SET @rowIndex += 1;

	END  
  
	IF(@ingAllergen != '' OR 'Allergen' IN (@searchCol1, @searchCol2 ,@searchCol3))
	BEGIN  

		SET @fromStatement += CHAR(13) + 'OUTER APPLY (
		SELECT STRING_AGG(AM.AllergenName, '', '') AS Allergen
			FROM AllergenMaster AM
			INNER JOIN IngredientAllergenMapping IAM ON IAM.AllergenID = AM.AllergenID
			WHERE IAM.IngredientID = IM.IngredientID
		) AS Allergen';  
		SET @selectStatement += ', Allergen.Allergen';  
  
	END  

	SET @rowIndex = 1
	IF(@ingColumn != '')  
	BEGIN

		WHILE(EXISTS(SELECT TOP 1 1 FROM @tblIngMasterCol WHERE ItemID = @rowIndex ))  
		BEGIN  
  
			SET @col = (SELECT TOP 1 ItemValue FROM @tblIngMasterCol WHERE ItemID = @rowIndex);  
			IF(ISNULL(@col,'') != '')  
			BEGIN

				SET @selectStatement += ', ' + (SELECT [dbo].GetColumnByParams(@col,'IngredientMaster',1));
				IF(ISNULL(@unitMeasurment,'') = '' AND @col = 'PrimaryUnitWeight')  
				BEGIN  
					SET @fromStatement += CHAR(13) + 'LEFT JOIN UnitOfMeasurementMaster UM ON UM.UnitOfMeasurementID = IM.UnitOfMeasurementID'; 
				END

				IF(@col = 'CreatedBy')
				BEGIN
					SET @fromStatement += CHAR(13) + 'LEFT JOIN UserMaster UMCB ON IM.CreatedBy = UMCB.UserID';
				END
				ELSE IF(@col = 'UpdatedBy')
				BEGIN
					SET @fromStatement += CHAR(13) + 'LEFT JOIN UserMaster UMUB ON IM.UpdatedBy = UMUB.UserID';
				END

			END  
			SET @rowIndex += 1;  
  
		END

	END 
 
	IF EXISTS(SELECT TOP 1 1 from @tblSuplierCol) 
	BEGIN

		SET @selectStatement += CASE WHEN @mfgrLocaion = 1 THEN ',ISM.ManufactureLocation AS MfgrLocation' ELSE '' END +
		CASE WHEN (@randDCode = 1 AND @siteID = 1) THEN ', IM.ONTResearchCode AS ''R&DCode''' ELSE '' END +
		CASE WHEN (@randDCode = 1 AND @siteID = 3) THEN ', IM.ANJResearchCode AS ''R&DCode''' ELSE '' END +
		CASE WHEN (@randDCode = 1 AND @siteID NOT IN (1,3)) THEN ', NULL AS ''R&DCode''' ELSE '' END +
		CASE WHEN @mfgrDetail = 1 THEN ', ISM.ManufactureDetail AS MfgrDetails' ELSE '' END +
		CASE WHEN @mfgr = 1 THEN ', SM.SupplierName AS Mfgr' ELSE '' END +
		CASE WHEN @broker = 1 THEN ', BM.BrokerName AS Broker' ELSE '' END +
		CASE WHEN @brokerDesc = 1 THEN ', ISM.BrokerDescription AS BrokerDesc' ELSE '' END +
		CASE WHEN @brokerDetail = 1  THEN ', ISM.BrokerDetail AS BrokerDetail' ELSE '' END +
		CASE WHEN @lbCost = 1 THEN ', ISM.Price AS [$LB]' ELSE '' END +
		CASE WHEN @quoteDate = 1 THEN ', ISM.QuotedDate AS QuotedDate' ELSE '' END +
		CASE WHEN @kosherCode = 1 THEN ', KCM.KosherCode AS KosherCode' ELSE '' END +
		CASE WHEN @kosherAgency = 1  THEN ', ISM.KosherAgency AS KosherAgency' ELSE '' END + 
		CASE WHEN @partNumber = 1  THEN (CASE WHEN @siteID > 1 THEN ', PN.PartNumber AS PartCode' ELSE ', IM.JDECode AS PartCode' END) ELSE '' END +
		CASE WHEN @kosherExp = 1  THEN ', ISM.KosherExpireDate AS KosherExpireDate' ELSE '' END 

	END

	IF(@searchCol1 != '')
	BEGIN
		SET @whereStatement += CHAR(13) + (SELECT dbo.GetIngredientDynamicCondition(@searchCol1, '', @searchCol1Value));
	END

	IF(@searchCol2 != '')
	BEGIN 
		SET @whereStatement += CHAR(13) + (SELECT dbo.GetIngredientDynamicCondition(@searchCol2, @searchCol2Condition, @searchCol2Value));
	END

	IF(@searchCol3 != '')
	BEGIN
		SET @whereStatement += CHAR(13) + (SELECT dbo.GetIngredientDynamicCondition(@searchCol3, @searchCol3Condition, @searchCol3Value));
	END

	SET @query = @selectStatement + CHAR(13) + @fromStatement + CHAR(13) + @whereStatement + CHAR(13) + 'ORDER BY ' + ISNULL(@sortColumn,'PartNumber') + ' ' + ISNULL(@sortDirection,'');

	EXEC(@query);

	SET NOCOUNT OFF;

END