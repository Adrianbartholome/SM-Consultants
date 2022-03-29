-- Database Creation and Insert

Use IT488_USPS2

create table Drug_Allergy_List 
(
	Drug_Allergy_ID Int Not Null Identity(1,1) Primary Key,
	Drug_Allergy_Name VARCHAR(50),
);

Use IT488_USPS

