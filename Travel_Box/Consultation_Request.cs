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
    public partial class Consultation_Request : Form
    {
        string agency_email;
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");
        public Consultation_Request(string agency_email)
        {
            this.agency_email = agency_email;
            InitializeComponent();
            LoadAllData();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.KeyDown += dataGridView1_KeyDown;
        }

        private void LoadAllData()
        {
            try
            {
                con.Open();

                string query = @"
    SELECT * 
    FROM consultation_requests 
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

        private void button4_Click(object sender, EventArgs e)
        {
            LoadAllData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please fill request id.");
                return;
            }

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM consultation_requests WHERE request_id= @id", con);
                cmd.Parameters.AddWithValue("@id", textBox2.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("information found successfully.");
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                }
                else
                {
                    MessageBox.Show("No record found with the provided request ID.");
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
            string Id = null;

            // Check if a row is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Id = dataGridView1.SelectedRows[0].Cells["request_id"].Value?.ToString();
            }
            else if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                // If no row is selected, fallback to the ID in the TextBox
                Id = textBox2.Text;
            }

            // If neither a row is selected nor a valid ID is provided in the TextBox
            if (string.IsNullOrEmpty(Id))
            {
                MessageBox.Show("Please select a row or fill the ID.");
                return;
            }

            // Use a 'using' block to ensure the connection is closed automatically
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;"))
            {
                try
                {
                    con.Open(); // Open connection

                    SqlCommand cmd = new SqlCommand("DELETE FROM consultation_requests WHERE request_id = @id", con);
                    cmd.Parameters.AddWithValue("@id", Id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Request deleted successfully.");
                        LoadAllData(); // Refresh the grid after deletion
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
                    string Id = dataGridView1.SelectedRows[0].Cells["booking_id"].Value.ToString();

                    // Confirm with the user before deleting
                    DialogResult result = MessageBox.Show($"Are you sure you want to delete booking ID {Id}?",
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

                                SqlCommand cmd = new SqlCommand("SELECT * FROM consultation_requests WHERE request_id= @id", con);
                                cmd.Parameters.AddWithValue("@id", Id);

                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Request deleted successfully.");
                                    LoadAllData(); // Refresh the grid after deletion
                                }
                                else
                                {
                                    MessageBox.Show("No Request found with the provided ID.");
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
