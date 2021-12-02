CREATE PROCEDURE rptFormulaClaims
(
	@formulaID INT
)
AS
BEGIN

	DECLARE @Clalms VARCHAR(500) = (SELECT DISTINCT STRING_AGG(CM.ClaimGroupType, ', ')
		FROM FormulaClaimMapping FCM 
		INNER JOIN ClaimMaster CM ON CM.ClaimID = FCM.ClaimID
		WHERE FCM.FormulaID = @formulaID )

	IF(ISNULL(@Clalms,'') = '')
	BEGIN

		SET @Clalms = 'None'
		
	END
	
	SELECT 
		CM.ClaimID, 
		CM.ClaimCode, 
		CM.ClaimDescription, 
		CM.ClaimGroupType, 
		CM.HasImpact, 
		FCM.FormulaClaimMapID, 
		FCM.[Description],
		@Clalms AS Claims
	FROM ClaimMaster AS CM
	LEFT JOIN FormulaClaimMapping AS FCM ON FCM.ClaimID = CM.ClaimID AND FCM.FormulaID = @formulaID
	LEFT JOIN FormulaClaimMapping AS FCMD ON FCM.ClaimID = CM.ClaimID AND FCMD.FormulaID = @formulaID AND FCM.[Description] IS NOT NULL
	ORDER BY CM.ClaimID

END