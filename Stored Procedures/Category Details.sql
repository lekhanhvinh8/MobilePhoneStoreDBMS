create proc Sp_Catagory_Details(@id int)
as
select * from Categories where CategoryID = @id

exec Sp_Catagory_Details 3