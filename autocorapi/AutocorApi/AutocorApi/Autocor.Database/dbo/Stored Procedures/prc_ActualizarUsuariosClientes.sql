-- =============================================
-- Author:		Carrión, Rodrigo
-- Create date: 29/01/2018
-- Description:	Migra clientes a usuarios que no hayan sido migrados
-- =============================================
CREATE PROCEDURE [dbo].[prc_ActualizarUsuariosClientes]
	@desactivarClientes bit = 0
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @rows INT = 0;

	INSERT INTO UsuariosGlobal (Usuario, Nombre, Rol, Activo, FechaCreacionUtc, Password, CodigoCliente)
	SELECT
		CAST(CODCLI AS NVARCHAR(20)) AS Usuario,
		NOMBRE AS Nombre,
		'CLIENTE' AS Rol,
		1 as Activo,
		GETUTCDATE() as FechaCreacionUtc,
		CUIT AS Password,
          CODCLI AS CodigoCliente
	FROM CLIENTES
	WHERE CODCLI NOT IN
		(SELECT usu.CodigoCliente FROM UsuariosGlobal usu WHERE Rol = 'CLIENTE')

	-- retorna la cantidad de usuarios insertados
	SELECT @rows = @@ROWCOUNT;

	IF @desactivarClientes = 1
		UPDATE UsuariosGlobal 
		SET Activo = 0
		WHERE Rol = 'CLIENTE' 
			AND CodigoCliente NOT IN
				(SELECT CODCLI FROM CLIENTES)

	RETURN @rows;
END