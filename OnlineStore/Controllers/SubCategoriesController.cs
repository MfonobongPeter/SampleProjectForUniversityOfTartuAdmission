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
    public class SubCategoriesController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();

        // GET: SubCategories
        public ActionResult Show()
        {
            object subCategoryList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    try
                    {
                        subCategoryList =
                                        (from u in db.SubCategories
                                         join ct in db.Categories
                                on u.CategoryID equals ct.CategoryID

                                         select new SubCategoryViewModel
                                         {
                                             SubCategoryID = u.SubCategoryID,
                                             CategoryName = ct.CategoryName,
                                             SubCategoryName = u.SubCategoryName,
                                             CreatedBy = u.CreatedBy,
                                             CreatedOn = u.CreatedOn
                                         }).ToList();
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
            
            return View(subCategoryList);

            //return View(subCategories);
        }

        // GET: SubCategories/Details/5
        public ActionResult Details(int? id)
        {         
            object subCategoryList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    try
                    {
                        subCategoryList =
                                        (from u in db.SubCategories
                                         join ct in db.Categories
                                on u.CategoryID equals ct.CategoryID
                                         where u.SubCategoryID == id
                                         select new SubCategoryViewModel
                                         {
                                             SubCategoryID = u.SubCategoryID,
                                             CategoryName = ct.CategoryName,
                                             SubCategoryName = u.SubCategoryName,
                                             CreatedBy = u.CreatedBy,
                                             CreatedOn = u.CreatedOn
                                         }).FirstOrDefault();
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
            
            return View(subCategoryList);
        }

        // GET: SubCategories/Create
        public ActionResult Add()
        {
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    LoadDropDownList();
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

        // POST: SubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "SubCategoryID,CategoryID,CreatedBy,CreatedOn,SubCategoryName")] SubCategoryViewModel subCat)
        {
           
            try
            {
                var subCategoryAdd = new SubCategory
                {
                    CategoryID = Convert.ToInt16(subCat.CategoryID.ToString()),
                    SubCategoryName = subCat.SubCategoryName,
                    CreatedBy = Session["username"].ToString(),
                    CreatedOn = DateTime.Now,
                };

                var isExist = (from l in db.SubCategories.Where(x => x.SubCategoryName == subCat.SubCategoryName) select l).FirstOrDefault();
                if (isExist == null)
                {
                    db.SubCategories.Add(subCategoryAdd);
                    db.SaveChanges();

                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Saved Successfully!");
                    LoadDropDownList();
                }
                else
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Sub-Category Name " + isExist.SubCategoryName + " already exist, Please enter a new Sub-Category Name!");
                    LoadDropDownList();
                }
            }
            catch(Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Error: " + ex.Message);
            }
            return View();
        }

        // GET: SubCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            object subCategoryList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    try
                    {
                        subCategoryList =
                                        (from u in db.SubCategories
                                         join ct in db.Categories
                                on u.CategoryID equals ct.CategoryID
                                         where u.SubCategoryID == id
                                         select new SubCategoryViewModel
                                         {
                                             SubCategoryID = u.SubCategoryID,
                                             CategoryName = ct.CategoryName,
                                             SubCategoryName = u.SubCategoryName,
                                             CreatedBy = u.CreatedBy,
                                             CreatedOn = u.CreatedOn
                                         }).FirstOrDefault();
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
           
            return View(subCategoryList);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubCategoryID,CategoryID,CreatedBy,CreatedOn,SubCategoryName")] SubCategoryViewModel subCat)
        {
            if (subCat.SubCategoryName != null)
            {
                try
                {
                    var subCategoryList = (from d in db.SubCategories.Where(x => x.SubCategoryID == subCat.SubCategoryID) select d).FirstOrDefault();
                    if (subCategoryList != null)
                    {
                        subCategoryList.SubCategoryName = subCat.SubCategoryName;
                        subCategoryList.CreatedBy = Session["username"].ToString();
                        subCategoryList.CreatedOn = DateTime.Now;
                        db.SaveChanges();
                        ViewBag.DisplayMessage = "success";
                        ModelState.AddModelError("", "Updated Successfully!");
                    }
                }
                catch(Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
                //return RedirectToAction("Show");
            }
            else
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Enter Category Name!");
            }
            return View(subCat);
        }

        // GET: SubCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            object subCategoryList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    try
                    {
                        subCategoryList =
                                        (from u in db.SubCategories
                                         join ct in db.Categories
                                on u.CategoryID equals ct.CategoryID
                                         where u.SubCategoryID == id
                                         select new SubCategoryViewModel
                                         {
                                             SubCategoryID = u.SubCategoryID,
                                             CategoryName = ct.CategoryName,
                                             SubCategoryName = u.SubCategoryName,
                                             CreatedBy = u.CreatedBy,
                                             CreatedOn = u.CreatedOn
                                         }).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", ex.Message);
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
            return View(subCategoryList);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var del = (from f in db.SubCategories.Where(x => x.SubCategoryID == id) select f).FirstOrDefault();
            try
            {
               // SubCategory subCategory = db.SubCategories.Find(id);
               
                db.SubCategories.Remove(del);
                db.SaveChanges();
                //ViewBag.DisplayMessage = "success";
                //ModelState.AddModelError("", "Deleted Successfully!");
            }
            catch(Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return RedirectToAction("Show");
        }

       
        private void LoadDropDownList()
        {
            try
            {
                //security dropdown starts here
                var fkCategory = (from ctg in db.Categories
                                  select new
                                  {
                                      ctg.CategoryID,
                                      ctg.CategoryName
                                  }
                ).ToList();
                var items = fkCategory.Select(t => new SelectListItem
                {
                    Text = t.CategoryName,
                    Value = t.CategoryID.ToString()
                }).ToList();

                ViewBag.fkCategoryList = items;               
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Error: " + ex.Message);
            }

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
