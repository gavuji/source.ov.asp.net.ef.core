CREATE PROCEDURE [dbo].[GetFormulaHomePageHeaderInfo]    
(    
    
 @formulaID INT   
)      
AS      
BEGIN    
    
 SET NOCOUNT ON;    
    
 SELECT   
 FormulaCode,  
 ProductDescription AS ProductName,  
 COALESCE(PM.ProjectCode,PM.NPICode) AS ProjectCode,  
 FM.FormulaReference,  
 FM.FormulaStatusCode,  
 FM.DieNumber,  
 FM.ActualWaterPercentage 
  
FROM FormulaMaster FM  
 LEFT JOIN ProjectMaster PM ON FM.FormulaProjectID = PM.ProjectID 
  where FormulaID = @formulaID    
    
END  
  