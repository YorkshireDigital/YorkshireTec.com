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
    [GroupId] NVARCHAR (255)  NULL,
    PRIMARY KEY CLUSTERED ([UniqueName] ASC),
    CONSTRAINT [FKABF0877E453DFDE3] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id])
);




GO



GO


