--Site Master
SET IDENTITY_INSERT [dbo].[SiteMaster] ON 
GO
INSERT [dbo].[SiteMaster] ([SiteID], [SiteCode], [SiteDescription], [S30CodePrefix], [IsActive], [CreatedBy], [CreatedOn]) 
VALUES (1, N'ONT', N'Ontario', 6, 1, NULL, GETDATE()),
(2, N'LAC', N'Lachine', 2, 1, NULL, GETDATE()),
(3, N'ANJ', N'Anjou', 1, 1, NULL, GETDATE()),
(4, N'ANA', N'Anaheim', 4, 1, NULL, GETDATE()),
(5, N'SLC', N'Salt Lake City', 7, 1, NULL, GETDATE()),
(6, N'IRW', N'IRW',NULL,0,GETDATE())
GO
SET IDENTITY_INSERT [dbo].[SiteMaster] OFF
GO

--Product Type Master
SET IDENTITY_INSERT [dbo].[ProductTypeMaster] ON 
GO
INSERT [dbo].[ProductTypeMaster] ([ProductTypeID], [ProductType], [IsActive], [CreatedBy], [CreatedOn]) 
VALUES (1, N'Bar', 1, NULL, GETDATE()),
(2, N'Powder', 1, NULL, GETDATE())
GO
SET IDENTITY_INSERT [dbo].[ProductTypeMaster] OFF
GO

--Product Type Master
SET IDENTITY_INSERT [dbo].[SiteProductTypeMapping] ON 
GO
INSERT [dbo].[SiteProductTypeMapping] ([SiteProductMapID], [SiteID], [ProductTypeID]) 
VALUES (1, 1, 1),
(2, 2, 1),
(3, 3, 1),
(4, 4, 2),
(5, 5, 2)
GO
SET IDENTITY_INSERT [dbo].[SiteProductTypeMapping] OFF
GO

--Instruction Category Master
SET IDENTITY_INSERT [dbo].[InstructionCategoryMaster] ON 
GO
INSERT [dbo].[InstructionCategoryMaster] ([InstructionCategoryID], [InstructionCategory], [IsActive], [CreatedBy], [CreatedOn]) 
VALUES (1, N'Liquids', 1, NULL, GETDATE()),
(2, N'Mixing', 1, NULL, GETDATE()),
(3, N'After Mixing', 1, NULL, GETDATE()),
(4, N'Coating/Fondant', 1, NULL, GETDATE()),
(5, N'OPS1 - Dries', 1, NULL, GETDATE()),
(6, N'OPS2 - Liquids', 1, NULL, GETDATE()),
(7, N'OPS3 - Add to mixer', 1, NULL, GETDATE()),
(8, N'OPS4 - Top Layer', 1, NULL, GETDATE()),
(9, N'OPS5 - Ctg/Drizzle/Topicals', 1, NULL, GETDATE()),
(10, N'Other', 1, NULL, GETDATE())
GO
SET IDENTITY_INSERT [dbo].[InstructionCategoryMaster] OFF
GO

--Site Instruction Category Mapping
SET IDENTITY_INSERT [dbo].[SiteInstructionCategoryMapping] ON 
GO
INSERT [dbo].[SiteInstructionCategoryMapping] ([SiteInstructionCategoryMapID], [SiteID], [InstructionCategoryID], [CreatedBy], [CreatedOn]) 
VALUES (1, 1, 1, NULL, GETDATE()),
(2, 1, 2, NULL, GETDATE()),
(3, 1, 3, NULL, GETDATE()),
(4, 2, 5, NULL, GETDATE()),
(6, 2, 6, NULL, GETDATE()),
(7, 2, 7, NULL, GETDATE()),
(8, 2, 8, NULL, GETDATE()),
(9, 2, 9, NULL, GETDATE()),
(10, 3, 1, NULL, GETDATE()),
(11, 3, 2, NULL, GETDATE()),
(12, 3, 3, NULL, GETDATE()),
(13, 3, 4, NULL, GETDATE()),
(14, 4, 10, NULL, GETDATE()),
(15, 5, 10, NULL, GETDATE())
GO
SET IDENTITY_INSERT [dbo].[SiteInstructionCategoryMapping] OFF
GO

