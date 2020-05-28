CREATE TABLE [dbo].[FECHA] (
    [fecha_actualizacion] DATETIME2 (0)   DEFAULT (NULL) NULL,
    [porcentaje]          DECIMAL (19, 4) DEFAULT ((0.0000)) NULL,
    [descuento]           DECIMAL (19, 4) DEFAULT ((0.0000)) NULL,
    [empresa]             NVARCHAR (100)  DEFAULT (NULL) NULL,
    [domicilio]           NVARCHAR (100)  DEFAULT (NULL) NULL,
    [telefono]            NVARCHAR (100)  DEFAULT (NULL) NULL,
    [localidad]           NVARCHAR (100)  DEFAULT (NULL) NULL,
    [provincia]           NVARCHAR (100)  DEFAULT (NULL) NULL,
    [mail]                NVARCHAR (100)  DEFAULT (NULL) NULL,
    [noticias]            NVARCHAR (254)  DEFAULT (NULL) NULL,
    [mail_autocor]        NVARCHAR (100)  DEFAULT (NULL) NULL,
    [host]                NVARCHAR (50)   DEFAULT (NULL) NULL,
    [usr]                 NVARCHAR (50)   DEFAULT (NULL) NULL,
    [psw]                 NVARCHAR (50)   DEFAULT (NULL) NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.fecha', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FECHA';

