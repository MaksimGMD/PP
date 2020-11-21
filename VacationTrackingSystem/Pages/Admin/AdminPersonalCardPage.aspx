<%@ Page Title="Личные дела | Учёт отпусков" Language="C#" MasterPageFile="~/Pages/Admin/AdminPanel.Master" AutoEventWireup="true" CodeBehind="AdminPersonalCardPage.aspx.cs" Inherits="VacationTrackingSystem.Pages.Admin.AdminPersonalCardPage" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:SqlDataSource ID="sdsUsers" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsPositions" runat="server"></asp:SqlDataSource>
    <script type="text/javascript">
        function isDelete() {
            return confirm("Вы уверенны, что хотите удалить выбранную запись?");
        }
    </script>
    <div class="container">
        <center>
        <h2>Личные дела</h2>
    </center>
        <div class="row">
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="tbSurname">Фамилия</label>
                    <asp:TextBox ID="tbSurname" runat="server" placeholder="Фамилия" class="form-control" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="Error" ErrorMessage="Введите фамилию" Display="Dynamic" ControlToValidate="tbSurname"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="tbName">Имя</label>
                    <asp:TextBox ID="tbName" runat="server" placeholder="Имя" class="form-control" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="Error" ErrorMessage="Введите имя" Display="Dynamic" ControlToValidate="tbName"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="tbMiddleName">Отчество</label>
                    <asp:TextBox ID="tbMiddleName" runat="server" placeholder="Отчество" class="form-control" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="Error" ErrorMessage="Введите отчество" Display="Dynamic" ControlToValidate="tbMiddleName"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="tbDaysAmount">Количество дней</label>
                    <asp:TextBox ID="tbDaysAmount" runat="server" placeholder="Количество дней" class="form-control" TextMode="Number" min="0"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Введите количество дней" ControlToValidate="tbDaysAmount" CssClass="Error" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="ddlPosition">Должность</label>
                    <asp:DropDownList ID="ddlPosition" runat="server" class="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="tbLogin">Логин</label>
                    <asp:TextBox ID="tbLogin" runat="server" placeholder="Логин" class="form-control" MaxLength="20"></asp:TextBox>
                    <asp:Label ID="lblLoginCheck" runat="server" CssClass="Error" Visible="false" Text="Пользователь с таким логином уже есть в базе" Display="Dynamic"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="Error" ErrorMessage="Введите логин" ControlToValidate="tbLogin" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" CssClass="Error" ErrorMessage="Слишком короткий логин"
                        ControlToValidate="tbLogin" Display="Dynamic" ValidationExpression="\S{8,20}"></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="form-group">
                    <label for="tbPassword">Пароль</label>
                    <asp:TextBox ID="tbPassword" runat="server" placeholder="Пароль" class="form-control" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" CssClass="Error" ErrorMessage="Введите пароль" ControlToValidate="tbPassword" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="Error"
                        ErrorMessage="Длина пароля должна быть больше 6 символов, содеражть минимум одну ланитскую букву, одну цифру и один из символов !@#$%^&*_" ControlToValidate="tbPassword" Display="Dynamic"
                        ValidationExpression="(?=.*[0-9])(?=.*[!@#$%^&*_])((?=.*[a-z])|(?=.*[A-Z]))[0-9a-zA-Z!@#$%^&*_]{6,}"></asp:RegularExpressionValidator>
                </div>
            </div>
        </div>
        <asp:Button ID="btInsert" runat="server" Text="Добавить" CssClass="btn btn-dark" ToolTip="Добавить новую запись" OnClick="btInsert_Click" />
        <asp:Button ID="btUpdate" runat="server" Text="Обновить" CssClass="btn btn-dark" ToolTip="Обновить запись" Style="margin-left: 1%" Visible="false" OnClick="btUpdate_Click" />
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
                        title="Фильтр" causesvalidation="False" style="height: 36px!important; margin: 0 0 0 1%; color: #e1f1ff;">
                        Фильтр
                    </button>
                    <asp:Button ID="btCancel" runat="server" Text="Отмена" CssClass="btn btn-dark"
                        ToolTip="Отменить поиск и фильтрацию" CausesValidation="False" Style="margin: 0 0 0 1%; color: #e1f1ff;" Visible="false" OnClick="btCancel_Click" />
                </div>
            </div>
        </div>
        <div id="SelectedMessage" class="form-inline" runat="server" visible="false" display="Dynamic">
            <button id="btCanselSelected" runat="server" CausesValidation="False" onserverclick="btCanselSelected_Click"
                style="background: none; padding: 0; margin-right: 5px; border: none">
                <i class="fa fa-times" title="Отменить выбор строки"></i>
            </button>
            <asp:Label ID="lblSelectedRow" runat="server" Style="font-weight: 500"></asp:Label>
        </div>
        <div class="table" style="overflow-x: auto; width: 100%">
           <asp:GridView ID="gvUsers" runat="server" AllowSorting="true"
                CssClass="table table-condensed table-hover" UseAccessibleHeader="true" CurrentSortDirection="ASC" Font-Size="14px" 
                AlternatingRowStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvUsers_RowDataBound" OnRowDeleting="gvUsers_RowDeleting" OnSelectedIndexChanged="gvUsers_SelectedIndexChanged" OnSorting="gvUsers_Sorting">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btDelete" runat="server" ImageUrl="~/Content/img/delete-icon.png" ControlStyle-Width="24px" CommandName="Delete" OnClientClick="return isDelete()" ToolTip="Удалить" CausesValidation="false"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
