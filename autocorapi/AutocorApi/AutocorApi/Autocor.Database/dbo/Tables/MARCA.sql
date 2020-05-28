CREATE TABLE [dbo].[MARCA] (
    [MAR_CODIGO]      NVARCHAR (1)   NOT NULL,
    [MAR_DESCRIPCION] NVARCHAR (254) DEFAULT (NULL) NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.marca', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MARCA';

