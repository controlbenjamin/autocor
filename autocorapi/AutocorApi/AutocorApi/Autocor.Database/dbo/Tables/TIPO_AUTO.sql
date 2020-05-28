CREATE TABLE [dbo].[TIPO_AUTO] (
    [TIP_CODIGO]      INT            NOT NULL,
    [MAR_CODIGO]      NVARCHAR (1)   DEFAULT (NULL) NULL,
    [TIP_DESCRIPCION] NVARCHAR (254) DEFAULT (NULL) NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.tipo_auto', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TIPO_AUTO';

