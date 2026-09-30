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
    public partial class Package_Details : Form
    {
        string agency_email;
        string packageId;
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");

        public Package_Details(string packageId, string agency_email)
        {
            InitializeComponent();
            this.agency_email = agency_email;
            this.packageId = packageId;
            this.Load += new System.EventHandler(this.Package_Details_Load);
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
               // MessageBox.Show("Connection Opened Successfully!");  // Debugging point 1: Check if connection opens successfully

                // SQL query to retrieve package details for the given package ID and agency email
                string query = @"SELECT package_id, package_name, destination, price_per_person, duration, 
                                image_1, image_2, image_3, image_4, image_5, 
                                lowest_customer_capacity, highest_customer_capacity, 
                                refund_stutus, requirments, overview, details 
                        FROM tour_packages 
                        WHERE package_id = @PackageId 
                        AND agency_id = (SELECT agency_id FROM agency WHERE agency_email = @Email)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@PackageId", packageId);  // Assuming packageId is passed in the constructor
                cmd.Parameters.AddWithValue("@Email", agency_email);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        // Clear existing controls in each panel to avoid duplicates
                        panel2.Controls.Clear();
                        panel3.Controls.Clear();
                        panel4.Controls.Clear();
                        panel5.Controls.Clear();
                        panel6.Controls.Clear();
                        panel7.Controls.Clear();
                        panel8.Controls.Clear();
                        panel9.Controls.Clear();
                        panel10.Controls.Clear();
                        panel11.Controls.Clear();
                        panel12.Controls.Clear();

                        // Add Package ID to Panel 2
                        Label lblPackageId = new Label
                        {
                            Text = reader["package_id"].ToString(),
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            ForeColor = Color.Navy,
                            AutoSize = true,
                            Margin = new Padding(10)
                        };
                        panel2.Controls.Add(lblPackageId);

                        // Add Package Name to Panel 3
                        Label lblPackageName = new Label
                        {
                            Text = reader["package_name"].ToString(),
                            Font = new Font("Arial", 16, FontStyle.Bold),
                            ForeColor = Color.Navy,
                            AutoSize = false,  // Disable automatic resizing
                            MaximumSize = new Size(panel3.Width - 10, 0),  // Set max width for wrapping
                            Size = new Size(panel3.Width - 20, 0),  // Adjust label width to fit within the panel
                            AutoEllipsis = false, // Ensure text is not truncated
                            Margin = new Padding(10),
                            Dock = DockStyle.Top,  // Optional: Ensures the label takes the full width of the panel
                        };

                        // Set the label height dynamically based on its content
                        lblPackageName.Height = TextRenderer.MeasureText(lblPackageName.Text, lblPackageName.Font, lblPackageName.Size, TextFormatFlags.WordBreak).Height;
                        panel3.Controls.Add(lblPackageName);

                        // Add Destination to Panel 4
                        Label lblDestination = new Label
                        {
                            Text = reader["destination"].ToString(),
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Navy,
                            AutoSize = true,
                            Margin = new Padding(10)
                        };
                        panel4.Controls.Add(lblDestination);

                        // Add Duration to Panel 5
                        Label lblDuration = new Label
                        {
                            Text = reader["duration"].ToString() + " days",
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Navy,
                            AutoSize = true,
                            Margin = new Padding(10)
                        };
                        panel5.Controls.Add(lblDuration);

                        // Add Lowest Customer Capacity to Panel 6
                        Label lblLowestCapacity = new Label
                        {
                            Text =  reader["lowest_customer_capacity"].ToString(),
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Navy,
                            AutoSize = true,
                            Margin = new Padding(10)
                        };
                        panel6.Controls.Add(lblLowestCapacity);

                        // Add Highest Customer Capacity to Panel 7
                        Label lblHighestCapacity = new Label
                        {
                            Text = reader["highest_customer_capacity"].ToString(),
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Navy,
                            AutoSize = true,
                            Margin = new Padding(10)
                        };
                        panel7.Controls.Add(lblHighestCapacity);

                        // Add Refund Status to Panel 8
                        Label lblRefundStatus = new Label
                        {
                            Text = reader["refund_stutus"].ToString(),
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Navy,
                            AutoSize = true,
                            Margin = new Padding(10)
                        };
                        panel8.Controls.Add(lblRefundStatus);

                        // Add Requirements to Panel 9
                        Label lblRequirements = new Label
                        {
                            Text = reader["requirments"].ToString(),
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Navy,
                            AutoSize = false,  // Disable automatic resizing
                            MaximumSize = new Size(panel9.Width - 10, 0),  // Set max width for wrapping
                            Size = new Size(panel9.Width - 20, 0),  // Adjust label width to fit within the panel
                            AutoEllipsis = false, // Ensure text is not truncated
                            Margin = new Padding(10),
                            Dock = DockStyle.Top,  // Optional: Ensures the label takes the full width of the panel
                        };

                        // Set the label height dynamically based on its content
                        lblRequirements.Height = TextRenderer.MeasureText(lblRequirements.Text, lblRequirements.Font, lblRequirements.Size, TextFormatFlags.WordBreak).Height;
                        panel9.Controls.Add(lblRequirements);

                        // Add Overview to Panel 10
                        Label lblOverview = new Label
                        {
                            Text = reader["overview"].ToString(),
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Navy,
                            AutoSize = false,  // Disable automatic resizing
                            MaximumSize = new Size(panel10.Width - 10, 0),  // Set max width for wrapping
                            Size = new Size(panel10.Width - 20, 0),  // Adjust label width to fit within the panel
                            AutoEllipsis = false, // Ensure text is not truncated
                            Margin = new Padding(10),
                            Dock = DockStyle.Top,  // Optional: Ensures the label takes the full width of the panel
                        };

                        // Set the label height dynamically based on its content
                        lblOverview.Height = TextRenderer.MeasureText(lblOverview.Text, lblOverview.Font, lblOverview.Size, TextFormatFlags.WordBreak).Height;
                        panel10.Controls.Add(lblOverview);

                        // Add Details to Panel 11
                        Label lblDetails = new Label
                        {
                            Text =  reader["details"].ToString(),
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Navy,
                            AutoSize = false,  // Disable automatic resizing
                            MaximumSize = new Size(panel11.Width - 10, 0),  // Set max width for wrapping
                            Size = new Size(panel11.Width - 20, 0),  // Adjust label width to fit within the panel
                            AutoEllipsis = false, // Ensure text is not truncated
                            Margin = new Padding(10),
                            Dock = DockStyle.Top,  // Optional: Ensures the label takes the full width of the panel
                        };

                        // Set the label height dynamically based on its content
                        lblDetails.Height = TextRenderer.MeasureText(lblDetails.Text, lblDetails.Font, lblDetails.Size, TextFormatFlags.WordBreak).Height;
                        panel11.Controls.Add(lblDetails);

                        // Add Price Per Person to Panel 12
                        Label lblPricePerPerson = new Label
                        {
                            Text =  reader["price_per_person"].ToString()+" BDT",
                            Font = new Font("Arial", 10, FontStyle.Regular),
                            ForeColor = Color.Navy,
                            AutoSize = true,
                            Margin = new Padding(10)
                        };
                        panel12.Controls.Add(lblPricePerPerson);

                        // Display Image 1 if available
                        if (reader["image_1"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])reader["image_1"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                pictureBox1.Image = Image.FromStream(ms);
                                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;  // Optional, if you want to stretch the image
                            }
                        }
                        if (reader["image_2"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])reader["image_2"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                pictureBox2.Image = Image.FromStream(ms);
                                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;  // Optional, if you want to stretch the image
                            }
                        }
                        if (reader["image_3"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])reader["image_3"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                pictureBox3.Image = Image.FromStream(ms);
                                pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;  // Optional, if you want to stretch the image
                            }
                        }
                        if (reader["image_4"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])reader["image_4"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                pictureBox4.Image = Image.FromStream(ms);
                                pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;  // Optional, if you want to stretch the image
                            }
                        }
                        if (reader["image_5"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])reader["image_5"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                pictureBox5.Image = Image.FromStream(ms);
                                pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;  // Optional, if you want to stretch the image
                            }
                        }


                    }
                }
                else
                {
                    MessageBox.Show("No data found for this package.");  // Debugging point 3: No rows returned
                }

                reader.Close();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("SQL Error: " + sqlEx.Message);  // Capture detailed SQL errors
            }
            catch (Exception ex)
            {
                MessageBox.Show("General Error: " + ex.Message);  // Capture other types of errors
            }
            finally
            {
                con.Close();
                //MessageBox.Show("Connection Closed");  // Debugging point 4: Verify connection closes
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Agency_Dashboard agencyDashboard = new Agency_Dashboard(agency_email);
            agencyDashboard.Show();
            this.Hide();
            agencyDashboard.FormClosed += (s, args) => this.Close();
        }
    }
}
