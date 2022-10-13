using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Student_Info_Mgt_System
{
    class DBConnection
    {
        private DBString connStr;
        private DBConnection()
        {
            connStr = new DBString();
            Server = connStr.Server;
            DatabaseName = connStr.TableName;
            UserName = connStr.User;
            Password = connStr.Password;
        }

        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public MySqlConnection Connection { get; set; }

        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }

        public bool IsConnect()
        {
            try
            {            
                if (Connection == null)
                {
                    if (String.IsNullOrEmpty(DatabaseName))
                        return false;
                    string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
                    Connection = new MySqlConnection(connstring);
                    Connection.Open();
                }
                if (Connection.State == System.Data.ConnectionState.Closed)
                    Open();
            }catch (MySqlException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Database Connection Error");
                return false;
            }
            return true;
        }

        public void Close()
        {
            Connection.Close();
        }

        public void Open()
        {
            Connection.Open();
        }

    }
}
