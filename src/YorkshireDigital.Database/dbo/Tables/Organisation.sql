CREATE TABLE [dbo].[Organisation] (
    [Id]        NVARCHAR (255)  NOT NULL,
    [Name]      NVARCHAR (255)  NULL,
    [ShortName] NVARCHAR (255)  NULL,
    [Headline]  VARCHAR (8000)  NULL,
    [About]     VARCHAR (8000)  NULL,
    [Colour]    NVARCHAR (255)  NULL,
    [Logo]      VARBINARY (MAX) NULL,
    [Photo]     VARBINARY (MAX) NULL,
    [Website]   NVARCHAR (255)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO


