create proc Sp_Product_List
as
select * from Products where Status = 1
order by Name

exec Sp_Product_List