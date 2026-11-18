
using System;
using System.Data.SQLite;
using System.IO;
using System.Web;

namespace WebDocSachBTL
{
    public static class DatabaseHelper
    {
        public static SQLiteConnection GetConnection()
        {
            // Lấy đường dẫn file SQLite
            string dbPath = HttpContext.Current.Server.MapPath(
                "~/App_Data/WebDocSach.db");

            // Kiểm tra file có tồn tại không
            if (!File.Exists(dbPath))
            {
                throw new FileNotFoundException(
                    "Không tìm thấy cơ sở dữ liệu: " + dbPath);
            }

            // Chuỗi kết nối SQLite
            string connStr =
                "Data Source=" + dbPath +
                ";Version=3;Foreign Keys=True;";

            // Trả về kết nối chưa mở
            return new SQLiteConnection(connStr);
        }
    }
}
