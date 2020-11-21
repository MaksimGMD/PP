using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VacationTrackingSystem.Pages.Admin
{
    public partial class AdminPanel : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DBConnection connection = new DBConnection();
                //Создаёт запрос cookie
                HttpCookie cookieReq = Request.Cookies["Authorization"];
                if (cookieReq != null)
                {
                    switch (connection.userRole(Convert.ToInt32(cookieReq["Logged"])))
                    {
                        //Сотрудник отдела кадров
                        case ("Сотрудник отдела кадров"):
                            navbarDropdown.Text = "Сотрудник отдела кадров";
                            aUsers.Visible = false;
                            aType.Visible = false;
                            dvOut.Visible = true;
                            break;
                        //Администратор
                        case ("Администратор"):
                            navbarDropdown.Text = "Администратор";
                            dvOut.Visible = true;
                            break;
                        default:
                            navbarDropdown.Text = "Авторизоваться";
                            dvIn.Visible = true;
                            break;
                    }
                }
            }
        }
        //Выход из учётной записи
        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Users/MainPage.aspx");
            //Удаление cookies
            HttpCookie cookie = new HttpCookie("Authorization");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
        }
        //Вход в учётную запись
        protected void Unnamed2_Click(object sender, EventArgs e)
        {
            Response.Redirect("../AuthorizationPage.aspx");
        }
    }
}