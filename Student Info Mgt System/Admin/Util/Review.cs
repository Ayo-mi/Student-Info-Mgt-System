using MySql.Data.MySqlClient;
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

namespace Student_Info_Mgt_System.Admin.Util
{
    public partial class Review : Form
    {
        private string stuId;
        private string revType;
        private string idd;
        public Review(string [] param)
        {
            InitializeComponent();
            this.stuId = param[0];
            this.idd = param[1];
            this.revType = param[2];
            PrepareReview(stuId, idd);
        }
        public Review()
        {
            InitializeComponent();            
        }

        public Bunifu.Framework.UI.BunifuCards Card
        {
            get { return card; }
        }

        public Panel Panel2 { get { return panel2; } }
        public Panel Panel3 { get { return panel3; } }
        public Panel Panel1 { get { return panel1; } }
        public Panel PanelBio { get { return revBioPanel; } }
        public string Idd { get { return idd; } }

        private void PrepareReview(string stuId, string dataId)
        {
            string Server, DatabaseName, UserName, Password;
            DBString connStr = new DBString();
            Server = connStr.Server;
            DatabaseName = connStr.TableName;
            UserName = connStr.User;
            Password = connStr.Password;

           
            string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
            MySqlConnection conn = new MySqlConnection(connstring);
            conn.Open();
            
                try
                {
                    string cmdText = "Select fName, lName, dp from stu_bio where jamb_num=@id Limit 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn);
                    cmd.Parameters.AddWithValue("@id", stuId);

                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        stuNam.Text = dr["fName"].ToString() + " " + dr["lName"].ToString();
                        revTyp.Text = revType;
                        id.Text = dataId;

                        if(dr["dp"] != DBNull.Value)
                        {
                            byte [] byteImage = (byte[])dr["dp"];
                            using (MemoryStream ms = new MemoryStream(byteImage))
                            {
                                //saving to jpg image
                                Image img = new Bitmap(ms);
                                disPic.Image = img;
                            }
                        }
                    }
                    dr.Dispose();
                    dr.Close();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Prepare Review");
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
                    string cmdText = "Select file1 from rev_stu_aca_his where id=@id LIMIT 1";
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

                    MessageBox.Show("File successfully loaded! Opening file....",
                        "View Certificate File", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    System.Diagnostics.Process prc = new System.Diagnostics.Process();
                    prc.StartInfo.FileName = fileName;
                    prc.Start();
                    myData.Close();
                    conn.Close();
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
                    string cmdText = "Select file2 from rev_stu_aca_his where id=@id LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader myData = cmd.ExecuteReader();

                    if (!myData.HasRows)
                        throw new Exception("There are no File to save");

                    myData.Read();

                    FileSize = 150000;//myData.GetUInt32(100);
                    rawData = new byte[FileSize];

                    myData.GetBytes(myData.GetOrdinal("file2"), 0, rawData, 0, (int)FileSize);

                    fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fs.Write(rawData, 0, (int)FileSize);
                    fs.Close();

                    MessageBox.Show("File successfully loaded! Opening file....",
                        "View Certificate File", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    System.Diagnostics.Process prc = new System.Diagnostics.Process();
                    prc.StartInfo.FileName = fileName;
                    prc.Start();
                    myData.Close();
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Get Data");
                }
            }

        }

        private void id_Click(object sender, EventArgs e)
        {
            if (revTyp.Text == "Bio Data")
            {
                new BioData(stuId, id.Text, stuNam.Text);
            }
            else if(revTyp.Text == "Academic History Data")
            {
                new AcademicHis(stuId, id.Text, stuNam.Text);
            }
            else if(revTyp.Text == "Academic Info. Data")
            {
                new AcademicInfo(stuId, id.Text, stuNam.Text);
            }
            else if(revTyp.Text == "Next of Kin Data")
            {
                new NextOfKin(stuId, id.Text, stuNam.Text);
            }
            
        }
    
        
        public class NextOfKin
        {
            private Review obj = new Review();
            string idd;
            string stuId;

