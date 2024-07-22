using Oracle.ManagedDataAccess.Client;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StudiiClinice
{
    public partial class Questions : Form
    {
        private OracleConnection con = null;

        public Questions()
        {
            this.setConnection();
            
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void setConnection()
        {


            // Construirea șirului de conexiune
            string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=Irina_PC)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=SYSTEM;Password=admin;";


            con = new OracleConnection(connectionString);
            try
            {
                con.Open();
                Console.WriteLine("Connection successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        private void updateDataGrid1(string query)
        {



            try
            {
                // Check if the connection is closed and open it if necessary
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                    Console.WriteLine("Connection successful!");
                }

                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();

                // Create a DataTable to hold the data
                DataTable dt = new DataTable();
                dt.Load(dr);

                // Bind the DataTable to the DataGridView
                dataGridView1.DataSource = dt.DefaultView;

                // Close the data reader
                dr.Close();
            }
            catch (OracleException ex)
            {
                // Handle Oracle database exceptions
                MessageBox.Show("An error occurred while fetching data from the database: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            updateDataGrid1("SELECT s.study_name \"NUME_STUDIU\", COUNT(d.doctor_id) AS \"NR_DOCTORI\" " +
                    "FROM Doctors d, Studies s " +
                    "WHERE d.doctor_id = s.doctor_id " +
                    "GROUP BY s.study_name");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            
            string studyName = (string)textBox1.Text.Trim();
            updateDataGrid1("SELECT p.p_first_name, p.p_last_name, s.study_name " +
               "FROM Patients p, Studies s " +
               "WHERE p.study_id = s.study_id AND s.study_name = '" + studyName + "'");

        }

        

        private void button4_Click(object sender, EventArgs e)
        {
            

            int age = Convert.ToInt32(textBox2.Text);
            updateDataGrid1("SELECT p.p_first_name, p.p_last_name, s.study_name " +
               "FROM Patients p, Studies s " +
               "WHERE p.study_id = s.study_id AND p.age < '" + age + "'");
        }

       

        private void button7_Click(object sender, EventArgs e)
        {
           /* string location = textBox3.Text;
            updateDataGrid1("SELECT study_id , study_name , location  " +
               "FROM  Studies  " +
               "WHERE location=:location ");*/

        }

        private void button6_Click(object sender, EventArgs e)
        {
            /*string location1 = textBox4.Text;
            updateDataGrid1("SELECT r.result_id , r.measurement , r.measurement_value , r.measurement_unit , s.location , r.data_source " +
               "FROM  Studies s , Results r  " +
               "WHERE s.study_id = r.study_id AND s.location=:location1 ");*/

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
