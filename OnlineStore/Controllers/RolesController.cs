using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineStore;

namespace OnlineStore.Controllers
{
    public class RolesController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();

        // GET: Roles
        public ActionResult Show()
        {
            object list = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    list = db.Roles.ToList();
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
            return View(list);
        }

        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            Role roles = null;
            if (Session["Username"] != null)
            {
                 var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    roles = db.Roles.Find(id);
                    if (roles == null)
                    {
                        return HttpNotFound();
                    }
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
            
            return View(roles);
        }

        // GET: Roles/Create
        public ActionResult Add()
        {
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
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

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "RoleiD,RoleName,RoleDesc")] Role role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isExist = (from l in db.Roles.Where(x => x.RoleName == role.RoleName) select l).FirstOrDefault();
                    if (isExist == null)
                    {
                        db.Roles.Add(role);
                        db.SaveChanges();
                        //return RedirectToAction("Show");]
                        ViewBag.DisplayMessage = "success";
                        ModelState.AddModelError("", "Role saved successfully!");
                    }
                    else
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", "Role Name " + isExist.RoleName + " already exist, Please enter a new Role!");
                        //LoadDropDownList();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Error!" + ex.Message);
                }
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            Role roles = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    roles = db.Roles.Find(id);
                    if (roles == null)
                    {
                        return HttpNotFound();
                    }
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

            return View(roles);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleiD,RoleName,RoleDesc")] Role role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(role).State = EntityState.Modified;
                    db.SaveChanges();
                    // return RedirectToAction("Show");
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Role updated successfully!");
                }
                catch (Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Error!" + ex.Message);
                }
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            Role roles = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    roles = db.Roles.Find(id);
                    if (roles == null)
                    {
                        return HttpNotFound();
                    }
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
            
            return View(roles);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Role role = db.Roles.Find(id);
            try
            {
                
                db.Roles.Remove(role);
                db.SaveChanges();
                // return RedirectToAction("Show");
                ViewBag.DisplayMessage = "success";
                ModelState.AddModelError("", "Role deleted successfully!");
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Error!" + ex.Message);
            }
            return View(role);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
