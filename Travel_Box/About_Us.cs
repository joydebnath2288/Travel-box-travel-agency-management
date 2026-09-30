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
    public partial class About_Us : Form
    {
        string customer_email, location;
        public About_Us(string customer_email, string location)
        {
            this.customer_email = customer_email;
            this.location = location;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Customer_Packages Customer_packages = new Customer_Packages(customer_email, location);
            Customer_packages.Show();
            this.Hide();
            Customer_packages.FormClosed += (s, args) => this.Close();
        }
    }
}
