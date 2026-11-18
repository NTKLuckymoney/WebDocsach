
using System;
using System.Data;
using System.Data.SQLite;
using System.Web.UI.WebControls;

namespace WebDocSachBTL
{
    public partial class BanBe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Bat buoc dang nhap de su dung chuc nang ban be.
            if (Session["MaND"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadDuLieu();
            }
        }

        private void LoadDuLieu()
        {
            int me = Convert.ToInt32(Session["MaND"]);

            using (SQLiteConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Open();

                // 1. Lay danh sach ban be va loi moi.
                const string sqlBB = @"
                    SELECT
                        CASE
                            WHEN bb.MaND1 = @me
                            THEN bb.MaND2
                            ELSE bb.MaND1
                        END AS MaBan,

                        bb.MaND1 AS NguoiGui,
                        bb.TrangThai,
                        nd.HoTen,
                        nd.TenDangNhap

                    FROM BanBe bb

                    JOIN NguoiDung nd
                        ON nd.MaND =
                            CASE
                                WHEN bb.MaND1 = @me
                                THEN bb.MaND2
                                ELSE bb.MaND1
                            END

                    WHERE
                        bb.MaND1 = @me
                        OR bb.MaND2 = @me

                    ORDER BY
                        bb.TrangThai DESC,
                        nd.HoTen COLLATE NOCASE";

                using (SQLiteCommand cmd =
                    new SQLiteCommand(sqlBB, conn))
                {
                    cmd.Parameters.AddWithValue("@me", me);

                    DataTable dt = new DataTable();

                    using (SQLiteDataAdapter da =
                        new SQLiteDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    rptBanBe.DataSource = dt;
                    rptBanBe.DataBind();
                }

                // 2. Lay danh sach nguoi chua ket ban.
                const string sqlGY = @"
                    SELECT
                        MaND,
                        HoTen,
                        TenDangNhap

                    FROM NguoiDung

                    WHERE MaND <> @me

                      AND MaND NOT IN
                      (
                          SELECT MaND2
                          FROM BanBe
                          WHERE MaND1 = @me

                          UNION

                          SELECT MaND1
                          FROM BanBe
                          WHERE MaND2 = @me
                      )

                    ORDER BY HoTen COLLATE NOCASE";

                using (SQLiteCommand cmd =
                    new SQLiteCommand(sqlGY, conn))
                {
                    cmd.Parameters.AddWithValue("@me", me);

                    DataTable dt = new DataTable();

                    using (SQLiteDataAdapter da =
                        new SQLiteDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    rptGoiY.DataSource = dt;
                    rptGoiY.DataBind();
                }
            }
        }

        // Xu ly chap nhan loi moi hoac huy ket ban.
        protected void rptBanBe_ItemCommand(
            object source,
            RepeaterCommandEventArgs e)
        {
            if (Session["MaND"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }

            // Chi xu ly hai lenh hop le.
            if (e.CommandName != "ChapNhan" &&
                e.CommandName != "XoaBan")
            {
                return;
            }

            int ban;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out ban))
            {
                return;
            }

            int me = Convert.ToInt32(Session["MaND"]);

            using (SQLiteConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql;

                if (e.CommandName == "ChapNhan")
                {
                    // Chi chap nhan loi moi do nguoi khac gui.
                    sql = @"
                        UPDATE BanBe

                        SET TrangThai = 1

                        WHERE MaND1 = @ban
                          AND MaND2 = @me
                          AND TrangThai = 0";
                }
                else
                {
                    // Huy ket ban hoac huy loi moi.
                    sql = @"
                        DELETE FROM BanBe

                        WHERE
                            (MaND1 = @me AND MaND2 = @ban)
                            OR
                            (MaND1 = @ban AND MaND2 = @me)";
                }

                using (SQLiteCommand cmd =
                    new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@me", me);
                    cmd.Parameters.AddWithValue("@ban", ban);

                    cmd.ExecuteNonQuery();
                }
            }

            // Cap nhat lai danh sach.
            LoadDuLieu();
        }

        // Xu ly gui loi moi ket ban.
        protected void rptGoiY_ItemCommand(
            object source,
            RepeaterCommandEventArgs e)
        {
            if (Session["MaND"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }

            if (e.CommandName != "GuiLoiMoi")
            {
                return;
            }

            int nguoiNhan;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out nguoiNhan))
            {
                return;
            }

            int me = Convert.ToInt32(Session["MaND"]);

            using (SQLiteConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Open();

                // Kiem tra nguoi nhan ton tai.
                // Khong cho phep tu ket ban.
                // Khong cho phep gui loi moi trung.
                const string sql = @"
                    INSERT INTO BanBe
                    (
                        MaND1,
                        MaND2,
                        TrangThai
                    )

                    SELECT
                        @me,
                        @nhan,
                        0

                    WHERE @me <> @nhan

                      AND EXISTS
                      (
                          SELECT 1
                          FROM NguoiDung
                          WHERE MaND = @nhan
                      )

                      AND NOT EXISTS
                      (
                          SELECT 1
                          FROM BanBe

                          WHERE
                              (MaND1 = @me AND MaND2 = @nhan)
                              OR
                              (MaND1 = @nhan AND MaND2 = @me)
                      )";

                using (SQLiteCommand cmd =
                    new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@me", me);

                    cmd.Parameters.AddWithValue(
                        "@nhan", nguoiNhan
                    );

                    cmd.ExecuteNonQuery();
                }
            }

            // Tai lai danh sach sau khi gui loi moi.
            LoadDuLieu();
        }
    }
}
