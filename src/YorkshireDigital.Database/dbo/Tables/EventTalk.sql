CREATE TABLE [dbo].[EventTalk] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Title]    NVARCHAR (255) NULL,
    [Synopsis] VARCHAR (8000) NULL,
    [SynopsisFormat] VARCHAR (50)	 NULL,
    [Speaker]  NVARCHAR (255) NULL,
    [Link]     NVARCHAR (255) NULL,
    [EventId]  NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK92CC0FEE3B3A61BF] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([UniqueName])
);




GO



GO


