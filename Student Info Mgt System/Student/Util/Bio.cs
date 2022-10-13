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
    public partial class Bio : Form
    {
        private Student st;
        private Users.Users user;
        private string imgFile;
        private string id;
        public Bio(Student st)
        {
            InitializeComponent();
            this.st = st;
        }

        public Bio(Users.Users user, string id)
        {
            InitializeComponent();
            this.user = user;
            this.id = id;
        }

        public Bunifu.UI.WinForms.BunifuGroupBox box
        {
            get { return bunifuGroupBox1; }
        }

        public string JambId
        {
            set { jambId.Text = value; }
        }

        private bool isRequested(string id)
        {
            DBConnection conn = DBConnection.Instance();
            bool a = true;
            if (conn.IsConnect())
            {

                try
                {

                    string cmdText = "Select status from rev_stu_bio where status=@sta and stuId=@id Limit 1";
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
            if (string.IsNullOrEmpty(lName.Text) || dp == null || string.IsNullOrEmpty(imgFile) || string.IsNullOrEmpty(oName.Text) || string.IsNullOrEmpty(jambId.Text) || sex.SelectedIndex == -1 || mStatus.SelectedIndex == -1 || string.IsNullOrEmpty(address.Text)|| string.IsNullOrEmpty(tel.Text)|| string.IsNullOrEmpty(email.Text)|| string.IsNullOrEmpty(state.Text) || string.IsNullOrEmpty(lga.Text)|| string.IsNullOrEmpty(country.Text))
            {
                MessageBox.Show("All fields are required before sending reviews", "Student Bio Data");
                return;
            }
            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {
                
                try
                {
                    byte[] ImageData;
                    FileStream fs = new FileStream(imgFile, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    ImageData = br.ReadBytes((int)fs.Length);
                    br.Close();
                    fs.Close();
                    string [] dat = dob.Value.ToString().Split();
                    string cmdText = "INSERT INTO rev_stu_bio (jambId, fName, lName, sex, marital_status, dob, addre, tel, email, state, lga, country, status, stuId, dp)" +
                        " VALUES (@id, @fname, @lname, @sex, @ms, @dob, @addr, @tel, @email, @state, @lga, @cou, @sta, @stuid, @dp);";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);                    
                    cmd.Parameters.AddWithValue("@id", jambId.Text.Trim());
                    cmd.Parameters.AddWithValue("@fname", oName.Text.Trim());
                    cmd.Parameters.AddWithValue("@lname", lName.Text.Trim());
                    cmd.Parameters.AddWithValue("@sex", sex.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@ms", mStatus.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@dob", dat[0].Trim());
                    cmd.Parameters.AddWithValue("@addr", address.Text.Trim());
                    cmd.Parameters.AddWithValue("@tel", tel.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", email.Text.Trim());
                    cmd.Parameters.AddWithValue("@state", state.Text.Trim());
                    cmd.Parameters.AddWithValue("@lga", lga.Text.Trim());
                    cmd.Parameters.AddWithValue("@cou", country.Text.Trim());
                    cmd.Parameters.AddWithValue("@sta", 0);
                    cmd.Parameters.AddWithValue("@stuid", studentId.Trim());
                    cmd.Parameters.AddWithValue("@dp", ImageData);
                    int row = cmd.ExecuteNonQuery();

                    if (row > 0)
                    {
                        prompt.Show(this, "Review Request Sent Successfully", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success,
                    4000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft);
                        conn.Close();
                    }
                    else
                    {
                        MessageBox.Show("Request was not sent successfully, try again.", "Student Bio Data Request");
                    }
                    
                }
                catch (MySqlException ex)
                {                    
                    MessageBox.Show(ex.Message, "Send Bio Review");
                }

            }
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Image files | *.jpg;*.png;*.jpeg";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    imgFile = openFileDialog1.FileName;
                    dp.Image = Image.FromFile(openFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
