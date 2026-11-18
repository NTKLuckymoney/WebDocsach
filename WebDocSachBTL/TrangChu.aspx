<%@ Page Title="Trang chủ" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrangChu.aspx.cs" Inherits="WebDocSachBTL.TrangChu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="hero-banner">
        <h1>Khám phá kho tri thức & sáng tác cộng đồng</h1>
        <p>Đọc hàng ngàn đầu sách, tạp chí chuyên ngành và chia sẻ tác phẩm của riêng bạn tới danh sách bạn bè.</p>
        <a href="DangTaiSach.aspx" class="btn" style="background:#fff; color:#ff6122;">✍️ Bắt đầu viết & đăng sách</a>
    </div>

    <div class="section-bar">
        <h2>📖 Thư viện tài liệu</h2>
        <div>
            <span style="font-size:14px; color:#555; margin-right:8px;">Thể loại:</span>
            <asp:DropDownList ID="ddlChuDe" runat="server" CssClass="input-ctrl" AutoPostBack="true" OnSelectedIndexChanged="ddlChuDe_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>

    <div class="book-grid">
        <asp:Repeater ID="rptSach" runat="server">
            <ItemTemplate>
                <div class="book-card">
                    <div class="cover-wrap">
                        <img src='<%# Eval("AnhBia") %>' class="book-cover" alt="Bìa sách"
                             onerror="this.onerror=null;this.src='data:image/svg+xml;utf8,<svg xmlns=%22http://www.w3.org/2000/svg%22 width=%22300%22 height=%22220%22><rect width=%22100%25%22 height=%22100%25%22 fill=%22%2334495e%22/><text x=%2250%25%22 y=%2250%25%22 font-family=%22Arial%22 font-size=%2220%22 fill=%22%23ecf0f1%22 text-anchor=%22middle%22 dy=%22.3em%22>📘 SÁCH TRỰC TUYẾN</text></svg>';" />
                        <span class='badge <%# Convert.ToInt32(Eval("QuyenXem")) == 1 ? "badge-friend" : "badge-public" %>'>
                            <%# Convert.ToInt32(Eval("QuyenXem")) == 1 ? "👥 Chỉ bạn bè" : "🌐 Công khai" %>
                        </span>
                    </div>
                    <div class="card-body">
                        <div class="card-cat"><%# Eval("TenCD") %></div>
                        <div class="card-title"><%# Eval("TieuDe") %></div>
                        <div class="card-author">Tác giả: <%# Eval("TacGia") %></div>
                        <div class="card-desc"><%# Eval("MoTaNgan") %></div>
                        <div class="card-footer">
                            <span>👁️ <%# Eval("LuotXem") %> lượt đọc</span>
                            <a href='ChiTietSach.aspx?id=<%# Eval("MaTL") %>' class="btn btn-outline" style="padding:4px 12px;">Đọc ngay</a>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>