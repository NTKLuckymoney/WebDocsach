<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocSach.aspx.cs" Inherits="WebDocSachBTL.DocSach" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .reader-toolbar { background: #fff; padding: 12px 20px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.08); display: flex; flex-wrap: wrap; justify-content: space-between; align-items: center; gap: 12px; margin-bottom: 20px; position: sticky; top: 70px; z-index: 90; }
        .reader-paper { background: #ffffff; color: #222222; padding: 45px 55px; border-radius: 12px; box-shadow: 0 4px 20px rgba(0,0,0,0.07); max-width: 860px; margin: 0 auto; transition: background 0.3s, color 0.3s; }
        .theme-sepia { background: #f4ecd8 !important; color: #5b4636 !important; }
        .theme-dark { background: #1c1e22 !important; color: #dcdcdc !important; }
        .book-content { font-size: 18px; line-height: 1.85; white-space: pre-line; text-align: justify; margin-top: 25px; }
        .tool-group { display: flex; align-items: center; gap: 6px; }
    </style>

    <!-- Thanh công cụ đọc sách (Client-side JS + Cookie) -->
    <div class="reader-toolbar">
        <div>
            <asp:HyperLink ID="lnkQuayLai" runat="server" CssClass="btn btn-light">&laquo; Thông tin sách</asp:HyperLink>
        </div>

        <div class="tool-group">
            <span style="font-size:13px; color:#666;">Cỡ chữ:</span>
            <button type="button" class="btn btn-light" onclick="doiCoChu(-2)">A-</button>
            <span id="lblSizeHienTai" style="font-weight:700; min-width:38px; text-align:center;">18px</span>
            <button type="button" class="btn btn-light" onclick="doiCoChu(2)">A+</button>
        </div>

        <div class="tool-group">
            <span style="font-size:13px; color:#666;">Font:</span>
            <select id="selFont" class="input-ctrl" onchange="doiFont(this.value)" style="padding:5px 8px;">
                <option value="'Segoe UI', sans-serif">Hiện đại (Sans)</option>
                <option value="Georgia, 'Times New Roman', serif">Sách in (Serif)</option>
            </select>
        </div>

        <div class="tool-group">
            <span style="font-size:13px; color:#666;">Màu nền:</span>
            <button type="button" class="btn" style="background:#fff; border:1px solid #ccc; color:#000;" onclick="doiTheme('light')">☀️ Sáng</button>
            <button type="button" class="btn" style="background:#f4ecd8; border:1px solid #d5c5a3; color:#5b4636;" onclick="doiTheme('sepia')">📜 Giấy cũ</button>
            <button type="button" class="btn" style="background:#222; color:#fff;" onclick="doiTheme('dark')">🌙 Tối</button>
        </div>
    </div>

    <!-- Khung trang sách -->
    <div id="khungSach" class="reader-paper">
        <div style="text-align:center; border-bottom:1px solid rgba(128,128,128,0.25); padding-bottom:20px;">
            <small style="text-transform:uppercase; letter-spacing:1px; opacity:0.75;"><asp:Label ID="lblChuDe" runat="server"></asp:Label></small>
            <h1 style="font-size:30px; margin:8px 0;"><asp:Label ID="lblTieuDe" runat="server"></asp:Label></h1>
            <div style="font-size:15px; opacity:0.8;">Tác giả: <b><asp:Label ID="lblTacGia" runat="server"></asp:Label></b></div>
        </div>

        <div id="noiDungDoc" class="book-content">
            <asp:Label ID="lblNoiDung" runat="server"></asp:Label>
        </div>

        <div style="text-align:center; margin-top:40px; padding-top:20px; border-top:1px solid rgba(128,128,128,0.25);">
            <p style="opacity:0.7; margin-bottom:12px;">— Hết nội dung tài liệu —</p>
            <a href="TrangChu.aspx" class="btn btn-primary">Khám phá sách khác</a>
        </div>
    </div>

    <script type="text/javascript">
var coChu = 18;

function luuCookie(ten, giaTri) {
    document.cookie = ten + "=" + encodeURIComponent(giaTri) + ";path=/;max-age=" + (30 * 24 * 60 * 60);
}

function docCookie(ten) {
    var match = document.cookie.match(new RegExp('(^| )' + ten + '=([^;]+)'));
    return match ? decodeURIComponent(match[2]) : null;
}

function doiCoChu(buoc) {
    coChu += buoc;
    if (coChu < 14) coChu = 14;
    if (coChu > 30) coChu = 30;
    document.getElementById("noiDungDoc").style.fontSize = coChu + "px";
    document.getElementById("lblSizeHienTai").innerText = coChu + "px";
    luuCookie("ReaderSize", coChu);
}

function doiFont(font) {
    document.getElementById("noiDungDoc").style.fontFamily = font;
    luuCookie("ReaderFont", font);
}

function doiTheme(theme) {
    var paper = document.getElementById("khungSach");
    paper.classList.remove("theme-sepia", "theme-dark");
    if (theme === "sepia") paper.classList.add("theme-sepia");
    if (theme === "dark") paper.classList.add("theme-dark");
    luuCookie("ReaderTheme", theme);
}

// Tự động khôi phục chế độ đọc từ Cookie khi mở trang
window.onload = function () {
    var savedSize = docCookie("ReaderSize");
    if (savedSize) {
        coChu = parseInt(savedSize);
        document.getElementById("noiDungDoc").style.fontSize = coChu + "px";
        document.getElementById("lblSizeHienTai").innerText = coChu + "px";
    }
    var savedFont = docCookie("ReaderFont");
    if (savedFont) {
        document.getElementById("noiDungDoc").style.fontFamily = savedFont;
        document.getElementById("selFont").value = savedFont;
    }
    var savedTheme = docCookie("ReaderTheme");
    if (savedTheme) {
        doiTheme(savedTheme);
    }
};
</script>
</asp:Content>
