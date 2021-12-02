CREATE TABLE FormulaRevision
(
	FormulaRevisionID INT IDENTITY(1,1) NOT NULL,
	FormulaID INT NOT NULL,
	RevisionNumber INT NOT NULL,
	ProcessCode VARCHAR(2) NOT NULL,
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_FormulaRevision_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_FormulaRevision PRIMARY KEY (FormulaRevisionID),
	CONSTRAINT FK_FormulaRevision_FormulaMaster FOREIGN KEY (FormulaID) REFERENCES FormulaMaster(FormulaID)
)