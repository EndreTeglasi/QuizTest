CREATE TABLE [dbo].[Avatar] (
    [Id]   INT             IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX)  NOT NULL,
    [Data] VARBINARY (MAX) NOT NULL,
    CONSTRAINT [PK_Avatar] PRIMARY KEY CLUSTERED ([Id] ASC)
);



