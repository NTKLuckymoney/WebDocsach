<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DangTaiSach.aspx.cs" Inherits="WebDocSachBTL.DangTaiSach" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box-panel" style="max-width: 760px; margin: 10px auto;">
        <h2 style="margin-bottom: 6px;">✍️ Đóng góp sách & Tài liệu mới</h2>
        <p style="color: #666; font-size: 14px; margin-bottom: 22px;">
            Chia sẻ tác phẩm sáng tác, tài liệu học tập hoặc tạp chí chuyên ngành tới cộng đồng.
        </p>

        <asp:Label ID="lblThongBao" runat="server" style="display:block; margin-bottom:16px; font-weight:600;"></asp:Label>

        <div style="display:grid; grid-template-columns: 1fr 1fr; gap: 16px;">
            <div class="form-group">
                <label>Tiêu đề sách / tài liệu (*)</label>
                <asp:TextBox ID="txtTieuDe" runat="server" CssClass="input-ctrl w-100" placeholder="VD: Lập trình Web ASP.NET cơ bản"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Tác giả (*)</label>
                <asp:TextBox ID="txtTacGia" runat="server" CssClass="input-ctrl w-100" placeholder="Tên tác giả hoặc bút danh"></asp:TextBox>
            </div>
        </div>

        <div style="display:grid; grid-template-columns: 1fr 1fr; gap: 16px;">
            <div class="form-group">
                <label>Thể loại / Chủ đề</label>
                <asp:DropDownList ID="ddlChuDe" runat="server" CssClass="input-ctrl w-100"></asp:DropDownList>
            </div>
            <div class="form-group">
                <label>Quyền xem tài liệu</label>
                <asp:DropDownList ID="ddlQuyenXem" runat="server" CssClass="input-ctrl w-100">
                    <asp:ListItem Value="0">🌐 Công khai (Tất cả mọi người)</asp:ListItem>
                    <asp:ListItem Value="1">👥 Chỉ bạn bè (Friend List)</asp:ListItem>
                </asp:DropDownList>
                <small id="lblGhiChuQuyen" runat="server" style="color:#888; font-size:12px;">
                    Đăng nhập để sử dụng chế độ chia sẻ riêng cho bạn bè.
                </small>
            </div>
        </div>

        <div style="display:grid; grid-template-columns: 1fr 1fr; gap: 16px;">
            <div class="form-group">
                <label>Ảnh bìa sách (.jpg, .png - tùy chọn)</label>
                <asp:FileUpload ID="fuAnhBia" runat="server" CssClass="input-ctrl w-100" accept=".jpg,.jpeg,.png,.webp" />
            </div>
            <div class="form-group">
                <label>Tải lên file nội dung (.txt - tùy chọn)</label>
                <asp:FileUpload ID="fuFileNoiDung" runat="server" CssClass="input-ctrl w-100" accept=".txt" />
            </div>
        </div>

        <div class="form-group">
            <label>Mô tả ngắn gọn (*)</label>
            <asp:TextBox ID="txtMoTa" runat="server" TextMode="MultiLine" Rows="2" CssClass="input-ctrl w-100" placeholder="Tóm tắt nội dung chính hiển thị ngoài thẻ sách..."></asp:TextBox>
        </div>

        <div class="form-group">
            <label>Nội dung chi tiết của sách (Có thể gõ trực tiếp hoặc tải file .txt ở trên)</label>
            <asp:TextBox ID="txtNoiDung" runat="server" TextMode="MultiLine" Rows="8" CssClass="input-ctrl w-100" placeholder="Nhập nội dung các chương sách tại đây..."></asp:TextBox>
        </div>

        <div style="margin-top: 20px;">
            <asp:Button ID="btnDangSach" runat="server" Text="📤 Gửi tài liệu lên hệ thống" CssClass="btn btn-primary"
                OnClientClick="return kiemTraDangSach();" OnClick="btnDangSach_Click" />
            <a href="TrangChu.aspx" class="btn btn-light" style="margin-left: 8px;">Hủy bỏ</a>
        </div>
    </div>

    <script type="text/javascript">
function kiemTraDangSach() {
    var tieuDe = document.getElementById('<%= txtTieuDe.ClientID %>').value.trim();
            var tacGia = document.getElementById('<%= txtTacGia.ClientID %>').value.trim();
            var moTa = document.getElementById('<%= txtMoTa.ClientID %>').value.trim();

            if (tieuDe === "" || tacGia === "" || moTa === "") {
                alert("Vui lòng nhập đầy đủ Tiêu đề, Tác giả và Mô tả ngắn!");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
