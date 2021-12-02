CREATE TABLE SiteProductionLineMapping
(
	SiteProductionLineMapID INT IDENTITY(1,1) NOT NULL,
	SiteID INT NOT NULL,
	ProductionLineID INT NOT NULL,
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_SiteProductionLineMapping_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_SiteProductionLineMapping PRIMARY KEY (SiteProductionLineMapID),
	CONSTRAINT FK_SiteProductionLineMapping_SiteMaster FOREIGN KEY (SiteID) REFERENCES SiteMaster(SiteID),
	CONSTRAINT FK_SiteProductionLineMapping_ProductionLineMaster FOREIGN KEY (ProductionLineID) REFERENCES ProductionLineMaster(ProductionLineID)
)