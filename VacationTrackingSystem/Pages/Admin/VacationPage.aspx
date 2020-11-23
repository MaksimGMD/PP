<%@ Page Title="График отпусков | Учёт отпусков" Language="C#" MasterPageFile="~/Pages/Admin/AdminPanel.Master" AutoEventWireup="true" CodeBehind="VacationPage.aspx.cs" Inherits="VacationTrackingSystem.Pages.Admin.VacationPage" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function isDelete() {
            return confirm("Вы уверенны, что хотите удалить выбранную запись?");
        }
    </script>
    <div class="container">
        <asp:SqlDataSource runat="server" ID="sdsVacationOrder"></asp:SqlDataSource>
        <center>
        <h2>График отпусков</h2>
    </center>
        <div class="row">
            <div class="col-lg-8">
                <div class="form-group">
                    <label for="tbEmployee">Сотрудник</label>
                    <asp:TextBox ID="tbEmployee" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="tbPosition">Должность</label>
                    <asp:TextBox ID="tbPosition" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-8">
                <div class="form-group">
                    <label for="tbTypeVacation">Отпуск</label>
                    <asp:TextBox ID="tbTypeVacation" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="tbStatus">Статус отпуска</label>
                    <asp:TextBox ID="tbStatus" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="tbStartDate">Дата начала отпуска</label>
                    <asp:TextBox ID="tbStartDate" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="tbEndDate">Дата окончания отпуска</label>
                    <asp:TextBox ID="tbEndDate" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="tbAplicationDate">Дата заявления</label>
                    <asp:TextBox ID="tbAplicationDate" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="tbOrderDate">Дата приказа</label>
                    <asp:TextBox ID="tbOrderDate" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="tbOrderNumber">Номер приказа</label>
                    <asp:TextBox ID="tbOrderNumber" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <button id="btConfirm" runat="server" class="btn btn-dark" tooltip="Согласовать отпуск" onserverclick="btConfirm_Click" type="button" causesvalidation="false" style="width: auto; margin-left: 15px;" visible="false">Согласовать</button>
            <button id="btReject" runat="server" type="button" class="btn btn-dark" onserverclick="btReject_Click" causesvalidation="false" style="margin-left: 1%; width: auto" visible="false">Отклонить</button>
        </div>
    </div>
    <hr />
    <div class="grid-system mt-4">
        <div class="row mb-2">
            <div class="col">
                <div class="form-inline">
                    <asp:TextBox ID="tbSearch" CssClass="form-control" runat="server" placeholder="Поиск" type="text" TextMode="Search" Width="200px"></asp:TextBox>
                    <button runat="server" id="btSearch" class="btn-search" title="Поиск" causesvalidation="False" onserverclick="btSearch_Click">
                        <i class="fa fa-search"></i>
                    </button>
                    <button runat="server" id="btFilter" class="btn btn-dark" onserverclick="btFilter_Click"
                        title="Фильтр" causesvalidation="false" style="height: 36px!important; margin: 0 0 0 1%; color: #e1f1ff;">
                        Фильтр</button>
                    <asp:Button ID="btCancel" runat="server" Text="Отмена" CssClass="btn btn-dark"
                        ToolTip="Отменить поиск и фильтрацию" CausesValidation="False" Style="margin: 0 0 0 1%; color: #e1f1ff;" Visible="false" OnClick="btCancel_Click" />
                </div>
            </div>
        </div>
        <div id="SelectedMessage" class="form-inline" runat="server" visible="false" display="Dynamic">
            <button id="btCanselSelected" runat="server" causesvalidation="False" onserverclick="btCanselSelected_Click"
                style="background: none; padding: 0; margin-right: 5px; border: none">
                <i class="fa fa-times" title="Отменить выбор строки"></i>
            </button>
            <asp:Label ID="lblSelectedRow" runat="server" Style="font-weight: 500"></asp:Label>
        </div>
        <div class="table" style="overflow-x: auto; width: 100%">
            <asp:GridView ID="gvVacation" runat="server" AllowSorting="true"
                CssClass="table table-condensed table-hover" UseAccessibleHeader="true" CurrentSortDirection="ASC" Font-Size="14px"
                AlternatingRowStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvVacation_RowDataBound" OnSelectedIndexChanged="gvVacation_SelectedIndexChanged" OnSorting="gvVacation_Sorting">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btDelete" runat="server" ImageUrl="~/Content/img/delete-icon.png" ControlStyle-Width="24px" CommandName="Delete" OnClientClick="return isDelete()" ToolTip="Удалить" CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