--Permission Master
SET IDENTITY_INSERT [dbo].[PermissionMaster] ON 
GO
INSERT [dbo].[PermissionMaster] ([PermissionID], [PermissionFor], [IsDeleted], [CreatedOn]) 
VALUES (1, N'Things that no one else can do', 0, GETDATE()),
(2, N'Manage formula instruction list', 0, GETDATE()),
(3, N'Manage customer name list', 0, GETDATE()),
(4, N'Manage supplier name list', 0, GETDATE()),
(5, N'Edit Formula Ingredient & %''s', 0, GETDATE()),
(6, N'Edit Formula Sub Designators/Instructions/Ingredient Order', 0, GETDATE()),
(7, N'Edit formula attributes', 0, GETDATE()),
(8, N'Delete/Archive a formula', 0, GETDATE()),
(9, N'Edit Ingredient data', 0, GETDATE()),
(10, N'Delete/Archive an Ingredient', 0, GETDATE()),
(11, N'Edit Food Safety data', 0, GETDATE()),
(12, N'Print Batch Sheets: Production/Lab', 0, GETDATE()),
(13, N'View/Print formula reports', 0, GETDATE()),
(14, N'View/Print ingredient reports', 0, GETDATE())
GO
SET IDENTITY_INSERT [dbo].[PermissionMaster] OFF
GO

