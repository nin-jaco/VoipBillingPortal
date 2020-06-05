using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QCASTBilling.DAL;

namespace QCASTBilling.BLL
{
    public class Client
    {
        public enum clientSearchType
        {
            exact,
            containing
        }

        private string connectionString { get; set; }

        public int ClientID { get; set; }
        public string ClientDesc { get; set; }
        public int RangeFrom { get; set; }
        public int RangeTo { get; set; }
        public string ClientCode { get; set; }
        public string AccountingCode { get; set; }
        public string ClientUserName { get; set; }
        public string ClientPassword { get; set; }
        public string ProfileName { get; set; }
        public int ProfileId { get; set; }
        public clientSearchType searchType { get; set; }



        public Client(string connString)
        {
            this.connectionString = connString;
        }


        public DataSet getClients()
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            return data.GetClients();
        }

        public DataSet searchClients(string searchValue)
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            return data.SearchClients(this.searchType.ToString(), searchValue);
        }

        public void getClient(string ClientId)
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            DataSet ds = data.GetClient(ClientId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                this.ClientID = System.Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                this.ClientDesc = ds.Tables[0].Rows[0][1].ToString();
                this.RangeFrom = System.Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString());
                this.RangeTo = System.Convert.ToInt32(ds.Tables[0].Rows[0][3].ToString());
                this.ClientCode = ds.Tables[0].Rows[0][4].ToString();
                this.AccountingCode = ds.Tables[0].Rows[0][5].ToString();
                this.ClientUserName = ds.Tables[0].Rows[0][6].ToString();
                this.ClientPassword = ds.Tables[0].Rows[0][7].ToString();
            }
        }

        public bool saveClient()
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            return data.saveClient(this.ClientID.ToString(),
                                 this.ClientDesc.ToString(),
                                 System.Convert.ToString(this.RangeFrom),
                                 System.Convert.ToString(this.RangeTo),
                                 this.ClientCode,
                                 this.AccountingCode,
                                 this.ClientUserName,
                                 this.ClientPassword,
                                 5);
        }

        public int insertClient()
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            object retVal = data.insertClient(this.ClientDesc.ToString(),
                                 System.Convert.ToString(this.RangeFrom),
                                 System.Convert.ToString(this.RangeTo),
                                 this.ClientCode,
                                 this.AccountingCode,
                                 this.ClientUserName,
                                 this.ClientPassword,
                                 5);
            if (retVal == null)
            {
                return 0;
            }
            else
            {
                return System.Convert.ToInt32(retVal);
            }
        }

        public bool deleteClient()
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            return data.deleteClient(this.ClientID.ToString());
        }



    }
}
