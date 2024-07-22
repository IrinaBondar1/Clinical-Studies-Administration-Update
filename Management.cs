using Oracle.ManagedDataAccess.Client;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;



namespace StudiiClinice
{

    public partial class Form2 : Form
    {
        private OracleConnection con = null;

        public Form2()
        {
            this.setConnection();
            InitializeComponent();
            this.Load += Form2_Load;
            button7.Click += new EventHandler(button7_Click);
            button10.Click += new EventHandler(button10_Click);
            button13.Click += new EventHandler(button13_Click);
            button8.Click += new EventHandler(button8_Click);
            button9.Click += new EventHandler(button9_Click);
            button1.Click += new EventHandler(button1_Click);
            button2.Click += new EventHandler(button2_Click);
            button3.Click += new EventHandler(button3_Click);
            button4.Click += new EventHandler(button4_Click);
            button17.Click += new EventHandler(button17_Click);
            button14.Click += new EventHandler(button14_Click);
            button11.Click += new EventHandler(button11_Click);
            button15.Click += new EventHandler(button15_Click);
            button18.Click += new EventHandler(button18_Click);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Any initialization code for Form2 can go here
        }

        private void button1_Click(object sender, EventArgs e)
        {

            groupBox1.Visible = true;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            groupBox4.Visible = false;


            string query = "SELECT * FROM STUDIES ORDER BY STUDY_ID";
            updateDataGrid1(query);
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
            groupBox1.Visible = false;
            groupBox2.Visible = true;
            groupBox4.Visible = false;

            string query = "SELECT * FROM DOCTORS ORDER BY DOCTOR_ID";
            updateDataGrid1(query);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            groupBox3.Visible = true;
            groupBox4.Visible = false;

            string query = "SELECT * FROM PATIENTS ORDER BY PATIENT_ID";
            updateDataGrid1(query);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            groupBox4.Visible = true;

            string query = "SELECT * FROM RESULTS ORDER BY RESULT_ID";
            updateDataGrid1(query);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void setConnection()
        {
           

            // Construirea șirului de conexiune
            string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=Irina_PC)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=******;Password=*****;";


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

        private void button5_Click(object sender, EventArgs e)
        {

           

        }

    

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                // Deschide conexiunea dacă nu este deja deschisă
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Creează comanda Oracle pentru inserare
                OracleCommand cmd = con.CreateCommand();

                cmd.CommandText = "SELECT result_id_seq.NEXTVAL FROM dual";
                decimal result = (decimal)cmd.ExecuteScalar();
                long result_id = Convert.ToInt64(result);

                cmd.CommandText = "INSERT INTO Results (result_id, study_id, patient_id, measurement , measurement_value, measurement_unit, data, status, comments, data_source) " +
                                  "VALUES (:result_id, :study_id, :patient_id,  :measurement , :measurement_value, :measurement_unit, :data, :status, :comments, :data_source)";
                cmd.CommandType = CommandType.Text;

                
                cmd.Parameters.Add("result_id", OracleDbType.Int32).Value =result_id;


                int studyId;
                if (string.IsNullOrEmpty(textBox24.Text))
                {
                    MessageBox.Show("Study ID trebuie să fie completat.");
                }
                
                else if (textBox24.Text.Length > 10)
                {
                    MessageBox.Show("Study ID nu poate depăși 10 cifre.");
                }
               
                else
                {
                    if (!int.TryParse(textBox24.Text, out studyId))
                    {
                        MessageBox.Show("Study ID trebuie să fie un număr întreg.");
                    }
                    else
                    {
                        
                        string queryCheckStudyId = "SELECT COUNT(*) FROM Studies WHERE study_id = :studyId";
                        using (OracleCommand checkStudyIdCmd = new OracleCommand(queryCheckStudyId, con))
                        {
                            checkStudyIdCmd.Parameters.Add("studyId", OracleDbType.Int32).Value = studyId;

                            int count = Convert.ToInt32(checkStudyIdCmd.ExecuteScalar());

                            if (count == 0)
                            {
                                MessageBox.Show("Study ID nu există.");
                            }
                            else
                            {
                                
                                cmd.Parameters.Add("study_id", OracleDbType.Int32).Value = studyId;
                            }
                        }
                    }
                }




                int patientId;
                if (string.IsNullOrEmpty(textBox25.Text))
                {
                    MessageBox.Show("Patient ID trebuie să fie completat.");
                }
              
                else if (textBox25.Text.Length > 10)
                {
                    MessageBox.Show("Patient ID nu poate depăși 10 cifre.");
                }
                
                else
                {
                    if (!int.TryParse(textBox25.Text, out patientId))
                    {
                        MessageBox.Show("Patient ID trebuie să fie un număr întreg.");
                    }
                    else
                    {
                        
                        string queryCheckPatientId = "SELECT COUNT(*) FROM Patients WHERE patient_id = :patientId";
                        using (OracleCommand checkPatientIdCmd = new OracleCommand(queryCheckPatientId, con))
                        {
                            checkPatientIdCmd.Parameters.Add("patientId", OracleDbType.Int32).Value = patientId;

                            int count = Convert.ToInt32(checkPatientIdCmd.ExecuteScalar());

                            if (count == 0)
                            {
                                MessageBox.Show("Patient ID nu există.");
                            }
                            else
                            {
                                
                                cmd.Parameters.Add("patient_id", OracleDbType.Int32).Value = patientId;
                            }
                        }
                    }
                }


                if(comboBox4.Text.Length > 100 )
                {
                    MessageBox.Show("Measurement depășește lungimea maximă permisă de 100 de caractere. ");
                }else if (string.IsNullOrEmpty(comboBox4.Text))
                {
                    MessageBox.Show("Measurement trebuie sa fie completat.");
                }
                else
                {
                    cmd.Parameters.Add("measurement", OracleDbType.Varchar2).Value = comboBox4.Text;
                }

                
                if (string.IsNullOrEmpty(textBox26.Text))
                {
                    MessageBox.Show("Measurement Value trebuie să fie completat.");
                }
                else if (!decimal.TryParse(textBox26.Text, out _))
                {
                    MessageBox.Show("Measurement Value trebuie să fie un număr valid.");
                }
                else
                {
                    cmd.Parameters.Add("measurement_value", OracleDbType.Decimal).Value = Convert.ToDecimal(textBox26.Text);
                }

                
                if (string.IsNullOrEmpty(comboBox1.Text))
                {
                    MessageBox.Show("Measurement Unit trebuie să fie completat.");
                }
                else
                {
                    cmd.Parameters.Add("measurement_unit", OracleDbType.Varchar2).Value = comboBox1.Text;
                }

               
                if (dateTimePicker3.Value < DateTime.MinValue || dateTimePicker3.Value > DateTime.MaxValue)
                {
                    MessageBox.Show("Data selectată nu este validă.");
                }
                else
                {
                    cmd.Parameters.Add("data", OracleDbType.Date).Value = dateTimePicker3.Value;
                }

                // Validare pentru status
                if (string.IsNullOrEmpty(comboBox2.Text))
                {
                    MessageBox.Show("Status trebuie să fie completat.");
                }
                else
                {
                    cmd.Parameters.Add("status", OracleDbType.Varchar2).Value = comboBox2.Text;
                }


                
                string commentsValue = textBox30.Text.Trim(); // Trim pentru a elimina spațiile albe de la început și sfârșit

                if (string.IsNullOrEmpty(commentsValue))
                {
                    commentsValue = "fara comentarii";
                }
                // Adaugă parametrul și valoarea acestuia
                cmd.Parameters.Add("comments", OracleDbType.Varchar2).Value = commentsValue;


                // Validare pentru data_source
                if (string.IsNullOrEmpty(comboBox3.Text))
                {
                    MessageBox.Show("Data Source trebuie să fie completat.");
                }
                else
                {
                    cmd.Parameters.Add("data_source", OracleDbType.Varchar2).Value = comboBox3.Text;
                }


                

                // Execută comanda
                int rowsAffected = cmd.ExecuteNonQuery();

                // Afișează un mesaj dacă inserarea a fost reușită
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Inserarea a fost realizată cu succes.");
                    // Actualizează DataGridView-ul
                    updateDataGrid1("SELECT * FROM Results ORDER BY RESULT_ID");
                }
                else
                {
                    MessageBox.Show("Inserarea a eșuat.");
                }
            }
            catch (OracleException ex)
            {
                // Gestionează excepțiile Oracle
                MessageBox.Show("A apărut o eroare în timpul inserării datelor în baza de date: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Gestionează alte excepții
                MessageBox.Show("A apărut o eroare neașteptată: " + ex.Message);
            }
            finally
            {
                // Închide conexiunea
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }


        }

