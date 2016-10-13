using System;
using System.IO;
using System.Windows.Forms;

namespace SLOLC
{
    public partial class Form1 : Form
    {
        private bool accountcheck = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoLMethod calls = new LoLMethod();

            calls.SetLogin(listBox1.SelectedIndex);

            int counter = 1;
            string line;
            string fullPath = Path.GetFullPath(@"info.txt");
            StreamReader file = new StreamReader(fullPath);
            while ((line = file.ReadLine()) != null)
            {
                string[] acc = line.Split(':');
                accountcheck = calls.CheckAccount(acc[0], acc[1], acc[2] + ":" + acc[3]);
                Text = "Password Tested: " + counter;

                if (accountcheck == true)
                {
                    listBox2.Items.Add("Username: " + acc[0] + " Password: " + acc[1]);
                }
                counter++;

            }
            file.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            AboutBox1 form = new AboutBox1();
            form.Show();
        }
    }
}
