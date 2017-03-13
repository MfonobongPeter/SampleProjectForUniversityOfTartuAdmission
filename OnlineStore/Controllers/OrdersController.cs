using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    public class OrdersController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();
        public ActionResult Show()
        {
            //var isDeleted = false;
            object OrderList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    try
                    {
                        OrderList =
                                        (from o in db.Orders                                        
                                         join u in db.Users on o.UserID equals u.UserID
                                         where o.IsDeleted == false
                                         select new OrdersViewModel
                                         {
                                             OrderID = o.OrderID,
                                             UserID = o.UserID,
                                             ProductID = o.ProductID,
                                             Amount = o.OrderAmount,
                                             Email = u.UserEmail,
                                             Phone = u.UserPhone,
                                             TransactionStatus = o.OrderTransactionStatus,
                                             DeliveryStatus = o.OrderDeliveryStatus,
                                             OrderType = o.OrderType,
                                             OrderDate = o.OrderDate
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
            

            return View(OrderList);
        }

        public ActionResult ShowDeleted()
        {
            //var isDeleted = false;
            object OrderList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    try
                    {
                        OrderList =
                                        (from o in db.Orders
                                         join u in db.Users
                                on o.UserID equals u.UserID
                                         where o.IsDeleted == true
                                         select new OrdersViewModel
                                         {
                                             OrderID = o.OrderID,
                                             UserID = o.UserID,
                                             ProductID = o.ProductID,
                                             Amount = o.OrderAmount,
                                             Email = u.UserEmail,
                                             Phone = u.UserPhone,
                                             TransactionStatus = o.OrderTransactionStatus,
                                             DeliveryStatus = o.OrderDeliveryStatus,
                                             OrderType = o.OrderType,
                                             OrderDate = o.OrderDate
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
            
            return View(OrderList);

        }

        public ActionResult Delete(int? id)
        {
            object OrderList = "";

            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    try
                    {
                        OrderList =
                                        (from l in db.Orders
                                         join u in db.Users on
                                         l.UserID equals u.UserID
                                         join c in db.Countries on u.CountryID equals c.CountryID
                                         where l.OrderID == id
                                         select new OrdersViewModel
                                         {
                                             OrderID = l.OrderID,
                                             UserID = l.UserID,
                                             ProductID = l.ProductID,
                                             Amount = l.OrderAmount,
                                             Phone = u.UserPhone,
                                             Name = u.UserFirstName + " " + u.UserLastName,
                                             Email = u.UserEmail,
                                             Address = u.UserBillingAddress,
                                             Country = c.CountryName,
                                             State = u.UserState,
                                             City = u.UserCity,
                                             TransactionNumber = l.OrderTransactionNumber,
                                             TransactionStatus = l.OrderTransactionStatus,
                                             DeliveryStatus = l.OrderDeliveryStatus,
                                             OrderType = l.OrderType,
                                             OrderDate = l.OrderDate
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
            return View(OrderList);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var resetOrderIsDeleted = (from o in db.Orders.Where(x => x.OrderID == id) select o).FirstOrDefault();
                if (resetOrderIsDeleted != null)
                {
                    var resetOrderDetailIsDeleted = (from od in db.OrderDetails.Where(x => x.OrderID == id) select od).FirstOrDefault();
                    if (resetOrderDetailIsDeleted != null)
                    {
                        resetOrderDetailIsDeleted.IsDeleted = true;
                        resetOrderIsDeleted.IsDeleted = true;
                        db.SaveChanges();
                        //ViewBag.DisplayMessage = "success";
                        //ModelState.AddModelError("", "Deleted Successfully!");
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return RedirectToAction("Show");
        }

        
        public ActionResult Details(int? id)
        {
            object OrderList = "";
            
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    try
                    {
                        OrderList =
                                        (from l in db.Orders
                                         join u in db.Users on
                                         l.UserID equals u.UserID
                                         join c in db.Countries on u.CountryID equals c.CountryID
                                         join p in db.Products on l.ProductID equals p.ProductID
                                         where l.OrderID == id
                                         select new OrderViewDetailViewModel
                                         {
                                             OrderID = l.OrderID,
                                             Ordername = u.UserFirstName + " "+u.UserLastName,
                                             ProductName = p.ProductName,
                                             Amount = l.OrderAmount,
                                             Phone = u.UserPhone,
                                             Name = u.UserFirstName + " " +u.UserLastName,
                                             Email = u.UserEmail,
                                             Address = u.UserAddress1,
                                             Country = c.CountryName,
                                             State = u.UserState,
                                             City = u.UserCity,
                                             TransactionNumber = l.OrderTransactionNumber,
                                             TransactionStatus = l.OrderTransactionStatus,
                                             DeliveryStatus = l.OrderDeliveryStatus,
                                             OrderType = l.OrderType,
                                             OrderDate = l.OrderDate
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
            

            return View(OrderList);
        }

        public ActionResult Edit(int? id)
        {
            //object OrderList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    try
                    {
                      var  OrderList = (from l in db.Orders 
                                     join d in db.OrderDeliveryStatus on l.OrderDeliveryStatus equals d.DeliveryStatus
                                     where l.OrderID == id
                                         select new
                                         {
                                            d.RowID
                                         }).FirstOrDefault();


                      int deliveryStatusID = OrderList.RowID;
                      ViewBag.OrderDeliveryStatus = new SelectList(db.OrderDeliveryStatus, "RowID", "DeliveryStatus", deliveryStatusID);
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
            
            return View();
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = " DeliveryStatus")] OrderEditViewModel edvm, int id)
        {
            var getDeliveryStatusName = (from d in db.OrderDeliveryStatus.Where(d => d.RowID == edvm.DeliveryStatus)
                                     select new
                                         {
                                             d.DeliveryStatus
                                         }).FirstOrDefault();

            if(getDeliveryStatusName != null)
            {
                var getDeliveryStatus = (from d in db.Orders.Where(d => d.OrderID == id)
                                         select d).FirstOrDefault();
                if(getDeliveryStatus !=null)
                {
                    try
                    {
                        getDeliveryStatus.OrderDeliveryStatus = getDeliveryStatusName.DeliveryStatus;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", "Error: " + ex.Message);
                    }   
                }
                           
            }
            return RedirectToAction("Show");
        }


        public ActionResult UndoDelete(int? id)
        {
            object OrderList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    try
                    {
                        OrderList =
                                        (from l in db.Orders
                                         join u in db.Users on
                                         l.UserID equals u.UserID
                                         join c in db.Countries on u.CountryID equals c.CountryID
                                         join p in db.Products on l.ProductID equals p.ProductID
                                         where l.OrderID == id
                                         select new OrderViewDetailViewModel
                                         {
                                             OrderID = l.OrderID,
                                             Ordername = u.UserFirstName + " " + u.UserLastName,
                                             ProductName = p.ProductName,
                                             Amount = l.OrderAmount,
                                             Phone = u.UserPhone,
                                             Name = u.UserFirstName + " " +u.UserLastName,
                                             Email = u.UserEmail,
                                             Address = u.UserBillingAddress,
                                             Country = c.CountryName,
                                             State = u.UserState,
                                             City = u.UserCity,
                                             TransactionNumber = l.OrderTransactionNumber,
                                             TransactionStatus = l.OrderTransactionStatus,
                                             DeliveryStatus = l.OrderDeliveryStatus,
                                             OrderType = l.OrderType,
                                             OrderDate = l.OrderDate
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
            
            return View(OrderList);
        }



        [HttpPost, ActionName("UndoDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult UndoDeleteConfirmed(int id)
        {
            try
            {
                var resetOrderIsDeleted = (from o in db.Orders.Where(x => x.OrderID == id) select o).FirstOrDefault();
                if (resetOrderIsDeleted != null)
                {
                    var resetOrderDetailIsDeleted = (from od in db.OrderDetails.Where(x => x.OrderID == id) select od).FirstOrDefault();
                    if (resetOrderDetailIsDeleted != null)
                    {
                        resetOrderDetailIsDeleted.IsDeleted = false;
                        resetOrderIsDeleted.IsDeleted = false;
                        db.SaveChanges();
                        //ViewBag.DisplayMessage = "success";
                        //ModelState.AddModelError("", "Deleted Successfully!");
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return RedirectToAction("ShowDeleted");
        }


        public ActionResult ViewOrderReport()
        {
            object getOrder = null;
            if (Session["Username"] != null)
            {

                //long userId = int.Parse(Session["userID"].ToString());
                try
                {
                    getOrder = (from o in db.Orders.Where(x => x.IsDeleted == false)
                                select new FEOrderViewModel
                                {
                                    OrderID = o.OrderID,
                                    TransID = o.OrderTransactionNumber.Substring(0, 8),
                                    UrlTransID = o.OrderTransactionNumber,
                                    TotalAmount = o.OrderAmount,
                                    Status = o.OrderDeliveryStatus,
                                    Date = o.OrderDate
                                }).ToList();
                }
                catch (Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(getOrder);
            //return View();
        }

        public ActionResult ViewOrderDetailsReport(int id, string name)
        {
            //var transID4 = Request.QueryString["id"].ToString();
            FECombineOrderDetailsViewModel model = new FECombineOrderDetailsViewModel();
            if (Session["username"] != null)
            {
                try
                {
                    var orderDetails = (from o in db.Orders
                                        join od in db.OrderDetails on o.OrderID equals od.OrderID
                                        join p in db.Products on o.ProductID equals p.ProductID
                                        join u in db.Users on o.UserID equals u.UserID
                                        join c in db.Countries on u.CountryID equals c.CountryID
                                        where (o.OrderTransactionNumber == name)
                                        select new FEOrderDetailsViewModel
                                        {
                                            ProductID = o.ProductID,
                                            OrderID = o.OrderID,
                                            TransactionID = o.OrderTransactionNumber,
                                            ProductName = od.ProductName,
                                            ProductSku = o.ProductModelNumber,
                                            ProductFrontViewThumb = p.ProductFrontViewThumbnail.Substring(1),
                                            ProductQuantity = o.OrderQuantity,
                                            ProductUnitPrice = o.OrderUnitPrice,
                                            ProductTotalPrice = o.OrderQuantity * o.OrderUnitPrice,
                                        }).ToList();
                    model.List = orderDetails;
                }
                catch (Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", ex.Message);
                }

                try
                {
                    var orderDetailsItems = (from o in db.Orders
                                             join od in db.OrderDetails on o.OrderID equals od.OrderID
                                             //join p in db.Products on o.ProductID equals p.ProductID
                                             join u in db.Users on o.UserID equals u.UserID
                                             join c in db.Countries on u.UserBillingCountry equals c.CountryID.ToString()
                                             join cd in db.Countries on u.CountryID equals cd.CountryID
                                             where (o.OrderTransactionNumber == name)
                                             select new FEOrderDetailsItemsViewModel
                                             {

                                                 OrderID = o.OrderID,
                                                 TransIDShort = o.OrderTransactionNumber.Substring(0, 8).ToUpper(),
                                                 TransactionID = o.OrderTransactionNumber,
                                                 OrderDate = o.OrderDate,
                                                 OrderDeliveryStatus = o.OrderDeliveryStatus,
                                                 UserFirstName = u.UserFirstName,
                                                 UserLastName = u.UserLastName,
                                                 UserCountry = cd.CountryName,
                                                 UserState = u.UserState,
                                                 UserCity = u.UserCity,
                                                 UserPhoneNumber = u.UserPhone,
                                                 UserAddress1 = u.UserAddress1,
                                                 UserBillingAddress = u.UserBillingAddress,
                                                 UserBillingCity = u.UserBillingCity,
                                                 UserBillingCountry = c.CountryName,
                                                 UserBillingState = u.UserBillingState,
                                                 OrderDeliveryMethod = o.OrderDeliveryOption,
                                                 OrderPaymentMethod = o.OrderPaymentOption,
                                                 UserBillingFirstName = u.UserBillingFirstName,
                                                 UserBillingLastName = u.UserBillingLastName,
                                                 UserBillingPhone = u.UserBillingPhone
                                             }).FirstOrDefault();
                    model.OrderItems = orderDetailsItems;

                    ViewBag.CartTotal = GetTotalPriceForOrderSummary(name);
                    if (ViewBag.CartTotal > 0)
                    {
                        ViewBag.CartTotals = GetTotalPriceForOrderSummary(name);
                        ShippingAndHandling sh = GetShippingAndHandlingAmount();
                        ViewBag.ShippingAndHandling = sh.ShippingAndHandlingCharges;
                        //ViewBag.ShippingAndHandling = 1000m;
                        ViewBag.Tax = sh.Vat;
                        ViewBag.Discount = 0.00m;
                        ViewBag.OrderSummaryTotal = ViewBag.CartTotal + ViewBag.ShippingAndHandling + ViewBag.Tax;
                    }
                    else
                    {
                        ViewBag.ShippingAndHandling = 0.00m;
                        ViewBag.Tax = 0.00m;
                        ViewBag.Discount = 0.00m;
                        ViewBag.OrderSummaryTotal = 0.00m;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", ex.Message);
                }
                //}
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        public decimal GetTotalPriceForOrderSummary(string transNum)
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = 0.00m;
            try
            {
                HttpCookie anonymousID = Request.Cookies["CartSessionID"];
                if (anonymousID != null)
                {
                    total = (from orderItems in db.Orders
                             where orderItems.OrderTransactionNumber == transNum
                             select (int?)orderItems.OrderQuantity * orderItems.OrderUnitPrice).Sum();
                }
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return total ?? decimal.Zero;
        }

        public ShippingAndHandling GetShippingAndHandlingAmount()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            ShippingAndHandling ShippingAndHandlingAmount = null;
            ShippingAndHandlingAmount = (from sh in db.ShippingAndHandlings select sh).FirstOrDefault();

            return ShippingAndHandlingAmount;
        }
        public ActionResult PrintOrderReport()
        {
            return View();
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