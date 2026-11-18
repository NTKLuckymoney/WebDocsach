
using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Text;

namespace WebDocSachBTL
{
    public partial class DangTaiSach : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadChuDe();

                // Khach chua dang nhap chi duoc dang sach cong khai.
                if (Session["MaND"] == null)
                {
                    ddlQuyenXem.SelectedValue = "0";
                    ddlQuyenXem.Enabled = false;
                    lblGhiChuQuyen.Visible = true;
                }
                else
                {
                    lblGhiChuQuyen.Visible = false;
                }
            }
        }

        private void LoadChuDe()
        {
            using (SQLiteConnection conn = DatabaseHelper.GetConnection())
            {
                const string sql = "SELECT MaCD, TenCD FROM ChuDe ORDER BY MaCD";

                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ddlChuDe.DataSource = dt;
                    ddlChuDe.DataTextField = "TenCD";
                    ddlChuDe.DataValueField = "MaCD";
                    ddlChuDe.DataBind();
                }
            }
        }

        protected void btnDangSach_Click(object sender, EventArgs e)
        {
            // Kiem tra du lieu nguoi dung nhap.
            string tieuDeNhap = txtTieuDe.Text.Trim();
            string tacGiaNhap = txtTacGia.Text.Trim();
            string moTaNhap = txtMoTa.Text.Trim();

            if (string.IsNullOrWhiteSpace(tieuDeNhap) ||
                string.IsNullOrWhiteSpace(tacGiaNhap) ||
                string.IsNullOrWhiteSpace(moTaNhap))
            {
                HienThiLoi(
                    "Vui lòng nhập đầy đủ Tiêu đề, Tác giả và Mô tả ngắn!"
                );
                return;
            }

            // Lay ma chu de.
            int maCD;

            if (!int.TryParse(ddlChuDe.SelectedValue, out maCD))
            {
                HienThiLoi("Vui lòng chọn một chủ đề hợp lệ!");
                return;
            }

            // Ma hoa HTML truoc khi luu.
            string tieuDe = Server.HtmlEncode(tieuDeNhap);
            string tacGia = Server.HtmlEncode(tacGiaNhap);
            string moTa = Server.HtmlEncode(moTaNhap);
            string noiDung = Server.HtmlEncode(
                txtNoiDung.Text.Trim()
            );

            // Neu tai file TXT thi lay noi dung tu file.
            if (fuFileNoiDung.HasFile)
            {
                string extTxt = Path.GetExtension(
                    fuFileNoiDung.FileName
                ).ToLowerInvariant();

                if (extTxt != ".txt")
                {
                    HienThiLoi(
                        "File nội dung chỉ hỗ trợ định dạng .txt!"
                    );
                    return;
                }

                using (StreamReader reader = new StreamReader(
                    fuFileNoiDung.PostedFile.InputStream,
                    Encoding.UTF8))
                {
                    noiDung = Server.HtmlEncode(
                        reader.ReadToEnd()
                    );
                }
            }

            // Kiem tra noi dung sach.
            if (string.IsNullOrWhiteSpace(noiDung))
            {
                HienThiLoi(
                    "Vui lòng nhập nội dung sách hoặc tải lên file .txt!"
                );
                return;
            }

            // Kiem tra dinh dang anh bia.
            string anhBiaPath = "";
            string extAnh = "";

            if (fuAnhBia.HasFile)
            {
                extAnh = Path.GetExtension(
                    fuAnhBia.FileName
                ).ToLowerInvariant();

                if (extAnh != ".jpg" &&
                    extAnh != ".jpeg" &&
                    extAnh != ".png" &&
                    extAnh != ".webp")
                {
                    HienThiLoi(
                        "Chỉ chấp nhận file ảnh định dạng .jpg, .jpeg, .png hoặc .webp!"
                    );
                    return;
                }
            }

            // Khach chi duoc dang cong khai.
            // Thanh vien co the chon chi ban be.
            int quyenXem =
                Session["MaND"] != null &&
                ddlQuyenXem.SelectedValue == "1"
                    ? 1
                    : 0;

            // Admin duyet ngay, thanh vien va khach cho duyet.
            bool laAdmin =
                Session["VaiTro"] != null &&
                Session["VaiTro"].ToString() == "1";

            int trangThaiDuyet = laAdmin ? 1 : 0;

            // Luu anh bia vao thu muc Uploads.
            if (fuAnhBia.HasFile)
            {
                string uploadFolder = Server.MapPath(
                    "~/Uploads/"
                );

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Tao ten file duy nhat de tranh ghi de.
                string fileName =
                    Guid.NewGuid().ToString("N") + extAnh;

                fuAnhBia.SaveAs(
                    Path.Combine(uploadFolder, fileName)
                );

                anhBiaPath = "Uploads/" + fileName;
            }

            // Cau lenh INSERT SQLite.
            const string sql = @"
                INSERT INTO TaiLieu
                (
                    TieuDe,
                    TacGia,
                    MoTaNgan,
                    NoiDung,
                    AnhBia,
                    MaCD,
                    MaNguoiDang,
                    QuyenXem,
                    TrangThaiDuyet
                )
                VALUES
                (
                    @td,
                    @tg,
                    @mt,
                    @nd,
                    @anh,
                    @macd,
                    @mand,
                    @quyen,
                    @duyet
                )";

            using (SQLiteConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SQLiteCommand cmd =
                    new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@td", tieuDe
                    );

                    cmd.Parameters.AddWithValue(
                        "@tg", tacGia
                    );

                    cmd.Parameters.AddWithValue(
                        "@mt", moTa
                    );

                    cmd.Parameters.AddWithValue(
                        "@nd", noiDung
                    );

                    cmd.Parameters.AddWithValue(
                        "@anh", anhBiaPath
                    );

                    cmd.Parameters.AddWithValue(
                        "@macd", maCD
                    );

                    cmd.Parameters.AddWithValue(
                        "@mand",
                        Session["MaND"] ?? (object)DBNull.Value
                    );

                    cmd.Parameters.AddWithValue(
                        "@quyen", quyenXem
                    );

                    cmd.Parameters.AddWithValue(
                        "@duyet", trangThaiDuyet
                    );

                    cmd.ExecuteNonQuery();
                }
            }

            // Xoa du lieu form sau khi dang thanh cong.
            txtTieuDe.Text = "";
            txtTacGia.Text = "";
            txtMoTa.Text = "";
            txtNoiDung.Text = "";

            // Thong bao ket qua.
            lblThongBao.ForeColor = Color.Green;

            lblThongBao.Text = laAdmin
                ? "✅ Đăng sách thành công! Tài liệu đã hiển thị ngay trên trang chủ."
                : "✅ Gửi tài liệu thành công! Vui lòng chờ Quản trị viên kiểm duyệt trước khi hiển thị công khai.";
        }

        private void HienThiLoi(string thongBao)
        {
            lblThongBao.ForeColor = Color.Red;
            lblThongBao.Text = thongBao;
        }
    }
}
