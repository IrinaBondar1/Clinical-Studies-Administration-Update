using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using Oracle.ManagedDataAccess.Client;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StudiiClinice
{
    public partial class ClinicalStudies : Form
    {
        
        Form2 myAplication=new Form2();
        public ClinicalStudies()
        {
            InitializeComponent();
        }

       

        private void ClinicalStudies_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox2.Text == "ADMIN")
            {
                
                this.Hide();
                myAplication.ShowDialog();
                this.Close();
            }else if(textBox2.Text=="")
            {
                MessageBox.Show("Please enter a password!");
            }
            else
            { MessageBox.Show("Incorrect password!"); }
        }
    }

}
