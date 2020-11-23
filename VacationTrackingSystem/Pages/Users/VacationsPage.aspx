<%@ Page Title="Отпуска | Учёт отпусков" Language="C#" MasterPageFile="~/Pages/Users/UsersPanel.Master" AutoEventWireup="true" CodeBehind="VacationsPage.aspx.cs" Inherits="VacationTrackingSystem.Pages.Users.VacationsPage" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:SqlDataSource ID="sdsType" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsVacations" runat="server"></asp:SqlDataSource>
    <div class="alert alert-danger" role="alert" id="alert" runat="server" visible="false">
        <ul style="margin: 0px">
            <li runat="server" id="liDaysAmountError" visible="false">Выбрано больше дней, чем доступно для отпуска.</li>
            <li runat="server" id="liIntervalError" visible="false">Добавить отпуск можно минимимум за 2 недели до его начала.</li>
            <li runat="server" id="liCorrectDate" visible="false">Дата начала отпуска, не может быть больше даты окончания.</li>
        </ul>
    </div>
    <div class="forms">
        <h3>Отпуск</h3>
        <div class="row justify-content-end mr-1">
            <span>Доступно дней:
                <asp:Label ID="lbldaysAvailable" runat="server" Style="font-weight: 500; font-size: 18px;"></asp:Label></span>
        </div>
        <hr>
        <div class="row">
            <div class="col-lg-6">
                <div class="form-group">
                    <label for="tbStartDate">Начало отпуска</label>
                    <asp:TextBox ID="tbStartDate" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="Error" ErrorMessage="Введите дату" Display="Dynamic" ControlToValidate="tbStartDate"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="form-group">
                    <label for="tbEndDate">Конец отпуска</label>
                    <asp:TextBox ID="tbEndDate" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="Error" ErrorMessage="Введите дату" Display="Dynamic" ControlToValidate="tbEndDate"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <div class="form-group">
                    <label for="ddlType">Тип отпуска</label>
                    <asp:DropDownList ID="ddlType" runat="server" class="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row mt-2">
            <div class="col">
                <button runat="server" id="btInsert" class="btn btn-success" title="Поиск" onserverclick="btInsert_Click">
                    Добавить <i class="fa fa-plus"></i>
                </button>
                <asp:Button ID="btUpdate" runat="server" Text="Изменить" CssClass="btn btn-primary" ToolTip="Обновить" OnClick="btUpdate_Click" Visible="false" />
                <asp:Button ID="btDelete" runat="server" Text="Удалить" CssClass="btn btn-danger" ToolTip="Удалить выбранную запись" Visible="false" CausesValidation="false" OnClick="btDelete_Click" />
            </div>
        </div>
    </div>
    <div class="row justify-content-end mt-1" style="margin-right: 0px">
        <asp:Button ID="btCancel" runat="server" Text="Отменить выбор" Style="margin-top: 5px;" CssClass="btn btn-primary btn-sm mx-1" ToolTip="Отменить выбор записи" Visible="false" CausesValidation="false" OnClick="btCancel_Click" />
    </div>
    <div class="table mt-2" style="overflow-x: auto; width: 100%">
        <asp:GridView ID="gvVacation" runat="server" AllowSorting="true"
            CssClass="table table-condensed table-hover" UseAccessibleHeader="true" CurrentSortDirection="ASC" Font-Size="14px"
            AlternatingRowStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvVacation_RowDataBound" OnSelectedIndexChanged="gvVacation_SelectedIndexChanged" OnSorting="gvVacation_Sorting">
        </asp:GridView>
    </div>
</asp:Content>