--Claim Master
SET IDENTITY_INSERT [dbo].[ClaimMaster] ON 
GO
INSERT [dbo].[ClaimMaster] ([ClaimID], [ClaimCode], [ClaimDescription], [ClaimGroupType], [HasImpact], [IsActive], [IsDeleted], [CreatedBy], [CreatedOn]) 
VALUES (1, N'Kosher', N'Contains only Kosher ingredients', N'Kosher', 1, 1, 0, NULL, GETDATE()),
(2, N'ouDcert', N'Certified OU Dairy', N'Kosher', 1, 1, 0, NULL, GETDATE()),
(3, N'nonK', N'Contains non-Kosher ingredients', N'Kosher', 1, 1, 0, NULL, GETDATE()),
(4, N'KPcert', N'Certified Kosher Pareve', N'Kosher', 1, 1, 0, NULL, GETDATE()),
(5, N'KcertD', N'Certified Kosher Dairy', N'Kosher', 1, 1, 0, NULL, GETDATE()),
(6, N'Hcomp', N'Complies with Halal requirements', N'Halal', 0, 1, 0, NULL, GETDATE()),
(7, N'Hcert', N'IFANCA certifies that all certificates and statements meet the requirements for Halal', N'Halal', 1, 1, 0, NULL, GETDATE()),
(8, N'GFree', N'Gluten-free per FDA (<20ppm)', N'Gluten', 1, 1, 0, NULL, GETDATE()),
(9, N'GFCO', N'Gluten-free certified (<10ppm)', N'Gluten', 1, 1, 0, NULL, GETDATE()),
(10, N'nonGMO_', N'No ingredient with GM origins', N'GMO', 0, 1, 0, NULL, GETDATE()),
(11, N'GMOFree', N'No ingredient with GM detected', N'GMO', 0, 1, 0, NULL, GETDATE()),
(12, N'PV', N'NonGMO Project Verified', N'GMO', 1, 1, 0, NULL, GETDATE()),
(13, N'nonGMOsoy', N'No soy ingredient with GM origins', N'GMO', 0, 1, 0, NULL, GETDATE()),
(14, N'ORG70', N'70 - 94.9% organic ingredients', N'Organic', 0, 1, 0, NULL, GETDATE()),
(15, N'ORG95', N'95 - 99.9% organic ingredients', N'Organic', 1, 1, 0, NULL, GETDATE()),
(16, N'ORG100', N'100% organic ingredients', N'Organic', 1, 1, 0, NULL, GETDATE()),
(17, N'noSOY', N'No soy ingredient added; must use NON-SOY RELEASE AGENT', N'Other', 1, 1, 0, NULL, GETDATE()),
(18, N'noDAIRY', N'No milk-based ingredient added', N'Other', 1, 1, 0, NULL, GETDATE()),
(19, N'no_rBST', N'Milk is from cows not treated with rBST or rBGH (no added hormones)', N'Other', 0, 1, 0, NULL, GETDATE()),
(20, N'VGN_', N'Product in compliance with the no animal product requirement for vegan', N'Other', 0, 1, 0, NULL, GETDATE()),
(21, N'VGNcert', N'Certified that product contains no animal product of any kind - "vegan"', N'Other', 1, 1, 0, NULL, GETDATE()),
(22, N'VEG', N'Contains no prohibited ingredient as defined by the customer', N'Other', 0, 1, 0, NULL, GETDATE()),
(23, N'ProB', N'Contains probiotics', N'Other', 1, 1, 0, NULL, GETDATE()),
(24, N'RAC', N'All cocoa and other specified ingredients are Rainforest Alliance Certified', N'None', 0, 1, 0, NULL, GETDATE()),
(25, N'LoGly', N'Product is low glycemic (requires customer documentation)', N'None', 0, 1, 0, NULL, GETDATE()),
(26, N'Healthy', N'Check w/ regulatory (<3g fat, <1g sat fat, <480mg Na, <60mg cholesterol, >10%DV vit A, C, Ca, Fe, PRO, or FIB)', N'None', 0, 1, 0, NULL, GETDATE()),
(27, N'NSF', N'Certified by NSF with symbol on packaging [includes NSF Sport]', N'None', 0, 1, 0, NULL, GETDATE()),
(28, N'IC', N'Certified by Informed Choice with symbol on packaging', N'None', 0, 1, 0, NULL, GETDATE()),
(29, N'IS', N'Certified by Informed Sport with symbol on packaging', N'None', 0, 1, 0, NULL, GETDATE()),
(30, N'FairT', N'Customer specified ingredients are sourced from Fair Trade countries', N'None', 0, 1, 0, NULL, GETDATE()),
(31, N'GrassF', N'Customer specified ingredients are sourced from Grass-Fed animals', N'None', 0, 1, 0, NULL, GETDATE()),
(32, N'WholeG', N'Contains 8g or > whole grain and has the Whole Grain Stamp certification on the package', N'None', 0, 1, 0, NULL, GETDATE()),
(33, N'Pcomp', N'Contains ingredients that comply with the Paleo approved list', N'None', 0, 1, 0, NULL, GETDATE()),
(34, N'Pcert', N'Certified by a Paleo agency with symbol on packaging', N'None', 0, 1, 0, NULL, GETDATE())
GO
SET IDENTITY_INSERT [dbo].[ClaimMaster] OFF
GO

