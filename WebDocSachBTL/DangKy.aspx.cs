using System;
using System.Data.SQLite;
using System.Drawing;

namespace WebDocSachBTL
{
    public partial class DangKy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnDangKy_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text.Trim();
            string pass = txtPass.Text.Trim();
            string hoTen = Server.HtmlEncode(txtHoTen.Text.Trim());
            string email = Server.HtmlEncode(txtEmail.Text.Trim());

            // Kiem tra tren server (khong chi dua vao JavaScript).
            if (string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(hoTen) ||
                pass.Length < 6 ||
                pass != txtRePass.Text)
            {
                lblThongBao.ForeColor = Color.Red;
                lblThongBao.Text = "Vui lòng nhập đủ thông tin, mật khẩu ít nhất 6 ký tự và xác nhận khớp!";
                return;
            }

            using (SQLiteConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // 1. Kiem tra ten dang nhap da ton tai.
                using (SQLiteCommand cmdCheck = new SQLiteCommand(
                    "SELECT COUNT(*) FROM NguoiDung WHERE TenDangNhap = @u", conn))
                {
                    cmdCheck.Parameters.AddWithValue("@u", user);
                    int tonTai = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    if (tonTai > 0)
                    {
                        lblThongBao.ForeColor = Color.Red;
                        lblThongBao.Text = "Tên đăng nhập này đã có người sử dụng, vui lòng chọn tên khác!";
                        return;
                    }
                }

                // 2. Them nguoi dung moi vao SQLite.
                const string sqlInsert =
                    "INSERT INTO NguoiDung (TenDangNhap, MatKhau, HoTen, Email, VaiTro) " +
                    "VALUES (@u, @p, @ten, @email, 0)";

                using (SQLiteCommand cmd = new SQLiteCommand(sqlInsert, conn))
                {
                    cmd.Parameters.AddWithValue("@u", user);
                    cmd.Parameters.AddWithValue("@p", pass);
                    cmd.Parameters.AddWithValue("@ten", hoTen);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.ExecuteNonQuery();
                }

                lblThongBao.ForeColor = Color.Green;
                lblThongBao.Text = "Đăng ký thành công! Bạn có thể bấm 'Đăng nhập ngay' để vào hệ thống.";
            }
        }
    }
}
