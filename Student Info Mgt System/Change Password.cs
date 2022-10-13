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

namespace Student_Info_Mgt_System
{
    public partial class Change_Password : Form
    {
        private string id;
        public Change_Password(string id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
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
                    cmd.Parameters.AddWithValue("@id", id);
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
    }
}
