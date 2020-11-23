using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace VacationTrackingSystem.Pages.Users
{
    public partial class VacationsPage : System.Web.UI.Page
    {
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (DBConnection.idUser != 0)
            {
                QR = DBConnection.qrVacation_List + "where [PersonalCardID] = '" + DBConnection.idUser + "'";
                if (!IsPostBack)
                {
                    gvFill(QR);
                    ddlTypeFill();
                    pageDataFill();
                }
            }
            else
            {
                Response.Redirect("../AuthorizationPage.aspx");
            }
        }
        //Заполнение таблицы
        private void gvFill(string qr)
        {
            sdsVacations.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsVacations.SelectCommand = qr;
            sdsVacations.DataSourceMode = SqlDataSourceMode.DataReader;
            gvVacation.DataSource = sdsVacations;
            gvVacation.DataBind();
        }

        //Заполнение данными списка типов
        private void ddlTypeFill()
        {
            sdsType.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsType.SelectCommand = DBConnection.qrType;
            sdsType.DataSourceMode = SqlDataSourceMode.DataReader;
            ddlType.DataSource = sdsType;
            ddlType.DataTextField = "Тип отпуска";
            ddlType.DataValueField = "ID";
            ddlType.DataBind();
        }
        //Заполнение страницы данными из БД
        private void pageDataFill()
        {
            SqlCommand command = new SqlCommand("", DBConnection.connection);
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "select [DaysAmount] from [PersonalCard] where [IDPersonalCard] = '" + DBConnection.idUser + "'";
            DBConnection.connection.Open();
            lbldaysAvailable.Text = command.ExecuteScalar().ToString();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        //Очитска полей
        protected void Cleaner()
        {
            btCancel.Visible = false;
            btDelete.Visible = false;
            DBConnection.idRecord = 0;
            tbEndDate.Text = string.Empty;
            tbStartDate.Text = string.Empty;
            ddlType.SelectedIndex = 0;
            liDaysAmountError.Visible = false;
            liIntervalError.Visible = false;
            liCorrectDate.Visible = false;
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            string selectedType = ddlType.SelectedItem.Text;
            DataProcedures procedures = new DataProcedures();
            //Дата начала
            DateTime theDate = DateTime.ParseExact(tbStartDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            string startDate = theDate.ToString("dd.MM.yyyy");
            //Дата окончания
            DateTime theDate1 = DateTime.ParseExact(tbEndDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            string endDate = theDate1.ToString("dd.MM.yyyy");
            DateTime dateNow = DateTime.UtcNow.Date;
            //Расчёт 2 недель
            TimeSpan weeks = theDate - dateNow;
            double twoWeeks = weeks.TotalDays;
            //Расчёт количества дней 
            TimeSpan days = theDate1 - theDate;
            double daysAmount = days.TotalDays; //Кол-во дней
            //Первая дата > второй даты
            if (theDate > theDate1)
            {
                alert.Visible = true;
                liCorrectDate.Visible = true;
            }
            else
            {
                liCorrectDate.Visible = false;
                //Проверка, что до отпуска >2 недель
                if (Convert.ToInt32(twoWeeks) < 14)
                {
                    alert.Visible = true;
                    liIntervalError.Visible = true;
                }
                else
                {
                    liIntervalError.Visible = false;
                    //Проверка количетсва доступных дней
                    if (Convert.ToInt32(lbldaysAvailable.Text) < daysAmount)
                    {
                        alert.Visible = true;
                        liDaysAmountError.Visible = true;
                    }
                    else
                    {
                        liDaysAmountError.Visible = false;
                        alert.Visible = false;
                        try
                        {
                            int vacatinDays = Convert.ToInt32(lbldaysAvailable.Text) - Convert.ToInt32(daysAmount);
                            procedures.VacationList_Insert(startDate, endDate, dateNow.ToString(), "Ожидание", Convert.ToInt32(daysAmount), Convert.ToInt32(ddlType.SelectedValue), DBConnection.idUser);
                            procedures.Days_Amount_Update(vacatinDays, DBConnection.idUser);
                            Cleaner();
                            gvFill(QR);
                            pageDataFill();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + selectedType + ", успешно добавлен. Ожидайте подтверждения отпуска руководителем.')", true);
                        }
                        catch
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось добавить запись :(')", true);
                        }
                    }
                }
            }

        }
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            string selectedType = ddlType.SelectedItem.Text;
            DataProcedures procedures = new DataProcedures();
            //Дата начала
            DateTime theDate = DateTime.ParseExact(tbStartDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            string startDate = theDate.ToString("dd.MM.yyyy");
            //Дата окончания
            DateTime theDate1 = DateTime.ParseExact(tbEndDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            string endDate = theDate1.ToString("dd.MM.yyyy");
            DateTime dateNow = DateTime.UtcNow.Date;
            //Расчёт 2 недель
            TimeSpan weeks = theDate - dateNow;
            double twoWeeks = weeks.TotalDays;
            //Расчёт количества дней 
            TimeSpan days = theDate1 - theDate;
            double daysAmount = days.TotalDays; //Кол-во дней
            //Первая дата > второй даты
            if (theDate > theDate1)
            {
                alert.Visible = true;
                liCorrectDate.Visible = true;
            }
            else
            {
                liCorrectDate.Visible = false;
                //Проверка, что до отпуска >2 недель
                if (Convert.ToInt32(twoWeeks) < 14)
                {
                    alert.Visible = true;
                    liIntervalError.Visible = true;
                }
                else
                {
                    liIntervalError.Visible = false;
                    //Проверка количетсва доступных дней
                    if (Convert.ToInt32(lbldaysAvailable.Text) < daysAmount)
                    {
                        alert.Visible = true;
                        liDaysAmountError.Visible = true;
                    }
                    else
                    {
                        liDaysAmountError.Visible = false;
                        alert.Visible = false;
                        try
                        {
                            //Количество дней 
                            GridViewRow rows = gvVacation.SelectedRow;
                            int daysToUpdate = Convert.ToInt32(rows.Cells[5].Text.ToString()); //Дне в оформленном отпуске
                            daysToUpdate += Convert.ToInt32(lbldaysAvailable.Text); //Дне в оформленном отпуске + остаток
                            int vacationDays = daysToUpdate - Convert.ToInt32(daysAmount);
                            ddlType.SelectedValue = rows.Cells[1].Text.ToString();
                            procedures.VacationList_Update(DBConnection.idRecord, startDate, endDate, "Ожидание", Convert.ToInt32(daysAmount), Convert.ToInt32(ddlType.SelectedValue.ToString()), DBConnection.idUser);
                            procedures.Days_Amount_Update(vacationDays, DBConnection.idUser);
                            Cleaner();
                            gvFill(QR);
                            pageDataFill();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + selectedType + ", успешно добавлен. Ожидайте подтверждения отпуска руководителем.')", true);
                        }
                        catch
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось добавить запись :(')", true);
                        }
                    }
                }
            }
        }

        //Отмена выбора
        protected void btCancel_Click(object sender, EventArgs e)
        {
            Cleaner();
            btUpdate.Visible = false;
            btDelete.Visible = false;
            DBConnection.idRecord = 0;
            gvFill(QR);
        }

        protected void gvVacation_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvVacation.Rows)
            {
                if (row.RowIndex == gvVacation.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            int selectedRow = gvVacation.SelectedRow.RowIndex;
            GridViewRow rows = gvVacation.SelectedRow;
            if(rows.Cells[6].Text.ToString() == "Ожидание")
            {
                DBConnection.idRecord = Convert.ToInt32(rows.Cells[0].Text.ToString());
                ddlType.SelectedValue = rows.Cells[1].Text.ToString();
                tbStartDate.Text = Convert.ToDateTime(rows.Cells[3].Text.ToString()).ToString("yyyy-MM-dd");
                tbEndDate.Text = Convert.ToDateTime(rows.Cells[4].Text.ToString()).ToString("yyyy-MM-dd");
                btCancel.Visible = true;
                btUpdate.Visible = true;
                btDelete.Visible = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Запись больше нельзя изменять или удалять')", true);
            }
        }

        protected void gvVacation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvVacation, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }

        protected void gvVacation_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string strField = string.Empty;
            switch (e.SortExpression)
            {
                case ("Тип отпуска"):
                    e.SortExpression = "[TypeName]";
                    break;
                case ("Дата начала"):
                    e.SortExpression = "[VacationStartDate]";
                    break;
                case ("Дата окончания"):
                    e.SortExpression = "[VacationEndDate]";
                    break;
                case ("Количество дней"):
                    e.SortExpression = "[DaysAmount]";
                    break;
                case ("Статус отпуска"):
                    e.SortExpression = "[Status]";
                    break;
                case ("Дата заявления"):
                    e.SortExpression = "[ApplicationDate]";
                    break;
            }
            sortGridView(gvVacation, e, out sortDirection, out strField);
            string strDirection = sortDirection
                == SortDirection.Ascending ? "ASC" : "DESC";
            gvFill(QR + " order by " + e.SortExpression + " " + strDirection);
        }
        private void sortGridView(GridView gridView,
         GridViewSortEventArgs e,
         out SortDirection sortDirection,
         out string strSortField)
        {
            strSortField = e.SortExpression;
            sortDirection = e.SortDirection;

            if (gridView.Attributes["CurrentSortField"] != null &&
                gridView.Attributes["CurrentSortDirection"] != null)
            {
                if (strSortField ==
                    gridView.Attributes["CurrentSortField"])
                {
                    if (gridView.Attributes["CurrentSortDirection"]
                        == "ASC")
                    {
                        sortDirection = SortDirection.Descending;
                    }
                    else
                    {
                        sortDirection = SortDirection.Ascending;
                    }
                }
            }
            gridView.Attributes["CurrentSortField"] = strSortField;
            gridView.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC"
                : "DESC");
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            int days; //Дни для добавления к дням отпуска
            try
            {
                DataProcedures procedures = new DataProcedures();
                //Количество дней 
                GridViewRow rows = gvVacation.SelectedRow;
                days = Convert.ToInt32(rows.Cells[5].Text.ToString());
                days += Convert.ToInt32(lbldaysAvailable.Text);
                ddlType.SelectedValue = rows.Cells[1].Text.ToString();
                procedures.Days_Amount_Update(days, DBConnection.idUser);
                procedures.VacationList_Delete(DBConnection.idRecord);
                Cleaner();
                gvFill(QR);
                pageDataFill();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Запись успешно удалена.')", true);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось удалить запись :(')", true);
            }
        }
    }

}