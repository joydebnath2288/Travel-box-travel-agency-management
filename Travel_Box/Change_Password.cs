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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Travel_Box
{
    public partial class Change_Password : Form
    {
        private string to;
        private string database_name;
        public Change_Password(string to, string database_name)
        {
            this.to = to;
            this.database_name = database_name;
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");
        private void button1_Click(object sender, EventArgs e)
        {
            // Check if all fields are filled
            if (string.IsNullOrWhiteSpace(textBox4.Text) ||
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
            else
            {
                if(database_name=="customer")
                {
                    // If all validations pass, proceed with saving the data
                    try
                    {
                        con.Open();


                        // Insert new record
                        SqlCommand cmd = new SqlCommand("update customer set password=@password where email= @email", con);
                        cmd.Parameters.AddWithValue("@password", textBox4.Text);
                        cmd.Parameters.AddWithValue("@email", to);
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
                else if(database_name=="admin")
                {
                    try
                    {
                        con.Open();


                        // Insert new record
                        SqlCommand cmd = new SqlCommand("update admin set password=@password where email= @email", con);
                        cmd.Parameters.AddWithValue("@password", textBox4.Text);
                        cmd.Parameters.AddWithValue("@email", to);
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
                else if (database_name == "agency")
                {
                    try
                    {
                        con.Open();


                        // Insert new record
                        SqlCommand cmd = new SqlCommand("update agency set agency_password=@password where agency_email= @email", con);
                        cmd.Parameters.AddWithValue("@password", textBox4.Text);
                        cmd.Parameters.AddWithValue("@email", to);
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
                else
                {
                    MessageBox.Show("Password can't save");
                }
                
            }
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
