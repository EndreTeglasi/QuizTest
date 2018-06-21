CREATE TABLE [dbo].[UserQuiz] (
    [UserId] INT NOT NULL,
    [QuizId] INT NOT NULL,
    CONSTRAINT [PK_UserQuiz] PRIMARY KEY CLUSTERED ([UserId] ASC, [QuizId] ASC),
    CONSTRAINT [FK_UserQuiz_Quiz] FOREIGN KEY ([QuizId]) REFERENCES [dbo].[Quiz] ([Id]),
    CONSTRAINT [FK_UserQuiz_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

