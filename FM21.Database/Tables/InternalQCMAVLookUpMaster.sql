
CREATE TABLE [dbo].[InternalQCMAVLookUpMaster](
	[InternalQCMAVLookUpMasterID] [int] IDENTITY(1,1) NOT NULL,
	[TotalBarWeight] [numeric](18,2) NOT NULL,
	[Subtract] [numeric](18,2) NOT NULL,
	[IsActive] [bit] DEFAULT(1),
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NOT NULL DEFAULT(GetDATE()),
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_InternalQCMAVLookUpMaster] PRIMARY KEY CLUSTERED 
(
	[InternalQCMAVLookUpMasterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

