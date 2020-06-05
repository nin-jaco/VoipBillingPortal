using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using QCASTBilling.BLL;


public partial class Pages_Reports : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Form.SubmitDisabledControls = true;
        this.txtStartDate.Attributes.Add("readonly", "readonly");
        this.txtEndDate.Attributes.Add("readonly", "readonly");

        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Master.FindControl("lblTitle");
            if (lblTitle != null)
            {
                lblTitle.Text = ConfigurationManager.AppSettings["AppName"].ToString() + "Reports";
            }

            if (Session["profilename"].ToString() != "Client")
            {
                Client client = new Client(ConfigurationManager.AppSettings["connstring"].ToString());
                this.lstClient.DataSource = client.getClients();
                this.lstClient.DataTextField = "ClientDesc";
                this.lstClient.DataValueField = "ClientId";
                this.lstClient.DataBind();
                this.lstClient.Items.Insert(0, "-- Select Client --");
                this.lstClient.SelectedIndex = 0;

                this.tblInvoices.Attributes.Add("style", "visibility:hidden");
                this.tblDateRange.Attributes.Add("style", "visibility:hidden");
            }
            else if (Session["profilename"].ToString() == "Client")
            {
                Invoice inv = new Invoice(ConfigurationManager.AppSettings["connString"].ToString());
                DataSet ds = null;

                if (Session["profilename"].ToString() == "Client")
                {
                    ds = inv.GetInvoiceHistory(Session["ClientId"].ToString());
                }
                else
                {
                    ds = inv.GetInvoiceHistory(this.lstClient.SelectedValue.ToString());
                }

                this.lstClientHistory.DataSource = ds;
                this.lstClientHistory.DataTextField = "InvoiceDate";
                this.lstClientHistory.DataValueField = "InvoiceId";
                this.lstClientHistory.DataBind();

                if (inv.HasUnInvoicedRecords(this.lstClient.SelectedValue.ToString()) == true)
                {
                    this.lstClientHistory.Items.Insert(0, "-- Current invoice --");
                    this.lstClientHistory.SelectedIndex = 0;
                }

                this.tblDateRange.Attributes.Add("style", "visibility:hidden");
            }

            this.lstReport.Items.Add(new ListItem("Current statement", "CURR_STATEMENT"));
            this.lstReport.Items.Add(new ListItem("Current - Summary by Origin", "ORIGIN"));
            this.lstReport.Items.Add(new ListItem("Top 20 Calls by Cost", "TOP20_COST"));
            this.lstReport.Items.Add(new ListItem("Top 20 Calls by Duration", "TOP20_DURATION"));
            this.lstReport.Items.Add(new ListItem("International", "INTERNATIONAL"));
            this.lstReport.Items.Add(new ListItem("Undefined Destinations", "UNDEFINED_DEST"));
        }

        ClientScript.RegisterHiddenField("h_lstClient", this.lstClient.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_lstReport", this.lstReport.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_StartDate", this.txtStartDate.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_EndDate", this.txtEndDate.ClientID.ToString());
        this.imgRunReport.Attributes.Add("onclick", "javascript:return CheckForClient();");

        if (Session["profilename"].ToString() == "Client")
        {
            this.trClientRow.Visible = false;
        }

    }

    protected void imgRunReport_Click(object sender, ImageClickEventArgs e)
    {
        switch (this.lstReport.SelectedValue.ToString())
        {
            case "CURR_STATEMENT":
                if (this.lstClientHistory.SelectedValue == "-- Current invoice --")
                {
                    this.RunStatement();
                }
                else
                {
                    this.RunHistory();
                }
                break;
            case "ORIGIN":
                this.RunOriginSummary();
                break;
            case "TOP20_COST":
                this.RunTop20ByCost();
                break;
            case "TOP20_DURATION":
                this.RunTop20ByDuration();
                break;
            case "INTERNATIONAL":
                this.RunInternational();
                break;
            case "UNDEFINED_DEST":
                this.RunUndefinedDest();
                break;
                
        }
    }

    protected void RunOriginSummary()
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString());
        conn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "uspRptSummaryByOrigin";
        cmd.CommandType = CommandType.StoredProcedure;

        if (Session["profilename"].ToString() == "Client")
        {
            cmd.Parameters.Add(new SqlParameter("@ClientId", Session["ClientId"].ToString()));
        }
        else
        {
            cmd.Parameters.Add(new SqlParameter("@ClientId", this.lstClient.SelectedValue.ToString()));
        }
        SqlDataReader dr = cmd.ExecuteReader();

        dsInvoiceSummary dsSummary = new dsInvoiceSummary();
        while (dr.Read())
        {
            //dsSummary.Tables[0].Rows.Add(dr[0].ToString(), System.Convert.ToDouble(dr[1].ToString()));
            dsSummary.Tables[0].Rows.Add(dr[0].ToString(), dr[1].ToString(), System.Convert.ToDouble(dr[2].ToString()), dr[3].ToString());
        }

        this.rptViewer.LocalReport.DataSources.Clear();
        this.rptViewer.Reset();

        Microsoft.Reporting.WebForms.ReportDataSource rdInvSummary = new Microsoft.Reporting.WebForms.ReportDataSource("dsInvoiceSummary", dsSummary.Tables[0]);
        this.rptViewer.LocalReport.DataSources.Add(rdInvSummary);

        this.rptViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        this.rptViewer.LocalReport.ReportPath = @"Reports\StatementSummaryBySrc.rdlc";

        Microsoft.Reporting.WebForms.ReportParameter ClientDescription = null;
        if (Session["profilename"].ToString() == "Client")
        {
            ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", Session["ClientDesc"].ToString());
        }
        else
        {
            ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", this.lstClient.SelectedItem.ToString());
        }
        this.rptViewer.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { ClientDescription });

        this.rptViewer.LocalReport.EnableHyperlinks = true;
        this.rptViewer.LocalReport.Refresh();
    }

    protected void RunStatement()
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString());
        conn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "uspInvoiceDetail";
        cmd.CommandType = CommandType.StoredProcedure;
        if (Session["profilename"].ToString() == "Client")
        {
            cmd.Parameters.Add(new SqlParameter("@ClientId", Session["ClientId"].ToString()));
        }
        else
        {
            cmd.Parameters.Add(new SqlParameter("@ClientId", this.lstClient.SelectedValue.ToString()));
        }

        string EndDate = DateTime.Now.ToString("dd-MM-yyyy");
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

        // Configure report params
        Microsoft.Reporting.WebForms.ReportParameter ClientDescription = null;
        if (Session["profilename"].ToString() == "Client")
        {
            ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", Session["ClientDesc"].ToString());
        }
        else
        {
            ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", this.lstClient.SelectedItem.ToString());
        }

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
            //dsSummaryHistory.Tables[0].Rows.Add(dr[0].ToString(), System.Convert.ToDouble(dr[1].ToString()));
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
                         select (dta.Field<DateTime>("CallDate"))).FirstOrDefault();

        var endDate = (from dta in dsDetailHistory.Tables[0].AsEnumerable()
                       orderby dta.Field<DateTime>("CallDate") ascending
                       select (dta.Field<DateTime>("CallDate"))).LastOrDefault();

        // Configure report params
        Microsoft.Reporting.WebForms.ReportParameter ClientDescription = null;
        if (Session["profilename"].ToString() == "Client")
        {
            ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", Session["ClientDesc"].ToString());
        }
        else
        {
            ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", this.lstClient.SelectedItem.ToString());
        }

        Microsoft.Reporting.WebForms.ReportParameter ReportDescription = new Microsoft.Reporting.WebForms.ReportParameter("ReportDescription", "From : " + startDate + Environment.NewLine + "To     : " + endDate);
        this.rptViewer.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { ClientDescription, ReportDescription });

        this.rptViewer.LocalReport.Refresh();
    }

    protected void RunTop20ByCost()
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString());
        conn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "uspRptTop20ByCost";
        cmd.CommandType = CommandType.StoredProcedure;
        if (Session["profilename"].ToString() == "Client")
        {
            cmd.Parameters.Add(new SqlParameter("@ClientId", Session["ClientId"].ToString()));
        }
        else
        {
            cmd.Parameters.Add(new SqlParameter("@ClientId", this.lstClient.SelectedValue.ToString()));
        }

        string StartDate = this.txtStartDate.Text.Replace("/", "-");
        StartDate = StartDate.Substring(3, 2) + "-" + StartDate.Substring(0, 2) + "-" + StartDate.Substring(6, 4);

        string EndDate = this.txtEndDate.Text.Replace("/", "-");
        EndDate = EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + "-" + EndDate.Substring(6, 4);

        cmd.Parameters.Add(new SqlParameter("@StartDate", StartDate));
        cmd.Parameters.Add(new SqlParameter("@EndDate", EndDate));


        SqlDataReader dr = cmd.ExecuteReader();

        dsInvoiceDetail dsDetail = new dsInvoiceDetail();
        while (dr.Read())
        {
            dsDetail.Tables[0].Rows.Add(dr[0].ToString(), System.Convert.ToDateTime(dr[1].ToString()), dr[2].ToString(), dr[3].ToString(), System.Convert.ToInt32(dr[4].ToString()), System.Convert.ToDouble(dr[5].ToString()));
        }

        dr.Close();

        this.rptViewer.LocalReport.DataSources.Clear();
        this.rptViewer.Reset();

        Microsoft.Reporting.WebForms.ReportDataSource rdInvDetail = new Microsoft.Reporting.WebForms.ReportDataSource("dsInvoiceDetail", dsDetail.Tables[0]);
        this.rptViewer.LocalReport.DataSources.Add(rdInvDetail);

        this.rptViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        this.rptViewer.LocalReport.ReportPath = @"Reports\Top20ByCost.rdlc";

        // Get the date range to show on the report
        var startDate = (from dta in dsDetail.Tables[0].AsEnumerable()
                         orderby dta.Field<DateTime>("CallDate") ascending
                         select (dta.Field<DateTime>("CallDate"))).FirstOrDefault();

        var endDate = (from dta in dsDetail.Tables[0].AsEnumerable()
                       orderby dta.Field<DateTime>("CallDate") ascending
                       select (dta.Field<DateTime>("CallDate"))).LastOrDefault();

        // Configure report params
        Microsoft.Reporting.WebForms.ReportParameter ClientDescription = null;
        if (Session["profilename"].ToString() == "Client")
        {
            ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", Session["ClientDesc"].ToString());
        }
        else
        {
            ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", this.lstClient.SelectedItem.ToString());
        }

        Microsoft.Reporting.WebForms.ReportParameter ReportDescription = new Microsoft.Reporting.WebForms.ReportParameter("ReportDescription", "From : " + startDate + Environment.NewLine + "To     : " + endDate);
        this.rptViewer.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { ClientDescription, ReportDescription });

        this.rptViewer.LocalReport.Refresh();
    }

    protected void RunTop20ByDuration()
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString());
        conn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "uspRptTop20ByDuration";
        cmd.CommandType = CommandType.StoredProcedure;
        if (Session["profilename"].ToString() == "Client")
        {
            cmd.Parameters.Add(new SqlParameter("@ClientId", Session["ClientId"].ToString()));
        }
        else
        {
            cmd.Parameters.Add(new SqlParameter("@ClientId", this.lstClient.SelectedValue.ToString()));
        }

        cmd.Parameters.Add(new SqlParameter("@StartDate", this.txtStartDate.Text));
        cmd.Parameters.Add(new SqlParameter("@EndDate", this.txtEndDate.Text));

        SqlDataReader dr = cmd.ExecuteReader();

        dsInvoiceDetail dsDetail = new dsInvoiceDetail();
        while (dr.Read())
        {
            dsDetail.Tables[0].Rows.Add(dr[0].ToString(), System.Convert.ToDateTime(dr[1].ToString()), dr[2].ToString(), dr[3].ToString(), System.Convert.ToInt32(dr[4].ToString()), System.Convert.ToDouble(dr[5].ToString()));
        }

        dr.Close();

        this.rptViewer.LocalReport.DataSources.Clear();
        this.rptViewer.Reset();

        Microsoft.Reporting.WebForms.ReportDataSource rdInvDetail = new Microsoft.Reporting.WebForms.ReportDataSource("dsInvoiceDetail", dsDetail.Tables[0]);
        this.rptViewer.LocalReport.DataSources.Add(rdInvDetail);

        this.rptViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        this.rptViewer.LocalReport.ReportPath = @"Reports\Top20ByDuration.rdlc";

        // Get the date range to show on the report
        var startDate = (from dta in dsDetail.Tables[0].AsEnumerable()
                         orderby dta.Field<DateTime>("CallDate") ascending
                         select (dta.Field<DateTime>("CallDate"))).FirstOrDefault();

        var endDate = (from dta in dsDetail.Tables[0].AsEnumerable()
                       orderby dta.Field<DateTime>("CallDate") ascending
                       select (dta.Field<DateTime>("CallDate"))).LastOrDefault();

        // Configure report params
        Microsoft.Reporting.WebForms.ReportParameter ClientDescription = null;
        if (Session["profilename"].ToString() == "Client")
        {
            ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", Session["ClientDesc"].ToString());
        }
        else
        {
            ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", this.lstClient.SelectedItem.ToString());
        }

        Microsoft.Reporting.WebForms.ReportParameter ReportDescription = new Microsoft.Reporting.WebForms.ReportParameter("ReportDescription", "From : " + startDate + Environment.NewLine + "To     : " + endDate);
        this.rptViewer.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { ClientDescription, ReportDescription });

        this.rptViewer.LocalReport.Refresh();
    }

    protected void RunUndefinedDest()
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString());
        conn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "uspUndefinedDestinations";
        cmd.CommandType = CommandType.StoredProcedure;

        SqlDataReader dr = cmd.ExecuteReader();

        dsUndefinedDest dsDetail = new dsUndefinedDest();
        while (dr.Read())
        {
            dsDetail.Tables[0].Rows.Add(System.Convert.ToDateTime(dr[0].ToString()), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
        }

        dr.Close();

        this.rptViewer.LocalReport.DataSources.Clear();
        this.rptViewer.Reset();

        Microsoft.Reporting.WebForms.ReportDataSource rdInvDetail = new Microsoft.Reporting.WebForms.ReportDataSource("dsUndefinedDest", dsDetail.Tables[0]);
        this.rptViewer.LocalReport.DataSources.Add(rdInvDetail);

        this.rptViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        this.rptViewer.LocalReport.ReportPath = @"Reports\UndefinedDestinations2.rdlc";

        this.rptViewer.LocalReport.Refresh();
    }

    protected void RunInternational()
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString());
        conn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "uspRptInternational";
        cmd.CommandType = CommandType.StoredProcedure;
        if (Session["profilename"].ToString() == "Client")
        {
            cmd.Parameters.Add(new SqlParameter("@ClientId", Session["ClientId"].ToString()));
        }
        else
        {
            cmd.Parameters.Add(new SqlParameter("@ClientId", this.lstClient.SelectedValue.ToString()));
        }


        cmd.Parameters.Add(new SqlParameter("@StartDate", this.txtStartDate.Text));
        cmd.Parameters.Add(new SqlParameter("@EndDate", this.txtEndDate.Text));

        SqlDataReader dr = cmd.ExecuteReader();

        dsInvoiceDetail dsDetail = new dsInvoiceDetail();
        while (dr.Read())
        {
            dsDetail.Tables[0].Rows.Add(dr[0].ToString(), System.Convert.ToDateTime(dr[1].ToString()), dr[2].ToString(), dr[3].ToString(), System.Convert.ToInt32(dr[4].ToString()), System.Convert.ToDouble(dr[5].ToString()));
        }

        dr.Close();

        this.rptViewer.LocalReport.DataSources.Clear();
        this.rptViewer.Reset();

        Microsoft.Reporting.WebForms.ReportDataSource rdInvDetail = new Microsoft.Reporting.WebForms.ReportDataSource("dsInvoiceDetail", dsDetail.Tables[0]);
        this.rptViewer.LocalReport.DataSources.Add(rdInvDetail);

        this.rptViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        this.rptViewer.LocalReport.ReportPath = @"Reports\International.rdlc";

        // Get the date range to show on the report
        var startDate = (from dta in dsDetail.Tables[0].AsEnumerable()
                         orderby dta.Field<DateTime>("CallDate") ascending
                         select (dta.Field<DateTime>("CallDate"))).FirstOrDefault();

        var endDate = (from dta in dsDetail.Tables[0].AsEnumerable()
                       orderby dta.Field<DateTime>("CallDate") ascending
                       select (dta.Field<DateTime>("CallDate"))).LastOrDefault();

        // Configure report params
        Microsoft.Reporting.WebForms.ReportParameter ClientDescription = null;
        if (Session["profilename"].ToString() == "Client")
        {
            ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", Session["ClientDesc"].ToString());
        }
        else
        {
            ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", this.lstClient.SelectedItem.ToString());
        }

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

            this.tblInvoices.Attributes.Add("style", "visibility:hidden");
            this.tblDateRange.Attributes.Add("style", "visibility:hidden");
        }
        else if (this.lstReport.SelectedIndex == 1)
        {
            this.tblInvoices.Attributes.Add("style", "visibility:hidden");
            this.tblDateRange.Attributes.Add("style", "visibility:hidden");
        }
        else
        {
            Invoice inv = new Invoice(ConfigurationManager.AppSettings["connString"].ToString());
            DataSet ds = inv.GetInvoiceHistory(this.lstClient.SelectedValue.ToString());
            if (ds.Tables[0].Rows.Count == 0)
            {
                this.lstClientHistory.Items.Clear();
                this.lstClientHistory.Items.Insert(0, "-- Current invoice --");
                this.lstClientHistory.SelectedIndex = 0;

                if (inv.HasUnInvoicedRecords(this.lstClient.SelectedValue.ToString()) == true)
                {

                    this.tblInvoices.Attributes.Add("style", "visibility:none");
                    this.tblDateRange.Attributes.Add("style", "visibility:hidden");
                }
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

                this.tblInvoices.Attributes.Add("style", "visibility:none");
                this.tblDateRange.Attributes.Add("style", "visibility:hidden");
            }
        }
    }

    protected void lstReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.lstClient.SelectedValue.ToString() == "-- Select Client --")
        {
            if (this.lstReport.SelectedValue.ToString() == "UNDEFINED_DEST")
            {
                this.tblInvoices.Attributes.Add("style", "visibility:hidden");
                this.tblDateRange.Attributes.Add("style", "visibility:hidden");
            }
            else
            {
                this.tblInvoices.Attributes.Add("style", "visibility:hidden");
                this.tblDateRange.Attributes.Add("style", "visibility:none");
            }
        }
        else
        {
            if (this.lstReport.SelectedValue.ToString() == "CURR_STATEMENT")
            {
                this.tblInvoices.Attributes.Add("style", "visibility:none");
                this.tblDateRange.Attributes.Add("style", "visibility:hidden");
            }
            else if (this.lstReport.SelectedValue.ToString() == "ORIGIN")
            {
                this.tblInvoices.Attributes.Add("style", "visibility:hidden");
                this.tblDateRange.Attributes.Add("style", "visibility:hidden");
            }
            else
            {
                this.tblInvoices.Attributes.Add("style", "visibility:hidden");
                this.tblDateRange.Attributes.Add("style", "visibility:none");
            }
        }

        this.rptViewer.LocalReport.ReportPath = "";
        this.rptViewer.LocalReport.Refresh();
    }

    protected void rptViewer_Drillthrough(object sender, Microsoft.Reporting.WebForms.DrillthroughEventArgs e)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString());
        conn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "uspRptDetailByOrigin";
        cmd.CommandType = CommandType.StoredProcedure;
        if (Session["profilename"].ToString() == "Client")
        {
            cmd.Parameters.Add(new SqlParameter("@ClientId", Session["ClientId"].ToString()));
        }
        else
        {
            cmd.Parameters.Add(new SqlParameter("@ClientId", this.lstClient.SelectedValue.ToString()));
        }

        Microsoft.Reporting.WebForms.ReportParameterInfoCollection DrillThroughValues =  e.Report.GetParameters();

        cmd.Parameters.Add(new SqlParameter("@Src", DrillThroughValues[0].Values[0]));
        SqlDataReader dr = cmd.ExecuteReader();

        dsInvoiceDetail ds = new dsInvoiceDetail();
        while (dr.Read())
        {
            ds.Tables[0].Rows.Add(dr[0].ToString(), System.Convert.ToDateTime(dr[1].ToString()), dr[2].ToString(), dr[3].ToString(), System.Convert.ToInt32(dr[4].ToString()), System.Convert.ToDouble(dr[5].ToString()));
        }

        dr.Close();
        conn.Close();
        conn.Dispose();

        this.rptViewer.LocalReport.DataSources.Clear();
        this.rptViewer.Reset();

        LocalReport drillThroughReport = (LocalReport)e.Report;

        Microsoft.Reporting.WebForms.ReportDataSource rdInvDetail = new Microsoft.Reporting.WebForms.ReportDataSource("dsInvoiceDetailwhatever", ds.Tables[0]);
        drillThroughReport.DataSources.Add(rdInvDetail);

        this.rptViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        this.rptViewer.LocalReport.ReportPath = @"Reports\StatementDetailBySrc.rdlc";

        Microsoft.Reporting.WebForms.ReportParameter Origin = new Microsoft.Reporting.WebForms.ReportParameter("Origin", DrillThroughValues[0].Values[0]);
        this.rptViewer.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { Origin });

        this.rptViewer.LocalReport.Refresh();
    }


}