CREATE TABLE [dbo].[UserRole] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Role]   NVARCHAR (255) NULL,
    [Claims] NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



