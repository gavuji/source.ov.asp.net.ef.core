CREATE TABLE BrokerMaster
(
	BrokerID INT IDENTITY(1,1) NOT NULL,
	BrokerName VARCHAR(50) NOT NULL,
	[Address] VARCHAR(200),
	Email VARCHAR(50),
	PhoneNumber VARCHAR(30),
	IsActive BIT NOT NULL CONSTRAINT DF_BrokerMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_BrokerMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_BrokerMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_BrokerMaster PRIMARY KEY (BrokerID)
)