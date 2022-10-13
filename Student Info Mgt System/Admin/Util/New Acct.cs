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
    public partial class New_Acct : Form
    {
        //private Panel currentFormPanel;
        public New_Acct()
        {
            InitializeComponent();
        }

        private void getAdminForm()
        {
            formPanel.Controls.Clear();
            Add_Admin obj = new Add_Admin(this);
            this.Size = new Size(800, 489);
            formPanel.Controls.Add(obj.panel);
            obj.panel.Dock = DockStyle.Fill;
            obj.panel.BringToFront();
            obj.panel.Visible = true;
        }

        private void getDeptForm()
        {
            formPanel.Controls.Clear();
            Add_Dept obj = new Add_Dept(this);
            this.Size = new Size(800, 489);
            formPanel.Controls.Add(obj.panel);
            obj.panel.Dock = DockStyle.Fill;
            obj.panel.BringToFront();
            obj.panel.Visible = true;
        }

        private void getSchForm()
        {
            formPanel.Controls.Clear();
            Add_Sch obj = new Add_Sch(this);
            this.Size = new Size(800, 489);
            formPanel.Controls.Add(obj.panel);
            obj.panel.Dock = DockStyle.Fill;
            obj.panel.BringToFront();
            obj.panel.Visible = true;
        }

        private void getStudentForm()
        {
            formPanel.Controls.Clear();
            Add_Student obj = new Add_Student(this);
            this.Size = new Size(800, 489);
            formPanel.Controls.Add(obj.panel);
            obj.panel.Dock = DockStyle.Fill;
            obj.panel.BringToFront();
            obj.panel.Visible = true;
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void acctType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (acctType.SelectedIndex == 0)
            {
                getAdminForm();
            }
            else if (acctType.SelectedIndex == 1)
            {
                getSchForm();
            }
            else if (acctType.SelectedIndex == 2)
            {
                getDeptForm();
            }
            else if (acctType.SelectedIndex == 3)
            {
                getStudentForm();
            }
            else
            {
                formPanel.Controls.Clear();
                this.Size = new Size(594, 310);
            }
        }
    }
}
