using MySql.Data.MySqlClient;
using Student_Info_Mgt_System.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Info_Mgt_System.Student.Util
{
    public partial class Academic_Info : Form
    {
        private Student st;
        private Users.Users user;
        private string id;
        public Academic_Info(Student st)
        {
            InitializeComponent();
            this.st = st;
        }

        public Academic_Info(Users.Users user, string id, string[] opt)
        {
            InitializeComponent();
            this.user = user;
            this.id = id;
            sch.Items.Clear();
            sch.Items.Add(opt[0]);
            sch.SelectedIndex = 0;
            opt = opt.Where((source, index) => index != 0).ToArray();
            dept.Items.Clear();
            foreach (string i in opt)
            {
                if (!string.IsNullOrEmpty(i))
                {
                    dept.Items.Add(i);
                }
            }
            
            dept.SelectedIndex = 0;
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

                    string cmdText = "Select status from rev_stu_aca_info where status=@sta and stuId=@id Limit 1";
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
                        a= true;
                    }
                    else
                    {
                        a= false;
                       
                    }
                    dr.Dispose();
                    dr.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    a= true;

                }
            }
            return a;
        }

        private void sch_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (sch.SelectedIndex == 0)
            {
                dept.Items.Clear();
                dept.Text = "Select Department";
                dept.Items.AddRange(new object[] {
                    "Computer Science",
                    "Statistic"});
            }
            else if (sch.SelectedIndex == 1)
            {
                dept.Items.Clear();
                dept.Text = "Select Department";
                dept.Items.AddRange(new object[] {
                    "Electrical Engineering",
                    "Industrial Safety"});
            }
            else if (sch.SelectedIndex == 2)
            {
                dept.Items.Clear();
                dept.Text = "Select Department";
                dept.Items.AddRange(new object[] {
                    "Petroleum Marketing"});
            }
            else dept.Items.Clear(); 

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
            if (string.IsNullOrEmpty(matNo.Text) || string.IsNullOrEmpty(yos.Text) || string.IsNullOrEmpty(session.Text) || sch.SelectedIndex==-1 || dept.SelectedIndex==-1 || mos.SelectedIndex==-1 || semester.SelectedIndex==-1)
            {
                MessageBox.Show("All fields are required before sending reviews", "Student Academic Information Data");
                return;
            }

            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {

                try
                {

                    string cmdText = "INSERT INTO rev_stu_aca_info (mat_no, sch, dept, year, study_mode, semester, session, status, stuId)" +
                        " VALUES (@mat, @sch, @dpt, @yr, @sm, @sem, @ses, @sta, @stuid);";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@mat", matNo.Text.Trim());
                    cmd.Parameters.AddWithValue("@sch", sch.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@dpt", dept.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@sm", mos.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@sem", semester.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@yr", yos.Text.Trim());
                    cmd.Parameters.AddWithValue("@ses", session.Text.Trim());
                    cmd.Parameters.AddWithValue("@sta", 0);
                    cmd.Parameters.AddWithValue("@stuid", studentId.Trim());
                    int row = cmd.ExecuteNonQuery();

                    if (row > 0)
                    {
                        prompt.Show(this, "Review Request Sent Successfully", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success,
                    4000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft);
                        conn.Close();
                    }
                    else
                    {
                        MessageBox.Show("Request was not sent successfully, try again.", "Student Academic Information Data Request");
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
