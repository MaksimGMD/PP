<%@ Page Title="Типы отпуска | Учёт отпусков" Language="C#" MasterPageFile="~/Pages/Admin/AdminPanel.Master" AutoEventWireup="true" CodeBehind="VacationTypePage.aspx.cs" Inherits="VacationTrackingSystem.Pages.Admin.VacationTypePage" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:SqlDataSource ID="sdsType" runat="server"></asp:SqlDataSource>
    <div class="container">
        <center>
            <h2>Типы отпусков</h2>
        </center>
        <div class="row">
            <div class="col-lg-6 mt-2">
                <div class="form-group">
                    <label for="tbType">Тип отпуска</label>
                    <asp:TextBox ID="tbType" runat="server" placeholder="Тип отпуска" class="form-control" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="Error" ErrorMessage="Введите тип отпуска" Display="Dynamic" ControlToValidate="tbType"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <asp:Button ID="btInsert" runat="server" Text="Добавить" CssClass="btn btn-dark" ToolTip="Добавить новую запись" OnClick="btInsert_Click" />
        <asp:Button ID="btUpdate" runat="server" Text="Обновить" CssClass="btn btn-dark" ToolTip="Обновить запись" Style="margin-left: 1%" Visible="false" OnClick="btUpdate_Click" />
    </div>
    <div class="grid-system mt-5">
        <div id="SelectedMessage" class="form-inline mt-2" runat="server" visible="false" display="Dynamic">
            <button id="btCanselSelected" runat="server" onserverclick="btCanselSelected_Click"
                style="background: none; padding: 0; margin-right: 5px; border: none">
                <i class="fa fa-times" title="Отменить выбор строки"></i>
            </button>
            <asp:Label ID="lblSelectedRow" runat="server" Style="font-weight: 500"></asp:Label>
        </div>
        <div class="table" style="overflow-x: auto; width: 100%; text-align: center">
            <asp:GridView ID="gvType" runat="server" AllowSorting="true"
                CssClass="table table-condensed table-hover" UseAccessibleHeader="true" CurrentSortDirection="ASC" Font-Size="14px"
                AlternatingRowStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvType_RowDataBound" OnRowDeleting="gvType_RowDeleting" OnSelectedIndexChanged="gvType_SelectedIndexChanged" OnSorting="gvType_Sorting">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btDelete" runat="server" ImageUrl="~/Content/img/delete-icon.png" CausesValidation="False" ControlStyle-Width="24px" CommandName="Delete" OnClientClick="return isDelete()" ToolTip="Удалить" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
