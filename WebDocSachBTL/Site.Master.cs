using System;

namespace WebDocSachBTL
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MaND"] != null)
            {
                pnlKhach.Visible = false;
                pnlThanhVien.Visible = true;
                lblTenUser.Text = Session["HoTen"].ToString();
                liBanBe.Visible = true;

                if (Session["VaiTro"] != null && Session["VaiTro"].ToString() == "1")
                {
                    liAdmin.Visible = true;
                }
            }
            else
            {
                pnlKhach.Visible = true;
                pnlThanhVien.Visible = false;
                liBanBe.Visible = false;
                liAdmin.Visible = false;
            }
        }

        protected void btnTim_Click(object sender, EventArgs e)
        {
            string tuKhoa = Server.UrlEncode(txtTimKiem.Text.Trim());
            Response.Redirect("TrangChu.aspx?q=" + tuKhoa);
        }

        protected void lbtnThoat_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("TrangChu.aspx");
        }
    }
}