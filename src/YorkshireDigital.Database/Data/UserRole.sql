/*
 *	Data for [UserRole]
 */
SET IDENTITY_INSERT [UserRole] ON

MERGE INTO [UserRole] AS Target
USING (VALUES
	(1, N'Admin', N'Admin')
)
AS Source([Id], [Role], [Claims])
ON Target.[Id] = Source.[Id]
WHEN NOT MATCHED BY Target THEN
	INSERT ([Id], [Role], [Claims])
	VALUES ([Id], [Role], [Claims])
WHEN MATCHED THEN
	UPDATE SET	Role = source.Role,
				Claims = source.Claims
;

SET IDENTITY_INSERT [UserRole] OFF