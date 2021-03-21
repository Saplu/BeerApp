
CREATE PROCEDURE [dbo].[upsertBeerHasHop]

	@in_hop_name varchar(100),
	@in_beer_name varchar(100),
	@in_hop_weight varchar(100),
	@in_hop_boiling_time varchar(100)
AS
BEGIN
	declare @v_id int,
	@v_beer_id int,
	@v_hop_id int;
	
	SET NOCOUNT ON;
	select @v_beer_id = isnull((select id from Beer where title = @in_beer_name), -1);
	select @v_hop_id = isnull((select id from Hop where title = @in_hop_name), -1);
	select @v_id = isnull((select id from Beer_has_hop where 
		beer_id = @v_beer_id and
		hop_id = @v_hop_id), -1)
	if @v_id = -1 begin
		insert into Beer_has_hop values (
			@v_beer_id,
			@v_hop_id,
			@in_hop_weight,
			@in_hop_boiling_time)
	end
	else begin
		update Beer_has_hop set
			weight_g = @in_hop_weight,
			boiling_time_min = @in_hop_boiling_time
		where id = @v_id
	end
    
END


