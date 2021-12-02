CREATE TABLE FormulaCriteriaMapping 
(
	FormulaCriteriaMapID INT IDENTITY(1,1) NOT NULL,
	FormulaID INT NOT NULL,
	CriteriaID INT NOT NULL,
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_FormulaCriteriaMapping_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_FormulaCriteriaMapping PRIMARY KEY (FormulaCriteriaMapID),
	CONSTRAINT FK_FormulaCriteriaMappingFormulaMaster FOREIGN KEY (FormulaID) REFERENCES FormulaMaster(FormulaID),
	CONSTRAINT FK_FormulaCriteriaMappingCriteriaMaster FOREIGN KEY (CriteriaID) REFERENCES CriteriaMaster(CriteriaID),
)