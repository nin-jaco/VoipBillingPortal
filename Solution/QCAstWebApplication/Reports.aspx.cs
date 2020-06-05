using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.Reporting.WebForms;
using QCAstSolution.Classes;
using QCAstWebApplication.Datasets;
using System.Web.Security;
using QCAstWebApplication.QCAstServiceReference;

namespace QCAstWebApplication
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["IdUser"] == null)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                else
                {
                    var idUser = int.Parse(Session["IdUser"].ToString());
                    try
                    {
                        using (var serviceClient = new QCAstServiceClient())
                        {
                            var user = serviceClient.GetUserFromId(idUser);
                            if (user.IdProfile != 4)
                            {
                                Response.Redirect("ErrorPage.aspx?Error=NotAuthorised");
                            }
                            else
                            {
                                ddlReport.DataSource = serviceClient.GetAllReports();
                                ddlReport.DataBind();
                                ddlReport.Items.Insert(0, new ListItem("Please select..", "0"));
                                ddlClient.DataSource = serviceClient.GetAllClients("All", "");
                                ddlClient.DataBind();
                                ddlClient.Items.Insert(0, new ListItem("Please select..", "0"));
                                ClearListAndTextBoxes();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        NotifyWebmasterOfError("Billing", "Reports PageLoad", ex.ToString());   
                    }
                }
            }
        }

        protected void ClearListAndTextBoxes()
        {
            ddlClient.SelectedIndex = 0;
            txtStartDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            trDateRow.Visible = false;
            trClientRow.Visible = false;
            iRunReport.Enabled = false;
            rptViewer.LocalReport.ReportPath = "";
            rptViewer.LocalReport.Refresh();
            lblReportName.Text = "";
        }

        protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlReport.SelectedValue)
            {
                case "0":
                    ClearListAndTextBoxes();
                    break;
                case "1":
                    lblReportName.Text = "Current Statement";
                    trDateRow.Visible = false;
                    trClientRow.Visible = true;
                    iRunReport.Enabled = true;
                    RunCurrentStatement();
                    break;
                case "2":
                    lblReportName.Text = "Current-Summary by Origin";
                    trDateRow.Visible = false;
                    trClientRow.Visible = true;
                    iRunReport.Enabled = true;
                    RunSummaryByOrigin();
                    break;
                case "3":
                    lblReportName.Text = "Top 20 Calls by Cost";
                    trDateRow.Visible = true;
                    trClientRow.Visible = true;
                    iRunReport.Enabled = true;
                    RunTopTwentyCallsByCost();
                    break;
                case "4":
                    lblReportName.Text = "Top 20 Calls by Duration";
                    trDateRow.Visible = true;
                    trClientRow.Visible = true;
                    iRunReport.Enabled = true;
                    RunTopTwentyCallsByDuration();
                    break;
                case "5":
                    lblReportName.Text = "International";
                    trDateRow.Visible = false;
                    trClientRow.Visible = false;
                    iRunReport.Enabled = true;
                    RunInternational();
                    break;
                case "6":
                    lblReportName.Text = "Undefined Destinations";
                    trDateRow.Visible = false;
                    trClientRow.Visible = false;
                    iRunReport.Enabled = true;
                    RunUndefinedDestinations();
                    break;
                case "7":
                    lblReportName.Text = "Historical Summary";
                    trDateRow.Visible = false;
                    trClientRow.Visible = false;
                    iRunReport.Enabled = true;
                    RunHistoricalSummary();
                    break;
                default:
                    break;
            }
        }

        private void RunUndefinedDestinations()
        {
            lblError.Text = "";
            try
            {
                using(var client  = new QCAstServiceClient())
                {
                    rptViewer.LocalReport.DataSources.Clear();
                    rptViewer.Reset();

                    var rdInvDetail = new ReportDataSource("dsUndefinedDest", client.GetUndefinedDestinations().AsEnumerable());
                    rptViewer.LocalReport.DataSources.Add(rdInvDetail);

                    rptViewer.ProcessingMode = ProcessingMode.Local;
                    rptViewer.LocalReport.ReportPath = @"Reports\UndefinedDestinations2.rdlc";

                    rptViewer.LocalReport.Refresh();
                }
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("Billing", "Reports", ex.ToString());
                lblError.Text = "The website encountered a problem running the report.  The webmaster has been notified and will be attending to the problem.";
            }
        }

        private void RunTopTwentyCallsByDuration()
        {
            DateTime periodEndDate;
            DateTime periodStartDate;
            var clientId = int.Parse(ddlClient.SelectedValue);
            if (!DateTime.TryParseExact(Request.Form[txtEndDate.UniqueID], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out periodEndDate) | !DateTime.TryParseExact(Request.Form[txtEndDate.UniqueID], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out periodStartDate))
            {
                Response.Write("<script type='text/javascript'>alert('Please select valid dates in the Start Date and End Date fields!');</script>");
            }
            else if (ddlClient.SelectedIndex == 0)
            {
                Response.Write("<script type='text/javascript'>alert('Please select a client!');</script>");
            }
            else
            {
                try
                {
                    using (var client = new QCAstServiceClient())
                    {
                        rptViewer.LocalReport.DataSources.Clear();
                        rptViewer.Reset();
                        var topTwenty = client.GetTop20ByDuration(clientId, periodStartDate, periodEndDate);
                        rptViewer.LocalReport.DataSources.Add(new ReportDataSource("dsInvoiceDetail", topTwenty.AsEnumerable()));
                        rptViewer.ProcessingMode = ProcessingMode.Local;
                        rptViewer.LocalReport.ReportPath = @"Reports\Top20ByDuration.rdlc";
                        var startDate = topTwenty.OrderByDescending(p => p.CallDate).LastOrDefault().CallDate;
                        var endDate = topTwenty.OrderByDescending(p => p.CallDate).FirstOrDefault().CallDate;
                        var clientDescription = new ReportParameter("ClientDescription", ddlClient.SelectedValue);
                        var reportDescription = new ReportParameter("ReportDescription", "From : " + startDate + Environment.NewLine + "To     : " + endDate);
                        rptViewer.LocalReport.SetParameters(new ReportParameter[] { clientDescription, reportDescription });
                        rptViewer.LocalReport.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    NotifyWebmasterOfError("Billing", "Report RunTopTwentyCallsByDuration", ex.ToString());
                }
            }
        }

        private void RunTopTwentyCallsByCost()
        {
            DateTime periodEndDate;
            DateTime periodStartDate;
            var clientId = int.Parse(ddlClient.SelectedValue);
            if (!DateTime.TryParseExact(Request.Form[txtEndDate.UniqueID], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out periodEndDate) | !DateTime.TryParseExact(Request.Form[txtEndDate.UniqueID], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out periodStartDate))
            {
                Response.Write("<script type='text/javascript'>alert('Please select valid dates in the Start Date and End Date fields!');</script>");
            }
            else if (ddlClient.SelectedIndex == 0)
            {
                Response.Write("<script type='text/javascript'>alert('Please select a client!');</script>");
            }
            else
            {
                try
                {
                    using (var client = new QCAstServiceClient())
                    {
                        rptViewer.LocalReport.DataSources.Clear();
                        rptViewer.Reset();
                        var topTwenty = client.GetTop20ByCost(clientId, periodStartDate, periodEndDate);
                        rptViewer.LocalReport.DataSources.Add(new ReportDataSource("dsInvoiceDetail", topTwenty.AsEnumerable()));
                        rptViewer.ProcessingMode = ProcessingMode.Local;
                        rptViewer.LocalReport.ReportPath = @"Reports\Top20ByCost.rdlc";
                        var startDate = topTwenty.OrderByDescending(p => p.CallDate).LastOrDefault().CallDate;
                        var endDate = topTwenty.OrderByDescending(p => p.CallDate).FirstOrDefault().CallDate;
                        var clientDescription = new ReportParameter("ClientDescription", ddlClient.SelectedValue);
                        var reportDescription = new ReportParameter("ReportDescription", "From : " + startDate + Environment.NewLine + "To     : " + endDate);
                        rptViewer.LocalReport.SetParameters(new ReportParameter[] { clientDescription, reportDescription });
                        rptViewer.LocalReport.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    NotifyWebmasterOfError("Billing", "Report RunTopTwentyCallsByDuration", ex.ToString());
                }
            }
        }

        private void RunSummaryByOrigin()
        {
            var clientId = int.Parse(ddlClient.SelectedValue);
            if (ddlClient.SelectedIndex == 0)
            {
                Response.Write("<script type='text/javascript'>alert('Please select a client!');</script>");
            }
            else
            {
                try
                {
                    using (var client = new QCAstServiceClient())
                    {
                        var clientName = client.GetClientFromId(clientId).ClientName;
                        rptViewer.LocalReport.DataSources.Add(new ReportDataSource("dsInvoiceSummary", client.GetSummaryByOrigin(clientId)));
                        rptViewer.ProcessingMode = ProcessingMode.Local;
                        rptViewer.LocalReport.ReportPath = @"Reports\StatementSummaryBySrc.rdlc";
                        var clientDescription = new ReportParameter("ClientDescription", clientName);
                        rptViewer.LocalReport.SetParameters(new[] { clientDescription });
                        rptViewer.LocalReport.EnableHyperlinks = true;
                        rptViewer.LocalReport.Refresh();
                        rptViewer.LocalReport.DataSources.Clear();
                        rptViewer.Reset();
                    }
                }
                catch (Exception ex)
                {
                    NotifyWebmasterOfError("Billing", "Report RunTopTwentyCallsByDuration", ex.ToString());
                }
            }
        }

        private void RunCurrentStatement()
        {
            lblError.Text = "";
            try
            {
                DateTime periodEndDate;
                var idClient = int.Parse(ddlClient.SelectedValue);
                if (DateTime.TryParseExact(Request.Form[txtEndDate.UniqueID], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out periodEndDate))
                {
                    using (var client = new QCAstServiceClient())
                    {
                        rptViewer.LocalReport.DataSources.Clear();
                        rptViewer.Reset();
                        var rdInvDetail = new ReportDataSource("dsInvoiceDetail", client.GetInvoiceDetail);
                        var rdInvSummary = new ReportDataSource("dsInvoiceSummary", dsSummary.Tables[0]);
                        rptViewer.LocalReport.DataSources.Add(rdInvDetail);
            rptViewer.LocalReport.DataSources.Add(rdInvSummary);

            rptViewer.ProcessingMode = ProcessingMode.Local;
            rptViewer.LocalReport.ReportPath = @"Reports\InvoiceDetail.rdlc";

            // Get the date range to show on the report
            var startDate = (from dta in dsDetail.Tables[0].AsEnumerable()
                             orderby dta.Field<DateTime>("CallDate") ascending
                             select (dta.Field<DateTime>("CallDate"))).FirstOrDefault();

            var endDate = (from dta in dsDetail.Tables[0].AsEnumerable()
                           orderby dta.Field<DateTime>("CallDate") ascending
                           select (dta.Field<DateTime>("CallDate"))).LastOrDefault();

            // Configure report params
            ReportParameter ClientDescription = null;
            if (Session["profilename"].ToString() == "Client")
            {
                ClientDescription = new ReportParameter("ClientDescription", Session["ClientDesc"].ToString());
            }
            else
            {
                ClientDescription = new Microsoft.Reporting.WebForms.ReportParameter("ClientDescription", this.lstClient.SelectedItem.ToString());
            }

            Microsoft.Reporting.WebForms.ReportParameter ReportDescription = new Microsoft.Reporting.WebForms.ReportParameter("ReportDescription", "From : " + startDate + Environment.NewLine + "To     : " + endDate);
            this.rptViewer.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { ClientDescription, ReportDescription });

            this.rptViewer.LocalReport.Refresh();
                        }
                    }
                }
                else
                {
                    Response.Write("<script type='text/javascript'>alert('Please select a valid date');</script>");
                }
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("Billing", "Billing RunCurrentInvoice", ex.ToString());
                lblError.Text =
                    "The website encountered a problem retrieving the invoice detail for this report. The webmaster has been notified and will be attending to the problem.";
            }
        }

        protected void iRunReport_Click(object sender, ImageClickEventArgs e)
        {

        }

        public void NotifyWebmasterOfError(string website, string page, string error)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress("jaco@qcsolutions.co.za"));
            message.Subject = "Exception experienced on page";
            message.Body = string.Format("Website {0}, Page {1}, error {2}", website, page, error);
            var client = new SmtpClient();
            client.Send(message);
        }

        private static string[] GetLastSixMonths()
        {
            var months = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                if (DateTimeFormatInfo.CurrentInfo != null)
                    months.Add(DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Now.Month - i));
            }
            return months.ToArray();
        }





        protected bool RunStatement()
        {
            /*
            
             * */
            return true;
        }

        protected bool RunHistory()
        {
            /*
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
             * */
            return true;
        }

        protected void RunInternational()
        {
            /*
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
             * */
        }

        protected void lstClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            // Reset the report viewer irrespective of what was showing at the time
            rptViewer.LocalReport.ReportPath = "";
            rptViewer.LocalReport.Refresh();

            // Check for any previous invoice runs for the currently-selected client and, if found, populate the drop-down accordingly
            if (lstClient.SelectedIndex == 0)
            {
                //Invoice inv = new Invoice(ConfigurationManager.AppSettings["connString"].ToString());
                //if(inv.HasUnInvoicedRecords(this.lstClient.SelectedValue.ToString()) == true)
                //{
                lstClientHistory.Items.Clear();
                lstClientHistory.Items.Insert(0, "-- Current invoice --");
                lstClientHistory.SelectedIndex = 0;
                //}

                tblInvoices.Visible = false;
                tblDateRange.Visible = false;
            }
            else if (lstReport.SelectedIndex == 1)
            {
                tblInvoices.Visible = false;
                tblDateRange.Visible = false;
            }
            else
            {
                /*List<Invoice> ds = serviceClient.GetInvoiceHistory(this.lstClient.SelectedValue.ToString());
                if (ds.Count == 0)
                {
                    lstClientHistory.Items.Clear();
                    lstClientHistory.Items.Insert(0, "-- Current invoice --");
                    lstClientHistory.SelectedIndex = 0;

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
             * */
        }



        protected void rptViewer_Drillthrough(object sender, Microsoft.Reporting.WebForms.DrillthroughEventArgs e)
        {
            /*cmd.CommandType = CommandType.StoredProcedure;
            if (Session["profilename"].ToString() == "Client")
            {
                cmd.Parameters.Add(new SqlParameter("@ClientId", Session["ClientId"].ToString()));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@ClientId", this.lstClient.SelectedValue.ToString()));
            }

            Microsoft.Reporting.WebForms.ReportParameterInfoCollection DrillThroughValues = e.Report.GetParameters();

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
             * */
        }
        
        protected void RunHistoricalSummary()
        {
            /*
            int month = DateTime.Now.Month;

            using (QCAstServiceClient serviceClient = new QCAstServiceClient())
            {
                //List<KeyValuePair<string, List<object>>> graphSeries = serviceClient.BillingsForGraph(month, idClient);
                

                Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                .SetTitle(new Title { Text = "Graphical Historical Summary Report" })
                .SetSubtitle(new Subtitle { Text = "Source: qcsolutions.co.za" })
                .SetXAxis(new XAxis
                {
                    Categories = GetLastSixMonths(),
                    Title = new XAxisTitle { Text = string.Empty }
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title = new YAxisTitle
                    {
                        Text = "Call Totals",
                        Align = AxisTitleAligns.High
                    },
                    Categories = new[] { "5000", "10000", "15000", "20000", "25000", "30000", "35000", "40000" }
                })
                .SetTooltip(new Tooltip { Formatter = "function() { return 'R'+ this.series.name +': '+ this.y +''; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        PointPadding = 0.2,
                        BorderWidth = 0
                    }
                })
                .SetLegend(new Legend
                {
                    Layout = Layouts.Vertical,
                    Align = HorizontalAligns.Right,
                    VerticalAlign = VerticalAligns.Top,
                    X = -10,
                    Y = 100,
                    Floating = true,
                    BorderWidth = 1,
                    BackgroundColor = ColorTranslator.FromHtml("#FFFFFF"),
                    Shadow = true
                })
                //.SetCredits(new Credits { Enabled = false })
                
                .SetSeries(new[]
                           {
                               new Series { Name = "Partner", Data = new Data(networkPartnerTotals.ToArray()) },
                               new Series { Name = "Landline", Data = new Data(landlineTotals.ToArray()) },
                               new Series { Name = "Mobile", Data = new Data(internationalTotals.ToArray()) }
                           });
                
            grapSum.Text = chart.ToHtmlString();
            graphContainer.Visible = true;

            }*/
        }

        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void lstClientHistory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}