        /* INSERT IN TABELUL STUDIES */

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                // Deschide conexiunea dacă nu este deja deschisă
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Creează comanda Oracle pentru inserare
                OracleCommand cmd = con.CreateCommand();

                cmd.CommandText = "SELECT study_id_seq.NEXTVAL FROM dual";
                decimal result = (decimal)cmd.ExecuteScalar();
                long study_id = Convert.ToInt64(result);

                cmd.CommandText = "INSERT INTO Studies (study_id, study_name, start_date, location, doctor_id, stage, budget, sponsor) " +
                                  "VALUES (:study_id, :study_name, :start_date, :location, :doctor_id, :stage, :budget, :sponsor)";
                cmd.CommandType = CommandType.Text;

                // Adaugare parametrii și validarea valorilor
                cmd.Parameters.Add("study_id", OracleDbType.Int32).Value = study_id;

                /*VALIDARE SI ADAUGARE STUDY NAME*/
                if (textBox2.Text.Length > 50)
                {
                    // Mesaj de eroare în cazul în care lungimea depășește limita
                    MessageBox.Show("Study Name depășește lungimea maximă permisă de 50 de caractere.");
                }
                else if (string.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show("Study Name trebuie sa fie completat");
                }
                else
                {
                    cmd.Parameters.Add("study_name", OracleDbType.Varchar2).Value = textBox2.Text;
                }


                /*VALIDARE SI ADAUGARE DATE*/
                if (dateTimePicker1.Value < DateTime.MinValue || dateTimePicker1.Value > DateTime.MaxValue)
                {
                    // Mesaj de eroare în cazul în care data nu este validă
                    MessageBox.Show("Data selectată nu este validă.");
                }
                else
                {

                    cmd.Parameters.Add("start_date", OracleDbType.Date).Value = dateTimePicker1.Value;
                }


                /*VALIDARE SI ADAUGARE LOCATION*/
                if (comboBox6.Text.Length > 20)
                {
                    MessageBox.Show("Location depășește lungimea maximă permisă de 20 de caractere.");
                }else if (string.IsNullOrEmpty(comboBox6.Text))
                {
                    MessageBox.Show("Location trebuie sa fie completat.");
                }
                else
                {
                    cmd.Parameters.Add("location", OracleDbType.Varchar2).Value = comboBox6.Text;
                }



