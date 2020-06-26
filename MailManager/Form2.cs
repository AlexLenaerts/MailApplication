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
        public Form2()
        {
            InitializeComponent();
            textBox1.Text = Form1.SenTo;
            textBox2.Text = Form1.Subject;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            Manage.SendMessage(textBox1.Text, textBox2.Text, textBox3.Text);
            textBox1.Text = "Destinataires";
            textBox2.Text = "Object";
            textBox3.Text = "Message";
        }
    }
}
