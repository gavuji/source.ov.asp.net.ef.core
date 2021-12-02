CREATE TABLE [dbo].[PermissionMaster](
	[PermissionID] [int] IDENTITY(1,1) NOT NULL,
	[PermissionFor] [varchar](100) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_PermissionMaster] PRIMARY KEY CLUSTERED 
(
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PermissionMaster] ADD  CONSTRAINT [DF__Permissio__Statu__08B54D69]  DEFAULT ((1)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[PermissionMaster] ADD  CONSTRAINT [DF__Permissio__Creat__09A971A2]  DEFAULT (getdate()) FOR [CreatedOn]
GO