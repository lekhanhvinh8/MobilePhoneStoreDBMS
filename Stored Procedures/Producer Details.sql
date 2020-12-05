create proc Sp_Producer_Details(@id int)
as
select * from Producers where ProducerID = @id

exec Sp_Producer_Details 3