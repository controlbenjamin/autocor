﻿CREATE TABLE [dbo].[STOCK] (
    [CODPIEZA]             NVARCHAR (7)    NOT NULL,
    [MAR_CODIGO]           NVARCHAR (1)    CONSTRAINT [DF__STOCK__MAR_CODIG__01142BA1] DEFAULT (NULL) NULL,
    [TIP_CODIGO]           INT             CONSTRAINT [DF__STOCK__TIP_CODIG__02084FDA] DEFAULT (NULL) NULL,
    [RUB_CODIGO]           INT             CONSTRAINT [DF__STOCK__RUB_CODIG__02FC7413] DEFAULT (NULL) NULL,
    [NROORIGINAL]          NVARCHAR (200)  CONSTRAINT [DF__STOCK__NROORIGIN__03F0984C] DEFAULT (NULL) NULL,
    [DESCRIP]              NVARCHAR (200)  CONSTRAINT [DF__STOCK__DESCRIP__04E4BC85] DEFAULT (NULL) NULL,
    [APLICACION]           NVARCHAR (80)   CONSTRAINT [DF__STOCK__APLICACIO__05D8E0BE] DEFAULT (NULL) NULL,
    [PRECIO]               DECIMAL (19, 4) CONSTRAINT [DF__STOCK__PRECIO__06CD04F7] DEFAULT (NULL) NULL,
    [NACIONAL]             NVARCHAR (1)    CONSTRAINT [DF__STOCK__NACIONAL__07C12930] DEFAULT (NULL) NULL,
    [CATALOGO]             NVARCHAR (1)    CONSTRAINT [DF__STOCK__CATALOGO__08B54D69] DEFAULT (NULL) NULL,
    [IMPRIMIR]             NVARCHAR (1)    CONSTRAINT [DF__STOCK__IMPRIMIR__09A971A2] DEFAULT (NULL) NULL,
    [FOTO]                 NVARCHAR (100)  CONSTRAINT [DF__STOCK__FOTO__0A9D95DB] DEFAULT (NULL) NULL,
    [SECUENCIA]            INT             CONSTRAINT [DF__STOCK__SECUENCIA__0B91BA14] DEFAULT (NULL) NULL,
    [CODCOMP]              INT             CONSTRAINT [DF__STOCK__CODCOMP__0C85DE4D] DEFAULT (NULL) NULL,
    [ORIGEN]               NVARCHAR (50)   CONSTRAINT [DF__STOCK__ORIGEN__0D7A0286] DEFAULT (NULL) NULL,
    [FEC_ALTA]             DATETIME2 (0)   CONSTRAINT [DF__STOCK__FEC_ALTA__0E6E26BF] DEFAULT (NULL) NULL,
    [PRECIO_DOLAR]         DECIMAL (19, 4) CONSTRAINT [DF__STOCK__PRECIO_DO__0F624AF8] DEFAULT (NULL) NULL,
    [COTIZACION]           DECIMAL (19, 4) CONSTRAINT [DF__STOCK__COTIZACIO__10566F31] DEFAULT (NULL) NULL,
    [CODPIEZA_MADRE]       NVARCHAR (7)    CONSTRAINT [DF__STOCK__CODPIEZA___114A936A] DEFAULT (NULL) NULL,
    [FEC_ULT_MOVIMI]       DATETIME2 (0)   CONSTRAINT [DF__STOCK__FEC_ULT_M__123EB7A3] DEFAULT (NULL) NULL,
    [ARTICULO]             NVARCHAR (12)   CONSTRAINT [DF__STOCK__ARTICULO__1332DBDC] DEFAULT (NULL) NULL,
    [CANTIDAD]             INT             CONSTRAINT [DF__STOCK__CANTIDAD__14270015] DEFAULT (NULL) NULL,
    [FECBAJA]              DATETIME2 (0)   CONSTRAINT [DF__STOCK__FECBAJA__151B244E] DEFAULT (NULL) NULL,
    [BAJASTOCK]            NVARCHAR (1)    CONSTRAINT [DF__STOCK__BAJASTOCK__160F4887] DEFAULT (NULL) NULL,
    [OBSERVACION]          NVARCHAR (254)  CONSTRAINT [DF__STOCK__OBSERVACI__17036CC0] DEFAULT (NULL) NULL,
    [OFERTA_TIPO]          NVARCHAR (1)    CONSTRAINT [DF__STOCK__OFERTA_TI__17F790F9] DEFAULT (NULL) NULL,
    [OFERTA_IMPORTE]       DECIMAL (19, 4) CONSTRAINT [DF__STOCK__OFERTA_IM__18EBB532] DEFAULT (NULL) NULL,
    [OFERTA_VALIDEZ_HASTA] DATETIME2 (0)   CONSTRAINT [DF__STOCK__OFERTA_VA__19DFD96B] DEFAULT (NULL) NULL,
    [OFERTA_SIGNO]         NVARCHAR (1)    CONSTRAINT [DF__STOCK__OFERTA_SI__1AD3FDA4] DEFAULT (NULL) NULL,
    [OFERTA_FECHA_CARGA]   DATETIME2 (0)   CONSTRAINT [DF__STOCK__OFERTA_FE__1BC821DD] DEFAULT (NULL) NULL,
    [PARA1]                NVARCHAR (10)   CONSTRAINT [DF__STOCK__PARA1__1CBC4616] DEFAULT (NULL) NULL,
    [PARA2]                NVARCHAR (10)   CONSTRAINT [DF__STOCK__PARA2__1DB06A4F] DEFAULT (NULL) NULL,
    [PARA3]                NVARCHAR (10)   CONSTRAINT [DF__STOCK__PARA3__1EA48E88] DEFAULT (NULL) NULL,
    [PARA4]                NVARCHAR (10)   CONSTRAINT [DF__STOCK__PARA4__1F98B2C1] DEFAULT (NULL) NULL,
    [PARA5]                NVARCHAR (10)   CONSTRAINT [DF__STOCK__PARA5__208CD6FA] DEFAULT (NULL) NULL,
    [PARA6]                NVARCHAR (10)   CONSTRAINT [DF__STOCK__PARA6__2180FB33] DEFAULT (NULL) NULL,
    [PARA7]                NVARCHAR (10)   CONSTRAINT [DF__STOCK__PARA7__22751F6C] DEFAULT (NULL) NULL,
    [PARA8]                NVARCHAR (10)   CONSTRAINT [DF__STOCK__PARA8__236943A5] DEFAULT (NULL) NULL,
    [PARA9]                NVARCHAR (10)   CONSTRAINT [DF__STOCK__PARA9__245D67DE] DEFAULT (NULL) NULL,
    [PARA10]               NVARCHAR (10)   CONSTRAINT [DF__STOCK__PARA10__25518C17] DEFAULT (NULL) NULL,
    [PRECIO_FUTURO]        DECIMAL (19, 4) CONSTRAINT [DF__STOCK__PRECIO_FU__2645B050] DEFAULT (NULL) NULL,
    [VARIACION]            NVARCHAR (1)    CONSTRAINT [DF__STOCK__VARIACION__2739D489] DEFAULT (NULL) NULL,
    [ESPECIAL]             NVARCHAR (1)    CONSTRAINT [DF__STOCK__ESPECIAL__282DF8C2] DEFAULT (NULL) NULL,
    [INCRE]                NVARCHAR (1)    CONSTRAINT [DF__STOCK__INCRE__29221CFB] DEFAULT (NULL) NULL,
    [PORCE]                DECIMAL (18)    CONSTRAINT [DF__STOCK__PORCE__2A164134] DEFAULT (NULL) NULL,
    [NOVEDAD]              NVARCHAR (1)    CONSTRAINT [DF__STOCK__NOVEDAD__2B0A656D] DEFAULT (NULL) NULL,
    [ARTICULON]            DECIMAL (18)    CONSTRAINT [DF__STOCK__ARTICULON__2BFE89A6] DEFAULT (NULL) NULL,
    [UVTA]                 INT             NULL,
    [STOCK_ACTUAL]         INT             NULL,
    [STOCK_ACTUALIZACION]  DATETIME        NULL
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_STOCK]
    ON [dbo].[STOCK]([CODPIEZA] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N'autocor.stock', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STOCK';

