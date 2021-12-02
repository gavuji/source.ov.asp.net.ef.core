CREATE TABLE SiteMaster 
(
    SiteID INT IDENTITY(1,1) NOT NULL,
    SiteCode VARCHAR(10) NOT NULL,
    SiteDescription VARCHAR(250),
    S30CodePrefix VARCHAR(5),
    [IsActive] BIT NOT NULL CONSTRAINT DF_SiteMaster_IsActive DEFAULT(1),
    CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_SiteMaster_CreatedOn DEFAULT(GETDATE()),
    UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_SiteMaster PRIMARY KEY (SiteID)
);