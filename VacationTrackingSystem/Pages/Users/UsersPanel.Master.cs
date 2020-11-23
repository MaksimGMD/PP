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
            //if (!IsPostBack)
            //{
            //    DBConnection connection = new DBConnection();
            //    switch (DBConnection.idUser != 0)
            //    {
            //        case (true):
            //            liOut.Visible = true;
            //            liPersonalCard.Visible = true;
            //            break;
            //        case (false):
            //            liOut.Visible = false;
            //            liPersonalCard.Visible = false;
            //            break;
            //    }
            //}
        }
        //Выход из учётной записи
        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            Response.Redirect("../AuthorizationPage.aspx");
            DBConnection.idUser = 0;
        }

        protected void btIn_Click(object sender, EventArgs e)
        {

        }
    }
}