create user MagniFoodSchema IDENTIFIED BY MagniFoodSchema;

grant all privileges to MagniFoodSchema;


Create table MagniFoodSchema.Person(
    PersonID varchar(100) NOT NULL PRIMARY KEY,
    PersonName varchar(1000) NOT NULL,
    PersonEmailID varchar(1000) NOT NULL unique,
    PersonMobContactNumber varchar(100) NOT NULL
);
-- append only table
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
    CustomerPassword varchar(1000) NOT NULL,
    CustomerStatus varchar(100) NOT NULL,
    PersonID varchar(100),
  constraint fk_Person_PersonID foreign key (PersonID) 
      references Person (PersonID)  
);

insert into MagniFoodSchema.Customer values ('customer01','noraml','abc@123','active','person01');
insert into MagniFoodSchema.Customer values ('customer02','Premium','abc@123','active','person02');
insert into MagniFoodSchema.Customer values ('customer03','noraml','abc@123','active','person02');
insert into MagniFoodSchema.Customer values ('customer04','noraml','abc@123','active','person03');

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.Customer;
-- get 1 person based on ID
select * from  MagniFoodSchema.Customer where CustomerID Like'%customer01%';



-- endpoint queries:
-- get full customer info
select * from MagniFoodSchema.Customer c ,MagniFoodSchema.Person p where c.PersonID= p.PersonID;


--endpoint to active/inactive user
select * from MagniFoodSchema.Customer c ,MagniFoodSchema.Person p where c.PersonID= p.PersonID and CustomerStatus like '%active%';


Create table MagniFoodSchema.CafeteriaVendor(
    CafeteriaVendorID varchar(100) NOT NULL PRIMARY KEY,
    CafeteriaVendorCompanyName varchar(1000) NOT NULL,
    CafeteriaVendorStatus varchar(100)NOT NULL
);


insert into MagniFoodSchema.CafeteriaVendor values ('vendor01','MagniFood - Magnitude Software','active');
insert into MagniFoodSchema.CafeteriaVendor values ('vendor02','MagniFood - Magnitude Software','active');
insert into MagniFoodSchema.CafeteriaVendor values ('vendor03','MagniFood - Magnitude Software','active');
insert into MagniFoodSchema.CafeteriaVendor values ('vendor04','MagniFood - Magnitude Software','active');

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.CafeteriaVendor;
-- get 1 person based on ID
select * from  MagniFoodSchema.CafeteriaVendor where CafeteriaVendorID Like'%vendor01%';

drop table MagniFoodSchema.CafeteriaVendorUser;

Create table MagniFoodSchema.CafeteriaVendorUser(
    CafeteriaVendorUserID varchar(100) NOT NULL PRIMARY KEY,
    CafeteriaVendorUserPassword varchar(1000) NOT NULL,
    CafeteriaVendorUserStatus varchar(100)NOT NULL,
    CafeteriaVendorID varchar(100) NOT NULL,
    PersonID varchar(100),
  constraint fk_CV_CafeteriaVendorID foreign key (CafeteriaVendorID) 
      references CafeteriaVendor (CafeteriaVendorID), 
  constraint fk_Person_PersonID2 foreign key (PersonID) 
      references Person (PersonID) 
);

insert into MagniFoodSchema.CafeteriaVendorUser values ('vendoruser01','abc@123','active','vendor01','person01');
insert into MagniFoodSchema.CafeteriaVendorUser values ('vendoruser02','abc@123','active','vendor01','person02');
insert into MagniFoodSchema.CafeteriaVendorUser values ('vendoruser03','abc@123','active','vendor02','person03');
insert into MagniFoodSchema.CafeteriaVendorUser values ('vendoruser04','abc@123','active','vendor02','person04');

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.CafeteriaVendorUser;
-- get 1 person based on ID
select * from  MagniFoodSchema.CafeteriaVendorUser where CafeteriaVendorUserID Like'%vendoruser04%';


Create table MagniFoodSchema.Cafe(
    CafeID varchar(100) NOT NULL PRIMARY KEY,
    CafeName varchar(1000) NOT NULL,
    CafeStatus varchar(100) NOT NULL,
    CafeteriaVendorID varchar(100) NOT NULL,
      constraint fk_CV_CafeteriaVendorID2 foreign key (CafeteriaVendorID) 
      references CafeteriaVendor (CafeteriaVendorID)  
);

