CREATE TABLE SitterWidthMaster
(
	SitterWidthID INT IDENTITY(1,1) NOT NULL,
	SitterWidth VARCHAR(30) NOT NULL,
	SiteID INT NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_SitterWidthMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_SitterWidthMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_SitterWidthMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_SitterWidthMaster PRIMARY KEY (SitterWidthID),
	CONSTRAINT FK_SitterWidthMaster_SiteMaster FOREIGN KEY (SiteID) REFERENCES SiteMaster(SiteID)
)