/*
 *	Data for [User]
 */
MERGE INTO [User] AS Target
USING (VALUES
	(N'D18D19DC-5324-479C-8800-384696EB3248', N'yorksdigital', N'AANkOyQS4vybWtqKmAtri43TV89rTRmhnAb02T/jPfV9chYmQ/UI9uUEjLF0fVcrlA==', N'Yorkshire Digital', N'info@yorkshiredigital.com', NULL, N'Unknown', NULL, NULL, 0, N'Unsubscribed')
)
AS Source([Id], [Username], [Password], [Name], [Email], [MailingListEmail], [Gender], [Locale], [Picture], [Validated], [MailingListState])
ON Target.[Id] = Source.[Id]
WHEN NOT MATCHED BY Target THEN
	INSERT ([Id], [Username], [Password], [Name], [Email], [MailingListEmail], [Gender], [Locale], [Picture], [Validated], [MailingListState])
	VALUES ([Id], [Username], [Password], [Name], [Email], [MailingListEmail], [Gender], [Locale], [Picture], [Validated], [MailingListState])
WHEN MATCHED THEN
	UPDATE SET	Username = source.Username,
				Password = source.Password,
				Name = source.Name,
				Email = source.Email,
				MailingListEmail = source.MailingListEmail,
				Gender = source.Gender,
				Locale = source.Locale,
				Picture = source.Picture,
				Validated = source.Validated,
				MailingListState = source.MailingListState
;