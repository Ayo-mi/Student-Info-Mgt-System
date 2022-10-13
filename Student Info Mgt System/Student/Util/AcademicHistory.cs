using MySql.Data.MySqlClient;
using Student_Info_Mgt_System.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Info_Mgt_System.Student.Util
{
    public partial class AcademicHistory : Form
    {
        private Student st;
        private Users.Users user;
        private byte[] file1;
        private byte[] file2;
        private string id;
        public AcademicHistory(Student st)
        {
            InitializeComponent();
            this.st = st;
        }

        public AcademicHistory(Users.Users user, string id)
        {
            InitializeComponent();
            this.user = user;
            this.id = id;
        }

        public Bunifu.UI.WinForms.BunifuGroupBox box
        {
            get { return bunifuGroupBox1; }
        }

        private bool isRequested(string id)
        {
            DBConnection conn = DBConnection.Instance();
            bool a = true;
            if (conn.IsConnect())
            {

                try
                {

                    string cmdText = "Select status from rev_stu_aca_his where status=@sta and stuId=@id Limit 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", id.Trim());
                    cmd.Parameters.AddWithValue("@sta", 0);

                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        if (st == null && user != null)
                        {
                            MessageBox.Show("Student have a pending review request, wait till it's\n approved/reject before making another request", "Review Request");
                        }
                        else if (user == null && st != null)
                        {
                            MessageBox.Show("You have a pending review request, wait till it's\n approved/reject before making another request", "Review Request");
                        }
                        dr.Dispose();
                        dr.Close();
                        conn.Close();
                        a = true;
                    }
                    else
                    {
                        a = false;
                    }

                    dr.Dispose();
                    dr.Close();
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    a = true;
                }

            }
            return a;
        }

        private string RainCheck()
        {
            string studentId="";
            if (st == null && user != null)
            {
                studentId = id;
            }
            else if (user == null && st != null)
            {
                studentId = st.StuId;
            }
            else
            {
                studentId = Microsoft.VisualBasic.Interaction.InputBox("Enter student jamb registration number", "Enter Student's ID Number");
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {
                    try
                    {
                        string cmdText = "Select id from stu_bio where jamb_num=@id LIMIT 1";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", studentId);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        bool isFound = false;
                        while (reader.Read())
                        {
                            isFound = true;
                        }

                        reader.Close();
                        conn.Close();
                        if (!isFound)
                        {
                            MessageBox.Show("Invalid student ID or doesn't exist", "Modify Data");
                            
                        }
                    }

                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                        conn.Close();
                    }
                }
            }
            if (studentId == "")
            {
                return "";
            }
            else return studentId;
        }

        private byte[] prepareFile(string filePath)
        {
            byte[] file=null;
            try
            {
                file = File.ReadAllBytes(filePath);
            }catch(IOException e)
            {
                MessageBox.Show("An Error occur! Try Again.", "ERROR");
            }
            return file;
        }

        private byte[] prepareFile2(string filePath)
        {
            byte[] file = null;
            try
            {
                file = File.ReadAllBytes(filePath);
            }
            catch (IOException e)
            {
                MessageBox.Show("An Error occur! Try Again.", "ERROR");
            }
            return file;
        }

        private void bunifuRadioButton1_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (radio1.Checked || radio2.Checked || radio3.Checked)
            {
                bunifuGroupBox1.Controls.Add(panel1);
                panel1.Location = new Point(41, 145);
                panel1.Visible = true;
                bunifuGroupBox1.Size = new Size(934, 256+36);
            }
            else
            {
                panel1.Visible=false;
            }
        }

        private void radio4_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (radio4.Checked)
            {
                bunifuGroupBox1.Controls.Add(panel2);
                panel2.Location = new Point(41, 145);
                panel2.Visible = true;
                bunifuGroupBox1.Size = new Size(934, 313+36);
            }
            else
            {
                panel2.Visible = false;
            }
        }

        private void radio5_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (radio5.Checked)
            {
                bunifuGroupBox1.Size = new Size(934, 313+36);
                bunifuGroupBox1.Controls.Add(panel3);
                panel3.Location = new Point(41, 145);
                //panel3.Dock = DockStyle.Bottom;
                panel3.Visible = true;
            }
            else
            {
                panel3.Visible = false;
            }
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "PDF file | *.pdf";
                
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fileSize = new FileInfo(openFileDialog1.FileName);
                    if(fileSize.Length <= 150000)
                    {
                        fileName.Text = openFileDialog1.SafeFileName;
                        //filepath1 = openFileDialog1.FileName;

                        //FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                        //BinaryReader bs = new BinaryReader(fs);
                        file1 = prepareFile(openFileDialog1.FileName);
                        //fs.Close();
                        //bs.Close();
                    }
                    else
                    {
                       MessageBox.Show("File size must be not be more than 150kb", "Uploade Certification File");
                    }   
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Uploade Certification File");
            }
        }

        private void signinbtn_Click(object sender, EventArgs e)
        {
            string studentId;
            
            if (st == null && user != null)
            {
                studentId = id;
            }
            else if (user == null && st != null)
            {
                studentId = st.StuId;
            }
            else
            {
                studentId = Microsoft.VisualBasic.Interaction.InputBox("Enter student jamb registration number", "Enter Student's ID Number");
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {
                    try
                    {
                        string cmdText = "Select id from stu_bio where jamb_num=@id LIMIT 1";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", studentId);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        bool isFound = false;
                        while (reader.Read())
                        {
                            isFound = true;
                        }

                        reader.Close();
                        conn.Close();
                        if (!isFound)
                        {
                            MessageBox.Show("Invalid student ID or doesn't exist", "Modify Data");
                            return;
                        }
                    }

                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                        conn.Close();
                    }
                }
            }
            if (isRequested(studentId)) return;
            string certType = "";
            if (radio1.Checked)
            {
                certType = "WAEC";
            }else if (radio2.Checked)
            {
                certType = "NECO";
            }else if (radio3.Checked)
            {
                certType = "GCE";
            }
            else
            {
                certType = "";
            }

            if(certType=="" || file1==null || file1.Length<=0)
            {
                MessageBox.Show("Upload your Result file and select a certificate examination board", "Academic History");
                return;
            }

            try
            {
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {

                    try
                    {

                        string cmdText = "INSERT INTO rev_stu_aca_his (cert_name1, file1, status, stuId)" +
                            " VALUES (@name, @file, @sta, @stuid);";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@name", certType.Trim());                     
                        cmd.Parameters.AddWithValue("@file", file1);                     
                        cmd.Parameters.AddWithValue("@sta", 0);
                        cmd.Parameters.AddWithValue("@stuid", studentId.Trim());
                        int row = cmd.ExecuteNonQuery();

                        if (row > 0)
                        {
                            prompt.Show(this, "Review Request Sent Successfully", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success,
                        4000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft);
                            conn.Close();
                            file1 = null;
                            file2 = null;
                        }
                        else
                        {
                            MessageBox.Show("Request was not sent successfully, try again.", "Student Academics History Data Request");
                        }

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Request Data");
                    }

                }
            }
            catch(Exception ex)
            {

            }
        }

        private void bunifuButton22_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "PDF file | *.pdf";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fileSize = new FileInfo(openFileDialog1.FileName);
                    if (fileSize.Length <= 150000)
                    {
                        othlbl.Text = openFileDialog1.SafeFileName;
                        //filepath1 = openFileDialog1.FileName;

                        //FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                        //BinaryReader bs = new BinaryReader(fs);
                        file1 = prepareFile(openFileDialog1.FileName);
                        //fs.Close();
                        //bs.Close();
                    }
                    else
                    {
                        MessageBox.Show("File size must be not be more than 150kb", "Uploade Certification File");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuButton25_Click(object sender, EventArgs e)
        {
            string studentId;
            if (st == null && user != null)
            {
                studentId = id;
            }
            else if (user == null && st != null)
            {
                studentId = st.StuId;
            }
            else
            {
                studentId = Microsoft.VisualBasic.Interaction.InputBox("Enter student jamb registration number", "Enter Student's ID Number");
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {
                    try
                    {
                        string cmdText = "Select id from stu_bio where jamb_num=@id LIMIT 1";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", studentId);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        bool isFound = false;
                        while (reader.Read())
                        {
                            isFound = true;
                        }

                        reader.Close();
                        conn.Close();
                        if (!isFound)
                        {
                            MessageBox.Show("Invalid student ID or doesn't exist", "Modify Data");
                            return;
                        }
                    }

                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                        conn.Close();
                    }
                }
            }

            if (isRequested(studentId)) return;
            string certType = otherCert.Text;

            if (certType == "" || file1 == null || file1.Length <= 0)
            {
                MessageBox.Show("Upload your Result file and select a certificate examination board", "Academic History");
                return;
            }

            try
            {
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {

                    try
                    {

                        string cmdText = "INSERT INTO rev_stu_aca_his (cert_name1, file1, status, stuId)" +
                            " VALUES (@name, @file, @sta, @stuid);";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@name", certType.Trim());
                        cmd.Parameters.AddWithValue("@file", file1);
                        cmd.Parameters.AddWithValue("@sta", 0);
                        cmd.Parameters.AddWithValue("@stuid", studentId.Trim());
                        int row = cmd.ExecuteNonQuery();

                        if (row > 0)
                        {
                            prompt.Show(this, "Review Request Sent Successfully", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success,
                        4000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft);
                            conn.Close();
                            file1 = null;
                        }
                        else
                        {
                            MessageBox.Show("Request was not sent successfully, try again.", "Student Academics History Data Request");
                        }

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void bunifuButton23_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "PDF file | *.pdf";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fileSize = new FileInfo(openFileDialog1.FileName);
                    if (fileSize.Length <= 150000)
                    {
                        fi1.Text = openFileDialog1.SafeFileName;
                        //filepath1 = openFileDialog1.FileName;

                        //FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                        //BinaryReader bs = new BinaryReader(fs);
                        file1 = prepareFile(openFileDialog1.FileName);
                        //fs.Close();
                        //bs.Close();
                    }
                    else
                    {
                        MessageBox.Show("File size must be not be more than 150kb", "Uploade Certification File 1");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuButton26_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "PDF file | *.pdf";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fileSize = new FileInfo(openFileDialog1.FileName);
                    if (fileSize.Length <= 150000)
                    {
                        fi2.Text = openFileDialog1.SafeFileName;
                        //filepath1 = openFileDialog1.FileName;

                        //FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                        //BinaryReader bs = new BinaryReader(fs);
                        file2 = prepareFile2(openFileDialog1.FileName);
                        //fs.Close();
                        //bs.Close();
                    }
                    else
                    {
                        MessageBox.Show("File size must be not be more than 150kb", "Uploade Certification File");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuButton24_Click(object sender, EventArgs e)
        {
            string studentId;
            if (st == null && user != null)
            {
                studentId = id;
            }
            else if (user == null && st != null)
            {
                studentId = st.StuId;
            }
            else
            {
                studentId = Microsoft.VisualBasic.Interaction.InputBox("Enter student jamb registration number", "Enter Student's ID Number");
                DBConnection con = DBConnection.Instance();

                if (con.IsConnect())
                {
                    try
                    {
                        string cmdText = "Select id from stu_bio where jamb_num=@id LIMIT 1";
                        MySqlCommand cmd = new MySqlCommand(cmdText, con.Connection);
                        cmd.Parameters.AddWithValue("@id", studentId);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        bool isFound = false;
                        while (reader.Read())
                        {
                            isFound = true;
                        }

                        reader.Close();
                        con.Close();
                        if (!isFound)
                        {
                            MessageBox.Show("Invalid student ID or doesn't exist", "Modify Data");
                            return;
                        }
                    }

                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                        con.Close();
                    }
                }
            }

            if (isRequested(studentId)) return;
            string [] inpu = twoSitn.Text.Trim().Split(',');
            if (inpu.Length < 2 || inpu.Length > 2 || file1 == null || file1.Length <= 0 || file2 == null || file2.Length <=0)
            {
                MessageBox.Show("Certificate must be 2, enter their names and separate them with a comma and updload the files");
                return;
            }

            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {

                try
                {

                    string cmdText = "INSERT INTO rev_stu_aca_his (cert_name1, cert_name2, file1, file2, status, stuId)" +
                            " VALUES (@name, @name2, @file, @file2, @sta, @stuid);";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@name", inpu[0].Trim());
                    cmd.Parameters.AddWithValue("@name2", inpu[1].Trim());
                    cmd.Parameters.AddWithValue("@file", file1);
                    cmd.Parameters.AddWithValue("@file2", file2);
                    cmd.Parameters.AddWithValue("@sta", 0);
                    cmd.Parameters.AddWithValue("@stuid", studentId.Trim());
                    int row = cmd.ExecuteNonQuery();

                    if (row > 0)
                    {
                        prompt.Show(this, "Review Request Sent Successfully", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success,
                    4000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft);
                        conn.Close();
                        file1 = null;
                    }
                    else
                    {
                        MessageBox.Show("Request was not sent successfully, try again.", "Student Academics History Data Request");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
    }
}
