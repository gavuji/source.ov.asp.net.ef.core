CREATE TABLE ProjectMaster
(
	ProjectID INT IDENTITY(1,1) NOT NULL,
	ProjectCode INT NOT NULL,
	NPICode VARCHAR(30),
	ProjectDescription VARCHAR(150) NOT NULL,
	CustomerID INT,
	IsActive BIT NOT NULL CONSTRAINT DF_ProjectMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT Null CONSTRAINT DF_ProjectMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_ProjectMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_ProjectMaster PRIMARY KEY (ProjectID),
	CONSTRAINT FK_ProjectMaster_Customer FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
)