using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Travel_Box
{
    
    public partial class Admin_Dashboard : Form
    {
        private string admin_email;
        public Admin_Dashboard(string admin_email)
        {
            this.admin_email = admin_email;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Manage_Agency Manage_Agency = new Manage_Agency(admin_email);
            Manage_Agency.Show();
            this.Hide();
            Manage_Agency.FormClosed += (s, args) => this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login Login = new Login();
            Login.Show();
            this.Hide();
            Login.FormClosed += (s, args) => this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Manage_Customer Manage_Customer = new Manage_Customer(admin_email);
            Manage_Customer.Show();
            this.Hide();
            Manage_Customer.FormClosed += (s, args) => this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Admin_Booked_Packages Admin_Booked_Packages = new Admin_Booked_Packages(admin_email);
            Admin_Booked_Packages.Show();
            this.Hide();
            Admin_Booked_Packages.FormClosed += (s, args) => this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Manage_Admin Manage_Admin = new Manage_Admin(admin_email);
            Manage_Admin.Show();
            this.Hide();
            Manage_Admin.FormClosed += (s, args) => this.Close();
        }
    }
}
