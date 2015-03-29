CREATE TABLE [dbo].[Event] (
    [UniqueName]     NVARCHAR (255)  NOT NULL,
    [Photo]          VARBINARY (MAX) NULL,
    [Title]          NVARCHAR (1000) DEFAULT (NULL) NULL,
    [Synopsis]       NVARCHAR (MAX)  DEFAULT (NULL) NULL,
    [Start]          DATETIME2 (0)   DEFAULT (NULL) NULL,
    [End]            DATETIME2 (0)   DEFAULT (NULL) NULL,
    [Location]       NVARCHAR (255)  DEFAULT (NULL) NULL,
    [Region]         NVARCHAR (255)  DEFAULT (NULL) NULL,
    [Price]          DECIMAL (19, 5) DEFAULT (NULL) NULL,
    [OrganisationId] NVARCHAR (255)  DEFAULT (NULL) NULL,
    CONSTRAINT [PK_Event_UniqueName] PRIMARY KEY CLUSTERED ([UniqueName] ASC),
    CONSTRAINT [Event$FKA2FD7DF6453DFDE3] FOREIGN KEY ([OrganisationId]) REFERENCES [dbo].[organisation] ([Id]),
    CONSTRAINT [FKA2FD7DF6453DFDE3] FOREIGN KEY ([OrganisationId]) REFERENCES [dbo].[organisation] ([Id]),
    CONSTRAINT [Event$UniqueName] UNIQUE NONCLUSTERED ([UniqueName] ASC)
);


GO
CREATE NONCLUSTERED INDEX [OrganisationId]
    ON [dbo].[Event]([OrganisationId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'yorksdb.Event', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Event';

