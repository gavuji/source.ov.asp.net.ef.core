CREATE TABLE [dbo].[SupplierMaster]
(
	SupplierID INT IDENTITY(1,1) NOT NULL,
    SupplierName VARCHAR(50) NULL,
	[Address] VARCHAR(150) Null,
	Email varchar(30) NULL,
	PhoneNumber varchar(30) NULL,
	SupplierAbbreviation1 varchar(100) NULL,
	SupplierAbbreviation2 varchar(100) NULL,
    [IsActive] BIT DEFAULT(1) NULL,
	IsDeleted bit NULL DEFAULT 1,
	CreatedBy INT,
	CreatedOn DATETIME DEFAULT GETDATE() NOT NULL,
	UpdatedBy INT,
	UpdatedOn DATETIME DEFAULT GETDATE() NULL,
	CONSTRAINT PK_SupplierMaster PRIMARY KEY (SupplierID)
)
