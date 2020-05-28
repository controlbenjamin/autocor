CREATE TABLE [dbo].[USUARIOS] (
    [USR_ID]   INT           DEFAULT ((0)) NOT NULL,
    [USR_NAME] NVARCHAR (50) DEFAULT (NULL) NULL,
    [USR_PASS] NVARCHAR (50) DEFAULT (NULL) NULL,
    CONSTRAINT [PK_usuarios_USR_ID] PRIMARY KEY CLUSTERED ([USR_ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [USR_ID]
    ON [dbo].[USUARIOS]([USR_ID] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.usuarios', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USUARIOS';

