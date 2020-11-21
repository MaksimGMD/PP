using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Crypt_Library;

namespace VacationTrackingSystem.Pages
{
    public partial class AuthorizationPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btLogin_Click(object sender, EventArgs e)
        {
            DBConnection connection = new DBConnection();
            //Проверка авторизациия
            switch (Convert.ToString(connection.Authorization(tbLogin.Text, tbPassword.Text)))
            {
                case ("0"):
                    lblAuthError.Visible = true;
                    break;
                default:
                    // Создать объект cookie
                    HttpCookie cookie = new HttpCookie("Authorization");
                    // Установить значения в нем
                    cookie["Logged"] = Convert.ToString(connection.Authorization(tbLogin.Text, tbPassword.Text));
                    Response.Cookies.Add(cookie);
                    lblAuthError.Visible = false;
                    //Создаёт запрос cookie
                    HttpCookie cookieReq = Request.Cookies["Authorization"];
                    if (cookieReq != null)
                    {
                        //Проверка прав по должности
                        switch (connection.userRole(Convert.ToInt32(cookieReq["Logged"])))
                        {
                            //Администратор
                            case ("Администратор"):
                                Response.Redirect("");
                                break;
                            //Сотрудник отдела кадров
                            case (""):
                                Response.Redirect("");
                                break;
                            //Обычный сотрудник
                            default:
                                Response.Redirect("MainPage.aspx");
                                break;
                        }
                    }
                    break;
            }
        }
    }
}