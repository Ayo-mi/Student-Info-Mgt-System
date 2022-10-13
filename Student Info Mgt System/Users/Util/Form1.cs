using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Info_Mgt_System.Users.Util
{
    public partial class Form1 : Form
    {
        private string id;
        public Form1(string id, string name)
        {
            InitializeComponent();
            this.id = id;
            this.Text = name + " (" + id + ") Details";
            GetBioData(id);
            GetAcaHis(id);
            GetAcaInfo(id);
            GetNxtofKin(id);

            if (IsCleared(id))
            {
                aiAcpt.Text = "CLEARED";
                bunifuButton21.Text = "CLEARED";
                bunifuButton22.Text = "CLEARED";
                bunifuButton23.Text = "CLEARED";

                aiAcpt.Enabled = false;
                bunifuButton21.Enabled = false;
                bunifuButton22.Enabled = false;
                bunifuButton23.Enabled = false;
            }
        }

        private void GetBioData(string id)
        {
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {

                try
                {
                    string cmdText = "Select * from stu_bio where jamb_num=@id LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    //bool isFound = false;
                    while (dr.Read())
                    {
                                             
                        sNam.Text = string.IsNullOrEmpty(dr["lName"].ToString()) ? "Last Name not set" : dr["lName"].ToString();

                        oNam.Text = string.IsNullOrEmpty(dr["fName"].ToString()) ? "First Name not set" : dr["fName"].ToString();

                        sex.Text = string.IsNullOrEmpty(dr["sex"].ToString()) ? "Gender not set" : dr["sex"].ToString();

                        maritalStatus.Text = string.IsNullOrEmpty(dr["marital_status"].ToString()) ? "Marital Status not set" : dr["marital_status"].ToString();

                        DateTime d = new DateTime();
                        if (!string.IsNullOrEmpty(dr["dob"].ToString()))
                        {
                            d = Convert.ToDateTime(dr["dob"]);
                        }
                        
                        dob.Text = string.IsNullOrEmpty(dr["dob"].ToString()) ? "Date of Birth not set" : d.ToString("dddd, dd MMM yyyy");

                        email.Text = string.IsNullOrEmpty(dr["email"].ToString()) ? "Email Address not set" : dr["email"].ToString();

                        tel.Text = string.IsNullOrEmpty(dr["tel"].ToString()) ? "Mobile Number not set" : dr["tel"].ToString();

                        state.Text = string.IsNullOrEmpty(dr["state"].ToString()) ? "State of Origin not set" : dr["state"].ToString();

                        lga.Text = string.IsNullOrEmpty(dr["lga"].ToString()) ? "Local Govt. Area not set" : dr["lga"].ToString();

                        cou.Text = string.IsNullOrEmpty(dr["country"].ToString()) ? "Country not set" : dr["country"].ToString();

                        addr.Text = string.IsNullOrEmpty(dr["addre"].ToString()) ? "Home Address not set" : dr["addre"].ToString();

                        if (dr["dp"] != DBNull.Value)
                        {
                            byte[] byteImage = (byte[])dr["dp"];
                            using (MemoryStream ms = new MemoryStream(byteImage))
                            {
                                //saving to jpg image
                                Image img = new Bitmap(ms);
                                dp.Image = img;
                            }
                        }
                    }
                    dr.Close();
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Data");
                }
            }
        }

        private void GetNxtofKin(string id)
        {
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {

                try
                {
                    string cmdText = "Select * from stu_nxt_kin where stuId=@id LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    //bool isFound = false;
                    while (dr.Read())
                    {

                        nam.Text = string.IsNullOrEmpty(dr["name"].ToString()) ? "Name not set" : dr["name"].ToString();

                        conAddr.Text = string.IsNullOrEmpty(dr["addre"].ToString()) ? "Contact Address not set" : dr["addre"].ToString();

                        phon.Text = string.IsNullOrEmpty(dr["tel"].ToString()) ? "Contact Phone Number not set" : dr["tel"].ToString();

                        emai.Text = string.IsNullOrEmpty(dr["email"].ToString()) ? "Email Address not set" : dr["email"].ToString();
                    }
                    dr.Close();
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Data");
                }
            }
        }

        private void GetAcaInfo(string id)
        {
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {

                try
                {
                    string cmdText = "Select * from stu_aca_info where stuId=@id LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    //bool isFound = false;
                    while (dr.Read())
                    {

                        matno.Text = string.IsNullOrEmpty(dr["mat_no"].ToString()) ? "Matriculation Number not set" : dr["mat_no"].ToString();

                        sch.Text = string.IsNullOrEmpty(dr["sch"].ToString()) ? "School not set" : dr["sch"].ToString();

                        dpt.Text = string.IsNullOrEmpty(dr["dept"].ToString()) ? "Department not set" : dr["dept"].ToString();

                        yr.Text = string.IsNullOrEmpty(dr["year"].ToString()) ? "Year not set" : dr["year"].ToString();

                        mos.Text = string.IsNullOrEmpty(dr["study_mode"].ToString()) ? "Mode of Study not set" : dr["study_mode"].ToString();

                        sess.Text = string.IsNullOrEmpty(dr["session"].ToString()) ? "Session not set" : dr["session"].ToString();

                        sem.Text = string.IsNullOrEmpty(dr["semester"].ToString()) ? "Semester not set" : dr["semester"].ToString();
                    }
                    dr.Close();
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Data");
                }
            }
        }

        private void GetAcaHis(string id)
        {
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {

                try
                {
                    string cmdText = "Select cert_name1, cert_name2, file1, file2 from stu_aca_his where stuId=@id LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    //bool isFound = false;
                    while (dr.Read())
                    {
                        if (string.IsNullOrEmpty(dr["cert_name1"].ToString()))
                        {
                            button1.Visible = false;
                        }
                        else
                        {
                            button1.Visible = true;
                        }

                        if (string.IsNullOrEmpty(dr["cert_name2"].ToString()))
                        {
                            button2.Visible = false;
                            cert2.Visible = false;
                        }
                        else
                        {
                            button2.Visible = true;
                            cert2.Visible = true;
                        }

                        cert1.Text = string.IsNullOrEmpty(dr["cert_name1"].ToString()) ? "Certificate not set" : dr["cert_name1"].ToString();

                        cert2.Text = string.IsNullOrEmpty(dr["cert_name2"].ToString()) ? "Certificate not set" : dr["cert_name2"].ToString();

                    }
                    dr.Close();
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Data");
                }
            }
        }

        private void getCert(string id)
        {

            DBConnection conn = DBConnection.Instance();
            UInt32 FileSize;
            byte[] rawData;
            FileStream fs;
            string fileName = Path.GetTempFileName() + ".pdf";

            if (conn.IsConnect())
            {

                try
                {
                    string cmdText = "Select file1 from stu_aca_his where stuId=@id LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader myData = cmd.ExecuteReader();

                    if (!myData.HasRows)
                        throw new Exception("There are no File to save");

                    myData.Read();

                    FileSize = 150000;//myData.GetUInt32(100);
                    rawData = new byte[FileSize];

                    myData.GetBytes(myData.GetOrdinal("file1"), 0, rawData, 0, (int)FileSize);

                    fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fs.Write(rawData, 0, (int)FileSize);
                    fs.Close();

                    myData.Dispose();
                    myData.Close();
                    conn.Close();
                    MessageBox.Show("File successfully loaded! Opening file....",
                        "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Process prc = new Process();
                    prc.StartInfo.FileName = fileName;
                    prc.Start();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Data");
                }
            }

        }

        private void getCert2(string id)
        {

            DBConnection conn = DBConnection.Instance();
            UInt32 FileSize;
            byte[] rawData;
            FileStream fs;
            string fileName = Path.GetTempFileName() + ".pdf";

            if (conn.IsConnect())
            {

                try
                {
                    string cmdText = "Select file2 from stu_aca_his where stuId=@id LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader myData = cmd.ExecuteReader();

                    if (!myData.HasRows)
                        throw new Exception("There are no File to save");

                    myData.Read();

                    FileSize = 150000;  // 150kb;
                    rawData = new byte[FileSize];

                    myData.GetBytes(myData.GetOrdinal("file2"), 0, rawData, 0, (int)FileSize);

                    fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fs.Write(rawData, 0, (int)FileSize);
                    fs.Close();
                    myData.Dispose();
                    myData.Close();
                    conn.Close();

                    MessageBox.Show("File successfully loaded! Opening file....",
                        "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Process prc = new Process();
                    prc.StartInfo.FileName = fileName;
                    prc.Start();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Data");
                }
            }

        }

        private bool IsCleared(string id)
        {
            bool cleared = false;

            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {

                try
                {
                    string cmdText = "Select cleared from stu_aca_info where stuId=@id LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    //bool isFound = false;
                    while (dr.Read())
                    {
                        if (dr.GetInt16(0) == 1)
                        {
                            cleared = true;
                        }else if (dr.GetInt16(0) == 0)
                        {
                            cleared = false;
                        }
                    }
                    dr.Close();
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Clear Student");
                }
            }

            return cleared;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            getCert(id);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            getCert2(id);
        }

        private void bunifuButton23_Click(object sender, EventArgs e)
        {
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {

                try
                {
                    string cmdText = "Update stu_aca_info set cleared = @cl where stuId=@id LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@cl", 1);
                    int row = cmd.ExecuteNonQuery();

                    if (row == 1)
                    {
                        aiAcpt.Text = "CLEARED";
                        bunifuButton21.Text = "CLEARED";
                        bunifuButton22.Text = "CLEARED";
                        bunifuButton23.Text = "CLEARED";

                        aiAcpt.Enabled = false;
                        bunifuButton21.Enabled = false;
                        bunifuButton22.Enabled = false;
                        bunifuButton23.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("An error occured, try again", "Clear Student");
                    }              
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Clear Student");
                }
            }
        }
    }
}
