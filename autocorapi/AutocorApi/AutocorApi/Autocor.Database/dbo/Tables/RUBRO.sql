CREATE TABLE [dbo].[RUBRO] (
    [RUB_CODIGO]          INT             NOT NULL,
    [RUB_DESCRIPCION]     NVARCHAR (254)  DEFAULT (NULL) NULL,
    [RUB_FECHA]           DATETIME2 (0)   DEFAULT (NULL) NULL,
    [RUB_CANT_PARA]       INT             DEFAULT (NULL) NULL,
    [RUB_DESCUENTO]       DECIMAL (19, 4) DEFAULT (NULL) NULL,
    [RUB_DESCRI_CATALOGO] NVARCHAR (254)  DEFAULT (NULL) NULL,
    [RUB_FRASE]           NVARCHAR (254)  DEFAULT (NULL) NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.rubro', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RUBRO';

