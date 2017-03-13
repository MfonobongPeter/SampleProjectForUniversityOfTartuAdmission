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
    public class LgasController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();

        // GET: Lgas
        public ActionResult Show()
        {
            //var lgas = db.Lgas.Include(l => l.State);
            //return View(lgas.ToList());
            object lgaList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    try
                    {
                        lgaList =
                                        (from l in db.Lgas
                                         join s in db.States
                                on l.StateID equals s.StateID

                                         select new LgaViewModel
                                         {
                                             LgaID = l.LgaID,
                                             StateID = l.StateID,
                                             StateName = s.StateName,
                                             LgaName = l.LgaName
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
           
            return View(lgaList);

        }

        // GET: Lgas/Details/5
        public ActionResult Details(int? id)
        {
            object lgaList = "";

            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    try
                    {
                        lgaList =
                                        (from l in db.Lgas
                                         join s in db.States
                                on l.StateID equals s.StateID
                                         where l.LgaID == id
                                         select new LgaViewModel
                                         {
                                             LgaID = l.LgaID,
                                             StateID = l.StateID,
                                             StateName = s.StateName,
                                             LgaName = l.LgaName
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
            
            return View(lgaList);
        }

        // GET: Lgas/Create
        public ActionResult Add()
        {
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
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

        // POST: Lgas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "LgaID,StateID,LgaName")] LgaViewModel lga)
        {
            try
            {
                var lgaAdd = new Lga
                {
                    StateID = Convert.ToInt16(lga.StateID.ToString()),
                    LgaID = Convert.ToInt16(lga.LgaID.ToString()),
                    LgaName= lga.LgaName
                };
                var isExist = (from l in db.Lgas.Where(x => x.LgaName == lga.LgaName) select l).FirstOrDefault();
                if(isExist == null)
                {
                    db.Lgas.Add(lgaAdd);
                    db.SaveChanges();
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("",  " Saved Successfully!");
                    LoadDropDownList();
                }
                else
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "LGA Name "+isExist.LgaName +" already exist, Please enter a new Lga!");
                    LoadDropDownList();
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Error: " + ex.Message);
            }
            return View(lga);
        }

        // GET: Lgas/Edit/5
        public ActionResult Edit(int? id)
        {
            object lgaList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    try
                    {
                        lgaList =
                            (from l in db.Lgas
                             join s in db.States
                             on l.StateID equals s.StateID
                             where l.LgaID == id
                             select new LgaViewModel
                             {
                                 LgaID = l.LgaID,
                                 StateID = l.StateID,
                                 StateName = s.StateName,
                                 LgaName = l.LgaName
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
            return View(lgaList);
        }

        // POST: Lgas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LgaID,StateID,LgaName")] LgaViewModel lga)
        {
           
            if (lga.LgaName != null)
            {
                try
                {
                    var lgaList = (from d in db.Lgas.Where(x => x.LgaID == lga.LgaID) select d).FirstOrDefault();
                    if (lgaList != null)
                    {
                        lgaList.LgaName = lga.LgaName;
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
            }
            else
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Enter Lga Name!");
            }
            return View(lga);
        }

        // GET: Lgas/Delete/5
        public ActionResult Delete(int? id)
        {
            object lgaList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    try
                    {
                        lgaList =
                            (from l in db.Lgas
                             join s in db.States
                             on l.StateID equals s.StateID
                             where l.LgaID == id
                             select new LgaViewModel
                             {
                                 LgaID = l.LgaID,
                                 StateID = l.StateID,
                                 StateName = s.StateName,
                                 LgaName = l.LgaName
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
            
            return View(lgaList);
        }

        // POST: Lgas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var remove = (from d in db.Lgas.Where(x => x.LgaID == id) select d).FirstOrDefault();
                if (remove !=null)
                {
                    db.Lgas.Remove(remove);
                    db.SaveChanges();
                    //ViewBag.DisplayMessage = "success";
                    //ModelState.AddModelError("", "Deleted Successfully!");
                }
               
            }
            catch (Exception ex)
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
                var fkState = (from st in db.States
                                  select new
                                  {
                                      st.StateID,
                                      st.StateName
                                  }
                ).ToList();
                var items = fkState.Select(t => new SelectListItem
                {
                    Text = t.StateName,
                    Value = t.StateID.ToString()
                }).ToList();

                ViewBag.fkStateList = items;
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
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
