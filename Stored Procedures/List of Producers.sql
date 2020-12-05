create proc Sp_Producer_List
as
select * from Producers
order by Name

exec Sp_Producer_List