USE IT488_USPS

-- Create Tables.sql
/*
Create table Customers_List
(
Customer_ID int primary key,
Customer_FirstName VarChar(255),
Customer_LastName VarChar(255),
Customer_DateOfBirth Date,
Customer_Phone VarChar(10),
Customer_Email VarChar(255),
Customer_Address VarChar(255),
Customer_City VarChar(255),
Customer_State VarChar(255),
Customer_ZipCode VarChar(255),
Customer_DrugAllergy Text,
)

Create table Customers_Payment_Info
(
Customer_Payment_ID int primary key,
Customer_CreditCard_Number int,
Customer_Credit_ExpDate Date,
)

Create table Doctor_List
(
Doctor_ID int primary key,
Doctor_FirstName VarChar(255),
Doctor_LastName VarChar(255),
Doctor_Phone VarChar(10),
Doctor_Email VarChar(255),
)

Create table Prescription_Item
(
Prescription_ID int primary key,
Prescription_Name VarChar(255),
Prescription_Quantity int,
Prescription_Price SmallMoney,
)

Create table Purchase_Orders
(
PO_ID int primary key,
PO_Date date,
PO_Total int,
)
*/

Create table Order_History
(
OH_ID int identity (1,1) primary key
Customer_ID int,
Prescription_ID int,

)
