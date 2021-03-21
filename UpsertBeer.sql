
CREATE PROCEDURE [dbo].[upsertBeer]

	@in_title varchar(100),
	@in_amount int,
	@in_ibu int,
	@in_alcohol_percentage float,
	@in_density_start float,
	@in_density_end float,
	@in_malt_extract_used_kg float
AS
BEGIN
	declare @v_id int;

	SET NOCOUNT ON;
	select @v_id = isnull((select id from Beer where title = @in_title), -1)
	if @v_id = -1 begin
		insert into Beer values (
			@in_title,
			@in_amount,
			@in_ibu,
			@in_alcohol_percentage,
			getdate(),
			NULL,
			@in_density_start,
			@in_density_end,
			@in_malt_extract_used_kg)
	end
    else begin
		update Beer set 
			amount_l = @in_amount,
			ibu = @in_ibu,
			alcohol_percentage = @in_alcohol_percentage,
			modified_date = getdate(),
			density_start = @in_density_start,
			density_end = @in_density_end,
			malt_extract_used_kg = @in_malt_extract_used_kg
		where id = @v_id;
	end
END


