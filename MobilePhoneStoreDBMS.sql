--Create Database
create database MobilePhoneStoreDBMS;
go
use MobilePhoneStoreDBMS;
--Create some initial tables
go
create table Products(
	ProductID int not null identity(1,1),
	Name nvarchar(50) not null,
	Quantity int not null,
	Description nvarchar(max),
	Status bit not null,
	Price int not null,
);
go
create table Producers(
	ProducerID int identity(1,1) not null,
	Name nvarchar(50) not null,
);
go
create table Categories(
	CategoryID int identity(1,1) not null,
	Name nvarchar(50) not null,
);
go
create table ProductSpecifications(
	SpecificationID int identity(1,1) not null,
	Name nvarchar(50) not null,
	Description nvarchar(max),
);
go
create table Customers(
	CustomerID int not null,
	Name nvarchar(50),
	PhoneNumber nvarchar(20),
	Email nvarchar(50),
);
go
create table Orders(
	OrderID int identity(1,1) not null,
	OrderTime datetime not null,
	status int,
);
go
create table Roles(
	RoleID int identity(1,1) not null,
	RoleName nvarchar(50) not null,
	Descriptions nvarchar(100),
);
go
create table Accounts(
	AccID int identity(1,1) not null,
	Username nvarchar(20),
	Password nvarchar(20),
);

--create primary key and foreign relationships between initial tables
go
alter table Products add ProducerID int not null;
alter table Products add CategoryID int not null;
alter table Orders add CustomerID int not null;
alter table Accounts add hasRole int not null;

go
alter table Producers add constraint ProducersPK primary key(ProducerID);
alter table Categories add constraint CategoriesPK primary key(CategoryID);
alter table Products add constraint ProductsPK primary key(ProductID);
alter table Customers add constraint CustomerPK primary key(CustomerID);
alter table ProductSpecifications add constraint ProductSpecificationsPK primary key(SpecificationID);
alter table Roles add constraint RolesPK primary key(RoleID);
alter table Accounts add constraint AccountsPK primary key(AccID);
alter table Orders add constraint OrderPK primary key(OrderID);
go
alter table Products add constraint ProductsProdcerIDFK foreign key(ProducerID) references Producers(ProducerID);
alter table Products add constraint ProductsCategoryIDFK foreign key(CategoryID) references Categories(CategoryID);
alter table Customers add constraint CustomersAccIDFK foreign key(CustomerID) references Accounts(AccID);
alter table Orders add constraint OrderCustomerIDFK foreign key(CustomerID) references Customers(CustomerID);
alter table Accounts add constraint AccountsHasRoleFK foreign key(hasRole) references Roles(RoleID);
--create weak entity tables
go
create table SpecificationValues(
	SpecificationID int not null,
	Value nvarchar(50) not null,

	constraint SpecificationValuesPK primary key (SpecificationID, Value),
	constraint SpecificationValuesSpecificationIDFK foreign key(SpecificationID) references ProductSpecifications(SpecificationID),
);
go
create table AvatarOfProduct(
	productID int not null,
	imageFile image not null,

	constraint AvatarOfProductPK primary key(productID),
	constraint AvatarOfProductProductIDFK foreign key(ProductID) references Products(ProductID),
);
--create many to many relationship tables
go
create table HasSpecification(
	ProductID int not null,
	SpecificationID int not null,
	Value nvarchar(50) not null,

	constraint HasSpecificationPK primary key(ProductID, SpecificationID, Value),
	constraint HasSpecificationProductIDFK foreign key(ProductID) references Products(ProductID),
	constraint HasSpecidicationSpecificationIDAndValueFK foreign key(SpecificationID, Value) references SpecificationValues(SpecificationID, Value),
);
go
create table Carts(
	ProductID int not null,
	CustomerID int not null,
	amount int,
	
	constraint CartsPK primary key(ProductID, CustomerID),
	constraint CartsProductIDFK foreign key(ProductID) references Products(ProductID),
	constraint CartsCustomerIDFK foreign key(CustomerID) references Customers(CustomerID),
);
go
create table ProductsOfOrder(
	OrderID int not null,
	ProductID int not null,
	amount int,

	constraint ProductOfOrderPK primary key(OrderID, ProductID),
	constraint ProductsOfOrderOrderIDFK foreign key(OrderID) references Orders(OrderID),
	constraint ProductsOfOrderProductIDFK foreign key(ProductID) references Products(ProductID),
)
go
--add constraints
	--Products table
	alter table Products add constraint products_quantity_default_value default 0 for Quantity;
	alter table Products add constraint products_quantity_greater_than_0 check(Quantity >= 0);
	alter table Products add constraint products_status_default_value default 0 for Status;
	alter table Products add constraint products_price_greater_than_0 check(Price >= 0);
	alter table Products add constraint products_name_unique unique(Name);
	
	--Producers table
	alter table Producers add constraint producers_name_unique unique(Name);
	
	--Categories table
	alter table Categories add constraint categories_name_unique unique(Name);

	--ProductSpecifications table
	alter table ProductSpecifications add constraint productSpecifications_name_unique unique(Name)

	--roles table
	alter table Roles add constraint roles_roleName_unique unique(RoleName);

	--Accounts table
	alter table Accounts add constraint accounts_Username_unique unique(Username);

	--Orders table
	alter table Orders add constraint orders_status_default_value default 0 for status;
	alter table Orders add constraint orders_customerID_orderTime_unique unique(CustomerID, OrderTime);
	alter table Orders add constraint orders_orderTime_default_value default CAST(getdate() AS datetime) for orderTime;

	--Carts table
	alter table Carts add constraint Carts_amount_default_value default 1 for amount;

	--ProductsOfOrder table
	alter table ProductsOfOrder add constraint ProductsOfOrder_amount_default_value default 1 for amount;

