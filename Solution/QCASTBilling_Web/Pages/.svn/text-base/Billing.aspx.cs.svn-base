using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using QCASTBilling.BLL;
using System.Data;

public partial class Pages_Billing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Form.SubmitDisabledControls = true;
        this.txtEndDate.Attributes.Add("readonly", "readonly");
        ClientScript.RegisterHiddenField("h_EndDate", this.txtEndDate.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_ClientHistory", this.lstClientHistory.ClientID.ToString());
        this.imgRunReport.Attributes.Add("onclick", "javascript:return CheckBeforeRun();");

        if (!Page.IsPostBack)
        {
            this.txtEndDate.Text = DateTime.Now.ToString("dd-MM-yyyy");

            Label lblTitle = (Label)Master.FindControl("lblTitle");
            if (lblTitle != null)
            {
                lblTitle.Text = ConfigurationManager.AppSettings["AppName"].ToString() + "Billing";
            }

            Client client = new Client(ConfigurationManager.AppSettings["connstring"].ToString());
            this.lstClient.DataSource = client.getClients();
            this.lstClient.DataTextField = "ClientDesc";
            this.lstClient.DataValueField = "ClientId";
            this.lstClient.DataBind();
            this.lstClient.Items.Insert(0, "-- Select Client --");
            this.lstClient.SelectedIndex = 0;

            this.lstClientHistory.Items.Insert(0, "-- Current invoice --");
            this.lstClientHistory.SelectedIndex = 0;
        }

        this.btnCreateInvoice.Attributes.Add("style", "visibility:hidden");
        ClientScript.RegisterHiddenField("h_lstClient", this.lstClient.ClientID.ToString());
        this.imgRunReport.Attributes.Add("onclick", "javascript:return CheckForClient();");
    }

    protected void imgRunReport_Click(object sender, ImageClickEventArgs e)
    {
        if (this.lstClientHistory.SelectedIndex == 0)
        {
            RunCurrent();
            if (this.lstClientHistory.SelectedValue == "-- Current invoice --")
            {
                this.btnCreateInvoice.Attributes.Add("style", "visibility:none");
            }
        }
        else
        {
            RunHistory();
        }
    }

    protected void RunCurrent()
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString());
        conn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "uspInvoiceDetail";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@ClientId", this.lstClient.SelectedValue.ToString()));

        string EndDate = this.txtEndDate.Text.Replace("/", "-");
        EndDate = EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + "-" + EndDate.Substring(6, 4);
        cmd.Parameters.Add(new SqlParameter("@EndDate", EndDate));

        SqlDataReader dr = cmd.ExecuteReader();

        dsInvoiceDetail dsDetail = new dsInvoiceDetail();
        while (dr.Read())
        {
            dsDetail.Tables[0].Rows.Add(dr[0].ToString(), System.Convert.ToDateTime(dr[1].ToString()), dr[2].ToString(), dr[3].ToString(), System.Convert.ToInt32(dr[4].ToString()), System.Convert.ToDouble(dr[5].ToString()));
        }

        dr.Close();
        dsInvoiceSummary dsSummary = new dsInvoiceSummary();
        cmd.CommandText = "uspInvoiceSummary";
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            dsSummary.Tables[0].Rows.Add(dr[0].ToString(), dr[1].ToString(), System.Convert.ToDouble(dr[2].ToString()), dr[3].ToString());
        }

        dr.Close();
        conn.Close();
        conn.Dispose();

        this.rptViewer.LocalReport.DataSources.Clear();
        this.rptViewer.Reset();

        Microsoft.Reporting.WebForms.ReportDataSource rdInvDetail = new Microsoft.Reporting.WebForms.ReportDataSource("dsInvoiceDetail", dsDetail.Tables[0]);
        Microsoft.Reporting.WebForms.ReportDataSource rdInvSummary = new Microsoft.Reporting.WebForms.ReportDataSource("dsInvoiceSummary", dsSummary.Tables[0]);
        this.rptViewer.LocalReport.DataSources.Add(rdInvDetail);
        this.rptViewer.LocalReport.DataSources.Add(rdInvSummary);

        this.rptViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        this.rptViewer.LocalReport.ReportPath = @"Reports\InvoiceDetail.rdlc";

        // Get the date range to show on the report
        var startDate = (from dta in dsDetail.Tables[0].AsEnumerable()
                         orderby dta.Field<DateTime>("CallDate") ascending
                         select (dta.Field<DateTime>("CallDate"))).FirstOrDefault();

        var endDate = (from dta in dsDetail.Tables[0].AsEnumerable()
                       orderby dta.Field<DateTime>("CallDate") ascending
                       select (dta.Field<DateTime>("CallDate"))).LastOrDefault();

        Microsoft.Reporting.WebForms.ReportParameter ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", this.lstClient.SelectedItem.ToString());
        Microsoft.Reporting.WebForms.ReportParameter ReportDescription = new Microsoft.Reporting.WebForms.ReportParameter("ReportDescription", "From : " + startDate + Environment.NewLine + "To     : " + endDate);
        this.rptViewer.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { ClientDescription, ReportDescription });

        this.rptViewer.LocalReport.Refresh();
    }

    protected void RunHistory()
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString());
        conn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "uspInvoiceDetailHistory";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@InvoiceId", this.lstClientHistory.SelectedValue.ToString()));
        SqlDataReader dr = cmd.ExecuteReader();

        dsInvoiceDetail dsDetailHistory = new dsInvoiceDetail();
        while (dr.Read())
        {
            dsDetailHistory.Tables[0].Rows.Add(dr[0].ToString(), System.Convert.ToDateTime(dr[1].ToString()), dr[2].ToString(), dr[3].ToString(), System.Convert.ToInt32(dr[4].ToString()), System.Convert.ToDouble(dr[5].ToString()));
        }

        dr.Close();
        dsInvoiceSummary dsSummaryHistory = new dsInvoiceSummary();
        cmd.CommandText = "uspInvoiceSummaryHistory";
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            dsSummaryHistory.Tables[0].Rows.Add(dr[0].ToString(), dr[1].ToString(), System.Convert.ToDouble(dr[2].ToString()), dr[3].ToString());
        }

        dr.Close();
        conn.Close();
        conn.Dispose();

        this.rptViewer.LocalReport.DataSources.Clear();
        this.rptViewer.Reset();

        Microsoft.Reporting.WebForms.ReportDataSource rdInvDetail = new Microsoft.Reporting.WebForms.ReportDataSource("dsInvoiceDetail", dsDetailHistory.Tables[0]);
        Microsoft.Reporting.WebForms.ReportDataSource rdInvSummary = new Microsoft.Reporting.WebForms.ReportDataSource("dsInvoiceSummary", dsSummaryHistory.Tables[0]);
        this.rptViewer.LocalReport.DataSources.Add(rdInvDetail);
        this.rptViewer.LocalReport.DataSources.Add(rdInvSummary);

        this.rptViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        this.rptViewer.LocalReport.ReportPath = @"Reports\InvoiceDetail.rdlc";

        // Get the date range to show on the report
        var startDate = (from dta in dsDetailHistory.Tables[0].AsEnumerable()
                         orderby dta.Field<DateTime>("CallDate") ascending
                         select (dta.Field<DateTime>("CallDate"))).First();

        var endDate = (from dta in dsDetailHistory.Tables[0].AsEnumerable()
                       orderby dta.Field<DateTime>("CallDate") ascending
                       select (dta.Field<DateTime>("CallDate"))).Last();


        Microsoft.Reporting.WebForms.ReportParameter ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", this.lstClient.SelectedItem.ToString());
        Microsoft.Reporting.WebForms.ReportParameter ReportDescription = new Microsoft.Reporting.WebForms.ReportParameter("ReportDescription", "From : " + startDate + Environment.NewLine + "To     : " + endDate);
        this.rptViewer.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { ClientDescription, ReportDescription });

        this.rptViewer.LocalReport.Refresh();
    }

    protected void lstClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Reset the report viewer irrespective of what was showing at the time
        this.rptViewer.LocalReport.ReportPath = "";
        this.rptViewer.LocalReport.Refresh();

        // Check for any previous invoice runs for the currently-selected client and, if found, populate the drop-down accordingly
        if (this.lstClient.SelectedIndex == 0)
        {
            //Invoice inv = new Invoice(ConfigurationManager.AppSettings["connString"].ToString());
            //if(inv.HasUnInvoicedRecords(this.lstClient.SelectedValue.ToString()) == true)
            //{
                this.lstClientHistory.Items.Clear();
                this.lstClientHistory.Items.Insert(0, "-- Current invoice --");
                this.lstClientHistory.SelectedIndex = 0;
            //}
        }
        else
        {
            Invoice inv = new Invoice(ConfigurationManager.AppSettings["connString"].ToString());
            DataSet ds = inv.GetInvoiceHistory(this.lstClient.SelectedValue.ToString());
            if (ds.Tables[0].Rows.Count == 0)
            {
                //if (inv.HasUnInvoicedRecords(this.lstClient.SelectedValue.ToString()) == true)
                //{
                    this.lstClientHistory.Items.Clear();
                    this.lstClientHistory.Items.Insert(0, "-- Current invoice --");
                    this.lstClientHistory.SelectedIndex = 0;
                //}
            }
            else
            {
                this.lstClientHistory.DataSource = ds;
                this.lstClientHistory.DataTextField = "InvoiceDate";
                this.lstClientHistory.DataValueField = "InvoiceId";
                this.lstClientHistory.DataBind();

                if (inv.HasUnInvoicedRecords(this.lstClient.SelectedValue.ToString()) == true)
                {
                    this.lstClientHistory.Items.Insert(0, "-- Current invoice --");
                    this.lstClientHistory.SelectedIndex = 0;
                }
            }
        }
    }

    protected void btnCreateInvoice_Click(object sender, EventArgs e)
    {
        Invoice inv = new Invoice(ConfigurationManager.AppSettings["connString"].ToString());

        string EndDate = this.txtEndDate.Text.Replace("/", "-");
        EndDate = EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + "-" + EndDate.Substring(6, 4);

        if (inv.CreateInvoice(this.lstClient.SelectedValue.ToString(), Session["UserId"].ToString(), EndDate))
        {
            DataSet ds = inv.GetInvoiceHistory(this.lstClient.SelectedValue.ToString());

            this.lstClientHistory.DataSource = ds;
            this.lstClientHistory.DataTextField = "InvoiceDate";
            this.lstClientHistory.DataValueField = "InvoiceId";
            this.lstClientHistory.DataBind();

            this.lstClientHistory.Items.Insert(0, "-- Current invoice --");
            this.lstClientHistory.SelectedIndex = ds.Tables[0].Rows.Count;
            this.btnCreateInvoice.Attributes.Add("style", "visibility:hidden");
        }

    }
}