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

namespace MailManager
{
    public partial class Form2 : Form
    {
        bool firstClick = true;
        bool firstClick1 = true;
        bool firstClick2 = true;



        public Form2()
        {
            InitializeComponent();
            textBox1.Text = Form1.SenTo;
            textBox2.Text = Form1.Subject;
            CreateMyMultilineTextBox();

        }
        private void textBox1_Click(object sender, EventArgs e)
        {
            if (firstClick)
            {
                textBox1.Text = string.Empty;
                firstClick = false;
            }
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
            Manage.SendMessage(textBox1.Text, textBox2.Text, textBox3.Text);
            textBox1.Text = "Destinataires";
            textBox2.Text = "Object";
            textBox3.Text = "Message";
            CreateMyMultilineTextBox();

        }


        public void addAttachement(Message objMessage)
        {
            /*
            List<MessagePart> attachment = objMessage.FindAllAttachments();

            if (attachment.Count > 0)
            {
                //message.FileName = attachment[0].FileName.Trim();
                //message.Attachment = attachment;
            }
            */
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
    }
}