                /*VALIDARE SI ADAUGARE DOCTOR ID*/
if (string.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show("Doctor ID trebuie să fie completat.");
                }
                else
                {
                    int doctorId;
                    if (!int.TryParse(textBox4.Text, out doctorId))
                    {
                        MessageBox.Show("Doctor ID trebuie să fie un număr întreg.");
                    }
                    else
                    {
                        if (doctorId < 100)
                        {
                            MessageBox.Show("Doctor ID trebuie să fie mai mare sau egal cu 100.");
                        }
                        else
                        {
                            // Verificare existență doctor_id în tabelul doctors
                            string query = "SELECT COUNT(*) FROM doctors WHERE doctor_id = :doctorId";
                            using (OracleCommand checkDoctorCmd = new OracleCommand(query, con))
                            {
                                checkDoctorCmd.Parameters.Add("doctorId", OracleDbType.Int32).Value = doctorId;

                                int doctorCount = Convert.ToInt32(checkDoctorCmd.ExecuteScalar());

                                if (doctorCount == 0)
                                {
                                    MessageBox.Show("Doctor ID nu există în tabelul doctors.");
                                }
                                else
                                {
                                    cmd.Parameters.Add("doctor_id", OracleDbType.Int32).Value = doctorId;
                                }
                            }
                        }
                    }
                }



                /*VALIDARE SI ADAUGARE STAGE*/
                if (comboBox7.Text.Length > 15)
                {
                    MessageBox.Show("Stage depășește lungimea maximă permisă de 15 caractere.");
                }
                else if (string.IsNullOrEmpty(comboBox7.Text))
                {
                    MessageBox.Show("Stage trebuie sa fie completat.");
                }
                else
                {
                    cmd.Parameters.Add("stage", OracleDbType.Varchar2).Value = comboBox7.Text;
                }



                /*VALIDARE SI ADAUGARE BUDGET*/
                if (Convert.ToDecimal(textBox6.Text) < 1000)
                {
                    MessageBox.Show("Buget invalid.");
                }else if (string.IsNullOrEmpty(textBox6.Text))
                {
                    MessageBox.Show("Budget trebuie sa fie completat");
                }
                else
                {
                    cmd.Parameters.Add("budget", OracleDbType.Decimal).Value = Convert.ToDecimal(textBox6.Text);
                }



                /*VALIDARE SI ADAUGARE SPONSOR*/
                if (textBox7.Text.Length > 15)
                {
                    MessageBox.Show("Sponsor depășește lungimea maximă permisă de 15 caractere.");
                }else if (string.IsNullOrEmpty(textBox7.Text))
                {
                    MessageBox.Show("Sponsor trebuie sa fie completat");
                }
                else
                {
                    cmd.Parameters.Add("sponsor", OracleDbType.Varchar2).Value = textBox7.Text;
                }


                // Verificare dacă același doctor_id este asociat cu același study_id
                string queryCheckDoctorId = "SELECT COUNT(*) FROM Studies WHERE study_id = :study_id AND doctor_id = :doctor_id";
                using (OracleCommand checkDoctorIdCmd = new OracleCommand(queryCheckDoctorId, con))
                {
                    checkDoctorIdCmd.Parameters.Add("study_id", OracleDbType.Int32).Value = study_id;
                    checkDoctorIdCmd.Parameters.Add("doctor_id", OracleDbType.Int32).Value = textBox4.Text;

                    int count = Convert.ToInt32(checkDoctorIdCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Același doctor_id este deja asociat cu acest studiu.");
                        
                    }

                }


                // Execută comanda
                int rowsAffected = cmd.ExecuteNonQuery();

