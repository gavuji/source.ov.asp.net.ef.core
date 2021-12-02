CREATE PROCEDURE rptFormulaCriteria
(
	@formulaID INT
)
AS
BEGIN

	SELECT 
		CM.CriteriaID, 
		CM.CriteriaOrder, 
		CM.CriteriaDescription, 
		CM.ColorCode, 
		FCM.FormulaCriteriaMapID
	FROM CriteriaMaster AS CM
	LEFT JOIN FormulaCriteriaMapping AS FCM ON FCM.CriteriaID = CM.CriteriaID AND FCM.FormulaID = @formulaID
	ORDER BY CM.CriteriaOrder

END