using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace LyricEditor
{
    class EditorDBConnect : DBConnect
    {
        //Constructor
        public EditorDBConnect(string server, string database, string uid, string password): base(server, database, uid, password)
        {
            
        }

        public int GetSongID(string artist, string title)
        {
            string query = "SELECT id FROM songs WHERE artist=\"" + artist + "\" AND title=\"" + title + "\"";
            int retval = 0;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    retval = (int)dataReader["id"];
                }
                dataReader.Close();

                this.CloseConnection();
            }
            
            return retval;
        }

        public int GetNextID()
        {
            string query = "select MAX(id) from songs";
            int retval = 0;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                retval = (int)cmd.ExecuteScalar() + 1;

                this.CloseConnection();
            }

            return retval;
        }
    }
}
