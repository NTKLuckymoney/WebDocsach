<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuanTri.aspx.cs" Inherits="WebDocSachBTL.QuanTri" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .stat-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 16px; margin-bottom: 24px; }
        .stat-card { background: #fff; padding: 20px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.06); border-left: 4px solid #ff6122; }
        .stat-num { font-size: 26px; font-weight: 800; color: #111; margin-top: 4px; }
        .admin-table { width: 100%; border-collapse: collapse; margin-top: 12px; font-size: 14px; }
        .admin-table th, .admin-table td { padding: 12px 10px; border-bottom: 1px solid #eee; text-align: left; }
        .admin-table th { background: #f8f9fa; font-weight: 700; color: #333; }
        .status-wait { background: #fff3cd; color: #856404; padding: 3px 9px; border-radius: 12px; font-size: 12px; font-weight: 600; }
        .status-ok { background: #d1e7dd; color: #0f5132; padding: 3px 9px; border-radius: 12px; font-size: 12px; font-weight: 600; }
    </style>

    <h2 style="margin-bottom: 18px;">🛡️ Bảng điều khiển Quản trị viên</h2>

    <!-- Thống kê tổng quan -->
    <div class="stat-grid">
        <div class="stat-card">
            <div style="color:#666; font-size:13px;">📚 Tổng số tài liệu</div>
            <div class="stat-num"><asp:Label ID="lblTongSach" runat="server" Text="0"></asp:Label></div>
        </div>
        <div class="stat-card" style="border-left-color:#ffc107;">
            <div style="color:#666; font-size:13px;">⏳ Đang chờ duyệt</div>
            <div class="stat-num"><asp:Label ID="lblChoDuyet" runat="server" Text="0"></asp:Label></div>
        </div>
        <div class="stat-card" style="border-left-color:#198754;">
            <div style="color:#666; font-size:13px;">👥 Thành viên</div>
            <div class="stat-num"><asp:Label ID="lblTongUser" runat="server" Text="0"></asp:Label></div>
        </div>
        <div class="stat-card" style="border-left-color:#0d6efd;">
            <div style="color:#666; font-size:13px;">👁️ Tổng lượt đọc</div>
            <div class="stat-num"><asp:Label ID="lblTongLuotXem" runat="server" Text="0"></asp:Label></div>
        </div>
    </div>

    <!-- Khu vực 1: Kiểm duyệt & Quản lý Tài liệu -->
    <div class="box-panel" style="margin-bottom: 24px; overflow-x: auto;">
        <h3>📋 Danh sách tài liệu đóng góp & Kiểm duyệt</h3>
        <table class="admin-table">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Tiêu đề tài liệu</th>
                    <th>Tác giả / Người đăng</th>
                    <th>Thể loại</th>
                    <th>Chế độ</th>
                    <th>Trạng thái</th>
                    <th>Thao tác</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptTaiLieu" runat="server" OnItemCommand="rptTaiLieu_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td>#<%# Eval("MaTL") %></td>
                            <td>
                                <strong><%# Eval("TieuDe") %></strong><br />
                                <small style="color:#777;">Ngày đăng: <%# Eval("NgayDang", "{0:dd/MM/yyyy}") %></small>
                            </td>
                            <td>
                                <%# Eval("TacGia") %><br />
                                <small style="color:#666;">Đăng bởi: <%# Eval("NguoiDang") %></small>
                            </td>
                            <td><%# Eval("TenCD") %></td>
                            <td><%# Convert.ToInt32(Eval("QuyenXem")) == 1 ? "👥 Bạn bè" : "🌐 Công khai" %></td>
                            <td>
                                <%# Convert.ToInt32(Eval("TrangThaiDuyet")) == 1 
                                    ? "<span class='status-ok'>✅ Đã duyệt</span>" 
                                    : "<span class='status-wait'>⏳ Chờ duyệt</span>" %>
                            </td>
                            <td style="white-space:nowrap;">
                                <asp:Button ID="btnDuyet" runat="server" Text="Duyệt" CssClass="btn btn-primary" style="padding:4px 10px; font-size:12px;"
                                    CommandName="Duyet" CommandArgument='<%# Eval("MaTL") %>'
                                    Visible='<%# Convert.ToInt32(Eval("TrangThaiDuyet")) == 0 %>' />
                                
                                <asp:Button ID="btnAn" runat="server" Text="Ẩn bài" CssClass="btn btn-light" style="padding:4px 10px; font-size:12px;"
                                    CommandName="An" CommandArgument='<%# Eval("MaTL") %>'
                                    Visible='<%# Convert.ToInt32(Eval("TrangThaiDuyet")) == 1 %>' />

                                <asp:Button ID="btnXoa" runat="server" Text="Xóa" CssClass="btn" style="background:#dc3545; color:#fff; padding:4px 10px; font-size:12px; margin-left:4px;"
                                    CommandName="Xoa" CommandArgument='<%# Eval("MaTL") %>'
                                    OnClientClick="return confirm('Bạn có chắc chắn muốn xóa vĩnh viễn tài liệu này?');" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>

    <!-- Khu vực 2: Thêm nhanh Thể loại / Chủ đề -->
    <div class="box-panel">
        <h3 style="margin-bottom: 12px;">🏷️ Quản lý Thể loại sách</h3>
        <div style="display:flex; flex-wrap:wrap; gap:10px; margin-bottom:16px;">
            <asp:TextBox ID="txtTenCD" runat="server" CssClass="input-ctrl" placeholder="Tên thể loại mới..." style="min-width:240px;"></asp:TextBox>
            <asp:TextBox ID="txtMoTaCD" runat="server" CssClass="input-ctrl" placeholder="Mô tả ngắn..." style="flex:1; min-width:240px;"></asp:TextBox>
            <asp:Button ID="btnThemCD" runat="server" Text="+ Thêm thể loại" CssClass="btn btn-primary" OnClick="btnThemCD_Click" />
        </div>
        <div style="display:flex; flex-wrap:wrap; gap:8px;">
            <asp:Repeater ID="rptChuDe" runat="server">
                <ItemTemplate>
                    <span style="background:#f1f3f5; padding:6px 14px; border-radius:20px; font-size:13px; border:1px solid #ddd;">
                        <b><%# Eval("TenCD") %></b> (<%# Eval("SoSach") %> sách)
                    </span>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
