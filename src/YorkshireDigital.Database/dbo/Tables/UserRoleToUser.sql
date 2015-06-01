CREATE TABLE [dbo].[UserRoleToUser] (
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [UserRoleId] INT              NOT NULL,
    CONSTRAINT [FK7F17E34261C53527] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[UserRole] ([Id]),
    CONSTRAINT [FK7F17E342E58B1CE6] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