--add views
go
create view Specifications_of_all_product as
select p.Name as name, ps.Name as specification, s.Value as Value
from Products p inner join HasSpecification h on p.ProductID = h.ProductID
					inner join SpecificationValues s on h.SpecificationID = s.SpecificationID and h.Value = s.Value
					inner join ProductSpecifications ps on s.SpecificationID = ps.SpecificationID;
					
---
go
create view view_Category_List
as
select * from Categories
--- 
--add Functions
go
create function GetSpecifications(@productID int)
returns table as
return
	select p.Name as name, ps.Name as specification, s.Value as Value
	from Products p inner join HasSpecification h on p.ProductID = h.ProductID
					inner join SpecificationValues s on h.SpecificationID = s.SpecificationID and h.Value = s.Value
					inner join ProductSpecifications ps on s.SpecificationID = ps.SpecificationID
	where p.ProductID = @productID;

go
create function GetRoleId(@roleName nvarchar(50)) returns int as
begin
	declare @id int;

	select @id = r.RoleID
	from Roles r
	where r.RoleName = @roleName

	return @id;
end;
go 
create function SplitSpecificationValuesString(@string nvarchar(2000)) 
returns @table table(specificationID int, value nvarchar(50)) as
begin
	-- string input must be in format: "1,4GB|2,128GB|3,1 Sims..."
	if(len(@string) = 0)
	begin
		return
	end

	Declare @Cnt int;
	Set @Cnt = 1;

	While (Charindex('|',@string)>0)
	Begin
		declare @substringleft nvarchar(max);
		set @substringleft = Substring(@string,1,Charindex('|',@string)-1);
		
		declare @specificationID int;
		declare @value nvarchar(max);

		set @specificationID = cast(Substring(@substringleft,1,Charindex(',',@substringleft)-1) AS int);
		set @value = Substring(@substringleft,Charindex(',',@substringleft) + 1, len(@substringleft));

		Insert Into @table (specificationID, value) values (@specificationID, @value);

		Set @string = Substring(@string, Charindex('|',@string) + 1, len(@string));

		Set @Cnt = @Cnt + 1
	End

	set @specificationID = cast(Substring(@string,1,Charindex(',',@string)-1) AS int);
	set @value = Substring(@string,Charindex(',',@string) + 1, len(@string));

	Insert Into @table (specificationID, value) values (@specificationID, @value);

	Return
end;

go
create function GetTotalOrderCost(@orderID int) returns int as
begin
	declare @totalCost int;

	select @totalCost = sum(p.Price * po.amount)
	from Orders o inner join ProductsOfOrder po on o.OrderID = po.OrderID
				  inner join Products p on po.ProductID = p.ProductID
	where o.OrderID = @orderID;

	return @totalCost;
end
--add stored procedures
go
create procedure AddNewSpecificationToAProduct @ProductID int, @SpecificationID int, @Value nvarchar(max) as
begin
	Insert into HasSpecification Values (@ProductID, @SpecificationID, @Value);
end;
go
create procedure AddNewProduct @name nvarchar(50)
							   , @description nvarchar(max) = ''
							   , @quantity int
							   , @status bit
							   , @price int
							   , @producerID int
							   , @categoryID int
							   , @imageFile image
							   , @specificationValuesString nvarchar(2000)
							   , @isSuccess bit output as
