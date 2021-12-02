CREATE TABLE NutrientTypeMaster
(
	NutrientTypeID INT IDENTITY(1,1) NOT NULL,
	TypeName VARCHAR(20) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_NutrientTypeMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_NutrientTypeMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_NutrientTypeMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_NutrientTypeMaster PRIMARY KEY (NutrientTypeID)
)