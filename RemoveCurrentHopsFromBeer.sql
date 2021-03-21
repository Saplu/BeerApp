
CREATE PROCEDURE [dbo].[RemoveCurrentHopsFromBeer] 
	-- Add the parameters for the stored procedure here
	@in_beer_name varchar(100)
AS
BEGIN
	declare @v_beer_id int;

	SET NOCOUNT ON;

	select @v_beer_id = isnull((select id from Beer where title = @in_beer_name), -1) 

    delete from Beer_has_hop where beer_id = @v_beer_id;

END




