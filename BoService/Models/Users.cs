using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoService.Models
{
    public class Users
    {
        public string User_Name { get; set; }
        public string User_Password { get; set; }

        public string User_Email_Address { get; set; }
        public string User_Mobile_Number { get; set; }


        internal BoAppDB Db { get; set; }

        public Users()
        {
            User_Name = string.Empty;
            User_Password = string.Empty;
            User_Email_Address = string.Empty;
            User_Mobile_Number = string.Empty;

        }

        internal Users(BoAppDB db)
        {
            Db = db;
            User_Name = string.Empty;
            User_Password = string.Empty;
            User_Email_Address = string.Empty;
            User_Mobile_Number = string.Empty;
        }

        public List<Users> GetUsersList(string strUserName, string strUserOassword)
        {
            List<Users> returnList = new List<Users>();
            try
            {
                using var cmd = Db.Connection.CreateCommand();
                string commandText = "SELECT * FROM edc.users where user_name = " + "'" + strUserName + "'" + "and user_password = " + "'" + strUserOassword + "'";
                cmd.CommandText = commandText;
                returnList = ReadAllAsync(cmd.ExecuteReader());
            }
            catch (Exception Ex)
            {
                returnList = null;
            }
            return returnList;
        }

        private List<Users> ReadAllAsync(System.Data.Common.DbDataReader reader)
        {
            List<Users> posts = new List<Users>();
            try
            {
                using (reader)
                {
                    while (reader.Read())
                    {
                        var post = new Users(Db)
                        {
                            User_Name = reader.GetString(0),
                            User_Password = reader.GetString(1),
                        };
                        posts.Add(post);
                    }
                }
            }
            catch(Exception Ex)
            {
                posts = null;
            }
            return posts;
        }

        public bool IsUserRegistered()
        {
            bool bIsUserRegistered = false;
            try
            {
                List<Users> result = GetUsersList(User_Name, User_Password);
                if (result != null && result.Count > 0)
                {
                    bIsUserRegistered = true;

                }
                else
                {
                    bIsUserRegistered = false;
                }
            }
            catch(Exception Ex)
            {
                bIsUserRegistered = false;
            }
            return bIsUserRegistered;
        }
    }
}
