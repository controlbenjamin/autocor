CREATE TABLE [dbo].[DESCARGAS] (
    [ID]          INT            IDENTITY (34, 1) NOT NULL,
    [DESCRIPCION] NVARCHAR (100) DEFAULT (NULL) NULL,
    [ENLACE]      NVARCHAR (400) DEFAULT (NULL) NULL
);


GO
CREATE NONCLUSTERED INDEX [ID]
    ON [dbo].[DESCARGAS]([ID] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.descargas', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DESCARGAS';

