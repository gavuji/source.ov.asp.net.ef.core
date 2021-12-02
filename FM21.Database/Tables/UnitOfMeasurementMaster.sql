
CREATE TABLE UnitOfMeasurementMaster
(
	UnitOfMeasurementID INT IDENTITY(1,1) NOT NULL,
	MeasurementUnit VARCHAR(10) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_UnitOfMeasurementMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_UnitOfMeasurementMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_UnitOfMeasurementMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_UnitOfMeasurementMaster PRIMARY KEY (UnitOfMeasurementID)
)