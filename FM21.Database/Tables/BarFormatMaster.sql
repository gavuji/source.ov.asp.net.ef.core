CREATE TABLE BarFormatMaster
(
	BarFormatID INT IDENTITY(1,1) NOT NULL,
	BarFormatCode VARCHAR(50),
	BarFormatType VARCHAR(30) NOT NULL,
	BarFormatDescription VARCHAR(100) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_BarFormatMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT Null CONSTRAINT DF_BarFormatMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_BarFormatMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	DisplayOrder INT NOT NULL
	CONSTRAINT PK_BarFormatMaster PRIMARY KEY (BarFormatID) 
)