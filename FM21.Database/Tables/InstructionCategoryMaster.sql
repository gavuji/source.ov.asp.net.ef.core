CREATE TABLE InstructionCategoryMaster
(
    InstructionCategoryID INT IDENTITY(1,1) NOT NULL,
    InstructionCategory NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_InstructionCategoryMaster_IsActive DEFAULT(1),
    CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_InstructionCategoryMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_InstructionCategoryMaster PRIMARY KEY (InstructionCategoryID)
);