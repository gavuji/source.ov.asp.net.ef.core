CREATE TABLE FormulaTypeProductMapping
(
	FormulaTypeProductMapID INT IDENTITY(1,1) NOT NULL,
	FormulaTypeID INT NOT NULL,
	ProductID INT NOT NULL,
	CreatedBy INT,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_FormulaTypeProductMapping_CreatedOn DEFAULT(GETDATE()),
	UpdatedBy INT,
	UpdatedOn DATETIME,
	CONSTRAINT PK_FormulaTypeProductMapping PRIMARY KEY (FormulaTypeProductMapID),
	CONSTRAINT FK_FormulaTypeProductMapping_FormulaTypeMaster FOREIGN KEY (FormulaTypeID) REFERENCES FormulaTypeMaster(FormulaTypeID),
	CONSTRAINT FK_FormulaTypeProductMapping_ProductTypeMaster FOREIGN KEY (ProductID) REFERENCES ProductTypeMaster(ProductTypeID)
)