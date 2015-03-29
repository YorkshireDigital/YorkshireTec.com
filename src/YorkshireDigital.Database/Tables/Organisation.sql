CREATE TABLE [dbo].[Organisation] (
    [Id]        NVARCHAR (255)  NOT NULL,
    [Headline]  NVARCHAR (MAX)  DEFAULT (NULL) NULL,
    [About]     NVARCHAR (MAX)  DEFAULT (NULL) NULL,
    [Name]      NVARCHAR (255)  DEFAULT (NULL) NULL,
    [ShortName] NVARCHAR (255)  DEFAULT (NULL) NULL,
    [Colour]    NVARCHAR (255)  DEFAULT (NULL) NULL,
    [Logo]      VARBINARY (MAX) NULL,
    [Photo]     VARBINARY (MAX) NULL,
    [Website]   NVARCHAR (255)  DEFAULT (NULL) NULL,
    CONSTRAINT [PK_Organisation_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'yorksdb.Organisation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Organisation';

