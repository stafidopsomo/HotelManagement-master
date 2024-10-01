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

namespace HotelManagement
{
    public partial class Login : Form
    {

        private int userID;
        public Login()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = userTB.Text;
            string password = passTB.Text;
            DB cs = new DB();
            MySqlConnection con = new MySqlConnection(cs.getConnString());
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from hot_usr where usr_username = '" + username + "' AND usr_password = '" + password + "'", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    MessageBox.Show("Επιτυχής είσοδος!", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    userID = reader.GetInt32("usr_ID");
                    Close();
                }
                else
                {
                    MessageBox.Show("Τα στοιχεία που δώσατε δεν είναι έγκυρα.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //userTB.Text = string.Empty;
                passTB.Text = string.Empty;
            }
            catch (MySqlException)
            {
                MessageBox.Show("Πρόβλημα επικοινωνίας με την βάση δεδομένων. Η εφαρμογή θα τερματιστεί.", "Μήνυμα εφαρμογής", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        public int getID()
        {
            return userID;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Signup s = new Signup();
            s.ShowDialog();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) { 
                passTB.PasswordChar = '\0';
            }
            else
            {
                passTB.PasswordChar = '•';
            }
        }
    }
}
