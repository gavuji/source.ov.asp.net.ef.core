CREATE TABLE UnitServingMaster
(
	UnitServingID INT IDENTITY(1,1) NOT NULL,
	UnitServingType VARCHAR(20),
	UnitDescription VARCHAR(100),
	ProductTypeID INT,
	IsActive BIT NOT NULL CONSTRAINT DF_UnitServingMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_UnitServingMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_UnitServingMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_UnitServingMaster PRIMARY KEY (UnitServingID) 
)