CREATE TABLE FormulaDatasheetMapping
(
	FormulaDatasheetMapID INT IDENTITY(1,1) NOT NULL,
	FormulaID INT NOT NULL,
	DatasheetFormatID INT,
	NutrientID INT,
	[Target] NUMERIC(30, 14),
	[Override] NUMERIC(30, 14),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_FormulaDatasheetMapping_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_FormulaDatasheetMapping PRIMARY KEY (FormulaDatasheetMapID),
	CONSTRAINT FK_FormulaDatasheetMapping_FormulaMaster FOREIGN KEY (FormulaID) REFERENCES FormulaMaster(FormulaID),
	CONSTRAINT FK_FormulaDatasheetMapping_DatasheetFormatMaster FOREIGN KEY (DatasheetFormatID) REFERENCES DatasheetFormatMaster(DatasheetFormatID),
	CONSTRAINT FK_FormulaDatasheetMapping_NutrientMaster FOREIGN KEY (NutrientID) REFERENCES NutrientMaster(NutrientID)
)