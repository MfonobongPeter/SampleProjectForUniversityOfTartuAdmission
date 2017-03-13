using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    public class AdmindefaultController : Controller
    {
        //private OnlineStoreEntities db = new OnlineStoreEntities()
            // GET: Admin
        public ActionResult DashBoard()
        {
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1)
                {

                }
                else
                {
                    return RedirectToAction("AdminLogin", "Users");
                }
            }
            else
            {
                return RedirectToAction("AdminLogin", "Users");
            }
            return View();
        }
    }
}