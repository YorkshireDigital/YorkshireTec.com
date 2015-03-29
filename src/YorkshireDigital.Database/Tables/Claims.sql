CREATE TABLE [dbo].[Claims] (
    [UserRoleId] INT            NOT NULL,
    [Value]      NVARCHAR (255) NULL,
    CONSTRAINT [FKD081AB5561C53527] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[UserRole] ([Id])
);

