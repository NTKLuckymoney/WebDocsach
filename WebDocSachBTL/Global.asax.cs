using System;
using System.Web;

namespace WebDocSachBTL
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Application["TongTruyCap"] = 1024;
            Application["DangOnline"] = 1;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["TongTruyCap"] = (int)Application["TongTruyCap"] + 1;
            Application["DangOnline"] = (int)Application["DangOnline"] + 1;
            Application.UnLock();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            int online = (int)Application["DangOnline"];
            if (online > 1) Application["DangOnline"] = online - 1;
            Application.UnLock();
        }
    }
}