CREATE PROCEDURE GetIngredientPartInfo  
(   
	@ingredientID INT
)    
AS  
BEGIN    
     
	SET NOCOUNT ON;    
    
	SELECT TOP 1    
		IM.IngredientUsed AS ActiveStatus,    
		IM.RMDescription,     
		IM.NutrientLink,       
		ICM.IngredientCategoryCode,  
		BM.BrokerName,  
		SM.SupplierName,  
		KCM.KosherCode,   
		IM.InternalXReference,   
		IM.ExternalXReference,  
		--IM.usageRate  
		IM.ONTResearchCode,  
		IM.JDECode,  
		'100 %As Is' AS Activity,  
		(CASE WHEN(ISPM.SiteID = 5 ) THEN ISPM.PartNumber ELSE NULL END) AS SLCCode,
		(CASE WHEN(ISPM.SiteID = 3 ) THEN ISPM.PartNumber ELSE NULL END) AS ANJCode,
		(CASE WHEN(ISPM.SiteID = 2 ) THEN ISPM.PartNumber ELSE NULL END) AS LACCode,
		(CASE WHEN(ISPM.SiteID = 4 ) THEN ISPM.PartNumber ELSE NULL END) AS ANACode,
		IM.ANJResearchCode,  
		IM.IRWPart,    
		(COALESCE(IM.ExternalXReference, '') + ', '+ COALESCE(IM.AltCode, '') + ', '+ COALESCE(IM.ShakleeCode, '')) AS CustomerCode,    
		IM.ExclusivityAlert,  
		Allergen.Allergen,     
		IM.IngredientBreakDown,    
		IM.IngredientList,    
		IM.DataSourceNote,   
		IM.SupplierNote AS AlertNote,  
		(UM.DisplayName +' '+ FORMAT(IM.GeneralDate,'dd/MM/yyyy hh:mm')) AS GeneralUpdate,  
		(UMNC.DisplayName +' '+ FORMAT(IM.NutrientDataChangeDate,'dd/MM/yyyy hh:mm')) AS NutritionUpdate,  
		FORMAT(IM.UpdatedOn,'dd/MM/yyyy hh:mm') AS UpdateDate  
	FROM IngredientMaster IM    
		LEFT JOIN IngredientSupplierMapping ISM ON ISM.IngredientID = IM.IngredientID  
		LEFT JOIN IngredientSitePartMapping ISPM ON ISPM.IngredientID = IM.IngredientID  
		LEFT JOIN BrokerMaster BM ON BM.BrokerID = ISM.BrokerID  
		LEFT JOIN KosherCodeMaster KCM ON KCM.KosherCodeID = ISM.KosherCodeID  
		LEFT JOIN SupplierMaster SM ON SM.SupplierID = ISM.ManufactureID  
		LEFT JOIN IngredientCategoryMaster ICM ON ICM.IngredientCategoryID = IM.IngredientCategoryID  
		LEFT JOIN UserMaster UM ON UM.UserID = IM.GeneralDataChangedBy  
		LEFT JOIN UserMaster UMNC ON UMNC.UserID = IM.UpdatedBy  
		OUTER APPLY (    
			SELECT STRING_AGG(AM.AllergenName, ', ') AS Allergen
			FROM AllergenMaster AM
			INNER JOIN IngredientAllergenMapping IAM ON IAM.AllergenID = AM.AllergenID
			WHERE IAM.IngredientID = @ingredientID   
		) AS Allergen      
	WHERE IM.IngredientID = @ingredientID  

END