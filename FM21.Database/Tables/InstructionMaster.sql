CREATE TABLE InstructionMaster 
(
    InstructionMasterID INT IDENTITY(1,1) NOT NULL,
	SiteProductMapID INT NOT NULL,
	InstructionCategoryID INT NOT NULL,
	InstructionGroupID INT NOT NULL,
    DescriptionEn VARCHAR(150) NOT NULL,
	DescriptionFr NVARCHAR(150),
	DescriptionEs NVARCHAR(150),
	GroupDisplayOrder INT NOT NULL,
	GroupItemDisplayOrder INT NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_InstructionMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT DF_InstructionMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_InstructionMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_InstructionMaster PRIMARY KEY (InstructionMasterID),
	CONSTRAINT FK_InstructionMasterSiteProductTypeMapping FOREIGN KEY (SiteProductMapID) REFERENCES SiteProductTypeMapping(SiteProductMapID),
	CONSTRAINT FK_InstructionMasterInstructionCategory FOREIGN KEY (InstructionCategoryID) REFERENCES InstructionCategoryMaster(InstructionCategoryID),
	CONSTRAINT FK_InstructionMasterInstructionGroupMaster FOREIGN KEY (InstructionGroupID) REFERENCES InstructionGroupMaster(InstructionGroupID)
);