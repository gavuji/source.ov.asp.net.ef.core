CREATE TABLE InstructionGroupMaster
(
	InstructionGroupID INT IDENTITY(1,1) NOT NULL,
	InstructionGroupName VARCHAR(100) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_InstructionGroupMaster_IsActive DEFAULT(1),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_InstructionGroupMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_InstructionGroupMaster PRIMARY KEY (InstructionGroupID)
)