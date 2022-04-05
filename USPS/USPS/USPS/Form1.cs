using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public static bool adminPanelSwitch = false;
        public static bool newUserCheck = false;
        public static bool noAdditionalData = false;
        static bool editCheck = false;
        static bool saveCheck = false;
        static string userPass;
        static string userID;
        public bool refillCheck = false;
        public static bool loginFailed = false;

        public Form1()
        {
            InitializeComponent();
            //Change title bar name from 'Form1' to 'USPS'
            this.Text = "USPS";

            //hide currently unused elements
            userPanel.Visible = pharmPanel.Visible = adminPanel.Visible = refillPanel.Visible = false;

            //set form to autosize to currently used elements
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saveUser.Enabled = savePharm.Enabled = saveAdmin.Enabled = false;
        }

        private void formSwitch1()
        {
            if (customerPanelSwitch == true)
            {
                //switch visible panels
                loginPanel.Visible = false;
                //shift 2nd panel to first panel position
                userPanel.Location = new System.Drawing.Point(27, 29);
                userPanel.Visible = true;
                if (newUserCheck)
                {
                    editUser.PerformClick();
                }
            }
            else if (adminPanelSwitch == true)
            {
                loginPanel.Visible = false;
                adminPanel.Location = new System.Drawing.Point(23, 29);
                adminPanel.Visible = true;
            }
            else if (pharmPanelSwitch == true)
            {
                loginPanel.Visible = false;
                pharmPanel.Location = new System.Drawing.Point(27, 29);
                pharmPanel.Visible = true;
            }
            else
            {
                loginPanel.Visible = true;
                userPanel.Visible = pharmPanel.Visible = adminPanel.Visible = false;
                newUser.Checked = false;
            }
        }
        public static void mySystemMessage(string systemMessage)
        {
            string sysMess = systemMessage;
            MessageBox.Show(sysMess);
        }
        private void newUser_CheckedChanged(object sender, EventArgs e)
        {
            if (newUser.Checked == true)
            {
                newUserCheck = true;
            }
        }
        private void submit_Click(object sender, EventArgs e)
        {
            userID = user.Text;
            userPass = pass.Text;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost\\SQLExpress";
            builder.InitialCatalog = "IT488_USPS";
            builder.TrustServerCertificate = true;
            builder.Password = userPass;
            builder.UserID = userID;

            db = new DB();
            
            db.DBLogin(userPass, userID);
            if (!loginFailed)
            {
                infoFill();
            }
        }
        public void infoFill()
        {
            //populate user query data into dictionary, then use those keys to populate textboxes
            Dictionary<string, string> info = db.infoUpdateQuery();
            List<string> scriptInfo = new List<string>();
            List<int> refillInfo = new List<int>();
            List<string> prescribersInfo = new List<string>();
            string recent = "";
            if (noAdditionalData == false)
            {
                scriptInfo = db.scripts();
                refillInfo = db.refills();
                prescribersInfo = db.prescribers();
                recent = db.recentOrder();
            }
            
            if (info != null)
            {
                fnameUser.Text = fnamePharm.Text = fnameAdmin.Text = info["fname"];
                lnameUser.Text = lnamePharm.Text = lnameAdmin.Text = info["lname"];
                dobUser.Text = dobPharm.Text = dobAdmin.Text = info["dob"];
                emailUser.Text = emailPharm.Text = emailAdmin.Text = info["email"];
                telephoneUser.Text = telephonePharm.Text = telephoneAdmin.Text = info["phone"];
                streetUser.Text = streetPharm.Text = streetAdmin.Text = info["address"];
                stateUser.Text = statePharm.Text = stateAdmin.Text = info["state"];
                cityUser.Text = cityPharm.Text = cityAdmin.Text = info["city"];
                zipUser.Text = zipPharm.Text =zipAdmin.Text = info["zip"];
                allergiesUser.Text = allergiesPharm.Text = allergiesAdmin.Text = info["allergy"];
                ccUser.Text = ccPharm.Text = ccAdmin.Text = info["cc"];
                expUser.Text = expPharm.Text = expAdmin.Text = info["exp"];
                insuranceUser.Text = insurancePharm.Text = insuranceAdmin.Text = info["insure"];
                if (info["docFname"] != "")
                {
                    drFname.Text = info["docFname"];
                    drLname.Text = info["docLname"];
                    drPhone.Text = info["docPhone"];
                    drEmail.Text = info["docEmail"];
                }
                if (scriptInfo.Count > 0)
                {
                    prescript1Refill.Text = prescript1Pharm.Text = prescript1Admin.Text = scriptInfo[0];
                    prescript2Refill.Text = prescript2Pharm.Text = prescript2Admin.Text = scriptInfo[1];
                    prescript3Refill.Text = prescript3Pharm.Text = prescript3Admin.Text = scriptInfo[2];
                    prescript4Refill.Text = prescript4Pharm.Text = prescript4Admin.Text = scriptInfo[3];
                    prescript5Refill.Text = prescript5Pharm.Text = prescript5Admin.Text = scriptInfo[4];
                }
                if (refillInfo.Count > 0)
                {
                    refill1Refill.Text = refill1Pharm.Text = refill1Admin.Text = refillInfo[0].ToString();
                    refill2Refill.Text = refill2Pharm.Text = refill2Admin.Text = refillInfo[1].ToString();
                    refill3Refill.Text = refill3Pharm.Text = refill3Admin.Text = refillInfo[2].ToString();
                    refill4Refill.Text = refill4Pharm.Text = refill4Admin.Text = refillInfo[3].ToString();
                    refill5Refill.Text = refill5Pharm.Text = refill5Admin.Text = refillInfo[4].ToString();
                }
                if (prescribersInfo.Count > 0)
                {
                    prescriber1Admin.Text = prescriber1Pharm.Text = prescribersInfo[0];
                    prescriber2Admin.Text = prescriber2Pharm.Text = prescribersInfo[1];
                    prescriber3Admin.Text = prescriber3Pharm.Text = prescribersInfo[2];
                    prescriber4Admin.Text = prescriber4Pharm.Text = prescribersInfo[3];
                    prescriber5Admin.Text = prescriber5Pharm.Text = prescribersInfo[4];
                }
                if (recent != "")
                {
                    shippedPharm.Text = shippedAdmin.Text = recent;
                }

            }
            if (!loginFailed)
            {
                if (userID == "IT488_Admin")
                {
                    adminPanelSwitch = true;
                }
                else if (userID == "Pharmacist")
                {
                    pharmPanelSwitch = true;
                }
                else if (info != null)
                {
                    customerPanelSwitch = true;
                }
                formSwitch1();
            }
        }
        public void searchResults(Dictionary<string, string> info, List<string> scriptInfo, 
            List<int> refillInfo, List<string> prescribersInfo, string recent)
        {
            if (info != null)
            {
                fnameUser.Text = fnamePharm.Text = fnameAdmin.Text = info["fname"];
                lnameUser.Text = lnamePharm.Text = lnameAdmin.Text = info["lname"];
                dobUser.Text = dobPharm.Text = dobAdmin.Text = info["dob"];
                emailUser.Text = emailPharm.Text = emailAdmin.Text = info["email"];
                telephoneUser.Text = telephonePharm.Text = telephoneAdmin.Text = info["phone"];
                streetUser.Text = streetPharm.Text = streetAdmin.Text = info["address"];
                stateUser.Text = statePharm.Text = stateAdmin.Text = info["state"];
                cityUser.Text = cityPharm.Text = cityAdmin.Text = info["city"];
                zipUser.Text = zipPharm.Text = zipAdmin.Text = info["zip"];
                allergiesUser.Text = allergiesPharm.Text = allergiesAdmin.Text = info["allergy"];
                ccUser.Text = ccPharm.Text = ccAdmin.Text = info["cc"];
                expUser.Text = expPharm.Text = expAdmin.Text = info["exp"];
                insuranceUser.Text = insurancePharm.Text = insuranceAdmin.Text = info["insure"];
                drFname.Text = info["docFname"];
                drLname.Text = info["docLname"];
                drPhone.Text = info["docPhone"];
                drEmail.Text = info["docEmail"];
                if (scriptInfo.Count > 0)
                {
                    prescript1Refill.Text = prescript1Pharm.Text = prescript1Admin.Text = scriptInfo[0];
                    prescript2Refill.Text = prescript2Pharm.Text = prescript2Admin.Text = scriptInfo[1];
                    prescript3Refill.Text = prescript3Pharm.Text = prescript3Admin.Text = scriptInfo[2];
                    prescript4Refill.Text = prescript4Pharm.Text = prescript4Admin.Text = scriptInfo[3];
                    prescript5Refill.Text = prescript5Pharm.Text = prescript5Admin.Text = scriptInfo[4];
                }
                if (refillInfo.Count > 0)
                {
                    refill1Refill.Text = refill1Pharm.Text = refill1Admin.Text = refillInfo[0].ToString();
                    refill2Refill.Text = refill2Pharm.Text = refill2Admin.Text = refillInfo[1].ToString();
                    refill3Refill.Text = refill3Pharm.Text = refill3Admin.Text = refillInfo[2].ToString();
                    refill4Refill.Text = refill4Pharm.Text = refill4Admin.Text = refillInfo[3].ToString();
                    refill5Refill.Text = refill5Pharm.Text = refill5Admin.Text = refillInfo[4].ToString();
                }
                if (prescribersInfo.Count > 0)
                {
                    prescriber1Admin.Text = prescriber1Pharm.Text = prescribersInfo[0];
                    prescriber2Admin.Text = prescriber2Pharm.Text = prescribersInfo[1];
                    prescriber3Admin.Text = prescriber3Pharm.Text = prescribersInfo[2];
                    prescriber4Admin.Text = prescriber4Pharm.Text = prescribersInfo[3];
                    prescriber5Admin.Text = prescriber5Pharm.Text = prescribersInfo[4];
                }
                if (recent != "")
                {
                    shippedPharm.Text = shippedAdmin.Text = recent;
                }
            }
        }
        public void infoUpdate()
        {
            //update dictionary values with edited data from textboxes and send to db for updating database

            //user side
            if (userPanel.Visible == true)
            {
                Dictionary<string, string> info = new Dictionary<string, string>();
                info.Add("fname", fnameUser.Text);
                info.Add("lname", lnameUser.Text);
                info.Add("dob", dobUser.Text);
                info.Add("phone", telephoneUser.Text);
                info.Add("email", emailUser.Text);
                info.Add("address", streetUser.Text);
                info.Add("city", cityUser.Text);
                info.Add("state", stateUser.Text);
                info.Add("zip", zipUser.Text);
                info.Add("allergy", allergiesUser.Text);
                info.Add("cc", ccUser.Text);
                info.Add("exp", expUser.Text);
                info.Add("docFname", drFname.Text);
                info.Add("docLname", drLname.Text);
                info.Add("docPhone", drPhone.Text);
                info.Add("docEmail", drEmail.Text);
                info.Add("insure", insuranceUser.Text);

                db.infoUpdater(info);
            }
            //pharm side
            else if (pharmPanel.Visible == true)
            {
                Dictionary<string, string> info = new Dictionary<string, string>();
                info.Add("fname", fnamePharm.Text);
                info.Add("lname", lnamePharm.Text);
                info.Add("dob", dobPharm.Text);
                info.Add("phone", telephonePharm.Text);
                info.Add("email", emailPharm.Text);
                info.Add("address", streetPharm.Text);
                info.Add("city", cityPharm.Text);
                info.Add("state", statePharm.Text);
                info.Add("zip", zipPharm.Text);
                info.Add("allergy", allergiesPharm.Text);
                info.Add("cc", ccPharm.Text);
                info.Add("exp", expPharm.Text);
                info.Add("docFname", drFname.Text);
                info.Add("docLname", drLname.Text);
                info.Add("docPhone", drPhone.Text);
                info.Add("docEmail", drEmail.Text);
                info.Add("insure", insurancePharm.Text);

                db.infoUpdater(info);
            }
            //admin side
            else if (adminPanel.Visible == true)
            {
                Dictionary<string, string> info = new Dictionary<string, string>();
                info.Add("fname", fnameAdmin.Text);
                info.Add("lname", lnameAdmin.Text);
                info.Add("dob", dobAdmin.Text);
                info.Add("phone", telephoneAdmin.Text);
                info.Add("email", emailAdmin.Text);
                info.Add("address", streetAdmin.Text);
                info.Add("city", cityAdmin.Text);
                info.Add("state", stateAdmin.Text);
                info.Add("zip", zipAdmin.Text);
                info.Add("allergy", allergiesAdmin.Text);
                info.Add("cc", ccAdmin.Text);
                info.Add("exp", expAdmin.Text);
                info.Add("docFname", drFname.Text);
                info.Add("docLname", drLname.Text);
                info.Add("docPhone", drPhone.Text);
                info.Add("docEmail", drEmail.Text);
                info.Add("insure", insuranceAdmin.Text);

                db.infoUpdater(info);
            }
        }

        //user side edits and saves
        private void editUser_Click(object sender, EventArgs e)
        {
            //user side edit checks to enable changes
            if (!editCheck)
            {
                DialogResult dR = MessageBox.Show("Continue to edit user information.", "System Message", MessageBoxButtons.OKCancel);
                if (dR == DialogResult.OK)
                {
                    editCheck = saveCheck = true;
                    editUser.Enabled = false;
                    saveUser.Enabled = true;

                    fnameUser.ReadOnly = false;
                    lnameUser.ReadOnly = false;
                    dobUser.ReadOnly = false;
                    streetUser.ReadOnly = false;
                    cityUser.ReadOnly = false;
                    stateUser.ReadOnly = false;
                    zipUser.ReadOnly = false;
                    insuranceUser.ReadOnly = false;
                    ccUser.ReadOnly = false;
                    allergiesUser.ReadOnly = false;
                    telephoneUser.ReadOnly = false;
                    emailUser.ReadOnly = false;
                }
                else
                {
                    editCheck = false;
                    saveCheck = false;
                }
            }
        }
        private void saveUser_Click(object sender, EventArgs e)
        {
            //user side save checks to verify intent to save and then disable textboxes again
            if (saveCheck)
            {
                DialogResult dR = MessageBox.Show("Save user information?", "System Message", MessageBoxButtons.OKCancel);
                if (dR == DialogResult.OK)
                {
                    infoUpdate();
                    saveCheck = false;
                    saveUser.Enabled = false;
                    savePharm.Enabled = false;

                    fnameUser.ReadOnly = true;
                    lnameUser.ReadOnly = true;
                    dobUser.ReadOnly = true;
                    streetUser.ReadOnly = true;
                    cityUser.ReadOnly = true;
                    stateUser.ReadOnly = true;
                    zipUser.ReadOnly = true;
                    insuranceUser.ReadOnly = true;
                    ccUser.ReadOnly = true;
                    allergiesUser.ReadOnly = true;
                    telephoneUser.ReadOnly = true;
                    emailUser.ReadOnly = true;

                    editUser.Enabled = true;
                    editPharm.Enabled = true;
                    editCheck = false;
                }
            }
        }
        //pharm side edits and saves
        private void editPharm_Click(object sender, EventArgs e)
        {
            //pharm side edit checks
            if (!editCheck)
            {
                DialogResult dR = MessageBox.Show("Edit user information?", "System Message", MessageBoxButtons.OKCancel);
                if (dR == DialogResult.OK)
                {
                    editCheck = true;
                    saveCheck = true;
                    editPharm.Enabled = false;
                    searchPharm.Enabled = false;
                    savePharm.Enabled = true;

                    fnamePharm.ReadOnly = false;
                    lnamePharm.ReadOnly = false;
                    dobPharm.ReadOnly = false;
                    telephonePharm.ReadOnly = false;
                    emailPharm.ReadOnly = false;
                    streetPharm.ReadOnly = false;
                    statePharm.ReadOnly = false;
                    cityPharm.ReadOnly = false;
                    zipPharm.ReadOnly = false;
                    insurancePharm.ReadOnly = false;
                    ccPharm.ReadOnly = false;
                    allergiesPharm.ReadOnly = false;
                }
                else
                {
                    editCheck = false;
                    saveCheck = false;
                }
            }
        }
        private void savePharm_Click(object sender, EventArgs e)
        {
            //pharm side save checks
            if (saveCheck)
            {
                DialogResult dR = MessageBox.Show("Save user information?", "System Message", MessageBoxButtons.OKCancel);
                if (dR == DialogResult.OK)
                {
                    infoUpdate();
                    saveCheck = false;
                    savePharm.Enabled = false;
                    searchPharm.Enabled = true;

                    fnamePharm.ReadOnly = true;
                    lnamePharm.ReadOnly = true;
                    dobPharm.ReadOnly = true;
                    telephonePharm.ReadOnly = true;
                    emailPharm.ReadOnly = true;
                    streetPharm.ReadOnly = true;
                    statePharm.ReadOnly = true;
                    cityPharm.ReadOnly = true;
                    zipPharm.ReadOnly = true;
                    insurancePharm.ReadOnly = true;
                    ccPharm.ReadOnly = true;
                    allergiesPharm.ReadOnly = true;

                    editPharm.Enabled = true;
                    editCheck = false;
                }
            }
        }
        //trigger event when enter key pressed in search field
        private void SearchKeyUp_Pharm(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Do something
                Dictionary<string, string> info = db.searchCustomers(searchPharm.Text);
                List<string> scriptInfo = db.scripts();
                List<int> refillInfo = db.refills();
                List<string> prescribersInfo = db.prescribers(); 
                string recent = db.recentOrder();
                searchResults(info, scriptInfo, refillInfo, prescribersInfo, recent);
                e.Handled = true;
            }
        }
        private void searchPharm_TextChanged(object sender, EventArgs e)
        {
            searchPharm.KeyUp += SearchKeyUp_Pharm;
        }
        //admin side edits and saves
        private void editAdmin_Click(object sender, EventArgs e)
        {
            //admin side edit checks
            if (!editCheck)
            {
                DialogResult dR = MessageBox.Show("Edit user information?", "System Message", MessageBoxButtons.OKCancel);
                if (dR == DialogResult.OK)
                {
                    editCheck = true;
                    saveCheck = true;
                    editAdmin.Enabled = false;
                    searchAdmin.Enabled = false;
                    saveAdmin.Enabled = true;

                    fnameAdmin.ReadOnly = false;
                    lnameAdmin.ReadOnly = false;
                    dobAdmin.ReadOnly = false;
                    telephoneAdmin.ReadOnly = false;
                    emailAdmin.ReadOnly = false;
                    streetAdmin.ReadOnly = false;
                    stateAdmin.ReadOnly = false;
                    cityAdmin.ReadOnly = false;
                    zipAdmin.ReadOnly = false;
                    insuranceAdmin.ReadOnly = false;
                    ccAdmin.ReadOnly = false;
                    allergiesAdmin.ReadOnly = false;
                }
                else
                {
                    editCheck = false;
                    saveCheck = false;
                }
            }
        }
        private void saveAdmin_Click(object sender, EventArgs e)
        {
            //admin side save checks
            if (saveCheck)
            {
                DialogResult dR = MessageBox.Show("Save user information?", "System Message", MessageBoxButtons.OKCancel);
                if (dR == DialogResult.OK)
                {
                    infoUpdate();
                    saveCheck = false;
                    saveAdmin.Enabled = false;
                    searchAdmin.Enabled = true;

                    fnameAdmin.ReadOnly = true;
                    lnameAdmin.ReadOnly = true;
                    dobAdmin.ReadOnly = true;
                    telephoneAdmin.ReadOnly = true;
                    emailAdmin.ReadOnly = true;
                    streetAdmin.ReadOnly = true;
                    stateAdmin.ReadOnly = true;
                    cityAdmin.ReadOnly = true;
                    zipAdmin.ReadOnly = true;
                    insuranceAdmin.ReadOnly = true;
                    ccAdmin.ReadOnly = true;
                    allergiesAdmin.ReadOnly = true;

                    editAdmin.Enabled = true;
                    editCheck = false;
                }
            }
        }
        private void SearchKeyUp_Admin(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Do something
                Dictionary<string, string> info = db.searchCustomers(searchAdmin.Text);
                List<string> scriptInfo = db.scripts();
                List<int> refillInfo = db.refills();
                List<string> prescriberInfo = db.prescribers();
                string recent = db.recentOrder();
                searchResults(info, scriptInfo, refillInfo, prescriberInfo, recent);
                e.Handled = true;
            }
        }
        private void searchAdmin_TextChanged(object sender, EventArgs e)
        {
            searchAdmin.KeyUp += SearchKeyUp_Admin;
        }
        private void report_btn_Click(object sender, EventArgs e)
        {
            //open and close the dataGrid, send report query
            if (dataGridView1.Visible == false)
            {
                report_btn.Text = "Close Report";
                dataGridView1.Location = new System.Drawing.Point(450, 29);
                
                if (dailyFill_chkbox.Checked)
                {
                    dataGridView1.DataSource = db.dailyFillQuery();
                    dataGridView1.Visible = true;
                }
                else
                {
                    if (orderStatus.Text == "Shipped")
                    {
                        dataGridView1.DataSource = db.orderStatusShippedQuery(dateRange1.Text, dateRange2.Text);
                        dataGridView1.Visible = true;

                    }
                    else if (orderStatus.Text == "Pending")
                    {
                        dataGridView1.DataSource = db.orderStatusPendingQuery(dateRange1.Text, dateRange2.Text);
                        dataGridView1.Visible = true;
                    }
                    else
                    {
                        mySystemMessage("Please select 'Shipped' or 'Pending' orders from the drop down.");
                    }
                }
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            else
            {
                dataGridView1.Visible = false;
                report_btn.Text = "Request Report";
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
                if (refillPanel.Visible)
                {
                    refillPanel.Visible = false;
                }
                if (prescriberPanel.Visible)
                {
                    prescriberPanel.Visible = false;
                }
            }
        }
        private void prescriberName_DoubleClick(object sender, EventArgs e)
        {
            prescriberPanel.Visible = true;
            prescriberPanel.Location = new System.Drawing.Point(450, 29);
            if (dataGridView1.Visible)
            {
                dataGridView1.Visible = false;
            }
        }
        private void prescriberCloseClick(object sender, EventArgs e)
        {
            prescriberPanel.Visible = false;
        }
        //refil panel stuff
        private void requestRefill_Click(object sender, EventArgs e)
        {
            refillPanel.Location = new System.Drawing.Point(450, 29);
            refillPanel.Size = new System.Drawing.Size(287, 315);
            refillPanel.Visible = true;
        }
        private void backRefill_Click(object sender, EventArgs e)
        {
            refillPanel.Visible = false;
        }
        //refill checkbox area -- customer
        private void submitRefill_Click(object sender, EventArgs e)
        {
            string orders = "";

            if ((checkBox1Refill.Checked) && (refill1Refill.Text != "0"))
            {
                orders += prescript1Refill.Text + "\n";
            }
            if ((checkBox2Refill.Checked) && (refill2Refill.Text != "0"))
            {
                orders += prescript2Refill.Text + "\n";
            }
            if ((checkBox3Refill.Checked) && (refill3Refill.Text != "0"))
            {
                orders += prescript3Refill.Text + "\n";
            }
            if ((checkBox4Refill.Checked) && (refill4Refill.Text != "0"))
            {
                orders += prescript4Refill.Text + "\n";
            }
            if ((checkBox5Refill.Checked) && (refill5Refill.Text != "0"))
            {
                orders += prescript5Refill.Text + "\n";
            }
            
            string orderConf = $"Prescriptions:\n\n" + orders + "__________\n\n" + "Shipped to:\n\n" 
                + fnameUser.Text + " " + lnameUser.Text +"\n" + streetUser.Text + "\n" + cityUser.Text 
                + ", " + stateUser.Text + " " + zipUser.Text;

            DialogResult dR = MessageBox.Show(orderConf, "Confirm your order", MessageBoxButtons.OKCancel);
            if (dR == DialogResult.OK)
            {
                mySystemMessage("Your order has been placed!");
            }
        }
        private void checkBox1Refill_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1Refill.Checked == true)
            {
                if (refill1Refill.Text == "0")
                {
                    mySystemMessage($"You have no available refills of {prescript1Refill.Text}. " +
                        $"Please unselect this item.");
                }
                else
                {
                    submitRefill.Enabled = true;
                }
            }
            else if ((checkBox5Refill.Checked == true) || (checkBox4Refill.Checked == true) ||
                (checkBox3Refill.Checked == true) || (checkBox2Refill.Checked == true) || (checkBox1Refill.Checked == true))
            {
                submitRefill.Enabled = true;
            }
            else
            {
                submitRefill.Enabled = false;
            }
        }
        private void checkBox2Refill_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2Refill.Checked == true)
            {
                if (refill2Refill.Text == "0")
                {
                    mySystemMessage($"You have no available refills of {prescript2Refill.Text}. " +
                        $"Please unselect this item.");
                }
                else
                {
                    submitRefill.Enabled = true;
                }
            }
            else if ((checkBox5Refill.Checked == true) || (checkBox4Refill.Checked == true) ||
                (checkBox3Refill.Checked == true) || (checkBox2Refill.Checked == true) || (checkBox1Refill.Checked == true))
            {
                submitRefill.Enabled = true;
            }
            else
            {
                submitRefill.Enabled = false;
            }
        }
        private void checkBox3Refill_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3Refill.Checked == true)
            {
                if (refill3Refill.Text == "0")
                {
                    mySystemMessage($"You have no available refills of {prescript3Refill.Text}. " +
                        $"Please unselect this item.");
                }
                else
                {
                    submitRefill.Enabled = true;
                }
            }
            else if ((checkBox5Refill.Checked == true) || (checkBox4Refill.Checked == true) ||
                (checkBox3Refill.Checked == true) || (checkBox2Refill.Checked == true) || (checkBox1Refill.Checked == true))
            {
                submitRefill.Enabled = true;
            }
            else
            {
                submitRefill.Enabled = false;
            }
        }
        private void checkBox4Refill_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4Refill.Checked == true)
            {
                if (refill4Refill.Text == "0")
                {
                    mySystemMessage($"You have no available refills of {prescript4Refill.Text}. " +
                        $"Please unselect this item.");
                }
                else
                {
                    submitRefill.Enabled = true;
                }
            }
            else if ((checkBox5Refill.Checked == true) || (checkBox4Refill.Checked == true) ||
                (checkBox3Refill.Checked == true) || (checkBox2Refill.Checked == true) || (checkBox1Refill.Checked == true))
            {
                submitRefill.Enabled = true;
            }
            else
            {
                submitRefill.Enabled = false;
            }
        }
        private void checkBox5Refill_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5Refill.Checked == true)
            {
                if (refill5Refill.Text == "0")
                {
                    mySystemMessage($"You have no available refills of {prescript5Refill.Text}. " +
                        $"Please unselect this item.");
                }
                else
                {
                    submitRefill.Enabled = true;
                }
            }
            else if ((checkBox5Refill.Checked == true) || (checkBox4Refill.Checked == true) ||
                (checkBox3Refill.Checked == true) || (checkBox2Refill.Checked == true) || (checkBox1Refill.Checked == true))
            {
                submitRefill.Enabled = true;
            }
            else
            {
                submitRefill.Enabled = false;
            }
        }
        //refil checkbox area -- pharm
        private void submitPharm_Click(object sender, EventArgs e)
        {
            string orders = "";

            if ((checkBox1Pharm.Checked) && (refill1Pharm.Text != "0"))
            {
                orders += prescript1Pharm.Text + "\n";
            }
            if ((checkBox2Pharm.Checked) && (refill2Pharm.Text != "0"))
            {
                orders += prescript2Pharm.Text + "\n";
            }
            if ((checkBox3Pharm.Checked) && (refill3Pharm.Text != "0"))
            {
                orders += prescript3Pharm.Text + "\n";
            }
            if ((checkBox4Pharm.Checked) && (refill4Pharm.Text != "0"))
            {
                orders += prescript4Pharm.Text + "\n";
            }
            if ((checkBox5Pharm.Checked) && (refill5Pharm.Text != "0"))
            {
                orders += prescript5Pharm.Text + "\n";
            }

            string orderConf = $"Prescriptions:\n\n" + orders + "__________\n\n" + "Shipped to:\n\n"
                + fnameUser.Text + " " + lnameUser.Text + "\n" + streetUser.Text + "\n" + cityUser.Text
                + ", " + stateUser.Text + " " + zipUser.Text;

            DialogResult dR = MessageBox.Show(orderConf, "Confirm your order", MessageBoxButtons.OKCancel);
            if (dR == DialogResult.OK)
            {
                mySystemMessage("Your order has been placed!");
            }
        }
        private void checkBox1Pharm_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1Pharm.Checked == true)
            {
                if (refill1Pharm.Text == "0")
                {
                    mySystemMessage($"You have no available refills of {prescript1Pharm.Text}. " +
                        $"Please unselect this item.");
                }
                else
                {
                    SubmitOrderPharm.Enabled = true;
                }
            }
            else if ((checkBox1Pharm.Checked == true) || (checkBox2Pharm.Checked == true) ||
                (checkBox3Pharm.Checked == true) || (checkBox4Pharm.Checked == true) || (checkBox5Pharm.Checked == true))
            {
                SubmitOrderPharm.Enabled = true;
            }
            else
            {
                SubmitOrderPharm.Enabled = false;
            }
        }
        private void checkBox2Pharm_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2Pharm.Checked == true)
            {
                if (refill2Pharm.Text == "0")
                {
                    mySystemMessage($"You have no available refills of {prescript2Pharm.Text}. " +
                        $"Please unselect this item.");
                }
                else
                {
                    SubmitOrderPharm.Enabled = true;
                }
            }
            else if ((checkBox1Pharm.Checked == true) || (checkBox2Pharm.Checked == true) ||
                (checkBox3Pharm.Checked == true) || (checkBox4Pharm.Checked == true) || (checkBox5Pharm.Checked == true))
            {
                SubmitOrderPharm.Enabled = true;
            }
            else
            {
                SubmitOrderPharm.Enabled = false;
            }
        }
        private void checkBox3Pharm_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3Pharm.Checked == true)
            {
                if (refill3Pharm.Text == "0")
                {
                    mySystemMessage($"You have no available refills of {prescript3Pharm.Text}. " +
                        $"Please unselect this item.");
                }
                else
                {
                    SubmitOrderPharm.Enabled = true;
                }
            }
            else if ((checkBox1Pharm.Checked == true) || (checkBox2Pharm.Checked == true) ||
                (checkBox3Pharm.Checked == true) || (checkBox4Pharm.Checked == true) || (checkBox5Pharm.Checked == true))
            {
                SubmitOrderPharm.Enabled = true;
            }
            else
            {
                SubmitOrderPharm.Enabled = false;
            }
        }
        private void checkBox4Pharm_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4Pharm.Checked == true)
            {
                if (refill4Pharm.Text == "0")
                {
                    mySystemMessage($"You have no available refills of {prescript4Pharm.Text}. " +
                        $"Please unselect this item.");
                }
                else
                {
                    SubmitOrderPharm.Enabled = true;
                }
            }
            else if ((checkBox1Pharm.Checked == true) || (checkBox2Pharm.Checked == true) ||
                (checkBox3Pharm.Checked == true) || (checkBox4Pharm.Checked == true) || (checkBox5Pharm.Checked == true))
            {
                SubmitOrderPharm.Enabled = true;
            }
            else
            {
                SubmitOrderPharm.Enabled = false;
            }
        }
        private void checkBox5Pharm_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5Pharm.Checked == true)
            {
                if (refill5Pharm.Text == "0")
                {
                    mySystemMessage($"You have no available refills of {prescript5Pharm.Text}. " +
                        $"Please unselect this item.");
                }
                else
                {
                    SubmitOrderPharm.Enabled = true;
                }
            }
            else if ((checkBox1Pharm.Checked == true) || (checkBox2Pharm.Checked == true) ||
                (checkBox3Pharm.Checked == true) || (checkBox4Pharm.Checked == true) || (checkBox5Pharm.Checked == true))
            {
                SubmitOrderPharm.Enabled = true;
            }
            else
            {
                SubmitOrderPharm.Enabled = false;
            }
        }
        // ---- textbox validation section -----

        // Login page validation
        private void user_Validating(object sender, CancelEventArgs e)
        {
            //this allows the form to be closed without displaying the validation method
            if (this.ActiveControl.Equals(sender))
            {
                return;
            }
            //this is the regular null-check validation
            if (String.IsNullOrEmpty(user.Text.Trim()))
            {
                //the e.Cancel bit keeps the cursor from leaving the field until the error is completed
                e.Cancel = true;
                mySystemMessage("Fields cannot be left blank.");
            }
        }
        private void pass_Validating(object sender, CancelEventArgs e)
        {
            if (this.ActiveControl.Equals(sender))
            {
                return;
            }
            if (String.IsNullOrEmpty(pass.Text.Trim()))
            {
                e.Cancel = true;
                mySystemMessage("Fields cannot be left blank.");
            }
        }
        // panel 1 Customer validation
        private void fnameUser_Validating(object sender, CancelEventArgs e)
        {
            //the editCheck makes sure that you are actually editing the data when validations are triggered
            //otherwise it tries to validate even when you aren't editing/changing anything
            if (editCheck)
            {
                fnameUser.Text = fnameUser.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(fnameUser.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (fnameUser.Text.isAlpha())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters allowed in name fields.");
                }
            }
        }
        private void lnameUser_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                lnameUser.Text = lnameUser.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(lnameUser.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (lnameUser.Text.isAlpha())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters allowed in name fields.");
                }
            }
        }
        private void dobUser_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                dobUser.Text = dobUser.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(dobUser.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (!ValidationFunctions.isDOB(dobUser.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("DOB must be in ##/##/#### format.");
                }
            }
        }
        private void streetUser_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                streetUser.Text = streetUser.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(streetUser.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (streetUser.Text.isAlphaNum())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters and numbers allowed in Address field.");
                }
            }
        }
        private void cityUser_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                cityUser.Text = cityUser.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(cityUser.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (cityUser.Text.isAlpha())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters allowed in City field.");
                }
            }
        }
        private void stateUser_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                stateUser.Text = stateUser.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(stateUser.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (stateUser.Text.isAlpha())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters allowed in State field.");
                }
            }
        }
        private void stateUser_TextChanged(object sender, EventArgs e)
        {
            //force uppercase for valid state code
            stateUser.Text = stateUser.Text.ToUpper();
        }
        private void zipUser_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                zipUser.Text = zipUser.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(zipUser.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (zipUser.Text.isNum())
                {
                    e.Cancel = true;
                    mySystemMessage("Zipcode field must be in ##### format.");
                }
            }
        }
        private void insuranceUser_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                insuranceUser.Text = insuranceUser.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(insuranceUser.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (insuranceUser.Text.isAlphaNum())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters and numbers allowed in Address field.");
                }
            }
        }
        private void emailUser_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                emailUser.Text = emailUser.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(emailUser.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (!ValidationFunctions.IsEmail(emailUser.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Please enter a valid email address.");
                }
            }
        }
        private void telephoneUser_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                telephoneUser.Text = telephoneUser.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(telephoneUser.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (telephoneUser.Text.isPhone())
                {
                    e.Cancel = true;
                    mySystemMessage("Telephone Number field must be in ###-###-#### format.");
                }
            }
        }
        private void telephoneUser_TextChanged(object sender, EventArgs e)
        {
            //when inputting a phone number, automatically insert "-" after first three digits and
            //next three digits, then put the cursor after the dash
            if (telephoneUser.Text.Length > 2)
            {
                if (telephoneUser.Text[telephoneUser.Text.Length - 1] != '-')
                {
                    if (telephoneUser.Text.Length == 3)
                    {
                        telephoneUser.Text = telephoneUser.Text + "-";
                        telephoneUser.SelectionStart = 4;
                    }
                    if (telephoneUser.Text.Length == 7)
                    {
                        telephoneUser.Text = telephoneUser.Text + "-";
                        telephoneUser.SelectionStart = 8;
                    }
                }
            }       
        }
        private void telephoneUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (telephoneUser.Text.Length > 2)
            {
                if (telephoneUser.Text[telephoneUser.Text.Length - 1] == '-')
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        telephoneUser.Text = telephoneUser.Text.Remove(telephoneUser.Text.Length - 2, 1);
                        telephoneUser.SelectionStart = telephoneUser.TextLength;
                    }
                }
            }
        }
        private void ccUser_Validating(object sender, CancelEventArgs e)
        {
            //We can include regex to verify that input is a valid cc number, but for
            //now it simply checks for the right number of digits
            if (editCheck)
            {
                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                ccUser.Text = ccUser.Text.Trim();

                if (String.IsNullOrEmpty(ccUser.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if ((ccUser.Text.isNum()) || (ccUser.Text.Length < 15))
                {
                    e.Cancel = true;
                    mySystemMessage("Payment Method field must be between 15 and 19 numerical characters.");
                }
            }
        }
        private void allergiesUser_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                allergiesUser.Text = allergiesUser.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                //no null check, as this is the only field that may not always be used
                if (allergiesUser.Text.isAlphaNum())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters and numbers allowed in Drug Allergies field.");
                }
            }
        }
        //panel 2 Pharm validation

        private void fnamePharm_Validating(object sender, CancelEventArgs e)
        {
            //the editCheck makes sure that you are actually editing the data when validations are triggered
            //otherwise it tries to validate even when you aren't editing/changing anything
            if (editCheck)
            {
                fnamePharm.Text = fnamePharm.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(fnamePharm.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (fnamePharm.Text.isAlpha())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters allowed in name fields.");
                }
            }
        }
        private void lnamePharm_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                lnamePharm.Text = lnamePharm.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(lnamePharm.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (lnamePharm.Text.isAlpha())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters allowed in name fields.");
                }
            }
        }
        private void dobPharm_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                dobPharm.Text = dobPharm.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(dobPharm.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (!ValidationFunctions.isDOB(dobPharm.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("DOB must be in ##/##/#### format.");
                }
            }
        }
        private void streetPharm_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                streetPharm.Text = streetPharm.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(streetPharm.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (streetPharm.Text.isAlphaNum())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters and numbers allowed in Address field.");
                }
            }
        }
        private void cityPharm_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                cityPharm.Text = cityPharm.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(cityPharm.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (cityPharm.Text.isAlpha())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters allowed in City field.");
                }
            }
        }
        private void statePharm_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                statePharm.Text = statePharm.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(statePharm.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (statePharm.Text.isAlpha())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters allowed in State field.");
                }
            }
        }
        private void statePharm_TextChanged(object sender, EventArgs e)
        {
            //force uppercase for valid state code
            stateUser.Text = stateUser.Text.ToUpper();
        }
        private void zipPharm_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                zipPharm.Text = zipPharm.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(zipPharm.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (zipPharm.Text.isNum())
                {
                    e.Cancel = true;
                    mySystemMessage("Zipcode field must be in ##### format.");
                }
            }
        }
        private void insurancePharm_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                insurancePharm.Text = insurancePharm.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(insurancePharm.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (insurancePharm.Text.isAlphaNum())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters and numbers allowed in Address field.");
                }
            }
        }
        private void emailPharm_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                emailPharm.Text = emailPharm.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(emailPharm.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (!ValidationFunctions.IsEmail(emailPharm.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Please enter a valid email address.");
                }
            }
        }
        private void telephonePharm_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                telephonePharm.Text = telephonePharm.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(telephonePharm.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (telephonePharm.Text.isPhone())
                {
                    e.Cancel = true;
                    mySystemMessage("Telephone Number field must be in ###-###-#### format.");
                }
            }
        }
        private void telephonePharm_TextChanged(object sender, EventArgs e)
        {
            //when inputting a phone number, automatically insert "-" after first three digits and
            //next three digits, then put the cursor after the dash
            if (telephonePharm.Text.Length > 2)
            {
                if (telephonePharm.Text[telephonePharm.Text.Length - 1] != '-')
                {
                    if (telephonePharm.Text.Length == 3)
                    {
                        telephonePharm.Text = telephonePharm.Text + "-";
                        telephonePharm.SelectionStart = 4;
                    }
                    if (telephonePharm.Text.Length == 7)
                    {
                        telephonePharm.Text = telephonePharm.Text + "-";
                        telephonePharm.SelectionStart = 8;
                    }
                }
            }
        }
        private void telephonePharm_KeyDown(object sender, KeyEventArgs e)
        {
            if (telephonePharm.Text.Length > 2)
            {
                if (telephonePharm.Text[telephonePharm.Text.Length - 1] == '-')
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        telephonePharm.Text = telephonePharm.Text.Remove(telephonePharm.Text.Length - 2, 1);
                        telephonePharm.SelectionStart = telephonePharm.TextLength;
                    }
                }
            }
        }
        private void ccPharm_Validating(object sender, CancelEventArgs e)
        {
            //We can include regex to verify that input is a valid cc number, but for
            //now it simply checks for the right number of digits
            if (editCheck)
            {
                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                ccPharm.Text = ccPharm.Text.Trim();

                if (String.IsNullOrEmpty(ccPharm.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if ((ccPharm.Text.isNum()) || (ccPharm.Text.Length < 15))
                {
                    e.Cancel = true;
                    mySystemMessage("Payment Method field must be between 15 and 19 numerical characters.");
                }
            }
        }
        private void allergiesPharm_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                allergiesPharm.Text = allergiesPharm.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                //no null check, as this is the only field that may not always be used
                if (allergiesPharm.Text.isAlphaNum())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters and numbers allowed in Drug Allergies field.");
                }
            }
        }
        //admin panel validating

        private void fnameAdmin_Validating(object sender, CancelEventArgs e)
        {
            //the editCheck makes sure that you are actually editing the data when validations are triggered
            //otherwise it tries to validate even when you aren't editing/changing anything
            if (editCheck)
            {
                fnameAdmin.Text = fnameAdmin.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(fnameAdmin.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (fnameAdmin.Text.isAlpha())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters allowed in name fields.");
                }
            }
        }
        private void lnameAdmin_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                lnameAdmin.Text = lnameAdmin.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(lnameAdmin.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (lnameAdmin.Text.isAlpha())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters allowed in name fields.");
                }
            }
        }
        private void dobAdmin_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                dobAdmin.Text = dobAdmin.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(dobAdmin.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (!ValidationFunctions.isDOB(dobAdmin.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("DOB must be in ##/##/#### format.");
                }
            }
        }
        private void streetAdmin_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                streetAdmin.Text = streetAdmin.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(streetAdmin.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (streetAdmin.Text.isAlphaNum())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters and numbers allowed in Address field.");
                }
            }
        }
        private void cityAdmin_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                cityAdmin.Text = cityAdmin.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(cityAdmin.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (cityAdmin.Text.isAlpha())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters allowed in City field.");
                }
            }
        }
        private void stateAdmin_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                stateAdmin.Text = stateAdmin.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(stateAdmin.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (stateAdmin.Text.isAlpha())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters allowed in State field.");
                }
            }
        }
        private void stateAdmin_TextChanged(object sender, EventArgs e)
        {
            //force uppercase for valid state code
            stateAdmin.Text = stateAdmin.Text.ToUpper();
        }
        private void zipAdmin_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                zipAdmin.Text = zipAdmin.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(zipAdmin.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (zipAdmin.Text.isNum())
                {
                    e.Cancel = true;
                    mySystemMessage("Zipcode field must be in ##### format.");
                }
            }
        }
        private void insuranceAdmin_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                insuranceAdmin.Text = insuranceAdmin.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(insuranceAdmin.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (insuranceAdmin.Text.isAlphaNum())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters and numbers allowed in Address field.");
                }
            }
        }
        private void emailAdmin_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                emailAdmin.Text = emailAdmin.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(emailAdmin.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (!ValidationFunctions.IsEmail(emailAdmin.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Please enter a valid email address.");
                }
            }
        }
        private void telephoneAdmin_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                telephoneAdmin.Text = telephoneAdmin.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                if (String.IsNullOrEmpty(telephoneAdmin.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if (telephoneAdmin.Text.isPhone())
                {
                    e.Cancel = true;
                    mySystemMessage("Telephone Number field must be in ###-###-#### format.");
                }
            }
        }
        private void telephoneAdmin_TextChanged(object sender, EventArgs e)
        {
            if (telephoneAdmin.Text.Length > 2)
            {
                if (telephoneAdmin.Text[telephoneAdmin.Text.Length - 1] != '-')
                {
                    if (telephoneAdmin.Text.Length == 3)
                    {
                        telephoneAdmin.Text = telephoneAdmin.Text + "-";
                        telephoneAdmin.SelectionStart = 4;
                    }
                    if (telephoneAdmin.Text.Length == 7)
                    {
                        telephoneAdmin.Text = telephoneAdmin.Text + "-";
                        telephoneAdmin.SelectionStart = 8;
                    }
                }
            }
        }
        private void telephoneAdmin_KeyDown(object sender, KeyEventArgs e)
        {
            if (telephoneAdmin.Text.Length > 2)
            {
                if (telephoneAdmin.Text[telephoneAdmin.Text.Length - 1] == '-')
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        telephoneAdmin.Text = telephoneAdmin.Text.Remove(telephoneAdmin.Text.Length - 2, 1);
                        telephoneAdmin.SelectionStart = telephoneAdmin.TextLength;
                    }
                }
            }
        }
        private void ccAdmin_Validating(object sender, CancelEventArgs e)
        {
            //We can include regex to verify that input is a valid cc number, but for
            //now it simply checks for the right number of digits
            if (editCheck)
            {
                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                ccAdmin.Text = ccAdmin.Text.Trim();

                if (String.IsNullOrEmpty(ccAdmin.Text))
                {
                    e.Cancel = true;
                    mySystemMessage("Fields cannot be left blank.");
                }

                if ((ccAdmin.Text.isNum()) || (ccAdmin.Text.Length < 15))
                {
                    e.Cancel = true;
                    mySystemMessage("Payment Method field must be between 15 and 19 numerical characters.");
                }
            }
        }
        private void allergiesAdmin_Validating(object sender, CancelEventArgs e)
        {
            if (editCheck)
            {
                allergiesAdmin.Text = allergiesAdmin.Text.Trim();

                if (this.ActiveControl.Equals(sender))
                {
                    return;
                }

                //no null check, as this is the only field that may not always be used
                if (allergiesAdmin.Text.isAlphaNum())
                {
                    e.Cancel = true;
                    mySystemMessage("Only letters and numbers allowed in Drug Allergies field.");
                }
            }
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
        public static bool isPhone(this string @this)
        {
            return Regex.IsMatch(@this, "[^0-9-]");
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
        //simple email validation
        //from user Cogwheel @ https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
        public static bool IsEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
