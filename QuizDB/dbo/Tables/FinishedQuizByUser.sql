CREATE TABLE [dbo].[FinishedQuizByUser] (
    [UserId]     INT      NOT NULL,
    [QuizId]     INT      NOT NULL,
    [QuestionId] INT      NOT NULL,
    [Score]      INT      NOT NULL,
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [Date]       DATETIME NOT NULL,
    CONSTRAINT [PK_FinishedQuizByUser] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FinishedQuizByUser_Question] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Question] ([Id]),
    CONSTRAINT [FK_FinishedQuizByUser_Quiz] FOREIGN KEY ([QuizId]) REFERENCES [dbo].[Quiz] ([Id]),
    CONSTRAINT [FK_FinishedQuizByUser_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

