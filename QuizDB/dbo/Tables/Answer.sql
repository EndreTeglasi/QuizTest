CREATE TABLE [dbo].[Answer] (
    [Id]          INT          NOT NULL,
    [Answer]      VARCHAR (50) NOT NULL,
    [QuestionId]  INT          NOT NULL,
    [AnswerCheck] BIT          NOT NULL,
    CONSTRAINT [PK_Answer] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Answer_Question] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Question] ([Id])
);

