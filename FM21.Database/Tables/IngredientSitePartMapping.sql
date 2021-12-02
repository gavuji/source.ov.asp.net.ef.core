CREATE TABLE IngredientSitePartMapping
(
	IngredientSitePartMapID INT IDENTITY(1,1) NOT NULL,
	IngredientID INT NOT NULL,
	SiteID INT NOT NULL,
	PartNumber VARCHAR(50) NOT NULL,
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_IngredientSitePartMapping_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_IngredientSitePartMapping PRIMARY KEY (IngredientSitePartMapID),
	CONSTRAINT FK_IngredientSitePartMapping_IngredientMaster FOREIGN KEY (IngredientID) REFERENCES IngredientMaster(IngredientID),
	CONSTRAINT FK_IngredientSitePartMapping_SiteMaster FOREIGN KEY (SiteID) REFERENCES SiteMaster(SiteID)
)