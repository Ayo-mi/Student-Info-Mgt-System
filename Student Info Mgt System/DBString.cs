using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Info_Mgt_System
{
    class DBString
    {
        private string server_name = "localhost";
        private string user = "root";
        private string password = "";
        private string table_name = "stu_mgt_db";


        public string Server
        {
            get { return server_name; }
        }

        public string User
        {
            get { return user; }
        }

        public string Password
        {
            get { return password; }
        }

        public string TableName
        {
            get { return table_name; }
        }
    }
}
