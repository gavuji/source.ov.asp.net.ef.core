CREATE TABLE RoleMaster
(
    RoleID INT IDENTITY(1,1) NOT NULL,
    RoleName VARCHAR(50) NOT NULL,
	RoleDescription VARCHAR(100),
    IsActive BIT NOT NULL CONSTRAINT DF_RoleMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT Null CONSTRAINT DF_RoleMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_RoleMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_RoleMaster PRIMARY KEY (RoleID)
);