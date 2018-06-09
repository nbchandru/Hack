alter session set "_ORACLE_SCRIPT"=true;

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

insert into MagniFoodSchema.Cafe values ('cafe01','Nandhi1','active','vendor01');
insert into MagniFoodSchema.Cafe values ('cafe02','Nandhi2','active','vendor01');
insert into MagniFoodSchema.Cafe values ('cafe03','Nandhi3','active','vendor01');
insert into MagniFoodSchema.Cafe values ('cafe04','Nandhi4','active','vendor02');

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.Cafe;
-- get 1 person based on ID
select * from  MagniFoodSchema.Cafe where CafeID Like'%cafe01%';
-- Post : add qeurry
insert into MagniFoodSchema.Cafe values ('cafeIDX','NameX','active','vendorIDX');
-- Post : add edit
Update MagniFoodSchema.Cafe SET CafeID= 'cafe01',CafeName= 'Nandhi9',CafeStatus='active',CafeteriaVendorID='vendor01' where CafeID like '%cafe01%';
--Post: delete/disable
Update MagniFoodSchema.Cafe SET CafeStatus='disabled' where CafeteriaVendorID like '%vendor01%' and CafeID like '%cafe01%';

Create table MagniFoodSchema.CafeStoreHouseInventory(
    CafeStoreHouseIngredientID varchar(100) NOT NULL PRIMARY KEY,
    CafeStoreHouseIngredientName varchar(1000) NOT NULL,
    CafeStoreHouseIngredientQuantity BINARY_FLOAT default 0.0,
    CafeStoreHouseIngredientQuantityUnit varchar(100),
    CafeStoreHouseIngredientDescription clob,
    CafeID varchar(100) NOT NULL,
    constraint fk_Cafe_CafeID foreign key (CafeID) 
      references Cafe (CafeID)
);

insert into MagniFoodSchema.CafeStoreHouseInventory values ('ingredient01','Dough',10000,'grams','made from pillsberry','cafe01');
insert into MagniFoodSchema.CafeStoreHouseInventory values ('ingredient02','Chicken',10000,'grams','made from pillsberry','cafe01');
insert into MagniFoodSchema.CafeStoreHouseInventory values ('ingredient03','Egg',10000,'grams','made from pillsberry','cafe01');
insert into MagniFoodSchema.CafeStoreHouseInventory values ('ingredient04','cooking oil',1000,'ml','made from pillsberry','cafe01');
insert into MagniFoodSchema.CafeStoreHouseInventory values ('ingredient05','Secret  Sauce',100000,'Packet ','made from pillsberry','cafe01');
insert into MagniFoodSchema.CafeStoreHouseInventory values ('ingredient06','Tomato',100000,'pieces','made from pillsberry','cafe01');

-- endpoint queries: 
-- get all ingredients
select * from  MagniFoodSchema.CafeStoreHouseInventory;
-- get 1 ingredient based on ingredient id
select * from  MagniFoodSchema.CafeStoreHouseInventory where CafeStoreHouseIngredientID Like'%Ingredient01%';
-- get all ingredients from 1 caffe
select * from  MagniFoodSchema.CafeStoreHouseInventory where CafeID Like'%cafe01%';
-- get 1 ingredient based on ingredient id for 1 caffe
select * from  MagniFoodSchema.CafeStoreHouseInventory where CafeID Like'%cafe01%' and CafeStoreHouseIngredientID Like'%Ingredient01%';
-- Post : add qeurry
insert into MagniFoodSchema.CafeStoreHouseInventory values ('Ingredient05','Secret  Sauce','1','Packet ','made from pillsberry','cafe01');
-- Post : edit
Update MagniFoodSchema.Cafe SET CafeStoreHouseIngredientID= 'cafe01',
CafeStoreHouseIngredientName= 'Nandhi9',CafeStoreHouseIngredientQuantity='active',
CafeStoreHouseIngredientQuantityUnit='vendor01',CafeStoreHouseIngredientDescription= 'big description' 
where CafeID like '%cafe01%' and CafeStoreHouseIngredientID = 'Ingredient01';



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

