using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using QCAstSolution.Classes;
using QCAstWebApplication.QCAstServiceReference;

namespace QCAstWebApplication
{
    public partial class AdminBilling : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdUser"] == null)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            else
            {
                if (!IsPostBack)
                {
                    try
                    {
                        using (QCAstServiceClient client = new QCAstServiceClient())
                        {
                            int idUser = int.Parse(Session["IdUser"].ToString());
                            User user = client.GetUserFromId(idUser);

                            if (user.IdProfile != 4)
                            {
                                Response.Redirect("ErrorPage.aspx?Error=NotAuthorised", false);
                            }
                            else
                            {
                                ddlClients.DataSource = client.GetAllClients("All", "");
                                ddlClients.DataBind();
                                ddlClients.Items.Insert(0, new ListItem("Please select..", "00"));
                                ClearListBoxes();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        NotifyWebmasterOfError("Billing", "Billing Page_Load", ex.ToString());
                        Response.Redirect("ErrorPage.aspx?Error=GeneralError");
                    }
                }
            }
        }
        
        protected void ClearListBoxes()
        {
            ddlClients.SelectedIndex = 0;
            ddlClientInvoiceHistory.Items.Clear();
            ddlClientInvoiceHistory.Items.Insert(0, new ListItem("-- Current Invoice --", "0"));
            btnCreateInvoice.Visible = false;
            txtEndDate.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1).ToString("yyyy-MM-dd");
        }

        protected void DdlClientsSelectedIndexChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (ddlClients.SelectedIndex == 0)
            {
                ClearListBoxes();
            }
            else
            {
                try
                {
                    using (var client = new QCAstServiceClient())
                    {
                        ddlClientInvoiceHistory.Items.Clear();
                        ddlClientInvoiceHistory.DataSource =
                            client.GetInvoicesForClient(int.Parse(ddlClients.SelectedValue));
                        ddlClientInvoiceHistory.DataBind();
                        ddlClientInvoiceHistory.Items.Insert(0, new ListItem("-- Current Invoice --", "0"));
                    }
                }
                catch (Exception ex)
                {
                    NotifyWebmasterOfError("Billing", "Billing ddlSelectedIndex_Changed", ex.ToString());
                    lblError.Text =
                        "The website encountered a problem retrieving the list of invoices for this client.  The webmaster has been notified and will be attending to the problem shortly.";
                }
            }
        }

        protected void IbGoClick(object sender, ImageClickEventArgs e)
        {
            if (ddlClients.SelectedIndex == 0)
            {
                Response.Write("<script type='text/javascript'>alert('Please select a client');</script>");
            }
            else
            {
                if (ddlClientInvoiceHistory.SelectedValue == "0")
                {
                    RunCurrentInvoice();
                }
                else
                {
                    RunInvoiceHistory();
                }
            }
        }

        private void RunCurrentInvoice()
        {
            lblError.Text = "";
            try
            {
                DateTime periodEndDate;
                var idClient = int.Parse(ddlClients.SelectedValue);
                if (DateTime.TryParseExact(Request.Form[txtEndDate.UniqueID], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out periodEndDate))
                {
                    using (var client = new QCAstServiceClient())
                    {
                        rptViewer.LocalReport.DataSources.Clear();
                        rptViewer.Reset();
                        List<InvoiceDetail> invoiceDetails = client.GetUninvoicedDetailsForClient(idClient, periodEndDate);
                        List<InvoiceSummary> invoiceSummaries =
                            client.GetUninvoicedSummaryForClient(idClient, periodEndDate);
                        if (invoiceDetails.Count == 0)
                        {
                            lblError.Text = string.Format("No data available for Client {0}", ddlClients.SelectedItem.Text);
                        }
                        else
                        {
                            var rdInvDetail = new ReportDataSource("dsInvoiceDetail",
                                                                                invoiceDetails.AsEnumerable());
                            var rdInvSummary = new ReportDataSource("dsInvoiceSummary",
                                                                                 invoiceSummaries.AsEnumerable());

                            rptViewer.LocalReport.DataSources.Add(rdInvDetail);
                            rptViewer.LocalReport.DataSources.Add(rdInvSummary);
                            rptViewer.ProcessingMode = ProcessingMode.Local;
                            rptViewer.LocalReport.ReportPath = @"Reports\InvoiceDetail.rdlc";

                            var startDate =
                                invoiceDetails.OrderByDescending(p => p.CallDate).LastOrDefault().CallDate.ToString();
                            var endDate =
                                invoiceDetails.OrderByDescending(p => p.CallDate).FirstOrDefault().CallDate.ToString();

                            var clientDescription = new ReportParameter("ClientDescription",
                                                                                    ddlClients.SelectedItem.ToString());
                            var reportDescription = new ReportParameter("ReportDescription",
                                                                                    "From : " + startDate +
                                                                                    Environment.NewLine + "To     : " +
                                                                                    endDate);
                            var totalDuration = new ReportParameter("TotalDuration",
                                                                                client.GetTotalDuration(idClient,
                                                                                                        periodEndDate));
                            rptViewer.LocalReport.SetParameters(new ReportParameter[]
                                                                    {
                                                                        clientDescription, reportDescription, totalDuration
                                                                    });
                            rptViewer.LocalReport.Refresh();
                            btnCreateInvoice.Visible = true;
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

        private void RunInvoiceHistory()
        {
            lblError.Text = "";
            try
            {
                var idClient = int.Parse(ddlClients.SelectedValue);
                var idInvoice = int.Parse(ddlClientInvoiceHistory.SelectedValue);
                using (var client = new QCAstServiceClient())
                {
                    rptViewer.LocalReport.DataSources.Clear();
                    rptViewer.Reset();
                    var invoiceDate = client.GetInvoiceDate(idInvoice);
                    var invoiceDetails = client.GetInvoicedDetailsForClient(idClient, idInvoice);
                    var invoiceSummaries = client.GetInvoicedSummaryForClient(idClient, idInvoice);
                    if (invoiceDetails.Count == 0)
                    {
                        lblError.Text = string.Format("No data available for Client {0}", ddlClients.SelectedItem.Text);
                    }
                    else
                    {
                        var rdInvDetail = new ReportDataSource("dsInvoiceDetail",
                                                                            invoiceDetails.AsEnumerable());
                        var rdInvSummary = new ReportDataSource("dsInvoiceSummary",
                                                                                invoiceSummaries.AsEnumerable());

                        rptViewer.LocalReport.DataSources.Add(rdInvDetail);
                        rptViewer.LocalReport.DataSources.Add(rdInvSummary);
                        rptViewer.ProcessingMode = ProcessingMode.Local;
                        rptViewer.LocalReport.ReportPath = @"Reports\InvoiceDetail.rdlc";

                        var startDate =
                            invoiceDetails.OrderByDescending(p => p.CallDate).LastOrDefault().CallDate.ToString();
                        var endDate =
                            invoiceDetails.OrderByDescending(p => p.CallDate).FirstOrDefault().CallDate.ToString();

                        var clientDescription = new ReportParameter("ClientDescription",
                                                                                ddlClients.SelectedItem.ToString());
                        var reportDescription = new ReportParameter("ReportDescription",
                                                                                "From : " + startDate +
                                                                                Environment.NewLine + "To     : " +
                                                                                endDate);
                        var totalDuration = new ReportParameter("TotalDuration",
                                                                            client.GetTotalDuration(idClient, invoiceDate));
                        rptViewer.LocalReport.SetParameters(new ReportParameter[]
                                                                {
                                                                    clientDescription, reportDescription, totalDuration
                                                                });
                        rptViewer.LocalReport.Refresh();
                        btnCreateInvoice.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("Billing", "AdminBilling RunInvoiceHistory", ex.ToString());
                lblError.Text =
                    "The website encountered a problem retrieving the invoice detail for this report. The webmaster has been notified and will be attending to the problem.";
            }
        }

        public void NotifyWebmasterOfError(string website, string page, string error)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(new MailAddress("jaco@qcsolutions.co.za"));
                message.Subject = "Exception experienced on page";
                message.Body = string.Format("Website {0}, Page {1}, error {2}", website, page, error);
                SmtpClient client = new SmtpClient();
                client.Send(message);
            }
            catch (Exception)
            {
                
            }
        }

        protected void BtnCreateInvoiceClick(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            DateTime endDate;
            var idUser = int.Parse(Session["IdUser"].ToString());
            var idClient = int.Parse(ddlClients.SelectedValue);
            if(DateTime.TryParseExact(Request.Form[txtEndDate.UniqueID], "yyyy-MM-dd",
                                               CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                try
                {
                    using (var client = new QCAstServiceClient())
                    {
                        bool success = client.CreateInvoice(idClient, endDate, idUser);
                        if (success)
                        {
                            lblSuccess.Text = "Invoice has been successfully captured in the database.";
                            ClearListBoxes();
                        }
                        else
                        {
                            lblError.Text =
                                "The website encountered a problem creating an invoice. The webmaster has been notified and will be attending to the problem.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    NotifyWebmasterOfError("Billing", "AdminBilling btnCreateInvoice", ex.ToString());
                    lblError.Text =
                        "The website encountered a problem creating an invoice.  The webmaster has been notified and will be attending to the problem.";
                }
            }
        else
        {
            Response.Write("<script type='text/javascript'>alert('Please enter a valid date');</script>");    
        }
        }
    }
}