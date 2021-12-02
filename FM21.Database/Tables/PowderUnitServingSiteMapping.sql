CREATE TABLE PowderUnitServingSiteMapping
(
	PowderUnitServingSiteMapID INT IDENTITY(1,1) NOT NULL,
	PowderUnitServingID INT NOT NULL,
	SiteProductMapID INT NOT NULL,
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_PowderUnitServingSiteMapping_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_PowderUnitServingSiteMapping PRIMARY KEY (PowderUnitServingSiteMapID),
	CONSTRAINT FK_PowderUnitServingSiteMapping_UnitServingMaster FOREIGN KEY (PowderUnitServingID) REFERENCES UnitServingMaster(UnitServingID),
	CONSTRAINT FK_PowderUnitServingSiteMapping_SiteProductTypeMapping FOREIGN KEY (SiteProductMapID) REFERENCES SiteProductTypeMapping(SiteProductMapID)
)