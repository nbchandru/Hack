alter session set "_ORACLE_SCRIPT"=true;

create user MagniFoodSchema IDENTIFIED BY MagniFoodSchema;

grant all privileges to MagniFoodSchema;

drop table  MagniFoodSchema.Person;

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
(   CustomerID varchar(100) NOT NULL PRIMARY KEY,
    CustomerType varchar(1000) NOT NULL,
    CustomerPassword varchar(1000) NOT NULL,
    CustomerStatus varchar(100) NOT NULL,
  constraint fk_Person_PersonID foreign key (CustomerID) 
      references MagniFoodSchema.Person (PersonID)  
);

insert into MagniFoodSchema.Customer values ('person01','noraml','abc@123','active');
insert into MagniFoodSchema.Customer values ('person02','Premium','abc@123','active');
insert into MagniFoodSchema.Customer values ('person03','noraml','abc@123','active');
insert into MagniFoodSchema.Customer values ('person04','noraml','abc@123','active');

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.Customer;
-- get 1 person based on ID
select * from  MagniFoodSchema.Customer where CustomerID Like'%person01%';



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
-- get 1 person based on ID when active
select * from  MagniFoodSchema.CafeteriaVendor where CafeteriaVendorID Like'%vendor01%' and CafeteriaVendorStatus like '%active%';


drop table MagniFoodSchema.CafeteriaVendorUser;

Create table MagniFoodSchema.CafeteriaVendorUser(
    CafeteriaVendorUserID varchar(100),
    CafeteriaVendorUserPassword varchar(1000) NOT NULL,
    CafeteriaVendorUserStatus varchar(100)NOT NULL,
    CafeteriaVendorID varchar(100) NOT NULL,
  constraint fk_CV_CafeteriaVendorID foreign key (CafeteriaVendorID) 
      references MagniFoodSchema.CafeteriaVendor (CafeteriaVendorID), 
  constraint fk_Person_PersonID2 foreign key (CafeteriaVendorUserID) 
      references MagniFoodSchema.Person (PersonID) 
);

insert into MagniFoodSchema.CafeteriaVendorUser values ('person01','abc@123','active','vendor01');
insert into MagniFoodSchema.CafeteriaVendorUser values ('person02','abc@123','active','vendor01');
insert into MagniFoodSchema.CafeteriaVendorUser values ('vendoruser03','abc@123','active','vendor02');
insert into MagniFoodSchema.CafeteriaVendorUser values ('person03','abc@123','active','vendor02');

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.CafeteriaVendorUser;
-- get 1 person based on ID
select * from  MagniFoodSchema.CafeteriaVendorUser where personid Like'%person01%';
-- endpoint queries:
-- get full customer info
select * from MagniFoodSchema.CafeteriaVendorUser c ,MagniFoodSchema.Person p where c.PersonID= p.PersonID;
--endpoint to active/inactive user
select * from MagniFoodSchema.CafeteriaVendorUser c ,MagniFoodSchema.Person p where c.PersonID= p.PersonID and c.CafeteriaVendorUserStatus like '%active%';



