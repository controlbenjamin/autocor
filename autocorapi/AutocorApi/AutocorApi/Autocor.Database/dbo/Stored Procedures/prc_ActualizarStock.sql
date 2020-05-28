-- =============================================
-- Author: Carrión, Rodrigo
-- Create date: 27/02/2018
-- Description: Copia la cantidad de stock desde STOCK_CANTIDAD a STOCK
-- =============================================
CREATE PROCEDURE [dbo].[prc_ActualizarStock]
	@CantidadRegistros int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CodigoPieza nvarchar(7);
	DECLARE @Stock int;

	SET @CantidadRegistros = 0;

    -- Insert statements for procedure here
     DECLARE stock_cursor CURSOR
	   FOR SELECT CODPIEZA, STOCK FROM STOCK_CANTIDAD
    OPEN stock_cursor
    FETCH NEXT FROM stock_cursor
    INTO @CodigoPieza, @Stock;
    WHILE @@FETCH_STATUS = 0
    BEGIN
	   
	   UPDATE STOCK 
	   SET STOCK.STOCK_ACTUAL = @Stock, STOCK.STOCK_ACTUALIZACION = GETUTCDATE()	
	   WHERE STOCK.CODPIEZA = @CodigoPieza

	   SET @CantidadRegistros = @CantidadRegistros + @@ROWCOUNT;
	   
	   -- siguiente
	   FETCH NEXT FROM stock_cursor
	   INTO @CodigoPieza, @Stock;
    END
    CLOSE stock_cursor;
    DEALLOCATE stock_cursor;

END
GO
--GRANT EXECUTE
--    ON OBJECT::[dbo].[prc_ActualizarStock] TO [catalogo]
--    AS [dbo];

