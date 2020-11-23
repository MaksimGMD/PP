using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VacationTrackingSystem.Pages.Admin
{
    public partial class ExtensionVacationPage : System.Web.UI.Page
    {
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            QR = DBConnection.qrExtension;
            if (!IsPostBack)
            {
                gvFill(QR);
                ddlOrderFill();
            }
        }
        private void gvFill(string qr)
        {
            sdsExtension.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsExtension.SelectCommand = qr;
            sdsExtension.DataSourceMode = SqlDataSourceMode.DataReader;
            gvExtension.DataSource = sdsExtension;
            gvExtension.DataBind();
        }
        //Заполнение списка приказов
        private void ddlOrderFill()
        {
            sdsVacationNumber.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsVacationNumber.SelectCommand = DBConnection.qrOrdersList;
            sdsVacationNumber.DataSourceMode = SqlDataSourceMode.DataReader;
            ddlOrder.DataSource = sdsVacationNumber;
            ddlOrder.DataTextField = "Приказ";
            ddlOrder.DataValueField = "IDVacationOrder";
            ddlOrder.DataBind();
        }
        //Очитска полей
        protected void Cleaner()
        {
            SelectedMessage.Visible = false;
            DBConnection.idRecord = 0;
            ddlOrder.SelectedIndex = 0;
            tbEndDate.Text = string.Empty;
            tbReason.Text = string.Empty;
            tbStartDate.Text = string.Empty;
        }
        protected void gvExtension_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string strField = string.Empty;
            switch (e.SortExpression)
            {
                case ("Номер отпуска"):
                    e.SortExpression = "[OrderNumber]";
                    break;
                case ("Номер приказа"):
                    e.SortExpression = "[ExtensionOrderNumber]";
                    break;
                case ("Дата приказа"):
                    e.SortExpression = "[ExtensionDate]";
                    break;
                case ("Причина продления"):
                    e.SortExpression = "[ExtensionReason]";
                    break;
                case ("Должность"):
                    e.SortExpression = "[PositionName]";
                    break;
                case ("Дата начала"):
                    e.SortExpression = "[ExtensionStartDate]";
                    break;
                case ("Дата окончания"):
                    e.SortExpression = "[ExtensionEndDate]";
                    break;
            }
            sortGridView(gvExtension, e, out sortDirection, out strField);
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

        protected void gvExtension_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[2].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvExtension, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }
        //Удаление
        protected void gvExtension_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int Index = Convert.ToInt32(e.RowIndex);
                DataProcedures procedure = new DataProcedures();
                GridViewRow rows = gvExtension.SelectedRow;
                DBConnection.idRecord = Convert.ToInt32(gvExtension.Rows[Index].Cells[1].Text.ToString());
                procedure.ExtensionOrder_Delete(DBConnection.idRecord);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Запись успешно удалена.')", true);
                gvFill(QR);
                Cleaner();
                ddlOrderFill();
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось удалить запись :(')", true);
            }
        }

        protected void gvExtension_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvExtension.Rows)
            {
                if (row.RowIndex == gvExtension.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            int selectedRow = gvExtension.SelectedRow.RowIndex;
            GridViewRow rows = gvExtension.SelectedRow;
            DBConnection.idRecord = Convert.ToInt32(rows.Cells[1].Text.ToString());
            tbEndDate.Text = Convert.ToDateTime(rows.Cells[8].Text.ToString()).ToString("yyyy-MM-dd");
            tbStartDate.Text = Convert.ToDateTime(rows.Cells[7].Text.ToString()).ToString("yyyy-MM-dd");
            tbReason.Text = rows.Cells[6].Text.ToString();
            SelectedMessage.Visible = true;
            lblSelectedRow.Text = "Выбрана строка с ID " + DBConnection.idRecord;
            btUpdate.Visible = true;
        }
        //Добавление
        protected void btInsert_Click(object sender, EventArgs e)
        {
            DataProcedures procedures = new DataProcedures();
            try
            {
                SqlCommand command = new SqlCommand("", DBConnection.connection);
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "select [VacationEndDate] from [VacationOrder] inner join VacationList on [IDVacationList] = [VacationListID] " +
                    "where [IDVacationOrder] like " + ddlOrder.SelectedValue.ToString() + "";
                DBConnection.connection.Open();
                string startDate = command.ExecuteScalar().ToString();
                command.ExecuteNonQuery();
                DBConnection.connection.Close();
                //Дата окончания
                DateTime theDate1 = DateTime.ParseExact(tbEndDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                string endDate = theDate1.ToString("dd.MM.yyyy");
                DateTime dateNow = DateTime.UtcNow.Date;
                procedures.ExtensionOrder_Insert(dateNow.ToString(), tbReason.Text.ToString(), startDate, endDate, Convert.ToInt32(ddlOrder.SelectedValue.ToString()));
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Запись успешно добавлен.')", true);
                gvFill(QR);
                Cleaner();
                ddlOrderFill();
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось добавить запись :(')", true);
            }
        }
        //Обновление
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            DataProcedures procedures = new DataProcedures();
            try
            {
                if (DBConnection.idRecord != 0)
                {
                    SqlCommand command = new SqlCommand("", DBConnection.connection);
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "select [VacationEndDate] from [VacationOrder] inner join VacationList on [IDVacationList] = [VacationListID] " +
                        "where [IDVacationOrder] like " + ddlOrder.SelectedValue.ToString() + "";
                    DBConnection.connection.Open();
                    string startDate = command.ExecuteScalar().ToString();
                    command.ExecuteNonQuery();
                    DBConnection.connection.Close();
                    //Дата окончания
                    DateTime theDate1 = DateTime.ParseExact(tbEndDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    string endDate = theDate1.ToString("dd.MM.yyyy");
                    DateTime dateNow = DateTime.UtcNow.Date;
                    procedures.ExtensionOrder_Update(DBConnection.idRecord, dateNow.ToString(), tbReason.Text.ToString(), startDate, endDate, Convert.ToInt32(ddlOrder.SelectedValue.ToString()));
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Запись успешно добавлен.')", true);
                    gvFill(QR);
                    Cleaner();
                    ddlOrderFill();
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось обновить запись запись :(')", true);
            }
        }
        //Отменить выбор
        protected void btCanselSelected_Click(object sender, EventArgs e)
        {
            Cleaner();
            btUpdate.Visible = false;
            DBConnection.idRecord = 0;
            SelectedMessage.Visible = false;
            gvFill(QR);
        }
        //Поиск
        protected void btSearch_Click(object sender, EventArgs e)
        {
            if (tbSearch.Text != "")
            {
                foreach (GridViewRow row in gvExtension.Rows)
                {
                    if (row.Cells[1].Text.Equals(tbSearch.Text) ||
                    row.Cells[3].Text.Equals(tbSearch.Text) ||
                    row.Cells[4].Text.Equals(tbSearch.Text) ||
                    row.Cells[5].Text.Equals(tbSearch.Text) ||
                    row.Cells[6].Text.Equals(tbSearch.Text) ||
                    row.Cells[7].Text.Equals(tbSearch.Text) ||
                    row.Cells[8].Text.Equals(tbSearch.Text))
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
                string newQR = QR + "where [IDExtensionOrder] like '%" + tbSearch.Text + "%' or [OrderNumber] like '%" + tbSearch.Text + "%' or [ExtensionOrderNumber] like '%" + tbSearch.Text + "%' or [ExtensionDate] like '%" + tbSearch.Text + "%' or" +
                    "[ExtensionReason] like '%" + tbSearch.Text + "%' or [ExtensionStartDate] like '%" + tbSearch.Text + "%' or [ExtensionEndDate] like '%" + tbSearch.Text + "%'";
                gvFill(newQR);
                btCancel.Visible = true;
            }
        }
        //Очистка поиска
        protected void btCancel_Click(object sender, EventArgs e)
        {
            tbSearch.Text = string.Empty;
            btCancel.Visible = false;
            gvFill(QR);
            ddlOrderFill();
        }
    }
}