CREATE PROCEDURE GetFormulaDetailsByFormulaID
(
	@formulaID INT
)
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @subFormulaRetrivalLevel INT = 1;
	DECLARE @siteID INT;

	IF(@formulaID = 0)
	BEGIN
	
		DECLARE @Counter INT = 1;
		DECLARE @tbl TABLE 
		(
			RowID INT,
			ParentRowID INT,
			RowNumber INT,
			HierarchyRowID VARCHAR(1000),
			[Level] INT,
			FormulaDetailMapID INT NOT NULL DEFAULT(0),
			ReferenceID	INT NOT NULL DEFAULT(0),
			ReferenceType INT NOT NULL DEFAULT(0),
			SubgroupPercent	NUMERIC,
			Amount NUMERIC,
			TotalPercent NUMERIC,
			OvgPercent NUMERIC,
			[Target] NUMERIC,
			Code VARCHAR,
			Unit VARCHAR,
			PartCode VARCHAR(50),
			[Description] VARCHAR(100),
			IsAlertInfoExist BIT NOT NULL DEFAULT(0)
		);
		
		WHILE ( @Counter <= 40)
		BEGIN

			INSERT INTO @tbl
			(RowID, ParentRowID, [Level])
			VALUES (@Counter, @Counter, 0);

			SET @Counter = @Counter  + 1;

		END

		SELECT * FROM @tbl;

	END
	ELSE
	BEGIN

		SET @siteID = (SELECT TOP 1 SPTM.SiteID 
						FROM FormulaMaster FM
						INNER JOIN SiteProductTypeMapping SPTM ON SPTM.SiteProductMapID = FM.SiteProductMapID
						WHERE FM.FormulaID = @formulaID);

		;WITH Formula(FormulaDetailMapID, ReferenceID, ReferenceType, RowNumber, [Level], HierarchyRowID) 
		AS (
			SELECT MDM.FormulaDetailMapID, MDM.ReferenceID, MDM.ReferenceType, MDM.RowNumber, CAST(0 AS INT) AS [Level], CAST(MDM.RowNumber AS VARCHAR(MAX))
			FROM FormulaDetailMapping MDM
			WHERE MDM.FormulaID = @formulaID

			UNION ALL

			SELECT 
				MDM.FormulaDetailMapID, MDM.ReferenceID, MDM.ReferenceType, MDM.RowNumber, [Level]+1,
				CAST((CASE WHEN Formula.HierarchyRowID != '' THEN (Formula.HierarchyRowID + '.') ELSE '' END + CAST(MDM.RowNumber AS VARCHAR(100))) AS VARCHAR(MAX))
			FROM FormulaDetailMapping MDM
			INNER JOIN Formula ON MDM.FormulaID = Formula.ReferenceID AND Formula.ReferenceType = 3
			WHERE Formula.[Level] < @subFormulaRetrivalLevel
		)
		SELECT IDENTITY(INT,1,1) AS RowID, F.FormulaDetailMapID, F.ReferenceID, F.ReferenceType, F.RowNumber, F.HierarchyRowID, F.[Level]
		INTO #tempFormula
		FROM Formula F
		ORDER BY CAST(F.HierarchyRowID AS FLOAT);

		SELECT F.RowID, ParentF.RowID AS ParentRowID, F.RowNumber, F.HierarchyRowID, F.[Level], F.FormulaDetailMapID, F.ReferenceID, F.ReferenceType,
		MDM.SubgroupPercent, MDM.Amount, MDM.TotalPercent, MDM.OvgPercent, MDM.[Target], MDM.Code, MDM.Unit, 
		COALESCE(IM.JDECode, PN.PartNumber, IM.ONTResearchCode, IM.ANJResearchCode, IM.IRWPart, IM.AltCode, IM.S30SubAssemblyCode) AS PartCode, 
		CASE F.ReferenceType WHEN 1 THEN MDM.InstructionDescription 
		WHEN 2 THEN STUFF(CONCAT('~', IM.UsageAlert, '~', IM.ExclusivityAlert, '~', IM.AlertReview, CASE WHEN IM.SupplierSeeNotes = '1' THEN '~SeeNote' ELSE '' END, 
		'~', IM.RMDescription, ' \ ', SM.SupplierName, ' ', ISM.ManufactureDetail, ' \ ', BM.BrokerName, ' ', ISM.BrokerDetail), 1, 1, '') 
		WHEN 3 THEN FM.FormulaCode + ' ' + FM.FormulaDescription ELSE '' END AS [Description],
		CASE WHEN CONCAT(IM.UsageAlert, IM.ExclusivityAlert, IM.AlertReview) = '' THEN 0 ELSE 1 END AS IsAlertInfoExist
		FROM #tempFormula F
		INNER JOIN #tempFormula ParentF ON ParentF.HierarchyRowID = REVERSE(SUBSTRING(REVERSE(F.HierarchyRowID), CHARINDEX('.', REVERSE(F.HierarchyRowID))+1, 100))
		INNER JOIN FormulaDetailMapping MDM ON MDM.FormulaDetailMapID = F.FormulaDetailMapID
		LEFT JOIN IngredientMaster IM ON IM.IngredientID = MDM.ReferenceID AND MDM.ReferenceType = 2
		LEFT JOIN FormulaMaster FM ON FM.FormulaID = MDM.ReferenceID AND MDM.ReferenceType = 3
		OUTER APPLY (SELECT TOP 1 ISPM.PartNumber
			FROM IngredientSitePartMapping ISPM
			WHERE ISPM.IngredientID = IM.IngredientID AND ISPM.SiteID = @siteID
		) AS PN
		OUTER APPLY (SELECT TOP 1 ISM.ManufactureID, ISM.BrokerID, ISM.ManufactureDetail, ISM.BrokerDetail 
			FROM IngredientSupplierMapping ISM
			WHERE ISM.IngredientID = IM.IngredientID AND ISM.SiteID = @siteID
		) AS ISM
		LEFT JOIN SupplierMaster SM ON SM.SupplierID = ISM.ManufactureID
		LEFT JOIN BrokerMaster BM ON BM.BrokerID = ISM.BrokerID;

		IF OBJECT_ID('tempdb..#tempFormula') IS NOT NULL
			DROP TABLE #tempFormula;

	END

	SET NOCOUNT OFF;
	
END