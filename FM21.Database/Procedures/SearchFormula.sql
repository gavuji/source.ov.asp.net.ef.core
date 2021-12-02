CREATE PROCEDURE SearchFormula
(
	@searchCol1 VARCHAR(50) = '',
	@searchCol1Value VARCHAR(50) = '',
	@searchCol2 VARCHAR(50) = '',
	@searchCol2Value VARCHAR(50) = '',
	@searchCol2Condition VARCHAR(20) = '',
	@searchCol3 VARCHAR(50) = '',
	@searchCol3Value VARCHAR(50) = '',
	@searchCol3Condition VARCHAR(20) = '',
	@siteID INT = 0,
	@columnList VARCHAR(100) = '',
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
	DECLARE @tblColumnList TABLE(ItemID INT, ItemValue VARCHAR(100));
	DECLARE @rowIndex INT = 1;
	DECLARE @col VARCHAR(50), @lineCode VARCHAR(50);
	DECLARE @finalSiteIDs VARCHAR(50) = '';

	SET @searchCol1 = ISNULL(@searchCol1, '');
	SET @searchCol2 = ISNULL(@searchCol2, '');
	SET @searchCol3 = ISNULL(@searchCol3, '');

	IF(@siteID = 0)
	BEGIN
		SELECT @finalSiteIDs = STRING_AGG(SiteID, ',') FROM SiteMaster WHERE IsActive = 1;
	END
	ELSE IF(@siteID  = -1)
	BEGIN
		SELECT @finalSiteIDs = STRING_AGG(SM.SiteID, ',')
		FROM SiteProductTypeMapping SPTM 
		INNER JOIN SiteMaster SM ON SM.SiteID = SPTM.SiteID 
		WHERE SM.IsActive = 1 AND SPTM.ProductTypeID = 1;
	END
	ELSE IF(@siteID  = -2)
	BEGIN
		SELECT @finalSiteIDs = STRING_AGG(SM.SiteID, ',')
		FROM SiteProductTypeMapping SPTM 
		INNER JOIN SiteMaster SM ON SM.SiteID = SPTM.SiteID 
		WHERE SM.IsActive = 1 AND SPTM.ProductTypeID = 2;
	END
	ELSE
	BEGIN
		SET @finalSiteIDs = @siteID;
	END

	INSERT INTO @tblColumnList  
	SELECT * FROM dbo.Split(@columnList,','); 
 
	SET @selectStatement = 'SELECT DISTINCT
		FM.FormulaID,
		FM.FormulaStatusCode,
		FM.FormulaCode ,
		COALESCE(C.CustomerAbbreviation1,C.CustomerAbbreviation2) AS CustomerName,
		FM.ProductDescription,
		FM.FlavourDescription,
		FM.AltCode,
		FM.CustomerCode,
		FM.S70Code,
		FM.AllergenCode,
		FM.FormulaReference,
		FM.PowderXRefCode,
		FM.FormulaTag,
		FM.BarFormatDescription,
		Claims';

	SET @fromStatement ='FROM FormulaMaster FM 
		LEFT JOIN Customer C ON C.CustomerId = FM.FormulaCustomerID
		OUTER APPLY (
			SELECT STRING_AGG(CM.ClaimCode, '', '') AS Claims
			FROM ClaimMaster CM
			INNER JOIN FormulaClaimMapping FCM ON FCM.ClaimID = CM.ClaimID AND FCM.FormulaID = FM.FormulaID
		) AS Claims';

	SET @whereStatement = 'WHERE (FM.IsActive = 1 AND FM.IsDeleted = 0)';

	IF(@siteID != 0)
	BEGIN

		SET @fromStatement += CHAR(13) + 'INNER JOIN SiteProductTypeMapping SPT ON SPT.SiteProductMapID = FM.SiteProductMapID AND SPT.SiteID IN ( ' + CAST(@finalSiteIDs AS VARCHAR(50)) + ' )';

	END

	SET @rowIndex = 1
	IF(@columnList != '')  
	BEGIN

		WHILE(EXISTS(SELECT TOP 1 1 FROM @tblColumnList WHERE ItemID = @rowIndex ))  
		BEGIN
  
			SET @col = (SELECT TOP 1 ItemValue FROM @tblColumnList WHERE ItemID = @rowIndex);  
			IF(ISNULL(@col,'') != '')  
			BEGIN

				IF( @col NOT IN ( 'Claims', 'AllergenCode' ) )
				BEGIN

					IF(CHARINDEX('ONT_', @col) = 1 OR  CHARINDEX('LAC_', @col) = 1 OR CHARINDEX('ANJ_', @col) = 1)
					BEGIN
						
						SET @lineCode = SUBSTRING(@col, CHARINDEX('_', @col)+1,20);
						SET @lineCode = CASE @lineCode WHEN 'RD' THEN 'R&D' ELSE @lineCode END;
						SET @fromStatement += CHAR(13) + 'LEFT JOIN FormulaProductionLineMapping ' + @col + '_FPLM ON ' + @col + '_FPLM.FormulaID = FM.FormulaID
							LEFT JOIN ProductionLineMixerMapping ' + @col + '_PLMM ON ' + @col + '_PLMM.ProductionLineMixerMapID = ' + @col + '_FPLM.ProductionLineMixerMapID
							LEFT JOIN SiteProductionLineMapping ' + @col + '_SPLM ON ' + @col + '_SPLM.SiteProductionLineMapID = ' + @col + '_PLMM.SiteProductionLineID 
							AND ' + @col + '_SPLM.SiteID = ' + CASE SUBSTRING(@col, 0,CHARINDEX('_', @col)) WHEN 'ONT' THEN '1' 
																								WHEN 'LAC' THEN '2'
																								WHEN 'ANJ' THEN '3' END + '
							LEFT JOIN ProductionLineMaster ' + @col + '_PLM ON ' + @col + '_PLM.ProductionLineID = ' + @col + '_SPLM.ProductionLineID AND ' + @col + '_PLM.LineCode = ''' + @lineCode + '''';
						SET @selectStatement += ', CASE WHEN ' + @col + '_PLM.LineCode IS NOT NULL THEN ' + @col + '_FPLM.Weight ELSE NULL END AS ' + @col;
					
					END
					ELSE
					BEGIN

						IF(@col = 'ProjectDescription')
						BEGIN
							SET @fromStatement += CHAR(13) + 'LEFT JOIN ProjectMaster PM ON PM.ProjectID = FM.FormulaProjectID';
						END
						ELSE IF(@col = 'PowderLiquidAmount')
						BEGIN
							SET @fromStatement += CHAR(13) + 'LEFT JOIN PowderLiquidMaster PLM ON PLM.PowderLiquidID = FM.PowderLiquidAmountID';
						END
						ELSE IF(@col = 'PrimaryLine')
						BEGIN
							SET @fromStatement += CHAR(13) + 'LEFT JOIN SiteProductionLineMapping SPLM_PL ON SPLM_PL.SiteProductionLineMapID = FM.PrimaryProductionLineID
									LEFT JOIN ProductionLineMaster PLM_PL ON PLM_PL.ProductionLineID = SPLM_PL.ProductionLineID';
						END
						ELSE IF(@col = 'CreatedBy')
						BEGIN
							SET @fromStatement += CHAR(13) + 'LEFT JOIN UserMaster UMCB ON FM.CreatedBy = UMCB.UserID';
						END
						ELSE IF(@col = 'UpdatedBy')
						BEGIN
							SET @fromStatement += CHAR(13) + 'LEFT JOIN UserMaster UMUB ON FM.UpdatedBy = UMUB.UserID';
						END

						SET @selectStatement += ', ' + (CASE @col
									WHEN 'ProjectDescription' THEN 'PM.ProjectDescription'
									WHEN 'PowderLiquidAmount' THEN 'PLM.LiquidDescription AS PowderLiquidAmount'
									WHEN 'ServingSize' THEN '(FM.CoreWeight+FM.ToppingWeight+FM.CoatingWeight) AS ServingSize'
									WHEN 'PrimaryLine' THEN 'PLM_PL.LineCode AS PrimaryLine'
									WHEN 'OpsNote' THEN 'CAST(FM.OpsNote AS VARCHAR(MAX)) AS OpsNote'
									WHEN 'CreatedDate' THEN 'CONVERT(VARCHAR, FM.CreatedOn, 1) AS CreatedOn'
									WHEN 'UpdatedDate' THEN 'CONVERT(VARCHAR, FM.UpdatedOn, 1) AS UpdatedOn'
									WHEN 'CreatedBy' THEN 'UMCB.DisplayName AS CreatedBy'
									WHEN 'UpdatedBy' THEN 'UMUB.DisplayName AS UpdatedBy'
									ELSE 'FM.'+ @col END);
					END

				END

			END  
			SET @rowIndex += 1;  
  
		END

	END

	IF('Description' IN (@searchCol1, @searchCol2 ,@searchCol3))
	BEGIN
		SET @fromStatement += CHAR(13) + 'LEFT JOIN FormulaRegulatoryCategoryMaster FRCM ON FRCM.FormulaRegulatoryCateoryID = FM.RegulatoryCategoryID';  
	END

	IF('Instructions' IN (@searchCol1, @searchCol2 ,@searchCol3))
	BEGIN
		SET @fromStatement += CHAR(13) + 'LEFT JOIN FormulaDetailMapping FDM ON FDM.FormulaID = FM.FormulaID AND FDM.ReferenceType = 1';  
	END

	IF('WhereUsed' IN (@searchCol1, @searchCol2 ,@searchCol3))
	BEGIN
		SET @fromStatement += CHAR(13) + 'LEFT JOIN FormulaDetailMapping FDMI ON FDMI.FormulaID = FM.FormulaID AND FDMI.ReferenceType = 2
										LEFT JOIN IngredientMaster IM ON IM.IngredientID = FDMI.ReferenceID
										OUTER APPLY (SELECT TOP 1 ISPM.PartNumber 
											FROM IngredientSitePartMapping ISPM
											WHERE ISPM.IngredientID = IM.IngredientID
										) AS PN';
	END

	IF(@searchCol1 != '')
	BEGIN
		SET @whereStatement += CHAR(13) + (SELECT dbo.GetFormulaDynamicCondition(@searchCol1, '',  @searchCol1Value));
	END

	IF(@searchCol2 != '')
	BEGIN
		SET @whereStatement += CHAR(13) + (SELECT dbo.GetFormulaDynamicCondition(@searchCol2, @searchCol2Condition,  @searchCol2Value));
	END

	IF(@searchCol3 != '')
	BEGIN
		SET @whereStatement += CHAR(13) + (SELECT dbo.GetFormulaDynamicCondition(@searchCol3, @searchCol3Condition,  @searchCol3Value));
	END

	SET @query = @selectStatement + CHAR(13) + @fromStatement + CHAR(13) + @whereStatement + CHAR(13) + 'ORDER BY ' + ISNULL(@sortColumn,'FormulaCode') + ' ' + ISNULL(@sortDirection,'');    
 
	EXEC(@query);

END