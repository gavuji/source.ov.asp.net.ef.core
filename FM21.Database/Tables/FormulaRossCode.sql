CREATE TABLE FormulaRossCode
(
	FormulaRossCodeID INT IDENTITY(1,1) NOT NULL,
	SiteID INT NOT NULL,
	IncrementNumber int NOT NULL,
	CustomerName VARCHAR(10) NULL,
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_FormulaRossCode_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_FormulaRossCode PRIMARY KEY (FormulaRossCodeID),
	CONSTRAINT FK_FormulaRossCode_SiteMaster FOREIGN KEY (SiteID) REFERENCES SiteMaster(SiteID)
)