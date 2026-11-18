using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebDocSachBTL
{
    public partial class TestDB_aspx : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "SELECT COUNT(*) FROM NguoiDung";

                    int soTaiKhoan = Convert.ToInt32(
                        cmd.ExecuteScalar());

                    Response.Write(
                        "Kết nối SQLite thành công! " +
                        "Số tài khoản: " + soTaiKhoan);
                }
            }
        }

    }
}