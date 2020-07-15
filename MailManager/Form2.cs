using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;

namespace MailManager
{
    public partial class Form2 : Form
    {
        bool firstClick = true;
        bool firstClick1 = true;
        bool firstClick2 = true;
        string filename;
        List<string> dest = new List<string>();


        public Form2()
        {
            InitializeComponent();
            textBox1.Text = Form1.SenTo;
            textBox2.Text = Form1.Subject;
            label1.Text = "";
            textBox1.KeyDown += textBox1_KeyDown;
            CreateMyMultilineTextBox();

        }
        private void getData(AutoCompleteStringCollection dataCollection)
        {
            string connetionString = null;
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Alexandre\Source\Repos\MailApplication\MailManager\DB\database1.mdf;Integrated Security=True");
            string sql = "SELECT DISTINCT dest FROM TBLFROM";
            try
            {
                con.Open();
                command = new SqlCommand(sql, con);
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                adapter.Dispose();
                command.Dispose();
                con.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    dataCollection.Add(row[0].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
        }
        private void textBox2_Click(object sender, EventArgs e)
        {
            
            if (firstClick1)
            {
                textBox2.Text = string.Empty;
                firstClick1 = false;
            }
        }
        private void textBox3_Click(object sender, EventArgs e)
        {
            if (firstClick2)
            {
                textBox3.Text = string.Empty;
                firstClick2 = false;
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            bool isatt;
            if (filename != "")
            {
                isatt = true;
            }
            else
            {
                isatt = false;
            }
            Manage.SendMessage(textBox1.Text, textBox2.Text, textBox3.Text, filename, isatt);
            textBox1.Text = "Destinataires";
            textBox2.Text = "Object";
            textBox3.Text = "Message";
            CreateMyMultilineTextBox();

        }

        
        public void CreateMyMultilineTextBox()
        {
            // Create an instance of a TextBox control.
            // Set the Multiline property to true.
            textBox3.Multiline = true;
            // Add vertical scroll bars to the TextBox control.
            textBox3.ScrollBars = ScrollBars.Vertical;
            // Allow the TAB key to be entered in the TextBox control.
            textBox3.AcceptsReturn = true;
            // Allow the TAB key to be entered in the TextBox control.
            textBox3.AcceptsTab = true;
            // Set WordWrap to true to allow text to wrap to the next line.
            textBox3.WordWrap = true;
        }

        private void  button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                string name = Path.GetFileName(filename);

                label1.Text = name;
            }
            else
            {
                filename = "";
            }
        }

        private void Form2_Load_1(object sender, EventArgs e)
        {
            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection DataCollection = new AutoCompleteStringCollection();
            getData(DataCollection);
            textBox1.AutoCompleteCustomSource = DataCollection;
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.Enter)
            {
                if (!(String.IsNullOrEmpty(textBox1.Text)) || textBox1.Text != "Destinataires")
                {
                    dest.Add(textBox1.Text);
                    textBox1.Text = string.Empty;
                    foreach (var element in dest)
                    {
                        if (element != "")
                        {
                            textBox1.Text += element + ";";
                        }
                    }
                    
                }
                
            }
        }
    }
}
