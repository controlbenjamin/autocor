CREATE TABLE [dbo].[RECOMENDACIONES] (
    [REC_ID]          INT            DEFAULT ((0)) NOT NULL,
    [REC_CREACION]    DATETIME2 (0)  DEFAULT (NULL) NULL,
    [REC_TITULO]      NVARCHAR (50)  DEFAULT (NULL) NULL,
    [REC_FIN]         DATETIME2 (0)  DEFAULT (NULL) NULL,
    [REC_DESCRIPCION] NVARCHAR (MAX) NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.recomendaciones', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RECOMENDACIONES';

