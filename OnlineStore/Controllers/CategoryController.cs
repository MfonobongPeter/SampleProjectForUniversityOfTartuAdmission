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
    public class CategorySetUpController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();

        // GET: Category
        public ActionResult ShowList()
        {
             object CategoryList ="";

             if (Session["Username"] != null)
             {
                 var role = Convert.ToInt16(Session["userrole"].ToString());
                 if (role == 1 || role == 3)
                 {
                     try
                     {
                         CategoryList =
                                         (from u in db.Categories

                                          select new CategorySetUpViewModel
                                          {
                                              CategoryID = u.CategoryID,
                                              CategoryName = u.CategoryName,
                                              CreatedBy = u.CreatedBy,
                                              CreatedOn = u.CreatedOn
                                          }).AsNoTracking().ToList();
                     }
                     catch (Exception ex)
                     {
                         ViewBag.DisplayMessage = "Info";
                         ModelState.AddModelError("", "Error: " + ex.Message);
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
            
            return View(CategoryList);
        }

        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            object category = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    category  = db.Categories.Find(id);
                    if (category == null)
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
            return View(category);
        }

        // GET: Category/Create
        public ActionResult Add(int? id)
        {
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
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

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "CategoryID,CategoryName,CreatedBy,CreatedOn")] CategorySetUpViewModel catVM)
        {
            if (catVM.CategoryName != null)
            {
                try
                {
                    var addCategory = new Category
                    {
                        CategoryName = catVM.CategoryName,
                        CreatedBy = Session["username"].ToString(),
                        CreatedOn = DateTime.Now
                    };

                    db.Categories.Add(addCategory);
                    db.SaveChanges();
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Saved Successfully!");

                }
               catch(Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
               // return View(catVM);
            }

            return View(catVM);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            object category = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    category = db.Categories.Find(id);
                    if (category == null)
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
            
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName,CreatedBy,CreatedOn")] Category category)
        {
            if (category.CategoryName != null)
            {
                try
                {
                    var categoryList = (from d in db.Categories.Where(x => x.CategoryID == category.CategoryID) select d).FirstOrDefault();
                    if (categoryList != null)
                    {
                        categoryList.CategoryName = category.CategoryName;
                        categoryList.CreatedBy = Session["username"].ToString();
                        categoryList.CreatedOn = DateTime.Now;
                        db.SaveChanges();
                        ViewBag.DisplayMessage = "success";
                        ModelState.AddModelError("", "Saved Successfully!");
                    }
                }
                catch(Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
               // return RedirectToAction("Show");
            }
            else
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Enter Category Name!");
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            object categoryList = null;
            if (Session["Username"] != null)
            {
                
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                     categoryList = db.Categories.Find(id);
                    if (categoryList == null)
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
            return View(categoryList);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Category category = db.Categories.Find(id);
            try
            {
                db.Categories.Remove(category);
                db.SaveChanges();
                ViewBag.DisplayMessage = "success";
                ModelState.AddModelError("", "Deleted Successfully!");
            }
           catch(Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Error: " + ex.Message);
            }
            //return RedirectToAction("Show");
            return View(category);
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
