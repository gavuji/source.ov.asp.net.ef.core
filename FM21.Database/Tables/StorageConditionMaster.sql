CREATE TABLE StorageConditionMaster
(
	StorageConditionID INT IDENTITY(1,1) NOT NULL,
	StorageDescription VARCHAR(300),
	StorageType VARCHAR(20),
	StorageGroupNumber INT,
	IsActive BIT NOT NULL CONSTRAINT DF_StorageConditionMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_StorageConditionMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_StorageConditionMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_StorageConditionMaster PRIMARY KEY (StorageConditionID)
)