CREATE TABLE [dbo].[EventsToInterests] (
    [EventId]    NVARCHAR (255) NOT NULL,
    [InterestId] INT            NOT NULL,
    CONSTRAINT [FK57286C043B3A61BF] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([UniqueName]),
    CONSTRAINT [FK57286C0496FD13BC] FOREIGN KEY ([InterestId]) REFERENCES [dbo].[Interest] ([Id])
);



