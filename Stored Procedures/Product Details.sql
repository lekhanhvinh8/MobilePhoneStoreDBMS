create proc Sp_Product_Details(@id int)
as
select * from Products where Status = 1 and ProductID = @id

exec Sp_Product_Details 6