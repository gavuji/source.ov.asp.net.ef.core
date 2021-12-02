CREATE PROCEDURE [dbo].[GetFormulaChangeCodes]          
(          
 @FormulaTypeCode VARCHAR(50) ,       
 @CurrentSiteId int ,      
 @ChangeSiteId int =0,      
 @FormulaCode VARCHAR(50)='' ,      
 @ServingSize VARCHAR(10)='',      
 @FormulaId INT = 0    
)            
AS            
BEGIN      
        
  DECLARE @SiteCode VARCHAR(5),      
   @RevisionNumber VARCHAR(10),      
   @FormulaSequence VARCHAR(10),      
   @ProcessCode VARCHAR(10),      
   @AssignedProcessCode VARCHAR(10)='',           
   @CurrentDisplayOrder int = 0,      
   @NextDisplayOrder int = 1,      
   @BaseCode VARCHAR(10)='',      
   @SpinCode VARCHAR(10) =''      
      
      
 SELECT @SiteCode = S30CodePrefix FROM SiteMaster WHERe siteID = @CurrentSiteId       
      
 SELECT TOP 1 @FormulaSequence =  RIGHT('00000' + CAST(IncrementNumber AS varchar(5)) ,5) FROM FormulaChangeCode FCC       
 INNER JOIN FormulaTypeMaster FTM ON FCC.FormulaTypeID = FTM.FormulaTypeID      
 WHERE FCC.SiteID = @CurrentSiteId AND FTM.FormulaTypeCode = @FormulaTypeCode ORDER BY FCC.CreatedOn DESC      
      
 SELECT @ProcessCode = ProcessCode FROM SiteProcessCode WHERE SiteID = @CurrentSiteId AND DisplayOrder = 1     
 SELECT @RevisionNumber =  RIGHT('00000' + CAST(RevisionNumber AS varchar(5)) ,3) FROM [FormulaRevision] WHERE FormulaID = @FormulaId ORDER BY CreatedOn DESC      
      
      
  IF(COALESCE(@FormulaCode,'')<>'')      
  BEGIN  
       
    SET @BaseCode =   SUBSTRING(@FormulaCode,1,9);      
    SET @SpinCode =   SUBSTRING(@FormulaCode,11,3);      
      
    SELECT TOP 1 @CurrentDisplayOrder =  DisplayOrder FROM [SiteProcessCode] WHERE ProcessCode = SUBSTRING(@FormulaCode,14,1) and SiteID = @CurrentSiteId        
    SELECT @AssignedProcessCode = ProcessCode FROM SiteProcessCode WHERE SiteID = @CurrentSiteId AND DisplayOrder = @CurrentDisplayOrder + @NextDisplayOrder   
         
  END     
       
  ELSE      
  BEGIN     
       
   IF (COALESCE(@FormulaSequence,'')='')      
    BEGIN      
      SELECT @FormulaSequence = RIGHT('00000' + CAST(1 AS varchar(5)) ,5)      
      SELECT @RevisionNumber =  RIGHT('00000' + CAST(1 AS varchar(5)) ,3)       
        
      SELECT (@FormulaTypeCode + @SiteCode + @FormulaSequence +'.'+ @RevisionNumber + @ProcessCode+'-'+ @ServingSize       
    ) AS FormulaCode,  ' is next New Project_'  AS Description       
      RETURN ;      
    END    
          
   ELSE      
     BEGIN     
        
    SELECT (@FormulaTypeCode + @SiteCode + RIGHT('00000' + CAST(@FormulaSequence + 1 AS varchar(5)) ,5) +'.'+     
    ISNULL(@RevisionNumber,RIGHT('00000' + CAST(1 AS varchar(5)) ,3)) + @ProcessCode+'-'+ @ServingSize       
    ) AS FormulaCode,  ' is next New Project_'  AS Description      
      RETURN ;      
     END      
  END  
      
   IF(COALESCE(@AssignedProcessCode,'')='')      
   BEGIN      
   SET @AssignedProcessCode = @ProcessCode      
   END      
      
       
    SELECT ( CASE WHEN COALESCE(@BaseCode,'') <>'' THEN @BaseCode ELSE @FormulaTypeCode + @SiteCode + RIGHT('00000' + CAST(@FormulaSequence AS varchar(5)) ,5) END       
     + '.'+ CASE WHEN COALESCE(@SpinCode,'') <>'' THEN @SpinCode ELSE RIGHT('00000' + CAST(@RevisionNumber AS varchar(5)) ,3) END + @AssignedProcessCode+'-'+ @ServingSize      
    ) AS FormulaCode,  ' is next Process Change_'  AS Description     
       
    UNION      
      
   SELECT ( CASE WHEN COALESCE(@BaseCode,'') <>'' THEN @BaseCode ELSE @FormulaTypeCode + @SiteCode + RIGHT('00000' + CAST(@FormulaSequence AS varchar(5)) ,5) END       
     + '.'+ RIGHT('00000' + CAST(@RevisionNumber + 1 AS varchar(5)) ,3) + @ProcessCode + '-'+ @ServingSize      
    ) AS FormulaCode,  ' is next Formula Revision_'  AS Description       
      
    UNION      
      
   SELECT (@FormulaTypeCode + @SiteCode+RIGHT('00000' + CAST(@FormulaSequence + 1 AS varchar(5)) ,5) + '.'+RIGHT('00000' + CAST(1 AS varchar(5)) ,3) +   
   @ProcessCode + '-' + @ServingSize) AS FormulaCode,  ' is next New Project_'  AS Description     
      
      
 END      
      