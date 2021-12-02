CREATE TABLE FormulaClaimMapping 
(
	FormulaClaimMapID INT IDENTITY(1,1) NOT NULL,
	FormulaID INT NOT NULL,
	ClaimID INT NOT NULL,
	[Description] VARCHAR(30),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_FormulaClaimMapping_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_FormulaClaimMapping PRIMARY KEY (FormulaClaimMapID),
	CONSTRAINT FK_FormulaClaimMappingFormulaMaster FOREIGN KEY (FormulaID) REFERENCES FormulaMaster(FormulaID),
	CONSTRAINT FK_FormulaClaimMappingClaimMaster FOREIGN KEY (ClaimID) REFERENCES ClaimMaster(ClaimID),
)