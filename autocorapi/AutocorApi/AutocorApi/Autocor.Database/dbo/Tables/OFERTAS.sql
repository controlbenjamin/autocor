CREATE TABLE [dbo].[OFERTAS] (
    [OF_ID]          INT            IDENTITY (19, 1) NOT NULL,
    [OF_FECHA]       DATE           NOT NULL,
    [OF_DESCRIPCION] NVARCHAR (MAX) NOT NULL,
    [OF_IMG]         NVARCHAR (255) DEFAULT (NULL) NULL,
    CONSTRAINT [PK_ofertas_OF_ID] PRIMARY KEY CLUSTERED ([OF_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.ofertas', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFERTAS';

