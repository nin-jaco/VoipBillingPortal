using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QCASTBilling.DAL;

namespace QCASTBilling.BLL
{
    public class Invoice
    {
        private string connectionString { get; set; }

        public int InvoiceID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int UserId { get; set; }

        public Invoice(string connString)
        {
            this.connectionString = connString;
        }

        public DataSet GetInvoiceHistory(string ClientId)
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            return data.GetInvoiceHistory(ClientId);
        }

        public bool CreateInvoice(string ClientId, string UserId, string EndDate)
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            return data.CreateInvoice(ClientId, UserId, EndDate);
        }

        public bool HasUnInvoicedRecords(string ClientId)
        {
            DAL.DataAccess data = new DataAccess(this.connectionString);
            return data.HasUnInvoicedRecords(ClientId);
        }

    }
}
