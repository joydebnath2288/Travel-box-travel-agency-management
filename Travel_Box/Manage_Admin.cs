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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Travel_Box
{
    public partial class Manage_Admin : Form
    {
        string admin_email;
        private string selectedAdminId = null;
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");
        public Manage_Admin(string admin_email)
        {
            InitializeComponent();
            this.admin_email = admin_email;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void FetchAllAgencies()
        {
            SqlCommand fetchCmd = new SqlCommand("SELECT * FROM admin", con);
            SqlDataAdapter adapter = new SqlDataAdapter(fetchCmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if any required fields are empty
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please fill in all the required fields.");
                return;
            }

            // Check if Password and Confirm Password match
            if (textBox6.Text != textBox7.Text)
            {
                MessageBox.Show("Password and Confirm Password do not match.");
                return;
            }

            try
            {
                con.Open();

                // Insert new record
                SqlCommand cmd = new SqlCommand("INSERT INTO admin (admin_id, name, email, password) VALUES (@id, @name, @email, @password)", con);
                cmd.Parameters.AddWithValue("@id", textBox1.Text);
                cmd.Parameters.AddWithValue("@name", textBox2.Text);
                cmd.Parameters.AddWithValue("@email", textBox3.Text);
                cmd.Parameters.AddWithValue("@password", textBox6.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully saved");

                // Fetch all records from the table
                FetchAllAgencies();
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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                FetchAllAgencies();
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
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please fill id.");
                return;
            }

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM admin WHERE admin_id = @admin_id", con);
                cmd.Parameters.AddWithValue("@admin_id", textBox1.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Admin information found successfully.");
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                }
                else
                {
                    MessageBox.Show("No record found with the provided Admin ID.");
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
            // Check if any row is selected
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            // Get the selected admin_id from the selected row
            string selectedAdminId = dataGridView1.SelectedRows[0].Cells["admin_id"].Value.ToString();

            DialogResult result = MessageBox.Show("Are you sure you want to delete this admin?", "Delete Confirmation", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    con.Open();

                    // Delete the selected admin
                    SqlCommand cmd = new SqlCommand("DELETE FROM admin WHERE admin_id = @admin_id", con);
                    cmd.Parameters.AddWithValue("@admin_id", selectedAdminId);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Admin deleted successfully.");

                    // Refresh the grid view
                    FetchAllAgencies();
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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please fill id.");
                return;
            }

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE admin SET name = @name, email = @email, password = @password WHERE admin_id = @id", con);

                // Add parameters
                cmd.Parameters.AddWithValue("@id", textBox1.Text);
                cmd.Parameters.AddWithValue("@name", textBox2.Text);
                cmd.Parameters.AddWithValue("@email", textBox3.Text);
                cmd.Parameters.AddWithValue("@password", textBox6.Text);

                // Execute the update command
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Admin information updated successfully.");
                    // Fetch updated data and bind to DataGridView
                    FetchAllAgencies();
                }
                else
                {
                    MessageBox.Show("No record found with the provided Admin ID.");
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

        private void button6_Click(object sender, EventArgs e)
        {
            Admin_Dashboard Admin_Dashboard = new Admin_Dashboard(admin_email);
            Admin_Dashboard.Show();
            this.Hide();
            Admin_Dashboard.FormClosed += (s, args) => this.Close();
        }
    }
}
