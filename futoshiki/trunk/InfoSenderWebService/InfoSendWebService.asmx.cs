/*
 * $Id$
 * 
 * Coursework – Futoshiki.InfoSendWebService
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 */


using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Web.Services;

namespace InfoSenderWebService
{
    /// <summary>
    /// Summary description for InfoSendWebService
    /// </summary>
    [WebService(Namespace = "http://localhost:8888/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class InfoSendWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public bool SendMail(string from, string to,string subject, string body)
        {
            bool isSend = true;
            try
            {
                MailMessage msg = new MailMessage();

                msg.From = new MailAddress(from);
                msg.To.Add(to);
                msg.Subject = subject;
                msg.Body = body;

                SmtpClient client = new SmtpClient();
                client.Credentials = 
                    new NetworkCredential("adriftrock@gmail.com", "hhhhhhhh0");
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                
                client.Send(msg);                                   
            }catch
            {
                isSend = false;
            }

            return isSend;
        } 
    }
}
