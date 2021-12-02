CREATE TABLE Customer
(
    CustomerID INT IDENTITY(1,1) NOT NULL,
    [Name] VARCHAR(50) NOT NULL,
	Email VARCHAR(50),
	[Address] NVARCHAR(200),
	PhoneNumber VARCHAR(20) NOT NULL,
	CustomerAbbreviation1 VARCHAR(50) NOT NULL,
	CustomerAbbreviation2 VARCHAR(50),
    IsActive BIT NOT NULL CONSTRAINT DF_Customer_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_Customer_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_Customer_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_Customer PRIMARY KEY (CustomerID)
);