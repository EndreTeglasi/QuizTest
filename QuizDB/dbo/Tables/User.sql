CREATE TABLE [dbo].[User] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [Email]      VARCHAR (50) NOT NULL,
    [Password]   VARCHAR (50) NOT NULL,
    [AvatarId]   INT          NOT NULL,
    [UserTypeId] INT          NOT NULL,
    [NickName]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_Avatar1] FOREIGN KEY ([AvatarId]) REFERENCES [dbo].[Avatar] ([Id]),
    CONSTRAINT [FK_User_UserType1] FOREIGN KEY ([UserTypeId]) REFERENCES [dbo].[UserType] ([Id])
);

