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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Travel_Box
{
    public partial class Manage_Agency : Form
    {
        private string admin_email;
        public Manage_Agency(string admin_email)
        {
            this.admin_email = admin_email;
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if any required fields are empty
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(comboBox2.Text) ||
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

                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd hh:mm:ss tt");

                SqlCommand cmd2 = new SqlCommand("SELECT admin_id FROM admin WHERE email = @Email", con);
                cmd2.Parameters.AddWithValue("@Email", admin_email);
                object admin_result = cmd2.ExecuteScalar();

                // Insert new record
                SqlCommand cmd = new SqlCommand("INSERT INTO agency (agency_id, agency_name, agency_email, agency_phone_number, agency_country, agency_address, agency_type, agency_password, join_date, admin_id) VALUES (@agency_id, @agency_name, @agency_email, @agency_phone_number, @agency_country, @agency_address, @agency_type, @agency_password, @join_date, @admin_id)", con);
                cmd.Parameters.AddWithValue("@agency_id", textBox1.Text);
                cmd.Parameters.AddWithValue("@agency_name", textBox2.Text);
                cmd.Parameters.AddWithValue("@agency_email", textBox3.Text);
                cmd.Parameters.AddWithValue("@agency_phone_number", textBox4.Text);
                cmd.Parameters.AddWithValue("@agency_country", comboBox1.Text);
                cmd.Parameters.AddWithValue("@agency_address", textBox5.Text);
                cmd.Parameters.AddWithValue("@agency_type", comboBox2.Text);
                cmd.Parameters.AddWithValue("@agency_password", textBox6.Text);
                cmd.Parameters.AddWithValue("@join_date", formattedDateTime);
                cmd.Parameters.AddWithValue("@admin_id", admin_result);

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

                SqlCommand cmd = new SqlCommand("UPDATE agency SET agency_name = @agency_name, agency_email = @agency_email, agency_phone_number = @agency_phone_number, agency_country = @agency_country, agency_address = @agency_address, agency_type = @agency_type, agency_password = @agency_password WHERE agency_id = @agency_id", con);

                // Add parameters
                cmd.Parameters.AddWithValue("@agency_id", textBox1.Text);
                cmd.Parameters.AddWithValue("@agency_name", textBox2.Text);
                cmd.Parameters.AddWithValue("@agency_email", textBox3.Text);
                cmd.Parameters.AddWithValue("@agency_phone_number", textBox4.Text);
                cmd.Parameters.AddWithValue("@agency_country", comboBox1.Text);
                cmd.Parameters.AddWithValue("@agency_address", textBox5.Text);
                cmd.Parameters.AddWithValue("@agency_type", comboBox2.Text);
                cmd.Parameters.AddWithValue("@agency_password", textBox6.Text);

                // Execute the update command
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Agency information updated successfully.");
                    // Fetch updated data and bind to DataGridView
                    FetchAllAgencies();
                }
                else
                {
                    MessageBox.Show("No record found with the provided Agency ID.");
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
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please fill id.");
                return;
            }

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM agency WHERE agency_id = @agency_id", con);
                cmd.Parameters.AddWithValue("@agency_id", textBox1.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Agency information found successfully.");
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                }
                else
                {
                    MessageBox.Show("No record found with the provided Agency ID.");
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please fill id.");
                return;
            }

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM agency WHERE agency_id = @agency_id", con);
                cmd.Parameters.AddWithValue("@agency_id", textBox1.Text);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Successfully deleted");
                    // Fetch updated data and bind to DataGridView
                    FetchAllAgencies();
                }
                else
                {
                    MessageBox.Show("No record found with the provided Agency ID.");
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

        private void FetchAllAgencies()
        {
            SqlCommand fetchCmd = new SqlCommand("SELECT * FROM agency", con);
            SqlDataAdapter adapter = new SqlDataAdapter(fetchCmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
    }
}
