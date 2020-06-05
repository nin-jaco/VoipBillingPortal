using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace QCASTBilling.DAL
{
    public class DataAccess
    {
        private SqlConnection conn = null;
        private SqlCommand cmd = new SqlCommand();

        public DataAccess(string connectString)
        {
            this.conn = new SqlConnection(connectString);
            this.cmd.Connection = this.conn;
        }


        // User stuff
        public DataSet GetUsers()
        {
            if (this.conn.State != ConnectionState.Open)
            {
                this.conn.Open();
            }

            cmd.CommandText = "Select UserId, ProfileId, UserName, UserPassword From [User]";
            IDataReader dr = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add("UserId", typeof(int));
            table.Columns.Add("ProfileId", typeof(int));
            table.Columns.Add("UserName", typeof(string));
            table.Columns.Add("UserPassword", typeof(string));
            ds.Tables.Add(table);

            while (dr.Read())
            {
                ds.Tables[0].Rows.Add(System.Convert.ToInt32(dr[0].ToString()), System.Convert.ToInt32(dr[1].ToString()), dr[2].ToString(), dr[3].ToString());
            }

            conn.Close();
            conn.Dispose();
            return ds;
        }

        public DataSet SearchUsers(string searchType, string searchValue)
        {
            if (this.conn.State != ConnectionState.Open)
            {
                this.conn.Open();
            }

            if (searchType == "exact")
            {
                cmd.CommandText = "Select UserId, ProfileId, UserName, UserPassword From [User] where UserName = '" + searchValue + "'";
            }
            else if (searchType == "containing") 
            {
                cmd.CommandText = "Select UserId, ProfileId, UserName, UserPassword From [User] where UserName like '%" + searchValue + "%'";
            }
            IDataReader dr = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add("UserId", typeof(int));
            table.Columns.Add("ProfileId", typeof(int));
            table.Columns.Add("UserName", typeof(string));
            table.Columns.Add("UserPassword", typeof(string));
            ds.Tables.Add(table);

            while (dr.Read())
            {
                ds.Tables[0].Rows.Add(System.Convert.ToInt32(dr[0].ToString()), System.Convert.ToInt32(dr[1].ToString()), dr[2].ToString(), dr[3].ToString());
            }

            conn.Close();
            conn.Dispose();
            return ds;
        }

        public DataSet GetUser(int userId)
        {

            if (this.conn.State != ConnectionState.Open)
            {
                this.conn.Open();
            }

            cmd.CommandText = "Select UserId, u.ProfileId as 'ProfileId', UserName, UserPassword, p.ProfileName " +
                              " From [User] u left join Profile p on u.ProfileId = p.ProfileId Where UserId = " + System.Convert.ToString(userId);
            IDataReader dr = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add("UserId", typeof(int));
            table.Columns.Add("ProfileId", typeof(int));
            table.Columns.Add("UserName", typeof(string));
            table.Columns.Add("UserPassword", typeof(string));
            table.Columns.Add("ProfileName", typeof(string));
            ds.Tables.Add(table);

            if (dr.Read())
            {
                ds.Tables[0].Rows.Add(System.Convert.ToInt32(dr[0].ToString()), System.Convert.ToInt32(dr[1].ToString()), dr[2].ToString(), dr[3].ToString(), dr[4].ToString());
            }

            conn.Close();
            conn.Dispose();
            return ds;
        }

        public DataSet GetUser(string UserName)
        {

            if (this.conn.State != ConnectionState.Open)
            {
                this.conn.Open();
            }

            cmd.CommandText = "Select UserId, u.ProfileId as 'ProfileId', UserName, UserPassword, p.ProfileName " +
                              " From [User] u left join Profile p on u.ProfileId = p.ProfileId Where u.UserName = '" + UserName + "'";
            IDataReader dr = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add("UserId", typeof(int));
            table.Columns.Add("ProfileId", typeof(int));
            table.Columns.Add("UserName", typeof(string));
            table.Columns.Add("UserPassword", typeof(string));
            table.Columns.Add("ProfileName", typeof(string));
            ds.Tables.Add(table);

            if (dr.Read())
            {
                ds.Tables[0].Rows.Add(System.Convert.ToInt32(dr[0].ToString()), System.Convert.ToInt32(dr[1].ToString()), dr[2].ToString(), dr[3].ToString(), dr[4].ToString());
            }

            conn.Close();
            conn.Dispose();
            return ds;
        }

        public DataSet GetUserProfiles()
        {
            if (this.conn.State != ConnectionState.Open)
            {
                this.conn.Open();
            }

            cmd.CommandText = "Select ProfileId, ProfileName From Profile Order By ProfileName";
            IDataReader dr = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add("ProfileId", typeof(int));
            table.Columns.Add("ProfileName", typeof(string));
            ds.Tables.Add(table);

            while (dr.Read())
            {
                ds.Tables[0].Rows.Add(System.Convert.ToInt32(dr[0].ToString()), dr[1].ToString());
            }

            conn.Close();
            conn.Dispose();
            return ds;
        }

        public bool saveUser(string UserId, string ProfileId, string UserName, string Password)
        {
            bool retVal = false;
            try
            {
                if (this.conn.State != ConnectionState.Open)
                {
                    this.conn.Open();
                }

                cmd.CommandText = "Update [User] set UserName = '" + UserName +
                                  "', ProfileId = " + ProfileId +
                                  ", UserPassword = '" + Password + "' " +
                                  " Where UserId = " + UserId;
                cmd.ExecuteNonQuery();
                retVal = true;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                if (this.conn != null)
                {
                    if (this.conn.State != ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return retVal;
        }

        public object insertUser(string ProfileId, string UserName, string UserPassword)
        {
            object newUserId = null;
            try
            {
                if (this.conn.State != ConnectionState.Open)
                {
                    this.conn.Open();
                }

                cmd.CommandText = "Insert into  [User] (ProfileId, UserName, UserPassword) values(" +
                                  ProfileId + ", '" +
                                  UserName + "', '" +
                                  UserPassword + "'); Select @@Identity";
                newUserId = cmd.ExecuteScalar();
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                if (this.conn != null)
                {
                    if (this.conn.State != ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return newUserId;
        }

        public bool deleteUser(string UserId)
        {
            bool retVal = false;
            try
            {
                if (this.conn.State != ConnectionState.Open)
                {
                    this.conn.Open();
                }

                cmd.CommandText = "Delete from  [User] Where UserId = " + UserId;
                cmd.ExecuteNonQuery();
                retVal = true;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                if (this.conn != null)
                {
                    if (this.conn.State != ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return retVal;
        }

        // Client stuff
        public DataSet GetClients()
        {
            if (this.conn.State != ConnectionState.Open)
            {
                this.conn.Open();
            }

            cmd.CommandText = "Select ClientId, ClientDesc From Client Where ClientCode <> '' And ClientId <> 0 Order By ClientDesc";
            IDataReader dr = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add("ClientId", typeof(int));
            table.Columns.Add("ClientDesc", typeof(string));
            ds.Tables.Add(table);

            while (dr.Read())
            {
                ds.Tables[0].Rows.Add(System.Convert.ToInt32(dr[0].ToString()), dr[1].ToString());
            }

            conn.Close();
            conn.Dispose();
            return ds;
        }

        public DataSet SearchClients(string searchType, string searchValue)
        {
            if (this.conn.State != ConnectionState.Open)
            {
                this.conn.Open();
            }

            if (searchType == "exact")
            {
                cmd.CommandText = "Select ClientId, ClientDesc, RangeFrom, RangeTo, ClientCode, AccountingCode, ClientUserName, ClientPassword from Client where ClientDesc = '" + searchValue + "'";
            }
            else if (searchType == "containing")
            {
                cmd.CommandText = "Select ClientId, ClientDesc, RangeFrom, RangeTo, ClientCode, AccountingCode, ClientUserName, ClientPassword from CLient where ClientDesc like '%" + searchValue + "%'";
            }
            IDataReader dr = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add("ClientId", typeof(int));
            table.Columns.Add("ClientDesc", typeof(string));
            table.Columns.Add("RangeFrom", typeof(int));
            table.Columns.Add("RangeTo", typeof(int));
            table.Columns.Add("ClientCode", typeof(string));
            table.Columns.Add("AccountingCode", typeof(string));
            table.Columns.Add("ClientUserName", typeof(string));
            table.Columns.Add("ClientPassword", typeof(string));
            ds.Tables.Add(table);

            while (dr.Read())
            {
                ds.Tables[0].Rows.Add(System.Convert.ToInt32(dr[0].ToString()), dr[1].ToString(), System.Convert.ToInt32(dr[2].ToString()), System.Convert.ToInt32(dr[3].ToString()), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
            }

            conn.Close();
            conn.Dispose();
            return ds;
        }

        public DataSet GetClient(string ClientId)
        {

            if (this.conn.State != ConnectionState.Open)
            {
                this.conn.Open();
            }

            cmd.CommandText = "Select ClientId, ClientDesc, RangeFrom, RangeTo, ClientCode, AccountingCode, ClientUserName, ClientPassword, ProfileId " +
                              " from  Client Where ClientId = " + ClientId;
            IDataReader dr = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add("ClientId", typeof(int));
            table.Columns.Add("ClientDesc", typeof(string));
            table.Columns.Add("RangeFrom", typeof(int));
            table.Columns.Add("RangeTo", typeof(int));
            table.Columns.Add("ClientCode", typeof(string));
            table.Columns.Add("AccountingCode", typeof(string));
            table.Columns.Add("ClientUserName", typeof(string));
            table.Columns.Add("ClientPassword", typeof(string));
            ds.Tables.Add(table);

            if (dr.Read())
            {
                ds.Tables[0].Rows.Add(System.Convert.ToInt32(dr[0].ToString()), dr[1].ToString(), System.Convert.ToInt32(dr[2].ToString()), System.Convert.ToInt32(dr[3].ToString()), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
            }

            conn.Close();
            conn.Dispose();
            return ds;
        }

        public bool saveClient(string ClientId, string ClientDesc, string RangeFrom, string RangeTo, string ClientCode, string AccountingCode, string ClientUserName, string ClientPassword, int ProfileId)
        {
            bool retVal = false;
            try
            {
                if (this.conn.State != ConnectionState.Open)
                {
                    this.conn.Open();
                }

                cmd.CommandText = "Update Client set ClientDesc = '" + ClientDesc + "', " +
                                  "RangeFrom = " + RangeFrom + ", " +
                                  "RangeTo = " + RangeTo + ", " +
                                  "ClientCode = '" + ClientCode + "', " +
                                  "AccountingCode = '" + AccountingCode + "', " +
                                  "ClientUserName = '" + ClientUserName + "', " +
                                  "ClientPassword = '" + ClientPassword + "' " +
                                  " Where ClientId = " + ClientId;
                cmd.ExecuteNonQuery();
                retVal = true;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                if (this.conn != null)
                {
                    if (this.conn.State != ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return retVal;
        }

        public object insertClient(string ClientDesc, string RangeFrom, string RangeTo, string ClientCode, string AccountingCode, string ClientUserName, string ClientPassword, int ProfileId)
        {
            object newClientId = null;
            try
            {
                if (this.conn.State != ConnectionState.Open)
                {
                    this.conn.Open();
                }

                cmd.CommandText = "Insert into Client (ClientDesc, RangeFrom, RangeTo, ClientCode, AccountingCode, ClientUserName, ClientPassword) values('" +
                                  ClientDesc + "', " +
                                  RangeFrom + ", " +
                                  RangeTo + ", '" +
                                  ClientCode + "', '" +
                                  AccountingCode + "', '" +
                                  ClientUserName + "', '" +
                                  ClientPassword + "'); Select @@Identity";
                newClientId = cmd.ExecuteScalar();
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                if (this.conn != null)
                {
                    if (this.conn.State != ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return newClientId;
        }

        public bool deleteClient(string ClientId)
        {
            bool retVal = false;
            try
            {
                if (this.conn.State != ConnectionState.Open)
                {
                    this.conn.Open();
                }

                cmd.CommandText = "Delete from Client Where ClientId = " + ClientId;
                cmd.ExecuteNonQuery();
                retVal = true;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                if (this.conn != null)
                {
                    if (this.conn.State != ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return retVal;
        }

        // Invoice stuff
        public DataSet GetInvoiceHistory(string ClientId)
        {
            if (this.conn.State != ConnectionState.Open)
            {
                this.conn.Open();
            }

            cmd.CommandText = "Select I.InvoiceId, I.InvoiceDate, I.UserID from Invoice I left join Billing B on I.InvoiceID = B.InvoiceId " +
                               "Where B.InvoiceId is not null And B.ClientID = '" + ClientId + "' Group By I.InvoiceId, I.InvoiceDate, I.UserID Order by I.InvoiceID Desc";
            IDataReader dr = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add("InvoiceId", typeof(int));
            table.Columns.Add("InvoiceDate", typeof(DateTime));
            table.Columns.Add("UserId", typeof(int));
            ds.Tables.Add(table);

            while (dr.Read())
            {
                ds.Tables[0].Rows.Add(System.Convert.ToInt32(dr[0].ToString()), System.Convert.ToDateTime(dr[1].ToString()), System.Convert.ToInt32(dr[2].ToString()));
            }

            conn.Close();
            conn.Dispose();
            return ds;
        }

        public bool CreateInvoice(string ClientId, string UserId, string EndDate)
        {
            bool retVal = false;
            try
            {
                if (this.conn.State != ConnectionState.Open)
                {
                    this.conn.Open();
                }

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = this.conn;
                cmd.CommandText = "uspCreateInvoice";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                cmd.Parameters.Add(new SqlParameter("@UserId", UserId));
                cmd.Parameters.Add(new SqlParameter("@EndDate", EndDate));
                cmd.ExecuteNonQuery();
                retVal = true;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                if (this.conn != null)
                {
                    if (this.conn.State != ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return retVal;
        }

        public bool HasUnInvoicedRecords(string ClientId)
        {
            if (this.conn.State != ConnectionState.Open)
            {
                this.conn.Open();
            }

            cmd.CommandText = "Select Count(*) from Billing Where InvoiceID is null and ClientID = '" + ClientId + "'";
            int objRetVal = (int)cmd.ExecuteScalar();

            conn.Close();
            conn.Dispose();

            if (objRetVal == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
