<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DangNhap.aspx.cs" Inherits="WebDocSachBTL.DangNhap" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box-panel" style="max-width: 420px; margin: 30px auto;">
        <h2 style="margin-bottom:20px; text-align:center;">Đăng nhập hệ thống</h2>
        <div class="form-group">
            <label>Tên đăng nhập</label>
            <asp:TextBox ID="txtUser" runat="server" CssClass="input-ctrl w-100" placeholder="VD: user1 hoặc admin"></asp:TextBox>
        </div>
        <div class="form-group">
            <label>Mật khẩu</label>
            <asp:TextBox ID="txtPass" runat="server" CssClass="input-ctrl w-100" TextMode="Password" placeholder="Mật khẩu mẫu: 123456"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:CheckBox ID="chkGhiNho" runat="server" Text=" Ghi nhớ đăng nhập (Cookie 7 ngày)" />
        </div>
        <asp:Button ID="btnLogin" runat="server" Text="Đăng nhập" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
        <p style="margin-top:15px; text-align:center; font-size:14px;">Chưa có tài khoản? <a href="DangKy.aspx" style="color:#ff6122; font-weight:600;">Đăng ký ngay</a></p>
        <asp:Label ID="lblThongBao" runat="server" ForeColor="Red" style="display:block; margin-top:10px; text-align:center;"></asp:Label>
    </div>
</asp:Content>
