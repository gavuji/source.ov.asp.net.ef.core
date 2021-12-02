CREATE TABLE ProductTypeMaster
(
	ProductTypeID INT IDENTITY(1,1) NOT NULL,
	ProductType VARCHAR(20) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_ProductTypeMaster_IsActive DEFAULT(1),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_ProductTypeMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_ProductTypeMaster PRIMARY KEY (ProductTypeID)
)