<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChiTietSach.aspx.cs" Inherits="WebDocSachBTL.ChiTietSach" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Thông báo chặn nếu truy cập trái phép sách Chỉ bạn bè -->
    <asp:Panel ID="pnlChanTruyCap" runat="server" Visible="false" CssClass="box-panel" style="text-align:center; padding:50px 20px;">
        <div style="font-size:48px; margin-bottom:10px;">🔒</div>
        <h2 style="color:#dc3545; margin-bottom:10px;">Tài liệu giới hạn quyền xem</h2>
        <p style="color:#555; margin-bottom:20px;">Tài liệu này được tác giả thiết lập chế độ <b>Chỉ bạn bè (Friend List)</b> mới có thể xem và đọc.</p>
        <a href="TrangChu.aspx" class="btn btn-primary">Quay lại Thư viện</a>
    </asp:Panel>

    <!-- Nội dung chi tiết sách -->
    <asp:Panel ID="pnlChiTiet" runat="server">
        <div class="box-panel" style="margin-bottom: 24px;">
            <div style="display:flex; flex-wrap:wrap; gap:30px;">
                <!-- Cột trái: Ảnh bìa -->
                <div style="width:240px; flex-shrink:0;">
                    <img id="imgBia" runat="server" style="width:100%; height:330px; object-fit:cover; border-radius:10px; box-shadow:0 6px 16px rgba(0,0,0,0.15);"
                         onerror="this.onerror=null;this.src='data:image/svg+xml;utf8,<svg xmlns=%22http://www.w3.org/2000/svg%22 width=%22240%22 height=%22330%22><rect width=%22100%25%22 height=%22100%25%22 fill=%22%232c3e50%22/><text x=%2250%25%22 y=%2250%25%22 font-family=%22Arial%22 font-size=%2218%22 fill=%22%23fff%22 text-anchor=%22middle%22>📘 BÌA SÁCH</text></svg>';" />
                </div>

                <!-- Cột phải: Thông tin & Nút đọc -->
                <div style="flex:1; min-width:280px; display:flex; flex-direction:column;">
                    <div style="margin-bottom:8px;">
                        <span style="background:#fff0e6; color:#ff6122; padding:4px 12px; border-radius:20px; font-size:13px; font-weight:700;">
                            <asp:Label ID="lblChuDe" runat="server"></asp:Label>
                        </span>
                        <span style="margin-left:10px; font-size:13px; color:#666;">
                            <asp:Label ID="lblQuyenXem" runat="server"></asp:Label>
                        </span>
                    </div>

                    <h1 style="font-size:28px; margin-bottom:10px; color:#111;"><asp:Label ID="lblTieuDe" runat="server"></asp:Label></h1>
                    
                    <div style="color:#555; font-size:14.5px; margin-bottom:16px; line-height:1.8;">
                        <div>✍️ <b>Tác giả:</b> <asp:Label ID="lblTacGia" runat="server"></asp:Label></div>
                        <div>👤 <b>Người đóng góp:</b> <asp:Label ID="lblNguoiDang" runat="server"></asp:Label></div>
                        <div>👁️ <b>Lượt đọc:</b> <asp:Label ID="lblLuotXem" runat="server"></asp:Label> &nbsp;|&nbsp; 📅 <b>Ngày đăng:</b> <asp:Label ID="lblNgayDang" runat="server"></asp:Label></div>
                    </div>

                    <div style="background:#f8f9fa; padding:15px; border-radius:8px; border-left:4px solid #ff6122; margin-bottom:20px; flex:1;">
                        <strong style="display:block; margin-bottom:6px; font-size:14px;">Giới thiệu nội dung:</strong>
                        <p style="color:#444; font-size:14.5px; line-height:1.6;"><asp:Label ID="lblMoTa" runat="server"></asp:Label></p>
                    </div>

                    <div>
                        <asp:HyperLink ID="lnkDocSach" runat="server" CssClass="btn btn-primary" style="padding:11px 26px; font-size:16px;">📖 Bắt đầu đọc trực tuyến</asp:HyperLink>
                        <a href="TrangChu.aspx" class="btn btn-light" style="padding:11px 20px; margin-left:10px;">&laquo; Quay lại</a>
                    </div>
                </div>
            </div>
        </div>

        <!-- Khu vực Bình luận & Thảo luận -->
        <div class="box-panel">
            <h3 style="margin-bottom:16px;">💬 Bình luận của độc giả</h3>
            
            <div style="background:#f8f9fa; padding:18px; border-radius:8px; margin-bottom:24px;">
                <asp:Panel ID="pnlTenKhachBL" runat="server" CssClass="form-group">
                    <label>Tên hiển thị của bạn (Chế độ Khách)</label>
                    <asp:TextBox ID="txtTenKhach" runat="server" CssClass="input-ctrl" style="max-width:300px;" placeholder="Nhập tên của bạn..."></asp:TextBox>
                </asp:Panel>

                <div class="form-group">
                    <label>Cảm nhận về tài liệu này</label>
                    <asp:TextBox ID="txtNoiDungBL" runat="server" TextMode="MultiLine" Rows="3" CssClass="input-ctrl w-100" placeholder="Chia sẻ suy nghĩ của bạn..."></asp:TextBox>
                </div>

                <asp:Button ID="btnGuiBL" runat="server" Text="Gửi bình luận" CssClass="btn btn-primary" OnClientClick="return kiemTraBL();" OnClick="btnGuiBL_Click" />
            </div>

            <asp:Repeater ID="rptBinhLuan" runat="server">
                <ItemTemplate>
                    <div style="padding:14px 0; border-bottom:1px solid #eee;">
                        <div style="display:flex; justify-content:space-between; margin-bottom:5px;">
                            <strong style="color:#222;">👤 <%# Eval("TenNguoiBinhLuan") %></strong>
                            <small style="color:#888;"><%# Eval("NgayBL", "{0:dd/MM/yyyy HH:mm}") %></small>
                        </div>
                        <p style="color:#444; font-size:14.5px; line-height:1.5;"><%# Eval("NoiDungBL") %></p>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Label ID="lblChuaCoBL" runat="server" Text="Chưa có bình luận nào. Hãy là người đầu tiên nhận xét!" ForeColor="#777" Visible="false"></asp:Label>
        </div>
    </asp:Panel>

    <script type="text/javascript">
        function kiemTraBL() {
    var nd = document.getElementById('<%= txtNoiDungBL.ClientID %>').value.trim();
    if (nd === "") {
        alert("Vui lòng nhập nội dung bình luận!");
        return false;
    }
    return true;
}
</script>
</asp:Content>
