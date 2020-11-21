using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VacationTrackingSystem.Pages.Users
{
    public partial class VacationsPage : System.Web.UI.Page
    {
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            QR = DBConnection.qrVacation_List;
            if (!IsPostBack)
            {
                gvFill(QR);
                ddlTypeFill();
                    HttpCookie cookieReq = Request.Cookies["My localhost cookie"];
            }
        }
        //Заполнение таблицы
        private void gvFill(string qr)
        {
            sdsVacations.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsVacations.SelectCommand = qr;
            sdsVacations.DataSourceMode = SqlDataSourceMode.DataReader;
            gvVacations.DataSource = sdsVacations;
            gvVacations.DataBind();
        }

        //Заполнение данными списка типов
        private void ddlTypeFill()
        {
            sdsType.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsType.SelectCommand = DBConnection.qrType;
            sdsType.DataSourceMode = SqlDataSourceMode.DataReader;
            ddlType.DataSource = sdsType;
            ddlType.DataTextField = "Тип отпуска";
            ddlType.DataValueField = "IDVacationType";
            ddlType.DataBind();
        }

        //Очитска полей
        protected void Cleaner()
        {
            tbEndDate.Text = string.Empty;
            tbStartDate.Text = string.Empty;
            ddlType.SelectedIndex = 0;
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                DataProcedures procedures = new DataProcedures();
                DateTime theDate = DateTime.ParseExact(tbStartDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                string startDate = theDate.ToString("dd.MM.yyyy");
                DateTime theDate1 = DateTime.ParseExact(tbEndDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                string endDate = theDate1.ToString("dd.MM.yyyy");
                DateTime dateNow = DateTime.UtcNow.Date;
                //Расчёт количества дней 
                TimeSpan days = theDate1 - theDate;
                double daysAmount = days.TotalDays;
                string selectedType = ddlType.SelectedItem.Text;
                procedures.VacationList_Insert(startDate, endDate, dateNow.ToString(), false, Convert.ToInt32(daysAmount), Convert.ToInt32(ddlType.SelectedValue), 2);
                Cleaner();
                gvFill(QR);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('"+ selectedType +", успешно добавлен.')", true);
            }
            catch
            {

            }

        }
        protected void btUpdate_Click(object sender, EventArgs e)
        {

        }

        protected void gvVacations_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvVacations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
        }
    }
}