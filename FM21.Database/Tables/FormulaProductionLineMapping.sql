CREATE TABLE FormulaProductionLineMapping
(
	FormulaProductionLineMapID INT IDENTITY(1,1) NOT NULL,
	FormulaID INT NOT NULL,
	ProductionLineID INT NOT NULL,
	ProductionMixerID INT,
	[Weight] DECIMAL(18,2),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_FormulaProductionLineMapping_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_FormulaProductionLineMapping PRIMARY KEY (FormulaProductionLineMapID),
	CONSTRAINT FK_FormulaProductionLineMapping_FormulaMaster FOREIGN KEY (FormulaID) REFERENCES FormulaMaster(FormulaID),
	CONSTRAINT FK_FormulaProductionLineMapping_ProductionLineMaster FOREIGN KEY (ProductionLineID) REFERENCES ProductionLineMaster(ProductionLineID),
	CONSTRAINT FK_FormulaProductionLineMapping_ProductionMixerMaster FOREIGN KEY (ProductionMixerID) REFERENCES ProductionMixerMaster(ProductionMixerID)
)