CREATE TABLE [dbo].[stock_grande] (
    [codpieza] NVARCHAR (50)  DEFAULT (NULL) NULL,
    [descri]   NVARCHAR (MAX) NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.stock_grande', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'stock_grande';