insert into MagniFoodSchema.CafeUser values ('cafeuser01','abc@123','active','cafe01','person01');
insert into MagniFoodSchema.CafeUser values ('cafeuser02','abc@123','active','cafe02','person02');
insert into MagniFoodSchema.CafeUser values ('cafeuser03','abc@123','active','cafe03','person03');
insert into MagniFoodSchema.CafeUser values ('cafeuser04','abc@123','active','cafe04','person04');        

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.CafeUser;
-- get 1 person based on ID
select * from  MagniFoodSchema.CafeUser where CafeUserID Like'%cafeuser01%';

        
Create table MagniFoodSchema.CafeMenu(
    CafeMenuID varchar(100) NOT NULL PRIMARY KEY,
    CafeMenuName varchar(100) NOT NULL,
    CafeMenuStatus varchar(100) NOT NULL,
    CafeID varchar(1000),
    constraint fk_Vendor_VendorID foreign key (CafeID) 
      references Vendor (CafeID)
);

insert into MagniFoodSchema.CafeMenu values ('cafemenu01','nandhiSpecialMenu01','active');    
insert into MagniFoodSchema.CafeMenu values ('cafemenu02','nandhiSpecialMenu02','active');  
insert into MagniFoodSchema.CafeMenu values ('cafemenu03','nandhiSpecialMenu03','active');  
insert into MagniFoodSchema.CafeMenu values ('cafemenu04','nandhiSpecialMenu04','active');  

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.CafeMenu;
-- get 1 person based on ID
select * from  MagniFoodSchema.CafeMenu where CafeMenuID Like'%cafemenu01%';


Create table MagniFoodSchema.FoodItem
(   
   FoodItemID varchar(100) NOT NULL PRIMARY KEY,
   FoodItemName varchar(1000) NOT NULL,
   FoodItemType varchar (1000) NOT NULL,
   FoodItemDescription clob,
   FoodItemCost BINARY_FLOAT,
   FoodItemStatus varchar(100) NOT NULL,
   CafeMenuID varchar(100) NOT NULL,
   constraint fk_CafeMenu_CafeMenuID foreign key (CafeMenuID) 
      references CafeMenu (CafeMenu)
); 


insert into MagniFoodSchema.FoodItem values ('fooditem01','Chicken Egg Wrap ','nonveg','special sauce with chicken wrapped in egg roll',100.01,'available','cafe01'); 
insert into MagniFoodSchema.FoodItem values ('fooditem02','Chicken Egg Wrap ','nonveg','special sauce with chicken wrapped in egg roll',100.01,'available','cafe01'); 
insert into MagniFoodSchema.FoodItem values ('fooditem03','Chicken Egg Wrap ','nonveg','special sauce with chicken wrapped in egg roll',100.01,'available','cafe01'); 
insert into MagniFoodSchema.FoodItem values ('fooditem04','Chicken Egg Wrap ','nonveg','special sauce with chicken wrapped in egg roll',100.01,'available','cafe01'); 

-- endpoint queries: 
-- get all food items
select * from  MagniFoodSchema.FoodItem;
-- get 1 food item on ID
select * from  MagniFoodSchema.FoodItem where FoodItemID Like'%fooditem01%';
-- add 1 food item
insert into MagniFoodSchema.FoodItem values ('fooditem01','Chicken Egg Wrap ','nonveg','special sauce with chicken wrapped in egg roll',100.01,'available','cafe01'); 
-- edit 1 food item
Update MagniFoodSchema.FoodItem SET FoodItemID= 'cafe01',
FoodItemName= 'Nandhi9',FoodItemType='active',
FoodItemDescription='vendor01',FoodItemCost= 'big description',FoodItemStatus = 'big description',CafeMenuID=''
where CafeMenuID like '%cafemenu01%' and FoodItemID like '%%';


Create table MagniFoodSchema.FoodItemRecipe
(   
   FoodItemID varchar(100) NOT NULL,
   FoodItemIngredientQuantity BINARY_FLOAT NOT NULL,
   FoodItemIngredientQuantityUnit varchar(100) NOT NULL,
   CafeStoreHouseIngredientID varchar(100) NOT NULL,
   constraint fk_FoodItem_FoodItemID foreign key (FoodItemID) 
      references FoodItem (FoodItemID),
  constraint fk_CSH_CSHIID foreign key (CafeStoreHouseIngredientID) 
      references CafeStoreHouseInventory (CafeStoreHouseIngredientID)
); 


insert into MagniFoodSchema.FoodItemRecipe values ('fooditem01',100,'gram','ingredient01'); 
insert into MagniFoodSchema.FoodItemRecipe values ('fooditem01',50,'gram','ingredient02');
insert into MagniFoodSchema.FoodItemRecipe values ('fooditem01',1,'gram','ingredient03');
insert into MagniFoodSchema.FoodItemRecipe values ('fooditem01',10,'ml','ingredient04');
insert into MagniFoodSchema.FoodItemRecipe values ('fooditem01',10,'packet','ingredient05');