            public NextOfKin(string stuId, string id, string name)
            {
                this.idd = id;
                this.stuId = stuId;
                if (!Prepare(id))
                {                   
                    MessageBox.Show("No Record Found", "Next of Kin Review", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                obj.nkAcpt.Click += new EventHandler(accept_btn);
                obj.nkRjt.Click += new EventHandler(reject_btn);

                Form1 frm = new Form1();
                frm.Controls.Add(Panel);
                Panel.Dock = DockStyle.Fill;
                frm.Text = name + " Next of Kin's Data";
                frm.ShowDialog();
            }
            public Panel Panel { get { return obj.panel2; } }

            private bool Prepare(string id)
            {
                bool isFound = false;
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {
                    try
                    {
                        string cmdText = "Select * from rev_stu_nxt_kin where id=@id and status=@st limit 1;";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@st", 0);
                        
                        MySqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            isFound = true;
                            obj.label50.Text=dr.GetString(1);
                            obj.label46.Text=dr.GetString(2);
                            obj.label30.Text=dr.GetString(3);
                            obj.label48.Text=dr.GetString(4);
                        }
                        dr.Dispose();
                        dr.Close();
                        conn.Close();
                       
                    }
                    
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Get Next of Kin Review");
                    }
                }
                return isFound;
            }

