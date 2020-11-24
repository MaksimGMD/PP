<%@ Page Title="Обратная связь | Учёт отпусков" Language="C#" MasterPageFile="~/Pages/Users/UsersPanel.Master" AutoEventWireup="true" CodeBehind="FeedbackPage.aspx.cs" Inherits="VacationTrackingSystem.Pages.Users.FeedbackPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="alert alert-success" role="alert" id="alert" runat="server" visible="false">
        Ваше сообщение успешно отправленно!
    </div>
    <div class="row justify-content-between">
        <div class="col-lg-6 mb-2">
            <div class="feedback-form">
                <h3>Оставить сообщение</h3>
                <div class="form-group">
                    <label for="tbPhone">Тема сообщения</label>
                    <asp:DropDownList ID="ddlTittle" runat="server" CssClass="form-control">
                        <asp:ListItem>Идея</asp:ListItem>
                        <asp:ListItem>Проблема</asp:ListItem>
                        <asp:ListItem>Вопрос</asp:ListItem>
                        <asp:ListItem>Благодраность</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="tbMessage">Сообщение</label>
                    <asp:TextBox ID="tbMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="Введите сообщение"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="Error" ErrorMessage="Введите сообщение" Display="Dynamic" ControlToValidate="tbMessage"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <label for="tbName">Имя</label>
                    <asp:TextBox ID="tbName" CssClass="form-control" runat="server" placeholder="Введите имя"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="Error" ErrorMessage="Введите имя" Display="Dynamic" ControlToValidate="tbName"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <label for="tbmail">Адрес почты</label>
                    <asp:TextBox ID="tbmail" runat="server" CssClass="form-control" placeholder="Введите адрес электронной почты" type="email" TextMode="Email"></asp:TextBox>
                </div>
                <asp:Button ID="btSubmit" runat="server" Text="Отправить" CssClass="btn btn-primary" OnClick="btSubmit_Click" />
                <hr />
            </div>
        </div>
        <div class="col-lg-5">
            <div class="contactInf">
                <h4>Контактная информация</h4>
                <hr />
                <h5><i class="fa fa-phone" aria-hidden="true"></i> Телефоны</h5>
                <ul>
                    <li class="mb-1">Техническая поддержка <span class="inf">8 800 000-00-00</span></li>
                    <li>Отдел кадров <span class="inf">+7 495 999-99-99</span></li>
                </ul>
                <hr />
                <h5><i class="fa fa-envelope-o" aria-hidden="true"></i> Электронная почта</h5>
                <ul>
                    <li><span class="inf">i_m.d.gritsuk@mpt.ru</span></li>
                </ul>
                <hr />
                <h5><i class="fa fa-map-marker" aria-hidden="true"></i> Адрес</h5>
                <ul>
                    <li>Основное здание <span class="inf">129075, Москва, Мурманский проезд, д. 14, к. 1</span></li>
                </ul>
                <hr />
            </div>
        </div>
    </div>
</asp:Content>
