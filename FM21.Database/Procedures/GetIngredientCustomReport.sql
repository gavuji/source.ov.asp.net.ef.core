CREATE PROCEDURE GetIngredientCustomReport
( 
	@nutrientColumn VARCHAR(MAX) = '',
	@rMSatusColumn VARCHAR(MAX) = '', 
	@ingColumn VARCHAR(MAX) = '', 
	@ingAllergen VARCHAR(MAX) = '', 
	@unitMeasurment VARCHAR(100) = '',
	@supplierColumn VARCHAR(MAX) = '',
	@siteIDs VARCHAR(50) = '0' --0 For All
)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @query VARCHAR(MAX);
	DECLARE @selectStatement VARCHAR(MAX);
	DECLARE @fromStatement VARCHAR(MAX);
	DECLARE @whereStatement VARCHAR(MAX);
	DECLARE @tblNutrientCol TABLE(ItemID INT, colName NVARCHAR(100)); 
	DECLARE @tblRMStatusColumns TABLE(ItemID INT, colName VARCHAR(100));
	DECLARE @tblIngMasterCol TABLE(ItemID INT, colName VARCHAR(100));
	DECLARE @tblSuplierCol TABLE(ItemID INT, colName VARCHAR(100));
	DECLARE @rowIndex INT = 1;
	DECLARE @col VARCHAR(50), @tblAlies VARCHAR(20);
	DECLARE @finalSiteIDs VARCHAR(50) = '';

	IF(@siteIDs = '0')
	BEGIN
		SELECT @finalSiteIDs = STRING_AGG(SiteID, ',') FROM SiteMaster WHERE IsActive = 1;
	END
	ELSE
	BEGIN
		SET @finalSiteIDs = @siteIDs;
	END

	INSERT INTO @tblSuplierCol
	SELECT * FROM dbo.Split(@supplierColumn, ',');

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
 
	INSERT INTO @tblNutrientCol
	SELECT funRes.ItemID, NM.NutrientID
	FROM dbo.Split(@nutrientColumn,',') AS funRes
	INNER JOIN NutrientMaster NM ON NM.[Name] = funRes.ItemValue;

	INSERT INTO @tblRMStatusColumns
	SELECT * FROM dbo.Split(@rMSatusColumn,',');
 
	INSERT INTO @tblIngMasterCol
	SELECT * FROM dbo.Split(@ingColumn,',');

	SET @selectStatement = '';
	SET @fromStatement = 'FROM IngredientMaster IM';
	SET @whereStatement = 'WHERE (IM.IsActive = 1 AND IM.IsDeleted = 0)';

	IF(@siteIDs != '0')
	BEGIN

		SET @fromStatement += CHAR(13) + 'INNER JOIN FormulaDetailMapping FDM ON FDM.ReferenceID = IM.IngredientID AND FDM.ReferenceType = 2
								INNER JOIN FormulaMaster FM ON FM.FormulaID = FDM.FormulaID
								INNER JOIN SiteProductTypeMapping SPTM ON SPTM.SiteProductMapID = FM.SiteProductMapID';

		SET @whereStatement += CHAR(13) + 'AND SPTM.SiteID IN (' + @finalSiteIDs + ')';

	END
 
	SELECT 
	@fromStatement += (CASE WHEN SM.SiteID <> 1 AND @partNumber = 1 THEN CHAR(13) + 'LEFT JOIN IngredientSitePartMapping ISPM_' + SM.SiteCode + ' ON ISPM_' + SM.SiteCode + '.IngredientID = IM.IngredientID AND ISPM_' + SM.SiteCode + '.SiteID = ' + CAST(SM.SiteID AS VARCHAR(10)) ELSE '' END +
		CHAR(13) + 'LEFT JOIN IngredientSupplierMapping ISM_' + SM.SiteCode + ' ON ISM_' + SM.SiteCode + '.IngredientID = IM.IngredientID AND ISM_' + SM.SiteCode + '.SiteID = ' + CAST(SM.SiteID AS VARCHAR(10)) +
		CHAR(13) + 'LEFT JOIN SupplierMaster SM_' + SM.SiteCode + ' ON SM_' + SM.SiteCode + '.SupplierID = ISM_' + SM.SiteCode + '.ManufactureID' +
		CHAR(13) + CASE WHEN(@broker = 1) THEN 'LEFT JOIN BrokerMaster BM_' + SM.SiteCode + ' ON BM_' + SM.SiteCode + '.BrokerID = ISM_' + SM.SiteCode + '.BrokerID' ELSE '' END +
		CHAR(13) + CASE WHEN(@kosherCode = 1) THEN 'LEFT JOIN KosherCodeMaster KCM_' + SM.SiteCode + ' ON KCM_' + SM.SiteCode + '.KosherCodeID = ISM_' + SM.SiteCode + '.KosherCodeID' ELSE '' END), 
	@selectStatement += CASE WHEN @partNumber = 1 THEN (CASE WHEN SM.SiteID <> 1 THEN ', ISPM_' + SM.SiteCode + '.PartNumber AS ' + SM.SiteCode + '_PartNumber' ELSE ', IM.JDECode AS ' + SM.SiteCode + '_PartNumber' END) ELSE '' END +
		CASE WHEN @randDCode = 1 THEN (CASE WHEN SM.SiteID IN (1, 3) THEN ', IM.' + SM.SiteCode + 'ResearchCode' ELSE ', NULL' END + ' AS ' + SM.SiteCode + '_ResearchCode') ELSE '' END +
		CASE WHEN @mfgr = 1 THEN ', SM_' + SM.SiteCode + '.SupplierName AS ' + SM.SiteCode + '_Manufacture' ELSE '' END +
		CASE WHEN @mfgrDesc = 1 THEN ', ISM_' + SM.SiteCode + '.ManufactureDescription AS ' + SM.SiteCode + '_ManufactureDescription' ELSE '' END +
		CASE WHEN @mfgrLocaion = 1 THEN ', ISM_' + SM.SiteCode + '.ManufactureLocation AS ' + SM.SiteCode + '_ManufactureLocation' ELSE '' END +
		CASE WHEN @mfgrDetail = 1 THEN ', ISM_' + SM.SiteCode + '.ManufactureDetail AS ' + SM.SiteCode + '_ManufactureDetail' ELSE '' END +
		CASE WHEN @broker = 1 THEN ', BM_' + SM.SiteCode + '.BrokerName AS ' + SM.SiteCode + '_Broker' ELSE '' END+
		CASE WHEN @brokerDesc = 1 THEN ', ISM_' + SM.SiteCode + '.BrokerDescription AS ' + SM.SiteCode + '_BrokerDescription' ELSE '' END +
		CASE WHEN @brokerDetail = 1 THEN ', ISM_' + SM.SiteCode + '.BrokerDetail AS ' + SM.SiteCode + '_BrokerDetail' ELSE '' END +
		CASE WHEN @lbCost = 1 THEN ', ISM_' + SM.SiteCode + '.Price AS ' + SM.SiteCode + '_Price' ELSE '' END +
		CASE WHEN @quoteDate = 1 THEN ', ISM_' + SM.SiteCode + '.QuotedDate AS ' + SM.SiteCode + '_QuotedDate' ELSE '' END +
		CASE WHEN @kosherCode = 1 THEN ', KCM_' + SM.SiteCode + '.KosherCode AS ' + SM.SiteCode + '_KosherCode' ELSE '' END +
		CASE WHEN @kosherAgency = 1 THEN ', ISM_' + SM.SiteCode + '.KosherAgency AS ' + SM.SiteCode + '_KosherAgency' ELSE '' END +
		CASE WHEN @kosherExp = 1 THEN ', FORMAT(ISM_' + SM.SiteCode + '.KosherExpireDate, ''MM-dd-yy'') AS ' + SM.SiteCode + '_KosherExpireDate' ELSE '' END 
	FROM SiteMaster SM
	INNER JOIN dbo.Split(@finalSiteIDs, ',') SD ON SD.ItemValue = SM.SiteID;

	WHILE(EXISTS(SELECT TOP 1 1 FROM @tblNutrientCol WHERE ItemID = @rowIndex))
	BEGIN

		SET @col = (SELECT TOP 1 colName FROM @tblNutrientCol WHERE ItemID = @rowIndex AND colName <> '0');
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
		SET @selectStatement += ', UM.MeasurementUnit AS UOM'; 
 
	END 

	SET @rowIndex = 1
	WHILE(EXISTS(SELECT TOP 1 1 FROM @tblRMStatusColumns WHERE ItemID = @rowIndex ))
	BEGIN

		SET @col = (SELECT TOP 1 colName FROM @tblRMStatusColumns WHERE ItemID = @rowIndex);
		IF(ISNULL(@col,'') != '')
		BEGIN
 
			SET @tblAlies = 'RSM' + CAST(@rowIndex AS VARCHAR(10));
			SET @fromStatement += CHAR(13) + 'LEFT JOIN RMStatusMaster ' + @tblAlies + ' ON ' + @tblAlies + '.RMStatusMasterID = IM.' + CASE @col
				WHEN 'GlutenStatus' THEN 'GlutenStatusID'
				WHEN 'OrganicStatus' THEN 'OrganicStatusID'
				WHEN 'HalalStatus' THEN 'HalalStatusID'
				WHEN 'RegulatoryStatus' THEN 'RegulatoryStatusID'
				WHEN 'Lethality' THEN 'SterilizationMethodID' END
			SET @selectStatement += ', ' + @tblAlies +  CASE @col 
				WHEN 'GlutenStatus' THEN '.RMStatus AS GlutenStatus'
				WHEN 'OrganicStatus' THEN '.RMStatus AS OrganicStatus'
				WHEN 'HalalStatus' THEN '.RMStatus AS HalalStatus' 
				WHEN 'Lethality' THEN '.RMStatus AS Lethality'
				WHEN 'RegulatoryStatus' THEN '.RMStatus AS RegulatoryStatus' END
		END
		SET @rowIndex += 1;

	END 
 
	IF( @ingAllergen != '' )
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
 
		WHILE(EXISTS(SELECT TOP 1 1 FROM @tblIngMasterCol WHERE ItemID = @rowIndex))
		BEGIN

			SET @col = (SELECT TOP 1 colName FROM @tblIngMasterCol WHERE ItemID = @rowIndex);
			IF(ISNULL(@col,'') != '')
			BEGIN

				SET @selectStatement += ', ' + (SELECT [dbo].GetColumnByParams(@col, 'IngredientMaster', 2)) 

				SET @fromStatement += CASE WHEN @col='CreatedBy' THEN ' LEFT JOIN UserMaster UMCB ON IM.CreatedBy = UMCB.UserID'
										WHEN @col='UpdatedBy' THEN ' LEFT JOIN UserMaster UMUB ON IM.CreatedBy = UMUB.UserID' ELSE '' END

				IF(ISNULL(@unitMeasurment,'') = '' AND @col ='PrimaryUnitWeight')      
				BEGIN 
					SET @fromStatement += CHAR(13) + 'LEFT JOIN UnitOfMeasurementMaster UM ON UM.UnitOfMeasurementID = IM.UnitOfMeasurementID'; 
				END

				IF(@col IN ('Category', 'CategoryDescrp') AND CHARINDEX('IngredientCategoryMaster ICM', @fromStatement) = 0)
				BEGIN
					SET @fromStatement += CHAR(13) + 'INNER JOIN IngredientCategoryMaster ICM ON ICM.IngredientCategoryID = IM.IngredientCategoryID'; 
				END
				
			END
			SET @rowIndex += 1;

		END

	END

	SET @query = 'SELECT' + STUFF(@selectStatement, 1, 1, ' ') + CHAR(13) + @fromStatement + CHAR(13) + @whereStatement + CHAR(13);

	EXEC(@query);

	SET NOCOUNT OFF;

END