using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Travel_Box
{
    public partial class Customer_Book : Form
    {
        string customer_email;
        string location;
        string packageId;
        string query, query2;
        DateTime journeyDate;
        float total_amount;
        string price_per_person;
        int verticalPosition;
        int lowestCustomerCapacity;
        int highestCustomerCapacity;
        float pricePerPerson;
        int numPersons;
        string date;
        bool isButton4Clicked = false;
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");

        public Customer_Book(string customer_email, string location, string packageId)
        {
            this.customer_email = customer_email;
            this.location = location;
            this.packageId = packageId;
            this.Load += new System.EventHandler(this.Package_Details_Load);
            InitializeComponent();
        }

        private void Package_Details_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                con.Open();

                // Retrieve customer information first
                query2 = @"SELECT first_name, last_name, phone_number
                   FROM customer
                   WHERE email = @Email";

                SqlCommand cmd2 = new SqlCommand(query2, con);
                cmd2.Parameters.AddWithValue("@Email", customer_email);
                SqlDataReader reader2 = cmd2.ExecuteReader();

                if (reader2.HasRows)
                {
                    panel2.Controls.Clear(); // Clear existing controls in panel2 to avoid duplicates
                    panel2.BackColor = Color.FromArgb(50, Color.White);
                    int verticalPosition = 20; // Starting Y position for the first label

                    while (reader2.Read())
                    {
                        // Add Customer Name Label
                        Label lblCustomerName = new Label
                        {
                            Text = "Customer Name: " + reader2["first_name"].ToString() + " " + reader2["last_name"].ToString(),
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Black,
                            AutoSize = true,
                            Location = new Point(10, verticalPosition)
                        };
                        panel2.Controls.Add(lblCustomerName);
                        verticalPosition += lblCustomerName.Height + 15;

                        // Add Phone Number Label
                        Label lblPhoneNumber = new Label
                        {
                            Text = "Phone Number: " + reader2["phone_number"].ToString(),
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Black,
                            AutoSize = true,
                            Location = new Point(10, verticalPosition)
                        };
                        panel2.Controls.Add(lblPhoneNumber);
                        verticalPosition += lblPhoneNumber.Height + 15;
                    }
                }
                else
                {
                    MessageBox.Show("No customer data found.");
                }

                reader2.Close();

                // Now retrieve package details for the given package ID
                query = @"SELECT package_id, package_name, destination, price_per_person, duration, refund_stutus,lowest_customer_capacity, highest_customer_capacity
                  FROM tour_packages 
                  WHERE package_id = @PackageId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@PackageId", packageId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    

                    while (reader.Read())
                    {
                        verticalPosition = panel2.Controls.Count > 0 ? panel2.Controls[panel2.Controls.Count - 1].Bottom + 15 : 10;


                        lowestCustomerCapacity = reader.GetInt32(reader.GetOrdinal("lowest_customer_capacity"));
                        highestCustomerCapacity = reader.GetInt32(reader.GetOrdinal("highest_customer_capacity"));
                        // Add Package Name Label
                        Label lblPackageName = new Label
                        {
                            Text = "Package Name: " + reader["package_name"].ToString(),
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Black,
                            AutoSize = false,
                            MaximumSize = new Size(panel2.Width - 10, 0),  // Set max width for wrapping
                            Size = new Size(panel2.Width - 10, 0),
                            Location = new Point(10, verticalPosition)
                        };
                        // Set the label height dynamically based on its content
                        lblPackageName.Height = TextRenderer.MeasureText(lblPackageName.Text, lblPackageName.Font, lblPackageName.Size, TextFormatFlags.WordBreak).Height;
                        panel2.Controls.Add(lblPackageName);

                        panel2.Controls.Add(lblPackageName);
                        verticalPosition += lblPackageName.Height + 15;

                        // Add Destination Label
                        Label lblDestination = new Label
                        {
                            Text = "Destination: " + reader["destination"].ToString(),
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Black,
                            AutoSize = true,
                            Location = new Point(10, verticalPosition)
                        };
                        panel2.Controls.Add(lblDestination);
                        verticalPosition += lblDestination.Height + 15;

                        // Add Duration Label
                        Label lblDuration = new Label
                        {
                            Text = "Duration: " + reader["duration"].ToString() + " days",
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Black,
                            AutoSize = true,
                            Location = new Point(10, verticalPosition)
                        };
                        panel2.Controls.Add(lblDuration);
                        verticalPosition += lblDuration.Height + 15;

                        // Add Refund Status Label
                        Label lblRefundStatus = new Label
                        {
                            Text = "Refund Policy: " + reader["refund_stutus"].ToString(),
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Black,
                            AutoSize = true,
                            Location = new Point(10, verticalPosition)
                        };
                        panel2.Controls.Add(lblRefundStatus);
                        verticalPosition += lblRefundStatus.Height + 15;

                        price_per_person = reader["price_per_person"].ToString();
                        pricePerPerson = float.Parse(reader["price_per_person"].ToString());


                        // Add Price Per Person Label
                        Label lblPricePerPerson = new Label
                        {
                            Text = "Price Per Person: " + reader["price_per_person"].ToString() + " BDT",
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Black,
                            AutoSize = true,
                            Location = new Point(10, verticalPosition)
                        };
                        panel2.Controls.Add(lblPricePerPerson);
                        verticalPosition += lblPricePerPerson.Height + 15;
                    }
                }
                else
                {
                    MessageBox.Show("No data found for this package.");
                }

                reader.Close();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("SQL Error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("General Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {


            // Clear previously added labels to avoid duplicates
            panel2.Controls.Clear();
            
                LoadData();


            // Validate number of persons input
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter the number of persons.");
                return;
            }

            if (!int.TryParse(textBox3.Text, out numPersons))
            {
                MessageBox.Show("Please enter a valid number of persons.");
                return;
            }

            if (numPersons < lowestCustomerCapacity)
            {
                MessageBox.Show("Minimum number of persons for this package booking is " + lowestCustomerCapacity);
                return;
            }

            if (numPersons > highestCustomerCapacity)
            {
                MessageBox.Show("Maximum number of persons for this package booking is " + highestCustomerCapacity);
                return;
            }

            // Calculate the total amount
            total_amount = pricePerPerson * numPersons;

            // Add the Number of Persons Label
            Label lblNumberOfPersons = new Label
            {
                Text = "Number of Persons: " + numPersons,
                Font = new Font("Arial", 10, FontStyle.Regular),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(10, verticalPosition)
            };
            panel2.Controls.Add(lblNumberOfPersons);
            verticalPosition += lblNumberOfPersons.Height + 15;

            // Add the Total Amount Label
            Label lblTotalAmount = new Label
            {
                Text = "Total Amount: " + total_amount + " BDT",
                Font = new Font("Arial", 10, FontStyle.Regular),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(10, verticalPosition)
            };
            panel2.Controls.Add(lblTotalAmount);
            verticalPosition += lblTotalAmount.Height + 15;

            journeyDate= dateTimePicker1.Value;
            date = journeyDate.ToString("dddd, MMMM dd, yyyy");
            // Add the Journey Date Label
            Label lblJourneyDate = new Label
            {
                Text = "Journey Date: " + date,
                Font = new Font("Arial", 10, FontStyle.Regular),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(10, verticalPosition)
            };
            panel2.Controls.Add(lblJourneyDate);
            isButton4Clicked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!isButton4Clicked)
            {
                MessageBox.Show("Please Genarate Your Bill.");
                return; // Prevent further execution
            }
            Customer_Mobile_Banking Customer_Mobile_Banking = new Customer_Mobile_Banking(customer_email, location, packageId, journeyDate, numPersons, total_amount);
            Customer_Mobile_Banking.Show();
            this.Hide();
            Customer_Mobile_Banking.FormClosed += (s, args) => this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isButton4Clicked)
            {
                MessageBox.Show("Please Genarate Your Bill.");
                return; // Prevent further execution
            }
            Customer_Card_Payment Customer_Card_Payment = new Customer_Card_Payment(customer_email, location, packageId, journeyDate, numPersons, total_amount);
            Customer_Card_Payment.Show();
            this.Hide();
            Customer_Card_Payment.FormClosed += (s, args) => this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!isButton4Clicked)
            {
                MessageBox.Show("Please Genarate Your Bill.");
                return; // Prevent further execution
            }
            Customer_Bank_Payment Customer_Bank_Payment = new Customer_Bank_Payment(customer_email, location, packageId, journeyDate, numPersons, total_amount);
            Customer_Bank_Payment.Show();
            this.Hide();
            Customer_Bank_Payment.FormClosed += (s, args) => this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Customer_Package_Details Customer_Package_Details = new Customer_Package_Details(customer_email, location, packageId);
            Customer_Package_Details.Show();
            this.Hide();
            Customer_Package_Details.FormClosed += (s, args) => this.Close();
        }
    }
}
