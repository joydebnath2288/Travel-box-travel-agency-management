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

namespace Travel_Box
{
    public partial class SignUpEmail : Form
    {
        public SignUpEmail()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please provide your Email ID.");
                return;
            }

            try
            {
                con.Open();

                // Create the SQL command to check if the email already exists
                SqlCommand cmd = new SqlCommand("SELECT password FROM customer WHERE email = @Email", con);
                cmd.Parameters.AddWithValue("@Email", textBox1.Text);
                object result = cmd.ExecuteScalar();

                SqlCommand cmd2 = new SqlCommand("SELECT password FROM admin WHERE email = @Email", con);
                cmd2.Parameters.AddWithValue("@Email", textBox1.Text);
                object admin_result = cmd2.ExecuteScalar();

                SqlCommand cmd3 = new SqlCommand("SELECT agency_password FROM agency WHERE agency_email = @Email", con);
                cmd3.Parameters.AddWithValue("@Email", textBox1.Text);
                object agency_result = cmd3.ExecuteScalar();

                if (result != null|| admin_result!=null|| agency_result!=null)
                {
                    MessageBox.Show("An account with this Email ID already exists.");
                }
                else
                {
                    string to = textBox1.Text;
                    string from = "sukumar1971211@gmail.com";
                    string pass = "kfdriaoffbewiamc";
                    string mail;

                    Random random = new Random();
                    int otp = random.Next(100000, 999999);
                    mail = "Use " + otp + " as your verification code for Travel Box.";

                    MailMessage message = new MailMessage();
                    message.To.Add(to);
                    message.From = new MailAddress(from);
                    message.Body = mail;
                    message.Subject = "Please verify your email address for Travel Box";

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                    {
                        EnableSsl = true,
                        Port = 587,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new System.Net.NetworkCredential(from, pass)
                    };

                    try
                    {
                        smtp.Send(message);
                        MessageBox.Show("Email sent successfully", "Email", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Navigate to the OTP form
                 
                        
                        SignUpOTP signUpOTP = new SignUpOTP(to, otp);
                        signUpOTP.Show();
                        this.Hide();
                        signUpOTP.FormClosed += (s, args) => this.Close();
                    
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error sending email: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
            login.FormClosed += (s, args) => this.Close();
        }
    }
}