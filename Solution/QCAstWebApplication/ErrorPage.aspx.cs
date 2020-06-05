using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QCAstWebApplication
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string errorCode = Request.QueryString["Error"];

            switch (errorCode)
            {
                case "GeneralError":
                    hErrorTitle.InnerText = "The website encountered an Error";
                    pErrorDescription.InnerText = "The webmaster has been notified and will be attending to the problem momentarily.";
                    break;
                case "BadRequest":
                    hErrorTitle.InnerText = "Bad Request";
                    pErrorDescription.InnerText = "";
                    break;
                case "NotAuthorised":
                    hErrorTitle.InnerText = "Not Authorised";
                    pErrorDescription.InnerText = "You are not authorised to view this section of the website.";
                    break;
                case "Forbidden":
                    hErrorTitle.InnerText = "Forbidden";
                    pErrorDescription.InnerText = "";
                    break;
                case "FileNotFound":
                    hErrorTitle.InnerText = "File Not Found";
                    pErrorDescription.InnerText = "";
                    break;
                case "RequestTimeout":
                    hErrorTitle.InnerText = "Request Timeout";
                    pErrorDescription.InnerText = "";
                    break;
                case "InternalServerError":
                    hErrorTitle.InnerText = "Internal Server Error";
                    pErrorDescription.InnerText = "";
                    break;
                case "ServiceUnavailable":
                    hErrorTitle.InnerText = "Service Unavailable";
                    pErrorDescription.InnerText = "";
                    break;
                default:
                    hErrorTitle.InnerText = "The website encountered an Error";
                    pErrorDescription.InnerText = errorCode;
                    break;
            }

        }
    }
}