Create table MagniFoodSchema.Cafe(
    CafeID varchar(100) NOT NULL PRIMARY KEY,
    CafeName varchar(1000) NOT NULL,
    CafeStatus varchar(100) NOT NULL,
    CafeteriaVendorID varchar(100) NOT NULL,
      constraint fk_CV_CafeteriaVendorID2 foreign key (CafeteriaVendorID) 
      references MagniFoodSchema.CafeteriaVendor (CafeteriaVendorID)  
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

Create table MagniFoodSchema.CafeteriaStoreHouseInventory(
    CafeteriaStoreHouseIngredientID varchar(100) NOT NULL PRIMARY KEY,
    CafeteriaStoreHouseIngredientName varchar(1000) NOT NULL,
    CafeteriaStoreHouseIngredientQuantity BINARY_FLOAT default 0.0,
    CafeteriaStoreHouseIngredientQuantityUnit varchar(100),
    CafeteriaStoreHouseIngredientDescription clob,
    CafeteriaVendorID varchar(100) NOT NULL,
    constraint fk_Cafe_CafeID3 foreign key (CafeteriaVendorID) 
      references MagniFoodSchema.CafeteriaVendor (CafeteriaVendorID)
);

insert into MagniFoodSchema.CafeteriaStoreHouseInventory values ('ingredient01','Dough',10000,'grams','made from pillsberry','vendor01');
insert into MagniFoodSchema.CafeteriaStoreHouseInventory values ('ingredient02','Chicken',10000,'grams','made from pillsberry','vendor01');
insert into MagniFoodSchema.CafeteriaStoreHouseInventory values ('ingredient03','Egg',10000,'grams','made from pillsberry','vendor01');
insert into MagniFoodSchema.CafeteriaStoreHouseInventory values ('ingredient04','cooking oil',1000,'ml','made from pillsberry','vendor01');
insert into MagniFoodSchema.CafeteriaStoreHouseInventory values ('ingredient05','Secret  Sauce',100000,'Packet ','made from pillsberry','vendor01');
insert into MagniFoodSchema.CafeteriaStoreHouseInventory values ('ingredient06','Tomato',100000,'pieces','made from pillsberry','vendor01');

-- endpoint queries: 
-- get all ingredients
select * from  MagniFoodSchema.CafeteriaStoreHouseInventory;
-- get 1 ingredient based on ingredient id
select * from  MagniFoodSchema.CafeteriaStoreHouseInventory where CafeteriaStoreHouseIngredientID Like'%ingredient01%';
-- get all ingredients from 1 caffe
select * from  MagniFoodSchema.CafeteriaStoreHouseInventory where CafeID Like'%cafe01%';
-- get 1 ingredient based on ingredient id for 1 caffe
select * from  MagniFoodSchema.CafeteriaStoreHouseInventory where CafeID Like'%cafe01%' and CafeteriaStoreHouseIngredientID Like'%ingredient01%';
-- Post : add qeurry
insert into MagniFoodSchema.CafeteriaStoreHouseInventory values ('Ingredient05','Secret  Sauce','1','Packet ','made from pillsberry','cafe01');
-- Post : edit
Update MagniFoodSchema.CafeteriaStoreHouseInventory SET CafeteriaStoreHouseIngredientID= 'cafe01',
CafeStoreHouseIngredientName= 'Nandhi9',CafeStoreHouseIngredientQuantity='active',
CafeStoreHouseIngredientQuantityUnit='vendor01',CafeStoreHouseIngredientDescription= 'big description' 
where CafeID like '%cafe01%' and CafeteriaStoreHouseIngredientID = 'Ingredient01';



drop table MagniFoodSchema.CafeUser;

Create table MagniFoodSchema.CafeUser(
    CafeUserID varchar(100),
    CafeUserPassword varchar(1000) NOT NULL,
    CafeUserStatus varchar(100) NOT NULL,
    CafeID varchar(100) NOT NULL,
    constraint fk_Cafe_CafeID foreign key (CafeID) 
      references MagniFoodSchema.Cafe (CafeID),
  constraint fk_Person_PersonID3 foreign key (CafeUserID) 
      references MagniFoodSchema.Person (PersonID)  
);

insert into MagniFoodSchema.CafeUser values ('person01','abc@123','active','cafe01','person01');
insert into MagniFoodSchema.CafeUser values ('person02','abc@123','active','cafe02','person02');
insert into MagniFoodSchema.CafeUser values ('person03','abc@123','active','cafe03','person03');
insert into MagniFoodSchema.CafeUser values ('person04','abc@123','active','cafe04','person04');        

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.CafeUser;
-- get 1 person based on ID
select * from  MagniFoodSchema.CafeUser where PersonID Like'%person01%';

    drop table     MagniFoodSchema.CafeMenu;
    
    
Create table MagniFoodSchema.CafeMenu(
    CafeMenuID varchar(100) NOT NULL PRIMARY KEY,
    CafeMenuName varchar(100) NOT NULL,
    CafeMenuStatus varchar(100) NOT NULL,
    CafeID varchar(1000),
    constraint fk_cafe_cafeID2 foreign key (CafeID) 
      references MagniFoodSchema.Cafe (CafeID)
);

insert into MagniFoodSchema.CafeMenu values ('cafemenu01','nandhiSpecialMenu01','active','cafe01');    
insert into MagniFoodSchema.CafeMenu values ('cafemenu02','nandhiSpecialMenu02','active','cafe01');  
insert into MagniFoodSchema.CafeMenu values ('cafemenu03','nandhiSpecialMenu03','active','cafe01');  
insert into MagniFoodSchema.CafeMenu values ('cafemenu04','nandhiSpecialMenu04','active','cafe01');  

-- endpoint queries: 
-- get all people
select * from  MagniFoodSchema.CafeMenu;
-- get 1 person based on ID
select * from  MagniFoodSchema.CafeMenu where CafeMenuID Like'%cafemenu01%';

 drop table     MagniFoodSchema.FoodItem;
 
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
      references MagniFoodSchema.CafeMenu (CafeMenuID)
); 


