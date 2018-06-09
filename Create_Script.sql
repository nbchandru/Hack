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
  constraint fk_Person_PersonID foreign key (PersonID) 
      references Person (PersonID)  
);

Create table CustomerOrder
(    OrderID varchar(100) NOT NULL PRIMARY KEY,
    OrderTotal BINARY_FLOAT,
    OrderTimestamp timestamp default systimestamp,
  constraint fk_Customer_CustomerID foreign key (CustomerID) 
      references Customer (CustomerID)  
);

Create table Vendor(
    VendorID varchar(100) NOT NULL PRIMARY KEY,
    VendorName varchar(1000) NOT NULL,
    VendorEmailID varchar(1000) NOT NULL 
);

Create table Menu(
    MenuID varchar(100) NOT NULL PRIMARY KEY,
    MenuName varchar(1000) NOT NULL,
  constraint fk_Vendor_VendorID foreign key (VendorID) 
      references Vendor (VendorID)  
);

Create table FoodItem
(
  constraint fk_Vendor_VendorID foreign key (VendorID) 
      references Vendor (VendorID) 
);


