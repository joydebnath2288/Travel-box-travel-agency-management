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
    public partial class Customer_Account_Information : Form
    {
        string customer_email, location;
        public Customer_Account_Information(string customer_email, string location)
        {
            this.customer_email = customer_email;
            this.location = location;
            InitializeComponent();
            LoadAllCustomerData();
        }
        private void LoadAllCustomerData()
        {
            // Connection string (modify it according to your setup)
            string connectionString = "Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;";
            string query = "SELECT * FROM customer WHERE email = @Email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", customer_email);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Clear panel before adding new data
                panel1.Controls.Clear();

                int yPosition = 10; // Starting position for labels

                while (reader.Read())
                {
                    // First name
                    Label lblFirstName = new Label
                    {
                        Text = "First Name: " + reader["first_name"].ToString(),
                        Location = new Point(10, yPosition),
                        Font = new Font("Arial", 12, FontStyle.Regular),
                        AutoSize = true
                    };
                    panel1.Controls.Add(lblFirstName);
                    yPosition += 30;

                    // Last name
                    Label lblLastName = new Label
                    {
                        Text = "Last Name: " + reader["last_name"].ToString(),
                        Location = new Point(10, yPosition),
                        Font = new Font("Arial", 12, FontStyle.Regular),
                        AutoSize = true
                    };
                    panel1.Controls.Add(lblLastName);
                    yPosition += 30;

                    // Gender
                    Label lblGender = new Label
                    {
                        Text = "Gender: " + reader["gender"].ToString(),
                        Location = new Point(10, yPosition),
                        Font = new Font("Arial", 12, FontStyle.Regular),
                        AutoSize = true
                    };
                    panel1.Controls.Add(lblGender);
                    yPosition += 30;

                    // Phone Number
                    Label lblPhoneNumber = new Label
                    {
                        Text = "Phone Number: " + reader["phone_number"].ToString(),
                        Location = new Point(10, yPosition),
                        Font = new Font("Arial", 12, FontStyle.Regular),
                        AutoSize = true
                    };
                    panel1.Controls.Add(lblPhoneNumber);
                    yPosition += 30;

                    // Email
                    Label lblEmail = new Label
                    {
                        Text = "Email: " + reader["email"].ToString(),
                        Location = new Point(10, yPosition),
                        Font = new Font("Arial", 12, FontStyle.Regular),
                        AutoSize = true
                    };
                    panel1.Controls.Add(lblEmail);
                    yPosition += 30;

                    // Country
                    Label lblCountry = new Label
                    {
                        Text = "Country: " + reader["country"].ToString(),
                        Location = new Point(10, yPosition),
                        Font = new Font("Arial", 12, FontStyle.Regular),
                        AutoSize = true

                    };
                    panel1.Controls.Add(lblCountry);
                    yPosition += 30;

                    
                }

                reader.Close();
            }
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
