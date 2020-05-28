CREATE TABLE [dbo].[PEDIDOS_WEB] (
    [CODPIEZA] NVARCHAR (50) DEFAULT (N'') NOT NULL,
    [CODCLI]   NVARCHAR (50) DEFAULT (N'') NOT NULL,
    [CANT]     INT           DEFAULT ((0)) NULL,
    CONSTRAINT [PK_pedidos_web_CODPIEZA] PRIMARY KEY CLUSTERED ([CODPIEZA] ASC, [CODCLI] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.pedidos_web', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PEDIDOS_WEB';