begin
	set @isSuccess = 0;

	set nocount on;
    declare @trancount int;
    set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction AddNewProducts --identifier, limited 32 characters

        -- Do the actual work here

		insert into Products(Name, Description, Quantity, Status, Price, ProducerID, CategoryID) values(@name, @description, @quantity, @status, @price, @producerID, @categoryID);

		declare @productID int;

		select @productID = Products.ProductID
		from Products
		where Products.Name = @name

		insert into AvatarOfProduct(productID, imageFile) values(@productID, @imageFile);

		insert into HasSpecification(ProductID, SpecificationID, Value) select @productID, specificationID, value from dbo.SplitSpecificationValuesString(@specificationValuesString);

		lbexit:
        if @trancount = 0
            commit;

		set @isSuccess = 1;
    end try
    begin catch
        declare @error int, @message varchar(4000), @xstate int;
        select @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(), @xstate = XACT_STATE();
        if @xstate = -1
            rollback;
        if @xstate = 1 and @trancount = 0
            rollback
        if @xstate = 1 and @trancount > 0
            rollback transaction AddNewProducts

        --raiserror ('usp_my_procedure_name: %d: %s', 16, 1, @error, @message) ;
    end catch
end;
go 
create procedure DeleteProduct @productID int, @isSuccess bit output as
begin
	set @isSuccess = 0;

	set nocount on;
    declare @trancount int;
    set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction DeleteProduct;
        -- Do the actual work here

		--Check if the product and it's avatar is exist
		if (not exists (
						select *
						from Products
						where Products.ProductID = @productID
						) or not exists (
										select *
										from AvatarOfProduct
										where AvatarOfProduct.productID = @productID
										))
		begin
			rollback; -- avoid return statment in try..catch and begin trans...commit... rollback
			return; --return when @@trancount == 0, rollback statement decrements @@trancout to 0 (clear, ex: trancount 3: --> 0)
		end

		--Delete avatar
		delete from AvatarOfProduct where AvatarOfProduct.productID = @productID;

		--Delete all specification values of this product
		delete from HasSpecification where HasSpecification.ProductID = @productID;

		--Delete the product
		delete from Products where products.ProductID = @productID;
		
		lbexit: --lbexit is a lable to jump
        if @trancount = 0
            commit;
    
		set @isSuccess = 1;
	end try
    begin catch
        declare @error int, @message varchar(4000), @xstate int;
        select @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(), @xstate = XACT_STATE();
        if @xstate = -1 --xstate = -1 mean the transaction is uncommittable and should be rolled back.
            rollback;
        if @xstate = 1 and @trancount = 0 --xsate = 1, the transaction is committable.
            rollback;
        if @xstate = 1 and @trancount > 0
            rollback transaction DeleteProduct;

        --raiserror ('usp_my_procedure_name: %d: %s', 16, 1, @error, @message) ;
    end catch
end;

go
create procedure CancelAnOrder @orderID int, @stateCanceled int, @isSuccess bit output as
begin
	set @isSuccess = 0;

	set nocount on;
	begin transaction CancelAnOrder;

	if(not exists(select * from Orders where Orders.OrderID = @orderID))
	begin
		rollback transaction CancelAnOrder;
		return;
	end;

	if((select Orders.status from Orders where Orders.OrderID = @orderID) != 0)
	begin
		rollback transaction CancelAnOrder;
		return;
	end;

	--Update product quantity for all products in order
	declare @tableToLoop table(id int identity(0,1), productID int, amount int);

	insert into @tableToLoop(productID, amount) (
													select p.ProductID, p.amount
													from ProductsOfOrder p
													where p.OrderID = @orderID
												);

	declare @i int;
	declare @n int;

	set @i = 0;
	set @n = ( select count(id) from @tableToLoop); 

	while (@i < @n)
	begin
		declare @productID int;
		declare @amount int;

		select @productID = t.productID, @amount = t.amount 
		from @tableToLoop t 
		where t.id = @i; 

		update Products
		set Quantity = Quantity + @amount
		where ProductID = @productID

		set @i = @i + 1;
	end;

	--Set status = Canceled
	update Orders
	set status = @stateCanceled
	where OrderID = @orderID;

	commit;
	set @isSuccess = 1;
end;

