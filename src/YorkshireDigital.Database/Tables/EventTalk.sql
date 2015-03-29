CREATE TABLE [dbo].[EventTalk] (
    [Id]       INT            IDENTITY (141, 1) NOT NULL,
    [Synopsis] NVARCHAR (MAX) DEFAULT (NULL) NULL,
    [Title]    NVARCHAR (255) DEFAULT (NULL) NULL,
    [Speaker]  NVARCHAR (255) DEFAULT (NULL) NULL,
    [Link]     NVARCHAR (255) DEFAULT (NULL) NULL,
    [EventId]  NVARCHAR (255) DEFAULT (NULL) NULL,
    CONSTRAINT [PK_EventTalk_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [EventTalk$FK92CC0FEE3B3A61BF] FOREIGN KEY ([EventId]) REFERENCES [dbo].[event] ([UniqueName]),
    CONSTRAINT [FK92CC0FEE3B3A61BF] FOREIGN KEY ([EventId]) REFERENCES [dbo].[event] ([UniqueName]),
    CONSTRAINT [EventTalk$Id] UNIQUE NONCLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [EventId]
    ON [dbo].[EventTalk]([EventId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'yorksdb.EventTalk', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EventTalk';

