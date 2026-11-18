<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DangKy.aspx.cs" Inherits="WebDocSachBTL.DangKy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function kiemTraDangKy() {
            var hoTen = document.getElementById('<%= txtHoTen.ClientID %>').value.trim();
            var user = document.getElementById('<%= txtUser.ClientID %>').value.trim();
            var pass = document.getElementById('<%= txtPass.ClientID %>').value;
            var rePass = document.getElementById('<%= txtRePass.ClientID %>').value;

            if (hoTen === "" || user === "" || pass === "") {
                alert("Vui lòng nhập đầy đủ Họ tên, Tên đăng nhập và Mật khẩu!");
                return false;
            }
            if (pass.length < 6) {
                alert("Mật khẩu phải có ít nhất 6 ký tự!");
                return false;
            }
            if (pass !== rePass) {
                alert("Mật khẩu nhập lại không khớp!");
                return false;
            }
            return true;
        }
    </script>

    <div class="box-panel" style="max-width: 450px; margin: 25px auto;">
        <h2 style="margin-bottom:20px; text-align:center;">Đăng ký thành viên mới</h2>
        
        <div class="form-group">
            <label>Họ và tên hiển thị</label>
            <asp:TextBox ID="txtHoTen" runat="server" CssClass="input-ctrl w-100" placeholder="VD: Phạm Minh Tuấn"></asp:TextBox>
        </div>
        <div class="form-group">
            <label>Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="input-ctrl w-100" placeholder="VD: tuan@gmail.com"></asp:TextBox>
        </div>
        <div class="form-group">
            <label>Tên đăng nhập</label>
            <asp:TextBox ID="txtUser" runat="server" CssClass="input-ctrl w-100" placeholder="Viết liền không dấu"></asp:TextBox>
        </div>
        <div class="form-group">
            <label>Mật khẩu</label>
            <asp:TextBox ID="txtPass" runat="server" CssClass="input-ctrl w-100" TextMode="Password" placeholder="Tối thiểu 6 ký tự"></asp:TextBox>
        </div>
        <div class="form-group">
            <label>Nhập lại mật khẩu</label>
            <asp:TextBox ID="txtRePass" runat="server" CssClass="input-ctrl w-100" TextMode="Password"></asp:TextBox>
        </div>

        <asp:Button ID="btnDangKy" runat="server" Text="Tạo tài khoản" CssClass="btn btn-primary w-100" 
            OnClientClick="return kiemTraDangKy();" OnClick="btnDangKy_Click" />
        
        <p style="margin-top:15px; text-align:center; font-size:14px;">
            Đã có tài khoản? <a href="DangNhap.aspx" style="color:#ff6122; font-weight:600;">Đăng nhập ngay</a>
        </p>
        <asp:Label ID="lblThongBao" runat="server" style="display:block; margin-top:10px; text-align:center; font-weight:600;"></asp:Label>
    </div>
</asp:Content>
