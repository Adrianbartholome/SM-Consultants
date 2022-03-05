using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace USPS
{
    public partial class Form1 : Form
    {
        DB db;
        public static bool customerPanelSwitch = false;
        public static bool pharmPanelSwitch = false;
        static bool editCheck = false;
        static bool saveCheck = false;
        static string userPass;
        static string userID;
        static string dbID;

        public Form1()
        {
            InitializeComponent();
            //Change title bar name from 'Form1' to 'USPS'
            this.Text = "USPS";
            //hide currently unused elements
            panel2.Visible = false;
            panel3.Visible = false;

            //set form to autosize to currently used elements
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Save.Enabled = false;
            saveChanges.Enabled = false;
        }

        private void formSwitch1()
        {
            if (customerPanelSwitch == true)
            {
                //switch visible panels
                panel1.Visible = false;
                panel2.Visible = true;
                //shift 2nd panel to first panel position
                panel2.Location = new System.Drawing.Point(27, 29);
            }
            else if (pharmPanelSwitch == true)
            {
                panel1.Visible = false;
                panel3.Visible = true;

                panel3.Location = new System.Drawing.Point(27, 29);
            }
            else
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;
            }
        }
        public static void errorBox(string errMessage)
        {
            string err = errMessage;
            MessageBox.Show(err);
        }

        private void user_TextChanged(object sender, EventArgs e)
        {
            userID = user.Text;
        }

        private void pass_TextChanged(object sender, EventArgs e)
        {
            
            userPass = pass.Text;
        }

        private void submit_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost\\SQLExpress";
            builder.InitialCatalog = "USPS_IT488";
            builder.TrustServerCertificate = true;
            builder.Password = userPass;
            builder.UserID = userID;

            db = new DB();

            db.DBLogin(userPass, userID);

            infoFill();

            formSwitch1();
        }

        public void infoFill()
        {
            //populate user query data into dictionary, then use those keys to populate textboxes
            Dictionary<string, string> info = db.infoUpdateQuery();

            if (info != null)
            {
                firstNameTextBox.Text = info["fname"];
                textBox_firstName.Text = info["fname"];
                lastNameTextBox.Text = info["lname"];
                textBox_lastName.Text = info["lname"];
                dateOfBirthTextBox.Text = info["dob"];
                textBox_dateOfBirth.Text = info["dob"];
                //emailTextBox.Text = info["email"];
                textBox_email.Text = info["email"];
                //phoneTextBox.Text = info["phone"];
                textBox_telephone.Text = info["phone"];
                streetAddressTextBox.Text = info["address"];
                textBox_streetAddress.Text = info["address"];
                stateTextBox.Text = info["state"];
                textBox_state.Text = info["state"];
                cityTextBox.Text = info["city"];
                textBox_city.Text = info["city"];
                zipCodeTextBox.Text = info["zip"];
                textBox_zipcode.Text = info["zip"];
                drugAllergyTextBox1.Text = info["allergy"];
                textBox_drugAllergies1.Text = info["allergy"];
                dbID = info["ID"];
            }

            //temporary solution to check for escalated privelege,
            //should change to check for 'admin' user

            if (Int16.Parse(dbID) == 1)
            {
                customerPanelSwitch = true;
            }
            else if (Int16.Parse(dbID) == 2)
            {
                pharmPanelSwitch = true;
            }
        }
        public void searchResults(Dictionary<string, string> info)
        {
            if (info != null)
            {
                firstNameTextBox.Text = info["fname"];
                textBox_firstName.Text = info["fname"];
                lastNameTextBox.Text = info["lname"];
                textBox_lastName.Text = info["lname"];
                dateOfBirthTextBox.Text = info["dob"];
                textBox_dateOfBirth.Text = info["dob"];
                //emailTextBox.Text = info["email"];
                textBox_email.Text = info["email"];
                //phoneTextBox.Text = info["phone"];
                textBox_telephone.Text = info["phone"];
                streetAddressTextBox.Text = info["address"];
                textBox_streetAddress.Text = info["address"];
                stateTextBox.Text = info["state"];
                textBox_state.Text = info["state"];
                cityTextBox.Text = info["city"];
                textBox_city.Text = info["city"];
                zipCodeTextBox.Text = info["zip"];
                textBox_zipcode.Text = info["zip"];
                drugAllergyTextBox1.Text = info["allergy"];
                textBox_drugAllergies1.Text = info["allergy"];
                dbID = info["ID"];
            }
        }

        public void infoUpdate()
        {
            //update dictionary values with edited data from textboxes and send to db for updating database

            //user side
            if (panel2.Visible == true)
            {
                Dictionary<string, string> info = new Dictionary<string, string>();
                info.Add("fname", firstNameTextBox.Text);
                info.Add("lname", lastNameTextBox.Text);
                info.Add("dob", dateOfBirthTextBox.Text);
                //info.Add("phone", phoneTextBox.Text);
                //info.Add("email", emailTextBox.Text);
                info.Add("address", streetAddressTextBox.Text);
                info.Add("city", cityTextBox.Text);
                info.Add("state", stateTextBox.Text);
                info.Add("zip", zipCodeTextBox.Text);
                info.Add("allergy", drugAllergyTextBox1.Text);

                db.infoUpdater(info);
            }
            //pharm side
            else
            {
                Dictionary<string, string> info = new Dictionary<string, string>();
                info.Add("fname", textBox_firstName.Text);
                info.Add("lname", textBox_lastName.Text);
                info.Add("dob", textBox_dateOfBirth.Text);
                info.Add("phone", textBox_telephone.Text);
                info.Add("email", textBox_email.Text);
                info.Add("address", textBox_streetAddress.Text);
                info.Add("city", textBox_city.Text);
                info.Add("state", textBox_state.Text);
                info.Add("zip", textBox_zipcode.Text);
                info.Add("allergy", textBox_drugAllergies1.Text);

                db.infoUpdater(info);
            }

            
        }
        private void requestRefill_Click(object sender, EventArgs e)
        {
            //temporary query for testing
            string names = db.getCustomerNames();
            MessageBox.Show(names);
        }

        private void edit_Click(object sender, EventArgs e)
        {
            //user side edit checks to enable changes
            if (!editCheck)
            {
                DialogResult dR = MessageBox.Show("Edit your information?", "System Message", MessageBoxButtons.OKCancel);
                if (dR == DialogResult.OK)
                {
                    editCheck = true;
                    saveCheck = true;
                    edit.Enabled = false;
                    Edit_onPharmForm.Enabled = false;
                    Save.Enabled = true;
                    saveChanges.Enabled = true;

                    firstNameTextBox.ReadOnly = false;
                    lastNameTextBox.ReadOnly = false;
                    dateOfBirthTextBox.ReadOnly = false;
                    streetAddressTextBox.ReadOnly = false;
                    cityTextBox.ReadOnly = false;
                    stateTextBox.ReadOnly = false;
                    zipCodeTextBox.ReadOnly = false;
                    insuranceTextBox.ReadOnly = false;
                    paymentMethodTextBox.ReadOnly = false;
                    drugAllergyTextBox1.ReadOnly = false;
                    drugAllergyTextBox2.ReadOnly = false;
                    drugAllergyTextBox3.ReadOnly = false;
                }
                else
                {
                    editCheck = false;
                    saveCheck = false;
                }
            }
            
        }

        private void Save_Click(object sender, EventArgs e)
        {
            //user side save checks to verify intent to save and then disable textboxes again
            if (saveCheck)
            {
                DialogResult dR = MessageBox.Show("Save your information?", "System Message", MessageBoxButtons.OKCancel);
                if (dR == DialogResult.OK)
                {
                    infoUpdate();
                    saveCheck = false;
                    Save.Enabled = false;
                    saveChanges.Enabled = false;

                    firstNameTextBox.ReadOnly = true;
                    lastNameTextBox.ReadOnly = true;
                    dateOfBirthTextBox.ReadOnly = true;
                    streetAddressTextBox.ReadOnly = true;
                    cityTextBox.ReadOnly = true;
                    stateTextBox.ReadOnly = true;
                    zipCodeTextBox.ReadOnly = true;
                    insuranceTextBox.ReadOnly = true;
                    paymentMethodTextBox.ReadOnly = true;
                    drugAllergyTextBox1.ReadOnly = true;
                    drugAllergyTextBox2.ReadOnly = true;
                    drugAllergyTextBox3.ReadOnly = true;

                    edit.Enabled = true;
                    Edit_onPharmForm.Enabled = true;
                    editCheck = false;
                }
            }
            
        }
        
        //logout and return to sign in screen
        private void logOutButton_Click(object sender, EventArgs e)
        {
            DialogResult dR = MessageBox.Show("Log Out?", "System Message", MessageBoxButtons.OKCancel);
            if (dR == DialogResult.OK)
            {
                db.logOut();
                formSwitch1();
            }
        }

        private void Edit_onPharmForm_Click(object sender, EventArgs e)
        {
            //pharm side edit checks
            if (!editCheck)
            {
                DialogResult dR = MessageBox.Show("Edit your information?", "System Message", MessageBoxButtons.OKCancel);
                if (dR == DialogResult.OK)
                {
                    editCheck = true;
                    saveCheck = true;
                    Edit_onPharmForm.Enabled = false;
                    saveChanges.Enabled = true;

                    textBox_firstName.ReadOnly = false;
                    textBox_lastName.ReadOnly = false;
                    textBox_dateOfBirth.ReadOnly = false;
                    textBox_telephone.ReadOnly = false;
                    textBox_email.ReadOnly = false;
                    textBox_streetAddress.ReadOnly = false;
                    textBox_state.ReadOnly = false;
                    textBox_city.ReadOnly = false;
                    textBox_zipcode.ReadOnly = false;
                    textBox_insurance.ReadOnly = false;
                    textBox_paymentMethod.ReadOnly = false;
                    textBox_drugAllergies1.ReadOnly = false;
                    textBox_drugAllergies2.ReadOnly = false;
                    textBox_drugAllergies3.ReadOnly = false;
                }
                else
                {
                    editCheck = false;
                    saveCheck = false;
                }
            }
        }

        private void saveChanges_Click(object sender, EventArgs e)
        {
            //pharm side save checks
            if (saveCheck)
            {
                DialogResult dR = MessageBox.Show("Save your information?", "System Message", MessageBoxButtons.OKCancel);
                if (dR == DialogResult.OK)
                {
                    infoUpdate();
                    saveCheck = false;
                    saveChanges.Enabled = false;

                    textBox_firstName.ReadOnly = true;
                    textBox_lastName.ReadOnly = true;
                    textBox_dateOfBirth.ReadOnly = true;
                    textBox_telephone.ReadOnly = true;
                    textBox_email.ReadOnly = true;
                    textBox_streetAddress.ReadOnly = true;
                    textBox_state.ReadOnly = true;
                    textBox_city.ReadOnly = true;
                    textBox_zipcode.ReadOnly = true;
                    textBox_insurance.ReadOnly = true;
                    textBox_paymentMethod.ReadOnly = true;
                    textBox_drugAllergies1.ReadOnly = true;
                    textBox_drugAllergies2.ReadOnly = true;
                    textBox_drugAllergies3.ReadOnly = true;

                    Edit_onPharmForm.Enabled = true;
                    editCheck = false;
                }
            }
        }
        //trigger event when enter key pressed in search field
        private void SearchKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Do something
                Dictionary<string, string> info = db.searchCustomers(textBox_searchName.Text);
                searchResults(info);
                e.Handled = true;
            }
        }

        private void textBox_searchName_TextChanged(object sender, EventArgs e)
        {
            textBox_searchName.KeyUp += SearchKeyUp;
        }

        // ---- textbox validation section -----

        // Login page validation
        private void user_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(user.Text.Trim()))
            {
                errorBox("Fields cannot be left blank.");
            }
        }

        private void pass_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(pass.Text.Trim()))
            {
                errorBox("Fields cannot be left blank.");
            }
        }

        // panel 1 validation
        private void firstNameTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(firstNameTextBox.Text.Trim()))
            {
                e.Cancel = true;
                errorBox("Fields cannot be left blank.");
            }
            if (firstNameTextBox.Text.Trim().isAlpha())
            {
                e.Cancel = true;
                errorBox("Only letters allowed in name fields.");
            }
        }

        private void lastNameTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(lastNameTextBox.Text.Trim()))
            {
                e.Cancel = true;
                errorBox("Fields cannot be left blank.");
            }
            if (lastNameTextBox.Text.Trim().isAlpha())
            {
                e.Cancel = true;
                errorBox("Only letters allowed in name fields.");
            }
        }

        private void dateOfBirthTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(dateOfBirthTextBox.Text.Trim()))
            {
                e.Cancel = true;
                errorBox("Fields cannot be left blank.");
            }
            if (!ValidationFunctions.isDOB(dateOfBirthTextBox.Text))
            {
                e.Cancel = true;
                errorBox("DOB must be in ##-##-#### format.");
            }
        }

        private void streetAddressTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(streetAddressTextBox.Text.Trim()))
            {
                e.Cancel = true;
                errorBox("Fields cannot be left blank.");
            }
            if (streetAddressTextBox.Text.Trim().isAlphaNum())
            {
                e.Cancel = true;
                errorBox("Only letters and numbers allowed in address field.");
            }
        }

        private void cityTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(cityTextBox.Text.Trim()))
            {
                errorBox("Fields cannot be left blank.");
            }
        }

        private void stateTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(stateTextBox.Text.Trim()))
            {
                errorBox("Fields cannot be left blank.");
            }
        }

        private void zipCodeTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(zipCodeTextBox.Text.Trim()))
            {
                errorBox("Fields cannot be left blank.");
            }
        }

        private void insuranceTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(insuranceTextBox.Text.Trim()))
            {
                errorBox("Fields cannot be left blank.");
            }
        }

        private void paymentMethodTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(paymentMethodTextBox.Text.Trim()))
            {
                errorBox("Fields cannot be left blank.");
            }
        }

        private void drugAllergyTextBox1_Validating(object sender, CancelEventArgs e)
        {
            /*
            if (String.IsNullOrEmpty(drugAllergyTextBox1.Text.Trim()))
            {
                errorBox("Fields cannot be left blank.");
            }
            */
        }

        //panel 2 validation

        //TODO

        //Validation functions
        

        // unused stuff. Will delete if all remains unused.

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void label1_Click_3(object sender, EventArgs e)
        {

        }

        private void label1_Click_4(object sender, EventArgs e)
        {

        }

        private void label1_Click_5(object sender, EventArgs e)
        {

        }

        private void firstNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void lastNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void password_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void cityTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label9_Click_1(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox30_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public static partial class ValidationFunctions
    {
        public static bool isAlpha(this string @this)
        {
            return Regex.IsMatch(@this, "[^a-zA-Z]");
        }
        public static bool isNum(this string @this)
        {
            return Regex.IsMatch(@this, "[^0-9]");
        }
        public static bool isAlphaNum(this string @this)
        {
            return Regex.IsMatch(@this, @"[^a-zA-Z0-9\s,.]");
        }
        public static bool isDOB(string dt)
        {
            //compares textbox to DateTime format with '/' as the separator to verify that it is valid
            try
            {
                string[] dateSplit = dt.Split('/');
                DateTime td = new DateTime(Convert.ToInt32(dateSplit[2]), 
                    Convert.ToInt32(dateSplit[0]), Convert.ToInt32(dateSplit[1]));

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
