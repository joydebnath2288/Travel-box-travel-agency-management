using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Travel_Box
{
    public partial class Add_Packages : Form
    {
        string agency_email;
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");

        public Add_Packages(string agency_email)
        {
            this.agency_email = agency_email;
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Agency_Dashboard agencyDashboard = new Agency_Dashboard(agency_email);
            agencyDashboard.Show();
            this.Hide();
            agencyDashboard.FormClosed += (s, args) => this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (AreFieldsValid())
            {
                try
                {
                    con.Open();
                    object agency_result = GetAgencyId(agency_email);

                    if (agency_result == null)
                    {
                        MessageBox.Show("Agency not found.");
                        return;
                    }

                    byte[] img1 = ImageToByteArray(pictureBox1.Image);
                    byte[] img2 = ImageToByteArray(pictureBox2.Image);
                    byte[] img3 = ImageToByteArray(pictureBox3.Image);
                    byte[] img4 = ImageToByteArray(pictureBox4.Image);
                    byte[] img5 = ImageToByteArray(pictureBox5.Image);

                    InsertPackageDetails(agency_result, img1, img2, img3, img4, img5);

                    MessageBox.Show("Successfully saved.");
                    Agency_Dashboard agencyDashboard = new Agency_Dashboard(agency_email);
                    agencyDashboard.Show();
                    this.Hide();
                    agencyDashboard.FormClosed += (s, args) => this.Close();
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] files = openFileDialog.FileNames;
                if (files.Length != 5)
                {
                    MessageBox.Show("Please select exactly 5 images.");
                    return;
                }

                pictureBox1.Image = Image.FromFile(files[0]);
                pictureBox2.Image = Image.FromFile(files[1]);
                pictureBox3.Image = Image.FromFile(files[2]);
                pictureBox4.Image = Image.FromFile(files[3]);
                pictureBox5.Image = Image.FromFile(files[4]);

                MessageBox.Show("Images uploaded successfully.");
            }
        }

        private bool AreFieldsValid()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text) ||
                string.IsNullOrWhiteSpace(textBox8.Text) ||
                string.IsNullOrWhiteSpace(textBox9.Text) ||
                string.IsNullOrWhiteSpace(textBox10.Text) ||
                pictureBox1.Image == null ||
                pictureBox2.Image == null ||
                pictureBox3.Image == null ||
                pictureBox4.Image == null ||
                pictureBox5.Image == null)
            {
                MessageBox.Show("Please fill in all the required fields and select images.");
                return false;
            }
            return true;
        }

        private object GetAgencyId(string email)
        {
            using (SqlCommand cmd2 = new SqlCommand("SELECT agency_id FROM agency WHERE agency_email = @Email", con))
            {
                cmd2.Parameters.AddWithValue("@Email", email);
                return cmd2.ExecuteScalar();
            }
        }

        private void InsertPackageDetails(object agency_id, byte[] img1, byte[] img2, byte[] img3, byte[] img4, byte[] img5)
        {
            string query = "INSERT INTO tour_packages (package_id, package_name, destination, duration, lowest_customer_capacity, highest_customer_capacity, price_per_person, refund_stutus, requirments, overview, details, agency_id, image_1, image_2, image_3, image_4, image_5) " +
                           "VALUES (@package_id, @package_name, @destination, @duration, @lowest_customer_capacity, @highest_customer_capacity, @price_per_person, @refund_stutus, @requirments, @overview, @details, @agency_id, @img1, @img2, @img3, @img4, @img5)";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@package_id", textBox1.Text);
                cmd.Parameters.AddWithValue("@package_name", textBox2.Text);
                cmd.Parameters.AddWithValue("@destination", textBox3.Text);
                cmd.Parameters.AddWithValue("@duration", textBox4.Text);
                cmd.Parameters.AddWithValue("@lowest_customer_capacity", textBox5.Text);
                cmd.Parameters.AddWithValue("@highest_customer_capacity", textBox6.Text);
                cmd.Parameters.AddWithValue("@price_per_person", textBox9.Text);
                cmd.Parameters.AddWithValue("@refund_stutus", comboBox1.Text);
                cmd.Parameters.AddWithValue("@requirments", textBox7.Text);
                cmd.Parameters.AddWithValue("@overview", textBox8.Text);
                cmd.Parameters.AddWithValue("@details", textBox10.Text);
                cmd.Parameters.AddWithValue("@agency_id", agency_id);
                cmd.Parameters.AddWithValue("@img1", img1);
                cmd.Parameters.AddWithValue("@img2", img2);
                cmd.Parameters.AddWithValue("@img3", img3);
                cmd.Parameters.AddWithValue("@img4", img4);
                cmd.Parameters.AddWithValue("@img5", img5);

                cmd.ExecuteNonQuery();
            }
        }

        private bool AreImagesUploaded()
        {
            if (pictureBox1.Image == null || pictureBox2.Image == null ||
                pictureBox3.Image == null || pictureBox4.Image == null || pictureBox5.Image == null)
            {
                MessageBox.Show("Please upload all images.");
                return false;
            }
            return true;
        }

        private byte[] ImageToByteArray(Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
    }
}

