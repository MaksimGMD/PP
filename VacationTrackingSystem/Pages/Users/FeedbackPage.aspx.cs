using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace VacationTrackingSystem.Pages.Users
{
    public partial class FeedbackPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                alert.Visible = false;
            }
        }
        protected void btSubmit_Click(object sender, EventArgs e)
        {
            //Отправка сообщения на почту
            try
            {
                int port = 587;
                bool enableSSL = true;

                string emailFrom = "bot.feedback@bk.ru"; //почта отправителя
                string password = "Privet@12345"; //пароль оправителя
                string emailTo = "i_m.d.gritsuk@mpt.ru"; //Почта получателя
                string subject = ddlTittle.SelectedValue.ToString(); //Заголовок сообщения
                string smtpAddress = "smtp.mail.ru"; //smtp протокол
                string tittle = ddlTittle.SelectedValue.ToString(); //Заголовок сообщения;
                string name = "от: " + tbName.Text.ToString() + " почта: " + tbmail.Text;
                string message = tbMessage.Text; //Сообщение

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = tittle + "\r\n" + name + "\r\n" + "---------------------------------------" + "\r\n" + message; //тело сообщения
                mail.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient(smtpAddress, port))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
                alert.Visible = true;
            }
            catch
            {
                alert.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось отправить сообщение :(')", true);
            }
        }
    }
}