insert into MagniFoodSchema.FoodItem values ('fooditem01','Chicken Egg Wrap ','nonveg','special sauce with chicken wrapped in egg roll',100.01,'available','cafemenu01'); 
insert into MagniFoodSchema.FoodItem values ('fooditem02','Chicken Egg Wrap ','nonveg','special sauce with chicken wrapped in egg roll',100.01,'available','cafemenu01'); 
insert into MagniFoodSchema.FoodItem values ('fooditem03','Chicken Egg Wrap ','nonveg','special sauce with chicken wrapped in egg roll',100.01,'available','cafemenu01'); 
insert into MagniFoodSchema.FoodItem values ('fooditem04','Chicken Egg Wrap ','nonveg','special sauce with chicken wrapped in egg roll',100.01,'available','cafemenu01'); 

-- endpoint queries: 
-- get all food items
select * from  MagniFoodSchema.FoodItem;
-- get all food item served on for 1 caffe based on caffe id
select * from  MagniFoodSchema.FoodItem f,MagniFoodSchema.CafeMenu m, MagniFoodSchema.Cafe c  where f.CAFEMENUID=m.CAFEMENUID and m.CAFEID=c.CAFEID and c.CafeID='cafe01'  ;
-- get 1 food item on ID
select * from  MagniFoodSchema.FoodItem where FoodItemID Like'%fooditem01%';
-- add 1 food item
insert into MagniFoodSchema.FoodItem values ('fooditem01','Chicken Egg Wrap ','nonveg','special sauce with chicken wrapped in egg roll',100.01,'available','cafemenu01'); 
-- edit 1 food item
Update MagniFoodSchema.FoodItem SET FoodItemID= 'fooditem01',
FoodItemName= 'Chicken Egg Wrap',FoodItemType='nonveg',
FoodItemDescription='special sauce with chicken wrapped in egg roll',FoodItemCost= 100.00,FoodItemStatus = 'available',CafeMenuID=''
where CafeMenuID like '%cafemenu01%' and FoodItemID like '%fooditem01%';