go
create procedure DeleteAnOrder @orderID int, @stateCanceled int, @isSuccess bit output as
begin
	set @isSuccess = 0;

	begin transaction DeleteAnOrder;
		if(not exists(select * from Orders where OrderID = @orderID))
		begin
			rollback transaction DeleteAnOrder;
			return;
		end;

		if((select status from Orders where Orders.OrderID = @orderID) <> @stateCanceled)
		begin
			rollback transaction DeleteAnOrder;
			return;
		end;

		declare @productID int;
		select @productID = p.ProductID from ProductsOfOrder p where p.OrderID = @orderID;

		delete from ProductsOfOrder
		where OrderID = @orderID;

		delete from Orders
		where OrderID = @orderID;

		commit;
		set @isSuccess = 1;
end;

go
create procedure SPGetTotalOrderCost @orderID int as
begin
	select dbo.GetTotalOrderCost(@orderID);
end
---
go
create proc Sp_Catagory_Details(@id int)
as
select * from Categories where CategoryID = @id

go
create proc Sp_Producer_List
as
select * from Producers
order by Name

go
create proc Sp_Product_List
as
select * from Products where Status = 1
order by Name

go
create proc Sp_Product_List_Of_Category(@categoryID int)
as
select * from Products where Status = 1 and CategoryID = @categoryID
order by Name

go
create proc Sp_Product_List_Of_Producer(@producerID int)
as
select * from Products where Status = 1 and ProducerID = @producerID
order by Name

go
create proc Sp_Producer_Details(@id int)
as
select * from Producers where ProducerID = @id

go
create proc Sp_Product_Details(@id int)
as
select * from Products where Status = 1 and ProductID = @id
---
go
create proc sp_Account_Login
	@username nvarchar(20),
	@password nvarchar(20)
as
begin
	
	declare @count int
	declare @role int
	declare @res int

	select @count = count(*) from Accounts where Username = @username and Password = @password
	if @count>0
		begin
			select @role = hasRole from Accounts where Username = @username
			set @res = @role
		end
	else
		set @res = 0

	select @res
end;

go
create proc sp_Account_Register
	@Name nvarchar(50),
	@PhoneNumber nvarchar(20),
	@Email nvarchar(50),
	@username nvarchar(20),
	@password nvarchar(20)
as
begin
	declare @count int
	declare @res bit
	declare @acc int
	select @count = count(*) from Accounts where Username = @username
	if @count>0
		set @res = 0
	else
	begin
		insert into Accounts values(@username, @password, dbo.GetRoleId('customer'))
		select @acc = AccID from Accounts where Username = @username
		insert into Customers values(@acc, @name, @PhoneNumber, @Email)
		set @res = 1
	end
	select @res
end;
---
--add triggers
go 
create trigger Carts_After_Insert_CheckConstrainAmountOfProduct on Carts after insert as
begin 
	--Preventing adding invalid number of products to cart

	declare @productId int;
	declare @amount int;

	select @productId = inserted.ProductID, @amount = inserted.amount from inserted;

	if(@amount > (
					select Products.Quantity
					from Products
					where Products.ProductID = @productId
				 ))
	begin
		rollback;
	end
end

go
create trigger Carts_After_Update_CheckContraintAmountOfProduct on Carts after update as
begin
	--Preventing updating invalid number of products to cart

	declare @productID int;
	declare @amountIncreased int;
	declare @quantityOfProduct int;

	select @productID = inserted.ProductID
	from inserted;

	select @amountIncreased = inserted.amount - deleted.amount
	from inserted, deleted;

	select @quantityOfProduct = Products.Quantity 
	from Products 
	where Products.ProductID = @productID;
	
	if(( @quantityOfProduct - @amountIncreased) <= 0)
	begin
		rollback;
	end
end;

go
create trigger Orders_After_Insert_DeleteCart on Orders after insert as
begin
	--Trigger fires when inserting an order(customerID) values(value)

	--Preventing creating an invalid order(do not have any product in cart of specific customer)
	--After inserting an order --> Adding products to order, delete all products in cart

	--begin transaction; --no need, because All DML statements are executed within a transaction. 
						 --The DML within the trigger will use the transaction context of the statement 
						 --that fired the trigger so all modifications, inside the trigger and out, are a single atomic operation.
						 --Any change that a trigger does is committed with the transaction that fired the trigger.
	declare @orderID int;
	declare @customerID int;

	select @orderID = inserted.OrderID, @customerID = inserted.CustomerID 
	from inserted;

	if(not exists(select * from Carts where Carts.CustomerID = @customerID))
	begin
		rollback; --@@trancount here = 1, rollback the entire transaction.
		return;
	end

	--insert all products in carts to ProductsOfOrder
		--insert into ProductsOfOrder(OrderID, ProductID, amount) select @orderID, Carts.ProductID, Carts.amount from Carts where Carts.CustomerID = @customerID;
		--the statement above fire trigger on just 1 time in ProductsOfOrder tables

	declare @tableToLoop table(id int identity(0,1), productID int, amount int);
	insert into @tableToLoop(productID, amount) select Carts.ProductID, Carts.amount from Carts where Carts.CustomerID = @customerID;

	declare @i int;
	declare @n int;
	set @i = 0;
	set @n = ( select count(id) from @tableToLoop); 

	while (@i < @n)
	begin
		insert into ProductsOfOrder(OrderID, ProductID, amount) values (@orderID
																		, (
																			select t.productID
																			from @tableToLoop t
																			where t.id = @i
																		  )
																		, (
																			select t.amount
																			from @tableToLoop t
																			where t.id = @i
																		  )
																	   );
		
		set @i = @i + 1;
	end

	delete from Carts where Carts.CustomerID = @customerID;
	--commit; can't commit
