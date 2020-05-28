CREATE TABLE [dbo].[STOCK_EQUIVALENCIA] (
    [CODPIEZA]   NVARCHAR (7) DEFAULT (NULL) NULL,
    [CODPIEZA_E] NVARCHAR (7) DEFAULT (NULL) NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.stock_equivalencia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STOCK_EQUIVALENCIA';

