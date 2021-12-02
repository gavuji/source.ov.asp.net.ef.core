CREATE TABLE RolePermissionMapping
(
	RolePermissionID INT IDENTITY(1, 1) NOT NULL,
	RoleID INT NOT NULL,
	PermissionID INT NOT NULL,
	PermissionType TINYINT NOT NULL,
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_RolePermissionMapping_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_RolePermission PRIMARY KEY (RolePermissionID),
	CONSTRAINT FK_RolePermissionRoleMaster FOREIGN KEY (RoleID) REFERENCES RoleMaster(RoleID),
	CONSTRAINT FK_RolePermissionPermissionMaster FOREIGN KEY (PermissionID) REFERENCES PermissionMaster(PermissionID)
);