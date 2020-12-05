create proc Sp_Product_List_Of_Producer(@producerID int)
as
select * from Products where Status = 1 and ProducerID = @producerID
order by Name

exec Sp_Product_List_Of_Producer 3