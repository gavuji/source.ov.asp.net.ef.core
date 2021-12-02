CREATE TABLE KosherCodeMaster
(
	KosherCodeID INT IDENTITY(1,1) NOT NULL,
	KosherCode VARCHAR(10) NOT NULL,
	KosherCodeDescription VARCHAR(200) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_KosherCodeMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_KosherCodeMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_KosherCodeMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_KosherCodeMaster PRIMARY KEY (KosherCodeID)
)