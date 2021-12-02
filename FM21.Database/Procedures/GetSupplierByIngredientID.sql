CREATE PROCEDURE GetSupplierByIngredientID
(
	@ingredientID INT
)
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT
		SM.SiteID,
		SM.SiteDescription,
		ISM.BrokerID,
		ISM.BrokerDetail,
		ISM.BrokerDescription,
		ISM.ManufactureID,
		ISM.ManufactureDetail,
		ISM.ManufactureDescription,
		ISM.ManufactureLocation,
		ISM.KosherCodeID,
		ISM.KosherAgency,
		ISM.KosherExpireDate,
		ISM.Price,
		ISM.QuotedDate,
		ISM.ManufactureLocation,
		ISM.IngredientSupplierID,
		ISM.IngredientID
	FROM SiteMaster SM
	LEFT JOIN IngredientSupplierMapping ISM ON ISM.SiteID = SM.SiteID AND ISM.IngredientID = @ingredientID
	WHERE SM.IsActive = 1

	SET NOCOUNT OFF;

END