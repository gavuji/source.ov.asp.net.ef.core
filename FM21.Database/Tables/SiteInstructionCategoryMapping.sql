CREATE TABLE SiteInstructionCategoryMapping
(
	SiteInstructionCategoryMapID INT IDENTITY(1,1) NOT NULL,
	SiteID INT NOT NULL,
	InstructionCategoryID INT NOT NULL,
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_SiteInstructionCategoryMapping_CreatedOn DEFAULT(GETDATE()),
	CONSTRAINT PK_SiteInstructionCategoryMapping PRIMARY KEY (SiteInstructionCategoryMapID),
	CONSTRAINT FK_InstructionSiteMapInstructionCategory FOREIGN KEY (InstructionCategoryID) REFERENCES InstructionCategoryMaster(InstructionCategoryID),
	CONSTRAINT FK_InstructionSiteMapSiteMaster FOREIGN KEY (SiteID) REFERENCES SiteMaster(SiteID)
)