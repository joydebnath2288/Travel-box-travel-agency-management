using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Travel_Box
{
    public partial class Agency_Dashboard : Form
    {
        string agency_email;
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");

        public Agency_Dashboard(string agency_email)
        {
            this.agency_email = agency_email;
            InitializeComponent();
            LoadPackages(); // Load all packages initially without any filters
        }

        // Method to apply filters based on price and duration
        private void ApplyFilters()
        {
            int? maxPrice = trackBar1.Value > trackBar1.Minimum ? (int?)trackBar1.Value : null; // Use nullable int for maxPrice
            List<string> durationConditions = GetDurationConditions(); // Get selected duration filters

            // Load packages with the applied filters
            LoadPackages(maxPrice, durationConditions);
        }

        // Method to get selected duration conditions
        private List<string> GetDurationConditions()
        {
            List<string> conditions = new List<string>();

            // Less than 6 hours
            if (checkBox1.Checked)
            {
                conditions.Add("(CASE " +
                               "WHEN duration LIKE '%days%' THEN CAST(LEFT(duration, CHARINDEX(' ', duration) - 1) AS INT) * 24 " +
                               "WHEN duration LIKE '%hours%' THEN CAST(LEFT(duration, CHARINDEX(' ', duration) - 1) AS INT) " +
                               "END) < 6");
            }

            // Between 6 and 12 hours
            if (checkBox2.Checked)
            {
                conditions.Add("(CASE " +
                               "WHEN duration LIKE '%days%' THEN CAST(LEFT(duration, CHARINDEX(' ', duration) - 1) AS INT) * 24 " +
                               "WHEN duration LIKE '%hours%' THEN CAST(LEFT(duration, CHARINDEX(' ', duration) - 1) AS INT) " +
                               "END) BETWEEN 6 AND 12");
            }

            // Between 12 and 24 hours
            if (checkBox3.Checked)
            {
                conditions.Add("(CASE " +
                               "WHEN duration LIKE '%days%' THEN CAST(LEFT(duration, CHARINDEX(' ', duration) - 1) AS INT) * 24 " +
                               "WHEN duration LIKE '%hours%' THEN CAST(LEFT(duration, CHARINDEX(' ', duration) - 1) AS INT) " +
                               "END) BETWEEN 12 AND 24");
            }

            // More than 24 hours
            if (checkBox4.Checked)
            {
                conditions.Add("(CASE " +
                               "WHEN duration LIKE '%days%' THEN CAST(LEFT(duration, CHARINDEX(' ', duration) - 1) AS INT) * 24 " +
                               "WHEN duration LIKE '%hours%' THEN CAST(LEFT(duration, CHARINDEX(' ', duration) - 1) AS INT) " +
                               "END) > 24");
            }

            return conditions;
        }


        private void LoadPackages(int? maxPrice = null, List<string> durationConditions = null)
        {
            try
            {
                con.Open();

                // Base query to fetch packages associated with the agency
                string query = "SELECT package_id, package_name, destination, price_per_person, duration, image_1, lowest_customer_capacity, highest_customer_capacity, refund_stutus " +
                               "FROM tour_packages WHERE agency_id = (SELECT agency_id FROM agency WHERE agency_email = @Email)";

                // Apply duration filter if conditions are present
                if (durationConditions != null && durationConditions.Count > 0)
                {
                    string durationFilter = string.Join(" OR ", durationConditions);
                    query += $" AND ({durationFilter})";
                }

                // Apply price filter only if maxPrice is provided (i.e., not null)
                if (maxPrice.HasValue)
                {
                    query += " AND price_per_person <= @MaxPrice";
                }

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", agency_email);

                // Add price parameter only if the price filter is applied
                if (maxPrice.HasValue)
                {
                    cmd.Parameters.AddWithValue("@MaxPrice", maxPrice.Value);
                }

                SqlDataReader reader = cmd.ExecuteReader();

                // Clear the panel before adding new buttons
                panel1.Controls.Clear();

                // Dynamically create buttons for each package
                int yPos = 5;
                int xPos = 20;
                int buttonWidth = 762;
                int buttonHeight = 250;

                while (reader.Read())
                {
                    // Create a new button for each package
                    Button packageButton = new Button
                    {
                        Size = new Size(buttonWidth, buttonHeight),
                        Location = new Point(xPos, yPos),
                        BackColor = Color.White
                    };

                    // Set the background image from the database (convert byte array to image)
                    byte[] imgData = (byte[])reader["image_1"];
                    using (MemoryStream ms = new MemoryStream(imgData))
                    {
                        Image img = Image.FromStream(ms);
                        packageButton.BackgroundImage = img;
                        packageButton.BackgroundImageLayout = ImageLayout.Stretch;
                    }

                    // Add labels for package details
                    Label packageNameLabel = new Label
                    {
                        Text = "  " + reader["package_name"].ToString(),
                        Font = new Font("Arial", 14, FontStyle.Bold),
                        Size = new Size(buttonWidth, 22),
                        BackColor = Color.FromArgb(130, Color.White),
                        TextAlign = ContentAlignment.MiddleLeft,
                        ForeColor = Color.Black,
                        Location = new Point(2, 166)
                    };

                    Label packageDetailsLabel = new Label
                    {
                        Text = "   " + reader["destination"].ToString() + "\n" +
                               "   Duration: " + reader["duration"].ToString() + "\n" +
                               "   Price: BDT " + reader["price_per_person"].ToString() + " per person",
                        Font = new Font("Arial", 10, FontStyle.Regular),
                        Size = new Size(600, buttonHeight - 190),
                        BackColor = Color.FromArgb(130, Color.White),
                        TextAlign = ContentAlignment.MiddleLeft,
                        ForeColor = Color.Black,
                        Location = new Point(2, 188)
                    };

                    Label packageDetailsLabel2 = new Label
                    {
                        Text = "From " + reader["lowest_customer_capacity"].ToString() + " to " + reader["highest_customer_capacity"].ToString() + " people  " + "\n" +
                               reader["refund_stutus"].ToString() + "  ",
                        Font = new Font("Arial", 10, FontStyle.Regular),
                        Size = new Size(160, buttonHeight - 190),
                        BackColor = Color.FromArgb(130, Color.White),
                        TextAlign = ContentAlignment.MiddleRight,
                        ForeColor = Color.Black,
                        Location = new Point(600, 188)
                    };

                    // Store package_id in the button's Tag property for later use
                    packageButton.Tag = reader["package_id"];
                    packageButton.Click += PackageButton_Click;

                    // Add labels to the button
                    packageButton.Controls.Add(packageNameLabel);
                    packageButton.Controls.Add(packageDetailsLabel);
                    packageButton.Controls.Add(packageDetailsLabel2);

                    // Add the button to the panel
                    panel1.Controls.Add(packageButton);

                    // Update the position for the next button
                    yPos += buttonHeight + 10;
                }

                reader.Close();
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


        // Event handler for clicking on a package button
        private void PackageButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            string packageId = clickedButton.Tag.ToString();

            // Open the package details form
            Package_Details packageDetails = new Package_Details(packageId, agency_email);
            packageDetails.Show();
            this.Hide();
            packageDetails.FormClosed += (s, args) => this.Close();
        }

        // Event handler for trackBar scroll (price filter)
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = trackBar1.Value.ToString() + " BDT"; // Update label with selected price
            ApplyFilters(); // Apply filters based on the new price
        }

        // Event handler for resetting filters
        private void button4_Click(object sender, EventArgs e)
        {
            // Reset the trackBar and checkboxes
            trackBar1.Value = trackBar1.Minimum;
            label3.Text = trackBar1.Value.ToString() + " BDT";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;

            // Reload packages without any filters
            LoadPackages();
        }

        // Other event handlers for buttons (Add, Delete, Logout)
        private void button1_Click(object sender, EventArgs e)
        {
            Add_Packages addPackages = new Add_Packages(agency_email);
            addPackages.Show();
            this.Hide();
            addPackages.FormClosed += (s, args) => this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
            login.FormClosed += (s, args) => this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Delete_Package deletePackage = new Delete_Package(agency_email);
            deletePackage.Show();
            this.Hide();
            deletePackage.FormClosed += (s, args) => this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Booked_Packages Booked_Packages = new Booked_Packages(agency_email);
            Booked_Packages.Show();
            this.Hide();
            Booked_Packages.FormClosed += (s, args) => this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Consultation_Request Consultation_Request = new Consultation_Request(agency_email);
            Consultation_Request.Show();
            this.Hide();
            Consultation_Request.FormClosed += (s, args) => this.Close();
        }
    }
}
