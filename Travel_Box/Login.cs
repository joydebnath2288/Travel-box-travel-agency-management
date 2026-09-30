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

namespace Travel_Box
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
          // panel1.BackColor = Color.FromArgb(130, Color.White);
        }
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                // Create the SQL command to retrieve the password for the given email
                SqlCommand cmd = new SqlCommand("SELECT password FROM customer WHERE email = @Email", con);
                cmd.Parameters.AddWithValue("@Email", textBox1.Text);

                // Execute the command and get the password from the database
                object result = cmd.ExecuteScalar();

               
                SqlCommand cmd2 = new SqlCommand("SELECT password FROM admin WHERE email = @Email", con);
                cmd2.Parameters.AddWithValue("@Email", textBox1.Text);
                object admin_result = cmd2.ExecuteScalar();

                SqlCommand cmd3 = new SqlCommand("SELECT agency_password FROM agency WHERE agency_email = @Email", con);
                cmd3.Parameters.AddWithValue("@Email", textBox1.Text);
                object agency_result = cmd3.ExecuteScalar();


                if (string.IsNullOrWhiteSpace(textBox1.Text) && string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Please enter Email and Password");
                }
                else if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Please enter Email.");
                }
                else if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Please enter password.");
                }
                else
                {
                    if (result != null)
                    {
                        // Convert the result to a string
                        string dbPassword = result.ToString();

                        // Compare the database password with the input in textBox2
                        if (dbPassword == textBox2.Text)
                        {
                            MessageBox.Show("Password matches!");
                            string customer_email = textBox1.Text;
                            Customer_Select_Location Customer_Select_Location = new Customer_Select_Location(customer_email);
                            Customer_Select_Location.Show();
                            this.Hide();
                            Customer_Select_Location.FormClosed += (s, args) => this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Password does not match.");
                        }
                    }

                    else if (admin_result != null)
                    {
                        // Convert the result to a string
                        string dbPassword = admin_result.ToString();

                        // Compare the database password with the input in textBox2
                        if (dbPassword == textBox2.Text)
                        {
                            MessageBox.Show("Password matches for admin!");
                            string admin_email = textBox1.Text;
                            Admin_Dashboard Admin_Dashboard = new Admin_Dashboard(admin_email);
                            Admin_Dashboard.Show();
                            this.Hide();
                            Admin_Dashboard.FormClosed += (s, args) => this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Password does not match for admin.");
                        }
                    }

                    else if (agency_result != null)
                    {
                        // Convert the result to a string
                        string dbPassword = agency_result.ToString();

                        // Compare the database password with the input in textBox2
                        if (dbPassword == textBox2.Text)
                        {
                            MessageBox.Show("Password matches for agency!");
                            string agency_email = textBox1.Text;
                            Agency_Dashboard Agency_Dashboard = new Agency_Dashboard(agency_email);
                            Agency_Dashboard.Show();
                            this.Hide();
                            Agency_Dashboard.FormClosed += (s, args) => this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Password does not match for agency.");
                        }
                    }

                    else
                    {
                        MessageBox.Show("Email not found.");
                    }
                }
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
            SignUpEmail SignUpEmail = new SignUpEmail();


            SignUpEmail.Show();
            this.Hide();
            SignUpEmail.FormClosed += (s, args) => this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Forgot_Password_Email Forgot_Password_Email = new Forgot_Password_Email();
            Forgot_Password_Email.Show();
            this.Hide();
            Forgot_Password_Email.FormClosed += (s, args) => this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
           /* Form1 Form1 = new Form1();
            Form1.Show();
            this.Hide();
            Form1.FormClosed += (s, args) => this.Close();*/
        }
    }
}
