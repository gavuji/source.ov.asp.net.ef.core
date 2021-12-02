CREATE TABLE ProductionMixerMaster
(
	ProductionMixerID INT IDENTITY(1,1) NOT NULL,
	MixerDescription VARCHAR(100) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_ProductionMixerMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_ProductionMixerMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_ProductionMixerMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_ProductionMixerMaster PRIMARY KEY (ProductionMixerID)
)