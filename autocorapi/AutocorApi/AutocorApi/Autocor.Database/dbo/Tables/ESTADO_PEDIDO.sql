CREATE TABLE [dbo].[ESTADO_PEDIDO] (
    [ID_ESTADO]   INT          NOT NULL,
    [DESCRIPCION] VARCHAR (20) NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.estado_pedido', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTADO_PEDIDO';