                // Afișează un mesaj dacă inserarea a fost reușită
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Inserarea a fost realizată cu succes.");
                    this.updateDataGrid1("SELECT * FROM STUDIES ORDER BY STUDY_ID");
                }
                else
                {
                    MessageBox.Show("Inserarea a eșuat.");
                }
            }
            catch (Exception ex)
            {
                // Gestionează alte excepții
                MessageBox.Show("A apărut o eroare neașteptată: " + ex.Message);
            }
            finally
            {
                // Închide conexiunea
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        //VALIDARE SI INSERARI IN TABELUL DOCTORS
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                // Deschide conexiunea dacă nu este deja deschisă
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Creează comanda Oracle pentru inserare
                OracleCommand cmd = con.CreateCommand();


                cmd.CommandText = "SELECT doctor_id_seq.NEXTVAL FROM dual";
                decimal result = (decimal)cmd.ExecuteScalar();
                long doctor_id = Convert.ToInt64(result);

                cmd.CommandText = "INSERT INTO Doctors (doctor_id, d_first_name, d_last_name, hire_date, d_number, d_email, specialization, experience) " +
                                  "VALUES (:doctor_id, :d_first_name, :d_last_name, :hire_date, :d_number, :d_email, :specialization, :experience)";
                cmd.CommandType = CommandType.Text;


                cmd.Parameters.Add("doctor_id", OracleDbType.Int64).Value = doctor_id;

                /*VALIDARE Si ADAUGARE D_FIRST_NAME */
                if (string.IsNullOrEmpty(textBox9.Text))
                {
                    MessageBox.Show("First Name trebuie sa fie completat.");
                }else if (textBox9.Text.Length >10)
                {
                    MessageBox.Show("First Name depășește lungimea maximă permisă de 10 caractere.");
                }
                else
                {
                    cmd.Parameters.Add("d_first_name", OracleDbType.Varchar2).Value = textBox9.Text;
                }


                /*VALIDARE SI ADAUGARE D_LAST_NAME*/
                if (string.IsNullOrEmpty(textBox10.Text))
                {
                    MessageBox.Show("Last Name trebuie sa fie completat.");
                }
                else if (textBox9.Text.Length > 10)
                {
                    MessageBox.Show("Last Name depășește lungimea maximă permisă de 10 caractere.");
                }
                else
                {
                    cmd.Parameters.Add("d_last_name", OracleDbType.Varchar2).Value = textBox10.Text;
                }



                /*VALIDARE SI ADAUGARE DATE*/
                if (dateTimePicker2.Value < DateTime.MinValue || dateTimePicker2.Value > DateTime.MaxValue)
                {
                    // Mesaj de eroare în cazul în care data nu este validă
                    MessageBox.Show("Data selectată nu este validă.");
                }
                else
                {

                    cmd.Parameters.Add("hire_date", OracleDbType.Date).Value = dateTimePicker2.Value;
                }




                /*VALIDARE SI ADAUGARE D_NUMBER*/
                if (string.IsNullOrEmpty(textBox11.Text))
                {
                    MessageBox.Show("Doctor Number trebuie sa fie completat.");
                }
                else if (textBox9.Text.Length > 10)
                {
                    MessageBox.Show("Doctor Number depășește lungimea maximă permisă de 10 caractere.");
                }
                else
                {
                    cmd.Parameters.Add("d_number", OracleDbType.Int32).Value = Convert.ToInt32(textBox11.Text);
                }



                /* VALIDARE SI ADAUGARE D_EMAIL */
                if (string.IsNullOrEmpty(textBox12.Text))
                {
                    MessageBox.Show("Doctor Email trebuie să fie completat.");
                }
                else if (textBox12.Text.Length > 25)
                {
                    MessageBox.Show("Doctor Email depășește lungimea maximă permisă de 25 de caractere.");
                }
                else if (!textBox12.Text.Contains("@"))
                {
                    MessageBox.Show("Doctor Email trebuie să conțină caracterul '@'.");
                }
                else
                {
                    cmd.Parameters.Add("d_email", OracleDbType.Varchar2).Value = textBox12.Text;
                }




                /*VALIDARE SI ADAUGARE SPECIALIZATION*/
                if (string.IsNullOrEmpty(comboBox5.Text))
                {
                    MessageBox.Show("Specialization trebuie sa fie completat.");
                }
                else if (comboBox5.Text.Length > 25)
                {
                    MessageBox.Show("Specialization depășește lungimea maximă permisă de 25 de caractere.");
                }
                else
                {
                    cmd.Parameters.Add("specialization", OracleDbType.Varchar2).Value = comboBox5.Text;
                }

                DateTime currentDate = DateTime.Now;
                DateTime hireDate = dateTimePicker2.Value;
                TimeSpan difference = currentDate - hireDate;
                cmd.Parameters.Add("experience", OracleDbType.Int32).Value = Convert.ToInt32(difference.TotalDays/ 365) ;

                // Execută comanda
                int rowsAffected = cmd.ExecuteNonQuery();

                // Afișează un mesaj dacă inserarea a fost reușită
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Inserarea a fost realizată cu succes.");
                    // Actualizează DataGridView-ul
                    updateDataGrid1("SELECT * FROM DOCTORS ORDER BY DOCTOR_ID");
                }
                else
                {
                    MessageBox.Show("Inserarea a eșuat.");
                }
            }
            catch (OracleException ex)
            {
                // Gestionează excepțiile Oracle
                MessageBox.Show("A apărut o eroare în timpul inserării datelor în baza de date: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Gestionează alte excepții
                MessageBox.Show("A apărut o eroare neașteptată: " + ex.Message);
            }
            finally
            {
                // Închide conexiunea
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                // Deschide conexiunea dacă nu este deja deschisă
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Creează comanda Oracle pentru inserare
                OracleCommand cmd = con.CreateCommand();


                cmd.CommandText = "SELECT patient_id_seq.NEXTVAL FROM dual";
                decimal result = (decimal)cmd.ExecuteScalar();
                long patient_id = Convert.ToInt64(result);

                cmd.CommandText = "INSERT INTO Patients (patient_id, p_first_name, p_last_name, study_id, age, gender, p_number, p_email) " +
                                  "VALUES (:patient_id, :p_first_name, :p_last_name, :study_id, :age, :gender, :p_number, :p_email)";
                cmd.CommandType = CommandType.Text;

                // Adaugă parametrii și valorile acestora din textBox-uri
                cmd.Parameters.Add("patient_id", OracleDbType.Int32).Value = patient_id;



                if(textBox16.Text.Length > 10 )
                {
                    MessageBox.Show("First Name depășește lungimea maximă permisă de 10 caractere.");
                }else if (string.IsNullOrEmpty(textBox16.Text))
                {
                    MessageBox.Show("First Name trebuie sa fie completat.");
                }
                else
                {
                    cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = textBox16.Text;
                }

                if (textBox17.Text.Length > 10)
                {
                    MessageBox.Show("Last Name depășește lungimea maximă permisă de 10 caractere.");
                }
                else if (string.IsNullOrEmpty(textBox17.Text))
                {
                    MessageBox.Show("Last Name trebuie sa fie completat.");
                }
                else
                {
                    cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = textBox17.Text;
                }




                int studyId;
                if (string.IsNullOrEmpty(textBox18.Text))
                {
                    MessageBox.Show("Study ID trebuie să fie completat.");
                }
                // Verificare dacă study_id are maxim 10 cifre
                else if (textBox18.Text.Length > 10)
                {
                    MessageBox.Show("Study ID nu poate depăși 10 cifre.");
                }
                // Verificare dacă study_id există în tabelul Studies
                else
                {
                    if (!int.TryParse(textBox18.Text, out studyId))
                    {
                        MessageBox.Show("Study ID trebuie să fie un număr întreg.");
                    }
                    else
                    {
                        // Verificare existență study_id în tabelul Studies
                        string queryCheckStudyId = "SELECT COUNT(*) FROM Studies WHERE study_id = :studyId";
                        using (OracleCommand checkStudyIdCmd = new OracleCommand(queryCheckStudyId, con))
                        {
                            checkStudyIdCmd.Parameters.Add("studyId", OracleDbType.Int32).Value = studyId;

                            int count = Convert.ToInt32(checkStudyIdCmd.ExecuteScalar());

                            if (count == 0)
                            {
                                MessageBox.Show("Study ID nu există.");
                            }
                            else
                            {
                                // Study ID este valid, adăugare în parametrii pentru comanda SQL
                                cmd.Parameters.Add("study_id", OracleDbType.Int32).Value = studyId;
                            }
                        }
                    }
                }
               
                if(Convert.ToInt32(textBox19.Text) <15)
                {
                    MessageBox.Show("Age trebuie sa fie >= 15 ");
                }else if(string.IsNullOrEmpty(textBox19.Text)) 
                {
                    MessageBox.Show("Age trebuie sa fie completat.");
                }
                else
                {
                    cmd.Parameters.Add("age", OracleDbType.Int32).Value = Convert.ToInt32(textBox19.Text);
                }




                string gender = comboBox8.Text.ToLower();

                if (string.IsNullOrEmpty(gender))
                {
                    MessageBox.Show("Gender trebuie sa fie completat.");
                }
                else
                {
                    cmd.Parameters.Add("gender", OracleDbType.Varchar2).Value = gender;
                }




                /*VALIDARE SI ADAUGARE P_NUMBER*/
                if (string.IsNullOrEmpty(textBox21.Text))
                {
                    MessageBox.Show("Patient Number trebuie sa fie completat.");
                }
                else if (textBox21.Text.Length > 10)
                {
                    MessageBox.Show("Patient Number depășește lungimea maximă permisă de 9 caractere.");
                }
                else
                {
                    cmd.Parameters.Add("p_number", OracleDbType.Int32).Value = Convert.ToInt32(textBox21.Text);
                }






                /* VALIDARE SI ADAUGARE P_EMAIL */
                if (string.IsNullOrEmpty(textBox22.Text))
                {
                    MessageBox.Show("Patient Email trebuie să fie completat.");
                }
                else if (textBox22.Text.Length > 25)
                {
                    MessageBox.Show("Patient Email depășește lungimea maximă permisă de 25 de caractere.");
                }
                else if (!textBox22.Text.Contains("@"))
                {
                    MessageBox.Show("Patient Email trebuie să conțină caracterul '@'.");
                }
                else
                {
                    cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = textBox22.Text;

                }



                // Execută comanda
                int rowsAffected = cmd.ExecuteNonQuery();

                // Afișează un mesaj dacă inserarea a fost reușită
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Inserarea a fost realizată cu succes.");
                    // Actualizează DataGridView-ul
                    updateDataGrid1("SELECT * FROM Patients ORDER BY PATIENT_ID");
                }
                else
                {
                    MessageBox.Show("Inserarea a eșuat.");
                }
            }
            catch (OracleException ex)
            {
                // Gestionează excepțiile Oracle
                MessageBox.Show("A apărut o eroare în timpul inserării datelor în baza de date: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Gestionează alte excepții
                MessageBox.Show("A apărut o eroare neașteptată: " + ex.Message);
            }
            finally
            {
                // Închide conexiunea
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        /*UPDATE STUDIES*/
        private void button8_Click(object sender, EventArgs e)
        {


            try
            {
                // Deschide conexiunea dacă nu este deja deschisă
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Creează comanda Oracle pentru actualizare
                OracleCommand cmd = con.CreateCommand();

                // Construiește comanda SQL de actualizare
                StringBuilder updateQuery = new StringBuilder("UPDATE STUDIES SET ");
                bool isAnyFieldUpdated = false;

                // Actualizează start_date dacă a fost modificat
                if (dateTimePicker1.Value != DateTime.MinValue)
                {
                    updateQuery.Append("start_date = :start_date");
                    cmd.Parameters.Add("start_date", OracleDbType.Date).Value = dateTimePicker1.Value;
                    isAnyFieldUpdated = true;
                }

                // Actualizează location dacă a fost modificat
                if (!string.IsNullOrEmpty(comboBox6.Text))
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("location = :location");
                    cmd.Parameters.Add("location", OracleDbType.Varchar2).Value = comboBox6.Text;
                    isAnyFieldUpdated = true;
                }

                // Actualizează doctor_id dacă a fost modificat
                if (!string.IsNullOrEmpty(textBox4.Text))
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("doctor_id = :doctor_id");
                    cmd.Parameters.Add("doctor_id", OracleDbType.Int32).Value = int.Parse(textBox4.Text);
                    isAnyFieldUpdated = true;
                }

                // Actualizează stage dacă a fost modificat
                if (!string.IsNullOrEmpty(comboBox7.Text))
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("stage = :stage");
                    cmd.Parameters.Add("stage", OracleDbType.Varchar2).Value = comboBox7.Text;
                    isAnyFieldUpdated = true;
                }

                // Actualizează budget dacă a fost modificat
                if (!string.IsNullOrEmpty(textBox6.Text))
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("budget = :budget");
                    cmd.Parameters.Add("budget", OracleDbType.Decimal).Value = decimal.Parse(textBox6.Text);
                    isAnyFieldUpdated = true;
                }

                // Actualizează sponsor dacă a fost modificat
                if (!string.IsNullOrEmpty(textBox7.Text))
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("sponsor = :sponsor");
                    cmd.Parameters.Add("sponsor", OracleDbType.Varchar2).Value = textBox7.Text;
                    isAnyFieldUpdated = true;
                }

                // Adaugă condiția WHERE
                updateQuery.Append(" WHERE study_name = :study_name");
                cmd.Parameters.Add("study_name", OracleDbType.Varchar2).Value = textBox2.Text;

                // Execută comanda de actualizare
                cmd.CommandText = updateQuery.ToString();
                cmd.CommandType = CommandType.Text;
                int rowsAffected = cmd.ExecuteNonQuery();

                

                // Afișează un mesaj dacă actualizarea a fost reușită
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Actualizarea studiului a fost realizată cu succes.");
                    updateDataGrid1("SELECT * FROM STUDIES ORDER BY STUDY_ID;");
                    
                }
                else
                {
                    MessageBox.Show("Actualizarea studiului a eșuat. Verifică datele introduse.");
                }
            }
            catch (OracleException ex)
            {
                // Gestionează excepțiile Oracle
                MessageBox.Show("A apărut o eroare în timpul actualizării datelor în baza de date: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Gestionează alte excepții
                MessageBox.Show("A apărut o eroare neașteptată: " + ex.Message);
            }
            finally
            {
                // Închide conexiunea
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                // Deschide conexiunea dacă nu este deja deschisă
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Creează comanda Oracle pentru actualizare
                OracleCommand cmd = con.CreateCommand();

                // Construiește comanda SQL de actualizare
                StringBuilder updateQuery = new StringBuilder("UPDATE Doctors SET ");
                bool isAnyFieldUpdated = false;

                // Actualizează d_first_name dacă a fost modificat
                

                // Actualizează hire_date dacă a fost modificat
                if (dateTimePicker2.Value >= DateTime.MinValue && dateTimePicker2.Value <= DateTime.MaxValue)
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("hire_date = :hire_date");
                    cmd.Parameters.Add("hire_date", OracleDbType.Date).Value = dateTimePicker2.Value;
                    isAnyFieldUpdated = true;
                }

                // Actualizează d_number dacă a fost modificat
                if (!string.IsNullOrEmpty(textBox11.Text) && textBox11.Text.Length <= 10)
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("d_number = :d_number");
                    cmd.Parameters.Add("d_number", OracleDbType.Int32).Value = Convert.ToInt32(textBox11.Text);
                    isAnyFieldUpdated = true;
                }

                // Actualizează d_email dacă a fost modificat
                if (!string.IsNullOrEmpty(textBox12.Text) && textBox12.Text.Length <= 25 && textBox12.Text.Contains("@"))
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("d_email = :d_email");
                    cmd.Parameters.Add("d_email", OracleDbType.Varchar2).Value = textBox12.Text;
                    isAnyFieldUpdated = true;
                }

                // Actualizează specialization dacă a fost modificat
                if (!string.IsNullOrEmpty(comboBox5.Text) && comboBox5.Text.Length <= 25)
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("specialization = :specialization");
                    cmd.Parameters.Add("specialization", OracleDbType.Varchar2).Value = comboBox5.Text;
                    isAnyFieldUpdated = true;
                }

                // Adaugă condiția WHERE
                updateQuery.Append(" WHERE d_first_name = :d_first_name AND d_last_name=:d_last_name");
                cmd.Parameters.Add(" d_first_name", OracleDbType.Varchar2).Value = textBox9.Text;
                cmd.Parameters.Add(" d_firstlast", OracleDbType.Varchar2).Value =textBox10.Text;


                // Execută comanda de actualizare
                cmd.CommandText = updateQuery.ToString();
                cmd.CommandType = CommandType.Text;
                int rowsAffected = cmd.ExecuteNonQuery();

                // Afișează un mesaj dacă actualizarea a fost reușită
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Actualizarea datelor medicului a fost realizată cu succes.");
                    updateDataGrid1("SELECT * FROM DOCTORS ORDER BY DOCTOR_ID");
                }
                else
                {
                    MessageBox.Show("Actualizarea datelor medicului a eșuat. Verifică datele introduse.");
                }
            }
            catch (OracleException ex)
            {
                // Gestionează excepțiile Oracle
                MessageBox.Show("A apărut o eroare în timpul actualizării datelor în baza de date: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Gestionează alte excepții
                MessageBox.Show("A apărut o eroare neașteptată: " + ex.Message);
            }
            finally
            {
                // Închide conexiunea
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }




        private void button14_Click(object sender, EventArgs e)
        {

            try 
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Creează comanda Oracle pentru actualizare
                OracleCommand cmd = con.CreateCommand();

                // Construiește comanda SQL de actualizare
                StringBuilder updateQuery = new StringBuilder("UPDATE Patients SET ");
                bool isAnyFieldUpdated = false;

                if (!string.IsNullOrEmpty(textBox18.Text) && textBox18.Text.Length <= 25)
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("study_id = :study_id");
                    cmd.Parameters.Add("study_id", OracleDbType.Int32).Value = textBox18.Text;
                    isAnyFieldUpdated = true;
                }

                // Actualizează p_last_name dacă a fost modificat
                if (!string.IsNullOrEmpty(textBox2.Text) && textBox2.Text.Length <= 25)
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("p_last_name = :p_last_name");
                    cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = textBox2.Text;
                    isAnyFieldUpdated = true;
                }

                if (!string.IsNullOrEmpty(textBox19.Text))
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("age = :age");
                    cmd.Parameters.Add("age", OracleDbType.Varchar2).Value = textBox19.Text;
                    isAnyFieldUpdated = true;
                }
                if (!string.IsNullOrEmpty(comboBox8.Text))
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("gender = :gender");
                    cmd.Parameters.Add("gender", OracleDbType.Varchar2).Value = comboBox8.Text;
                    isAnyFieldUpdated = true;
                }


                if (!string.IsNullOrEmpty(textBox21.Text) && textBox21.Text.Length <= 15)
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("p_number = :p_number");
                    cmd.Parameters.Add("p_number", OracleDbType.Varchar2).Value = textBox21.Text;
                    isAnyFieldUpdated = true;
                }


                // Actualizează p_address dacă a fost modificat
                if (!string.IsNullOrEmpty(textBox22.Text) && textBox22.Text.Length <= 25 && textBox22.Text.Contains("@"))
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("p_email = :p_email");
                    cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = textBox22.Text;
                    isAnyFieldUpdated = true;
                }

                // Adaugă condiția WHERE
                updateQuery.Append(" WHERE p_first_name = :p_first_name AND p_last_name = :p_last_name");
                cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = textBox16.Text;
                cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = textBox17.Text;

                // Execută comanda de actualizare
                cmd.CommandText = updateQuery.ToString();
                cmd.CommandType = CommandType.Text;
                int rowsAffected = cmd.ExecuteNonQuery();

                // Afișează un mesaj dacă actualizarea a fost reușită
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Actualizarea datelor pacientului a fost realizată cu succes.");
                    updateDataGrid1("SELECT * FROM PACIENTS ORDER BY PACIENT_ID");
                }
                else
                {
                    MessageBox.Show("Actualizarea datelor pacientului a eșuat. Verifică datele introduse.");
                }
            }
            catch (OracleException ex)
            {
                // Gestionează excepțiile Oracle
                MessageBox.Show("A apărut o eroare în timpul actualizării datelor în baza de date: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Gestionează alte excepții
                MessageBox.Show("A apărut o eroare neașteptată: " + ex.Message);
            }
            finally
            {
                // Închide conexiunea
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Creează comanda Oracle pentru actualizare
                OracleCommand cmd = con.CreateCommand();

                // Construiește comanda SQL de actualizare
                StringBuilder updateQuery = new StringBuilder("UPDATE RESULTS SET ");
                bool isAnyFieldUpdated = false;



                if (!string.IsNullOrEmpty(comboBox4.Text) && comboBox4.Text.Length <= 25)
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("measurement = :measurement");
                    cmd.Parameters.Add("measurement", OracleDbType.Int32).Value = comboBox4.Text;
                    isAnyFieldUpdated = true;
                }

                // Actualizează p_last_name dacă a fost modificat
                if (!string.IsNullOrEmpty(textBox26.Text) && textBox26.Text.Length <= 25)
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("measurement_value = :measurement_value");
                    cmd.Parameters.Add("measurement_value", OracleDbType.Int32).Value = textBox26.Text;
                    isAnyFieldUpdated = true;
                }


                if (!string.IsNullOrEmpty(comboBox1.Text) && comboBox1.Text.Length <= 25)
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("measurement_unit = :measurement_unit");
                    cmd.Parameters.Add("measurement_unit", OracleDbType.Varchar2).Value = comboBox1.Text;
                    isAnyFieldUpdated = true;
                }

                //date
                if (dateTimePicker3.Value >= DateTime.MinValue && dateTimePicker3.Value <= DateTime.MaxValue)
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("data = :data");
                    cmd.Parameters.Add("data", OracleDbType.Date).Value = dateTimePicker3.Value;
                    isAnyFieldUpdated = true;
                }




                if (!string.IsNullOrEmpty(comboBox2.Text))
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("status = :status ");
                    cmd.Parameters.Add("status ", OracleDbType.Varchar2).Value = comboBox2.Text;
                    isAnyFieldUpdated = true;
                }


                if (!string.IsNullOrEmpty(textBox30.Text) && textBox30.Text.Length <= 15)
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("comments = :comments");
                    cmd.Parameters.Add("comments", OracleDbType.Varchar2).Value = textBox30.Text;
                    isAnyFieldUpdated = true;
                }


                // Actualizează p_address dacă a fost modificat
                if (!string.IsNullOrEmpty(comboBox3.Text) && comboBox3.Text.Length <= 15)
                {
                    if (isAnyFieldUpdated)
                        updateQuery.Append(", ");
                    updateQuery.Append("data_source = :data_source");
                    cmd.Parameters.Add("data_source", OracleDbType.Varchar2).Value = comboBox3.Text;
                    isAnyFieldUpdated = true;
                }




                // Adaugă condiția WHERE
                updateQuery.Append(" WHERE study_id = :study_id AND patient_id = :patient_id");
                cmd.Parameters.Add("study_id", OracleDbType.Varchar2).Value = textBox24.Text;
                cmd.Parameters.Add("patient_id", OracleDbType.Varchar2).Value = textBox25.Text;

                // Execută comanda de actualizare
                cmd.CommandText = updateQuery.ToString();
                cmd.CommandType = CommandType.Text;
                int rowsAffected = cmd.ExecuteNonQuery();

                // Afișează un mesaj dacă actualizarea a fost reușită
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Actualizarea datelor pacientului a fost realizată cu succes.");
                    updateDataGrid1("SELECT * FROM RESULTS ORDER BY RESULT_ID");
                }
                else
                {
                    MessageBox.Show("Actualizarea datelor pacientului a eșuat. Verifică datele introduse.");
                }
            }
            catch (OracleException ex)
            {
                // Gestionează excepțiile Oracle
                MessageBox.Show("A apărut o eroare în timpul actualizării datelor în baza de date: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Gestionează alte excepții
                MessageBox.Show("A apărut o eroare neașteptată: " + ex.Message);
            }
            finally
            {
                // Închide conexiunea
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }



        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Creează comanda Oracle pentru ștergere
                OracleCommand cmd = con.CreateCommand();

                // Construiește comanda SQL de ștergere
                StringBuilder deleteQuery = new StringBuilder("DELETE FROM RESULTS ");

                // Adaugă condiția WHERE
                deleteQuery.Append("WHERE study_id = :study_id AND patient_id = :patient_id");
                cmd.Parameters.Add("study_id", OracleDbType.Varchar2).Value = textBox24.Text;
                cmd.Parameters.Add("patient_id", OracleDbType.Varchar2).Value = textBox25.Text;

                // Execută comanda de ștergere
                cmd.CommandText = deleteQuery.ToString();
                cmd.CommandType = CommandType.Text;
                int rowsAffected = cmd.ExecuteNonQuery();

                // Afișează un mesaj dacă ștergerea a fost reușită
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Ștergerea datelor din tabelul RESULTS a fost realizată cu succes.");
                    updateDataGrid1("SELECT * FROM RESULTS ORDER BY RESULT_ID");
                }
                else
                {
                    MessageBox.Show("Ștergerea datelor din tabelul RESULTS a eșuat. Verifică dacă înregistrarea există.");
                }
            }
            catch (OracleException ex)
            {
                // Gestionează excepțiile Oracle
                MessageBox.Show("A apărut o eroare în timpul ștergerii datelor din baza de date: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Gestionează alte excepții
                MessageBox.Show("A apărut o eroare neașteptată: " + ex.Message);
            }
            finally
            {
                // Închide conexiunea
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }


        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Creează comanda Oracle pentru ștergere
                OracleCommand cmd = con.CreateCommand();

                // Construiește comanda SQL de ștergere
                StringBuilder deleteQuery = new StringBuilder("DELETE FROM Patients ");

                // Adaugă condiția WHERE
                deleteQuery.Append("WHERE p_first_name = :p_first_name AND p_last_name = :p_last_name");
                cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = textBox16.Text;
                cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = textBox17.Text;

                // Execută comanda de ștergere
                cmd.CommandText = deleteQuery.ToString();
                cmd.CommandType = CommandType.Text;
                int rowsAffected = cmd.ExecuteNonQuery();

                // Afișează un mesaj dacă ștergerea a fost reușită
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Ștergerea datelor din tabelul Patients a fost realizată cu succes.");
                    updateDataGrid1("SELECT * FROM Patients ORDER BY PATIENT_ID");
                }
                else
                {
                    MessageBox.Show("Ștergerea datelor din tabelul Patients a eșuat. Verifică dacă pacientul există.");
                }
            }
            catch (OracleException ex)
            {
                // Gestionează excepțiile Oracle
                MessageBox.Show("A apărut o eroare în timpul ștergerii datelor din baza de date: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Gestionează alte excepții
                MessageBox.Show("A apărut o eroare neașteptată: " + ex.Message);
            }
            finally
            {
                // Închide conexiunea
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }


        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Creează comanda Oracle pentru ștergere
                OracleCommand cmd = con.CreateCommand();

                // Construiește comanda SQL de ștergere
                StringBuilder deleteQuery = new StringBuilder("DELETE FROM Doctors ");

                // Adaugă condiția WHERE
                deleteQuery.Append("WHERE d_first_name = :d_first_name AND d_last_name = :d_last_name");
                cmd.Parameters.Add("d_first_name", OracleDbType.Varchar2).Value = textBox9.Text;
                cmd.Parameters.Add("d_last_name", OracleDbType.Varchar2).Value = textBox10.Text;

                // Execută comanda de ștergere
                cmd.CommandText = deleteQuery.ToString();
                cmd.CommandType = CommandType.Text;
                int rowsAffected = cmd.ExecuteNonQuery();

                // Afișează un mesaj dacă ștergerea a fost reușită
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Ștergerea datelor din tabelul Doctors a fost realizată cu succes.");
                    updateDataGrid1("SELECT * FROM Doctors ORDER BY DOCTOR_ID");
                }
                else
                {
                    MessageBox.Show("Ștergerea datelor din tabelul Doctors a eșuat. Verifică dacă medicul există.");
                }
            }
            catch (OracleException ex)
            {
                // Gestionează excepțiile Oracle
                MessageBox.Show("A apărut o eroare în timpul ștergerii datelor din baza de date: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Gestionează alte excepții
                MessageBox.Show("A apărut o eroare neașteptată: " + ex.Message);
            }
            finally
            {
                // Închide conexiunea
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Creează comanda Oracle pentru ștergere
                OracleCommand cmd = con.CreateCommand();

                // Construiește comanda SQL de ștergere
                StringBuilder deleteQuery = new StringBuilder("DELETE FROM STUDIES ");

                // Adaugă condiția WHERE
                deleteQuery.Append("WHERE study_name = :study_name");
                cmd.Parameters.Add("study_name", OracleDbType.Varchar2).Value = textBox2.Text;

                // Execută comanda de ștergere
                cmd.CommandText = deleteQuery.ToString();
                cmd.CommandType = CommandType.Text;
                int rowsAffected = cmd.ExecuteNonQuery();

                // Afișează un mesaj dacă ștergerea a fost reușită
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Ștergerea datelor din tabelul STUDIES a fost realizată cu succes.");
                    updateDataGrid1("SELECT * FROM STUDIES ORDER BY STUDY_ID");
                }
                else
                {
                    MessageBox.Show("Ștergerea datelor din tabelul STUDIES a eșuat. Verifică dacă studiul există.");
                }
            }
            catch (OracleException ex)
            {
                // Gestionează excepțiile Oracle
                MessageBox.Show("A apărut o eroare în timpul ștergerii datelor din baza de date: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Gestionează alte excepții
                MessageBox.Show("A apărut o eroare neașteptată: " + ex.Message);
            }
            finally
            {
                // Închide conexiunea
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Questions questionsForm = new Questions();

            // Afișează QuestionsForm
            questionsForm.Show();
        }
    }
}
