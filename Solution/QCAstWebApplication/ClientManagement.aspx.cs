using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using QCAstSolution.Classes;
using QCAstWebApplication.QCAstServiceReference;

namespace QCAstWebApplication
{
    public partial class ClientManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    using (QCAstServiceClient client = new QCAstServiceClient())
                    {
                        int idUser = int.Parse(Session["IdUser"].ToString());
                        User user = client.GetUserFromId(idUser);

                        if (user.IdProfile != 4 & user.IdProfile != 3)
                        {
                            Response.Redirect("ErrorPage.aspx?Error=NotAuthorised", false);
                        }
                        else
                        {
                            ddlPrefContactMethod.DataSource = client.GetAllContactMethods();
                            ddlPrefContactMethod.DataBind();
                            lbSearchedClients.DataSource = client.GetAllClients("All", "");
                            lbSearchedClients.DataBind();
                            ddlPrefContactMethod.Items.Insert(0, new ListItem("Please select..", "0"));
                            trButtons.Visible = false;
                            ibEditClient.Enabled = false;
                            ibDeleteClient.Enabled = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    NotifyWebmasterOfError("Billing", "Client Management Page Load", ex.ToString());
                    Response.Redirect("ErrorPage.aspx?Error=GeneralError", true);
                }
            }
}
        
        protected void ClearListAndTextboxes()
        {
            tbClientId.Text = "";
            tbSearch.Text = "";
            tbAccountingCode.Text = "";
            tbCellular.Text = "";
            tbClientCode.Text = "";
            tbClientDesc.Text = "";
            tbEmail.Text = "";
            tbMaxDuration.Text = "";
            tbMaxIntDuration.Text = "";
            tbMaxIntPrice.Text = "";
            tbMaxPrice.Text = "";
            tbRangeFrom.Text = "";
            tbRangeTo.Text = "";
            lbSearchedClients.Items.Clear();
            trButtons.Visible = false;
            trIcons.Visible = true;
            ddlPrefContactMethod.SelectedIndex = 0;
            lblSearchError.Text = "";
            lblAmendError.Text = "";
            lblSuccess.Text = "";
            ibEditClient.Enabled = false;
            ibDeleteClient.Enabled = false;
            tbSearch.Enabled = true;
        }

        protected void ibBtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            lblAmendError.Text = "";
            bool clientExists = false;

            try
            {
                if (tbClientDesc.Text == "" | tbClientCode.Text == "" | tbAccountingCode.Text == "")
                {
                    lblAmendError.Text =
                        "Please provide values for the 'Client Name', 'Client Code' and 'Accounting Code' fields.";
                }
                else
                {
                    using (QCAstServiceClient client = new QCAstServiceClient())
                    {
                        clientExists = client.CheckIfClientExists(tbAccountingCode.Text.Trim());
                    }

                    if (clientExists)
                    {
                        lblAmendError.Text = "Client already exists in the database";
                    }
                    else
                    {
                        AddClient();
                    }
                }
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("Billing", "Client Management ibBtnSubmit_Click", ex.ToString());
                lblAmendError.Text =
                    "The website encountered a problem adding the client. The webmaster has been notified and will be attending to it shortly";
            }
        }

        private void AddClient()
        {
            bool success = false;
            string clientName = tbClientDesc.Text;
            if (tbClientDesc.Text == "" | tbClientCode.Text == "" | tbAccountingCode.Text == "")
            {
                lblAmendError.Text = "Please provide values for the Client Name, Client Code and Accounting Code fields";
            }
            else
            {
                try
                {
                    Client newClient = new Client
                                           {
                                               CellularNumber = tbCellular.Text != "" ? tbCellular.Text : null,
                                               AccountingCode = tbAccountingCode.Text,
                                               ClientCode = tbClientCode.Text.Trim(),
                                               ClientName = tbClientDesc.Text.Trim(),
                                               ContactMethod =
                                                   ddlPrefContactMethod.SelectedIndex == 0
                                                       ? 4
                                                       : int.Parse(ddlPrefContactMethod.SelectedValue),
                                               RangeFrom =
                                                   tbRangeFrom.Text != "" ? int.Parse(tbRangeFrom.Text) : (int?) null,
                                               RangeTo = tbRangeTo.Text != "" ? int.Parse(tbRangeTo.Text) : (int?) null,
                                               Email = tbEmail.Text != "" ? tbEmail.Text : null,
                                               MaxCallDuration =
                                                   tbMaxDuration.Text != ""
                                                       ? int.Parse(tbMaxDuration.Text)
                                                       : (int?) null,
                                               MaxCallPrice =
                                                   tbMaxPrice.Text != ""
                                                       ? decimal.Parse(tbMaxPrice.Text)
                                                       : (decimal?) null,
                                               MaxIntCallDuration =
                                                   tbMaxIntDuration.Text != ""
                                                       ? int.Parse(tbMaxIntDuration.Text)
                                                       : (int?) null,
                                               MaxIntCallPrice =
                                                   tbMaxIntPrice.Text != ""
                                                       ? decimal.Parse(tbMaxIntPrice.Text)
                                                       : (decimal?) null,
                                               LastUpdated = DateTime.Now
                                           };
                    using (QCAstServiceClient client = new QCAstServiceClient())
                    {
                        success = client.AddClient(newClient);
                    }
                    if (success)
                    {
                        ClearListAndTextboxes();
                        lblSuccess.Text = string.Format("Client '{0}' has been successfully added.", clientName);
                    }
                    else
                    {
                        lblAmendError.Text = string.Format("The website encountered and error saving Client '{0}'",
                                                           clientName);
                    }
                }
                catch (Exception ex)
                {
                    NotifyWebmasterOfError("Billing", "Client Management AddClient", ex.ToString());
                    lblAmendError.Text =
                        "The website encountered a problem saving the Client. The webmaster has been notified and will be attending to it shortly.";
                }
            }
        }

        protected void ibCancel_Click(object sender, ImageClickEventArgs e)
        {
            ClearListAndTextboxes();
        }

        protected void ibAddClient_Click(object sender, ImageClickEventArgs e)
        {
            ClearListAndTextboxes();
            tbClientId.Text = "(new)";
            tbSearch.Enabled = false;
            trIcons.Visible = false;
            trButtons.Visible = true;
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            string searchTerm = tbSearch.Text;
            ClearListAndTextboxes();

            if (searchTerm == "" & rblSearchFormat.SelectedValue != "All")
            {
                lblSearchError.Text = "Please enter a search term";
            }
            else
            {
                try
                {
                    using (QCAstServiceClient client = new QCAstServiceClient())
                    {
                        lblSearchError.Text = "";
                        List<Client> clients = client.GetAllClients(rblSearchFormat.SelectedValue, searchTerm);

                        if (clients == null)
                        {
                            lblSearchError.Text = "No results found";
                            lblSearchError.Visible = true;
                        }
                        else
                        {
                            lbSearchedClients.DataSource = clients;
                            lbSearchedClients.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    NotifyWebmasterOfError("Billing", "Client Management Search", ex.ToString());
                    lblSearchError.Text = "Error experienced";
                }
            }
        }

        protected void lbSearchedClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = int.Parse(((ListBox)sender).SelectedValue);
            ClearListAndTextboxes();
            try
            {
                using (QCAstServiceClient client = new QCAstServiceClient())
                {
                    Client fetchedClient = client.GetClientFromId(id);
                    tbClientId.Text = fetchedClient.IdClient.ToString();
                    tbClientDesc.Text = fetchedClient.ClientName;
                    tbRangeFrom.Text = fetchedClient.RangeFrom != null ? fetchedClient.RangeFrom.Value.ToString() : "";
                    tbRangeTo.Text = fetchedClient.RangeTo != null ? fetchedClient.RangeTo.Value.ToString() : "";
                    tbClientCode.Text = fetchedClient.ClientCode;
                    tbAccountingCode.Text = fetchedClient.AccountingCode;
                    tbCellular.Text = fetchedClient.CellularNumber ?? "";
                    tbEmail.Text = fetchedClient.Email ?? "";
                    ddlPrefContactMethod.SelectedIndex =
                        ddlPrefContactMethod.Items.IndexOf(
                            ddlPrefContactMethod.Items.FindByValue(fetchedClient.ContactMethod.ToString()));
                    tbMaxDuration.Text = fetchedClient.MaxCallDuration != null
                                             ? fetchedClient.MaxCallDuration.Value.ToString()
                                             : "";
                    tbMaxIntDuration.Text = fetchedClient.MaxIntCallDuration != null
                                                ? fetchedClient.MaxIntCallDuration.Value.ToString()
                                                : "";
                    string maxIntPrice = fetchedClient.MaxIntCallPrice != null
                                             ? fetchedClient.MaxIntCallPrice.Value.ToString()
                                             : "";
                    tbMaxIntPrice.Text = maxIntPrice != "" ? maxIntPrice.Remove(maxIntPrice.Length - 4, 3) : "";
                    string maxPrice = fetchedClient.MaxCallPrice != null
                                          ? fetchedClient.MaxCallPrice.Value.ToString()
                                          : "";
                    tbMaxPrice.Text = maxPrice != "" ? maxPrice.Remove(maxPrice.Length - 4, 3) : "";
                }
                ibEditClient.Enabled = true;
                ibDeleteClient.Enabled = true;
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("Billing", "User Management SearchSelectedIndexChanged", ex.ToString());
            }
        }

        protected void EditClient()
        {
            lblAmendError.Text = "";
            bool success = false;
            string clientName = tbClientDesc.Text;
            try
            {
                Client editClient = new Client
                                        {
                                            CellularNumber = tbCellular.Text != "" ? tbCellular.Text : null,
                                            AccountingCode = tbAccountingCode.Text,
                                            ClientCode = tbClientCode.Text,
                                            ClientName = tbClientDesc.Text,
                                            ContactMethod = ddlPrefContactMethod.SelectedIndex == 0 ? 4 : int.Parse(ddlPrefContactMethod.SelectedValue),
                                            IdClient = int.Parse(tbClientId.Text),
                                            MaxCallDuration = tbMaxDuration.Text != "" ? int.Parse(tbMaxDuration.Text) : (int?)null,
                                            MaxCallPrice = tbMaxPrice.Text != "" ? decimal.Parse(tbMaxPrice.Text) : (decimal?)null,
                                            MaxIntCallDuration = tbMaxIntDuration.Text != "" ? int.Parse(tbMaxIntDuration.Text) : (int?)null,
                                            MaxIntCallPrice = tbMaxIntPrice.Text != "" ? decimal.Parse(tbMaxIntPrice.Text) : (decimal?)null,
                                            RangeFrom = tbRangeFrom.Text != "" ? int.Parse(tbRangeFrom.Text) : (int?)null,
                                            RangeTo = tbRangeTo.Text != "" ? int.Parse(tbRangeTo.Text) : (int?)null,
                                            Email = tbEmail.Text != "" ? tbEmail.Text : null
                                        };
                using (QCAstServiceClient client = new QCAstServiceClient())
                {
                    success = client.EditClient(editClient);
                }
                if (success)
                {
                    ClearListAndTextboxes();
                    lblSuccess.Text = string.Format("Client {0} has been successfully amended.", clientName);
                }
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("Billing", "Client Management EditClient", ex.ToString());
                lblAmendError.Text = string.Format("The website experienced a problem editing client {0}. The webmaster has been notified and will be attending to the problem.", clientName);
            }
        }

        protected void DeleteClient()
        {
            lblAmendError.Text = "";
            bool success = false;
            string clientName = tbClientDesc.Text;
            try
            {
                using (QCAstServiceClient client = new QCAstServiceClient())
                {
                    success = client.DeleteClient(int.Parse(tbClientId.Text));
                }
                if (success)
                {
                    ClearListAndTextboxes();
                    lblSuccess.Text = string.Format("Client {0} has been successfully deleted.", clientName);
                }
                else
                {
                    lblAmendError.Text = "Cannot delete the client at this time. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("Billing", "Client Management", ex.ToString());
                lblAmendError.Text = string.Format("The website experienced a problem deleting client {0}. The webmaster has been notified and will be attending to the problem.", clientName);
            }
        }

        protected void ibDeleteClient_Click(object sender, ImageClickEventArgs e)
        {
            DeleteClient();
        }

        protected void ibEditClient_Click(object sender, ImageClickEventArgs e)
        {
            if (tbClientDesc.Text == "" | tbClientCode.Text == "" | tbAccountingCode.Text == "")
            {
                lblAmendError.Text = "Please provide values for the Client Name, Client Code and Accounting Code fields";
            }
            else
            {
                EditClient();
            }
        }

        public void NotifyWebmasterOfError(string website, string page, string error)
        {
            MailMessage message = new MailMessage();
            message.To.Add(new MailAddress("jaco@qcsolutions.co.za"));
            message.Subject = "Exception experienced on page";
            message.Body = string.Format("Website {0}, Page {1}, error {2}", website, page, error);
            SmtpClient client = new SmtpClient();
            client.Send(message);
        }
    }
}