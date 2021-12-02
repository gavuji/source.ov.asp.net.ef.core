CREATE TABLE ProductionLineMaster
(
	ProductionLineID INT IDENTITY(1,1) NOT NULL,
	LineCode VARCHAR(10),
	LineDescription VARCHAR(100) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_ProductionLineMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_ProductionLineMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_ProductionLineMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_ProductionLineMaster PRIMARY KEY (ProductionLineID)
)