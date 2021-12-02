CREATE TABLE ClaimMaster
(
	ClaimID INT IDENTITY(1,1) NOT NULL,
    ClaimCode VARCHAR(20) NOT NULL,
	ClaimDescription VARCHAR(200),
	ClaimGroupType VARCHAR(20),
	HasImpact BIT NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_ClaimMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_ClaimMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_ClaimMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_ClaimMaster PRIMARY KEY (ClaimID)
)