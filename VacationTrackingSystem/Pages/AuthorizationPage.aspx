<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthorizationPage.aspx.cs" Inherits="VacationTrackingSystem.Pages.AuthorizationPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="icon" href="../Content/img/icon.png" type="image/x-icon" />
    <link rel="stylesheet" href="../../Content/bootstrap.min.css" />
    <link rel="stylesheet" href="../../Content/UserStyles.css" />
    <title>Авторизация | Учёт отпусков</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-form">
            <h2>Авторизация</h2>
            <asp:Label ID="lblAuthError" runat="server" Text="Пожалуйста, введите корректный логин и пароль" style="color:#FF0000" Visible="false"></asp:Label>
            <br />
            <div class="form-group">
                <label for="tbPassword">Логин</label>
                <asp:TextBox ID="tbLogin" runat="server" class="form-control" aria-describedby="emailHelp" placeholder="Введите логин"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Введите логин" ControlToValidate="tbLogin" CssClass="Error" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group">
                <label for="tbPassword">Пароль</label>
                <asp:TextBox ID="tbPassword" runat="server" type="password" class="form-control" placeholder="Введите пароль"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Введите пароль" ControlToValidate="tbPassword" CssClass="Error" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <asp:Button ID="btLogin" runat="server" Text="Войти" OnClick="btLogin_Click" CssClass="btn btn-primary" />
        </div>
    </form>
</body>
</html>
