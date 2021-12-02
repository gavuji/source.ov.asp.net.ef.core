CREATE TABLE PkoPercentageMaster
(
	PkoPercentageMasterID INT IDENTITY(1,1) NOT NULL,
	PkoPercentageDescription VARCHAR(300) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_PkoPercentageMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT Null CONSTRAINT DF_PkoPercentageMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_PkoPercentageMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_PkoPercentageMaster PRIMARY KEY (PkoPercentageMasterID) 
)