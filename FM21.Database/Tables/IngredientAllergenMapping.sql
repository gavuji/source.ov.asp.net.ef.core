CREATE TABLE IngredientAllergenMapping
(
	IngredientAllergenMapID INT IDENTITY(1,1) NOT NULL,
	IngredientID INT NOT NULL,
	AllergenID INT NOT NULL,
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_IngredientAllergenMapping_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_IngredientAllergenMapping PRIMARY KEY (IngredientAllergenMapID),
	CONSTRAINT FK_IngredientAllergenMapping_AllergenMaster FOREIGN KEY (AllergenID) REFERENCES AllergenMaster(AllergenID),
	CONSTRAINT FK_IngredientAllergenMapping_IngredientMaster FOREIGN KEY (IngredientID) REFERENCES IngredientMaster(IngredientID)
)