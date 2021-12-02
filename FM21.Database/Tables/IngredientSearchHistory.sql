CREATE TABLE IngredientSearchHistory
(
	IngredientSearchID INT IDENTITY(1,1) NOT NULL,
	UserID INT NOT NULL,
	SearchData VARCHAR(100) NOT NULL,
	SearchDate DATETIME NOT NULL,
	CONSTRAINT PK_IngredientSearchHistory PRIMARY KEY (IngredientSearchID)
)