SET IDENTITY_INSERT [bplt].[Role] ON
	INSERT INTO [bplt].[Role] (RoleId, Name, Created) VALUES (1, 'Administrator', GetDate())
	INSERT INTO [bplt].[Role] (RoleId, Name, Created) VALUES (2, 'Super Administrator', GetDate())
SET IDENTITY_INSERT [bplt].[Role] OFF

SET IDENTITY_INSERT [bplt].[Permission] ON
	INSERT INTO [bplt].[Permission] (PermissionId, [RoleId], Name, Created) VALUES (1, 1, 'Admin', GetDate())
	INSERT INTO [bplt].[Permission] (PermissionId, [RoleId], Name, Created) VALUES (2, 2, 'SuperAdmin', GetDate())
SET IDENTITY_INSERT [bplt].[Permission] OFF

SET IDENTITY_INSERT [bplt].[Partner] ON
	INSERT INTO [bplt].[Partner] (PartnerId, Name, [Disabled], Created) values (1, 'Partner', 0, GetDate())
SET IDENTITY_INSERT [bplt].[Partner] OFF

SET IDENTITY_INSERT [bplt].[User] ON
	INSERT INTO [bplt].[User] (UserId, RoleId, Name, Email, Deleted, Created) VALUES (1, 1, 'Mr John Smith', 'john.smith@tetsuper.com', 0, GetDate())
	INSERT INTO [bplt].[User] (UserId, RoleId, Name, Email, Deleted, Created) VALUES (2, 2, 'Mrs Wo Smith', 'wo.smith@tetsuper.com', 0, GetDate())
SET IDENTITY_INSERT [bplt].[User] OFF

SET IDENTITY_INSERT [bplt].[UserPasswordCredential] ON
	INSERT INTO [bplt].[UserPasswordCredential] (UserPasswordCredentialId, UserId, [Login], PasswordHash, PasswordSalt, Created) values (1, 1, 'admin', 'tzz372n5WtIAgY5SvSclpfUyaRuXoMpEgMiRnMptbf29iqGm1RVznw==', 'fMjtdJTx2XBLdpmDXWJV', GetDate())
	INSERT INTO [bplt].[UserPasswordCredential] (UserPasswordCredentialId, UserId, [Login], PasswordHash, PasswordSalt, Created) values (2, 2, 'super', 'kLtziaXFt7C7lVZN6LNRXj9lb+Ybhzc1/ymyNvfJm/ZjUltvXMxyyg==', '7CC5dg2d/sYexAMJ/k7+', GetDate())
SET IDENTITY_INSERT [bplt].[UserPasswordCredential] OFF

SET IDENTITY_INSERT [bplt].[PartnerUser] ON
	INSERT INTO [bplt].[PartnerUser] (PartnerUserId, PartnerId, UserId) VALUES (1, 1, 1)
	INSERT INTO [bplt].[PartnerUser] (PartnerUserId, PartnerId, UserId) VALUES (2, 1, 2)
SET IDENTITY_INSERT [bplt].[PartnerUser] OFF
