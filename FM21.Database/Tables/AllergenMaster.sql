CREATE TABLE [dbo].[AllergenMaster]
(
	[AllergenID] INT IDENTITY(1,1) NOT NULL, 
    [AllergenCode] VARCHAR(10) NOT NULL, 
    [AllergenName] varchar(50) Not Null,
    [AllergenDescription_En] VARCHAR(50) NULL, 
    [AllergenDescription_Fr] NVARCHAR(100) NULL, 
    [AllergenDescription_Es] NVARCHAR(100) NULL, 
    [IsDeleted] BIT NULL DEFAULT 0, 
    [IsActive] BIT NULL DEFAULT 1,
    CreatedBy INT,
	CreatedOn DATETIME DEFAULT GETDATE() NOT NULL,
	UpdatedBy INT,
	UpdatedOn DATETIME DEFAULT GETDATE() NULL,
	[IsUSAAllergen] BIT NOT NULL CONSTRAINT DF_AllergenMaster_IsUSAAllergen DEFAULT(0),
    [IsCANADAAllergen] BIT NOT NULL CONSTRAINT DF_AllergenMaster_IsCANADAAllergen DEFAULT(0)
    CONSTRAINT PK_AllergenMaster PRIMARY KEY (AllergenID)
)
