CREATE TABLE FormulaChangeCode
(
	FormulaChangeCodeID INT IDENTITY(1,1) NOT NULL,
	FormulaTypeID INT NOT NULL,
	SiteID INT NOT NULL,
	IncrementNumber int NOT NULL,
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_FormulaChangeCode_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_FormulaChangeCode PRIMARY KEY (FormulaChangeCodeID),
	CONSTRAINT FK_FormulaChangeCode_FormulaTypeMaster FOREIGN KEY (FormulaTypeID) REFERENCES FormulaTypeMaster(FormulaTypeID),
	CONSTRAINT FK_FormulaChangeCode_SiteMaster FOREIGN KEY (SiteID) REFERENCES SiteMaster(SiteID)
)