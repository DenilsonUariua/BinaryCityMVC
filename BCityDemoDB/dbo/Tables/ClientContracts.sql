CREATE TABLE [dbo].[ClientContacts]
(
    [ClientId] INT NOT NULL,
    [ContactId] INT NOT NULL,
    CONSTRAINT [PK_ClientContacts] PRIMARY KEY ([ClientId], [ContactId]),
    CONSTRAINT [FK_ClientContacts_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ClientContacts_Contacts_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [Contacts]([Id]) ON DELETE CASCADE
);