CREATE TABLE CountryMaster
(
	CountryID INT IDENTITY(1,1) NOT NULL,
	CountryName VARCHAR(50) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_CountryMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_CountryMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_CountryMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_CountryMaster PRIMARY KEY (CountryID)
)