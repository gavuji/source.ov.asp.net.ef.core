CREATE TABLE ReleaseAgentMaster
(
	ReleaseAgentID INT IDENTITY(1,1) NOT NULL,
	ReleaseAgentDescription VARCHAR(300) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_ReleaseAgentMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT Null CONSTRAINT DF_ReleaseAgentMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_ReleaseAgentMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_ReleaseAgentMaster PRIMARY KEY (ReleaseAgentID) 
)