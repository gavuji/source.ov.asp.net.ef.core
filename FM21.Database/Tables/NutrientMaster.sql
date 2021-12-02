CREATE TABLE NutrientMaster
(
	NutrientID INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(200),
	NutrientTypeID INT NOT NULL,
	UnitOfMeasurementID INT NOT NULL,
	DefaultValue DECIMAL(18,14),
	IsShowOnTarget BIT NOT NULL,
	IsMandatory BIT NOT NULL,
	DisplayColumnOrder INT NOT NULL,
	DisplayItemOrder INT NOT NULL,
	IsAminoAcid BIT NOT NULL CONSTRAINT DF_NutrientMaster_IsAminoAcid DEFAULT(0),
	IsActiveNutrient BIT,
	IsActive BIT NOT NULL CONSTRAINT DF_NutrientMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_NutrientMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_NutrientMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_NutrientMaster PRIMARY KEY (NutrientID),
	CONSTRAINT FK_NutrientMaster_NutrientTypeMaster FOREIGN KEY (NutrientTypeID) REFERENCES NutrientTypeMaster(NutrientTypeID),
	CONSTRAINT FK_NutrientMaster_UnitOfMeasurementMaster FOREIGN KEY (UnitOfMeasurementID) REFERENCES UnitOfMeasurementMaster(UnitOfMeasurementID)
)