using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;


namespace HotelManagement
{
    public partial class Signup : Form
    {
        public Signup()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            countryBox.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = fNameBox.Text;
            string surname = lNameBox.Text;
            string username = userBox.Text;
            string email = emailBox.Text;
            string pass1 = pass1Box.Text;
            string pass2 = pass2Box.Text;
            DB cs = new DB();
            int error = 0;
            error = checkValidity(name,surname,username,email,pass1,pass2);
            switch (error)
            {
                case 0:
                    MessageBox.Show("Οι κωδικοί πρόσβασης δεν είναι οι ίδιοι!", "Λανθασμένα Στοιχεία", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -1: case -2:
                    MessageBox.Show("Το όνομα και το επώνυμο πρέπει να αποτελούνται αποκλειστικά από χαρακτήρες του ελληνικού ή του αγγλικού" +
                        " αλφάβητου.", "Λανθασμένα Στοιχεία", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -3:
                    MessageBox.Show("Το email δεν βρίσκεται στον σωστό μορφότυπο!", "Λανθασμένα Στοιχεία", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -4:
                    MessageBox.Show("Το πειδίο της διεύθυνσης δεν μπορεί να είναι κενό!", "Λανθασμένα Στοιχεία", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -5:
                    MessageBox.Show("Το πειδίο της περιοχής δεν μπορεί να είναι κενό!", "Λανθασμένα Στοιχεία", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -6:
                    MessageBox.Show("Το πειδίο της χώρας δεν μπορεί να είναι κενό!", "Λανθασμένα Στοιχεία", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -7:
                    MessageBox.Show("Το πειδίο του κινητού τηλεφώνου δεν μπορεί να είναι κενό!", "Λανθασμένα Στοιχεία", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case 1:
                    MySqlConnection con = new MySqlConnection(cs.getConnString());
                    try
                    {
                        con.Open();
                        MySqlCommand comm = con.CreateCommand();
                        comm.CommandText = "INSERT INTO hot_usr (usr_FName, usr_LName, usr_username, usr_password, usr_Email, usr_Type) VALUES (@name, @surname, @username, @pass, @email, 2)";
                        comm.Parameters.AddWithValue("@name", name);
                        comm.Parameters.AddWithValue("@surname", surname);
                        comm.Parameters.AddWithValue("@username", username);
                        comm.Parameters.AddWithValue("@pass", pass1);
                        comm.Parameters.AddWithValue("@email", email);
                        comm.ExecuteNonQuery();
                        MySqlCommand comm1 = con.CreateCommand();
                        comm1.CommandText = "insert into hot_usr_det (usr_ID, usrID_Street, usrID_Region, usrID_Country, usrID_Phone) " +
                            "values ((select max(usr_ID) from hot_usr), @street, @reg, @country, @phone)";
                        comm1.Parameters.AddWithValue("@street", addressBox.Text);
                        comm1.Parameters.AddWithValue("@reg", regionBox.Text);
                        comm1.Parameters.AddWithValue("@country", countryBox.SelectedItem.ToString());
                        comm1.Parameters.AddWithValue("@phone", phoneBox.Text);
                        comm1.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Η εγγραφή του νέου χρήστη ολοκληρώθηκε επιτυχώς!", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                    }
                    catch (MySqlException a)
                    {
                        MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων. Η εφαρμογή θα τερματιστεί." + a, "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                    break;
            }
            
        }
        int checkValidity(string name, string surname, string username, string email, string pass1, string pass2)
        {
            Regex only_letter_reg = new Regex(@"^[a-zA-ZΑ-Ωα-ωΆΈΉΊΌΎΏάέήίόύώ]+$");
            Regex email_reg = new Regex(@"(^\w|^\d)(\d|\w)*@(\d|\w)*\.(\d|\w)+");
            if (!only_letter_reg.IsMatch(name))
            {
                return -1;
            }
            if (!only_letter_reg.IsMatch(surname))
            {
                return -2;
            }
            if (!email_reg.IsMatch(email))
            {
                return -3;
            }
            if (!(addressBox.Text != null && addressBox.Text != ""))
            {
                return -4;
            }
            if (!(regionBox.Text != null && regionBox.Text != "")) 
            {
                return -5; 
            }
            if (!(countryBox.SelectedIndex >= 0))
            {
                return -6;
            }
            if (!(phoneBox.Text != null && phoneBox.Text != ""))
            {
                return -7;
            }

            if (pass1.Equals(pass2))
                return 1;
            else
                return 0;
        }
    }
}
