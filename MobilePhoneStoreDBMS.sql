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
	Image nvarchar(max),
);
go
create table Producers(
	ProducerID int identity(1,1) not null,
	Name nvarchar(50) not null unique,
);
go
create table Categories(
	CategoryID int identity(1,1) not null,
	Name nvarchar(50) not null unique,
);
go
create table ProductSpecifications(
	SpecificationID int identity(1,1) not null,
	Name nvarchar(50) not null,
	Description nvarchar(max),
);
go
create table Customers(
	CustomerID int identity(1,1) not null,
	Name nvarchar(50),
	PhoneNumber nvarchar(20),
	Email nvarchar(50),
);
go
create table AvatarOfProduct(
	productID int not null,
	imageFile image not null,
);
go
create table Roles(
RoleID int identity primary key,
RoleName nvarchar(50) not null,
Descriptions nvarchar(100)
);
go
create table Accounts(
AccID int identity primary key,
Username nvarchar(20),
Password nvarchar(20),
hasRole int references Roles(RoleID)
);

--create primary key and foreign relationships between initial tables
go
alter table Products add ProducerID int not null;
alter table Products add CategoryID int not null;
alter table Customers add hasAcc int;
go
alter table Producers add constraint ProducersPK primary key(ProducerID);
alter table Categories add constraint CategoriesPK primary key(CategoryID);
alter table Products add constraint ProductsPK primary key(ProductID);
alter table Customers add constraint CustomerPK primary key(CustomerID);
alter table ProductSpecifications add constraint ProductSpecificationsPK primary key(SpecificationID);
alter table AvatarOfProduct add constraint AvatarOfProductPK primary key(ProductID);
go
alter table Products add constraint ProductsProdcerIDFK foreign key(ProducerID) references Producers(ProducerID);
alter table Products add constraint ProductsCategoryIDFK foreign key(CategoryID) references Categories(CategoryID);
alter table AvatarOfProduct add constraint AvatarOfProductProductIDFK foreign key(ProductID) references Products(ProductID);
alter table Customers add constraint CustomersAccIDFK foreign key(hasAcc) references Accounts(AccID);

--create weak entity tables
go
create table Comments(
	CustomerID int not null,
	ProductID int not null,
	CommentTime datetime not null,
	Content nvarchar(max),

	constraint CommentsPK primary key(CustomerID, ProductID, CommentTime),
	constraint CommentsCustomerIDFk foreign key (CustomerID) references Customers (CustomerID),
	constraint CommentsProductIDFk foreign key (ProductID) references Products (ProductID), 
);
go
create table Orders(
	CustomerID int not null,
	ProductID int not null,
	OrderTime datetime not null,
	amount int,

	constraint OrdersPK primary key (ProductID, CustomerID, OrderTime),
	constraint OrdersCustomerIDFK foreign key(CustomerID) references Customers(CustomerID),
	constraint OrdersProductIDFK foreign key(ProductID) references Products(ProductID),
);
go
create table SpecificationValues(
	SpecificationID int not null,
	Value nvarchar(50) not null,

	constraint SpecificationValuesPK primary key (SpecificationID, Value),
	constraint SpecificationValuesSpecificationIDFK foreign key(SpecificationID) references ProductSpecifications(SpecificationID),
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
	amount int
	
	constraint CartsPK primary key(ProductID, CustomerID),
	constraint CartsProductIDFK foreign key(ProductID) references Products(ProductID),
	constraint CartsCustomerIDFK foreign key(CustomerID) references Customers(CustomerID),
);
go
create table Rates(
	ProductID int not null,
	CustomerID int not null,
	starts int,
	constraint RatesPK primary key(ProductID, CustomerID),
	constraint RatesCustomerIDFK foreign key(CustomerID) references Customers(CustomerID),
	constraint RatesProductIDFK foreign key(ProductID) references Products(ProductID),
);
go
--add constraints
	--Products table
	alter table Products add constraint products_quantity_default_value default 0 for Quantity;
	alter table Products add constraint products_quantity_greater_than_0 check(Quantity >= 0);
	alter table Products add constraint products_status_default_value default 0 for Status;
	alter table Products add constraint products_price_greater_than_0 check(Price >= 0);

	--
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
--add triggers
go
create trigger Orders_After_Insert_DeleteCart on Carts after delete as
begin
	if(exists (select * from deleted))
	insert into Orders(CustomerID, ProductID, OrderTime, amount) values ((select CustomerID from deleted), 
																		 (select ProductID from deleted),
																		 (select GETDATE()),
																		 (select amount from deleted));
end;
--add stored procedures
go
create procedure AddNewSpecificationToAProduct @ProductID int, @SpecificationID int, @Value nvarchar(max) as
begin
	Insert into HasSpecification Values (@ProductID, @SpecificationID, @Value);
end;

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
		insert into Accounts values(@username, @password, 2)
		select @acc = AccID from Accounts where Username = @username
		insert into Customers values(@name, @PhoneNumber, @Email, @acc)
		set @res = 1
	end
	select @res
end;
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

insert into Products(Name,Price,Description,ProducerID,CategoryID) values ('j3 Pro',4,'ProVip',1,2);
insert into Products(Name,Price,Description,ProducerID,CategoryID) values ('j7 Pro',7,'Max Pro',2,1);
insert into Products(Name,Price,Description,ProducerID,CategoryID) values ('iphone 7',10,'IOSPro',3,3);

insert into HasSpecification(ProductID, SpecificationID, Value) values(1,1,'4GB');
insert into HasSpecification(ProductID, SpecificationID, Value) values(1,2,'32GB');
insert into HasSpecification(ProductID, SpecificationID, Value) values(1,3,'2 Sims');
insert into HasSpecification(ProductID, SpecificationID, Value) values(1,4,'IOS');

insert into Customers(Name, PhoneNumber) values('Vinh','01234');
insert into Customers(Name, PhoneNumber) values('Nhan','02234');
insert into Customers(Name, PhoneNumber) values('Hung','01234');
insert into Customers(Name, PhoneNumber) values('Nghia','01234');

insert into Carts(CustomerID, ProductID, amount) values(1,1,5);
insert into Carts(CustomerID, ProductID, amount) values(2,1,1);

insert into Roles(RoleName, Descriptions) values('admin', 'chu cua hang');
insert into Roles(RoleName, Descriptions) values('user', 'nguoi dung');

insert into Accounts values('admin','gFuYE2Bpl7A=',1);




