CREATE TABLE FormulaDetailMapping
(
	FormulaDetailMapID INT IDENTITY(1,1) NOT NULL,
	FormulaID INT NOT NULL,
	ReferenceID INT,
	ReferenceType INT NOT NULL,
	InstructionDescription NVARCHAR(500),
	SubgroupPercent NUMERIC(18, 4),
	Amount NUMERIC(18, 4),
	TotalPercent NUMERIC(18, 4),
	OvgPercent NUMERIC(18, 4),
	[Target] NUMERIC(18, 4),
	RowNumber INT NOT NULL,
	Code VARCHAR(5),
	Unit VARCHAR(10),
	IsActive BIT NOT NULL CONSTRAINT DF_FormulaDetailMapping_IsActive DEFAULT(1),
	IsDeleted BIT NOT Null CONSTRAINT DF_FormulaDetailMapping_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_FormulaDetailMapping_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_FormulaDetailMapping PRIMARY KEY (FormulaDetailMapID),
	CONSTRAINT FK_FormulaDetailMapping_FormulaMaster FOREIGN KEY (FormulaID) REFERENCES FormulaMaster(FormulaID)
)