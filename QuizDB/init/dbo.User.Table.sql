SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [Email], [Password], [AvatarId], [UserTypeId], [NickName]) VALUES (0, N'valami@valami.com', N'valami', 0, 2, N'Valami')
INSERT [dbo].[User] ([Id], [Email], [Password], [AvatarId], [UserTypeId], [NickName]) VALUES (8, N'admin@admin.com', N'admin', 0, 1, N'Admin')
INSERT [dbo].[User] ([Id], [Email], [Password], [AvatarId], [UserTypeId], [NickName]) VALUES (9, N'miki@mik.com', N'miki', 1, 2, N'Miki')
INSERT [dbo].[User] ([Id], [Email], [Password], [AvatarId], [UserTypeId], [NickName]) VALUES (10, N'demo@demo.com', N'demo', 1, 2, N'Demo')
INSERT [dbo].[User] ([Id], [Email], [Password], [AvatarId], [UserTypeId], [NickName]) VALUES (11, N'dodo@dodo.com', N'dodo', 0, 2, N'Dodo')

SET IDENTITY_INSERT [dbo].[User] OFF 