insert into MagniFoodSchema.Cafe values ('Cafe01','Nandhi1','active','vendor01');
insert into MagniFoodSchema.Cafe values ('Cafe02','Nandhi2','active','vendor01');
insert into MagniFoodSchema.Cafe values ('Cafe03','Nandhi3','active','vendor01');
insert into MagniFoodSchema.Cafe values ('Cafe04','Nandhi4','active','vendor02');

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.Cafe;
-- get 1 person based on ID
select * from  MagniFoodSchema.Cafe where CafeID Like'%Cafe01%';
-- Post : add qeurry
insert into MagniFoodSchema.Cafe values ('CafeIDX','NameX','active','vendorIDX');
-- Post : add edit
Update MagniFoodSchema.Cafe SET CafeID= 'Cafe01',CafeName= 'Nandhi9',CafeStatus='active',CafeteriaVendorID='vendor01' where CafeID like '%Cafe01%';
--


drop table MagniFoodSchema.CafeUser;

Create table MagniFoodSchema.CafeUser(
    CafeUserID varchar(100) NOT NULL PRIMARY KEY,
    CafeUserPassword varchar(1000) NOT NULL,
    CafeUserStatus varchar(100) NOT NULL,
    CafeID varchar(100) NOT NULL,
    PersonID varchar(100),
    constraint fk_Cafe_CafeID foreign key (CafeID) 
      references Cafe (CafeID),
  constraint fk_Person_PersonID3 foreign key (PersonID) 
      references Person (PersonID)  
);

insert into MagniFoodSchema.CafeUser values ('CafeUser01','abc@123','active','Cafe01','person01');
insert into MagniFoodSchema.CafeUser values ('CafeUser02','abc@123','active','Cafe02','person02');
insert into MagniFoodSchema.CafeUser values ('CafeUser03','abc@123','active','Cafe03','person03');
insert into MagniFoodSchema.CafeUser values ('CafeUser04','abc@123','active','Cafe04','person04');        

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.CafeUser;
-- get 1 person based on ID
select * from  MagniFoodSchema.CafeUser where CafeUserID Like'%CafeUser01%';

        
Create table MagniFoodSchema.CafeMenu(
    CafeMenuID varchar(100) NOT NULL PRIMARY KEY,
    CafeMenuName varchar(100) NOT NULL,
    CafeMenuStatus varchar(100) NOT NULL,
    CafeID varchar(1000),
    constraint fk_Vendor_VendorID foreign key (CafeID) 
      references Vendor (CafeID)
);

Create table MagniFoodSchema.FoodItem
(   
   FoodItemID varchar(100) NOT NULL PRIMARY KEY,
   FoodItemName varchar(1000) NOT NULL,
   FoodItemType varchar (1000) NOT NULL,
   FoodItemDescription clob,
   FoodItemStatus varchar(100) NOT NULL,
   CafeMenuID varchar(100) NOT NULL,
   constraint fk_CafeMenu_CafeMenuID foreign key (CafeMenuID) 
      references CafeMenu (CafeMenu)
); 

Create table MagniFoodSchema.CustomerOrder
(   CustomerOrderID varchar(100) NOT NULL PRIMARY KEY,
    CustomerOrderStatus varchar(100) NOT NULL,
    CustomerOrderTotal BINARY_FLOAT,
    CustomerOrderTimestamp timestamp default systimestamp,
    CafeID varchar(100) NOT NULL,
    CustomerID varchar(100),
    constraint fk_Cafe_CafeID foreign key (CafeID) 
      references Cafe (CafeID),
  constraint fk_Customer_CustomerID foreign key (CustomerID) 
      references Customer (CustomerID)  
);

Create table MagniFoodSchema.CustomerOrderFoodList
(   OrderID varchar(100) NOT NULL,
    FoodItemID varchar(100)NOT NULL,
    FoodItemPreparationStatus varchar(100),
    CafeMenuID varchar(100) NOT NULL,
  constraint fk_CustomerOrder_CustomerOrderID foreign key (CustomerOrderID) 
      references Customer (CustomerOrderID),
    constraint fk_FoodItem_FoodItemID foreign key (FoodItemID) 
      references Customer (FoodItemID)
);
