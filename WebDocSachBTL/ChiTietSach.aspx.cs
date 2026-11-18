
using System;
using System.Data;
using System.Data.SQLite;
using System.Globalization;

namespace WebDocSachBTL
{
    public partial class ChiTietSach : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int maTL;

            // Kiem tra ID sach tren URL
            if (!int.TryParse(Request.QueryString["id"], out maTL)
                || maTL <= 0)
            {
                Response.Redirect("TrangChu.aspx");
                return;
            }

            // Neu da dang nhap thi an o nhap ten khach
            pnlTenKhachBL.Visible = (Session["MaND"] == null);

            if (!IsPostBack)
            {
                // Chi tai binh luan khi co quyen xem sach
                if (LoadThongTinSach(maTL))
                {
                    LoadBinhLuan(maTL);
                }
            }
        }

        // Ham ma hoa du lieu truoc khi hien thi
        private string HienThiAnToan(object giaTri)
        {
            string text = Convert.ToString(giaTri);

            return Server.HtmlEncode(
                Server.HtmlDecode(text)
            );
        }

        // ==========================================
        // 1. HIEN THI THONG TIN CHI TIET SACH
        // ==========================================

        private bool LoadThongTinSach(int maTL)
        {
            int maNDHienTai = 0;

            if (Session["MaND"] != null)
            {
                int.TryParse(
                    Session["MaND"].ToString(),
                    out maNDHienTai
                );
            }

            bool laAdmin =
                Session["VaiTro"] != null &&
                Session["VaiTro"].ToString() == "1";

            string sql = @"
                SELECT
                    tl.*,
                    cd.TenCD,

                    COALESCE(
                        nd.HoTen,
                        'Khách đóng góp'
                    ) AS NguoiDang,

                    CASE
                        WHEN EXISTS
                        (
                            SELECT 1
                            FROM BanBe bb
                            WHERE bb.TrangThai = 1
                            AND
                            (
                                (
                                    bb.MaND1 = @me
                                    AND bb.MaND2 = tl.MaNguoiDang
                                )
                                OR
                                (
                                    bb.MaND2 = @me
                                    AND bb.MaND1 = tl.MaNguoiDang
                                )
                            )
                        )
                        THEN 1
                        ELSE 0
                    END AS LaBanBe

                FROM TaiLieu tl

                JOIN ChuDe cd
                    ON tl.MaCD = cd.MaCD

                LEFT JOIN NguoiDung nd
                    ON tl.MaNguoiDang = nd.MaND

                WHERE tl.MaTL = @id
                  AND tl.TrangThaiDuyet = 1";

            using (SQLiteConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SQLiteCommand cmd =
                    new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", maTL);
                    cmd.Parameters.AddWithValue(
                        "@me", maNDHienTai
                    );

                    DataTable dt = new DataTable();

                    using (SQLiteDataAdapter da =
                        new SQLiteDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    // Khong tim thay sach hoac sach chua duyet
                    if (dt.Rows.Count == 0)
                    {
                        Response.Redirect("TrangChu.aspx");
                        return false;
                    }

                    DataRow r = dt.Rows[0];

                    int quyenXem =
                        Convert.ToInt32(r["QuyenXem"]);

                    int maNguoiDang =
                        r["MaNguoiDang"] != DBNull.Value
                            ? Convert.ToInt32(r["MaNguoiDang"])
                            : -1;

                    bool laBanBe =
                        Convert.ToInt32(r["LaBanBe"]) == 1;

                    // ==================================
                    // 2. KIEM TRA QUYEN XEM SACH
                    // ==================================

                    // Sach chi ban be:
                    // Chu sach, ban be hoac Admin duoc xem
                    if (quyenXem == 1 &&
                        maNDHienTai != maNguoiDang &&
                        !laBanBe &&
                        !laAdmin)
                    {
                        pnlChiTiet.Visible = false;
                        pnlChanTruyCap.Visible = true;

                        return false;
                    }

                    pnlChiTiet.Visible = true;
                    pnlChanTruyCap.Visible = false;

                    // ==================================
                    // 3. HIEN THI THONG TIN SACH
                    // ==================================

                    lblTieuDe.Text =
                        HienThiAnToan(r["TieuDe"]);

                    lblChuDe.Text =
                        HienThiAnToan(r["TenCD"]);

                    lblTacGia.Text =
                        HienThiAnToan(r["TacGia"]);

                    lblNguoiDang.Text =
                        HienThiAnToan(r["NguoiDang"]);

                    lblLuotXem.Text =
                        r["LuotXem"].ToString();

                    lblMoTa.Text =
                        HienThiAnToan(r["MoTaNgan"]);

                    lblQuyenXem.Text =
                        quyenXem == 1
                            ? "👥 Chỉ bạn bè"
                            : "🌐 Công khai";

                    // SQLite luu ngay duoi dang TEXT
                    DateTime ngayDang;

                    if (DateTime.TryParse(
                        r["NgayDang"].ToString(),
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out ngayDang))
                    {
                        lblNgayDang.Text =
                            ngayDang.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        lblNgayDang.Text = "";
                    }

                    // Hien thi anh bia
                    imgBia.Src =
                        r["AnhBia"].ToString();

                    // Duong dan doc sach
                    lnkDocSach.NavigateUrl =
                        "DocSach.aspx?id=" + maTL;

                    return true;
                }
            }
        }

        // ==========================================
        // 4. HIEN THI DANH SACH BINH LUAN
        // ==========================================

        private void LoadBinhLuan(int maTL)
        {
            string sql = @"
                SELECT
                    MaBL,
                    MaTL,
                    TenNguoiBinhLuan,
                    NoiDungBL,

                    COALESCE(
                        strftime(
                            '%d/%m/%Y %H:%M',
                            NgayBL
                        ),
                        ''
                    ) AS NgayBL

                FROM BinhLuan

                WHERE MaTL = @id

                ORDER BY BinhLuan.NgayBL DESC";

            using (SQLiteConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SQLiteCommand cmd =
                    new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", maTL);

                    DataTable dt = new DataTable();

                    using (SQLiteDataAdapter da =
                        new SQLiteDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    rptBinhLuan.DataSource = dt;
                    rptBinhLuan.DataBind();

                    // Hien thi thong bao neu chua co binh luan
                    lblChuaCoBL.Visible =
                        (dt.Rows.Count == 0);
                }
            }
        }

        // ==========================================
        // 5. XU LY GUI BINH LUAN
        // ==========================================

        protected void btnGuiBL_Click(
            object sender,
            EventArgs e)
        {
            int maTL;

            if (!int.TryParse(
                Request.QueryString["id"],
                out maTL) || maTL <= 0)
            {
                Response.Redirect("TrangChu.aspx");
                return;
            }

            // Kiem tra quyen xem truoc khi gui binh luan
            if (!LoadThongTinSach(maTL))
            {
                return;
            }

            // Lay noi dung binh luan
            string noiDungNhap =
                txtNoiDungBL.Text.Trim();

            if (string.IsNullOrWhiteSpace(noiDungNhap))
            {
                return;
            }

            // Lay ten nguoi binh luan
            string tenNguoiNhap;

            if (Session["HoTen"] != null)
            {
                tenNguoiNhap =
                    Session["HoTen"].ToString();
            }
            else
            {
                tenNguoiNhap =
                    string.IsNullOrWhiteSpace(
                        txtTenKhach.Text)
                        ? "Khách ẩn danh"
                        : txtTenKhach.Text.Trim();
            }

            // Ma hoa HTML de tranh XSS
            string tenNguoiBL =
                Server.HtmlEncode(
                    Server.HtmlDecode(tenNguoiNhap)
                );

            string noiDung =
                Server.HtmlEncode(noiDungNhap);

            // Cau lenh INSERT SQLite
            string sql = @"
                INSERT INTO BinhLuan
                (
                    MaTL,
                    TenNguoiBinhLuan,
                    NoiDungBL
                )
                VALUES
                (
                    @id,
                    @ten,
                    @nd
                )";

            using (SQLiteConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SQLiteCommand cmd =
                    new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@id", maTL
                    );

                    cmd.Parameters.AddWithValue(
                        "@ten", tenNguoiBL
                    );

                    cmd.Parameters.AddWithValue(
                        "@nd", noiDung
                    );

                    cmd.ExecuteNonQuery();
                }
            }

            // Xoa noi dung vua nhap
            txtNoiDungBL.Text = "";

            // Tai lai danh sach binh luan
            LoadBinhLuan(maTL);
        }
    }
}
