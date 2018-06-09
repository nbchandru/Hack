create user MagniFoodSchema IDENTIFIED BY MagniFoodSchema;

grant all privileges to MagniFoodSchema;


Create table MagniFoodSchema.Person(
    PersonID varchar(100) NOT NULL PRIMARY KEY,
    PersonName varchar(1000) NOT NULL,
    PersonEmailID varchar(1000) NOT NULL unique,
    PersonPassword varchar(1000) NOT NULL,
    PersonMobContactNumber varchar(100) NOT NULL
);

insert into MagniFoodSchema.Person values('person01','chandrasekar','bchandrasekar1@magnitude.com','+919008151740');
insert into MagniFoodSchema.Person values('person02','chandrasekar2','bchandrasekar2@magnitude.com','+919008151742');
insert into MagniFoodSchema.Person values('person03','chandrasekar3','bchandrasekar3@magnitude.com','+919008151743');
insert into MagniFoodSchema.Person values('person04','chandrasekar4','bchandrasekar4@magnitude.com','+919008151744');

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.Person;
-- get 1 person based on ID
select * from  MagniFoodSchema.Person where PersonID Like'%person04%';
-- get 1 person based on E-mail
select * from  MagniFoodSchema.Person where PersonEmailID Like'%bchandrasekar4%';


Create table MagniFoodSchema.Customer 
(    CustomerID varchar(100) NOT NULL PRIMARY KEY,
    CustomerType varchar(1000) NOT NULL,
    PersonID varchar(100),
  constraint fk_Person_PersonID foreign key (PersonID) 
      references Person (PersonID)  
);

insert into MagniFoodSchema.Customer values ('customer01','noraml','person01');
insert into MagniFoodSchema.Customer values ('customer02','Premium','person02');
insert into MagniFoodSchema.Customer values ('customer03','noraml','person02');
insert into MagniFoodSchema.Customer values ('customer04','noraml','person03');

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.Customer;
-- get 1 person based on ID
select * from  MagniFoodSchema.Customer where CustomerID Like'%customer01%';

-- endpoint queries:
-- get full customer info
select * from MagniFoodSchema.Customer c ,MagniFoodSchema.Person p where c.PersonID= p.PersonID;

Create table MagniFoodSchema.CustomerOrder
(    OrderID varchar(100) NOT NULL PRIMARY KEY,
    OrderTotal BINARY_FLOAT,
    OrderTimestamp timestamp default systimestamp,
    CustomerID varchar(100),
  constraint fk_Customer_CustomerID foreign key (CustomerID) 
      references Customer (CustomerID)  
);



Create table Vendor(
    VendorID varchar(100) NOT NULL PRIMARY KEY,
    VendorName varchar(1000) NOT NULL,
    VendorEmailID varchar(1000) NOT NULL 
);

Create table MagniFoodSchema.ListMenu(
    ListMenuName varchar(1000) NOT NULL,
    MenuID varchar(100) NOT NULL,
    VendorID varchar(1000),
      constraint fk_Menu_MenuID foreign key (MenuID) 
      references Vendor (MenuID),
      constraint fk_Vendor_VendorID foreign key (VendorID) 
      references Vendor (VendorID)
        );
        
Create table MagniFoodSchema.Menu(
    MenuID varchar(100) NOT NULL,
    FoodItemID varchar(100),
  constraint fk_FoodItem_FoodItemID foreign key (FoodItemID) 
      references FoodItem (FoodItem)
);

Create table MagniFoodSchema.FoodItem
(   
   FoodItemID varchar(100) NOT NULL PRIMARY KEY,
   FoodItemName varchar(1000) NOT NULL,
   FoodItemType varchar (1000) NOT NULL,
   FoodItemDescription clob
); 


