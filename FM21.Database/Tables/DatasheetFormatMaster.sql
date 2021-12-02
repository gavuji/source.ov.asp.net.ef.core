CREATE TABLE DatasheetFormatMaster
(
	DatasheetFormatID INT IDENTITY(1,1) NOT NULL,
	DatasheetCode VARCHAR(10),
	DatasheetDescription VARCHAR(300),
	IsActive BIT NOT NULL CONSTRAINT DF_DatasheetFormatMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT Null CONSTRAINT DF_DatasheetFormatMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_DatasheetFormatMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_DatasheetFormatMaster PRIMARY KEY (DatasheetFormatID)
)