using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.BunifuButton;
using MySql.Data.MySqlClient;
using Student_Info_Mgt_System.Admin.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Info_Mgt_System.Admin
{
    public partial class Admin : Form
    {
        private Main currentParent;
        private string userId;
        private int ac=0;
        //private Form currentChildForm;

        public Admin(Main p, string id)
        {
            InitializeComponent();
            currentParent = p;
            this.userId = id;

        }

        private void dashBtn()
        {
            BunifuButton2 btn = new BunifuButton2();
            btn.Text = "";
            btn.Size = new Size(60, 54);
            btn.IconPadding = 5;
            btn.IconLeftPadding = new Padding(5, 3, 3, 3);
            btn.IdleIconLeftImage = Properties.Resources.dashboard_layout_100px;
            nav.Controls.Add(btn);
            btn.Location = new Point(1, 70);
            btn.BringToFront();
            btn.Show();

            this.bunifuToolTip1.SetToolTip(btn, "");
            this.bunifuToolTip1.SetToolTipIcon(btn, null);
            this.bunifuToolTip1.SetToolTipTitle(btn, "Dashboard");

            btn.Click += new EventHandler(btn1_click);
            btn.Focus();
            void btn1_click(object sender, EventArgs ev)
            {
                bunifuPages1.SetPage(0);
                schNo.Text = TotalAcct(1);
                deptNo.Text = TotalAcct(2);
                stuNo.Text = TotalAcct(3);
                populateCharts();
                btn.Focus();
            }
        }

        private void reviewBtn()
        {
            BunifuButton2 btn = new BunifuButton2();
            btn.Text = "";
            btn.Size = new Size(60, 54);
            btn.IconPadding = 5;
            btn.IconLeftPadding = new Padding(5, 3, 3, 3);
            btn.IdleIconLeftImage = Properties.Resources.data_pending_100px;
            nav.Controls.Add(btn);
            btn.Location = new Point(1, 124);
            btn.BringToFront();
            btn.Show();

            this.bunifuToolTip1.SetToolTip(btn, "");
            this.bunifuToolTip1.SetToolTipIcon(btn, null);
            this.bunifuToolTip1.SetToolTipTitle(btn, "Review");

            btn.Click += new EventHandler(btn1_click);

            void btn1_click(object sender, EventArgs ev)
            {
                bunifuPages1.SetPage(1);
                btn.Focus();
                revFlowPanel.Controls.Clear();
                GetStuBioRev();
                GetStuNxtKinRev();
                GetStuAcaHisRev();
                GetStuAcaInfoRev();

                if (revFlowPanel.Controls.Count <= 0)
                {
                    revFlowPanel.Visible = false;
                    nonD.Visible = true;
                    nonD.BringToFront();
                }
                else
                {
                    revFlowPanel.Visible = true;
                    nonD.Visible = false;
                }
            }
        }

        private void viewBtn()
        {
            BunifuButton2 btn = new BunifuButton2();
            btn.Text = "";
            btn.Size = new Size(60, 54);
            btn.IconPadding = 5;
            btn.IconLeftPadding = new Padding(5, 3, 3, 3);
            btn.IdleIconLeftImage = Properties.Resources.people_100px;
            nav.Controls.Add(btn);
            btn.Location = new Point(1, 178);
            btn.BringToFront();
            btn.Show();

            this.bunifuToolTip1.SetToolTip(btn, "");
            this.bunifuToolTip1.SetToolTipIcon(btn, null);
            this.bunifuToolTip1.SetToolTipTitle(btn, "View Students");

            btn.Click += new EventHandler(btn1_click);

            void btn1_click(object sender, EventArgs ev)
            {
                getStudents();
                bunifuPages1.SetPage(2);
                btn.Focus();
            }
        }

        private void logoutBtn()
        {
            BunifuButton2 btn = new BunifuButton2();
            btn.Text = "";
            btn.Size = new Size(60, 54);
            btn.IconPadding = 5;
            btn.IconLeftPadding = new Padding(5, 3, 3, 3);
            btn.IdleIconLeftImage = Properties.Resources.logout_rounded_left_100px;
            nav.Controls.Add(btn);
            btn.Dock = DockStyle.Bottom;
            btn.BringToFront();
            btn.Show();

            this.bunifuToolTip1.SetToolTip(btn, "");
            this.bunifuToolTip1.SetToolTipIcon(btn, null);
            this.bunifuToolTip1.SetToolTipTitle(btn, "Sign Out");

            btn.Click += new EventHandler(btn1_click);

            void btn1_click(object sender, EventArgs ev)
            {
                currentParent.Pages1.SetPage(0);
                currentParent.Pages1.BringToFront();
                this.Close();
            }
        }

        private string TotalAcct(int privilege_no)
        {
            DBConnection conn = DBConnection.Instance();
            string count = "0";
            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "Select COUNT(*) as count from login_details where acctType=@id";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", privilege_no);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        count = reader.GetString(0);
                        
                    }
                    conn.Close();
                    
                }
                
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Total Account");
                }
                
            }
            return count;
        }

        private int TotalStuSch(string sch)
        {
            int count = 0;
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "Select COUNT(*) as count from stu_aca_info where sch=@name";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@name", sch);

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
                    MessageBox.Show(ex.Message, "Total School");
                }

            }
            return count;
        }

        private int TotalStuDept(string dept)
        {
            int count = 0;
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "Select COUNT(*) as count from stu_aca_info where dept=@name";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@name", dept);

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
                    MessageBox.Show(ex.Message, "Total School");
                }

            }
            return count;
        }

        private void populateCharts()
        {
            List<double> schData = new List<double>();
            List<double> deptData = new List<double>();
            double [] schVal = { TotalStuSch("Applied Science"), TotalStuSch("Engineering Technology"), TotalStuSch("Petroleum and Business Technology") };
            double[] deptVal = { TotalStuDept("Computer Science"), TotalStuDept("Electrical Engineering"), TotalStuDept("Industrial Safety"), TotalStuDept("Petroleum Marketing"), TotalStuDept("Statistic") };

            schData.AddRange(schVal);
            deptData.AddRange(deptVal);
            schLineChart.Data = schData;
            deptLineChart.Data = deptData;
        }

        private void GetStuBioRev()
        {
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "Select stuId, id from rev_stu_bio where status=@sta";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@sta", 0);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string[] vals = { reader["stuId"].ToString(), reader["id"].ToString(), "Bio Data" };
                        nonD.Visible = false;
                        revFlowPanel.Visible = true;
                        revFlowPanel.Dock = DockStyle.Fill;
                        Review rev = new Review(vals);
                        rev.Card.color = Color.SteelBlue;
                        revFlowPanel.Controls.Add(rev.Card);
                    }
                    reader.Dispose();
                    reader.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Bio Review");
                }

            }
        }

        private void GetStuAcaHisRev()
        {
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "Select stuId, id from rev_stu_aca_his where status=@sta";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@sta", 0);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string[] vals = { reader["stuId"].ToString(), reader["id"].ToString(), "Academic History Data" };
                        nonD.Visible = false;
                        revFlowPanel.Visible = true;
                        revFlowPanel.Dock = DockStyle.Fill;
                        Review rev = new Review(vals);
                        rev.Card.color = Color.Orange;
                        revFlowPanel.Controls.Add(rev.Card);
                    }
                    reader.Dispose();
                    reader.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Academic History Review");
                }

            }
        }

        private void GetStuAcaInfoRev()
        {
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "Select stuId, id from rev_stu_aca_info where status=@sta";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@sta", 0);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string[] vals = { reader["stuId"].ToString(), reader["id"].ToString(), "Academic Info. Data" };
                        nonD.Visible = false;
                        revFlowPanel.Visible = true;
                        revFlowPanel.Dock = DockStyle.Fill;
                        Review rev = new Review(vals);
                        rev.Card.color = Color.DarkBlue;
                        revFlowPanel.Controls.Add(rev.Card);
                    }
                    reader.Dispose();
                    reader.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Academic Info. Review");
                }

            }
        }

        private void GetStuNxtKinRev()
        {
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "Select stuId, id from rev_stu_nxt_kin where status=@sta";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@sta", 0);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string[] vals = { reader["stuId"].ToString(), reader["id"].ToString(), "Next of Kin Data" };
                        nonD.Visible = false;
                        revFlowPanel.Visible = true;
                        revFlowPanel.Dock = DockStyle.Fill;
                        Review rev = new Review(vals);
                        rev.Card.color = Color.Green;
                        revFlowPanel.Controls.Add(rev.Card);
                    }
                    reader.Dispose();
                    reader.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Next of Kin Review");
                }

            }
        }

        private void getStudents()
        {
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "SELECT b.fName fN, b.lName lN, b.jamb_num jN, i.dept d, i.sch sch FROM stu_mgt_db.stu_bio b LEFT JOIN stu_aca_info i ON b.jamb_num = i.stuId;";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    bunifuDataGridView1.Rows.Clear();
                    int i = 1;
                    while (reader.Read())
                    {
                        bunifuDataGridView1.Rows.Add(i.ToString(), reader["fN"].ToString() + " " + reader["lN"].ToString(), reader["jN"].ToString(), reader["sch"].ToString(), reader["d"].ToString());
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

        private void getUsers(int acct)
        {
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "SELECT * from users where acctType=@p;";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@p", acct);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    bunifuDataGridView2.Rows.Clear();
                    if (acct == 1)
                    {
                        bunifuDataGridView2.Columns[3].HeaderText = "School";
                    }else if (acct == 2)
                    {
                        bunifuDataGridView2.Columns[3].HeaderText = "Department";
                    }
                    else
                    {
                        reader.Dispose();
                        reader.Close();
                        conn.Close();
                        MessageBox.Show("Invalid Request", "Get Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int i = 1;
                    while (reader.Read())
                    {
                        bunifuDataGridView2.Rows.Add(i.ToString(), reader["names"].ToString(), reader["userID"].ToString(), reader["name"].ToString());
                        i++;
                    }
                    reader.Dispose();
                    reader.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Users");
                }

            }
        }

        private void getUsers(int acct, string q)
        {
            if (!string.IsNullOrEmpty(q))
            {
                DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                try
                {
                    string cmdText = "SELECT * from users where (userID like '%"+q+ "%' or names like '%" + q + "%' or name like '%" + q + "%') and acctType=@p;";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@p", acct);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    bunifuDataGridView2.Rows.Clear();
                    if (acct == 1)
                    {
                        bunifuDataGridView2.Columns[3].HeaderText = "School";
                    }
                    else if (acct == 2)
                    {
                        bunifuDataGridView2.Columns[3].HeaderText = "Department";
                    }
                    else
                    {
                        reader.Dispose();
                        reader.Close();
                        conn.Close();
                        MessageBox.Show("Invalid Request", "Get Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int i = 1;
                    while (reader.Read())
                    {
                        bunifuDataGridView2.Rows.Add(i.ToString(), reader["names"].ToString(), reader["userID"].ToString(), reader["name"].ToString());
                        i++;
                    }
                    reader.Dispose();
                    reader.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Users");
                }

            }
            }
            else
            {
                getUsers(acct);
            }
            
        }

        private void admin_Load(object sender, EventArgs e)
        {
            dashBtn();
            reviewBtn();
            viewBtn();
            logoutBtn();
            schNo.Text = TotalAcct(1);
            deptNo.Text = TotalAcct(2);
            stuNo.Text = TotalAcct(3);           
            populateCharts();
        }

        private void bunifuLabel13_Click(object sender, EventArgs e)
        {
            New_Acct new_Acct = new New_Acct();
            new_Acct.ShowDialog();
        }

        private void bunifuLabel1_Click(object sender, EventArgs e)
        {
            Change_Password cp = new Change_Password(userId);
            cp.ShowDialog();
        }

        private void bunifuTextBox1_TextChange(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(bunifuTextBox1.Text))
            {
                string query = bunifuTextBox1.Text;
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {
                    try
                    {
                        string cmdText = "SELECT b.fName fN, b.lName lN, i.sch, i.dept d, b.jamb_num jN FROM stu_bio b LEFT JOIN stu_aca_his h ON b.jamb_num = h.stuId" +
                            " LEFT JOIN stu_aca_info i ON b.jamb_num = i.stuId LEFT JOIN stu_nxt_kin k ON b.jamb_num = k.stuId where b.fName like '%"+query+"%'" +
                            " or b.lName like '%" + query + "%' or b.jamb_num like '%" + query + "%' or b.sex like '%" + query + "%' or b.marital_status like" +
                            " '%" + query + "%' or b.dp like '%" + query + "%' or b.dob like '%" + query + "%'" +
                            " or b.addre like '%" + query + "%' or b.tel like '%" + query + "%' or b.email like '%" + query + "%' or b.state like '%" + query + "%'" +
                            " or b.lga like'%" + query + "%' or b.country like'%" + query + "%' or h.cert_name1 like '%" + query + "%'" +
                            " or h.cert_name2 like '%" + query + "%' or i.mat_no like '%" + query + "%' or i.sch like'%" + query + "%' or" +
                            " i.dept like'%" + query + "%' or i.year like'%" + query + "%' or i.study_mode like'%" + query + "%' or i.semester like '%" + query + "%'" +
                            " or i.session like '%" + query + "%' or k.name like '%" + query + "%' or k.email like '%" + query + "%' or k.addre like '%" + query + "%' or k.tel like '%" + query + "%';";
                        
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        //cmd.Parameters.AddWithValue("@a", query);

                        MySqlDataReader reader = cmd.ExecuteReader();
                        bunifuDataGridView1.Rows.Clear();
                        int i = 1;
                        while (reader.Read())
                        {
                            bunifuDataGridView1.Rows.Add(i.ToString(), reader["fN"].ToString() + " " + reader["lN"].ToString(), reader["jN"].ToString(), reader["sch"].ToString(), reader["d"].ToString());
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
                getStudents();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridView1.SelectedRows.Count == 1)
            {
                string name = bunifuDataGridView1.SelectedRows[0].Cells[1].Value.ToString().Trim();
                string id = bunifuDataGridView1.SelectedRows[0].Cells[2].Value.ToString().Trim();

                //MessageBox.Show(name+" "+id);

                Users.Util.Form1 form = new Users.Util.Form1(id, name);
                form.Show();
            }
        }

        private void bunifuPanel2_MouseEnter(object sender, EventArgs e)
        {
            bunifuPanel2.BorderThickness = 3;
            bunifuPanel2.BorderColor = Color.Silver;
        }

        private void bunifuPanel2_MouseLeave(object sender, EventArgs e)
        {
            bunifuPanel2.BorderThickness = 2;
            bunifuPanel2.BorderColor = Color.AliceBlue;
        }

        private void bunifuLabel1_MouseEnter(object sender, EventArgs e)
        {
            bunifuPanel1.BorderThickness = 3;
            bunifuPanel1.BorderColor = Color.Silver;
        }

        private void bunifuLabel1_MouseLeave(object sender, EventArgs e)
        {
            bunifuPanel1.BorderThickness = 2;
            bunifuPanel1.BorderColor = Color.AliceBlue;
        }

        private void bunifuPictureBox4_Click(object sender, EventArgs e)
        {
            ac = 2;
            getUsers(ac);
            bunifuPages1.SetPage(3);
        }

        private void bunifuLabel6_Click(object sender, EventArgs e)
        {
            ac = 1;
            getUsers(ac);
            bunifuPages1.SetPage(3);
        }

        private void bunifuLabel2_Click(object sender, EventArgs e)
        {
            getStudents();
            bunifuPages1.SetPage(2);
        }

        private void bunifuTextBox2_TextChange(object sender, EventArgs e)
        {
            getUsers(ac, bunifuTextBox2.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridView1.SelectedRows.Count == 1)
            {
                string name = bunifuDataGridView1.SelectedRows[0].Cells[1].Value.ToString().Trim();
                var a = MessageBox.Show("Are you sure you want to delete " + name + "'s account?", "Delete Account", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                //
                if (a != DialogResult.Yes)
                {
                    return;
                }
                string id = bunifuDataGridView1.SelectedRows[0].Cells[2].Value.ToString().Trim();

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
                        getStudents();
                        MessageBox.Show("Account Deleted Successfully", "Delete Account");
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Delete Account - ERROR!");
                    }

                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
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
                string id = bunifuDataGridView2.SelectedRows[0].Cells[2].Value.ToString().Trim();

                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {
                    try
                    {
                        string cmdText = "delete from users where userID=@id;" +
                                        "delete from login_details where userId=@id;";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);

                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        getUsers(ac);
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
