CREATE TABLE [dbo].[INCORPORACIONES] (
    [INC_ID]          INT           IDENTITY (20, 1) NOT NULL,
    [INC_FECHA]       DATE          NOT NULL,
    [INC_DESCRIPCION] VARCHAR (MAX) NOT NULL,
    [INC_IMG]         VARCHAR (255) DEFAULT (NULL) NULL,
    CONSTRAINT [PK_incorporaciones_INC_ID] PRIMARY KEY CLUSTERED ([INC_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.incorporaciones', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INCORPORACIONES';

