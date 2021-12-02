CREATE PROCEDURE rptFormulaNotes
(
	@formulaID INT
)
AS
BEGIN

	SELECT 
		FM.FormulaChangeNote, 
		FM.TextureAppearanceNote, 
		FM.ProjectInfoNote, 
		FM.PackagingInfoForPowderNote,
		FM.DatasheetNote
	FROM FormulaMaster AS FM
	WHERE FM.FormulaID = @formulaID

END