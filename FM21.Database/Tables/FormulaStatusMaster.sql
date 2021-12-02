CREATE TABLE FormulaStatusMaster
(
	FormulaStatusID INT IDENTITY(1,1) NOT NULL,
	FormulaStatusCode VARCHAR(50) NOT NULL,
	FormulaStatusCodeDescription VARCHAR(300) NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_FormulaStatusMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT Null CONSTRAINT DF_FormulaStatusMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_FormulaStatusMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_FormulaStatusMaster PRIMARY KEY (FormulaStatusID) 
)