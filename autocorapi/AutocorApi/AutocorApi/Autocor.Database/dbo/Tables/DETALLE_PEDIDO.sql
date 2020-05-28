CREATE TABLE [dbo].[DETALLE_PEDIDO] (
    [ID_PEDIDO]       INT             NOT NULL,
    [COD_PIEZA]       NVARCHAR (7)    NOT NULL,
    [CANTIDAD]        INT             NOT NULL,
    [PRECIO_UNITARIO] DECIMAL (19, 4) NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.detalle_pedido', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DETALLE_PEDIDO';

