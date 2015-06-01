/*
 *	Data for [UserRole]
 */
MERGE INTO [UserRoleToUser] AS Target
USING (VALUES
	(N'D18D19DC-5324-479C-8800-384696EB3248', 1)
)
AS Source([UserId], [UserRoleId])
ON  Target.[UserId] = Source.[UserId]
AND Target.[UserRoleId] = Source.[UserRoleId]
WHEN NOT MATCHED BY Target THEN
	INSERT ([UserId], [UserRoleId])
	VALUES ([UserId], [UserRoleId])
;