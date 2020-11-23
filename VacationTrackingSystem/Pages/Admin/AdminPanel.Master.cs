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
                if (DBConnection.idUser != 0)
                {
                    switch (connection.userRole(DBConnection.idUser))
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
                            aUsers.Visible = false;
                            aType.Visible = false;
                            aExten.Visible = false;
                            aRecall.Visible = false;
                            aVacations.Visible = false;
                            break;
                    }
                }
            }
        }
        //Выход из учётной записи
        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Users/MainPage.aspx");
            DBConnection.idUser = 0;
        }
        //Вход в учётную запись
        protected void Unnamed2_Click(object sender, EventArgs e)
        {
            Response.Redirect("../AuthorizationPage.aspx");
        }
    }
}