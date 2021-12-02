CREATE TABLE FormulaTypeMaster
(
	FormulaTypeID INT IDENTITY(1,1) NOT NULL,
	FormulaTypeCode VARCHAR(10) NOT NULL,
	FormulaDescription VARCHAR(100),
	IsActive BIT NOT NULL CONSTRAINT DF_FormulaTypeMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT Null CONSTRAINT DF_FormulaTypeMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_FormulaTypeMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_FormulaTypeMaster PRIMARY KEY (FormulaTypeID)
)