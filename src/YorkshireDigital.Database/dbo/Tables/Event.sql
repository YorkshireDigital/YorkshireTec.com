CREATE TABLE [dbo].[Event] (
    [UniqueName]     NVARCHAR (255)  NOT NULL,
    [Title]          VARCHAR (1000)  NULL,
    [Synopsis]       VARCHAR (8000)  NULL,
    [Start]          DATETIME        NULL,
    [End]            DATETIME        NULL,
    [Location]       NVARCHAR (255)  NULL,
    [Region]         NVARCHAR (255)  NULL,
    [Price]          DECIMAL (19, 5) NULL,
    [Photo]          VARBINARY (MAX) NULL,
    [LastEditedOn]  DATETIME         NULL,
    [DeletedOn]     DATETIME         NULL,
    [GroupId] NVARCHAR (255)  NULL,
    [LastEditedById] UNIQUEIDENTIFIER NULL, 
    [DeletedById]	 UNIQUEIDENTIFIER NULL, 
    PRIMARY KEY CLUSTERED ([UniqueName] ASC),
    CONSTRAINT [FKABF0877E453DFDE3] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id]),
    CONSTRAINT [FK_Event_User_Edited] FOREIGN KEY ([LastEditedById]) REFERENCES [User]([Id]),
    CONSTRAINT [FK_Event_User_Deleted] FOREIGN KEY ([DeletedById]) REFERENCES [User]([Id])
);




GO



GO


