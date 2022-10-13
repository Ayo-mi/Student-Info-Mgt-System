using Bunifu.UI.WinForms;
using Student_Info_Mgt_System.Student.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;

namespace Student_Info_Mgt_System.Users
{
    public partial class Users : Form
    {
        private BunifuPanel buttonBorderPanel;
        private BunifuLabel currentButton;
        private Main currentParent;
        private Bio bio;
        private AcademicHistory academic_History;
        private NextOfKin next_Of_Kin;
        private Academic_Info academic_Info;
        private string id;
        private int acctType;
        private string studId;
        private int cl = 0;

        public Users(Main p, string id, int acctType)
        {
            InitializeComponent();
            currentParent = p;
            buttonBorderPanel = new BunifuPanel();
            buttonBorderPanel.Size = new Size(28, 5);
            bunifuPanel1.Controls.Add(buttonBorderPanel);
            
            this.id = id;
            this.acctType = acctType;
            if (acctType == 1)
            {
                p.Text = "School - " + p.Text;
            }else if(acctType == 2)
            {
                p.Text = "Department - " + p.Text;
            }
        }

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

        private string Getname(string id)
        {
            DBConnection con = DBConnection.Instance();
            string name = "";
            if (con.IsConnect())
            {

                try
                {
                    string cmdText = "Select name from users where userID=@id LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, con.Connection);
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader dr = cmd.ExecuteReader();
                    bool isFound = false;
                    while (dr.Read())
                    {
                        name = dr["name"].ToString();
                    }
                    dr.Close();
                    con.Close();
                }
                
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Name");
                }
            }
            return name;
        }

        private string[] DropdownOptions(string name)
        {
            string[] result = new string[3];

            if(acctType==2 && (name == "Computer Science" || name == "Statistic"))
            {
                result[0] = "Applied Science";
                result[1] = name;
            }
            else if(acctType == 2 && (name == "Electrical Engineering" || name == "Industrial Safety"))
            {
                result[0] = "Engineering Technology";
                result[1] = name;
            }
            else if (acctType == 2 && name == "Petroleum Marketing")
            {
                result[0] = "Petroleum and Business Technology";
                result[1] = name;
            }
            else if(acctType==1 && name == "Applied Science")
            {
                result[0] = name;
                result[1] = "Computer Science";
                result[2] = "Statistic";
            }
            else if(acctType==1 && name == "Engineering Technology")
            {
                result[0] = name;
                result[1] = "Electrical Engineering";
                result[2] = "Industrial Safety";
            }
            else if(acctType==1 && name == "Petroleum and Business Technology")
            {
                result[0] = name;
                result[1] = "Petroleum Marketing";
            }
            else
            {
                result[0] = "";
            }

            return result;
        }

