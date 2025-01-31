CREATE TABLE [dbo].[Clients]
(
    [Id] INT NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [ClientCode] NVARCHAR(6) NOT NULL,
    [Description] NVARCHAR(MAX) NULL
);