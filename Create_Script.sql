create user MagniFoodSchema IDENTIFIED BY MagniFoodSchema;

grant all privileges to MagniFoodSchema;


Create table Person(
    PersonID varchar(100) NOT NULL PRIMARY KEY,
    PersonName varchar(1000) NOT NULL,
    PersonEmailID varchar(1000) NOT NULL,
    PersonMobContactNumber varchar(100) NOT NULL
);

Create table Customer 
(    CustomerID varchar(100) NOT NULL PRIMARY KEY,
    CustomerType varchar(1000) NOT NULL,
    CustomerPayment varchar(100) NOT NULL,
    PersonID varchar(100),
  constraint fk_Person_PersonID foreign key (PersonID) 
      references Person (PersonID)  
);

Create table CustomerOrder
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

Create table ListMenu(
    MenuID varchar(100) NOT NULL,
    ListMenuName varchar(1000) NOT NULL,
    VendorID varchar(1000),
      constraint fk_Menu_MenuID foreign key (MenuID) 
      references Vendor (MenuID),
      constraint fk_Vendor_VendorID foreign key (VendorID) 
      references Vendor (VendorID)
        );
        
Create table Menu(
    MenuID varchar(100) NOT NULL,
    FoodItemID varchar(100),
  constraint fk_FoodItem_FoodItemID foreign key (FoodItemID) 
      references FoodItem (FoodItem)
);

Create table FoodItem
(   
   FoodItemID varchar(100) NOT NULL PRIMARY KEY,
   FoodItemName varchar(1000) NOT NULL,
   FoodItemType varchar (1000) NOT NULL,
   FoodItemDescription clob
); 


