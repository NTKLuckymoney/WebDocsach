using System;
using System.Data;
using System.Data.SQLite;
using System.Web.UI.WebControls;

namespace WebDocSachBTL
{
    public partial class TrangChu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadChuDe();
                LoadDanhSachSach();
            }
        }

        private void LoadChuDe()
        {
            using (SQLiteConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SQLiteDataAdapter da = new SQLiteDataAdapter(
                    "SELECT * FROM ChuDe", conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ddlChuDe.DataSource = dt;
                    ddlChuDe.DataTextField = "TenCD";
                    ddlChuDe.DataValueField = "MaCD";
                    ddlChuDe.DataBind();
                    ddlChuDe.Items.Insert(0,
                        new ListItem("-- Tất cả thể loại --", "0"));
                }
            }
        }

        private void LoadDanhSachSach()
        {
            int maND = Session["MaND"] != null
                ? Convert.ToInt32(Session["MaND"])
                : 0;

            int maCD;
            if (!int.TryParse(ddlChuDe.SelectedValue, out maCD))
            {
                maCD = 0;
            }

            string tuKhoa = Request.QueryString["q"] != null
                ? Request.QueryString["q"].Trim()
                : "";

            const string sql = @"
                SELECT tl.*, cd.TenCD
                FROM TaiLieu tl
                JOIN ChuDe cd ON tl.MaCD = cd.MaCD
                WHERE tl.TrangThaiDuyet = 1
                  AND (@maCD = 0 OR tl.MaCD = @maCD)
                  AND (
                      @q = ''
                      OR tl.TieuDe LIKE '%' || @q || '%'
                      OR tl.TacGia LIKE '%' || @q || '%'
                  )
                  AND (
                      tl.QuyenXem = 0
                      OR tl.MaNguoiDang = @maND
                      OR EXISTS (
                          SELECT 1
                          FROM BanBe bb
                          WHERE bb.TrangThai = 1
                            AND (
                                (bb.MaND1 = @maND AND bb.MaND2 = tl.MaNguoiDang)
                                OR
                                (bb.MaND2 = @maND AND bb.MaND1 = tl.MaNguoiDang)
                            )
                      )
                  )
                ORDER BY tl.NgayDang DESC";

            using (SQLiteConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@maCD", maCD);
                    cmd.Parameters.AddWithValue("@q", tuKhoa);
                    cmd.Parameters.AddWithValue("@maND", maND);

                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        rptSach.DataSource = dt;
                        rptSach.DataBind();
                    }
                }
            }
        }

        protected void ddlChuDe_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDanhSachSach();
        }
    }
}
