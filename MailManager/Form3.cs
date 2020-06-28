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
    }
}