end;

go
create trigger OrdersOfProduct_After_Insert_CheckConstraintAmountOfProduct on ProductsOfOrder after insert as
begin
	--Trigger fires when inserting a product to a specific order

	--Preventing inserting an invalid amount of products to an order
	--After inserting a product to a specific order --> Update quantity of this product

	declare @productID int;
	declare @amount int;
	declare @productQuantity int;
	
	select @productID = inserted.ProductID, @amount = inserted.amount
	from inserted;

	--print ' ' + convert(varchar(4), @productID) + ' ' + convert(varchar(4), @amount) + ' ';

	select @productQuantity = Products.Quantity
	from Products
	where Products.ProductID = @productID;

	if(@amount > @productQuantity)
	begin
		rollback;
		return;
	end

	update Products
	set Products.Quantity = Products.Quantity - @amount
	where Products.ProductID = @productID;
	
end;

go
create trigger Products_After_Insert_Update_DisableIfQuantityEqualZero on Products after insert, update  as
begin
	if(Update(Quantity))
	begin
		declare @productID int;

		select @productID = inserted.ProductID from inserted;

		if((select inserted.Quantity from inserted) = 0)
		begin
			update Products
			set Status = 0
			where ProductID = @productID;
		end
	end
end




go
--inserting some initial values
insert into Categories(Name) values ('Tablet');
insert into Categories(Name) values ('Android');
insert into Categories(Name) values ('IOS');
insert into Categories(Name) values ('Window Phone');

insert into Producers(Name) values ('SamSung');
insert into Producers(Name) values ('Sony');
insert into Producers(Name) values ('Apple');
insert into Producers(Name) values ('Xiaomi');
insert into Producers(Name) values ('LG');
insert into Producers(Name) values ('Motorola');
insert into Producers(Name) values ('Oppo');
insert into Producers(Name) values ('Lenovo');
insert into Producers(Name) values ('Nokia');

insert into ProductSpecifications(Name, Description) values ('Ram','Ramdom access memory');
insert into ProductSpecifications(Name, Description) values ('Rom','Read-only memory');
insert into ProductSpecifications(Name, Description) values ('Number of SIMs','no decription');
insert into ProductSpecifications(Name, Description) values ('OS','Operation System');

insert into SpecificationValues(SpecificationID, Value) values (1,'4GB');
insert into SpecificationValues(SpecificationID, Value) values (1,'8GB');
insert into SpecificationValues(SpecificationID, Value) values (1,'16GB');
insert into SpecificationValues(SpecificationID, Value) values (2,'32GB');
insert into SpecificationValues(SpecificationID, Value) values (2,'128GB');
insert into SpecificationValues(SpecificationID, Value) values (3,'2 Sims');
insert into SpecificationValues(SpecificationID, Value) values (3,'1 Sims');
insert into SpecificationValues(SpecificationID, Value) values (4,'Android');
insert into SpecificationValues(SpecificationID, Value) values (4,'IOS');

insert into Roles(RoleName, Descriptions) values('admin', 'chu cua hang');
insert into Roles(RoleName, Descriptions) values('seller', 'nguoi ban');
insert into Roles(RoleName, Descriptions) values('customer', 'nguoi dung');

insert into Accounts values('admin','gFuYE2Bpl7A=', dbo.GetRoleId('admin'));
insert into Accounts values('seller','gFuYE2Bpl7A=', dbo.GetRoleId('seller'));

exec sp_Account_Register 'Vinh', '0789612482', 'abc@gmail.com', 'customer', 'gFuYE2Bpl7A=';
exec sp_Account_Register 'Vinh', '0789612482', 'abc@gmail.com', 'customer1', 'gFuYE2Bpl7A=';
