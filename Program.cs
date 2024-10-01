using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagement
{
    static class Program
    {
        static int userID = -1; // Initialize with an invalid value
        static Login loginForm;

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            loginForm = new Login();
            DialogResult loginResult = loginForm.ShowDialog();
            if (loginResult == DialogResult.OK)
            {
                userID = loginForm.getID();
                MainMenu m = new MainMenu(userID);
                Application.Run(m);
            }
            else
            {
                return;
            }
        }
    }
}