--Criteria Master
SET IDENTITY_INSERT [dbo].[CriteriaMaster] ON 
GO
INSERT [dbo].[CriteriaMaster] ([CriteriaID], [CriteriaDescription], [ColorCode], [CriteriaOrder], [IsActive], [IsDeleted], [CreatedBy], [CreatedOn]) 
VALUES (1, N'Uses a subassembly that needs to be made prior to the production date', N'#eca29c', N'A', 1, 0, NULL, GETDATE()),
(2, N'Uses an ingredient layer that comes in boxes (not mixed in-house)', N'#eca29c', N'B', 1, 0, NULL, GETDATE()),
(3, N'Uses pre-ground nuts', N'#eca29c', N'C', 1, 0, NULL, GETDATE()),
(4, N'Uses pre-ground soy crips', N'#eca29c', N'D', 1, 0, NULL, GETDATE()),
(5, N'Mini bar recipe [L4]', N'#f7d9ad', N'E', 1, 0, NULL, GETDATE()),
(6, N'Sticky bar recipe which requires paper on trays if bars are trayed off [L1, L2, L4)', N'#f7d9ad', N'F', 1, 0, NULL, GETDATE()),
(7, N'Soft dough-on-dough recipe - might need a Peerless mixer as backup if conical doesn''t work', N'#f7d9ad', N'G', 1, 0, NULL, GETDATE()),
(8, N'Extruded recipe [L1 Aasted, L4 Hosokawa]', N'#f7d9ad', N'H', 1, 0, NULL, GETDATE()),
(9, N'Requires the 3mm screen for liquifier/mixer', N'#cebeea', N'I', 1, 0, NULL, GETDATE()),
(10, N'Requires high temp cook (>100°C)', N'#cebeea', N'J', 1, 0, NULL, GETDATE()),
(11, N'Requires Breddo to be rinsed before making layer syrup due to syrup colour', N'#cebeea', N'K', 1, 0, NULL, GETDATE()),
(12, N'Requires a separate cook kettle for gum/pectin blend used in aerated formula [L3, L4]', N'#cebeea', N'L', 1, 0, NULL, GETDATE()),
(13, N'Requires a separate cook kettle for caramel or fruit that needs to be blended and cooked', N'#cebeea', N'M', 1, 0, NULL, GETDATE()),
(14, N'Requires a separate melter for ingredients that need to be pre-melted for the mix deck', N'#cebeea', N'N', 1, 0, NULL, GETDATE()),
(15, N'Requires a floor hold time after mixing', N'#f7d9ad', N'O', 1, 0, NULL, GETDATE()),
(16, N'Requires one topping dispenser.', N'#87ceee', N'P', 1, 0, NULL, GETDATE()),
(17, N'Requires two topping dispensers.', N'#87ceee', N'Q', 1, 0, NULL, GETDATE()),
(18, N'Requires pretzel dispenser [L4}', N'#87ceee', N'R', 1, 0, NULL, GETDATE()),
(19, N'Requires Up-and-Down slitter table', N'#f7d9ad', N'S', 1, 0, NULL, GETDATE()),
(20, N'Requires woody inside the enrober  - need melter w/ sifter', N'#8596f4', N'T', 1, 0, NULL, GETDATE()),
(21, N'Requires woody outside the enrober for different coatings - need melter w// sifter [L2, L4]	', N'#8596f4', N'U', 1, 0, NULL, GETDATE()),
(22, N'Packed out as 2 bars per wrap', N'#f7d9ad', N'V', 1, 0, NULL, GETDATE()),
(23, N'Can run on Line 1', N'#6fe974', N'W', 1, 0, NULL, GETDATE()),
(24, N'Can run on Line 2', N'#6fe974', N'X', 1, 0, NULL, GETDATE()),
(25, N'Can run on Line 3', N'#6fe974', N'Y', 1, 0, NULL, GETDATE()),
(26, N'Can run on Line 4', N'#6fe974', N'Z', 1, 0, NULL, GETDATE())
GO
SET IDENTITY_INSERT [dbo].[CriteriaMaster] OFF
GO

--Nutrient Type Master
SET IDENTITY_INSERT [dbo].[NutrientTypeMaster] ON 
GO
INSERT INTO NutrientTypeMaster
(NutrientTypeID, TypeName, IsDeleted, IsActive)
VALUES (1, 'Macro Nutrients', 0, 1),
(2, 'Calories', 0, 1),
(3, 'Vitamins/Minerals', 0, 1),
(4, 'Amino Acids', 0, 1)
GO
SET IDENTITY_INSERT [dbo].[NutrientTypeMaster] OFF