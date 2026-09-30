using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Travel_Box
{
    public partial class Booked_Packages : Form
    {
        string agency_email;
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");
        public Booked_Packages(string agency_email)
        {
            InitializeComponent();
            this.agency_email = agency_email;
            LoadAllBookings();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.KeyDown += dataGridView1_KeyDown;

        }
        private void LoadAllBookings()
        {
            try
            {
                con.Open();

                string query = @"
    SELECT * 
    FROM booking 
    WHERE package_id IN (
        SELECT package_id 
        FROM tour_packages 
        WHERE agency_id = (
            SELECT agency_id 
            FROM agency 
            WHERE agency_email = @Email
        )
    )";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", agency_email);

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
            Agency_Dashboard Agency_Dashboard = new Agency_Dashboard(agency_email);
            Agency_Dashboard.Show();
            this.Hide();
            Agency_Dashboard.FormClosed += (s, args) => this.Close();
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
                    string query =@" SELECT TOP(@topN)
    COUNT(b.package_id) AS number_of_bookings,
    b.package_id, 
    tp.package_name,
    tp.destination,
    tp.price_per_person,
    tp.duration
FROM booking b
JOIN tour_packages tp
    ON b.package_id = tp.package_id
WHERE tp.package_id IN(
    SELECT package_id
    FROM tour_packages
    WHERE agency_id = (
        SELECT agency_id
        FROM agency
        WHERE agency_email = @Email
    )
)
GROUP BY b.package_id, tp.package_name, tp.destination, tp.price_per_person, tp.duration
ORDER BY COUNT(b.package_id) DESC";

                    // Prepare the SQL command
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@topN", topN);
                    cmd.Parameters.AddWithValue("@Email", agency_email);

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

        private void button4_Click(object sender, EventArgs e)
        {
            LoadAllBookings();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string bookingId = null;

            // Check if a row is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                bookingId = dataGridView1.SelectedRows[0].Cells["booking_id"].Value?.ToString();
            }
            else if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                // If no row is selected, fallback to the ID in the TextBox
                bookingId = textBox2.Text;
            }

            // If neither a row is selected nor a valid ID is provided in the TextBox
            if (string.IsNullOrEmpty(bookingId))
            {
                MessageBox.Show("Please select a row or fill the booking ID.");
                return;
            }

            // Use a 'using' block to ensure the connection is closed automatically
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;"))
            {
                try
                {
                    con.Open(); // Open connection

                    SqlCommand cmd = new SqlCommand("DELETE FROM booking WHERE booking_id = @id", con);
                    cmd.Parameters.AddWithValue("@id", bookingId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Booking deleted successfully.");
                        LoadAllBookings(); // Refresh the grid after deletion
                    }
                    else
                    {
                        MessageBox.Show("No booking found with the provided ID.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Get booking_id from the selected row
                    string bookingId = dataGridView1.SelectedRows[0].Cells["booking_id"].Value.ToString();

                    // Confirm with the user before deleting
                    DialogResult result = MessageBox.Show($"Are you sure you want to delete booking ID {bookingId}?",
                                                          "Confirm Delete",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        // Use 'using' block for connection management
                        using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;"))
                        {
                            try
                            {
                                con.Open(); // Open connection

                                SqlCommand cmd = new SqlCommand("DELETE FROM booking WHERE booking_id = @id", con);
                                cmd.Parameters.AddWithValue("@id", bookingId);

                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Booking deleted successfully.");
                                    LoadAllBookings(); // Refresh the grid after deletion
                                }
                                else
                                {
                                    MessageBox.Show("No booking found with the provided ID.");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("An error occurred: " + ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to delete.");
                }
            }
        }

    }
}
