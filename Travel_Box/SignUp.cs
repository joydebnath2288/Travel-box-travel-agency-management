using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Travel_Box
{
    public partial class SignUp : Form
    {
        private string to;
        public SignUp(string to)
        {
            this.to = to;
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");
        

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if all fields are filled
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            // Check if password and confirm password match
            if (textBox4.Text != textBox5.Text)
            {
                MessageBox.Show("Password and Confirm Password do not match.");
                return;
            }

            // Check if the checkbox is checked
            if (!checkBox1.Checked)
            {
                MessageBox.Show("Please accept the terms and conditions.");
                return;
            }

            // If all validations pass, proceed with saving the data
            try
            {
                con.Open();

                // Determine gender
                string gender = radioButton1.Checked ? radioButton1.Text : radioButton2.Text;

                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd hh:mm:ss tt");

                // Insert new record
                SqlCommand cmd = new SqlCommand("insert into customer (first_name, last_name, gender, phone_number, email, country, password, date_and_time) values (@first_name, @last_name, @gender, @phone_number, @email, @country, @password, @date_and_time)", con);
                cmd.Parameters.AddWithValue("@first_name", textBox1.Text);
                cmd.Parameters.AddWithValue("@last_name", textBox2.Text);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@phone_number", textBox3.Text);
                cmd.Parameters.AddWithValue("@email", to);
                cmd.Parameters.AddWithValue("@country", comboBox1.Text);
                cmd.Parameters.AddWithValue("@password", textBox4.Text);
                cmd.Parameters.AddWithValue("@date_and_time", formattedDateTime);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully saved");
                Login Login = new Login();
                Login.Show();
                this.Hide();
                Login.FormClosed += (s, args) => this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SignUp_Terms_Conditions SignUp_Terms_Conditions = new SignUp_Terms_Conditions();
            SignUp_Terms_Conditions.Show();
            //this.Hide();
           // SignUp_Terms_Conditions.FormClosed += (s, args) => this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Login Login = new Login();
            Login.Show();
            this.Hide();
            Login.FormClosed += (s, args) => this.Close();
        }
    }
}