        private int TotalStu(int acctType, string nam)
        {
            int count = 0;
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText="";
                    if (acctType == 1)
                    {
                        cmdText = "Select COUNT(*) as count from stu_aca_info where sch = @name";
                    }
                    else if(acctType == 2)
                    {
                        cmdText = "Select COUNT(*) as count from stu_aca_info where dept=@name";
                    }
                     
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@name", nam);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        count = Convert.ToInt32(reader.GetString(0));
                    }
                    reader.Dispose();
                    reader.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Total Student");
                }

            }
            return count;
        }
        
        private int RegisteredStu(int acctType, string nam)
        {
            int count = 0;
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText="";
                    if (acctType == 1)
                    {
                        cmdText = "SELECT count(*) FROM stu_aca_info i where i.mat_no<>'' and i.sch<>'' and i.dept<>'' and i.year<>'' " +
"and i.study_mode<>'' and i.semester<>'' and i.session<>'' and i.sch=@name;";
                    }
                    else if(acctType == 2)
                    {
                        cmdText = "SELECT count(*) FROM stu_aca_info i where i.mat_no<>'' and i.sch<>'' and i.dept<>'' and i.year<>'' " +
"and i.study_mode<>'' and i.semester<>'' and i.session<>'' and i.dept=@name;";
                    }
                     
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@name", nam);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        count = Convert.ToInt32(reader.GetString(0));
                    }
                    reader.Dispose();
                    reader.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Total Resgistered Student");
                }

            }
            return count;
        }

        private int TotalClearedStu(int acctType, string nam)
        {
            int count = 0;
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "";
                    if (acctType == 1)
                    {
                        cmdText = "Select COUNT(*) as count from stu_aca_info where sch = @name and cleared=@cl";
                    }
                    else if (acctType == 2)
                    {
                        cmdText = "Select COUNT(*) as count from stu_aca_info where dept=@name and cleared=@cl";
                    }

                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@name", nam);
                    cmd.Parameters.AddWithValue("@cl", 1);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        count = Convert.ToInt32(reader.GetString(0));
                    }
                    reader.Dispose();
                    reader.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Total Cleared Student");
                }

            }
            return count;
        }

        private int TotalUnclearedStu(int acctType, string nam)
        {
            int count = 0;
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "";
                    if (acctType == 1)
                    {
                        cmdText = "Select COUNT(*) as count from stu_aca_info where sch = @name and cleared=@cl";
                    }
                    else if (acctType == 2)
                    {
                        cmdText = "Select COUNT(*) as count from stu_aca_info where dept=@name and cleared=@cl";
                    }

                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@name", nam);
                    cmd.Parameters.AddWithValue("@cl", 0);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        count = Convert.ToInt32(reader.GetString(0));
                    }
                    reader.Dispose();
                    reader.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Total Uncleared Student");
                }

            }
            return count;
        }

        private void PopulatePieChart()
        {
            if (RegisteredStu(acctType, Getname(id))==0 && TotalClearedStu(acctType, Getname(id))==0 && TotalUnclearedStu(acctType, Getname(id))==0)
            {
                panel1.Visible = false;
                return;
            }
            else
            {
                panel1.Visible = true;
            }
            List<double> statistics = new List<double>();

            double[] val = { RegisteredStu(acctType, Getname(id)), TotalClearedStu(acctType, Getname(id)), TotalUnclearedStu(acctType, Getname(id)) };

            statistics.AddRange(val);
            
            bunifuPieChart1.Data = statistics;
        }

        private void getStudents(int acctType, string name)
        {
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "";
                    if (acctType == 1)
                    {
                        cmdText = "SELECT b.jamb_num id, b.fName fN, b.lName lN, i.mat_no mN, i.dept d, i.sch sch FROM stu_mgt_db.stu_bio b LEFT JOIN stu_aca_info i ON b.jamb_num = i.stuId where i.sch=@na;";
                    }else if (acctType == 2)
                    {
                        cmdText = "SELECT b.jamb_num id, b.fName fN, b.lName lN, i.mat_no mN, i.dept d, i.sch sch FROM stu_mgt_db.stu_bio b LEFT JOIN stu_aca_info i ON b.jamb_num = i.stuId where i.dept=@na;";
                    }
                    
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@na", name);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    bunifuDataGridView1.Rows.Clear();
                    int i = 1;
                    while (reader.Read())
                    {
                        bunifuDataGridView1.Rows.Add(i.ToString(), reader["fN"].ToString() + " " + reader["lN"].ToString(), reader["mN"].ToString(), reader["sch"].ToString(), reader["d"].ToString(), reader["id"].ToString());
                        i++;
                    }
                    reader.Dispose();
                    reader.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Students");
                }

            }
        }

        private void getCl_UnStudents(int acctType, string name, int stat)
        {
            DBConnection conn = DBConnection.Instance();

            if (stat == 0)
            {
                bunifuLabel7.Text = "List of Uncleared Students";
            }else if (stat == 1)
            {
                bunifuLabel7.Text = "List of Cleared Students";
            }
            else
            {
                MessageBox.Show("Invalid Request", "ERROR!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "";
                    if (acctType == 1)
                    {
                        cmdText = "SELECT b.jamb_num id, b.fName fN, b.lName lN, i.mat_no mN, i.dept d, i.sch sch FROM stu_mgt_db.stu_bio b LEFT JOIN stu_aca_info i ON b.jamb_num = i.stuId where i.sch=@na and i.cleared=@st;";
                    }
                    else if (acctType == 2)
                    {
                        cmdText = "SELECT b.jamb_num id, b.fName fN, b.lName lN, i.mat_no mN, i.dept d, i.sch sch FROM stu_mgt_db.stu_bio b LEFT JOIN stu_aca_info i ON b.jamb_num = i.stuId where i.dept=@na and i.cleared=@st;";
                    }

                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@na", name);
                    cmd.Parameters.AddWithValue("@st", stat);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    bunifuDataGridView2.Rows.Clear();
                    int i = 1;
                    while (reader.Read())
                    {
                        bunifuDataGridView2.Rows.Add(i.ToString(), reader["fN"].ToString() + " " + reader["lN"].ToString(), reader["mN"].ToString(), reader["sch"].ToString(), reader["d"].ToString(), reader["id"].ToString());
                        i++;
                    }
                    reader.Dispose();
                    reader.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Students");
                }

            }
        }

        private void users_Shown(object sender, EventArgs e)
        {
            ActiveButton(btnTab1);
            btnTab2.ForeColor = Color.Black;
        }

        private void btnTab1_Click(object sender, EventArgs e)
        {
            btnTab2.ForeColor = Color.Black;
            label2.ForeColor = Color.Black;

            stuNo.Text = TotalStu(acctType, Getname(id)).ToString();
            clNo.Text = TotalClearedStu(acctType, Getname(id)).ToString();
            unclNo.Text = TotalUnclearedStu(acctType, Getname(id)).ToString();
            PopulatePieChart();

            bunifuPages1.SetPage(0);
            ActiveButton(sender);
        }
         
        private void btnTab2_Click(object sender, EventArgs e)
        {
            studId = Interaction.InputBox("Enter student jamb registration number", "Enter Student's ID Number");
            if (string.IsNullOrEmpty(studId)) return;
            string nam = Getname(id);

            bio = new Bio(this, studId);
            next_Of_Kin = new NextOfKin(this, studId);
            academic_Info = new Academic_Info(this, studId, DropdownOptions(Getname(id)));
            academic_History = new AcademicHistory(this, studId);

            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {

                try
                {
                    string cmdText = "Select dept, sch from stu_aca_info where stuId=@id LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", studId);
                    //cmd.Parameters.AddWithValue("@psw", pswTxt.Text);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    bool isFound = false;
                    while (reader.Read())
                    {
                        isFound = true;
                        if(acctType == 1)
                        {
                            if(reader["sch"].ToString().Equals(nam) || string.IsNullOrEmpty(reader["sch"].ToString()))
                            {
                                btnTab1.ForeColor = Color.Black;
                                label2.ForeColor = Color.Black;
                                bunifuPages1.SetPage(1);
                                ActiveButton(sender);
                                studentFormPanel.Controls.Add(bio.box);
                                bio.JambId = studId.ToUpper();
                                studentFormPanel.Controls.Add(academic_History.box);
                                studentFormPanel.Controls.Add(next_Of_Kin.box);
                                studentFormPanel.Controls.Add(academic_Info.box);
                                reader.Close();
                                conn.Close();
                                return;
                            }
                            else if (!(string.IsNullOrEmpty(reader.GetString(1)) || reader.GetString(1).Equals(nam)))
                            {
                                MessageBox.Show("You don't have the privilege to make modification\nrequest for this student", "Modify Data");
                                reader.Close();
                                conn.Close();
                                return;
                            }
                        }
                        else if(acctType == 2)
                        {
                            if (reader["dept"].ToString().Equals(nam) || string.IsNullOrEmpty(reader["dept"].ToString()))
                            {
                                btnTab1.ForeColor = Color.Black;
                                label2.ForeColor = Color.Black;
                                bunifuPages1.SetPage(1);
                                ActiveButton(sender);
                                studentFormPanel.Controls.Add(bio.box);
                                bio.JambId = studId.ToUpper();
                                studentFormPanel.Controls.Add(academic_History.box);
                                studentFormPanel.Controls.Add(next_Of_Kin.box);
                                studentFormPanel.Controls.Add(academic_Info.box);
                                reader.Close();
                                conn.Close();
                                return;
                            }
                            else if ( !(string.IsNullOrEmpty(reader["dept"].ToString()) || reader["dept"].ToString().Equals(Getname(id))) )
                            {
                                MessageBox.Show("You don't have the privilege to make modification\nrequest for this student", "Modify Data");
                                reader.Close();
                                conn.Close();
                                return;
                            }
                        }
                    }
                    reader.Close();
                    conn.Close();
                    if(!isFound) MessageBox.Show("Invalid student ID or doesn't exist", "Modify Data");
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    conn.Close();
                }               
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            currentParent.Pages1.SetPage(0);
            currentParent.Pages1.BringToFront();
            this.Close();
        }

        private void bunifuPictureBox3_MouseEnter(object sender, EventArgs e)
        {
            bunifuPanel3.BorderThickness = 3;
            bunifuPanel3.BorderColor = Color.Silver;
        }

        private void bunifuPanel3_MouseLeave(object sender, EventArgs e)
        {
            bunifuPanel3.BorderThickness = 2;
            bunifuPanel3.BorderColor = Color.AliceBlue;
        }

        private void bunifuPanel3_Click(object sender, EventArgs e)
        {
            Change_Password cp = new Change_Password(id);
            cp.ShowDialog();
        }

        private void Users_Load(object sender, EventArgs e)
        {
            stuNo.Text = TotalStu(acctType, Getname(id)).ToString();
            clNo.Text = TotalClearedStu(acctType, Getname(id)).ToString();
            unclNo.Text = TotalUnclearedStu(acctType, Getname(id)).ToString();
            PopulatePieChart();
        }

        private void bunifuLabel5_Click(object sender, EventArgs e)
        {
            btnTab2.ForeColor = Color.Black;
            btnTab1.ForeColor = Color.Black;
            getStudents(acctType, Getname(id));
            bunifuPages1.SetPage(2);
            ActiveButton(sender);
        }

        private void bunifuTextBox1_TextChange(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(bunifuTextBox1.Text))
            {
                string query = bunifuTextBox1.Text;
                string name = Getname(id);
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {
                    try
                    {
                        string cmdText = "SELECT b.jamb_num id, b.fName fN, b.lName lN, i.sch, i.dept d, i.mat_no mN FROM stu_bio b LEFT JOIN stu_aca_his h ON b.jamb_num = h.stuId" +
                            " LEFT JOIN stu_aca_info i ON b.jamb_num = i.stuId where (b.fName like '%" + query + "%'" +
                            " or b.lName like '%" + query + "%' or b.jamb_num like '%" + query + "%' or b.sex like '%" + query + "%' or b.marital_status like" +
                            " '%" + query + "%' or b.dp like '%" + query + "%' or b.dob like '%" + query + "%'" +
                            " or b.tel like '%" + query + "%' or b.email like '%" + query + "%' or b.state like '%" + query + "%'" +
                            " or b.country like'%" + query + "%' or h.cert_name1 like '%" + query + "%'" +
                            " or h.cert_name2 like '%" + query + "%' or i.mat_no like '%" + query + "%' or i.sch like'%" + query + "%' or" +
                            " i.dept like'%" + query + "%' or i.year like'%" + query + "%' or i.study_mode like'%" + query + "%' or i.semester like '%" + query + "%'" +
                            " or i.session like '%" + query + "%') and ";
                        if (acctType == 1)
                        {
                            cmdText = cmdText + " i.sch=@na;";
                        }else if (acctType == 2)
                        {
                            cmdText = cmdText + " i.dept=@na;";
                        }

                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@na", name);

                        MySqlDataReader reader = cmd.ExecuteReader();
                        bunifuDataGridView1.Rows.Clear();
                        int i = 1;
                        while (reader.Read())
                        {
                            bunifuDataGridView1.Rows.Add(i.ToString(), reader["fN"].ToString() + " " + reader["lN"].ToString(), reader["mN"].ToString(), reader["sch"].ToString(), reader["d"].ToString(), reader["id"].ToString());
                            i++;
                        }
                        reader.Dispose();
                        reader.Close();
                        conn.Close();

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Get Students");
                    }

                }
            }
            else
            {
                getStudents(acctType, Getname(id));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridView1.SelectedRows.Count == 1)
            {
                string name = bunifuDataGridView1.SelectedRows[0].Cells[1].Value.ToString().Trim();
                string id = bunifuDataGridView1.SelectedRows[0].Cells[5].Value.ToString().Trim();

                //MessageBox.Show(name+" "+id);

                Util.Form1 form = new Util.Form1(id, name);
                form.Show();
            }
        }

        private void panelSales4_Click(object sender, EventArgs e)
        {
            cl = 1;
            getCl_UnStudents(acctType, Getname(id), cl);
            bunifuPages1.SetPage(3);
        }

        private void panelSales5_Click(object sender, EventArgs e)
        {
            cl = 0;
            getCl_UnStudents(acctType, Getname(id), cl);
            bunifuPages1.SetPage(3);
        }

        private void bunifuLabel2_Click(object sender, EventArgs e)
        {
            btnTab2.ForeColor = Color.Black;
            btnTab1.ForeColor = Color.Black;
            getStudents(acctType, Getname(id));
            bunifuPages1.SetPage(2);
        }

        private void bunifuTextBox2_TextChange(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(bunifuTextBox2.Text))
            {
                string query = bunifuTextBox2.Text;
                string name = Getname(id);
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {
                    try
                    {
                        string cmdText = "SELECT b.jamb_num id, b.fName fN, b.lName lN, i.sch, i.dept d, i.mat_no mN FROM stu_bio b LEFT JOIN stu_aca_his h ON b.jamb_num = h.stuId" +
                            " LEFT JOIN stu_aca_info i ON b.jamb_num = i.stuId where (b.fName like '%" + query + "%'" +
                            " or b.lName like '%" + query + "%' or b.jamb_num like '%" + query + "%' or b.sex like '%" + query + "%' or b.marital_status like" +
                            " '%" + query + "%' or b.dp like '%" + query + "%' or b.dob like '%" + query + "%'" +
                            " or b.tel like '%" + query + "%' or b.email like '%" + query + "%' or b.state like '%" + query + "%'" +
                            " or b.country like'%" + query + "%' or h.cert_name1 like '%" + query + "%'" +
                            " or h.cert_name2 like '%" + query + "%' or i.mat_no like '%" + query + "%' or i.sch like'%" + query + "%' or" +
                            " i.dept like'%" + query + "%' or i.year like'%" + query + "%' or i.study_mode like'%" + query + "%' or i.semester like '%" + query + "%'" +
                            " or i.session like '%" + query + "%') and i.cleared="+cl+" and ";
                        if (acctType == 1)
                        {
                            cmdText = cmdText + " i.sch=@na;";
                        }
                        else if (acctType == 2)
                        {
                            cmdText = cmdText + " i.dept=@na;";
                        }

                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@na", name);

                        MySqlDataReader reader = cmd.ExecuteReader();
                        bunifuDataGridView2.Rows.Clear();
                        int i = 1;
                        while (reader.Read())
                        {
                            bunifuDataGridView2.Rows.Add(i.ToString(), reader["fN"].ToString() + " " + reader["lN"].ToString(), reader["mN"].ToString(), reader["sch"].ToString(), reader["d"].ToString(), reader["id"].ToString());
                            i++;
                        }
                        reader.Dispose();
                        reader.Close();
                        conn.Close();

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Get Students");
                    }

                }
            }
            else
            {
                getCl_UnStudents(acctType, Getname(id), cl);
            }
        
    }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridView2.SelectedRows.Count == 1)
            {
                string name = bunifuDataGridView2.SelectedRows[0].Cells[1].Value.ToString().Trim();
                string id = bunifuDataGridView2.SelectedRows[0].Cells[5].Value.ToString().Trim();

                //MessageBox.Show(name+" "+id);

                Util.Form1 form = new Util.Form1(id, name);
                form.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridView1.SelectedRows.Count == 1)
            {
                string name = bunifuDataGridView1.SelectedRows[0].Cells[1].Value.ToString().Trim();
                var a = MessageBox.Show("Are you sure you want to delete "+name+"'s account?", "Delete Account", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                //
                if(a != DialogResult.Yes)
                {
                    return;
                }
                string id = bunifuDataGridView1.SelectedRows[0].Cells[5].Value.ToString().Trim();

                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {
                    try
                    {
                        string cmdText ="delete from stu_aca_his where stuId=@id;" +
                                        "delete from stu_aca_info where stuId=@id;" +
                                        "delete from stu_nxt_kin where stuId=@id;" +
                                        "delete from rev_stu_bio where stuId=@id;" +
                                        "delete from rev_stu_aca_his where stuId=@id;" +
                                        "delete from rev_stu_aca_info where stuId=@id;" +
                                        "delete from rev_stu_nxt_kin where stuId=@id;" +
                                        "delete from login_details where userId=@id;" +
                                        "delete from stu_bio where jamb_num=@id;";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        
                        cmd.Parameters.AddWithValue("@id", id);                        
                        cmd.ExecuteNonQuery();                        
                        conn.Close();
                        getStudents(acctType, Getname(id));
                        MessageBox.Show("Account Deleted Successfully", "Delete Account");
                    }
                    catch (MySqlException ex)
                    {                       
                        MessageBox.Show(ex.Message, "Delete Account - ERROR!");
                    }

                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridView2.SelectedRows.Count == 1)
            {
                string name = bunifuDataGridView2.SelectedRows[0].Cells[1].Value.ToString().Trim();
                var a = MessageBox.Show("Are you sure you want to delete " + name + "'s account?", "Delete Account", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                //
                if (a != DialogResult.Yes)
                {
                    return;
                }
                string id = bunifuDataGridView2.SelectedRows[0].Cells[5].Value.ToString().Trim();

                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {
                    try
                    {
                        string cmdText = "delete from stu_aca_his where stuId=@id;" +
                                        "delete from stu_aca_info where stuId=@id;" +
                                        "delete from stu_nxt_kin where stuId=@id;" +
                                        "delete from rev_stu_bio where stuId=@id;" +
                                        "delete from rev_stu_aca_his where stuId=@id;" +
                                        "delete from rev_stu_aca_info where stuId=@id;" +
                                        "delete from rev_stu_nxt_kin where stuId=@id;" +
                                        "delete from login_details where userId=@id;" +
                                        "delete from stu_bio where jamb_num=@id;";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);

                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        getCl_UnStudents(acctType, Getname(id), cl);
                        MessageBox.Show("Account Deleted Successfully", "Delete Account");
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Delete Account - ERROR!");
                    }

                }

            }
        }
    }
}

