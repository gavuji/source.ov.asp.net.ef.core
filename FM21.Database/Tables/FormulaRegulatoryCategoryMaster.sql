CREATE TABLE FormulaRegulatoryCategoryMaster
(
	FormulaRegulatoryCateoryID INT IDENTITY(1,1) NOT NULL,
	FormulaRegulatoryCategoryDescription VARCHAR(100) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_FormulaRegulatoryCategoryMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT Null CONSTRAINT DF_FormulaRegulatoryCategoryMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_FormulaRegulatoryCategoryMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_FormulaRegulatoryCategoryMaster PRIMARY KEY (FormulaRegulatoryCateoryID) 
)