using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VacationTrackingSystem.Pages.Admin
{
    public partial class VacationTypePage : System.Web.UI.Page
    {
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            QR = DBConnection.qrType;
            if (!IsPostBack)
            {
                gvFill(QR);
            }
        }

        private void gvFill(string qr)
        {
            sdsType.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsType.SelectCommand = qr;
            sdsType.DataSourceMode = SqlDataSourceMode.DataReader;
            gvType.DataSource = sdsType;
            gvType.DataBind();
        }
        //Очитска полей
        protected void Cleaner()
        {
            SelectedMessage.Visible = false;
            DBConnection.idRecord = 0;
            tbType.Text = "";
        }

        protected void gvType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvType, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }
        //Удаление записи
        protected void gvType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int Index = Convert.ToInt32(e.RowIndex);
                DataProcedures procedure = new DataProcedures();
                GridViewRow rows = gvType.SelectedRow;
                DBConnection.idRecord = Convert.ToInt32(gvType.Rows[Index].Cells[1].Text.ToString());
                procedure.VacationType_Delete(DBConnection.idRecord);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Запись успешно удалена.')", true);
                gvFill(QR);
                Cleaner();
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось удалить запись :(')", true);
            }
        }

        protected void gvType_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvType.Rows)
            {
                if (row.RowIndex == gvType.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            GridViewRow rows = gvType.SelectedRow;
            DBConnection.idRecord = Convert.ToInt32(rows.Cells[1].Text.ToString());
            tbType.Text = rows.Cells[2].Text.ToString();
            SelectedMessage.Visible = true;
            lblSelectedRow.Text = "Выбрана строка с ID " + DBConnection.idRecord;
            btUpdate.Visible = true;
        }

        protected void gvType_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string strField = string.Empty;
            switch (e.SortExpression)
            {
                case ("Название жанра"):
                    e.SortExpression = "[Genre_Name]";
                    break;
            }
            sortGridView(gvType, e, out sortDirection, out strField);
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

        protected void btInsert_Click(object sender, EventArgs e)
        {
            DataProcedures procedures = new DataProcedures();
            try
            {
                procedures.VacationType_Insert(tbType.Text);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Запись успешно добавлен.')", true);
                gvFill(QR);
                Cleaner();
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось добавить запись :(')", true);
            }
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (DBConnection.idRecord != 0)
                {
                    DataProcedures procedures = new DataProcedures();
                    procedures.VacationType_Update(DBConnection.idRecord, tbType.Text);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Запись успешно обновлена.')", true);
                    gvFill(QR);
                    Cleaner();
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
    }
}