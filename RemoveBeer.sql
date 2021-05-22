
CREATE PROCEDURE [dbo].[RemoveBeer] 
	
	@in_beer_name varchar(100)
AS
BEGIN
	declare @v_beer_id int;
	declare @v_result int;

	SET NOCOUNT ON;

	select @v_beer_id = isnull((select id from Beer where title = @in_beer_name), -1)

	if @v_beer_id = -1
		set @v_result = 1;

	else begin
	delete from Beer_has_hop where beer_id = @v_beer_id;
	delete from Beer where id = @v_beer_id;

	set @v_result = 0;
	end

	select @v_result as result
END