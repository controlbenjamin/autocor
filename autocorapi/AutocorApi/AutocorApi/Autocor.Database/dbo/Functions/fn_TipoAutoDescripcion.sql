
-- =============================================
-- Author: Rodrigo Carrión
-- Create date: 13/06/2018
-- Description: Obtiene la descripción de un tipo de auto
-- =============================================
CREATE FUNCTION [dbo].[fn_TipoAutoDescripcion]
(
	@IdTipoAuto int
)
RETURNS NVARCHAR(254)
AS
BEGIN
	DECLARE @descripcion nvarchar( 54);

	SELECT @descripcion = RTRIM(ta.TIP_DESCRIPCION)
     FROM TIPO_AUTO ta
     WHERE ta.TIP_CODIGO = @IdTipoAuto;

	RETURN @descripcion;

END