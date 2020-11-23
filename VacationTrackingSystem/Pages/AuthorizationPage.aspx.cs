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
                    lblAuthError.Visible = false;
                    if (DBConnection.idUser != 0)
                    {
                        //Проверка прав по должности
                        switch (connection.userRole(DBConnection.idUser))
                        {
                            //Администратор
                            case ("Администратор"):
                                Response.Redirect("Admin/AdminPersonalCardPage.aspx");
                                break;
                            //Сотрудник отдела кадров
                            case ("Сотрудник отдела кадров"):
                                Response.Redirect("Admin/VacationPage.aspx");
                                break;
                            //Обычный сотрудник
                            default:
                                Response.Redirect("Users/MainPage.aspx");
                                break;
                        }
                    }
                    break;
            }
        }
    }
}