

-- =============================================
-- Author: Rodrigo Carrión
-- Create date: 17/08/2018
-- Description: Genera usuarios viajantes (vendedores o zona)
-- =============================================
CREATE PROCEDURE [dbo].[prc_GenerarUsuariosViajantes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO UsuariosGlobal (Usuario, Nombre, Rol, Activo, FechaCreacionUtc, Password, ZonaViajante)
    SELECT CONCAT('zona',v.VEN_CODIGO), 
        v.VEN_NOMBRE,
        'VIAJANTE',
        1,
        GETUTCDATE(),
        'autocor123$',
        v.VEN_CODIGO 
    FROM    VENDEDOR v
    WHERE v.VEN_CODIGO NOT IN  (SELECT ug.ZonaViajante 
                           FROM UsuariosGlobal ug 
                           WHERE ug.Rol = 'VIAJANTE' 
                                AND ug.ZonaViajante IS NOT NULL);
END