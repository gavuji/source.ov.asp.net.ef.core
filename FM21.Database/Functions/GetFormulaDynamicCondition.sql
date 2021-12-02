CREATE FUNCTION GetFormulaDynamicCondition
(      
	 @searchCol VARCHAR(50),      
	 @searchCondition VARCHAR(50),      
	 @searchValue VARCHAR(50)      
)      
RETURNS VARCHAR(MAX)      
AS      
BEGIN      
      
	DECLARE @whereCondition VARCHAR(MAX);      
	DECLARE @Operator VARCHAR(20) = CASE WHEN ISNULL(@searchCondition, '') = 'NOT' THEN 'NOT LIKE' ELSE 'LIKE' END;      
	DECLARE @fieldCondition VARCHAR(20) = CASE WHEN ISNULL(@searchCondition, '') = 'NOT' THEN 'AND' ELSE 'OR' END;

	IF(ISNULL(@searchCondition, '') = '' OR ISNULL(@searchCondition, '') = 'NOT')
		SET @searchCondition = 'AND' ;      
       
	IF(@searchCol = 'FormulaCode')      
	BEGIN      
		SET @whereCondition = @searchCondition + ' (FM.FormulaCode ' + @Operator + ' ''%' + @searchValue + '%'')';      
	END
	ELSE IF(@searchCol = 'Customer')      
	BEGIN 
		SET @whereCondition = @searchCondition + ' (C.CustomerAbbreviation1 ' + @Operator + ' ''%' + @searchValue + '%'' ' 
								+ @fieldCondition + ' C.CustomerAbbreviation2 ' + @Operator + ' ''%' + @searchValue + '%'')';             
	END
	ELSE IF(@searchCol = 'Description')      
	BEGIN      
		SET @whereCondition = @searchCondition + ' (FM.ProductDescription ' + @Operator + ' ''%' + @searchValue + '%'' ' 
								+ @fieldCondition + ' FM.FlavourDescription ' + @Operator + ' ''%' + @searchValue + '%'' '
								+ @fieldCondition + ' FRCM.FormulaRegulatoryCategoryDescription ' + @Operator + ' ''%' + @searchValue + '%'')';             
	END
	ELSE IF(@searchCol = 'FormulaStatus')      
	BEGIN 
		SET @whereCondition = @searchCondition + ' (FM.FormulaStatusCode ' + @Operator + ' ''%' + @searchValue + '%'')';             
	END
	ELSE IF(@searchCol = 'Instructions')      
	BEGIN 
		SET @whereCondition = @searchCondition + ' (FDM.InstructionDescription ' + @Operator + ' ''%' + @searchValue + '%'')';             
	END
	ELSE IF(@searchCol = 'Claims')
	BEGIN      
		SET @whereCondition = @searchCondition + ' (Claims ' + @Operator + ' ''%' + @searchValue + '%'')';             
	END
	ELSE IF(@searchCol = 'WhereUsed')
	BEGIN
		SET @whereCondition = @searchCondition + ' (IM.JDECode ' + @Operator + ' ''%' + @searchValue + '%''     
			' + @fieldCondition + ' IM.ONTResearchCode ' + @Operator + ' ''%' + @searchValue + '%''    
			' + @fieldCondition + ' IM.ANJResearchCode ' + @Operator + ' ''%' + @searchValue + '%''    
			' + @fieldCondition + ' IM.S30SubAssemblyCode ' + @Operator + ' ''%' + @searchValue + '%''    
			' + @fieldCondition + ' PN.PartNumber ' + @Operator + ' ''%' + @searchValue + '%''    
			' + @fieldCondition + ' IM.IRWPart ' + @Operator + ' ''%' + @searchValue + '%'')';         
	END
	ELSE IF(@searchCol = 'General')
	BEGIN
       
		SET @whereCondition = '';
		SELECT @whereCondition =  @whereCondition + 'FM.[' + COLUMN_NAME + '] ' + @Operator + ' ''%' + @searchValue + '%'' ' + @fieldCondition + ' '
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'FormulaMaster'
		AND DATA_TYPE IN ('char','nchar','ntext','nvarchar','text','varchar');
		
		SET @whereCondition = LEFT(@whereCondition, LEN(@whereCondition)-LEN(@fieldCondition)) + ' '
						+ @fieldCondition + ' FM.FormulaStatusCode ' + @Operator + ' ''%' + @searchValue + '%''';
		SET @whereCondition = @searchCondition + ' (' + @whereCondition + ')';

	END
	ELSE
	BEGIN
    
		SET @whereCondition = @searchCondition + ' ' + CASE @searchCol
			WHEN 'FormulaTag ' THEN 'FM.FormulaTag'
			WHEN 'SaveDate' THEN 'CONVERT(VARCHAR, FM.CreatedOn, 1)'
			ELSE @searchCol END + ' ' + @Operator + ' ''%' + @searchValue + '%''';
    
	END

	RETURN @whereCondition;      

END