/* Department
 * 
 * cmdText = "SELECT count(*) FROM stu_bio b LEFT JOIN stu_aca_his h ON b.jamb_num = h.stuId LEFT JOIN stu_aca_info i ON b.jamb_num = i.stuId " +
"LEFT JOIN stu_nxt_kin k ON b.jamb_num = k.stuId where b.fName <> '' and b.lName <> '' and b.jamb_num <> '' and b.sex <> '' and b.marital_status <> '' " +
"and b.dp <> '' and b.dob <> '' and b.addre <> '' and b.tel<> '' and b.email<> '' and b.state<> '' and b.lga<>'' and " +
"b.country<>'' and (h.cert_name1<> '' or h.cert_name2<> '') and (h.file1<>'' or h.file2<>'')  and i.mat_no<>'' and i.sch<>'' and i.dept<>'' and i.year<>'' " +
"and i.study_mode<>'' and i.semester<>'' and i.session<>'' and k.name<>'' and k.email<>'' and k.addre<>'' and k.tel<>'' and i.dept=@name;";

*School
*
*cmdText = "SELECT count(*) FROM stu_bio b LEFT JOIN stu_aca_his h ON b.jamb_num = h.stuId LEFT JOIN stu_aca_info i ON b.jamb_num = i.stuId "+
"LEFT JOIN stu_nxt_kin k ON b.jamb_num = k.stuId where b.fName <> '' and b.lName <> '' and b.jamb_num <> '' and b.sex <> '' and b.marital_status <> '' "+
"and b.dp <> '' and b.dob <> '' and b.addre <> '' and b.tel<> '' and b.email<> '' and b.state<> '' and b.lga<>'' and " +
"b.country<>'' and (h.cert_name1<> '' or h.cert_name2<> '') and (h.file1<>'' or h.file2<>'')  and i.mat_no<>'' and i.sch<>'' and i.dept<>'' and i.year<>'' " +
"and i.study_mode<>'' and i.semester<>'' and i.session<>'' and k.name<>'' and k.email<>'' and k.addre<>'' and k.tel<>'' and i.sch=@name;";
*/
