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
    public class StatesController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();

        // GET: States
        public ActionResult Show()
        {
            object stateList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    try
                    {
                        stateList =
                                        (from u in db.States

                                         select new StateViewModel
                                         {
                                             StateID = u.StateID,
                                             StateName = u.StateName,
                                             ShippingFee = u.ShippingFee
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
            return View(stateList);
        }

        // GET: States/Details/5
        public ActionResult Details(int? id)
        {
            State state = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    state = db.States.Find(id);
                    if (state == null)
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
            
            return View(state);
        }

        // GET: States/Create
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

        // POST: States/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "StateID,StateName,ShippingFee")] StateViewModel state)
        {
            try
            {
                var isExist = (from l in db.States.Where(x => x.StateName == state.StateName) select l).FirstOrDefault();
                if (isExist == null)
                {
                    if (state.StateName != null)
                    {
                        var addState = new State
                        {
                            StateName = state.StateName,
                            ShippingFee = state.ShippingFee
                        };
                        db.States.Add(addState);
                        db.SaveChanges();
                        ViewBag.DisplayMessage = "success";
                        ModelState.AddModelError("", "Saved Successfully!");
                    }
                    else
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", "Enter State name");
                    }
                }
                else
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "State Name " + isExist.StateName + " already exist, Please enter a new state!");
                    //LoadDropDownList();
                }
            }
            catch(Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Error: " + ex.Message);
            }

            return View(state);
        }

        // GET: States/Edit/5
        public ActionResult Edit(int? id)
        {
            State state = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    state = db.States.Find(id);
                    if (state == null)
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
           
            return View(state);
        }

        // POST: States/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StateID,StateName,ShippingFee")] State state)
        {
           
            if (state.StateName != null)
            {
                try
                {
                    var stateList = (from d in db.States.Where(x => x.StateID == state.StateID) select d).FirstOrDefault();
                    if (stateList != null)
                    {
                        stateList.StateName = state.StateName;
                        stateList.ShippingFee = state.ShippingFee;
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
            return View(state);
        }

        // GET: States/Delete/5
        public ActionResult Delete(int? id)
        {
            State state = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    state = db.States.Find(id);
                    if (state == null)
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
            
            return View(state);
        }

        // POST: States/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                State state = db.States.Find(id);
                db.States.Remove(state);
                db.SaveChanges();
                //ViewBag.DisplayMessage = "success";
                //ModelState.AddModelError("", "Deleted Successfully!");
            }
            catch(Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Error: " + ex.Message);
            }
            return RedirectToAction("Show");
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
