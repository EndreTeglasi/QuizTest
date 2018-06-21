CREATE TABLE [dbo].[Question] (
    [Id]             INT           NOT NULL,
    [Question]       VARCHAR (MAX) NOT NULL,
    [Point]          INT           NOT NULL,
    [QuestionTypeId] INT           NOT NULL,
    [QuizId]         INT           NOT NULL,
    CONSTRAINT [PK_Question] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Question_QuestionType1] FOREIGN KEY ([QuestionTypeId]) REFERENCES [dbo].[QuestionType] ([Id]),
    CONSTRAINT [FK_Question_Quiz] FOREIGN KEY ([QuizId]) REFERENCES [dbo].[Quiz] ([Id])
);

