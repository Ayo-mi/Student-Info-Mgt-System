using Bunifu.UI.WinForms;
using MySql.Data.MySqlClient;
using Student_Info_Mgt_System.Student.Util;
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

namespace Student_Info_Mgt_System.Student
{
    public partial class Student : Form
    {
        private BunifuPanel buttonBorderPanel;
        private BunifuLabel currentButton;
        private Main currentParent;
        private Bio bio;
        private AcademicHistory academic_History;
        private NextOfKin next_Of_Kin;
        private Academic_Info academic_Info;

        public Student(Main p, string userId)
        {
            InitializeComponent();
            currentParent = p;
            buttonBorderPanel = new BunifuPanel();
            buttonBorderPanel.Size = new Size(28, 5);
            bunifuPanel1.Controls.Add(buttonBorderPanel);
            bio = new Bio(this);
            academic_History = new AcademicHistory(this);
            next_Of_Kin = new NextOfKin(this);
            academic_Info = new Academic_Info(this);
            this.StuId = userId;
        }

        public string StuId { get; }

        protected void ActiveButton(Object senderbtn)
        {
            if (senderbtn != null)
            {

                currentButton = (BunifuLabel)senderbtn;

                //left boarder button
                currentButton.ForeColor = Color.DodgerBlue;
                buttonBorderPanel.Size = new Size(currentButton.Size.Width - 10, 5);
                buttonBorderPanel.Location = new Point(currentButton.Location.X + 5, 37);
                buttonBorderPanel.ShowBorders = true;
                buttonBorderPanel.BorderRadius = 3;
                buttonBorderPanel.BorderThickness = 0;
                buttonBorderPanel.BorderColor = Color.DodgerBlue;
                buttonBorderPanel.BackgroundColor = Color.DodgerBlue;
                buttonBorderPanel.Visible = true;
                buttonBorderPanel.BringToFront();
                //currentButton.Focus();

            }
        }

        public void getCert(string id)
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

        public void getCert2(string id)
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

        private void btnTab1_Click(object sender, EventArgs e)
        {
            PrepareStudentData();
            btnTab2.ForeColor = Color.Black;
            bunifuPages1.SetPage(0);
            ActiveButton(sender);
        }

        private void btnTab2_Click(object sender, EventArgs e)
        {
            btnTab1.ForeColor = Color.Black;
            bunifuPages1.SetPage(1);
            ActiveButton(sender);
            studentFormPanel.Controls.Add(bio.box);
            bio.JambId = StuId.ToUpper();
            studentFormPanel.Controls.Add(academic_History.box);
            studentFormPanel.Controls.Add(next_Of_Kin.box);
            studentFormPanel.Controls.Add(academic_Info.box);
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
                        jamb_id.Text = dr["jamb_num"].ToString();
                        sNam.Text = string.IsNullOrEmpty(dr["lName"].ToString()) ? "Last Name not set" : dr["lName"].ToString();

                        oNam.Text = string.IsNullOrEmpty(dr["fName"].ToString()) ? "First Name not set" : dr["fName"].ToString();

                        sex.Text = string.IsNullOrEmpty(dr["sex"].ToString()) ? "Gender not set" : dr["sex"].ToString();

                        maritalStatus.Text = string.IsNullOrEmpty(dr["marital_status"].ToString()) ? "Marital Status not set" : dr["marital_status"].ToString();

                        dob.Text = string.IsNullOrEmpty(dr["dob"].ToString()) ? "Date of Birth not set" : dr["dob"].ToString();

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

        private void PrepareStudentData()
        {
            GetBioData(StuId);
            GetNxtofKin(StuId);
            GetAcaInfo(StuId);
            GetAcaHis(StuId);
        }

        private void student_Shown(object sender, EventArgs e)
        {
            ActiveButton(btnTab1);
            btnTab2.ForeColor = Color.Black;
            PrepareStudentData();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            currentParent.Pages1.SetPage(0);
            currentParent.Pages1.BringToFront();
            this.Close();
        }

        private void signinbtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(psw1.Text) || string.IsNullOrEmpty(psw2.Text))
            {
                MessageBox.Show("All fields are required", "Change Password");
                return;
            }
            else if (psw1.Text != psw2.Text)
            {
                MessageBox.Show("Password does not match", "Change Password");
                return;
            }
            else if (psw1.Text.Length < 6)
            {
                MessageBox.Show("Password can't be less than six (6) characters", "Change Password");
                return;
            }

            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {

                try
                {
                    string cmdText = "Update login_details set password=@psw where userId=@id LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", StuId);
                    cmd.Parameters.AddWithValue("@psw", psw1.Text.Trim());

                    int row = cmd.ExecuteNonQuery();

                    if (row > 0)
                    {
                        MessageBox.Show("Password Changed Successfully", "Change Password");
                    }
                    else
                    {
                        MessageBox.Show("An error occur. Try again", "Change Password");
                    }
                 
                    conn.Close();
                }
                catch (MySqlException mse)
                {
                    MessageBox.Show(mse.Message, "Change PAssword");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            getCert(StuId);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            getCert2(StuId);
        }
    }
}
