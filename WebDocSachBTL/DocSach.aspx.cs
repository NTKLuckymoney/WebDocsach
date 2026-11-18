using System;
using System.Data;
using System.Data.SQLite;

namespace WebDocSachBTL
{
    public partial class DocSach : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int maTL;
            if (!int.TryParse(Request.QueryString["id"], out maTL) || maTL <= 0)
            {
                Response.Redirect("TrangChu.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadNoiDungSach(maTL);
            }
        }

        private void LoadNoiDungSach(int maTL)
        {
            int maNDHienTai = 0;
            if (Session["MaND"] != null)
            {
                int.TryParse(Session["MaND"].ToString(), out maNDHienTai);
            }

            bool laAdmin = Session["VaiTro"] != null &&
                           Session["VaiTro"].ToString() == "1";

            const string sql = @"
                SELECT tl.*, cd.TenCD,
                       CASE WHEN EXISTS (
                           SELECT 1 FROM BanBe bb
                           WHERE bb.TrangThai = 1
                             AND ((bb.MaND1 = @me AND bb.MaND2 = tl.MaNguoiDang)
                               OR (bb.MaND2 = @me AND bb.MaND1 = tl.MaNguoiDang))
                       ) THEN 1 ELSE 0 END AS LaBanBe
                FROM TaiLieu tl
                JOIN ChuDe cd ON tl.MaCD = cd.MaCD
                WHERE tl.MaTL = @id AND tl.TrangThaiDuyet = 1";

            using (SQLiteConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                DataTable dt = new DataTable();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", maTL);
                    cmd.Parameters.AddWithValue("@me", maNDHienTai);

                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    Response.Redirect("TrangChu.aspx");
                    return;
                }

                DataRow r = dt.Rows[0];
                int quyenXem = Convert.ToInt32(r["QuyenXem"]);
                int maNguoiDang = r["MaNguoiDang"] == DBNull.Value
                    ? -1
                    : Convert.ToInt32(r["MaNguoiDang"]);
                bool laBanBe = Convert.ToInt32(r["LaBanBe"]) == 1;

                // Kiểm tra quyền ngay trên server, không chỉ ở giao diện.
                if (quyenXem == 1 &&
                    maNDHienTai != maNguoiDang &&
                    !laBanBe &&
                    !laAdmin)
                {
                    Response.Redirect("ChiTietSach.aspx?id=" + maTL);
                    return;
                }

                // Chỉ tăng lượt xem sau khi đã xác thực quyền đọc.
                using (SQLiteCommand cmdView = new SQLiteCommand(
                    "UPDATE TaiLieu SET LuotXem = COALESCE(LuotXem, 0) + 1 WHERE MaTL = @id",
                    conn))
                {
                    cmdView.Parameters.AddWithValue("@id", maTL);
                    cmdView.ExecuteNonQuery();
                }

                Session["SachDangDoc"] = r["TieuDe"].ToString();

                // ASP.NET Label render HTML: mã hóa dữ liệu để không chạy mã HTML/JS lạ.
                lblChuDe.Text = Server.HtmlEncode(r["TenCD"].ToString());
                lblTieuDe.Text = Server.HtmlEncode(r["TieuDe"].ToString());
                lblTacGia.Text = Server.HtmlEncode(r["TacGia"].ToString());
                lblNoiDung.Text = Server.HtmlEncode(r["NoiDung"].ToString());

                // DocSach.aspx đã có white-space: pre-line để giữ xuống dòng.
                lnkQuayLai.NavigateUrl = "ChiTietSach.aspx?id=" + maTL;
            }
        }
    }
}
