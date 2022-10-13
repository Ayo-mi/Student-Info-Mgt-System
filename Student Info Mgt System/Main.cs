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
    public partial class Main : Form
    {
        private Form currentChildForm;
       
        public Main()
        {
            InitializeComponent();            
        }

        public void openChildForm(Form childForm)
        {
            
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            deskpanel.Controls.Add(childForm);
            deskpanel.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        public Bunifu.UI.WinForms.BunifuPages Pages1
        {
            get { return pagesPanel; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //openChildForm(new Setup.setup(this));
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            pagesPanel.SetPage(0);
        }

        private void lblForgetPassword_Click(object sender, EventArgs e)
        {
            pagesPanel.SetPage(1);
        }

        private void bunifuButton25_Click(object sender, EventArgs e)
        {
            //openChildForm(new Admin.admin(this, "0"));
            if(String.IsNullOrEmpty(idTxt.Text) || String.IsNullOrEmpty(pswTxt.Text))
            {
                prompt.Show(this, "Enter your user ID and password", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information,
                   4000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleCenter);
                return;
            }

            DBConnection conn = DBConnection.Instance();

            if (conn.IsConnect())
            {

                try
                {
                    string cmdText = "Select userId, password, acctType from login_details where BINARY userId=@id and BINARY password=@psw LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                    cmd.Parameters.AddWithValue("@id", idTxt.Text);
                    cmd.Parameters.AddWithValue("@psw", pswTxt.Text);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    bool isFound = false;
                    while (reader.Read())
                    {
                        isFound = true;
                        if ((int)reader["acctType"] == 0)
                        {
                            string id = reader["userId"].ToString();

                            reader.Dispose();
                            reader.Close();
                            conn.Close();
                            pswTxt.Text = "";
                            openChildForm(new Admin.Admin(this, id));
                            return;
                        }
                        else if ((int)reader["acctType"] == 1 || (int)reader["acctType"] == 2)
                        {
                            string id = reader["userId"].ToString();
                            int acctType = reader.GetInt32(2);

                            reader.Dispose();
                            reader.Close();
                            conn.Close();
                            pswTxt.Text = "";
                            openChildForm(new Users.Users(this, id, acctType));
                            return;
                        }
                        else if ((int)reader["acctType"] == 3)
                        {
                            string id = reader["userId"].ToString();

                            reader.Dispose();
                            reader.Close();
                            conn.Close();
                            pswTxt.Text = "";
                            openChildForm(new Student.Student(this, id));
                            return;
                        }
                        else
                        {
                            reader.Dispose();
                            reader.Close();
                            conn.Close();
                            MessageBox.Show("This account is an invalid account", "Invalid Account");
                            return;
                        }
                    }                    
                    
                    if (!isFound)
                    {
                        reader.Dispose();
                        reader.Close();
                        conn.Close();
                        MessageBox.Show("Incorrect user ID or password", "Login");
                        return;
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Login");
                }

            }
            
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(bunifuTextBox9.Text) && string.IsNullOrEmpty(bunifuTextBox10.Text))
            {
                prompt.Show(this, "Enter your user ID and email address", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error,
                   5000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleCenter);
                return;
            }

            MessageBox.Show("You'll receive an email containing your new password if \nthe details you provide are valid.", "Forget Password");
        }
    }
}
