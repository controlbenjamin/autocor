CREATE TABLE [dbo].[ApiClients] (
    [IdCliente] UNIQUEIDENTIFIER CONSTRAINT [DF_API_CLIENTS_IdCliente] DEFAULT (newid()) NOT NULL,
    [Nombre]    NVARCHAR (50)    NOT NULL,
    [Activo]    BIT              NOT NULL,
    [Roles]     NVARCHAR (150)   NULL,
    CONSTRAINT [PK_API_CLIENTS] PRIMARY KEY CLUSTERED ([IdCliente] ASC)
);

