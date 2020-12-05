create proc Sp_Product_List_Of_Category(@categoryID int)
as
select * from Products where Status = 1 and CategoryID = @categoryID
order by Name

exec Sp_Product_List_Of_Category 2