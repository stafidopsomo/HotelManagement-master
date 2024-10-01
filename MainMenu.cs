using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace HotelManagement
{
    public partial class MainMenu : Form
    {

        private int userID;
        
        public MainMenu(int x)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            typeCombo.SelectedIndex = 0;
            searchPanel.Hide();
            resultsPanel.Hide();
            homePanel.Show();
            resultsLabelCust.Hide();
            dataGridView4.Hide();
            custDetailsPanel.Hide();
            userID = x;
            int y = updateUserPanel();
            if (y == 0)
            {
                Application.Exit();
            }
            else
            {
                updateHome();
            }
        }

        int updateUserPanel()
        {
            DB db = new DB();
            try
            {
                MySqlConnection con = new MySqlConnection(db.getConnString());
                con.Open();
                MySqlCommand comm = con.CreateCommand();
                comm.CommandText = "SELECT * FROM hot_usr where usr_ID = @userID";
                comm.Parameters.AddWithValue("@userID", userID);
                MySqlDataReader reader = comm.ExecuteReader();
                string userType = "-1";
                if (reader.Read())
                {
                    string userName = reader["usr_username"].ToString();
                    userType = reader["usr_Type"].ToString();
                    welcomeLabelCust.Text = welcomeLabelCust.Text + " " + userName;
                }
                con.Close();
                if (userType.Equals("1"))
                {
                    employeePanel.Show();
                    customerPanel.Hide();
                }
                else if (userType.Equals("2"))
                {
                    custHomePanel.Show();
                    employeePanel.Hide();
                    customerPanel.Show();
                    searchRoomDatePanel.Hide();
                    showRoomBtn.Hide();
                    roomDetailsPanel.Hide();
                    custBookingsPanel.Hide();
                    paymentPanel.Hide();
                }
                else
                {
                    MessageBox.Show("Πρόβλημα στις ρυθμίσεις του λογαριασμού του χρήστη. Η εφαρμογή θα τερματιστεί.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }
            }
            catch (MySqlException)
            {
                MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων.Κωδικος:14 Η εφαρμογή θα τερματιστεί.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            return 1;
        }

        void updateHome()
        {
            DB db = new DB();
            try
            {
                MySqlConnection con = new MySqlConnection(db.getConnString());
                con.Open();
                MySqlCommand comm1 = con.CreateCommand();
                string currDate = DateTime.Now.ToString("yyyy-MM-dd");
                comm1.CommandText = "SELECT booking_ID , concat(usr_FName,' ',usr_LName) as 'Name', " +
                    "checkout_Date,hot_bookings.room_ID,room_TypeN,booking_Comments,usr_Email,usrID_Phone, booking_cost,"+
                    "CAST(booking_Date AS DATE) , CAST(checkin_Date AS DATE) ,CAST(checkout_Date AS DATE) , ifnull(booking_Comments, ''),CONCAT(booking_cost, '€')," + 
                    "CASE WHEN booking_hasPaid = 0 THEN \"Όχι\" WHEN booking_hasPaid = 1 THEN \"Ναι\" END AS \"booking_hasPaid\" FROM hot_bookings " +
                    "INNER JOIN hot_usr ON hot_usr.usr_ID = hot_bookings.usr_ID " +
                    "INNER JOIN hot_usr_det ON hot_usr_det.usr_ID = hot_bookings.usr_ID " +
                    "INNER JOIN hot_rooms ON hot_bookings.room_ID = hot_rooms.room_ID " +
                    "WHERE checkin_Date = @currDate AND hasArrived = 0";
                comm1.Parameters.AddWithValue("@currDate", currDate);
                bool hasResults1;
                using (MySqlDataReader reader1 = comm1.ExecuteReader())
                {
                    DataTable dataTable1 = new DataTable();
                    dataTable1.Load(reader1);
                    dataGridView2.DataSource = dataTable1;
                    dataTable1.Columns["booking_ID"].ColumnName = "Αρ. Κράτησης";
                    dataTable1.Columns["Name"].ColumnName = "Όνομα Κράτησης";
                    dataTable1.Columns["room_ID"].ColumnName = "Αρ. Δωματίου";
                    dataTable1.Columns["room_TypeN"].ColumnName = "Τύπος Δωματίου";
                    dataTable1.Columns["checkout_Date"].ColumnName = "Ημ/νία Check-out";
                    dataTable1.Columns["booking_Comments"].ColumnName = "Σχόλια Κράτησης";
                    dataTable1.Columns["usrID_Phone"].ColumnName = "Τηλέφωνο Επικοινωνίας";
                    dataTable1.Columns["usr_Email"].ColumnName = "Email Επικοινωνίας";
                    dataTable1.Columns["booking_cost"].ColumnName = "Κόστος"; 
                    dataTable1.Columns["booking_hasPaid"].ColumnName = "Πληρωμένο";

                    hasResults1 = dataTable1.Rows.Count > 0;
                }
                if (hasResults1)
                {
                    noArrivalsLabel.Hide();
                    dataGridView2.Show();
                    markArrivalButton.Show();
                }
                else
                {
                    dataGridView2.Hide();
                    noArrivalsLabel.Show();
                    markArrivalButton.Hide();
                }
                MySqlCommand comm2 = con.CreateCommand();
                comm2.CommandText = "SELECT booking_ID , concat(usr_FName,' ',usr_LName) as 'Name', " +
                    "checkout_Date-checkin_Date as 'Nights',format((checkout_Date-checkin_Date)*room_Price,2) as 'Total Cost',hot_bookings.room_ID,room_TypeN,booking_Comments,usr_Email,usrID_Phone FROM hot_bookings " +
                    "INNER JOIN hot_usr ON hot_usr.usr_ID = hot_bookings.usr_ID " +
                    "INNER JOIN hot_usr_det ON hot_usr_det.usr_ID = hot_bookings.usr_ID " +
                    "INNER JOIN hot_rooms ON hot_bookings.room_ID = hot_rooms.room_ID " +
                    "WHERE checkout_Date = @currDate AND hasDeparted = 0";
                comm2.Parameters.AddWithValue("@currDate", currDate);
                bool hasResults2;
                using (MySqlDataReader reader2 = comm2.ExecuteReader())
                {
                    DataTable dataTable2 = new DataTable();
                    dataTable2.Load(reader2);
                    dataGridView3.DataSource = dataTable2;
                    dataTable2.Columns["booking_ID"].ColumnName = "Αρ. Κράτησης";
                    dataTable2.Columns["Name"].ColumnName = "Όνομα Κράτησης";
                    dataTable2.Columns["Nights"].ColumnName = "Διανυκτερεύσεις";
                    dataTable2.Columns["Total Cost"].ColumnName = "Συνολικό Κόστος";
                    dataTable2.Columns["room_ID"].ColumnName = "Αρ. Δωματίου";
                    dataTable2.Columns["room_TypeN"].ColumnName = "Τύπος Δωματίου";
                    dataTable2.Columns["booking_Comments"].ColumnName = "Σχόλια Κράτησης";
                    dataTable2.Columns["usrID_Phone"].ColumnName = "Τηλέφωνο Επικοινωνίας";
                    dataTable2.Columns["usr_Email"].ColumnName = "Email Επικοινωνίας";
                    hasResults2 = dataTable2.Rows.Count > 0;
                    if (hasResults2)
                    {
                        noDeparturesLabel.Hide();
                        dataGridView3.Show();
                        markDepartureButton.Show();
                    }
                    else
                    {
                        dataGridView3.Hide();
                        noDeparturesLabel.Show();
                        markDepartureButton.Hide();
                    }
                }
                con.Close();
            }
            catch (MySqlException)
            {
                MessageBox.Show("Πρόβληβα επικοινωνίας με την βάση δεδομένων. Η εφαρμογή θα τερματιστεί","Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

        }

        void searchDate_Click(object sender, EventArgs e)
        {
            if (typeCombo.SelectedIndex > -1 && dateBox.SelectedIndex > -1 && monthBox.SelectedIndex > -1 && yearBox.SelectedIndex > -1)
            {
                try
                {
                    DB db = new DB();
                    MySqlConnection con = new MySqlConnection(db.getConnString());
                    con.Open();
                    MySqlCommand comm = con.CreateCommand();
                    string input = monthBox.Text;
                    string monthNumber = input.Substring(0, 2);
                    string givenDate = yearBox.Text + "-" + monthNumber + "-" + dateBox.Text;
                    string grFormatDate = dateBox.Text + "-" + monthNumber + "-" + yearBox.Text;
                    comm.CommandText = "SELECT booking_ID,concat(usr_fname,' ',usr_lname) as usr_ID,room_ID," +
                        "checkin_Date, checkout_Date, usrID_Phone, booking_Comments FROM hot_bookings " +
                        "INNER JOIN hot_usr ON hot_bookings.usr_ID = hot_usr.usr_ID " +
                        "LEFT JOIN hot_usr_det on hot_usr.usr_ID = hot_usr_det.usr_ID where ";

                    if (typeCombo.SelectedIndex == 0)
                    {
                        comm.CommandText = comm.CommandText + " cast(booking_Date as date) = @givenDate";
                    }
                    else
                    {
                        comm.CommandText = comm.CommandText + "cast(checkin_Date as date) = @givenDate";
                    }
                    comm.Parameters.AddWithValue("@givenDate", givenDate);
                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        dataGridView1.DataSource = dataTable;
                        dataTable.Columns["booking_ID"].ColumnName = "Αρ. Κράτησης";
                        dataTable.Columns["usr_ID"].ColumnName = "Όνομα Κράτησης";
                        dataTable.Columns["room_ID"].ColumnName = "Αρ. Δωματίου";
                        dataTable.Columns["checkout_Date"].ColumnName = "Ημ/νία Check-out";
                        dataTable.Columns["checkin_Date"].ColumnName = "Ημ/νία Check-in";
                        dataTable.Columns["usrID_Phone"].ColumnName = "Τηλέφωνο Επικοινωνίας";
                        dataTable.Columns["booking_Comments"].ColumnName = "Σχόλια Κράτησης";
                    }
                    searchPanel.Hide();
                    resultsPanel.Show();
                    resultsLabel.Text = "Αποτελέσματα για " + typeCombo.SelectedText + " : " + grFormatDate;
                    con.Close();
                }
                catch (MySqlException)
                {
                    MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων.Κωδικος:1 Η εφαρμογή θα τερματιστεί", "Μήνυμα Εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("Πρέπει να επιλέξεις μία από τις επιλογές για όλα τα πεδία!", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void searchCustDetails_Click(object sender, EventArgs e)
        {
            if (!nameField.Text.Equals("") || !surnameField.Text.Equals("") || !phoneField.Text.Equals("") || !emailField.Text.Equals(""))
            {
                try
                {
                    DB db = new DB();
                    MySqlConnection con = new MySqlConnection(db.getConnString());
                    con.Open();
                    MySqlCommand comm = con.CreateCommand();
                    comm.CommandText = "SELECT booking_ID,concat(usr_fname,' ',usr_lname) as usr_ID,room_ID," +
                        "checkin_Date, checkout_Date, usrID_Phone FROM hot_bookings " +
                        "INNER JOIN hot_usr ON hot_bookings.usr_ID = hot_usr.usr_ID " +
                        "INNER JOIN hot_usr_det on hot_usr.usr_ID = hot_usr_det.usr_ID " +
                        "WHERE 1=1 ";
                    if (!nameField.Text.Equals(""))
                    {
                        comm.CommandText = comm.CommandText + "AND usr_FName LIKE CONCAT('%', @fname, '%') ";
                        comm.Parameters.AddWithValue("@fname", nameField.Text);
                    }
                    if (!surnameField.Text.Equals(""))
                    {
                        comm.CommandText = comm.CommandText + "AND usr_LName LIKE CONCAT('%', @lname, '%') ";
                        comm.Parameters.AddWithValue("@lname", surnameField.Text);
                    }
                    if (!phoneField.Text.Equals(""))
                    {
                        comm.CommandText = comm.CommandText + "AND usrID_Phone LIKE CONCAT('%', @phone, '%') ";
                        comm.Parameters.AddWithValue("@phone", phoneField.Text);
                    }
                    if (!emailField.Text.Equals(""))
                    {
                        comm.CommandText = comm.CommandText + "AND usr_Email LIKE CONCAT('%', @email, '%') ";
                        comm.Parameters.AddWithValue("@email", emailField.Text);
                    }

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        dataGridView1.DataSource = dataTable;
                        dataTable.Columns["booking_ID"].ColumnName = "Αρ. Κράτησης";
                        dataTable.Columns["usr_ID"].ColumnName = "Όνομα Κράτησης";
                        dataTable.Columns["room_ID"].ColumnName = "Αριθμός Δωματίου";
                        dataTable.Columns["checkin_Date"].ColumnName = "Ημερομηνία Check-in";
                        dataTable.Columns["checkout_Date"].ColumnName = "Ημερομηνία Check-out";
                        dataTable.Columns["usrID_Phone"].ColumnName = "Τηλέφωνο Επικοινωνίας";
                    }
                    searchPanel.Hide();
                    resultsPanel.Show();
                    resultsLabel.Text = "Αποτελέσματα για αναζήτηση με στοιχεία:";
                    con.Close();
                }
                catch (MySqlException)
                {
                    MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων.Κωδικος:2 Η εφαρμογή θα τερματιστεί", "Μήνυμα Εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("Πρέπει να συμπληρώσεις τουλάχιστον ένα από τα πεδία!", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            homePanel.Show();
            resultsPanel.Hide();
            searchPanel.Hide();
            updateHome();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private void searchBookingsClick(object sender, EventArgs e)
        {
            homePanel.Hide();
            resultsPanel.Hide();
            nextBookings();
            searchPanel.Show();
        }

        private void nextBookings()
        {
            try
            {


                DB db = new DB();
                MySqlConnection con = new MySqlConnection(db.getConnString());
                con.Open();
                MySqlCommand comm = con.CreateCommand();
                comm.CommandText = "SELECT booking_ID, room_ID, CONCAT(usr_FName, ' ', usr_LName) AS name, booking_Date, checkin_Date, checkout_Date, booking_comments, " +
                            "CASE WHEN booking_hasPaid = 0 THEN 'Όχι' WHEN booking_hasPaid = 1 THEN 'Ναι' END AS test " +
                            "FROM hot_bookings " +
                            "INNER JOIN hot_usr ON hot_bookings.usr_ID = hot_usr.usr_ID " +
                            "WHERE checkin_Date >= CURDATE() AND hasArrived = 0 " +
                            "LIMIT 10";
                using (MySqlDataReader reader = comm.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView7.DataSource = dataTable;
                    dataTable.Columns["booking_ID"].ColumnName = "Αρ. Κράτησης";
                    dataTable.Columns["name"].ColumnName = "Όνομα Κράτησης";
                    dataTable.Columns["room_ID"].ColumnName = "Αρ. Δωματίου";
                    dataTable.Columns["booking_Date"].ColumnName = "Ημ/νία Κράτησης";
                    dataTable.Columns["checkin_Date"].ColumnName = "Ημ/νία Check-in";
                    dataTable.Columns["checkout_Date"].ColumnName = "Ημ/νία Check-out";
                    dataTable.Columns["test"].ColumnName = "Πληρωμένη";
                    dataTable.Columns["booking_Comments"].ColumnName = "Σχόλια Κράτησης";
                }
            }
            catch (MySqlException)
            {
                MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων.Κωδικος:3 Η εφαρμογή θα τερματιστεί.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void markArrivalClick(object sender, EventArgs e)
        {
            if (dataGridView2.Visible)
            {
                DataGridViewRow selectedRow = dataGridView2.CurrentRow;
                if (selectedRow != null)
                {
                    DataGridViewCell cell1 = selectedRow.Cells[0];
                    object value1 = cell1.Value;
                    if (value1 != null)
                    {
                        string stringValue1 = value1.ToString();
                        DialogResult result = MessageBox.Show("Είστε σίγουροι για την πραγματοποίηση του Check-in της κράτησης με αριθμό: " + stringValue1 + ";", "Ενημέρωση δεδομένων εφαρμογής", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            bool prePaid = false;
                            DataGridViewCell cell2 = selectedRow.Cells[8];
                            DataGridViewCell cell3 = selectedRow.Cells[9];
                            object value2 = cell2.Value;
                            object value3 = cell3.Value;
                            if (value2 != null && value3 != null)
                            {
                                string stringValue2 = value2.ToString();
                                string stringValue3 = value3.ToString();
                                if (stringValue3 == "0") {
                                    MessageBox.Show("Ο πελάτης δεν έχει πληρώσει. Βεβαιωθείτε ότι λάβατε το ποσό πληρωμής: " +
                                        "" + stringValue2 + "€ κατά την διαδικασία του Check-in.", "Ενημέρωση δεδομένων εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    prePaid = true;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων.Κωδικος:4 Η εφαρμογή θα τερματιστεί.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Application.Exit();
                            }
                            DB db = new DB();
                            try
                            {
                                MySqlConnection con = new MySqlConnection(db.getConnString());
                                con.Open();
                                MySqlCommand update1 = con.CreateCommand();
                                if (prePaid)
                                {
                                    update1.CommandText = "UPDATE hot_bookings " +
                                    "SET hasArrived = 1 WHERE booking_ID = @bookingID";
                                }
                                else
                                {
                                    update1.CommandText = "UPDATE hot_bookings " +
                                    "SET hasArrived = 1, booking_hasPaid = 1 WHERE booking_ID = @bookingID";
                                }
                                update1.Parameters.AddWithValue("@bookingID", stringValue1);
                                update1.ExecuteNonQuery();
                                con.Close();
                                updateHome();
                            }
                            catch (MySqlException)
                            {
                                MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων.Κωδικος:5 Η εφαρμογή θα τερματιστεί.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Application.Exit();
                            }

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Δεν υπάρχουν αποτελέσματα για να πραγματοποιηθούν τροποποιήσεις", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void markDeparture_Click(object sender, EventArgs e)
        {
            if (dataGridView3.Visible)
            {
                DataGridViewRow selectedRow = dataGridView3.CurrentRow;
                if (selectedRow != null)
                {
                    DataGridViewCell cell1 = selectedRow.Cells[0];
                    // Retrieve the value from the cell
                    object value1 = cell1.Value;
                    // Check if the value is not null
                    if (value1 != null)
                    {
                        // Convert the value to the desired data type
                        string stringValue1 = value1.ToString();
                        DialogResult result = MessageBox.Show("Είστε σίγουροι πως η κράτηση με αριθμό: " + stringValue1 + " έχει πραγματοποιήσει check-out;", "Ενημέρωση δεδομένων εφαρμογής", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            DB db = new DB();
                            try
                            {
                                MySqlConnection con = new MySqlConnection(db.getConnString());
                                con.Open();
                                MySqlCommand update1 = con.CreateCommand();
                                update1.CommandText = "UPDATE hot_bookings " +
                                    "SET hasDeparted = 1 WHERE booking_ID = @bookingID";
                                update1.Parameters.AddWithValue("@bookingID", stringValue1);
                                update1.ExecuteNonQuery();
                                con.Close();
                                updateHome();
                            }
                            catch (MySqlException)
                            {
                                MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων.Κωδικος:6 Η εφαρμογή θα τερματιστεί.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Application.Exit();
                            }

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Δεν υπάρχουν αποτελέσματα για να πραγματοποιηθούν τροποποιήσεις", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void roomSearch_Click(object sender, EventArgs e)
        {
            homePanel.Hide();
            resultsPanel.Hide();
            searchPanel.Hide();
        }

        private void searchCustBtn_Click(object sender, EventArgs e)
        {
            custHomePanel.Hide();
            searchRoomDatePanel.Show();
            dataGridView4.Rows.Clear();
            dataGridView4.Columns.Clear();
            dataGridView4.Hide();
            showRoomBtn.Hide();
            resultsLabelCust.Hide();
            roomDetailsPanel.Hide();
            custBookingsPanel.Hide();
            custDetailsPanel.Hide();
            dateTimePicker1.Value = DateTime.Today.AddDays(1);
            dateTimePicker2.Value = DateTime.Today.AddDays(3);
        }

        private void bookingsCustBtn_Click(object sender, EventArgs e)
        {
            custHomePanel.Hide();
            searchRoomDatePanel.Hide();
            roomDetailsPanel.Hide();
            custBookingsPanel.Hide();
            custDetailsPanel.Show();
        }

        private void custAccBtn_Click(object sender, EventArgs e)
        {
            custHomePanel.Hide();
            searchRoomDatePanel.Hide();
            roomDetailsPanel.Hide();
            custBookingsPanel.Hide();
            custDetailsPanel.Show();
            DB db = new DB();
            try
            {
                MySqlConnection con = new MySqlConnection(db.getConnString());
                con.Open();
                MySqlCommand comm = con.CreateCommand();
                comm.CommandText = "SELECT * FROM hot_usr\r\nINNER JOIN hot_usr_det ON hot_usr_det.usr_ID = hot_usr.usr_ID\r\nWHERE hot_usr.usr_ID = @usr_ID";
                comm.Parameters.AddWithValue("@usr_ID", userID);
                using (MySqlDataReader reader = comm.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read()) { 
                            nameBox.Text = reader.GetString(1);
                            surnameBox.Text = reader.GetString(2);
                            usernameBox.Text = reader.GetString(3);
                            emailBox.Text = reader.GetString(5);
                            passBox.Text = reader.GetString(4);
                            addressBox.Text = reader.GetString(9);
                            regionBox.Text = reader.GetString(10);
                            countryBox.Text = reader.GetString(11);
                            phoneBox.Text = reader.GetString(12);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Πρόβλημα στην ανάκτηση των δεδομένων.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                con.Close();
            }catch (MySqlException w)
            {
                MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων.Κωδικος:7" + w, "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    

        private void homeButtonCustBtn_Click(object sender, EventArgs e)
        {
            custHomePanel.Show();
            searchRoomDatePanel.Hide();
            roomDetailsPanel.Hide();
            custBookingsPanel.Hide();
            custDetailsPanel.Hide();
        }

        private void searchDatesBtn_Click(object sender, EventArgs e)
        {

            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("Η ημερομηνία Check-out πρέπει να είναι μεταγενέστερη από την ημερομηνία Check-in.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dateTimePicker1.Value == dateTimePicker2.Value)
            {
                MessageBox.Show("Πρέπει να κάνεις αναζήτηση για τουλάχιστον μια διανυκτέρευση.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dataGridView4.Rows.Clear();
            dataGridView4.Columns.Clear();
            dataGridView4.Show();
            string date1 = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
            string date2 = dateTimePicker2.Value.Date.ToString("yyyy-MM-dd");
            resultsLabelCust.Text = "Αποτελέσματα μεταξύ ημερομηνιών: " + dateTimePicker1.Value.Date.ToString("dd-MM-yyyy") + " και " + dateTimePicker2.Value.Date.ToString("dd-MM-yyyy");
            resultsLabelCust.Show();
            dataGridView4.Columns.Add("RoomNumber", "Αρ. Δωματίου");
            dataGridView4.Columns.Add("RommType", "Τύπος Δωματίου");
            dataGridView4.Columns.Add("Cost", "Κόστος ανά βραδιά");
            DB db = new DB();
            try
            {
                MySqlConnection con = new MySqlConnection(db.getConnString());
                con.Open();
                MySqlCommand comm = con.CreateCommand();
                comm.CommandText = "SELECT hot_rooms.room_ID, room_TypeN, room_Price FROM hot_rooms" +
                    " LEFT JOIN hot_bookings hb ON hot_rooms.room_ID = hb.room_ID AND(@wantin BETWEEN hb.checkin_Date AND hb.checkout_Date OR" +
                    " @wantout BETWEEN hb.checkin_Date AND hb.checkout_Date) WHERE hb.room_ID IS NULL order by hot_rooms.room_ID; ";
                comm.Parameters.AddWithValue("@wantin", date1);
                comm.Parameters.AddWithValue("@wantout", date2);
                MySqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    string roomNumber = reader.GetString(0); // Assuming room number is retrieved from the first column
                    string picturePath = reader.GetString(1); // Assuming picture path is retrieved from the second column
                    string cost = reader.GetString(2) + " €"; // Assuming cost is retrieved from the third column

                    // Add a new row and populate the cells
                    dataGridView4.Rows.Add(roomNumber, picturePath, cost);
                }
                dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView4.AutoResizeColumns();
                showRoomBtn.Show();
                con.Close();
            }
            catch (MySqlException)
            {
                MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων.Κωδικος:8 Η εφαρμογή θα τερματιστεί.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

        }

        private void showRoomBtn_Click(object sender, EventArgs e)
        {
            resetText();
            DateTime date1 = dateTimePicker1.Value; // Get the value from the first DateTimePicker
            DateTime date2 = dateTimePicker2.Value; // Get the value from the second DateTimePicker

            TimeSpan difference = date2 - date1; // Subtract the dates
            int nights = (int)difference.Days;
            PhotoList.list.Clear();
            if (dataGridView4.Rows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView4.SelectedRows[0];
                DataGridViewCell cell = selectedRow.Cells[0];
                object value = cell.Value;
                string room_ID;
                if (value != null)
                {
                    room_ID = value.ToString();
                    roomDetailsPanel.Show();
                    searchRoomDatePanel.Hide();
                    roomInfoLabel.Text = "Πληροφορίες για το δωμάτιο " + room_ID;
                    DB db = new DB();
                    List<string> pic_path = new List<string>();
                    try
                    {
                        MySqlConnection con = new MySqlConnection(db.getConnString());
                        con.Open();
                        MySqlCommand comm1 = con.CreateCommand();
                        comm1.CommandText = "select * from hot_rooms_pics where room_ID = @roomID ";
                        comm1.Parameters.AddWithValue("@roomID", room_ID);
                        MySqlDataReader reader1 = comm1.ExecuteReader();
                        while (reader1.Read())
                        {
                            string workingDirectory = Environment.CurrentDirectory;
                            string path = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\Rooms\\" + reader1.GetString(2);
                            Console.WriteLine(path);
                            if (File.Exists(path))
                            {
                                PhotoList.list.Add(path);
                            }
                        }
                        if (PhotoList.list.Count > 0)
                        {
                            PhotoList.index = 0;
                            roomPicBox.ImageLocation = PhotoList.list[0];
                        }
                        else
                        {
                            PhotoList.index = -1;
                            string workingDirectory = Environment.CurrentDirectory;
                            string path = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\error.png";
                            roomPicBox.ImageLocation = path;
                            //roomPicBox.Size = new System.Drawing.Size(400,150)
                        }
                        reader1.Close();
                        MySqlCommand comm2 = con.CreateCommand();
                        comm2.CommandText = "select * from hot_rooms where room_ID = @roomID1";
                        comm2.Parameters.AddWithValue("@roomID1", room_ID);
                        MySqlDataReader reader2 = comm2.ExecuteReader();
                        while (reader2.Read())
                        {
                            int hasAC = (reader2.GetInt32(3));
                            int canSmoke = (reader2.GetInt32(4));
                            int hasHeater = (reader2.GetInt32(5));
                            int forFamily = (reader2.GetInt32(6));
                            int forAmea = (reader2.GetInt32(7));
                            int hasBalcony = (reader2.GetInt32(8));
                            int hasView = (reader2.GetInt32(9));
                            int hasWIFI = (reader2.GetInt32(10));
                            float cost = (reader2.GetFloat(11));
                            cost = cost * nights;
                            if (hasAC == 0) { acLabel.Text = acLabel.Text + " Όχι"; } else { acLabel.Text = acLabel.Text + " Ναι"; }
                            if (canSmoke == 0) { smokersLabel.Text = smokersLabel.Text + " Όχι"; } else { smokersLabel.Text = smokersLabel.Text + " Ναι"; }
                            if (hasHeater == 0) { heaterLabel.Text = heaterLabel.Text + " Όχι"; } else { heaterLabel.Text = heaterLabel.Text + " Ναι"; }
                            if (forFamily == 0) { familyLabel.Text = familyLabel.Text + " Όχι"; } else { familyLabel.Text = familyLabel.Text + " Ναι"; }
                            if (forAmea == 0) { ameaLabel.Text = ameaLabel.Text + " Όχι"; } else { ameaLabel.Text = ameaLabel.Text + " Ναι"; }
                            if (hasBalcony == 0) { balconyLabel.Text = balconyLabel.Text + " Όχι"; } else { balconyLabel.Text = balconyLabel.Text + " Ναι"; }
                            if (hasView == 0) { seaViewLabel.Text = seaViewLabel.Text + " Όχι"; } else { seaViewLabel.Text = seaViewLabel.Text + " Ναι"; }
                            if (hasWIFI == 0) { wifiLabel.Text = wifiLabel.Text + " Όχι"; } else { wifiLabel.Text = wifiLabel.Text + " Ναι"; }
                            if (nights == 1) { costLabel.Text = costLabel.Text + nights + " βράδυ: " + cost.ToString() + " €"; } else { costLabel.Text = costLabel.Text + nights + " βράδια: " + cost.ToString() + " €"; }
                        }
                        reader2.Close();
                        con.Close();
                    }
                    catch (MySqlException)
                    {
                        MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων.Κωδικος:9 Η εφαρμογή θα τερματιστεί.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
            }
            else
            {
                MessageBox.Show("Πρέπει να επιλέξεις ένα δωμάτιο!.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void resetText()
        {
            acLabel.Text = "Κλιματισμός:";
            smokersLabel.Text = "Χώρος Καπνιστών:";
            balconyLabel.Text = "Μπαλκόνι:";
            heaterLabel.Text = "Θέρμανση:";
            familyLabel.Text = "Οικογενειακό:";
            ameaLabel.Text = "Κατάλληλο για ΑμεΑ:";
            seaViewLabel.Text = "Θέα στην θάλασσα:";
            wifiLabel.Text = "Δωρεάν Wi-Fi:";
            costLabel.Text = "Συνολικό κόστος για ";
            commentTextBox.Text = "";
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            roomDetailsPanel.Hide();
            searchRoomDatePanel.Show();
            roomPicBox.Image = null;
        }

        private void prevImage_Click(object sender, EventArgs e)
        {
            string workingDirectory = Environment.CurrentDirectory;
            workingDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\Rooms\\";

            if (PhotoList.index == 0)
            {
            }
            else if (PhotoList.index == -1) { }
            else
            {

                roomPicBox.ImageLocation = PhotoList.list[PhotoList.index - 1];
                PhotoList.index = PhotoList.index - 1;
            }

        }

        private void nextImage_Click(object sender, EventArgs e)
        {
            string workingDirectory = Environment.CurrentDirectory;
            workingDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\Rooms\\";
            if (PhotoList.index == PhotoList.list.Count() - 1)
            {
            }
            else if (PhotoList.index == -1) { }
            else
            {
                roomPicBox.ImageLocation = PhotoList.list[PhotoList.index + 1];
                PhotoList.index = PhotoList.index + 1;
            }
        }

        private void bookBtn_Click(object sender, EventArgs e)
        {

            DateTime selectedDate1 = dateTimePicker1.Value.Date;
            DateTime selectedDate2 = dateTimePicker2.Value.Date;
            DialogResult loginResult = MessageBox.Show("Είσαι σίγουρος ότι θέλεις να πραγματοποιήσεις την κράτηση με ημερομηνία Check-in:" + selectedDate1.ToShortDateString() + " και ημερομηνία Check-out: " +
                selectedDate2.ToShortDateString() + ";", "Επιβεβαίωση Κράτησης", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk); ;
            if (loginResult == DialogResult.Yes)
            {
                TimeSpan difference = selectedDate2 - selectedDate1; // Subtract the dates
                int nights = (int)difference.Days;
                DataGridViewRow selectedRow = dataGridView4.SelectedRows[0];
                DataGridViewCell cell = selectedRow.Cells[0];
                object value = cell.Value;
                string room_ID;
                if (value != null)
                {

                    room_ID = value.ToString();
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string checkInDate = selectedDate1.ToString("yyyy-MM-dd");
                    string checkOutDate = selectedDate2.ToString("yyyy-MM-dd");
                    DB db = new DB();
                    try
                    {
                        MySqlConnection con = new MySqlConnection(db.getConnString());
                        con.Open();

                        MySqlCommand comm_cost = con.CreateCommand();
                        comm_cost.CommandText = "select room_Price from hot_rooms where room_ID = @roomID";
                        comm_cost.Parameters.AddWithValue("@roomID", room_ID);
                        MySqlDataReader reader = comm_cost.ExecuteReader();
                        float nightCost = 0;
                        if (reader.Read())
                        {
                            string tmp = reader.GetString(0);
                            if (float.TryParse(tmp, out nightCost))
                            {
                                nightCost = (float)Math.Round(nightCost, 2);
                            }
                        }
                        reader.Close();
                        MySqlCommand comm = con.CreateCommand();
                        if (string.IsNullOrEmpty(commentTextBox.Text))
                        {
                            comm.CommandText = "INSERT INTO hot_bookings (usr_ID, room_ID, booking_Date, checkin_Date, checkout_Date, booking_cost) VALUES " +
                            "(@userID, @roomID, @bookingDate,@checkIn,@checkOut,@bookingCost)";
                            comm.Parameters.AddWithValue("@userID", userID);
                            comm.Parameters.AddWithValue("@roomID", room_ID);
                            comm.Parameters.AddWithValue("@bookingDate", currentDate);
                            comm.Parameters.AddWithValue("@checkIn", checkInDate);
                            comm.Parameters.AddWithValue("@checkOut", checkOutDate);
                            comm.Parameters.AddWithValue("@bookingCost", nights * nightCost);
                            comm.ExecuteNonQuery();
                        }
                        else
                        {
                            comm.CommandText = "INSERT INTO hot_bookings (usr_ID, room_ID, booking_Date, checkin_Date, checkout_Date, booking_Comments, booking_cost) VALUES " +
                            "(@userID, @roomID, @bookingDate,@checkIn,@checkOut,@bookComm,@bookingCost)";
                            comm.Parameters.AddWithValue("@userID", userID);
                            comm.Parameters.AddWithValue("@roomID", room_ID);
                            comm.Parameters.AddWithValue("@bookingDate", currentDate);
                            comm.Parameters.AddWithValue("@checkIn", checkInDate);
                            comm.Parameters.AddWithValue("@checkOut", checkOutDate);
                            comm.Parameters.AddWithValue("@bookComm", commentTextBox.Text);
                            comm.Parameters.AddWithValue("@bookingCost", nights * nightCost);
                            comm.ExecuteNonQuery();
                        }
                        MessageBox.Show("Καταχωρήθηκε επιτυχώς η κράτησή σας!", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        roomDetailsPanel.Hide();
                        custHomePanel.Show();
                        con.Close();
                    }
                    catch (MySqlException)
                    {
                        MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων!Κωδικος:9", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            showRoomBtn.Hide();
            resultsLabelCust.Hide();
            dataGridView4.Hide();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            showRoomBtn.Hide();
            resultsLabelCust.Hide();
            dataGridView4.Hide();
        }

        private void bookingsCustBtn_Click_1(object sender, EventArgs e)
        {
            custHomePanel.Hide();
            searchRoomDatePanel.Hide();
            roomDetailsPanel.Hide();
            custBookingsPanel.Show();
            custDetailsPanel.Hide();
            DB db = new DB();
            try
            {
                MySqlConnection con = new MySqlConnection(db.getConnString());
                con.Open();

                MySqlCommand comm1 = con.CreateCommand();
                comm1.CommandText = "SELECT booking_ID as \"Αριθμός Κράτησης\",room_ID AS \"Αριθμός Δωματίου\", CAST(booking_Date AS DATE) AS \"Ημ/νία Κράτησης\", CAST(checkin_Date AS DATE) AS " +
                    "\"Ημ/νία Check-in\"\r\n,cast(checkout_Date AS DATE) AS \"Ημ/νία Check-out\", ifnull(booking_Comments,'') AS \"Σχόλια Κράτησης\",\r\nCONCAT(booking_cost,'€') " +
                    "AS \"Συνολικό κόστος\", \r\nCASE\r\n    WHEN booking_hasPaid = 0 THEN \"Όχι\"\r\n    WHEN booking_hasPaid = 1 THEN \"Ναι\"\r\n" +
                    "END\r\n    AS \"Πληρωμένο\"\r\nfrom hot_bookings where usr_ID = @userID AND hasArrived = 0";
                comm1.Parameters.AddWithValue("@userID", userID);
                DataTable dataTable1 = new DataTable();
                using (MySqlDataReader reader1 = comm1.ExecuteReader())
                {
                    if (reader1.HasRows)
                    {
                        noCurrResvLabel.Hide();
                        dataGridView5.Show();
                        dataTable1.Load(reader1);
                        dataGridView5.DataSource = dataTable1;
                    }
                    else
                    {
                        dataGridView5.Hide();
                        noCurrResvLabel.Show();
                    }
                }

                MySqlCommand comm2 = con.CreateCommand();
                comm2.CommandText = "SELECT booking_ID as \"Αριθμός Κράτησης\",room_ID AS \"Αριθμός Δωματίου\", CAST(booking_Date AS DATE) AS \"Ημ/νία Κράτησης\", CAST(checkin_Date AS DATE) AS " +
                    "\"Ημ/νία Check-in\"\r\n,CAST(checkout_Date AS DATE) AS \"Ημ/νία Check-out\", ifnull(booking_Comments,'') AS \"Σχόλια Κράτησης\",\r\nCONCAT(booking_cost,'€') " +
                    "AS \"Συνολικό κόστος\", \r\nCASE\r\n    WHEN booking_hasPaid = 0 THEN \"Όχι\"\r\n    WHEN booking_hasPaid = 1 THEN \"Ναι\"\r\n" +
                    "END\r\n    AS \"Πληρωμένο\"\r\nfrom hot_bookings where usr_ID = @userID AND hasDeparted = 1";
                comm2.Parameters.AddWithValue("@userID", userID);
                DataTable dataTable2 = new DataTable();
                using (MySqlDataReader reader2 = comm2.ExecuteReader())
                {
                    if (reader2.HasRows)
                    {
                        noPrevResvLabel.Hide();
                        dataGridView6.Show();
                        dataTable2.Load(reader2);
                        dataGridView6.DataSource = dataTable2;
                    }
                    else
                    {
                        dataGridView6.Hide();
                        noPrevResvLabel.Show();
                    }
                }
                con.Close();
            }
            catch (MySqlException)
            {
                MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων.Κωδικος:10 Η εφαρμογή θα τερματιστεί.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelBooking_Click(object sender, EventArgs e)
        {
            if (dataGridView5.Rows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView5.CurrentRow;
                DataGridViewCell cell1 = selectedRow.Cells[0];
                object value1 = cell1.Value;
                DataGridViewCell cell2 = selectedRow.Cells[3];
                object value2 = cell2.Value;
                string booking_ID,dateValue;
                if (value1 != null && value2 != null)
                {
                    booking_ID = value1.ToString();
                    dateValue = value2.ToString();
                    DateTime checkinDate = Convert.ToDateTime(dateValue);
                    DateTime currentDate = DateTime.Today;
                    TimeSpan difference = checkinDate  - currentDate;
                    int daysDifference = difference.Days;
                    if (daysDifference <= 3) 
                    {
                        MessageBox.Show("Δεν μπορεί να γίνει ακύρωση κράτησης για κρατήσεις που το Check-in είναι σε λιγότερο από 3 μέρες ή έχει περάσει.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        DB db = new DB();
                        try
                        {
                            MySqlConnection con = new MySqlConnection(db.getConnString());
                            con.Open();
                            MySqlCommand comm = con.CreateCommand();
                            comm.CommandText = "DELETE FROM hot_bookings WHERE booking_ID = @bookingID";
                            comm.Parameters.AddWithValue("bookingID", booking_ID);
                            comm.ExecuteNonQuery();
                            MessageBox.Show("Η κράτησή σας ακυρώθηκε επιτυχώς!", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            bookingsCustBtn_Click_1(this, EventArgs.Empty);
                            con.Close();
                        }
                        catch (MySqlException)
                        {
                            MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων!Κωδικος:11", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void saveChanges_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            try
            {
                MySqlConnection con = new MySqlConnection(db.getConnString());
                con.Open();
                MySqlCommand comm = con.CreateCommand();
                comm.CommandText = "UPDATE hot_usr INNER JOIN hot_usr_det ON hot_usr.usr_ID = hot_usr_det.usr_ID SET hot_usr.usr_ID = @user_ID" +
                    ", usr_FName = @fname, usr_LName = @lname, usr_username = @username, usr_Email = @email, usrID_Street = @street, " +
                    " usrID_Region = @region, usrID_Country = @country, usrID_Phone = @phone where hot_usr.usr_ID = @user_ID";

                comm.Parameters.AddWithValue("@user_ID", userID);
                comm.Parameters.AddWithValue("@fname", nameBox.Text);
                comm.Parameters.AddWithValue("@lname", surnameBox.Text);
                comm.Parameters.AddWithValue("@username", usernameBox.Text);
                comm.Parameters.AddWithValue("@email", emailBox.Text);
                comm.Parameters.AddWithValue("@street", addressBox.Text);
                comm.Parameters.AddWithValue("@region", regionBox.Text);
                comm.Parameters.AddWithValue("@country", countryBox.Text);
                comm.Parameters.AddWithValue("@phone", phoneBox.Text);
                comm.ExecuteNonQuery();
                MessageBox.Show("Οι αλλαγές σας πραγματοποιήθηκαν.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
            }catch (MySqlException)
            {
                MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων.Κωδικος:12", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void payBookingBtn_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView5.CurrentRow;
            DataGridViewCell cell1 = selectedRow.Cells[0];
            object value1 = cell1.Value;
            DataGridViewCell cell2 = selectedRow.Cells[6];
            object value2 = cell2.Value;
            DataGridViewCell cell3 = selectedRow.Cells[7];
            object value3 = cell3.Value;
            string booking_ID, cost, hasPaid;
            if (value1 != null && value2 != null && value3 != null)
            {
                booking_ID = value1.ToString();
                cost = value2.ToString();
                hasPaid = value3.ToString();
                if (hasPaid.Equals("Ναι"))
                {
                    MessageBox.Show("Έχει ήδη πραγματοποιηθεί πληρωμή για αυτή την κράτηση!", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    bookingPaymentLabel.Text = bookingPaymentLabel.Text + booking_ID;
                    ammountLabel.Text = ammountLabel.Text + cost;
                    custBookingsPanel.Hide();
                    paymentPanel.Show();
                }
            }
        }

        private void backBookingsBtn_Click(object sender, EventArgs e)
        {
            paymentPanel.Hide();
            custBookingsPanel.Show();
            bookingPaymentLabel.Text = "Πληρωμή για την κράτηση με αριθμό: ";
            ammountLabel.Text = "Συνολικό ποσό πληρωμής: ";
        }

        private void payBtn_Click(object sender, EventArgs e)
        {
            string creditNumRegex = @"^\d{16}$";
            string CVRegex = @"^\d{3,4}$";
            string expiryRegex = @"^(0[1-9]|1[0-2])\/\d{2}$";
            string nameRegex  = @"^\S+\s+\S+$";
            if (Regex.IsMatch(creditNumBox.Text, creditNumRegex))
            {
                if (Regex.IsMatch(CVBox.Text, CVRegex))
                {
                    if (Regex.IsMatch(expireBox.Text, expiryRegex))
                    {
                        if (Regex.IsMatch(beneficiaryBox.Text, nameRegex))
                        {
                            string booking_ID;
                            string pattern = @"\d+"; // Regular expression pattern to match digits
                            Match match = Regex.Match(bookingPaymentLabel.Text, pattern);

                            if (match.Success)
                            {
                                booking_ID = match.Value; // Returns the matched digits as a string
                            }
                            else
                            {
                                return; // No digits found
                            }
                            DB db = new DB();
                            try
                            {
                                MySqlConnection con = new MySqlConnection(db.getConnString());
                                con.Open();
                                MySqlCommand comm = con.CreateCommand();
                                comm.CommandText = "UPDATE hot_bookings set booking_hasPaid = 1 where booking_ID = @bookID";
                                comm.Parameters.AddWithValue("@bookID", booking_ID);
                                comm.ExecuteNonQuery();
                                MessageBox.Show("Η πληρωμή της κράτησης έχει πραγματοποιηθεί επιτυχώς!.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                paymentPanel.Hide();
                                bookingsCustBtn_Click_1(this,EventArgs.Empty);
                                con.Close();
                            }
                            catch (MySqlException)
                            {
                                MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων.Κωδικος:13", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Το ονοματεπώνυμο που δώσατε δεν είναι έγκυρο. Πρέπει να δοθεί τουλάχιστον το όνομα και το επίθετο του κατόχου της κάρτας.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Η ημερομηνία λήξης θα πρέπει να είναι της μορφής '12/25' (μμ/εε).", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Ο κωδικός ασφαλείας CV θα πρέπει να αποτελείται από 3 με 4 ψηφία.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Ο αριθμός της κάρτας σας θα πρέπει να εμπεριέχει αυστηρά 16 ψηφία.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void logoutBtnAdmin_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void searchBackBtn_Click(object sender, EventArgs e)
        {
            resultsPanel.Hide();
            homePanel.Hide();
            searchPanel.Show();
        }
    } 
}