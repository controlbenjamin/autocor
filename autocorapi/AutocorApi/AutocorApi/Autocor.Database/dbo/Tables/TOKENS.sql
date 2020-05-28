CREATE TABLE [dbo].[TOKENS] (
    [Id]              CHAR (32)      NOT NULL,
    [IdUsuario]       INT            NOT NULL,
    [ClientId]        CHAR (36)      NOT NULL,
    [IssuedUTC]       DATETIME       NOT NULL,
    [ExpiresUTC]      DATETIME       NOT NULL,
    [ProtectedTicket] VARCHAR (1500) NOT NULL,
    CONSTRAINT [PK_TOKENS] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TOKENS_UsuariosGlobal] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[UsuariosGlobal] ([IdUsuario])
);

