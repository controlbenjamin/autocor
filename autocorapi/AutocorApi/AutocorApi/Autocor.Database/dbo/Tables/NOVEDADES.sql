CREATE TABLE [dbo].[NOVEDADES] (
    [NOV_ID]          INT            IDENTITY (21, 1) NOT NULL,
    [NOV_DESCRIPCION] NVARCHAR (MAX) NULL,
    [NOV_CREACION]    DATE           DEFAULT (NULL) NULL,
    [NOV_FIN]         DATE           DEFAULT (NULL) NULL,
    CONSTRAINT [PK_novedades_NOV_ID] PRIMARY KEY CLUSTERED ([NOV_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.novedades', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOVEDADES';

