using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Travel_Box
{
    public partial class Customer_Mobile_Banking : Form
    {
        string customer_email;
        string location, packageId;
        DateTime journeyDate;
        int numPersons;
        float total_amount;
        string bookingId;
        string payment_Method;
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IES908U;Initial Catalog=Travel_Box;Integrated Security=True;");

        public Customer_Mobile_Banking(string customer_email, string location, string packageId, DateTime journeyDate, int numPersons, float total_amount)
        {
            this.customer_email = customer_email;
            this.location = location;
            this.packageId = packageId;
            this.journeyDate = journeyDate;
            this.numPersons = numPersons;
            this.total_amount = total_amount;
            InitializeComponent();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please enter your account number and pin.");
                return;
            }
            string paymentMethod = GetSelectedPaymentMethod(); ;

            if (paymentMethod == null)
            {
                MessageBox.Show("Please select a payment method.");
                return;
            }
            DateTime now = DateTime.Now;
            bookingId = now.ToString("yyyyMMddHHmmss");

            try
            {
                // Check if account number and pin are provided
                if (string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Please enter your account number and pin.");
                    return;
                }

                // Get selected payment method
                paymentMethod = GetSelectedPaymentMethod();
                if (paymentMethod == null)
                {
                    MessageBox.Show("Please select a payment method.");
                    return;
                }

                try
                {
                    con.Open();

                    // Check if the package exists in the tour_packages table
                    SqlCommand checkPackageCmd = new SqlCommand("SELECT COUNT(1) FROM tour_packages WHERE package_id = @PackageId", con);
                    checkPackageCmd.Parameters.AddWithValue("@PackageId", packageId);
                    int packageExists = (int)checkPackageCmd.ExecuteScalar();

                    if (packageExists == 0)
                    {
                        MessageBox.Show("Invalid Package ID. The package does not exist.");
                        return;
                    }

                    // Insert booking into the booking table
                    string query = @"INSERT INTO booking 
                                (booking_id, booking_date, travel_date, number_of_people, total_price, customer_email, transection_number, payment_method, package_id) 
                                VALUES (@BookingId, @BookingDate, @TravelDate, @NumPersons, @TotalPrice, @CustomerEmail, @TransectionNumber, @PaymentMethod, @Package_id)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@BookingId", bookingId);
                    cmd.Parameters.AddWithValue("@BookingDate", now); // Current booking date
                    cmd.Parameters.AddWithValue("@TravelDate", journeyDate); // Travel date from the form
                    cmd.Parameters.AddWithValue("@NumPersons", numPersons);
                    cmd.Parameters.AddWithValue("@TotalPrice", total_amount);
                    cmd.Parameters.AddWithValue("@CustomerEmail", customer_email);
                    cmd.Parameters.AddWithValue("@TransectionNumber", textBox3.Text); // Account number/pin
                    cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                    cmd.Parameters.AddWithValue("@Package_id", packageId); // Validated package ID

                    // Execute the query and check if it was successful
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Booking successful! Your booking ID is " + bookingId);
                        GeneratePDF();
                        Customer_Packages Customer_packages = new Customer_Packages(customer_email, location);
                        Customer_packages.Show();
                        this.Hide();
                        Customer_packages.FormClosed += (s, args) => this.Close();

                    }
                    else
                    {
                        MessageBox.Show("Booking failed. Please try again.");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }

            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }

        }

        private void GeneratePDF()
        {
            // Path where you want to save the PDF
            string pdfPath = @"C:\Users\Admin\Downloads\BookingBill_" + bookingId + ".pdf";

            // Create a document object
            Document doc = new Document(PageSize.A4);
            try
            {
                // Create a PDF writer instance
                PdfWriter.GetInstance(doc, new FileStream(pdfPath, FileMode.Create));

                // Open the document to write into it
                doc.Open();

                // Add content to the document
                doc.Add(new Paragraph("Travel Box"));
                doc.Add(new Paragraph("Booking Bill"));
                doc.Add(new Paragraph("Booking ID: " + bookingId));
                doc.Add(new Paragraph("Customer Email: " + customer_email));
                doc.Add(new Paragraph("Location: " + location));
                doc.Add(new Paragraph("Package ID: " + packageId));
                doc.Add(new Paragraph("Journey Date: " + journeyDate.ToShortDateString()));
                doc.Add(new Paragraph("Number of Persons: " + numPersons));
                doc.Add(new Paragraph("Total Amount: " + total_amount.ToString() + " Taka"));
                doc.Add(new Paragraph("Payment Method: " + GetSelectedPaymentMethod()));
                doc.Add(new Paragraph("Transaction Number: " + textBox3.Text));
                doc.Add(new Paragraph("Booking Date: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));

                // Optionally add more details, company logo, etc.
                MessageBox.Show("PDF bill generated successfully at: " + pdfPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF: " + ex.Message);
            }
            finally
            {
                // Close the document
                doc.Close();
            }
        }




        private string GetSelectedPaymentMethod()
            {
                if (radioButton1.Checked) return "Bkash";
                if (radioButton2.Checked) return "Nagad";
                if (radioButton3.Checked) return "Upay";
                if (radioButton4.Checked) return "Rocket";
                return null;
            }

            private void radioButton1_CheckedChanged(object sender, EventArgs e)
            {
            this.BackgroundImage = System.Drawing.Image.FromFile(@"C:\Users\Admin\Desktop\Final\All_Image\bkash.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            }

            private void radioButton2_CheckedChanged(object sender, EventArgs e)
            {
                this.BackgroundImage = System.Drawing.Image.FromFile(@"C:\Users\Admin\Desktop\Final\All_Image\Nagad.png");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }

            private void radioButton3_CheckedChanged(object sender, EventArgs e)
            {
                this.BackgroundImage = System.Drawing.Image.FromFile(@"C:\Users\Admin\Desktop\Final\All_Image\Upay.png");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }

        private void button1_Click(object sender, EventArgs e)
        {
            Customer_Book Customer_Book = new Customer_Book(customer_email, location, packageId);
            Customer_Book.Show();
            this.Hide();
            Customer_Book.FormClosed += (s, args) => this.Close();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
            {
                this.BackgroundImage = System.Drawing.Image.FromFile(@"C:\Users\Admin\Desktop\Final\All_Image\Rocket.png");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }


        }
    }

