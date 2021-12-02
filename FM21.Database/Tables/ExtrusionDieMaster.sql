CREATE TABLE ExtrusionDieMaster
(
	ExtrusionDieID INT IDENTITY(1,1) NOT NULL,
	ExtrusionDie VARCHAR(30) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_ExtrusionDieMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_ExtrusionDieMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_ExtrusionDieMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_ExtrusionDieMaster PRIMARY KEY (ExtrusionDieID)
)