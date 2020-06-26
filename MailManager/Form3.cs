using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MailManager
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            textBox1.Text = Form1.SenTo;
            textBox2.Text = Form1.Subject;
            textBox3.Text = Form1.Msg;
        }
    }
}
