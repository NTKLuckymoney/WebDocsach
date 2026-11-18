using System;
using System.Data.SQLite;
using System.Web;

namespace WebDocSachBTL
{
    public partial class DangNhap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.Cookies["BookHubUser"] != null)
            {
                txtUser.Text = Request.Cookies["BookHubUser"].Value;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            bool dangNhapThanhCong = false;

            using (SQLiteConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                const string sql =
                    "SELECT MaND, HoTen, VaiTro FROM NguoiDung " +
                    "WHERE TenDangNhap = @u AND MatKhau = @p LIMIT 1";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", txtUser.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtPass.Text.Trim());

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Chuyen INTEGER cua SQLite ve int de phu hop voi code cu.
                            Session["MaND"] = Convert.ToInt32(reader["MaND"]);
                            Session["HoTen"] = Convert.ToString(reader["HoTen"]);
                            Session["VaiTro"] = Convert.ToInt32(reader["VaiTro"]);
                            dangNhapThanhCong = true;
                        }
                    }
                }
            }

            if (dangNhapThanhCong)
            {
                if (chkGhiNho.Checked)
                {
                    HttpCookie ck = new HttpCookie("BookHubUser", txtUser.Text.Trim());
                    ck.Expires = DateTime.Now.AddDays(7);
                    ck.HttpOnly = true;
                    ck.Secure = Request.IsSecureConnection;
                    Response.Cookies.Add(ck);
                }

                Response.Redirect("TrangChu.aspx");
            }
            else
            {
                lblThongBao.Text = "Sai tên đăng nhập hoặc mật khẩu!";
            }
        }
    }
}