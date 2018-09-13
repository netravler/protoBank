using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// DBS
using System.Data.SqlClient;

namespace proto_Bank
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void getRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getRecordsTestConnection(textBox1.Text, textBox2.Text);
        }

        private void getRecordsTestConnection(string DBTOGRAB, string TBLGRAB)
        {
            String ConnectionString;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            String catstring = "";
            String sqlcommand = "";

            clearRTB1(); // clear the box!

            // setup db-connection
            try
            {
            ConnectionString = @"server=PAUL-HEI-LAP\SQLEXPRESS;user=" + textBox3.Text + ";password=" + textBox4.Text  + ";Integrated Security = True;database=" + DBTOGRAB;
                SqlConnection cn = new SqlConnection(ConnectionString);
                cn.Open();

                sqlcommand = "Select * from " + TBLGRAB;
                //sqlcommand = sqlcommand + holdit + "'";

                SqlDataAdapter transactions_1 = new SqlDataAdapter(sqlcommand, cn);
                transactions_1.Fill(dt);

                // Loop through all entries
                foreach (DataRow drRow in dt.Rows)
                {
                    foreach (var item in drRow.ItemArray)
                    {
                        catstring = catstring + item + " ";
                    }
                    richTextBox1.AppendText(catstring + "\n");
                    catstring = "";
                }

                cn.Close();
            }
            catch 
            {
                MessageBox.Show(this, @"server=PAUL-HEI-LAP\SQLEXPRESS;user=" + textBox3.Text + ";password=" + textBox4.Text + ";Integrated Security = True;database=" + DBTOGRAB, "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void testConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void testConnectionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // my version of testing a connection:
            //                                      open connection
            //                                          check for error
            //                                      close connection

            String ConnectionString;

            clearRTB1(); // clear the box!

            try
            {
                ConnectionString = @"server=PAUL-HEI-LAP\SQLEXPRESS;user=" + textBox3.Text + ";password=" + textBox4.Text + ";Integrated Security = True;database=" + textBox1.Text;
                SqlConnection cn = new SqlConnection(ConnectionString);
                cn.Open();
                cn.Close();
                testConnectionToolStripMenuItem1.Text = "Test Connection - Succeed";
            }
            catch
            {
                testConnectionToolStripMenuItem1.Text = "Test Connection - Fail";
            }

        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ConnectionString;
 
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            String sqlcommand = "";

            // setup db-connection
            try
            {
                ConnectionString = @"server=PAUL-HEI-LAP\SQLEXPRESS;user=" + textBox3.Text + ";password=" + textBox4.Text + ";Integrated Security = True;database=proto_Bank";
                SqlConnection cn = new SqlConnection(ConnectionString);
                cn.Open();

                string [] addRecords;

                foreach (string RTBLines in richTextBox1.Lines)
                {
                    if (RTBLines.Trim() != "")
                    {
                        addRecords = RTBLines.Split(' ');

                        sqlcommand = @"INSERT INTO " + textBox1.Text + @" (ACCTNAME, ACCTID, FNAME, MNAME, LNAME, PRENAME, POSTNAME) VALUES ('" + addRecords[0] + "','" + addRecords[1] + "','" + addRecords[2] + "','" + addRecords[3] + "','" + addRecords[4] + "','" + addRecords[5] + "','" + addRecords[6] + "')";
                        //DEMO DATA: DHEINTZ 10000002 Deborah Dee Heintz pre post 

                        SqlCommand command = new SqlCommand(sqlcommand, cn);

                        command.ExecuteNonQuery();
                    }
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteSQLTableEntry(richTextBox1.SelectedText.ToString(), textBox2.Text);
        }

        private void schemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            clearRTB1(); // clear the box!

             try
            {
                List<string> holdit = new List<string>();
                holdit = grabSchema(textBox2.Text, textBox1.Text);

                foreach (string element in holdit)
                {
                    richTextBox1.AppendText(element + "\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }   

        private List<string> grabSchema(string DTBToGrabSchemaFor, string grabDB)
        {
            String ConnectionString;
            int countIT = 0;

            String [] holdit;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            String catstring = "";
            String sqlcommand = "";

            clearRTB1(); // clear the box!

            try
            {
                List<string> sBase = new List<string>();

                ConnectionString = @"server=PAUL-HEI-LAP\SQLEXPRESS;user=" + textBox3.Text + ";password=" + textBox4.Text + ";Integrated Security = True;database=" + grabDB;
                SqlConnection cn = new SqlConnection(ConnectionString);
                cn.Open();

                sqlcommand = "exec sp_columns @table_name = " + DTBToGrabSchemaFor;

                SqlDataAdapter transactions_1 = new SqlDataAdapter(sqlcommand, cn);
                transactions_1.Fill(dt);

                // Loop through all entries
                foreach (DataRow drRow in dt.Rows)
                {
                    foreach (var item in drRow.ItemArray)
                    {
                        catstring = catstring + item + " ";
                    }

                    holdit = catstring.Split(' ');

                    sBase.Add(holdit[3]); 

                    countIT += 1;

                    catstring = "";
                }

                cn.Close();
                return sBase;

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        private void clearRTB1()
        {
            // kind of a stupid function but might need it in the future...
            richTextBox1.Clear();
        }

        private void deleteSQLTableEntry(string findThisAndKill, string killTable)
        {
            string ConnectionString;

            String sqlcommand = "";

            // setup db-connection
            try
            {
                ConnectionString = @"server=PAUL-HEI-LAP\SQLEXPRESS;user=" + textBox3.Text + ";password=" + textBox4.Text + ";Integrated Security = True;database=" + textBox1.Text;
                SqlConnection cn = new SqlConnection(ConnectionString);
                cn.Open();

                sqlcommand = @"DELETE FROM " + killTable + @" WHERE ACCTNAME = '" + findThisAndKill + "'";                       
                //DEMO DATA: DHEINTZ 10000002 Deborah Dee Heintz pre post 
                
                SqlCommand command = new SqlCommand(sqlcommand, cn);
                command.ExecuteNonQuery();
                
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void testFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // testing
            // run and get the schema from ms sql
            List<string> holdit = new List<string>();
            holdit = grabSchema(textBox2.Text, textBox1.Text);

            // keeping it simple for test by moving list into string []
            string[] dingding = holdit.ToArray();

            string [] biffers = makeRecordEditAddDeleteForm("test", dingding);
        }

        void rtxtExt1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void txtExt1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private string [] makeRecordEditAddDeleteForm(string formName, string [] schemaRef)
        {
            using (Form formNew = new Form())
            {

                int offset = 0;

                formNew.Text = formName.ToString();
                formNew.Size = new System.Drawing.Size(800, 400);


                foreach (string lineS in schemaRef)
                {
                    addtxtBox(lineS ,10 , 25 + offset, 150, 20, formNew);
                    offset += 25;
                }

                formNew.ShowDialog();
                return null;
            }
        }

        private void addtxtBox(string txtBoxName,int locationL, int locationT, int sizeL, int sizeLen, Form parentForm)
        {
            TextBox txtExt1 = new TextBox();
            {
                // textbox
                txtExt1.Name = txtBoxName.ToString();
                txtExt1.Text = txtBoxName.ToString();
                txtExt1.Location = new System.Drawing.Point(locationL, locationT);
                txtExt1.Size = new System.Drawing.Size(sizeL, sizeLen);

                //txtExt1.MouseDoubleClick += new MouseEventHandler(New_DoubleMouseClick);
                txtExt1.DoubleClick += delegate(object sender, EventArgs e)
                {
                    MessageBox.Show("this is a test!!! " + txtExt1.Name);
                };

                parentForm.Controls.Add(txtExt1);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
