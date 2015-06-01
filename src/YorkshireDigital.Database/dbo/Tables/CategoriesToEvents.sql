CREATE TABLE [dbo].[CategoriesToEvents] (
    [CategoryId] INT            NOT NULL,
    [EventId]    NVARCHAR (255) NOT NULL,
    CONSTRAINT [FKA188BB4D3B3A61BF] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([UniqueName]),
    CONSTRAINT [FKA188BB4D3E39DD44] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id])
);



