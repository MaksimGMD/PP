using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VacationTrackingSystem.Pages.Admin
{
    public partial class VacationPage : System.Web.UI.Page
    {
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (DBConnection.idUser != 0)
            {
                QR = DBConnection.qrVacation_Order;
                if (!IsPostBack)
                {
                    gvFill(QR);
                }
            }
            else
            {
                Response.Redirect("../AuthorizationPage.aspx");
            }
        }
        private void gvFill(string qr)
        {
            sdsVacationOrder.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsVacationOrder.SelectCommand = qr;
            sdsVacationOrder.DataSourceMode = SqlDataSourceMode.DataReader;
            gvVacation.DataSource = sdsVacationOrder;
            gvVacation.DataBind();
        }

        //Очистка полей
        private void Cleaner()
        {
            SelectedMessage.Visible = false;
            DBConnection.idRecord = 0;
            tbAplicationDate.Text = string.Empty;
            tbEmployee.Text = string.Empty;
            tbEndDate.Text = string.Empty;
            tbOrderDate.Text = string.Empty;
            tbOrderNumber.Text = string.Empty;
            tbPosition.Text = string.Empty;
            tbStartDate.Text = string.Empty;
            tbStatus.Text = string.Empty;
            tbTypeVacation.Text = string.Empty;
        }
        
        //Согласовать отпуск
        protected void btConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (DBConnection.idRecord != 0)
                {
                    DateTime dateNow = DateTime.UtcNow.Date;
                    DataProcedures procedures = new DataProcedures();
                    procedures.VacationOrder_Insert(dateNow.ToString(), DBConnection.idRecord);
                    procedures.VacationList_Status_Update(DBConnection.idRecord, "Согласован");
                    Response.Redirect(Request.Url.AbsoluteUri);
                    gvFill(QR);
                    Cleaner();
                    btConfirm.Visible = false;
                    btReject.Visible = false;
                    DBConnection.idRecord = 0;
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось выполнить :(')", true);
            }
        }
        //Отказать в отпуске
        protected void btReject_Click(object sender, EventArgs e)
        {
            try
            {
                if (DBConnection.idRecord != 0)
                {
                    int selectedRow = gvVacation.SelectedRow.RowIndex;
                    GridViewRow rows = gvVacation.SelectedRow;
                    int vacationDays = Convert.ToInt32(rows.Cells[9].Text.ToString());
                    int cardID = Convert.ToInt32(rows.Cells[3].Text.ToString());
                    int cardDays = Convert.ToInt32(rows.Cells[15].Text.ToString());
                    int daysAmount = cardDays + vacationDays;
                    DataProcedures procedures = new DataProcedures();
                    procedures.VacationList_Status_Update(DBConnection.idRecord, "Отклонен");
                    procedures.Days_Amount_Update(daysAmount, cardID);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Отпуск успешно отклонен.')", true);
                    gvFill(QR);
                    Cleaner();
                    btConfirm.Visible = false;
                    btReject.Visible = false;
                    DBConnection.idRecord = 0;
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось выполнить :(')", true);
            }
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
            if(rows.Cells[10].Text.ToString() == "Ожидание")
            {
                DBConnection.idRecord = Convert.ToInt32(rows.Cells[1].Text.ToString());
                tbEmployee.Text = rows.Cells[4].Text.ToString();
                tbPosition.Text = rows.Cells[5].Text.ToString();
                tbTypeVacation.Text = rows.Cells[6].Text.ToString();
                tbStartDate.Text = Convert.ToDateTime(rows.Cells[7].Text.ToString()).ToString("yyyy-MM-dd");
                tbEndDate.Text = Convert.ToDateTime(rows.Cells[8].Text.ToString()).ToString("yyyy-MM-dd");
                tbStatus.Text = rows.Cells[10].Text.ToString();
                tbAplicationDate.Text = rows.Cells[11].Text.ToString();
                tbOrderNumber.Text = string.Empty;
                tbOrderDate.Text = string.Empty;
                btConfirm.Visible = true;
                btReject.Visible = true;
                SelectedMessage.Visible = true;
                lblSelectedRow.Text = "Выбрана строка с ID " + DBConnection.idRecord;
            }
            else
            {
                DBConnection.idRecord = Convert.ToInt32(rows.Cells[1].Text.ToString());
                tbEmployee.Text = rows.Cells[4].Text.ToString();
                tbPosition.Text = rows.Cells[5].Text.ToString();
                tbTypeVacation.Text = rows.Cells[6].Text.ToString();
                tbStartDate.Text = rows.Cells[7].Text.ToString();
                tbEndDate.Text = rows.Cells[8].Text.ToString();
                tbStatus.Text = rows.Cells[10].Text.ToString();
                tbAplicationDate.Text = rows.Cells[11].Text.ToString();
                tbOrderNumber.Text = rows.Cells[13].Text.ToString();
                tbOrderDate.Text = rows.Cells[14].Text.ToString();
                btConfirm.Visible = false;
                btReject.Visible = false;
                SelectedMessage.Visible = true;
                lblSelectedRow.Text = "Выбрана строка с ID " + DBConnection.idRecord;
            }

        }

        protected void gvVacation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[12].Visible = false;
            e.Row.Cells[13].Visible = false;
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
                case ("ID"):
                    e.SortExpression = "[IDVacationList]";
                    break;
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
                    e.SortExpression = "[VacationList].[DaysAmount]";
                    break;
                case ("Статус отпуска"):
                    e.SortExpression = "[Status]";
                    break;
                case ("Дата заявления"):
                    e.SortExpression = "[ApplicationDate]";
                    break;
                case ("Дата приказа"):
                    e.SortExpression = "[OrderDate]";
                    break;
                case ("Сотрудник"):
                    e.SortExpression = "[Surname] + ' ' + [Name] + ' ' + [MiddleName]";
                    break;
                case ("Должность"):
                    e.SortExpression = "[PositionName]";
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
        //Отмена поиска
        protected void btCancel_Click(object sender, EventArgs e)
        {
            tbSearch.Text = string.Empty;
            btCancel.Visible = false;
            gvFill(QR);
        }

        //Отменить выбор
        protected void btCanselSelected_Click(object sender, EventArgs e)
        {
            Cleaner();
            btConfirm.Visible = false;
            btReject.Visible = false;
            DBConnection.idRecord = 0;
            SelectedMessage.Visible = false;
            gvFill(QR);
        }
        //Поиск
        protected void btSearch_Click(object sender, EventArgs e)
        {
            if (tbSearch.Text != "")
            {
                foreach (GridViewRow row in gvVacation.Rows)
                {
                    if (row.Cells[4].Text.Equals(tbSearch.Text) ||
                    row.Cells[1].Text.Equals(tbSearch.Text) ||
                    row.Cells[5].Text.Equals(tbSearch.Text) ||
                    row.Cells[7].Text.Equals(tbSearch.Text) ||
                    row.Cells[8].Text.Equals(tbSearch.Text) ||
                    row.Cells[9].Text.Equals(tbSearch.Text) ||
                    row.Cells[10].Text.Equals(tbSearch.Text) ||
                    row.Cells[11].Text.Equals(tbSearch.Text) ||
                    row.Cells[14].Text.Equals(tbSearch.Text))
                        row.BackColor = ColorTranslator.FromHtml("#a1f2be");
                    else
                        row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                }
                btCancel.Visible = true;
            }
        }
        //Фильтрация
        protected void btFilter_Click(object sender, EventArgs e)
        {
            if (tbSearch.Text != "")
            {
                string newQR = QR + "where [IDVacationList] like '%" + tbSearch.Text + "%' or [Surname] + ' ' + [Name] + ' ' + [MiddleName] like '%" + tbSearch.Text + "%' or [TypeName] like '%" + tbSearch.Text + "%' or [VacationStartDate] like '%" + tbSearch.Text + "%' or" +
                    "[VacationEndDate] like '%" + tbSearch.Text + "%' or [VacationList].[DaysAmount] like '%" + tbSearch.Text + "%' or [Status] like '%" + tbSearch.Text + "%' " +
                    "or [ApplicationDate] like '%" + tbSearch.Text + "%' or [OrderDate] like '%" + tbSearch.Text + "%' or [PositionName] like '%" + tbSearch.Text + "%'";
                gvFill(newQR);
                btCancel.Visible = true;
            }
        }
    }
}