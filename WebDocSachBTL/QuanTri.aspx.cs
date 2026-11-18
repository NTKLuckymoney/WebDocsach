
using System;
using System.Data;
using System.Data.SQLite;
using System.Web.UI.WebControls;

namespace WebDocSachBTL
{
    public partial class QuanTri : System.Web.UI.Page
    {
        private bool LaAdmin
        {
            get
            {
                return Session["VaiTro"] != null &&
                       Session["VaiTro"].ToString() == "1";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Chi quan tri vien moi duoc vao trang nay.
            if (!LaAdmin)
            {
                Response.Redirect("TrangChu.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadDuLieuQuanTri();
            }
        }

        private void LoadDuLieuQuanTri()
        {
            using (SQLiteConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // 1. Thong ke tong quan.
                const string sqlThongKe = @"
                    SELECT
                        (SELECT COUNT(*) FROM TaiLieu) AS TongSach,
                        (SELECT COUNT(*) FROM TaiLieu WHERE TrangThaiDuyet = 0) AS ChoDuyet,
                        (SELECT COUNT(*) FROM NguoiDung) AS TongUser,
                        (SELECT COALESCE(SUM(LuotXem), 0) FROM TaiLieu) AS TongLuotXem";

                using (SQLiteCommand cmd = new SQLiteCommand(sqlThongKe, conn))
                using (SQLiteDataReader rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        lblTongSach.Text = rd["TongSach"].ToString();
                        lblChoDuyet.Text = rd["ChoDuyet"].ToString();
                        lblTongUser.Text = rd["TongUser"].ToString();
                        lblTongLuotXem.Text = rd["TongLuotXem"].ToString();
                    }
                }

                // 2. Tai lieu cho duyet hien truoc.
                // NgayDang la TEXT trong SQLite.
                const string sqlTaiLieu = @"
                    SELECT
                        tl.MaTL,
                        tl.TieuDe,
                        tl.TacGia,
                        tl.MoTaNgan,
                        tl.NoiDung,
                        tl.AnhBia,
                        tl.LuotXem,
                        tl.MaCD,
                        tl.MaNguoiDang,
                        tl.QuyenXem,
                        tl.TrangThaiDuyet,
                        COALESCE(
                            strftime('%d/%m/%Y', tl.NgayDang),
                            tl.NgayDang,
                            ''
                        ) AS NgayDang,
                        cd.TenCD,
                        COALESCE(nd.HoTen, 'Khách vãng lai') AS NguoiDang
                    FROM TaiLieu tl
                    JOIN ChuDe cd ON tl.MaCD = cd.MaCD
                    LEFT JOIN NguoiDung nd
                        ON tl.MaNguoiDang = nd.MaND
                    ORDER BY
                        tl.TrangThaiDuyet ASC,
                        tl.NgayDang DESC,
                        tl.MaTL DESC";

                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sqlTaiLieu, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptTaiLieu.DataSource = dt;
                    rptTaiLieu.DataBind();
                }

                // 3. Danh sach chu de kem so luong sach.
                const string sqlChuDe = @"
                    SELECT
                        cd.MaCD,
                        cd.TenCD,
                        COUNT(tl.MaTL) AS SoSach
                    FROM ChuDe cd
                    LEFT JOIN TaiLieu tl
                        ON cd.MaCD = tl.MaCD
                    GROUP BY
                        cd.MaCD,
                        cd.TenCD
                    ORDER BY cd.MaCD";

                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sqlChuDe, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptChuDe.DataSource = dt;
                    rptChuDe.DataBind();
                }
            }
        }

        protected void rptTaiLieu_ItemCommand(
            object source,
            RepeaterCommandEventArgs e)
        {
            if (!LaAdmin)
            {
                Response.Redirect("TrangChu.aspx");
                return;
            }

            int maTL;

            if (e.CommandArgument == null ||
                !int.TryParse(e.CommandArgument.ToString(), out maTL) ||
                maTL <= 0)
            {
                return;
            }

            string sql;

            switch (e.CommandName)
            {
                case "Duyet":
                    sql = @"UPDATE TaiLieu
                            SET TrangThaiDuyet = 1
                            WHERE MaTL = @id";
                    break;

                case "An":
                    sql = @"UPDATE TaiLieu
                            SET TrangThaiDuyet = 0
                            WHERE MaTL = @id";
                    break;

                case "Xoa":
                    sql = @"DELETE FROM TaiLieu
                            WHERE MaTL = @id";
                    break;

                default:
                    return;
            }

            using (SQLiteConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", maTL);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadDuLieuQuanTri();
        }

        protected void btnThemCD_Click(object sender, EventArgs e)
        {
            if (!LaAdmin)
            {
                Response.Redirect("TrangChu.aspx");
                return;
            }

            string tenChuDe = txtTenCD.Text.Trim();

            if (string.IsNullOrWhiteSpace(tenChuDe))
            {
                return;
            }

            const string sql = @"
                INSERT INTO ChuDe (TenCD, MoTa)
                VALUES (@ten, @mota)";

            using (SQLiteConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@ten",
                        Server.HtmlEncode(tenChuDe)
                    );

                    cmd.Parameters.AddWithValue(
                        "@mota",
                        Server.HtmlEncode(txtMoTaCD.Text.Trim())
                    );

                    cmd.ExecuteNonQuery();
                }
            }

            txtTenCD.Text = "";
            txtMoTaCD.Text = "";

            LoadDuLieuQuanTri();
        }
    }
}
