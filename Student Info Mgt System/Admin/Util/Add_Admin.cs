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
    public partial class Add_Admin : Form
    {
        private New_Acct p;
        private DBConnection conn;
        private DBString connStr;
        public Add_Admin(New_Acct f)
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
                pswTxt.Text = lnameTxt.Text;
                psw.Text = "Default password: " + lnameTxt.Text.ToLower();
            }
            else
            {
                pswTxt.Text = lnameTxt.Text;
                psw.Text = "";
            }
            
        }

        private void signinbtn_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(lnameTxt.Text) && String.IsNullOrEmpty(fnameTxt.Text) && String.IsNullOrEmpty(idTxt.Text) && String.IsNullOrEmpty(pswTxt.Text))
            {
                prompt.Show(this, "Leave no field empty", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information,
                    4000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft, Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
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
                    string cmdText = "INSERT INTO admin (userID, fName, lName) VALUES (@id, @name, @sname);" +
                                    "INSERT INTO login_details (userId, password, acctType) VALUES (@id, @psw, @accTyp);";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    string sname = lnameTxt.Text.Substring(0, 1).ToUpper() + "" + lnameTxt.Text.Substring(1).ToLower();
                    string name = fnameTxt.Text.Substring(0, 1).ToUpper() + "" + fnameTxt.Text.Substring(1).ToLower();
                    cmd.Parameters.AddWithValue("@id", idTxt.Text);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@sname", sname);
                    cmd.Parameters.AddWithValue("@psw", lnameTxt.Text.ToLower());
                    cmd.Parameters.AddWithValue("@accTyp", 0);
                    cmd.ExecuteNonQuery();

                    idTxt.Text = "";
                    fnameTxt.Text = "";
                    lnameTxt.Text = "";
                    pswTxt.Text = "";
                    psw.Text = "";

                    prompt.Show(this, "Admin account created successfully", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information,
                    4000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft, Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                    conn.Close();
                }
                catch(MySqlException ex)
                {
                    if (ex.Number.Equals(1062))
                    {
                        MessageBox.Show("An Admin with userID: " + idTxt.Text.ToUpper() + " already exist", "Create Account - ERROR!"); ;

                        return;
                    }
                    MessageBox.Show(ex.Message, "Create Account - ERROR!");
                }
                
            }
        }
    }
}
