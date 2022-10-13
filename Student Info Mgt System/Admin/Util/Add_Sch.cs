using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Info_Mgt_System.Admin.Util
{
    public partial class Add_Sch : Form
    {
        private New_Acct p;
        private DBConnection conn;
        private DBString connStr;
        public Add_Sch(New_Acct f)
        {
            InitializeComponent();
            this.p = f;
            conn = DBConnection.Instance();
            connStr = new DBString();
        }

        public Panel panel
        {
            get { return panel1; }
        }

        private void signoutbtn_Click(object sender, EventArgs e)
        {
            this.p.Close();           
            
        }

        private void lnameTxt_TextChange(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(lnameTxt.Text) && lnameTxt.Text.Length != 0)
            {
                //pswTxt.Text = lnameTxt.Text;
            }
        }

        private void signinbtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(lnameTxt.Text) || dept.SelectedIndex == -1 || String.IsNullOrEmpty(idTxt.Text))
            {
                prompt.Show(this, "Leave no field empty", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information,
                    4000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft);
                return;
            }

            conn.Server = connStr.Server;
            conn.DatabaseName = connStr.TableName;
            conn.UserName = connStr.User;
            conn.Password = connStr.Password;

            if (conn.IsConnect())
            {
                
                try
                {
                    string cmdText = "INSERT INTO users (userID, names, acctType, name) VALUES (@id, @name, @accTyp, @nam);" +
                                    "INSERT INTO login_details (userId, password, acctType) VALUES (@id, @psw, @accTyp);";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", idTxt.Text);
                    cmd.Parameters.AddWithValue("@name", lnameTxt.Text);
                    cmd.Parameters.AddWithValue("@accTyp", 1);
                    cmd.Parameters.AddWithValue("@psw", "welcome1");
                    cmd.Parameters.AddWithValue("@nam", dept.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();

                    idTxt.Text = "";
                    lnameTxt.Text = "";
                    pswTxt.Text = "";
                    //psw.Text = "";
                    dept.SelectedIndex = -1;
                    dept.Text = "Select School";

                    prompt.Show(this, "School account created successfully", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information,
                    4000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft, Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    if (ex.Number.Equals(1062))
                    {
                        MessageBox.Show("A Staff with employee ID: " + idTxt.Text.ToUpper() + " already exist", "Create Account - ERROR!"); ;

                        return;
                    }
                    MessageBox.Show(ex.Message, "Create Account - ERROR!");
                }

            }
        }
    }
}
