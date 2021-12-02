CREATE PROCEDURE GetNutrientByIngredientID       
(          
  @ingredientID INT       
)          
AS          
BEGIN          
           
 SET NOCOUNT ON;          
          
 SELECT           
  NM.NutrientID,          
  NM.[Name],          
  NM.NutrientTypeID,          
  NTM.TypeName,          
  NM.UnitOfMeasurementID,          
  UOMM.MeasurementUnit,          
  NM.IsMandatory,          
  NM.IsShowOnTarget,          
  NM.DisplayColumnOrder,          
  NM.DisplayItemOrder,          
  CASE WHEN(@ingredientID>0) THEN INM.NutrientValue ELSE NM.DefaultValue END AS NutrientValue,          
  COALESCE(INM.IngredientNutrientMapID, 0 ) AS IngredientNutrientMapID,        
  NM.DefaultValue,    
  NM.IsActiveNutrient        
           
 FROM NutrientMaster NM          
 INNER JOIN NutrientTypeMaster NTM ON NTM.NutrientTypeID = NM.NutrientTypeID          
 INNER JOIN UnitOfMeasurementMaster UOMM ON UOMM.UnitOfMeasurementID = NM.UnitOfMeasurementID          
 LEFT JOIN IngredientNutrientMapping INM ON INM.NutrientID = NM.NutrientID AND INM.IngredientID = @ingredientID          
 WHERE NM.IsActive = 1 and NM.IsDeleted=0      
 ORDER BY DisplayColumnOrder, DisplayItemOrder ASC      
      
 SET NOCOUNT OFF;          
          
END