-- Database Creation and Insert
/*
Use IT488_USPS2

create table Prescription_Item 
(
	Prescription_ID Int Not Null Identity(1,1) Primary Key,
	Prescription_Name VARCHAR(50),
	Prescription_Quantity INT,
	Prescription_Refill INT,
	Prescription_Price VARCHAR(50)
);
*/
Use IT488_USPS2

insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Ibuprofen', 30, 1, '$98.97');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('calcium carbonate', 30, 2, '$12.99');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Oxygen', 30, 1, '$109.71');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Gabapentin', 30, 2, '$31.55');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Aloe socotrina, Antimonium cruda, Arsenicum album, Belladonna, Bryonia album, Chamomilla, Colocynthis, Podophyllum peltatum, Veratrum album', 30, 2, '$62.22');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('bupropion hydrobromide', 30, 2, '$107.95');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Enalapril Maleate', 30, 1, '$48.74');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Propranolol Hydrochloride', 30, 1, '$118.20');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Salsalate', 30, 2, '$70.94');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('buprenorphine', 30, 1, '$117.94');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('ACETAMINOPHEN, DEXTROMETHORPHAN HYDROBROMIDE, PHENYLEPHRINE HYDROCHLORIDE', 30, 2, '$96.00');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Methyl Salicylate, Capsaicin', 30, 0, '$31.34');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('OCTINOXATE and TITANIUM DIOXIDE', 30, 2, '$46.62');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('ALCOHOL', 30, 3, '$96.39');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Lamotrigine', 30, 3, '$71.17');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Minoxidil', 30, 2, '$85.52');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('GLYCERIN', 30, 3, '$61.02');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('BENZTROPINE MESYLATE', 30, 2, '$46.61');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Hydrocodone Bitartrate And Acetaminophen', 30, 0, '$46.69');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('NICOTINE', 30, 1, '$61.44');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('DIMETHICONE', 30, 2, '$26.55');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Horse Hair', 30, 2, '$97.94');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Enoxaparin Sodium', 30, 0, '$26.05');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Firebush/Burning Bush', 30, 1, '$61.23');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Dog Hair Canis spp.', 30, 2, '$97.19');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Acetaminophen, Aspirin, Caffeine', 60, 1, '$96.75');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('povidone-iodine', 60, 3, '$47.23');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Neomycin and Polymyxin B Sulfates and Bacitracin Zinc', 60, 2, '$114.39');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Minocycline hydrochloride', 60, 1, '$90.25');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Mirtazapine', 60, 0, '$33.13');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Midodrine Hydrochloride', 60, 1, '$39.27');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Ondansetron', 60, 0, '$11.95');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Risperidone', 60, 1, '$72.23');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('glyburide', 60, 1, '$43.46');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('nadolol and bendroflumethiazide', 60, 0, '$67.86');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Triazolam', 90, 3, '$51.53');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Captopril', 90, 0, '$12.60');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Ibuprofen', 90, 3, '$26.41');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Amoxicillin', 90, 0, '$63.39');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('ATRACTYLODES JAPONICA ROOT OIL', 90, 1, '$91.25');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('ranitidine hydrochloride', 90, 0, '$113.20');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Hemorrhoidal Rectal Suppositories', 90, 0, '$56.26');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('pioglitazone and metformin hydrochloride', 90, 2, '$53.96');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Diphenhydramine Hydrochloride and Phenylephrine Hydrochloride', 90, 2, '$92.30');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('tramadol hydrochloride', 90, 1, '$13.75');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Aspirin', 120, 0, '$27.49');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Octinoxate and Titanium dioxide', 120, 1, '$18.13');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Benzoyl Peroxide', 120, 0, '$33.00');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Povidone-Iodine', 120, 3, '$113.34');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Benzalkonium Chloride', 120, 1, '$36.57');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Felodipine', 120, 3, '$97.68');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('kidney bean', 120, 2, '$31.95');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Aluminum Chlorohydrate', 120, 0, '$64.53');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Diltiazem Hydrochloride', 120, 0, '$76.75');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Helium', 120, 3, '$48.57');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Ibuprofen', 180, 1, '$78.86');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('hydrocortisone', 180, 1, '$31.29');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Octinoxate and Octocrylene and Oxybenzone and Octisalate and Meradimate and Titantium Dioxide', 180, 0, '$63.50');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Ferrum Metallicum', 180, 0, '$107.55');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Titanium Dioxide', 180, 1, '$81.19');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Glimepiride', 180, 0, '$35.90');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Menthol', 180, 1, '$50.62');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('Sodium Fluoride', 180, 0, '$84.99');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('FAMOTIDINE', 180, 0, '$86.45');
insert into Prescription_Item (Prescription_Name, Prescription_Quantity, Prescription_Refill, Prescription_Price) values ('OCTINOXATE and TITANIUM DIOXIDE', 180, 0, '$42.97');
