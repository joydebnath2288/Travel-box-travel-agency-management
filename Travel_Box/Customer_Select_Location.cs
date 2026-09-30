using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace Travel_Box
{
    public partial class Customer_Select_Location : Form
    {
        private string customer_email;
        public Customer_Select_Location(string customer_email)
        {
            this.customer_email = customer_email;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login Login = new Login();
            Login.Show();
            this.Hide();
            Login.FormClosed += (s, args) => this.Close();
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            string location = comboBox1.Text;

            if (string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("Please select your location where you want to travel!");
            }
            else
            {
               
                Customer_Packages Customer_Packages = new Customer_Packages(customer_email,location);
                Customer_Packages.Show();
                this.Hide();
                Customer_Packages.FormClosed += (s, args) => this.Close();
            }
        }

       
    }
}