Create table MagniFoodSchema.FoodItemRecipe
(   
   FoodItemID varchar(100) NOT NULL,
   FoodItemIngredientQuantity BINARY_FLOAT NOT NULL,
   FoodItemIngredientQuantityUnit varchar(100) NOT NULL,
   CafeteriaStoreHouseIngredientID varchar(100) NOT NULL,
   constraint fk_FoodItem_FoodItemID foreign key (FoodItemID) 
      references MagniFoodSchema.FoodItem (FoodItemID),
  constraint fk_CSH_CSHIID foreign key (CafeteriaStoreHouseIngredientID) 
      references MagniFoodSchema.CafeteriaStoreHouse (CafeteriaStoreHouseIngredientID)
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
select * from  MagniFoodSchema.FoodItemRecipe where FoodItemID Like'%fooditem01%' and CafeteriaStoreHouseIngredientID like '%ingredient01%';
-- add 1 ingredient to a food item
insert into MagniFoodSchema.FoodItemRecipe values ('fooditem01',10,'packet','ingredient05');
-- edit 1 ingredient to a food item recipe
Update MagniFoodSchema.FoodItemRecipe SET FoodItemID= 'fooditem01',
FoodItemIngredientQuantity= 10,FoodItemIngredientQuantityUnit='grams',
CafeteriaStoreHouseIngredientID='ingredient01'where FoodItemID like '%fooditem01%' and CafeStoreHouseInventory like '%ingredient01%';


Create table MagniFoodSchema.CustomerOrder
(   CustomerOrderID varchar(100) NOT NULL PRIMARY KEY,
    CustomerOrderStatus varchar(100) NOT NULL,
    CustomerOrderTotal BINARY_FLOAT,
    CustomerOrderTimestamp timestamp default systimestamp,
    CafeID varchar(100) NOT NULL,
    CustomerID varchar(100),
    constraint fk_Cafe_CafeID4 foreign key (CafeID) 
      references MagniFoodSchema.Cafe (CafeID),
  constraint fk_Customer_CustomerID4 foreign key (CustomerID) 
      references MagniFoodSchema.Customer (CustomerID)  
);


insert into MagniFoodSchema.CustomerOrder (CustomerOrderID,CustomerOrderStatus,CustomerOrderTotal,CafeID,CustomerID) values ('customerorder01','pending',1000.01,'cafe01','customer01'); 
insert into MagniFoodSchema.CustomerOrder (CustomerOrderID,CustomerOrderStatus,CustomerOrderTotal,CafeID,CustomerID) values ('customerorder02','pending',1000.01,'cafe01','customer01'); 
insert into MagniFoodSchema.CustomerOrder (CustomerOrderID,CustomerOrderStatus,CustomerOrderTotal,CafeID,CustomerID) values ('customerorder03','delivered',1000.01,'cafe01','customer01'); 
insert into MagniFoodSchema.CustomerOrder (CustomerOrderID,CustomerOrderStatus,CustomerOrderTotal,CafeID,CustomerID) values ('customerorder04','started',1000.01,'cafe01','customer02'); 

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
      references MagniFoodSchema.CustomerOrder (CustomerOrderID),
    constraint fk_FoodItem_FoodItemID4 foreign key (FoodItemID) 
      references MagniFoodSchema.FoodItem (FoodItemID)
);

insert into MagniFoodSchema.CustomerOrderFoodList values ('customerorder01','fooditem01','orderplaced','cafemenu01'); 
insert into MagniFoodSchema.CustomerOrderFoodList values ('customerorder01','fooditem02','under preparation','cafemenu01'); 
insert into MagniFoodSchema.CustomerOrderFoodList values ('customerorder01','fooditem03','queued','cafemenu01'); 
insert into MagniFoodSchema.CustomerOrderFoodList values ('customerorder01','fooditem04','queued','cafemenu01'); 

-- endpoint queries: 
-- get all food items 
select * from  MagniFoodSchema.CustomerOrderFoodList;
-- get all active food items being cooked for 1 customer order
select * from  MagniFoodSchema.CustomerOrderFoodList where CustomerOrderID Like'%customerorder01%';
-- get all food customer ordered based on customer id
select * from  MagniFoodSchema.CustomerOrderFoodList f,MagniFoodSchema.CustomerOrder o, MagniFoodSchema.Customer c where f.CustomerOrderID=o.CUSTOMERORDERID and o.CUSTOMERID=c.CUSTOMERID and c.CUSTOMERID like '%customer01%';
-- get all active food items being cooked in 1 caffe
select * from  MagniFoodSchema.CustomerOrderFoodList x, MagniFoodSchema.FoodItem f,MagniFoodSchema.CafeMenu m, MagniFoodSchema.Cafe c where x.FoodItemID = f.FoodItemID and f.CAFEMENUID = m.CAFEMENUID and m.CafeID=c.CafeID and c.CafeID Like'%cafe01%';
-- add 1 food item to a order
insert into MagniFoodSchema.CustomerOrderFoodList values ('customerorder01','fooditem04','orderplaced','cafemenu01'); 
-- edit 1 food item placed in an ordr
Update MagniFoodSchema.FoodItemRecipe SET CustomerOrderID= 'customerorder01',
FoodItemID= 'fooditem01',FoodItemPreparationStatus='orderplaced',
CafeMenuID='cafemenu01' where CustomerOrderID like '%customerorder01%';
