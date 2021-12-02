CREATE TABLE RegulatoryMaster
(
	RegulatoryID INT IDENTITY(1,1) NOT NULL,
	NutrientID INT NOT NULL,
	OldUsa INT,
	CanadaNi INT,
	CanadaNf INT,
	NewUsRdi INT,
	Eu INT,
	Unit VARCHAR(50) NOT NULL,
	UnitPerMg INT NOT NULL,
	Active BIT NOT NULL CONSTRAINT DF_RegulatoryMaster_IsActive DEFAULT(1),
	IsDeleted BIT NOT Null CONSTRAINT DF_RegulatoryMaster_IsDeleted DEFAULT(0),
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_RegulatoryMaster_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_RegulatoryMaster PRIMARY KEY (RegulatoryID),
	CONSTRAINT FK_RegulatoryMaster_NutrientMaster FOREIGN KEY (NutrientID) REFERENCES NutrientMaster(NutrientID)
)