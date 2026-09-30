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
    public partial class Admin_Booked_Packages : Form
    {
        string admin_email;
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");
        public Admin_Booked_Packages(string admin_email)
        {
            this.admin_email = admin_email;
            InitializeComponent();
            LoadAllBookings();
        }

        private void LoadAllBookings()
        {
            try
            {
                con.Open();

                string query = @"
    SELECT * 
    FROM booking";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                SqlCommand cmd = new SqlCommand(query, con);

                DataTable dt = new DataTable();
                sda.SelectCommand = cmd; // Assign the command with parameters
                sda.Fill(dt);

                dataGridView1.DataSource = dt;  // Assuming you have a DataGridView control named 'dataGridView1'
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Admin_Dashboard Admin_Dashboard = new Admin_Dashboard(admin_email);
            Admin_Dashboard.Show();
            this.Hide();
            Admin_Dashboard.FormClosed += (s, args) => this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadAllBookings();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please fill booking id.");
                return;
            }

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM booking WHERE booking_id= @id", con);
                cmd.Parameters.AddWithValue("@id", textBox2.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Booking information found successfully.");
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                }
                else
                {
                    MessageBox.Show("No record found with the provided Booking ID.");
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
            {
                // Validate if input is a valid number
                if (!int.TryParse(textBox1.Text, out int topN))
                {
                    MessageBox.Show("Please enter a valid number.");
                    return;
                }

                try
                {
                    // Open the connection
                    con.Open();

                    // SQL Query to get top N most booked packages
                    string query = @"
    SELECT TOP (@topN) 
        COUNT(b.package_id) AS number_of_bookings,
        b.package_id, 
        tp.package_name,
        tp.destination,
        tp.price_per_person,
        tp.duration
    FROM booking b
    JOIN tour_packages tp
        ON b.package_id = tp.package_id
    GROUP BY b.package_id, tp.package_name, tp.destination, tp.price_per_person, tp.duration
    ORDER BY COUNT(b.package_id) DESC";

                    // Prepare the SQL command
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@topN", topN);

                    // Execute and fill the data into a DataTable
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    // Assuming you have a DataGridView control named 'dataGridViewTopPackages'
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("SQL Error: " + ex.Message);
                }
                finally
                {
                    // Close the connection
                    con.Close();
                }
            }
        }
    }
}
