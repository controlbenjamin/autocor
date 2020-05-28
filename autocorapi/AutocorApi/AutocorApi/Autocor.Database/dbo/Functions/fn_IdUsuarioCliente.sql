-- =============================================
-- Author:		Carrión, Rodrigo
-- Create date: 27/03/2018
-- Description:	Obtiene el id de usuario de un cliente
-- =============================================
CREATE FUNCTION [dbo].[fn_IdUsuarioCliente]
(
     @CodigoCliente int
)
RETURNS int
AS
BEGIN

    DECLARE @IdUsuario INT;

    SELECT @IdUsuario = ug.IdUsuario
    FROM dbo.UsuariosGlobal ug
    WHERE ug.Usuario = CAST(@CodigoCliente AS nvarchar(20))

    RETURN @IdUsuario;

END