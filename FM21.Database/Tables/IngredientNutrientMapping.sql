CREATE TABLE IngredientNutrientMapping
(
	IngredientNutrientMapID INT IDENTITY(1,1) NOT NULL,
	IngredientID INT NOT NULL,
	NutrientID INT NOT NULL,
	NutrientValue DECIMAL(30, 15),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_IngredientNutrientMapping_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_IngredientNutrientMapping PRIMARY KEY (IngredientNutrientMapID),
	CONSTRAINT FK_IngredientNutrientMapping_IngredientMaster FOREIGN KEY (IngredientID) REFERENCES IngredientMaster(IngredientID),
	CONSTRAINT FK_IngredientNutrientMapping_NutrientMaster FOREIGN KEY (NutrientID) REFERENCES NutrientMaster(NutrientID)
)