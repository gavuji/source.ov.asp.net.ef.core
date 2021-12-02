CREATE TABLE CriteriaMaster
(
	CriteriaID INT IDENTITY(1,1) NOT NULL,
	CriteriaDescription VARCHAR(100),
	ColorCode VARCHAR(15),
	CriteriaOrder VARCHAR(2),
	IsActive BIT NOT NULL CONSTRAINT DF_CriteriaMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_CriteriaMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_CriteriaMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_CriteriaMaster PRIMARY KEY (CriteriaID)
)