            void accept_btn(object sender, EventArgs ev)
            {
                var result = MessageBox.Show("Are you sure you want to accept the data?", "Next of Kin Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {

                    try
                    {
                        string cmdText = "Update stu_nxt_kin set name=@nam, addre=@addr, email=@ema, tel=@tel where stuId=@id LIMIT 1;" +
                            "Update rev_stu_nxt_kin set status=@st where id=@idd LIMIT 1;";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", stuId);
                        cmd.Parameters.AddWithValue("@idd", idd);
                        cmd.Parameters.AddWithValue("@nam", obj.label50.Text);
                        cmd.Parameters.AddWithValue("@addr", obj.label46.Text);
                        cmd.Parameters.AddWithValue("@ema", obj.label48.Text);
                        cmd.Parameters.AddWithValue("@tel", obj.label30.Text);
                        cmd.Parameters.AddWithValue("@st", 1);

                        int row = cmd.ExecuteNonQuery();

                        if (row > 1)
                        {
                            MessageBox.Show("Data Accepted", "Accept Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            obj.nkAcpt.Enabled = false;
                            obj.nkRjt.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("An error occur. Try again", "Accept Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        conn.Close();
                    }
                    catch (MySqlException mse)
                    {
                        MessageBox.Show(mse.Message, "Accept Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            void reject_btn(object sender, EventArgs ev)
            {
                var result = MessageBox.Show("Are you sure you want to reject the data?", "Next of Kin Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {

                    try
                    {
                        string cmdText = "Update rev_stu_nxt_kin set status=@st where id=@id LIMIT 1";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", idd);
                        cmd.Parameters.AddWithValue("@st", 1);

                        int row = cmd.ExecuteNonQuery();

                        if (row > 0)
                        {
                            MessageBox.Show("Data Rejected", "Reject Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            obj.nkAcpt.Enabled = false;
                            obj.nkRjt.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("An error occur. Try again", "Reject Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        conn.Close();
                    }
                    catch (MySqlException mse)
                    {
                        MessageBox.Show(mse.Message, "Reject Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public class AcademicInfo
        {
            private Review obj = new Review();
            string idd;
            string stuId;

            public AcademicInfo(string stuId, string id, string name)
            {
                this.idd = id;
                this.stuId = stuId;
                if (!Prepare(id))
                {
                    MessageBox.Show("No Record Found", "Academic Information Review", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                obj.aiAcpt.Click += new EventHandler(accept_btn);
                obj.aiRjt.Click += new EventHandler(reject_btn);

                Form1 frm = new Form1();
                frm.Controls.Add(Panel);
                Panel.Dock = DockStyle.Fill;
                frm.Text = name + " Academic Information Data";
                frm.ShowDialog();
            }
            public Panel Panel { get { return obj.panel1; } }

            private bool Prepare(string id)
            {
                bool isFound = false;
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {
                    try
                    {
                        string cmdText = "Select * from rev_stu_aca_info where id=@id and status=@st limit 1;";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@st", 0);

                        MySqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            isFound = true;
                            obj.label44.Text = dr.GetString(1).ToUpper();
                            obj.label42.Text = dr.GetString(2);
                            obj.label40.Text = dr.GetString(3);
                            obj.label38.Text = dr.GetString(4);
                            obj.label36.Text = dr.GetString(5);
                            obj.label32.Text = dr.GetString(6);
                            obj.label34.Text = dr.GetString(7);
                        }
                        dr.Dispose();
                        dr.Close();
                        conn.Close();

                    }

                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Get Academic Information Review");
                    }
                }
                return isFound;
            }

            void accept_btn(object sender, EventArgs ev)
            {
                var result = MessageBox.Show("Are you sure you want to accept the data?", "Academic Information Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {

                    try
                    {
                        string cmdText = "Update stu_aca_info set mat_no=@mat, sch=@sch, dept=@dept, year=@yr, study_mode=@stmo, " +
                            "semester=@sem, session=@sess where stuId=@id LIMIT 1;" +
                            "Update rev_stu_aca_info set status=@st where id=@idd LIMIT 1;";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", stuId);
                        cmd.Parameters.AddWithValue("@idd", idd);
                        cmd.Parameters.AddWithValue("@mat", obj.label44.Text);
                        cmd.Parameters.AddWithValue("@sch", obj.label42.Text);
                        cmd.Parameters.AddWithValue("@dept", obj.label40.Text);
                        cmd.Parameters.AddWithValue("@yr", obj.label38.Text);
                        cmd.Parameters.AddWithValue("@stmo", obj.label36.Text);
                        cmd.Parameters.AddWithValue("@sem", obj.label32.Text);
                        cmd.Parameters.AddWithValue("@sess", obj.label34.Text);
                        cmd.Parameters.AddWithValue("@st", 1);

                        int row = cmd.ExecuteNonQuery();

                        if (row > 1)
                        {
                            MessageBox.Show("Data Accepted", "Accept Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            obj.aiAcpt.Enabled = false;
                            obj.aiRjt.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("An error occur. Try again", "Accept Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        conn.Close();
                    }
                    catch (MySqlException mse)
                    {
                        MessageBox.Show(mse.Message, "Accept Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            void reject_btn(object sender, EventArgs ev)
            {
                var result = MessageBox.Show("Are you sure you want to reject the data?", "Academic Information Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {

                    try
                    {
                        string cmdText = "Update rev_stu_aca_info set status=@st where id=@id LIMIT 1";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", idd);
                        cmd.Parameters.AddWithValue("@st", 1);

                        int row = cmd.ExecuteNonQuery();

                        if (row > 0)
                        {
                            MessageBox.Show("Data Rejected", "Reject Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            obj.aiAcpt.Enabled = false;
                            obj.aiRjt.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("An error occur. Try again", "Reject Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        conn.Close();
                    }
                    catch (MySqlException mse)
                    {
                        MessageBox.Show(mse.Message, "Reject Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public class AcademicHis
        {
            private Review obj = new Review();
            string idd;
            string stuId;
            object fi1;
            object fi2;
            public AcademicHis(string stuId, string id, string name)
            {
                this.idd = id;
                this.stuId = stuId;
                if (!Prepare(id))
                {
                    MessageBox.Show("No Record Found", "Academic History Review", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                obj.ahAcpt.Click += new EventHandler(accept_btn);
                obj.ahRjt.Click += new EventHandler(reject_btn);

                obj.button1.Click += new EventHandler(view_btn1);
                obj.button2.Click += new EventHandler(view_btn2);

                Form1 frm = new Form1();
                frm.Controls.Add(Panel);
                Panel.Dock = DockStyle.Fill;
                frm.Text = name + " Academic History Data";
                frm.ShowDialog();
            }
            
            public Panel Panel { get { return obj.panel3; } }

            private bool Prepare(string id)
            {
                bool isFound = false;
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {
                    try
                    {
                        string cmdText = "Select * from rev_stu_aca_his where id=@id and status=@st limit 1;";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@st", 0);

                        MySqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            isFound = true;
                            obj.label52.Text = dr.GetString(1);
                            fi1 = dr["file1"];
                            if (String.IsNullOrEmpty(dr["cert_name2"].ToString()))
                            {
                                obj.label28.Visible = false;
                                obj.button2.Visible = false;
                                obj.label28.Text = "";
                            }
                            else
                            {
                                obj.label27.Visible = true;
                                obj.label28.Visible = true;
                                obj.button2.Visible = true;
                                obj.label28.Text = dr["cert_name2"].ToString();
                                fi2 = dr["file2"];
                            }
                        }
                        dr.Dispose();
                        dr.Close();
                        conn.Close();

                    }

                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Get Academic History Review");
                    }
                }
                return isFound;
            }

            void accept_btn(object sender, EventArgs ev)
            {
                var result = MessageBox.Show("Are you sure you want to accept the data?", "Academic History Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {

                    try
                    {
                        string cmdText = "Update stu_aca_his set cert_name1=@c1, cert_name2=@c2, file1=@f1, file2=@f2 where stuId=@id LIMIT 1;" +
                            "Update rev_stu_aca_his set status=@st where id=@idd LIMIT 1;";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", stuId);
                        cmd.Parameters.AddWithValue("@idd", idd);
                        cmd.Parameters.AddWithValue("@c1", obj.label52.Text);
                        cmd.Parameters.AddWithValue("@c2", obj.label28.Text);
                        cmd.Parameters.AddWithValue("@f1", fi1);
                        cmd.Parameters.AddWithValue("@f2", fi2);
                        cmd.Parameters.AddWithValue("@st", 1);

                        int row = cmd.ExecuteNonQuery();

                        if (row > 1)
                        {
                            MessageBox.Show("Data Accepted", "Accept Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            obj.ahAcpt.Enabled = false;
                            obj.ahRjt.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("An error occur. Try again", "Accept Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        conn.Close();
                    }
                    catch (MySqlException mse)
                    {
                        MessageBox.Show(mse.Message, "Accept Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            void reject_btn(object sender, EventArgs ev)
            {
                var result = MessageBox.Show("Are you sure you want to reject the data?", "Academic History Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {

                    try
                    {
                        string cmdText = "Update rev_stu_aca_his set status=@st where id=@id LIMIT 1";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", idd);
                        cmd.Parameters.AddWithValue("@st", 1);

                        int row = cmd.ExecuteNonQuery();

                        if (row > 0)
                        {
                            MessageBox.Show("Data Rejected", "Reject Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            obj.ahAcpt.Enabled = false;
                            obj.ahRjt.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("An error occur. Try again", "Reject Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        conn.Close();
                    }
                    catch (MySqlException mse)
                    {
                        MessageBox.Show(mse.Message, "Reject Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            void view_btn1(object sender, EventArgs ev)
            {
                obj.getCert(idd);
            }
            
            void view_btn2(object sender, EventArgs ev)
            {
                obj.getCert2(idd);
            }
        }

        public class BioData
        {
            private Review obj = new Review();
            string idd;
            string stuId;
            private byte[] arrimg;


            public BioData(string stuId, string id, string name)
            {
                this.idd = id;
                this.stuId = stuId;
                if (!Prepare(id))
                {
                    MessageBox.Show("No Record Found", "Bio Data Review", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                obj.bdAcpt.Click += new EventHandler(accept_btn);
                obj.bdRjt.Click += new EventHandler(reject_btn);

                Form1 frm = new Form1();
                frm.Controls.Add(Panel);
                Panel.Dock = DockStyle.Fill;
                frm.Text = name + " Bio Data";
                frm.ShowDialog();
            }
            public Panel Panel { get { return obj.revBioPanel; } }
            DateTime dt;
            private bool Prepare(string id)
            {
                bool isFound = false;
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {
                    try
                    {
                        string cmdText = "Select * from rev_stu_bio where id=@id and status=@st limit 1;";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@st", 0);

                        MySqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            isFound = true;
                            obj.label14.Text = dr.GetString(1);
                            obj.label4.Text = dr.GetString(2);
                            obj.label1.Text = dr.GetString(3);
                            obj.label6.Text = dr.GetString(4);
                            obj.label8.Text = dr.GetString(5);

                            arrimg = (byte[])dr["dp"];

                           // DateTime d = Convert.ToDateTime(dr.GetString(7));
                            dt = Convert.ToDateTime(dr.GetString(7));
                            obj.label10.Text = dt.ToString("ddd, dd MMM yyyy");
                           // obj.label10.Text = dr.GetString(7);

                            obj.label12.Text = dr.GetString(8);
                            obj.label24.Text = dr.GetString(9);
                            obj.label22.Text = dr.GetString(10);
                            obj.label20.Text = dr.GetString(11);
                            obj.label18.Text = dr.GetString(12);
                            obj.label16.Text = dr.GetString(13);
                        }
                        dr.Dispose();
                        dr.Close();
                        conn.Close();

                    }

                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Get Bio Data Review");
                    }
                }
                return isFound;
            }

            void accept_btn(object sender, EventArgs ev)
            {
                var result = MessageBox.Show("Are you sure you want to accept the data?", "Bio Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {

                    try
                    {
                        //DateTime d = Convert.ToDateTime(obj.label10.Text);
                        string cmdText = "Update stu_bio set jamb_num=@jmb, fName=@fn, lName=@ln, sex=@sz, marital_status=@ms, " +
                            "dp=@dp, dob=@dob, addre=@addr, tel=@tel, email=@ema, state=@state, lga=@lga, country=@cou where jamb_num=@id LIMIT 1;" +
                            "Update rev_stu_bio set status=@st where id=@idd LIMIT 1;";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", stuId);
                        cmd.Parameters.AddWithValue("@idd", idd);
                        cmd.Parameters.AddWithValue("@jmb", obj.label14.Text);
                        cmd.Parameters.AddWithValue("@fn", obj.label4.Text);
                        cmd.Parameters.AddWithValue("@ln", obj.label1.Text);
                        cmd.Parameters.AddWithValue("@sz", obj.label6.Text);
                        cmd.Parameters.AddWithValue("@ms", obj.label8.Text);
                        cmd.Parameters.AddWithValue("@dp", arrimg);
                        cmd.Parameters.AddWithValue("@dob", dt.ToString("MM/dd/yyyy"));
                        cmd.Parameters.AddWithValue("@addr", obj.label12.Text);
                        cmd.Parameters.AddWithValue("@tel", obj.label24.Text);
                        cmd.Parameters.AddWithValue("@ema", obj.label22.Text);
                        cmd.Parameters.AddWithValue("@state", obj.label20.Text);
                        cmd.Parameters.AddWithValue("@lga", obj.label18.Text);
                        cmd.Parameters.AddWithValue("@cou", obj.label16.Text);
                        cmd.Parameters.AddWithValue("@st", 1);

                        int row = cmd.ExecuteNonQuery();

                        if (row > 1)
                        {
                            MessageBox.Show("Data Accepted", "Accept Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            obj.bdAcpt.Enabled = false;
                            obj.bdRjt.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("An error occur. Try again", "Accept Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        conn.Close();
                    }
                    catch (MySqlException mse)
                    {
                        MessageBox.Show(mse.Message, "Accept Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            void reject_btn(object sender, EventArgs ev)
            {
                var result = MessageBox.Show("Are you sure you want to reject the data?", "Bio Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }
                DBConnection conn = DBConnection.Instance();

                if (conn.IsConnect())
                {

                    try
                    {
                        string cmdText = "Update rev_stu_bio set status=@st where id=@id LIMIT 1";
                        MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@id", idd);
                        cmd.Parameters.AddWithValue("@st", 1);

                        int row = cmd.ExecuteNonQuery();

                        if (row > 0)
                        {
                            MessageBox.Show("Data Rejected", "Reject Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            obj.bdAcpt.Enabled = false;
                            obj.bdRjt.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("An error occur. Try again", "Reject Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        conn.Close();
                    }
                    catch (MySqlException mse)
                    {
                        MessageBox.Show(mse.Message, "Reject Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}