
CREATE PROCEDURE [dbo].[upsertHop]
	
	@in_title varchar(100),
	@in_alpha float
AS
BEGIN

	DECLARE @v_id INT;

	SET NOCOUNT ON;
	SELECT @v_id = isnull ((select id from Hop where title = @in_title), -1)
	if @v_id = -1 
	begin
		insert into Hop values (@in_title, @in_alpha);
	end
	else begin
		update Hop set alpha_percentage = @in_alpha where id = @v_id;
	end

END


