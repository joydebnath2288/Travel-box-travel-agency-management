using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Travel_Box
{
    public partial class SignUpOTP : Form
    {
        private int otp;
        private string to;

        public SignUpOTP(string to, int otp)
        {
            InitializeComponent();
            this.otp = otp;
            this.to = to;

            // Attach TextChanged event handlers to all text boxes
            textBox1.TextChanged += new EventHandler(TextBox_TextChanged);
            textBox2.TextChanged += new EventHandler(TextBox_TextChanged);
            textBox3.TextChanged += new EventHandler(TextBox_TextChanged);
            textBox4.TextChanged += new EventHandler(TextBox_TextChanged);
            textBox5.TextChanged += new EventHandler(TextBox_TextChanged);
            textBox6.TextChanged += new EventHandler(TextBox_TextChanged);

            // Set focus to the first text box
            textBox1.Focus();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox currentTextBox = sender as TextBox;
            if (currentTextBox != null)
            {
                string input = currentTextBox.Text;

                // Ensure the text box contains only one digit
                if (input.Length > 1)
                {
                    currentTextBox.Text = input[0].ToString();
                    currentTextBox.SelectionStart = currentTextBox.Text.Length; // Move cursor to end
                }

                // Handle moving to the next or previous text box based on input length
                if (input.Length == 1)
                {
                    // Move to the next text box if input length is 1 and not using Backspace
                    if (currentTextBox == textBox1)
                        textBox2.Focus();
                    else if (currentTextBox == textBox2)
                        textBox3.Focus();
                    else if (currentTextBox == textBox3)
                        textBox4.Focus();
                    else if (currentTextBox == textBox4)
                        textBox5.Focus();
                    else if (currentTextBox == textBox5)
                        textBox6.Focus();
                }
                else if (input.Length == 0)
                {
                    // Move to the previous text box if Backspace is pressed and current text box is empty
                    if (currentTextBox == textBox2)
                        textBox1.Focus();
                    else if (currentTextBox == textBox3)
                        textBox2.Focus();
                    else if (currentTextBox == textBox4)
                        textBox3.Focus();
                    else if (currentTextBox == textBox5)
                        textBox4.Focus();
                    else if (currentTextBox == textBox6)
                        textBox5.Focus();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Example validation: Check if all text boxes contain exactly one digit
            string otpEntered = string.Join("", GetTextBoxes().Select(tb => tb.Text));

            if (otpEntered.Length == 6 && otpEntered.All(char.IsDigit))
            {
                // Convert to integer and compare with the expected OTP (if needed)
                if (int.TryParse(otpEntered, out int enteredOtp) && enteredOtp == otp)
                {
                    MessageBox.Show("OTP Verified Successfully!");
                    SignUp SignUp = new SignUp(to);
                    SignUp.Show();
                    this.Hide();
                    SignUp.FormClosed += (s, args) => this.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect OTP.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a 6-digit OTP.");
            }
        }

        private TextBox[] GetTextBoxes()
        {
            return new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6 };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SignUpEmail SignUpEmail = new SignUpEmail();
            SignUpEmail.Show();
            this.Hide();
            SignUpEmail.FormClosed += (s, args) => this.Close();
        }
    }
}
