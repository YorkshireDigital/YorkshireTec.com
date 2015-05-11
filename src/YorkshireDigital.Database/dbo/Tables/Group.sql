CREATE TABLE [dbo].[Group] (
    [Id]        NVARCHAR (255)  NOT NULL,
    [Name]      NVARCHAR (255)  NULL,
    [ShortName] NVARCHAR (255)  NULL,
    [Headline]  VARCHAR (8000)  NULL,
    [About]     VARCHAR (8000)  NULL,
    [Colour]    NVARCHAR (255)  NULL,
    [Logo]      VARBINARY (MAX) NULL,
    [Photo]     VARBINARY (MAX) NULL,
    [Website]   NVARCHAR (255)  NULL,
    [LastEditedOn]  DATETIME    NULL,
    [DeletedOn] DATETIME        NULL,
    [LastEditedById] UNIQUEIDENTIFIER NULL, 
    [DeletedById]	 UNIQUEIDENTIFIER NULL, 
    [MeetupId]	INT			NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Group_User_Edited] FOREIGN KEY ([LastEditedById]) REFERENCES [User]([Id]),
    CONSTRAINT [FK_Group_User_Deleted] FOREIGN KEY ([DeletedById]) REFERENCES [User]([Id])
);




GO


