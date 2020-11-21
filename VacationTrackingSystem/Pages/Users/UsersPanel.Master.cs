using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VacationTrackingSystem.Pages.Users
{
    public partial class UsersPanel : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //Выход из учётной записи
        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            Response.Redirect("MainPage.aspx");
            //Удаление cookies
            HttpCookie cookie = new HttpCookie("Authorization");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
        }
    }
}