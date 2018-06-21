CREATE TABLE [dbo].[Quiz] (
    [Id]       INT          NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [Archived] BIT          NOT NULL,
    CONSTRAINT [PK_Quiz] PRIMARY KEY CLUSTERED ([Id] ASC)
);

