CREATE TABLE [dbo].[PEDIDOS] (
    [ID_PEDIDO]        INT             IDENTITY (1288, 1) NOT NULL,
    [COD_CLIENTE]      FLOAT (53)      NOT NULL,
    [FECHA]            DATETIME2 (0)   NOT NULL,
    [ID_ESTADO]        INT             CONSTRAINT [DF__PEDIDOS__ID_ESTA__47DBAE45] DEFAULT ((0)) NOT NULL,
    [FECHA_ENVIO]      DATETIME2 (0)   CONSTRAINT [DF__PEDIDOS__FECHA_E__48CFD27E] DEFAULT (NULL) NULL,
    [PRECIO_TOTAL]     DECIMAL (19, 4) NOT NULL,
    [OBSERVACIONES]    NVARCHAR (500)  NOT NULL,
    [NRO_PEDI_SISTEMA] INT             NULL,
    [IdUsuario]        INT             NULL,
    CONSTRAINT [PK_pedidos_ID_PEDIDO] PRIMARY KEY CLUSTERED ([ID_PEDIDO] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.pedidos', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PEDIDOS';