-- endpoint queries: 
-- get all food ingredients for all items
select * from  MagniFoodSchema.FoodItemRecipe;
-- get all food ingredients for 1 fooditem
select * from  MagniFoodSchema.FoodItemRecipe where FoodItemID Like'%fooditem01%';
-- get 1 food ingredient for 1 fooditem
select * from  MagniFoodSchema.FoodItemRecipe where FoodItemID Like'%fooditem01%' and CafeStoreHouseIngredientID like '%ingredient01%';
-- add 1 ingredient to a food item
insert into MagniFoodSchema.FoodItemRecipe values ('fooditem01',10,'packet','ingredient05');
-- edit 1 ingredient to a food item recipe
Update MagniFoodSchema.FoodItemRecipe SET FoodItemID= 'cafe01',
FoodItemIngredientQuantity= 'Nandhi9',FoodItemIngredientQuantityUnit='active',
CafeStoreHouseIngredientID='vendor01'where FoodItemID like '%fooditem01%' and CafeStoreHouseInventory like '%ingredient01%';


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



insert into MagniFoodSchema.CustomerOrder values ('customerorder01','pending',1000.01,'cafe01','customer01'); 
insert into MagniFoodSchema.CustomerOrder values ('customerorder02','pending',1000.01,'cafe01','customer01'); 
insert into MagniFoodSchema.CustomerOrder values ('customerorder03','delivered',1000.01,'cafe01','customer01'); 
insert into MagniFoodSchema.CustomerOrder values ('customerorder04','started',1000.01,'cafe01','customer02'); 

-- endpoint queries: 
-- get all orders
select * from  MagniFoodSchema.CustomerOrder;
-- get all food ingredients for 1 cafe
select * from  MagniFoodSchema.CustomerOrder where CafeID Like'%cafe01%';
-- get all food ingredients for 1 Customer
select * from  MagniFoodSchema.CustomerOrder where CustomerID Like'%customer02%';
-- add 1 ingredient to a food item
insert into MagniFoodSchema.CustomerOrder values ('customerorder04','started',1000.01,'cafe01','customer02');
-- edit 1 customer order based on customerorder04 id 
Update MagniFoodSchema.FoodItemRecipe SET CustomerOrderID= 'customerorder01',
CustomerOrderStatus= 'pending',CustomerOrderTotal=100.00,
CafeID='cafe01',CustomerID='customer01' where CustomerOrderID like '%customerorder01%';


Create table MagniFoodSchema.CustomerOrderFoodList
(   CustomerOrderID varchar(100) NOT NULL,
    FoodItemID varchar(100)NOT NULL,
    FoodItemPreparationStatus varchar(100) DEFAULT 'orderplaced',
    CafeMenuID varchar(100) NOT NULL,
  constraint fk_CustomerOrder_CustomerOrderID foreign key (CustomerOrderID) 
      references Customer (CustomerOrderID),
    constraint fk_FoodItem_FoodItemID foreign key (FoodItemID) 
      references Customer (FoodItemID)
);

insert into MagniFoodSchema.CustomerOrder values ('customerorder01','fooditem01','orderplaced','cafemenu01'); 
insert into MagniFoodSchema.CustomerOrder values ('customerorder01','fooditem02','under preparation','cafemenu01'); 
insert into MagniFoodSchema.CustomerOrder values ('customerorder01','fooditem03','queued','cafemenu01'); 
insert into MagniFoodSchema.CustomerOrder values ('customerorder01','fooditem04','queued','cafemenu01'); 

-- endpoint queries: 
-- get all food items 
select * from  MagniFoodSchema.CustomerOrderFoodList;
-- get all active food items being cooked for 1 customer order
select * from  MagniFoodSchema.CustomerOrderFoodList where CustomerOrderID Like'%customerorder01%'
-- get all active food items being cooked in 1 caffe
select * from  MagniFoodSchema.CustomerOrderFoodList where CafeID Like'%cafe01%';
-- add 1 food item to a order
insert into MagniFoodSchema.CustomerOrder values ('customerorder01','fooditem04','orderplaced','cafemenu01'); 
-- edit 1 food item placed in an ordr
Update MagniFoodSchema.FoodItemRecipe SET CustomerOrderID= 'customerorder01',
FoodItemID= 'fooditem01',FoodItemPreparationStatus='orderplaced',
CafeMenuID='cafemenu01' where CustomerOrderID like '%customerorder01%';
