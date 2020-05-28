CREATE TABLE [dbo].[UsuariosGlobal] (
    [IdUsuario]        INT            IDENTITY (1, 1) NOT NULL,
    [Usuario]          NVARCHAR (20)  NOT NULL,
    [Nombre]           NVARCHAR (150) NULL,
    [FechaCreacionUtc] DATETIME       CONSTRAINT [DF_UsuariosGlobal_FechaCreacionUtc] DEFAULT (getutcdate()) NOT NULL,
    [Activo]           BIT            CONSTRAINT [DF_UsuariosGlobal_Activo] DEFAULT ((0)) NOT NULL,
    [Rol]              NVARCHAR (10)  NOT NULL,
    [Password]         NVARCHAR (255) NOT NULL,
    [CodigoCliente]    FLOAT (53)     NULL,
    [ZonaViajante]     INT            NULL,
    CONSTRAINT [PK_UsuariosGlobal] PRIMARY KEY CLUSTERED ([IdUsuario] ASC)
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UsuariosGlobal]
    ON [dbo].[UsuariosGlobal]([Usuario] ASC);

