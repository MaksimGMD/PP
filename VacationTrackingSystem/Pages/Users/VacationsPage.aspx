<%@ Page Title="Отпуска | Учёт отпусков" Language="C#" MasterPageFile="~/Pages/Users/UsersPanel.Master" AutoEventWireup="true" CodeBehind="VacationsPage.aspx.cs" Inherits="VacationTrackingSystem.Pages.Users.VacationsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-lg-12">
            <div class="form-group">
                <div class="alert alert-success alert-dismissible fade show" role="alert">
                    Отпуск успешно добавлен.
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="alert alert-danger alert-dismissible fade show" role="alert">
                    Не удалось добавить отпуск.
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
            </div>
        </div>
    </div>
    <div class="forms">
        <h3>Отпуска</h3>
        <hr>
        <div class="row">
            <div class="col-lg-6">
                <div class="form-group">
                    <label for="tbStartDate">Начало отпуска</label>
                    <asp:TextBox ID="tbStartDate" runat="server" type="password" class="form-control" TextMode="Date"></asp:TextBox>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="form-group">
                    <label for="tbEndDate">Конец отпуска</label>
                    <asp:TextBox ID="tbEndDate" runat="server" type="password" class="form-control" TextMode="Date"></asp:TextBox>
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
                <button runat="server" id="btInsert" class="btn btn-primary" title="Поиск" causesvalidation="False">
                    Добавить <i class="fa fa-plus"></i>
                </button>
                <asp:Button ID="btUpdate" runat="server" Text="Обновить" CssClass="btn btn-primary" ToolTip="Обновить" />
            </div>
        </div>
    </div>
    <div class="table" style="overflow-x: auto; width: 100%">
            <asp:GridView ID="gvVacations" runat="server" AllowSorting="true"
                CssClass="table table-condensed table-hover" UseAccessibleHeader="true" CurrentSortDirection="ASC" Font-Size="14px" AlternatingRowStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                <Columns>
                    <asp:TemplateField>
                      
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
</asp:Content>
