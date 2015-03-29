CREATE TABLE [dbo].[User] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [Username]         NVARCHAR (255)   NULL,
    [Password]         NVARCHAR (255)   NULL,
    [Name]             NVARCHAR (255)   NULL,
    [Email]            NVARCHAR (255)   NULL,
    [MailingListEmail] NVARCHAR (255)   NULL,
    [Gender]           NVARCHAR (255)   NULL,
    [Locale]           NVARCHAR (255)   NULL,
    [Picture]          NVARCHAR (255)   NULL,
    [Validated]        BIT              NULL,
    [MailingListState] NVARCHAR (255)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



