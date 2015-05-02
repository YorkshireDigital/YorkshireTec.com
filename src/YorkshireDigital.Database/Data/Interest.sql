/*
 *	Data for [Interest]
 */
SET IDENTITY_INSERT [Interest] ON

MERGE INTO [Interest] AS Target
USING (VALUES
	(1, N'Development'),
	(2, N'PHP'),
	(3, N'DevOps'),
	(4, N'Design'),
	(5, N'.NET'),
	(6, N'Ruby'),
	(7, N'Lean Startup'),
	(8, N'UX'),
	(9, N'Hacks'),
	(10, N'Bitcoin'),
	(11, N'Testing'),
	(12, N'Functional Programming'),
	(13, N'Game Development'),
	(14, N'JavaScript'),
	(15, N'SharePoint'),
	(16, N'Security'),
	(17, N'Amazon Web Services'),
	(18, N'Ecommerce'),
	(19, N'Salesforce'),
	(20, N'Business'),
	(21, N'Wordpress'),
	(22, N'Agile')
)
AS Source([Id], [Name])
ON Target.[Id] = Source.[Id]
WHEN NOT MATCHED BY Target THEN
	INSERT ([Id], [Name])
	VALUES ([Id], [Name])
WHEN MATCHED THEN
	UPDATE SET	[Name] = source.[Name]
;

SET IDENTITY_INSERT [Interest] OFF