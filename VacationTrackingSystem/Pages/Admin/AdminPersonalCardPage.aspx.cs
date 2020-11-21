using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Crypt_Library;

namespace VacationTrackingSystem.Pages.Admin
{
    public partial class AdminPersonalCardPage : System.Web.UI.Page
    {
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            QR = DBConnection.qrPersonalCard;
            if (!IsPostBack)
            {
                gvFill(QR);
                ddlPositionFill();
            }
        }

        private void gvFill(string qr)
        {
            sdsUsers.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsUsers.SelectCommand = qr;
            sdsUsers.DataSourceMode = SqlDataSourceMode.DataReader;
            gvUsers.DataSource = sdsUsers;
            gvUsers.DataBind();
        }
        //Заполнение списка должностей
        private void ddlPositionFill()
        {
            sdsPositions.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsPositions.SelectCommand = DBConnection.qrPositions;
            sdsPositions.DataSourceMode = SqlDataSourceMode.DataReader;
            ddlPosition.DataSource = sdsPositions;
            ddlPosition.DataTextField = "Должность";
            ddlPosition.DataValueField = "IDPosition";
            ddlPosition.DataBind();
        }
        //Очитска полей
        protected void Cleaner()
        {
            SelectedMessage.Visible = false;
            DBConnection.idRecord = 0;
            ddlPosition.SelectedIndex = 0;
            tbDaysAmount.Text = string.Empty;
            tbLogin.Text = string.Empty;
            tbMiddleName.Text = string.Empty;
            tbName.Text = string.Empty;
            tbPassword.Text = string.Empty;
            tbSurname.Text = string.Empty;
        }
        //Сортировка записей в таблице
        protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string strField = string.Empty;
            switch (e.SortExpression)
            {
                case ("Фамилия"):
                    e.SortExpression = "[Surname]";
                    break;
                case ("Имя"):
                    e.SortExpression = "[Name]";
                    break;
                case ("Отчество"):
                    e.SortExpression = "[MiddleName]";
                    break;
                case ("Количество дней"):
                    e.SortExpression = "[DaysAmount]";
                    break;
                case ("Должность"):
                    e.SortExpression = "[PositionName]";
                    break;
                case ("Логин"):
                    e.SortExpression = "[Login]";
                    break;
            }
            sortGridView(gvUsers, e, out sortDirection, out strField);
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

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[9].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvUsers, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }

        protected void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvUsers.Rows)
            {
                if (row.RowIndex == gvUsers.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            int selectedRow = gvUsers.SelectedRow.RowIndex;
            GridViewRow rows = gvUsers.SelectedRow;
            DBConnection.idRecord = Convert.ToInt32(rows.Cells[1].Text.ToString());
            tbSurname.Text = rows.Cells[2].Text.ToString();
            tbName.Text = rows.Cells[3].Text.ToString();
            tbMiddleName.Text = rows.Cells[4].Text.ToString();
            tbDaysAmount.Text = rows.Cells[5].Text.ToString();
            ddlPosition.SelectedValue = rows.Cells[6].Text.ToString();
            tbLogin.Text = rows.Cells[8].Text.ToString();
            tbPassword.Text = Crypt.Decrypt(rows.Cells[9].Text.ToString());
            SelectedMessage.Visible = true;
            lblSelectedRow.Text = "Выбрана строка с ID " + DBConnection.idRecord;
            btUpdate.Visible = true;
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int Index = Convert.ToInt32(e.RowIndex);
                DataProcedures procedure = new DataProcedures();
                GridViewRow rows = gvUsers.SelectedRow;
                DBConnection.idRecord = Convert.ToInt32(gvUsers.Rows[Index].Cells[1].Text.ToString());
                procedure.PersonalCard_Delete(DBConnection.idRecord);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Запись успешно удалена.')", true);
                gvFill(QR);
                Cleaner();
                ddlPositionFill();
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось удалить запись :(')", true);
            }
        }
        //Добавление записи
        protected void btInsert_Click(object sender, EventArgs e)
        {
            DataProcedures procedures = new DataProcedures();
            DBConnection connection = new DBConnection();
            try
            {
                if (connection.LoginCheck(tbLogin.Text) > 0)
                {
                    lblLoginCheck.Visible = true;
                }
                else
                {
                    lblLoginCheck.Visible = false;
                    procedures.PersonalCard_Insert(tbSurname.Text, tbName.Text, tbMiddleName.Text, Convert.ToInt32(tbDaysAmount.Text), tbLogin.Text,
                    tbPassword.Text, Convert.ToInt32(ddlPosition.SelectedValue.ToString()));
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Запись успешно добавлен.')", true);
                    gvFill(QR);
                    Cleaner();
                    ddlPositionFill();
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось добавить запись :(')", true);
            }
        }
        //Обновление записи
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (DBConnection.idRecord != 0)
                {
                    DataProcedures procedures = new DataProcedures();
                    procedures.PersonalCard_Update(DBConnection.idRecord, tbSurname.Text, tbName.Text, tbMiddleName.Text, Convert.ToInt32(tbDaysAmount.Text), tbLogin.Text,
                    tbPassword.Text, Convert.ToInt32(ddlPosition.SelectedValue.ToString()));
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Запись успешно обновлена.')", true);
                    gvFill(QR);
                    Cleaner();
                    ddlPositionFill();
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось обновить запись запись :(')", true);
            }
        }
        //Очистка поиска
        protected void btCancel_Click(object sender, EventArgs e)
        {
            tbSearch.Text = string.Empty;
            btCancel.Visible = false;
            gvFill(QR);
            ddlPositionFill();
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
                foreach (GridViewRow row in gvUsers.Rows)
                {
                        if(row.Cells[2].Text.Equals(tbSearch.Text) ||
                        row.Cells[3].Text.Equals(tbSearch.Text) ||
                        row.Cells[4].Text.Equals(tbSearch.Text) ||
                        row.Cells[5].Text.Equals(tbSearch.Text) ||
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
                string newQR = QR + "where [Surname] like '%" + tbSearch.Text + "%' or [Name] like '%" + tbSearch.Text + "%' or [MiddleName] like '%" + tbSearch.Text + "%' or" +
                    "[DaysAmount] like '%" + tbSearch.Text + "%' or [PositionName] like '%" + tbSearch.Text + "%' or [Login] like '%" + tbSearch.Text + "%'";
                gvFill(newQR);
                btCancel.Visible = true;
            }
        }
    }
}