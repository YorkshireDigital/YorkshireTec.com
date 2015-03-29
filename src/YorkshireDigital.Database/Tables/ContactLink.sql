CREATE TABLE [dbo].[ContactLink] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Type]           NVARCHAR (255) NULL,
    [Value]          NVARCHAR (255) NULL,
    [OrganisationId] NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

