using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;

namespace VacationTrackingSystem.Pages.Admin
{
    public partial class RecallVacationPage : System.Web.UI.Page
    {
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            QR = DBConnection.qrRecallOrder;
            if (!IsPostBack)
            {
                gvFill(QR);
                ddlOrderFill();
            }
        }
        private void gvFill(string qr)
        {
            sdsRecall.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsRecall.SelectCommand = qr;
            sdsRecall.DataSourceMode = SqlDataSourceMode.DataReader;
            gvRecall.DataSource = sdsRecall;
            gvRecall.DataBind();
        }
        //Очитска полей
        protected void Cleaner()
        {
            SelectedMessage.Visible = false;
            DBConnection.idRecord = 0;
            ddlOrder.SelectedIndex = 0;
            tbReason.Text = string.Empty;
            tbRecallDate.Text = string.Empty;
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
        //Добавление
        protected void btInsert_Click(object sender, EventArgs e)
        {
            DataProcedures procedures = new DataProcedures();
            try
            {
                //Дата отзыва
                DateTime theDate1 = DateTime.ParseExact(tbRecallDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                string recallDate = theDate1.ToString("dd.MM.yyyy");
                //Дата окончания
                DateTime dateNow = DateTime.UtcNow.Date;
                procedures.RecallOrder_Insert(dateNow.ToString(), tbReason.Text.ToString(), recallDate, Convert.ToInt32(ddlOrder.SelectedValue.ToString()));
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
                    //Дата отзыва
                    DateTime theDate1 = DateTime.ParseExact(tbRecallDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    string recallDate = theDate1.ToString("dd.MM.yyyy");
                    procedures.RecallOrder_Update(DBConnection.idRecord, tbReason.Text.ToString(), recallDate, Convert.ToInt32(ddlOrder.SelectedValue.ToString()));
                    Response.Redirect(Request.Url.AbsoluteUri);
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
        //Удаление
        protected void gvRecall_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                if (DBConnection.idRecord != 0)
                {
                    SqlCommand command = new SqlCommand("", DBConnection.connection);
                    int Index = Convert.ToInt32(e.RowIndex);
                    DataProcedures procedure = new DataProcedures();
                    GridViewRow rows = gvRecall.SelectedRow;
                    DBConnection.idRecord = Convert.ToInt32(gvRecall.Rows[Index].Cells[1].Text.ToString());
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "delete from [RecallOrder] where [IDRecallOrder] = '" + DBConnection.idRecord + "'";
                    DBConnection.connection.Open();
                    command.ExecuteNonQuery();
                    DBConnection.connection.Close();
                    Response.Redirect(Request.Url.AbsoluteUri);
                    gvFill(QR);
                    Cleaner();
                    ddlOrderFill();
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось удалить запись :(')", true);
            }
        }

        protected void gvRecall_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[2].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvRecall, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }

        protected void gvRecall_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvRecall.Rows)
            {
                if (row.RowIndex == gvRecall.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            int selectedRow = gvRecall.SelectedRow.RowIndex;
            GridViewRow rows = gvRecall.SelectedRow;
            DBConnection.idRecord = Convert.ToInt32(rows.Cells[1].Text.ToString());
            tbRecallDate.Text = Convert.ToDateTime(rows.Cells[7].Text.ToString()).ToString("yyyy-MM-dd");
            tbReason.Text = rows.Cells[6].Text.ToString();
            ddlOrder.SelectedValue = rows.Cells[2].Text.ToString();
            SelectedMessage.Visible = true;
            lblSelectedRow.Text = "Выбрана строка с ID " + DBConnection.idRecord;
            btUpdate.Visible = true;
        }

        protected void gvRecall_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string strField = string.Empty;
            switch (e.SortExpression)
            {
                case ("ID"):
                    e.SortExpression = "[IDRecallOrder]";
                    break;
                case ("Номер отпуска"):
                    e.SortExpression = "[OrderNumber]";
                    break;
                case ("Номер приказа"):
                    e.SortExpression = "[RecallOrderNumber]";
                    break;
                case ("Дата приказа"):
                    e.SortExpression = "[RecallOrderDate]";
                    break;
                case ("Причина отзыва"):
                    e.SortExpression = "[RecallReason]";
                    break;
                case ("Дата отзыва"):
                    e.SortExpression = "[RecallDate]";
                    break;
            }
            sortGridView(gvRecall, e, out sortDirection, out strField);
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
                foreach (GridViewRow row in gvRecall.Rows)
                {
                    if (row.Cells[1].Text.Equals(tbSearch.Text) ||
                    row.Cells[3].Text.Equals(tbSearch.Text) ||
                    row.Cells[4].Text.Equals(tbSearch.Text) ||
                    row.Cells[5].Text.Equals(tbSearch.Text) ||
                    row.Cells[6].Text.Equals(tbSearch.Text) ||
                    row.Cells[7].Text.Equals(tbSearch.Text))
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
                string newQR = QR + "where [IDRecallOrder] like '%" + tbSearch.Text + "%' or [OrderNumber] like '%" + tbSearch.Text + "%' or [RecallOrderNumber] like '%" + tbSearch.Text + "%' or [RecallOrderDate] like '%" + tbSearch.Text + "%' or" +
                    "[RecallReason] like '%" + tbSearch.Text + "%' or [RecallDate] like '%" + tbSearch.Text + "%'";
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