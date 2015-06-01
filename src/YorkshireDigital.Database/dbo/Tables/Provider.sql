CREATE TABLE [dbo].[Provider] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (255)   NULL,
    [PublicToken] NVARCHAR (255)   NULL,
    [SecretToken] NVARCHAR (255)   NULL,
    [ExpiresOn]   DATETIME         NULL,
    [Username]    NVARCHAR (255)   NULL,
    [UserId]      UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FKF5AF1A0BE58B1CE6] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

