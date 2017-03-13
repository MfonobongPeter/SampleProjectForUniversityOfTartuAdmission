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
    
    public class OrderDetailsController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();
        // GET: OrderDetails
        public ActionResult Show()
        {
            object orderDetail = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    try
                    {
                        orderDetail = (from p in db.OrderDetails.Where(x => x.IsDeleted == false)
                                       select new OrderDetailsViewModel
                                       {
                                           OrderDetailID = p.OrderDetailID,
                                           OrderID = p.OrderID,
                                           ProductID = p.ProductID,
                                           ProductName = p.ProductName,
                                           ProductSKU = p.ProductSKU,
                                           Quantity = p.ProductQuantity,
                                           ProductPrice = p.ProductPrice,
                                           OrderDate = p.CreatedOn,
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
            
            return View(orderDetail);
        }


        public ActionResult Details(int? id)
        {
            object OrderDetailList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    try
                    {
                        OrderDetailList =
                                        (from l in db.OrderDetails
                                         where l.OrderDetailID == id
                                         select new OrderDetailsViewModel
                                         {
                                             OrderDetailID = l.OrderDetailID,
                                             OrderID = l.OrderID,
                                             ProductID = l.ProductID,
                                             ProductName = l.ProductName,
                                             ProductPrice = l.ProductPrice,
                                             ProductSKU = l.ProductSKU,
                                             Quantity = l.ProductQuantity,
                                             OrderDate = l.CreatedOn
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
            
            return View(OrderDetailList);
        }


        //public ActionResult Delete(int? id)
        //{
        //    object OrderDetailList = "";
        //    try
        //    {
        //        OrderDetailList =
        //                        (from l in db.OrderDetails
        //                         where l.OrderDetailID == id
        //                         select new OrderDetailsViewModel
        //                         {
        //                             OrderDetailID = l.OrderDetailID,
        //                             OrderID = l.OrderID,
        //                             ProductID = l.ProductID,
        //                             ProductName = l.ProductName,
        //                             ProductPrice = l.ProductPrice,
        //                             ProductSKU = l.ProductSKU,
        //                             Quantity = l.ProductQuantity,
        //                             OrderDate = l.CreatedOn
        //                         }).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.DisplayMessage = "Info";
        //        ModelState.AddModelError("", "Error: " + ex.Message);
        //    }

        //    return View(OrderDetailList);
        //}

        

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    try
        //    {
        //        var resetIsDeleted = (from od in db.OrderDetails.Where(x => x.OrderDetailID == id) select od).FirstOrDefault();
        //        if (resetIsDeleted != null)
        //        {

        //            resetIsDeleted.IsDeleted = true;
        //            db.SaveChanges();
        //            //ViewBag.DisplayMessage = "success";
        //            //ModelState.AddModelError("", "Deleted Successfully!");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.DisplayMessage = "Info";
        //        ModelState.AddModelError("", ex.Message);
        //    }
        //    return RedirectToAction("Show");
        //}

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