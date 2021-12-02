CREATE FUNCTION GetIngredientDynamicCondition  
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
     
	IF(@searchCol = 'BrokerManuf')    
	BEGIN    
    
		SET @whereCondition = @searchCondition + ' (BM.BrokerName ' + @Operator + ' ''%' + @searchValue + '%'' ' 
							+ @fieldCondition + ' SM.SupplierName ' + @Operator + ' ''%' + @searchValue + '%'')';    
      
	END
	ELSE IF(@searchCol = 'General')    
	BEGIN  
     
		SET @whereCondition = '';  
		SELECT @whereCondition =  @whereCondition + 'IM.[' + COLUMN_NAME + '] ' + @Operator + ' ''%' + @searchValue + '%'' ' + @fieldCondition + ' '  
		FROM INFORMATION_SCHEMA.COLUMNS   
		WHERE TABLE_NAME = 'IngredientMaster' 
		AND DATA_TYPE IN ('char','nchar','ntext','nvarchar','text','varchar');

		SET @whereCondition = LEFT(@whereCondition, LEN(@whereCondition)-LEN(@fieldCondition));
		SET @whereCondition = @searchCondition +' (' + @whereCondition + ')';
    
	END  
	ELSE IF(@searchCol = 'PartNumber')    
	BEGIN    
    
		SET @whereCondition = @searchCondition + ' (IM.JDECode ' + @Operator + ' ''%' + @searchValue + '%''     
		' + @fieldCondition + ' IM.ONTResearchCode ' + @Operator + ' ''%' + @searchValue + '%''    
		' + @fieldCondition + ' IM.ANJResearchCode ' + @Operator + ' ''%' + @searchValue + '%''    
		' + @fieldCondition + ' IM.S30SubAssemblyCode ' + @Operator + ' ''%' + @searchValue + '%''    
		' + @fieldCondition + ' PN.PartNumber ' + @Operator + ' ''%' + @searchValue + '%''    
		' + @fieldCondition + ' IM.IRWPart ' + @Operator + ' ''%' + @searchValue + '%'')';    
      
	END    
	ELSE    
	BEGIN    
    
		SET @whereCondition = @searchCondition + ' ' + CASE @searchCol    
			WHEN 'Category ' THEN 'ICM.IngredientCategoryCode'    
			WHEN 'Description' THEN 'IM.RMDescription'    
			WHEN 'Listing' THEN 'IM.IngredientList'    
			WHEN 'KosherCode' THEN 'KC.KosherCode'    
			WHEN 'AlertStatus' THEN 'IM.AlertReview'    
			WHEN 'SaveDate' THEN 'CONVERT(VARCHAR, IM.CreatedOn, 1)'    
			ELSE @searchCol END + ' ' + @Operator + ' ''%' + @searchValue + '%''';    
    
	END
    
	RETURN @whereCondition;    
    
END