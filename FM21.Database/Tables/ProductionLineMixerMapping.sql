CREATE TABLE ProductionLineMixerMapping
(
	ProductionLineMixerMapID INT IDENTITY(1,1) NOT NULL,
	ProductionMixerID INT NOT NULL,
	SiteProductionLineID INT NOT NULL,
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_ProductionLineMixerMapping_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_ProductionLineMixerMapping PRIMARY KEY (ProductionLineMixerMapID),
	CONSTRAINT FK_ProductionLineMixerMapping_ProductionMixerMaster FOREIGN KEY (ProductionMixerID) REFERENCES ProductionMixerMaster(ProductionMixerID),
	CONSTRAINT FK_ProductionLineMixerMapping_SiteProductionLineMapping FOREIGN KEY (SiteProductionLineID) REFERENCES SiteProductionLineMapping(SiteProductionLineMapID)
)