CREATE TABLE [dbo].[ContactLink] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Type]           NVARCHAR (255) NULL,
    [Value]          NVARCHAR (255) NULL,
    [GroupId] NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FKFC3DA0BB453DFDE3] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id])
);



