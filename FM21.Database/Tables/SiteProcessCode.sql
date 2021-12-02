CREATE TABLE SiteProcessCode
(
	SiteProcessCodeID INT IDENTITY(1,1) NOT NULL,
	SiteID INT NOT NULL,
	ProcessCode VARCHAR(2) NOT NULL,
	DisplayOrder INT NULL,
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_SiteProcessCode_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_SiteProcessCode PRIMARY KEY (SiteProcessCodeID),
	CONSTRAINT FK_SiteProcessCode_SiteMaster FOREIGN KEY (SiteID) REFERENCES SiteMaster(SiteID)
)