using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using QCAstSolution.Classes;
using QCAstWebApplication.QCAstServiceReference;

namespace QCAstWebApplication
{
    public partial class ClientBilling : System.Web.UI.Page
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
                            if (user.IdClient == null)
                            {
                                Response.Redirect("AdminBilling.aspx");
                            }
                            else
                            {
                                int idClient = user.IdClient.Value;
                                ddlClientInvoiceHistory.Items.Clear();
                                ddlClientInvoiceHistory.DataSource = client.GetInvoicesForClient(idClient);
                                ddlClientInvoiceHistory.DataBind();
                                ddlClientInvoiceHistory.Items.Insert(0, new ListItem("Please select..", "0"));
                                Session.Add("IdClient", idClient);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        NotifyWebmasterOfError("Billing", "ClientBilling Page_Load", ex.ToString());
                        Response.Redirect("ErrorPage.aspx?Error=GeneralError");
                    }
                }
            }
        }

        protected void ibGo_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlClientInvoiceHistory.SelectedValue == "0")
            {
                Response.Write("<script type='text/javascript'>alert('Please select an Invoice');</script>");
            }
            else
            {
                RunInvoiceHistory();
            }
        }

        private void RunInvoiceHistory()
        {
            lblError.Text = "";
            try
            {
                int idClient = int.Parse(Session["IdClient"].ToString());
                int idInvoice = int.Parse(ddlClientInvoiceHistory.SelectedValue);
                using (QCAstServiceClient client = new QCAstServiceClient())
                {
                    Client clientName = client.GetClientFromId(idClient);
                    rptViewer.LocalReport.DataSources.Clear();
                    rptViewer.Reset();
                    DateTime invoiceDate = client.GetInvoiceDate(idInvoice);
                    List<InvoiceDetail> invoiceDetails = client.GetInvoicedDetailsForClient(idClient, idInvoice);
                    List<InvoiceSummary> invoiceSummaries = client.GetInvoicedSummaryForClient(idClient, idInvoice);
                    if (invoiceDetails.Count == 0)
                    {
                        lblError.Text = string.Format("No data available for invoice date {0}", invoiceDate);
                    }
                    else
                    {
                        ReportDataSource rdInvDetail = new ReportDataSource("dsInvoiceDetail",
                                                                            invoiceDetails.AsEnumerable());
                        ReportDataSource rdInvSummary = new ReportDataSource("dsInvoiceSummary",
                                                                                invoiceSummaries.AsEnumerable());

                        rptViewer.LocalReport.DataSources.Add(rdInvDetail);
                        rptViewer.LocalReport.DataSources.Add(rdInvSummary);
                        rptViewer.ProcessingMode = ProcessingMode.Local;
                        rptViewer.LocalReport.ReportPath = @"Reports\InvoiceDetail.rdlc";

                        string startDate =
                            invoiceDetails.OrderByDescending(p => p.CallDate).LastOrDefault().CallDate.ToString();
                        string endDate =
                            invoiceDetails.OrderByDescending(p => p.CallDate).FirstOrDefault().CallDate.ToString();

                        ReportParameter ClientDescription = new ReportParameter("ClientDescription", clientName.ClientName);
                        ReportParameter ReportDescription = new ReportParameter("ReportDescription",
                                                                                "From : " + startDate +
                                                                                Environment.NewLine + "To     : " +
                                                                                endDate);
                        ReportParameter TotalDuration = new ReportParameter("TotalDuration",
                                                                            client.GetTotalDuration(idClient, invoiceDate));
                        rptViewer.LocalReport.SetParameters(new ReportParameter[]
                                                                {
                                                                    ClientDescription, ReportDescription, TotalDuration
                                                                });
                        rptViewer.LocalReport.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("ClientBilling", "Billing RunInvoiceHistory", ex.ToString());
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

    }
}