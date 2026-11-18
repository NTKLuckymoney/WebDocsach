<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BanBe.aspx.cs" Inherits="WebDocSachBTL.BanBe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="display:grid; grid-template-columns: 1fr 1fr; gap: 24px;">
        <div class="box-panel">
            <h3 style="margin-bottom:15px;">👥 Danh sách bạn bè & Lời mời</h3>
            <asp:Repeater ID="rptBanBe" runat="server" OnItemCommand="rptBanBe_ItemCommand">
                <ItemTemplate>
                    <div style="display:flex; justify-content:space-between; align-items:center; padding:10px 0; border-bottom:1px solid #eee;">
                        <div>
                            <strong><%# Eval("HoTen") %></strong> (<%# Eval("TenDangNhap") %>)<br />
                            <small style="color:#666;"><%# Convert.ToInt32(Eval("TrangThai")) == 1 ? "✅ Đã là bạn bè" : "⏳ Đang chờ xác nhận" %></small>
                        </div>
                        <div>
                            <asp:Button ID="btnChapNhan" runat="server" Text="Chấp nhận" CssClass="btn btn-primary"
                                CommandName="ChapNhan" CommandArgument='<%# Eval("MaBan") %>'
                                Visible='<%# Convert.ToInt32(Eval("TrangThai")) == 0 && Convert.ToInt32(Eval("NguoiGui")) != Convert.ToInt32(Session["MaND"]) %>' />
                            <asp:Button ID="btnXoa" runat="server" Text="Hủy" CssClass="btn btn-light"
                                CommandName="XoaBan" CommandArgument='<%# Eval("MaBan") %>' />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="box-panel">
            <h3 style="margin-bottom:15px;">🔍 Tìm thành viên để kết bạn</h3>
            <asp:Repeater ID="rptGoiY" runat="server" OnItemCommand="rptGoiY_ItemCommand">
                <ItemTemplate>
                    <div style="display:flex; justify-content:space-between; align-items:center; padding:10px 0; border-bottom:1px solid #eee;">
                        <div>
                            <strong><%# Eval("HoTen") %></strong><br />
                            <small style="color:#666;">@<%# Eval("TenDangNhap") %></small>
                        </div>
                        <asp:Button ID="btnKetBan" runat="server" Text="+ Kết bạn" CssClass="btn btn-outline"
                            CommandName="GuiLoiMoi" CommandArgument='<%# Eval("MaND") %>' />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
