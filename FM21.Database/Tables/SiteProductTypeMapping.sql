CREATE TABLE SiteProductTypeMapping
(
	SiteProductMapID INT IDENTITY(1,1) NOT NULL,
	SiteID INT NOT NULL,
	ProductTypeID INT NOT NULL,
	CONSTRAINT PK_SiteProductTypeMapping PRIMARY KEY (SiteProductMapID),
	CONSTRAINT FK_SiteProductTypeMapping_SiteMaster FOREIGN KEY (SiteID) REFERENCES SiteMaster(SiteID),
	CONSTRAINT FK_SiteProductTypeMapping_ProductTypeMaster FOREIGN KEY (ProductTypeID) REFERENCES ProductTypeMaster(ProductTypeID)
)