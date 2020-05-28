CREATE TABLE [dbo].[VERSION] (
    [ID]        INT  IDENTITY (26, 1) NOT NULL,
    [FECHA]     DATE DEFAULT (NULL) NULL,
    [REGISTROS] INT  DEFAULT ((0)) NULL
);


GO
CREATE NONCLUSTERED INDEX [ID]
    ON [dbo].[VERSION]([ID] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.`version`', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VERSION';

