using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data;
using System.Windows.Forms;

namespace USPS
{
    public class DB
    {
        string connectionstring;
        static string userPass;
        static string userID;
        static string dbID;
        public DB()
        {

        }
        public void DBLogin(string pass, string user)
        {
            var conn = new SqlConnection();

            userPass = pass; // these take input from relevant fields
            userID = user;

            //builder class allows to use variables for user and password fields (get user input)
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost\\SQLExpress";
            builder.InitialCatalog = "IT488_USPS";
            builder.TrustServerCertificate = true;
            builder.Password = pass;
            builder.UserID = user;
            connectionstring = builder.ConnectionString;

            conn.ConnectionString = connectionstring;
            try
            {
                //open session
                conn.Open();
                Form1.failed = false;
                Form1.mySystemMessage("Successfully connected!");

            }
            catch (Exception ex)
            {
                Form1.failed = true;
                Form1.mySystemMessage("Login failed.");
                Console.WriteLine(ex.Message);
            }

        }

        public void logOut()
        {
            Form1.mySystemMessage("Successfully logged out.");
            Form1.customerPanelSwitch = Form1.adminPanelSwitch = Form1.pharmPanelSwitch = false;
        }

        public void infoUpdater(Dictionary<string, string> info)
        {
            //Check if new user
            if (Form1.newUserCheck)
            {
                SqlConnection cnn = new SqlConnection(connectionstring);
                cnn.Open();

                //check if username exists
                SqlCommand cmd = new SqlCommand("SELECT count(*) FROM Customers_List WHERE Customer_ID = @id", cnn);

                cmd.Parameters.AddWithValue("@id", dbID);

                int exists = (Int32)cmd.ExecuteScalar();

                //if username already exists, they have logged in with the correct credentials,
                //so inform them that the user exists and update their data
                if (exists > 0)
                {
                    Form1.mySystemMessage("User already exists. Updating your information.");
                    Form1.newUserCheck = false;
                    infoUpdater(info);
                }
                else
                {
                    cmd = new SqlCommand("INSERT INTO Customers_List (Customer_FirstName, Customer_LastName, " +
                    $"Customer_DateOfBirth, Customer_Address, Customer_City, Customer_State, Customer_ZipCode, " +
                    $"Customer_DrugAllergy, Customer_Phone, Customer_Email, Customer_UserName, Customer_Insurance) VALUES (@fn, @ln, " +
                    $"@db, @ad, @ci, @st, @zp, @al, @ph, @em, @id, @in)", cnn);

                    cmd.Parameters.AddWithValue("@fn", info["fname"]);
                    cmd.Parameters.AddWithValue("@ln", info["lname"]);
                    cmd.Parameters.AddWithValue("@db", info["dob"]);
                    cmd.Parameters.AddWithValue("@ad", info["address"]);
                    cmd.Parameters.AddWithValue("@ci", info["city"]);
                    cmd.Parameters.AddWithValue("@st", info["state"]);
                    cmd.Parameters.AddWithValue("@zp", info["zip"]);
                    cmd.Parameters.AddWithValue("@al", info["allergy"]);
                    cmd.Parameters.AddWithValue("@ph", info["phone"]);
                    cmd.Parameters.AddWithValue("@em", info["email"]);
                    cmd.Parameters.AddWithValue("@id", userID);
                    cmd.Parameters.AddWithValue("@in", info["insure"]);

                    cmd.ExecuteNonQuery();
                    Form1.newUserCheck = false;
                }
            }
            //if not new user do usual update
            else
            {
                SqlConnection cnn = new SqlConnection(connectionstring);
                cnn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE Customers_List SET Customer_FirstName = @fn, " +
                                $"Customer_LastName = @ln, Customer_DateOfBirth = @db, Customer_Address = @ad, " +
                                $"Customer_City = @ci, Customer_State = @st, Customer_ZipCode = @zp, " +
                                $"Customer_DrugAllergy = @al, Customer_Phone = @ph, Customer_Email = @em, " +
                                $"Customer_Insurance = @in WHERE Customer_ID = @id", cnn);

                cmd.Parameters.AddWithValue("@fn", info["fname"]);
                cmd.Parameters.AddWithValue("@ln", info["lname"]);
                cmd.Parameters.AddWithValue("@db", info["dob"]);
                cmd.Parameters.AddWithValue("@ad", info["address"]);
                cmd.Parameters.AddWithValue("@ci", info["city"]);
                cmd.Parameters.AddWithValue("@st", info["state"]);
                cmd.Parameters.AddWithValue("@zp", info["zip"]);
                cmd.Parameters.AddWithValue("@al", info["allergy"]);
                cmd.Parameters.AddWithValue("@ph", info["phone"]);
                cmd.Parameters.AddWithValue("@em", info["email"]);
                cmd.Parameters.AddWithValue("@in", info["insure"]);
                cmd.Parameters.AddWithValue("@id", dbID);

                using (cmd)
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Form1.mySystemMessage(ex.Message);
                    }
                }
                
            }
        }
        
        public Dictionary<string, string> infoUpdateQuery()
        {
            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("ID", "");
            info.Add("fname", "");
            info.Add("lname", "");
            info.Add("dob", "");
            info.Add("phone", "");
            info.Add("email", "");
            info.Add("address", "");
            info.Add("city", "");
            info.Add("state", "");
            info.Add("zip", "");
            info.Add("allergy", "");
            info.Add("payID", "");
            info.Add("cc", "");
            info.Add("exp", "");
            info.Add("doc", "");
            info.Add("docFname", "");
            info.Add("docLname", "");
            info.Add("docPhone", "");
            info.Add("docEmail", "");
            info.Add("insure", "");

            SqlDataReader dataReader;

            SqlConnection cnn = new SqlConnection(connectionstring);
            try
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand("SELECT CL.Customer_ID, Customer_FirstName, Customer_LastName, Customer_UserName, " +
                    "Customer_DateOfBirth, Customer_Phone, Customer_Email, Customer_Address, " +
                    "Customer_City, Customer_State, Customer_ZipCode, Customer_DrugAllergy, Customer_Payment_ID, " +
                    "Customer_Credit_Number, Customer_Credit_ExpDate, Customer_Insurance, Doctor_ID, Doctor_FirstName, " +
                    "Doctor_LastName, Doctor_Phone, Doctor_Email, PI.Prescription_ID, Prescription_Name, " +
                    "Prescription_Quantity, Prescription_Refill, PI.Prescription_Price, PO_ID, PO_Date, " +
                    "PO_Total, PO_ShipDate FROM Customers_List AS CL " +
                    "INNER JOIN Customers_Payment_Info AS CPI ON CL.Customer_ID = CPI.Customer_ID " +
                    "INNER JOIN Doctor_List AS DL ON CPI.Customer_ID = DL.Customer_ID " + 
                    "INNER JOIN Prescription_Item AS PI ON DL.Customer_ID = PI.Customer_ID " +
                    "INNER JOIN Purchase_Orders AS PO ON PI.Customer_ID = PO.Customer_ID " +
                    "WHERE Customer_UserName = @un", cnn);
                cmd.Parameters.AddWithValue("@un", userID);
                try
                {
                    dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                info["ID"] = dataReader["Customer_ID"].ToString();
                                info["fname"] = dataReader[1].ToString();
                                info["lname"] = dataReader[2].ToString();
                                if (dataReader[4].ToString().Length > 20)
                                {
                                    info["dob"] = dataReader[4].ToString().Remove(10);
                                }
                                else
                                {
                                    info["dob"] = dataReader[4].ToString().Remove(9);
                                }
                                info["phone"] = dataReader[5].ToString();
                                info["email"] = dataReader[6].ToString();
                                info["address"] = dataReader[7].ToString();
                                info["city"] = dataReader[8].ToString();
                                info["state"] = dataReader[9].ToString();
                                info["zip"] = dataReader[10].ToString();
                                info["allergy"] = dataReader[11].ToString();
                                info["payID"] = dataReader[12].ToString();
                                info["cc"] = dataReader[13].ToString();
                                if (dataReader[14].ToString().Length > 20)
                                {
                                    info["exp"] = dataReader[14].ToString().Remove(10);
                                }
                                else
                                {
                                    info["exp"] = dataReader[14].ToString().Remove(9);
                                }
                                info["doc"] = dataReader[16].ToString();
                                info["docFname"] = dataReader[17].ToString();
                                info["docLname"] = dataReader[18].ToString();
                                info["docPhone"] = dataReader[19].ToString();
                                info["docEmail"] = dataReader[20].ToString();
                                info["insure"] = dataReader[15].ToString();
                                dbID = info["ID"];
                                return info;
                            }
                            catch (Exception ex)
                            {
                                Form1.mySystemMessage("Error with query.");
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        dataReader.Close();
                        cnn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Form1.mySystemMessage(ex.Message);
                    return null;
                }
                Form1.noAdditionalData = true;

                cnn.Open();
                cmd = new SqlCommand("SELECT * FROM Customers_List, Customers_Payment_Info WHERE Customer_UserName = @un", cnn);
                cmd.Parameters.AddWithValue("@un", userID);
                try
                {
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        try
                        {
                            info["ID"] = dataReader["Customer_ID"].ToString();
                            info["fname"] = dataReader["Customer_FirstName"].ToString();
                            info["lname"] = dataReader["Customer_LastName"].ToString();
                            if (dataReader["Customer_DateOfBirth"].ToString().Length > 20)
                            {
                                info["dob"] = dataReader["Customer_DateOfBirth"].ToString().Remove(10);
                            }
                            else
                            {
                                info["dob"] = dataReader["Customer_DateOfBirth"].ToString().Remove(9);
                            }
                            info["phone"] = dataReader["Customer_Phone"].ToString();
                            info["email"] = dataReader["Customer_Email"].ToString();
                            info["address"] = dataReader["Customer_Address"].ToString();
                            info["city"] = dataReader["Customer_City"].ToString();
                            info["state"] = dataReader["Customer_State"].ToString();
                            info["zip"] = dataReader["Customer_ZipCode"].ToString();
                            info["allergy"] = dataReader["Customer_DrugAllergy"].ToString();
                            info["payID"] = dataReader["Customer_Payment_ID"].ToString();
                            info["cc"] = dataReader["Customer_Credit_Number"].ToString();
                            if (dataReader["Customer_Credit_ExpDate"].ToString().Length > 20)
                            {
                                info["exp"] = dataReader["Customer_Credit_ExpDate"].ToString().Remove(10);
                            }
                            else
                            {
                                info["exp"] = dataReader["Customer_Credit_ExpDate"].ToString().Remove(9);
                            }
                            info["insure"] = dataReader["Customer_Insurance"].ToString();
                            dbID = info["ID"];

                            return info;
                        }
                        catch (Exception ex)
                        {
                            Form1.mySystemMessage("Error with query.");
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Form1.mySystemMessage(ex.Message);
                    return null;
                }
                dbID = info["ID"];

                return info;
            }
            catch (Exception ex)
            {
                Form1.mySystemMessage(ex.Message);

            }
            return null;
        }

        public Dictionary<string, string> searchCustomers(string searchString)
        {
            //temp string to store and convert date if present
            string dt = "";

            //check the string to see if date is present, then store to string dt
            if (!Regex.IsMatch(searchString, "[^0-9/]"))
            {
                dt = searchString;
                string[] dateSplit = dt.Split('/');
                DateTime td = new DateTime(Convert.ToInt32(dateSplit[2]),
                    Convert.ToInt32(dateSplit[0]), Convert.ToInt32(dateSplit[1]));
                dt = td.ToShortDateString();
            }

            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("ID", "");
            info.Add("fname", "");
            info.Add("lname", "");
            info.Add("dob", "");
            info.Add("phone", "");
            info.Add("email", "");
            info.Add("address", "");
            info.Add("city", "");
            info.Add("state", "");
            info.Add("zip", "");
            info.Add("allergy", "");
            info.Add("payID", "");
            info.Add("cc", "");
            info.Add("exp", "");
            info.Add("doc", "");
            info.Add("docFname", "");
            info.Add("docLname", "");
            info.Add("docPhone", "");
            info.Add("docEmail", "");
            info.Add("insure", "");

            SqlDataReader dataReader;

            //if dt is not empty, search by date, otherwise search by name
            SqlConnection cnn = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand();
            cnn.Open();
            if (dt != "")
            {
                cmd = new SqlCommand("SELECT CL.Customer_ID, Customer_FirstName, Customer_LastName, Customer_UserName, " +
                    "Customer_DateOfBirth, Customer_Phone, Customer_Email, Customer_Address, " +
                    "Customer_City, Customer_State, Customer_ZipCode, Customer_DrugAllergy, Customer_Payment_ID, " +
                    "Customer_Credit_Number, Customer_Credit_ExpDate, Customer_Insurance, Doctor_ID, Doctor_FirstName, " +
                    "Doctor_LastName, Doctor_Phone, Doctor_Email, PI.Prescription_ID, Prescription_Name, " +
                    "Prescription_Quantity, Prescription_Refill, PI.Prescription_Price, PO_ID, PO_Date, " +
                    "PO_Total, PO_ShipDate FROM Customers_List AS CL " +
                    "INNER JOIN Customers_Payment_Info AS CPI ON CL.Customer_ID = CPI.Customer_ID " +
                    "INNER JOIN Doctor_List AS DL ON CPI.Customer_ID = DL.Customer_ID " +
                    "INNER JOIN Prescription_Item AS PI ON DL.Customer_ID = PI.Customer_ID " +
                    "INNER JOIN Purchase_Orders AS PO ON PI.Customer_ID = PO.Customer_ID " +
                    "WHERE Customer_DateOfBirth = @dti", cnn);
                cmd.Parameters.AddWithValue("@dti", dt);
            }
            else
            {
                cmd = new SqlCommand("SELECT CL.Customer_ID, Customer_FirstName, Customer_LastName, Customer_UserName, " +
                    "Customer_DateOfBirth, Customer_Phone, Customer_Email, Customer_Address, " +
                    "Customer_City, Customer_State, Customer_ZipCode, Customer_DrugAllergy, Customer_Payment_ID, " +
                    "Customer_Credit_Number, Customer_Credit_ExpDate, Customer_Insurance, Doctor_ID, Doctor_FirstName, " +
                    "Doctor_LastName, Doctor_Phone, Doctor_Email, PI.Prescription_ID, Prescription_Name, " +
                    "Prescription_Quantity, Prescription_Refill, PI.Prescription_Price, PO_ID, PO_Date, " +
                    "PO_Total, PO_ShipDate FROM Customers_List AS CL " +
                    "INNER JOIN Customers_Payment_Info AS CPI ON CL.Customer_ID = CPI.Customer_ID " +
                    "INNER JOIN Doctor_List AS DL ON CPI.Customer_ID = DL.Customer_ID " +
                    "INNER JOIN Prescription_Item AS PI ON DL.Customer_ID = PI.Customer_ID " +
                    "INNER JOIN Purchase_Orders AS PO ON PI.Customer_ID = PO.Customer_ID " +
                    "WHERE Customer_FirstName = @ss OR Customer_LastName = @ss", cnn);
                cmd.Parameters.AddWithValue("@ss", searchString);            
            }
            try
            {
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        try
                        {
                            info["ID"] = dataReader[0].ToString();
                            info["fname"] = dataReader[1].ToString();
                            info["lname"] = dataReader[2].ToString();
                            if (dataReader[4].ToString().Length > 20)
                            {
                                info["dob"] = dataReader[4].ToString().Remove(10);
                            }
                            else
                            {
                                info["dob"] = dataReader[4].ToString().Remove(9);
                            }
                            info["phone"] = dataReader[5].ToString();
                            info["email"] = dataReader[6].ToString();
                            info["address"] = dataReader[7].ToString();
                            info["city"] = dataReader[8].ToString();
                            info["state"] = dataReader[9].ToString();
                            info["zip"] = dataReader[10].ToString();
                            info["allergy"] = dataReader[11].ToString();
                            info["payID"] = dataReader[12].ToString();
                            info["cc"] = dataReader[13].ToString();
                            if (dataReader[14].ToString().Length > 20)
                            {
                                info["exp"] = dataReader[14].ToString().Remove(10);
                            }
                            else
                            {
                                info["exp"] = dataReader[14].ToString().Remove(9);
                            }
                            info["doc"] = dataReader[16].ToString();
                            info["docFname"] = dataReader[17].ToString();
                            info["docLname"] = dataReader[18].ToString();
                            info["docPhone"] = dataReader[19].ToString();
                            info["docEmail"] = dataReader[20].ToString();
                            info["insure"] = dataReader[15].ToString();
                        }
                        catch (Exception ex)
                        {
                            Form1.mySystemMessage("Error with query.");
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    dataReader.Close();
                    cnn.Close();
                    cnn.Open();
                    if (dt != "")
                    {
                        cmd = new SqlCommand("SELECT * FROM Customers_List, Customers_Payment_Info " +
                            "WHERE Customer_DateOfBirth = @dti", cnn);
                        cmd.Parameters.AddWithValue("@dti", dt);
                    }
                    else
                    {
                        cmd = new SqlCommand("SELECT * FROM Customers_List, Customers_Payment_Info " +
                            "WHERE Customer_FirstName = @ss OR Customer_LastName = @ss", cnn);
                        cmd.Parameters.AddWithValue("@ss", searchString);
                    }
                    
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        try
                        {
                            info["ID"] = dataReader["Customer_ID"].ToString();
                            info["fname"] = dataReader["Customer_FirstName"].ToString();
                            info["lname"] = dataReader["Customer_LastName"].ToString();
                            if (dataReader["Customer_DateOfBirth"].ToString().Length > 20)
                            {
                                info["dob"] = dataReader["Customer_DateOfBirth"].ToString().Remove(10);
                            }
                            else
                            {
                                info["dob"] = dataReader["Customer_DateOfBirth"].ToString().Remove(9);
                            }
                            info["phone"] = dataReader["Customer_Phone"].ToString();
                            info["email"] = dataReader["Customer_Email"].ToString();
                            info["address"] = dataReader["Customer_Address"].ToString();
                            info["city"] = dataReader["Customer_City"].ToString();
                            info["state"] = dataReader["Customer_State"].ToString();
                            info["zip"] = dataReader["Customer_ZipCode"].ToString();
                            info["allergy"] = dataReader["Customer_DrugAllergy"].ToString();
                            info["payID"] = dataReader["Customer_Payment_ID"].ToString();
                            info["cc"] = dataReader["Customer_Credit_Number"].ToString();
                            if (dataReader["Customer_Credit_ExpDate"].ToString().Length > 20)
                            {
                                info["exp"] = dataReader["Customer_Credit_ExpDate"].ToString().Remove(10);
                            }
                            else
                            {
                                info["exp"] = dataReader["Customer_Credit_ExpDate"].ToString().Remove(9);
                            }
                            info["insure"] = dataReader["Customer_Insurance"].ToString();
                        }
                        catch (Exception ex)
                        {
                            Form1.mySystemMessage("Error with query.");
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.mySystemMessage(ex.Message);
            }
            dbID = info["ID"];
            cnn.Close();
            scripts();
            return info;
        }
        public List<string> scripts()
        {
            List<string> scriptsInfo = new List<string>(5);
            SqlDataReader dataReader;

            SqlConnection cnn = new SqlConnection(connectionstring);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("SELECT Prescription_Name FROM Prescription_Item " +
                "WHERE Customer_ID = @id", cnn);
            cmd.Parameters.AddWithValue("@id", dbID);
            try
            {
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    try
                    {
                        scriptsInfo.Add(dataReader[0].ToString());
                    }
                    catch(Exception ex)
                    {
                        Form1.mySystemMessage("Error with query.");
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                Form1.mySystemMessage(ex.Message);
            }
            cnn.Close();
            while (scriptsInfo.Count() < 5)
            {
                scriptsInfo.Add("");
            }
            return scriptsInfo;
        }
        public List<int> refills()
        {
            List<int> refillInfo = new List<int>(5);
            SqlDataReader dataReader;

            SqlConnection cnn = new SqlConnection(connectionstring);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("SELECT Prescription_Refill FROM Prescription_Item " +
                "WHERE Customer_ID = @id", cnn);
            cmd.Parameters.AddWithValue("@id", dbID);
            try
            {
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    try
                    {
                        refillInfo.Add(Int32.Parse(dataReader[0].ToString()));
                    }
                    catch (Exception ex)
                    {
                        Form1.mySystemMessage("Error with query.");
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.mySystemMessage(ex.Message);
            }
            cnn.Close();
            while (refillInfo.Count() < 5)
            {
                refillInfo.Add(0);
            }
            return refillInfo;
        }
        public List<string> prescribers()
        {
            List<string> prescribersInfo = new List<string>(5);
            SqlDataReader dataReader;

            SqlConnection cnn = new SqlConnection(connectionstring);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("SELECT Doctor_FirstName, Doctor_LastName FROM Doctor_List " +
                "WHERE Customer_ID = @id", cnn);
            cmd.Parameters.AddWithValue("@id", dbID);
            try
            {
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    try
                    {
                        prescribersInfo.Add(dataReader[0].ToString() + " " + dataReader[1].ToString());
                    }
                    catch (Exception ex)
                    {
                        Form1.mySystemMessage("Error with query.");
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.mySystemMessage(ex.Message);
            }
            cnn.Close();
            while (prescribersInfo.Count() < 5)
            {
                prescribersInfo.Add("");
            }
            return prescribersInfo;
        }
        public string recentOrder()
        {
            string recent = "";
            SqlDataReader dataReader;

            SqlConnection cnn = new SqlConnection(connectionstring);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("SELECT TOP(1) PO_Date FROM Purchase_Orders WHERE Customer_ID = @id", cnn);
            cmd.Parameters.AddWithValue("@id", dbID);
            try
            {
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    try
                    {
                        recent = dataReader[0].ToString().Remove(10);
                    }
                    catch (Exception ex)
                    {
                        Form1.mySystemMessage("Error with query.");
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.mySystemMessage(ex.Message);
            }
            cnn.Close();
            return recent;
        }
        public BindingSource reportQuery(string date1, string date2)
        {
            //convert first date to digits
            date1 = date1.Substring(date1.IndexOf(',') + 2);
            if (date1.Contains("January"))
            {
                date1 = date1.Replace("January", "1");
            }
            if (date1.Contains("February"))
            {
                date1 = date1.Replace("February", "2");
            }
            if (date1.Contains("March"))
            {
                date1 = date1.Replace("March", "3");
            }
            if (date1.Contains("April"))
            {
                date1 = date1.Replace("April", "4");
            }
            if (date1.Contains("Mary"))
            {
                date1 = date1.Replace("May", "5");
            }
            if (date1.Contains("June"))
            {
                date1 = date1.Replace("June", "6");
            }
            if (date1.Contains("July"))
            {
                date1 = date1.Replace("July", "7");
            }
            if (date1.Contains("August"))
            {
                date1 = date1.Replace("August", "8");
            }
            if (date1.Contains("September"))
            {
                date1 = date1.Replace("September", "9");
            }
            if (date1.Contains("October"))
            {
                date1 = date1.Replace("October", "10");
            }
            if (date1.Contains("November"))
            {
                date1 = date1.Replace("November", "11");
            }
            if (date1.Contains("December"))
            {
                date1 = date1.Replace("December", "12");
            }
            date1 = date1.Replace(" ", "/");
            date1 = date1.Replace(",", string.Empty);

            //convert second date to digits
            date2 = date2.Substring(date2.IndexOf(',') + 2);
            if (date2.Contains("January"))
            {
                date2 = date2.Replace("January", "1");
            }
            if (date2.Contains("February"))
            {
                date2 = date2.Replace("February", "2");
            }
            if (date2.Contains("March"))
            {
                date2 = date2.Replace("March", "3");
            }
            if (date2.Contains("April"))
            {
                date2 = date2.Replace("April", "4");
            }
            if (date2.Contains("May"))
            {
                date2 = date2.Replace("May", "5");
            }
            if (date2.Contains("June"))
            {
                date2 = date2.Replace("June", "6");
            }
            if (date2.Contains("July"))
            {
                date2 = date2.Replace("July", "7");
            }
            if (date2.Contains("August"))
            {
                date2 = date2.Replace("August", "8");
            }
            if (date2.Contains("September"))
            {
                date2 = date2.Replace("September", "9");
            }
            if (date2.Contains("October"))
            {
                date2 = date2.Replace("October", "10");
            }
            if (date2.Contains("November"))
            {
                date2 = date2.Replace("November", "11");
            }
            if (date2.Contains("December"))
            {
                date2 = date2.Replace("December", "12");
            }
            date2 = date2.Replace(" ", "/");
            date2 = date2.Replace(",", String.Empty);

            BindingSource bindingSource1 = new BindingSource();

            SqlConnection cnn = new SqlConnection(connectionstring);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Purchase_Orders " +
                "WHERE PO_Date BETWEEN @d1 AND @d2 ORDER BY PO_Date DESC", cnn);
            cmd.Parameters.AddWithValue("@d1", date1);
            cmd.Parameters.AddWithValue("@d2", date2);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable
            {
                Locale = CultureInfo.InvariantCulture
            };
            adapter.Fill(table);
            bindingSource1.DataSource = table;

            return bindingSource1;
        }
    }
}
