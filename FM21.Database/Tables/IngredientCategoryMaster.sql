CREATE TABLE IngredientCategoryMaster
(
	IngredientCategoryID INT IDENTITY(1,1) NOT NULL,
	IngredientCategoryCode VARCHAR(20) NOT NULL,
	IngredientCategoryDescription VARCHAR(300) NOT NULL,
	IngredientCategoryGeneralDescription VARCHAR(100),
	IsSubAssemblyCategory BIT NOT NULL CONSTRAINT DF_IngredientCategoryMaster_IsSubAssemblyCategory DEFAULT(0),
	IsActive BIT NOT NULL CONSTRAINT DF_IngredientCategoryMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_IngredientCategoryMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_IngredientCategoryMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_IngredientCategoryMaster PRIMARY KEY (IngredientCategoryID)
)