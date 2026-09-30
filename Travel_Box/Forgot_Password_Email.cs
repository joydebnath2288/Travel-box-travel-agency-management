using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net;

namespace Travel_Box
{
    public partial class Forgot_Password_Email : Form
    {
        public Forgot_Password_Email()
        {
            InitializeComponent();
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

                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Please give your Email ID.");
                    return;
                }

                if (result == null && admin_result == null && agency_result == null)
                {
                    MessageBox.Show("Account not found.");
                    return;
                }
                else
                {
                    string database_name = string.Empty;

                    if (result != null)
                    {
                        database_name = "customer";
                    }
                    else if (admin_result != null)
                    {
                        database_name = "admin";
                    }
                    else if (agency_result != null)
                    {
                        database_name = "agency";
                    }
                      string to, from, pass, mail;
                      to =  textBox1.Text;
                      from = "sukumar1971211@gmail.com";
                      Random random = new Random();
                      int otp = random.Next(100000,999999);
                      mail = "Use " + otp + " as your verification code for VoyageVista. ";
                      pass = "kfdriaoffbewiamc";
                      MailMessage message = new MailMessage();
                      message.To.Add(to);
                      message.From = new MailAddress(from);
                      message.Body = mail;
                      message.Subject = "Please verify your email address for VoyageVista";
                      SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                      smtp.EnableSsl = true;
                      smtp.Port = 587;
                      smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                      smtp.Credentials = new NetworkCredential(from, pass);
                      try
                      {
                          smtp.Send(message);
                          MessageBox.Show("Email send successfully", "Email", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    
                    Forgot_Password_OTP Forgot_Password_OTP = new Forgot_Password_OTP(to, otp, database_name);
                    Forgot_Password_OTP.Show();
                    this.Hide();
                    Forgot_Password_OTP.FormClosed += (s, args) => this.Close();

                      }
                    catch (SmtpException ex) // Catch email-related exceptions
                    {
                        // Display error message indicating that email couldn't be sent
                        MessageBox.Show("Failed to send the email. Please check your connection and try again.", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Please enter a valid email address.");
                        return;
                    }

                    catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);

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
            Login Login = new Login();
            Login.Show();
            this.Hide();
            Login.FormClosed += (s, args) => this.Close();
        }
    }
}
