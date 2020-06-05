using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QCASTBilling.DAL;

namespace QCASTBilling.BLL
{
    public class User
    {
        public enum userSearchType
        {
            exact,
            containing
        }

        private string connectionString { get; set; }

        public int UserID { get; set; }
        public int UserProfileID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string ProfileName { get; set; }
        public userSearchType searchType { get; set; }

        public User(string connString)
        {
            this.connectionString = connString;
        }

        public DataSet getUsers()
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            return data.GetUsers();
        }

        public DataSet searchUsers(string searchValue)
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            return data.SearchUsers(this.searchType.ToString(), searchValue);
        }

        public void getUser(int UserId)
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            DataSet ds = data.GetUser(UserId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                this.UserID = System.Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                this.UserProfileID = System.Convert.ToInt32(ds.Tables[0].Rows[0][1].ToString());
                this.UserName = ds.Tables[0].Rows[0][2].ToString();
                this.UserPassword = ds.Tables[0].Rows[0][3].ToString();
                this.ProfileName = ds.Tables[0].Rows[0][4].ToString();
            }
        }

        public void getUser(string UserName)
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            DataSet ds = data.GetUser(UserName);
            if (ds.Tables[0].Rows.Count > 0)
            {
                this.UserID = System.Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                this.UserProfileID = System.Convert.ToInt32(ds.Tables[0].Rows[0][1].ToString());
                this.UserName = ds.Tables[0].Rows[0][2].ToString();
                this.UserPassword = ds.Tables[0].Rows[0][3].ToString();
                this.ProfileName = ds.Tables[0].Rows[0][4].ToString();
            }
        }

        public DataSet getUserProfiles()
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            return data.GetUserProfiles();
        }

        public bool saveUser()
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            return data.saveUser(this.UserID.ToString(),
                                 this.UserProfileID.ToString(),
                                 this.UserName,
                                 this.UserPassword);
        }

        public int insertUser()
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            object retVal = data.insertUser(this.UserProfileID.ToString(),
                                            this.UserName,
                                            this.UserPassword);
            if (retVal == null)
            {
                return 0;
            }
            else
            {
                return System.Convert.ToInt32(retVal);
            }
        }

        public bool deleteUser()
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            return data.deleteUser(this.UserID.ToString());
        }

    }
}
