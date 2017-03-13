using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using System.Data.Entity.Validation;
using System.ComponentModel;
using System.Net.Http;
//using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Configuration;
using System.Data.Entity.SqlServer;


namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();
        string smsNotificationNumbers = ConfigurationManager.AppSettings["SmsNotificationNumbers"].ToString(); 
        public ActionResult Index()
        {
            ViewBag.Title = "welcome to glorious evidence online store";
            ViewBag.MetaKeywords = "spa equipments, beauty equipments, waxing equipments, pedicure and manicure equipments, and hair dressing equipments";
            ViewBag.MetaDescription = "glorious evidence online store-for all for your spa, beauty, waxing, pedicure and manicure and hair dressing equipments";
            //var anonymousID = Request.AnonymousID;
            HttpCookie anonymousID = Request.Cookies["CartSessionID"];
           
            if (anonymousID != null)
            {
                decimal totalPrice = GetTotalPrice();
                int cartItems = GetTotalQuantity();
                try
                {
                    var model = (from m in db.ShoppingCarts.Where(x => x.CartSessionID == anonymousID.Value.ToString()) select m).FirstOrDefault();
                    if (model != null)
                    {
                        var cart = new ShoppingCart
                        {
                            CartSessionID = Request.AnonymousID,
                            ProductID = model.ProductID,
                            ProductSku = model.ProductSku,
                            ProductPrice = model.ProductPrice,
                            ProductQuantity = cartItems,
                            ProductName = model.ProductName,
                            FrontViewThumbNail = model.FrontViewThumbNail,
                            ProductTotalPrice = totalPrice,
                            DateCreated = DateTime.Now
                        };
                        Session["cart"] = cart;
                    }
                }
                catch(Exception)
                {

                }
            }
           
            object  productList = null;
            try
            {
                //ViewBag.Current = "Scheduler";
                productList = (from p in db.Products.OrderByDescending(x => x.ProductCreatedOn)
                               where p.ProductQuantity > 0
                                   select new
                                      FEDisplayProductsOnHomePageViewModel
                                   {
                                       ProductID = p.ProductID,
                                       ProductSideViewThumbnail = p.ProductSideViewThumbnail,
                                       ProductFrontViewThumbnail = p.ProductFrontViewThumbnail,
                                       ProductDescription = p.ProductshortDesc,
                                       ProductPrice = p.ProductUnitPrice,
                                       ProductLocation = p.ProductLocation,
                                       ProductName = p.ProductName,
                                       ProductQuantity = p.ProductQuantity,    
                                       ProductIsNew = p.ProductIsNew
                                   }).Take(80);
            }
            catch(Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return View(productList);
            //return View();
        }

        public ActionResult cLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult cLogin(UserLoginViewModel login)
        {
            try
            {
                var PwdHashing = new PasswordHashing();
                string username = login.UserEmail;
                string password = login.UserPassword;
                //int role = 0;
                var chkUser = (from l in db.Users
                               where l.UserEmail == username && l.IsDeleted == false && l.IsActivated == true
                               select l).FirstOrDefault();
                if (chkUser != null)
                {
                    try
                    {
                        var decriptPwd = PwdHashing.Decrypt(chkUser.UserPassword);
                        if (chkUser.UserEmail == username && decriptPwd == password)
                        {
                            Session["username"] = chkUser.UserEmail;
                            Session["password"] = chkUser.UserPassword;
                            Session["firstName"] = chkUser.UserFirstName;
                            Session["userID"] = chkUser.UserID;
                            return RedirectToAction("Dashboard", "Home");
                        }
                        else
                        {
                            ViewBag.DisplayMessage = "Info";
                            ModelState.AddModelError("", "Email or Password not valid!");
                        }
                    }
                    catch (Exception)
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", "Database Password not encripted!");
                    }

                }
                else
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "User does not exist!");
                }
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return View();
            
        }

        [HttpPost]
        public ActionResult PartialViewLoginOnLayoutPage(string emailmodal, string passwordmodal)
        {
            var model = new UserLoginViewModel
            {
                UserPassword = passwordmodal,
                UserEmail = emailmodal,
            };
            try
            {
                var PwdHashing = new PasswordHashing();
                string username = emailmodal;
                string password = passwordmodal;
                //int role = 0;
                var chkUser = (from l in db.Users
                               where l.UserEmail == username && l.IsDeleted == false && l.IsActivated == true
                               select l).FirstOrDefault();

                if (chkUser != null)
                {
                    try
                    {
                        var decriptPwd = PwdHashing.Decrypt(chkUser.UserPassword);
                        if (chkUser.UserEmail == emailmodal && decriptPwd == passwordmodal)
                        {
                            Session["username"] = chkUser.UserEmail;
                            Session["password"] = chkUser.UserPassword;
                            Session["firstName"] = chkUser.UserFirstName;
                            Session["userID"] = chkUser.UserID;
                            return RedirectToAction("Dashboard", "Home");
                        }
                        else
                        {
                            ViewBag.DisplayMessage = "Inf";
                            ModelState.AddModelError("", "Email or Password not valid!");
                            return PartialView("_CustomerLogin");
                        }
                    }
                    catch (Exception)
                    {
                        ViewBag.DisplayMessage = "Inf";
                        ModelState.AddModelError("", "Database Password not encripted!");
                        return PartialView("_CustomerLogin");
                    }

                }
                else
                {
                    ViewBag.DisplayMessage = "Inf";
                    ModelState.AddModelError("", "User does not exist!");
                    return PartialView("_CustomerLogin");
                }
            }
            catch (Exception)
            {
                return View("index");
            }
            
        }


        public ActionResult Products()
        {
            try
            {
                var catName = "";
                var getSubCatName = (from c in db.Products
                                     select new
                                     {
                                         c.ProductName
                                     }).ToList();
                int count = 10; // getting 10 random keywords from database
                var subCat = getSubCatName.OrderBy(t => Guid.NewGuid())
                        .Take(count);
                foreach (var d in subCat)
                {
                    catName = d.ProductName.ToLower() + "," + catName.ToLower();
                }
                var getit = catName;
                ViewBag.Title = "glorious evidence online store product list";
                ViewBag.MetaKeywords = getit;
                ViewBag.MetaDescription = "list of products in our showroom, we have different product categories.";
            }
            catch (Exception)
            {

            }
            object productList = null;
            try
            {
                //ViewBag.Current = "Scheduler";
                productList = (from p in db.Products.OrderByDescending(x => x.ProductCreatedOn)
                               where p.ProductQuantity > 0
                               select new
                                  FEDisplayProductsOnProductPageViewModel
                               {
                                   ProductID = p.ProductID,
                                   ProductSideViewThumbnail = p.ProductSideViewThumbnail,
                                   ProductFrontViewThumbnail = p.ProductFrontViewThumbnail,
                                   ProductDescription = p.ProductshortDesc,
                                   ProductPrice = p.ProductUnitPrice,
                                   ProductLocation = p.ProductLocation,
                                   ProductName = p.ProductName,
                                   ProductQuantity = p.ProductQuantity,
                                   ProductIsNew = p.ProductIsNew,
                                   ProductSKU = p.ProductSKU
                               }).Take(80);
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return View(productList);
            //return View();
        }

        public object LoadProductOnProductPage()
        {
            object productList = null;
            try
            {
                //ViewBag.Current = "Scheduler";
                productList = (from p in db.Products.OrderByDescending(x => x.ProductCreatedOn)
                               where p.ProductQuantity > 0
                               select new
                                  FEDisplayProductsOnProductPageViewModel
                               {
                                   ProductID = p.ProductID,
                                   ProductSideViewThumbnail = p.ProductSideViewThumbnail,
                                   ProductFrontViewThumbnail = p.ProductFrontViewThumbnail,
                                   ProductDescription = p.ProductshortDesc,
                                   ProductPrice = p.ProductUnitPrice,
                                   ProductLocation = p.ProductLocation,
                                   ProductName = p.ProductName,
                                   ProductQuantity = p.ProductQuantity,
                                   ProductIsNew = p.ProductIsNew
                               }).Take(80);
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return productList; 
        }

        public ActionResult About()
        {
            ViewBag.MetaKeywords = "about-glorious evidence Ltd, mission statement, vision statement, duly incorporated in Nigeria, and is 100% indigenous company ";
            ViewBag.MetaDescription = "glorious evidence online store About";

           

            ////var getcount = GetTotalQuantity();
            ////var sfee = GetTotalPriceForProductDeliveryPrice();
            ////var shippingFee = (sfee / getcount).ToString("N2");
            //OnlineStoreEntities db = new OnlineStoreEntities();
            //DateTime b = DateTime.Now;
            //var isNewUpdate = (from p in db.Products
            //                   //where p.ProductCreatedOn.Equals(b.Date)
            //                   select p).ToList();
            //if (isNewUpdate != null)
            //{
            //    foreach(var prod in isNewUpdate)
            //    {
            //        DateTime? a = prod.ProductCreatedOn;
            //        if (a.Value.Date.Equals(b.Date))
            //        {
            //            prod.ProductIsNew = true;
            //        }
            //        else
            //        {
            //            prod.ProductIsNew = false;
            //        }
            //    }
            //    db.SaveChanges();
            //}
                
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.MetaKeywords = "Contact-form";
            ViewBag.MetaDescription = "glorious evidence online store contact form";
            return View();
        }

        [HttpPost]
        public ActionResult Contact([Bind(Include = "Email,Firstname,Message,Lastname,Subject")] FEContactViewModel conMsg)
        {
            try
            {
                EmailNotification.ProcessEmailForContactForm(conMsg.Email, conMsg.Firstname, conMsg.Message, conMsg.Lastname, conMsg.Subject);
                ViewBag.DisplayMessage = "success";
                ModelState.AddModelError("", "Feedback sent Successfully!");
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "success";
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public ActionResult ProductDetails(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("index", "home");
            }
            var model = new FECombineViewModel();
            try
            {
                var getProdDetail = (from d in db.Products
                                     join c in db.Categories on d.CategoryID equals c.CategoryID
                                     join s in db.SubCategories on d.SubCategoryID equals s.SubCategoryID
                                     where (d.ProductID == id)
                                     select new
                                     {
                                         d.ProductshortDesc,
                                         d.ProductName,
                                         s.SubCategoryName,
                                         c.CategoryName
                                     }).FirstOrDefault();

                if (getProdDetail != null)
                {
                    string catName = getProdDetail.CategoryName;
                    string subCatName = getProdDetail.SubCategoryName;
                    string prodName = getProdDetail.ProductName;
                    string shortDesc = getProdDetail.ProductshortDesc;
                    string concatKeywords = prodName + "," + catName + "," + subCatName;

                    ViewBag.Title = prodName.ToLower();
                    ViewBag.MetaKeywords = concatKeywords.ToLower();
                    ViewBag.MetaDescription = shortDesc.ToLower();
                }
                model = LoadProducts(id);
                Session["id"] = id;
            }
            catch (Exception ex)
            {
                model = LoadProducts(id);
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            model = LoadProducts(id);
            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Productdetails(FECombineViewModel feCombne, int id, string command)
        {
            //var feCombne = new FECombineViewModel();
            var model = (from m in db.Products.Where(x => x.ProductID == id) select m).FirstOrDefault();

            if(model !=null)
             {
                
                 if (command == "Addtocart")
                 {
                    
                     try
                     {
                         if (model != null)
                         {                           
                                 var model2 = (from m in db.ShoppingCarts.Where(x => x.ProductID == model.ProductID) select m).FirstOrDefault();
                                 if (model2 == null)
                                 {
                                     var cart = InsertIntoShoppingCart(model);
                                     //var cartModel = new ShoppingCart();                                

                                     Session["cart"] = cart;
                                     var cartItems = (ShoppingCart)Session["cart"];
                                     cartItems.ProductQuantity = GetTotalQuantity();
                                 }
                                 else
                                 {
                                     HttpCookie myCookie = HttpContext.Request.Cookies["CartSessionID"];
                                     HttpCookie CartSessionID = null;
                                     var newCookies = Guid.NewGuid();
                                     if (myCookie != null)
                                     {
                                         CartSessionID = new HttpCookie("CartSessionID", myCookie.Value);
                                         CartSessionID.Expires = DateTime.Now.AddDays(10);
                                         HttpContext.Response.SetCookie(CartSessionID);
                                     }
                                     else
                                     {
                                         CartSessionID = new HttpCookie("CartSessionID", newCookies.ToString());
                                         CartSessionID.Expires = DateTime.Now.AddDays(10);
                                         HttpContext.Response.Cookies.Add(CartSessionID);
                                     }

                                     var quanttity = model2.ProductQuantity + 1;
                                     var cartPrice = model2.ProductPrice;
                                     model2.CartSessionID = CartSessionID.Value.ToString();
                                     model2.ProductID = model.ProductID;
                                     model2.ProductSku = model.ProductSKU;
                                     model2.ProductPrice = model.ProductUnitPrice;
                                     model2.ProductQuantity = quanttity;
                                     model2.ProductName = model.ProductName;
                                     model2.FrontViewThumbNail = model.ProductSideViewThumbnail;
                                     model2.ProductTotalPrice = (quanttity) * (cartPrice);
                                     // model2.UserName = Session["username"].ToString();
                                     model2.DateCreated = DateTime.Now;

                                     db.SaveChanges();
                                     Session["cart"] = model2;
                                     var cartItems = (ShoppingCart)Session["cart"];
                                     cartItems.ProductQuantity = GetTotalQuantity();
                                 }

                                 ViewBag.DisplayMessage = "success";
                                 ModelState.AddModelError("", model.ProductName + " added successfully to cart!");
                             
                         }

                     }
                     catch (DbEntityValidationException ex)
                     {
                         ModelState.AddModelError("", ex.Message);
                         ViewBag.DisplayMessage = "Info";
                     }
                 }
                 else if (command == "Addtowishlist")
                 {
                     HttpCookie myCookie = HttpContext.Request.Cookies["CartSessionID"];
                     HttpCookie CartSessionID = null;
                     var newCookies = Guid.NewGuid();
                     if (myCookie != null)
                     {
                         CartSessionID = new HttpCookie("CartSessionID", myCookie.Value);
                         CartSessionID.Expires = DateTime.Now.AddDays(10);
                         HttpContext.Response.SetCookie(CartSessionID);
                     }
                     else
                     {
                         CartSessionID = new HttpCookie("CartSessionID", newCookies.ToString());
                         CartSessionID.Expires = DateTime.Now.AddDays(10);
                         HttpContext.Response.Cookies.Add(CartSessionID);
                     }

                     decimal unitPrice = decimal.Parse(model.ProductUnitPrice.ToString());
                     int quantity =int.Parse(model.ProductQuantity.ToString());

                     decimal totalPrice = unitPrice * quantity;
                     try
                     {
                         var chkIfExist = (from c in db.WishLists.Where(x => x.ProductID.Equals(model.ProductID)) select c).FirstOrDefault();
                         if(chkIfExist == null )
                         {
                             var wishlist = new WishList
                             {
                                 WishListSessionID = CartSessionID.Value.ToString(),
                                 ProductID = model.ProductID,
                                 ProductModelNumber = model.ProductSKU,
                                 ProductPrice = model.ProductUnitPrice,
                                 ProductQuantity = model.ProductQuantity,
                                 ProductName = model.ProductName,
                                 ProductTotalPrice = totalPrice,
                                 ProductSideViewThumb = model.ProductSideViewThumbnail,
                                 ProductFrontViewThumb = model.ProductFrontViewThumbnail
                             };
                             //Session["wishlist"] = wishlist;
                             db.WishLists.Add(wishlist);
                             db.SaveChanges();
                             ViewBag.DisplayMessage = "success";
                             ModelState.AddModelError("", "Product successfully added to wishlist!");                             
                         }
                         else
                         {
                             ViewBag.DisplayMessage = "Info";
                             ModelState.AddModelError("", "This product has already been added to your wishlist!");   
                         }
                        
                     }
                     catch (Exception ex)
                     {
                         ModelState.AddModelError("", ex.Message);
                         ViewBag.DisplayMessage = "Info";
                     }
                 }
            }
            feCombne = LoadProducts(model.ProductID);      
            return View(feCombne);
        }


        private ShoppingCart InsertIntoShoppingCart(Product model)
        {
            var newCookies = Guid.NewGuid();
            HttpCookie myCookie = HttpContext.Request.Cookies["CartSessionID"];
            HttpCookie CartSessionID = null;
            if (myCookie != null)
            {
                CartSessionID = new HttpCookie("CartSessionID", myCookie.Value);
                CartSessionID.Expires = DateTime.Now.AddDays(10);
                HttpContext.Response.SetCookie(CartSessionID);
            }
            else
            {
                CartSessionID = new HttpCookie("CartSessionID", newCookies.ToString());
                CartSessionID.Expires = DateTime.Now.AddDays(10);
                HttpContext.Response.Cookies.Add(CartSessionID);
            }
            ShoppingCart cart = null;
            try
            {
                cart = new ShoppingCart
                {
                    CartSessionID = CartSessionID.Value.ToString(),
                    ProductID = model.ProductID,
                    ProductSku = model.ProductSKU,
                    ProductPrice = decimal.Parse(model.ProductUnitPrice.ToString()),
                    ProductQuantity = 1,
                    ProductName = model.ProductName,
                    FrontViewThumbNail = model.ProductFrontViewThumbnail,
                    ProductTotalPrice = model.ProductUnitPrice,
                    DateCreated = DateTime.Now
                };
                db.ShoppingCarts.Add(cart);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
           
            return cart;
        }
        
        public FECombineViewModel LoadProducts(long? id)
        {
            FECombineViewModel model = new FECombineViewModel();

            try
            {
                var productList = (from p in db.Products
                                   where p.ProductID == id && p.ProductQuantity > 0
                                   select new
                                       FEProductDetailsViewModel
                                   {
                                       ProductID = p.ProductID,
                                       ProductSideView = p.ProductSideView.Substring(1),
                                       ProductSideViewThumb = p.ProductSideViewThumbnail.Substring(1),
                                       ProductDesc = p.ProductshortDesc,
                                       ProductPrice = p.ProductUnitPrice,
                                       ProductLocation = p.ProductLocation,
                                       ProductName = p.ProductName,
                                       ProductQuantity = p.ProductQuantity,
                                       ProductFrontViewThumb = p.ProductFrontViewThumbnail.Substring(1),
                                       ProductFrontView = p.ProductFrontView.Substring(1),
                                       ProductAvailability = p.ProductAvailability,
                                       ProductColour = p.ProductColour,
                                       ProductLongDesc = p.ProductLongDesc,
                                       ProductSKU = p.ProductSKU,
                                       ProductWeight = p.ProductWeight,
                                       ProductIsNew = p.ProductIsNew

                                   }).FirstOrDefault();
                model.Details = productList;


                var getCategory = (from ci in db.Products where ci.ProductID == id select ci).FirstOrDefault();

                var relatedProductList = (from p in db.Products.Where(p => p.CategoryID == getCategory.CategoryID).OrderByDescending(x => x.ProductCreatedOn)
                                          where p.ProductQuantity > 0
                                          select new
                                              FERelatedProductListViewModel
                                          {
                                              ProductID = p.ProductID,
                                              ProductFrontViewThumb = p.ProductFrontViewThumbnail,
                                              ProductSideViewThumb = p.ProductSideViewThumbnail,
                                              ProductPrice = p.ProductUnitPrice,                                             
                                              ProductName = p.ProductName,
                                              ProductQuantity = p.ProductQuantity,
                                              ProductSKU = p.ProductSKU,
                                              ProductIsNew = p.ProductIsNew
                                          }).Take(11);

                model.List = relatedProductList;

            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return model;
        }

        public ActionResult ViewWishList()
        {
            object wishList = null;
            if (Session["username"] != null)
            {
                string userName = Session["Username"].ToString();
                HttpCookie anonymousID = Request.Cookies["CartSessionID"];
                var updateWhishListUsername = (from u in db.WishLists.Where(x => x.WishListSessionID.Equals(anonymousID.Value.ToString())) select u).ToList();
                if (updateWhishListUsername != null)
                {
                    foreach (var w in updateWhishListUsername)
                    {
                        w.UserName = userName;
                    }
                    db.SaveChanges();
                }
               
                wishList = (from w in db.WishLists
                            join k in db.Products
                            on w.ProductID equals k.ProductID
                            where (w.UserName == userName && k.ProductQuantity > 0)
                            select new FEShowWishListViewModel
                            {                              
                                ProductID = w.ProductID,
                                ProductName = w.ProductName,
                                ProductTotalPrice = w.ProductTotalPrice,
                                ProductModelNumber = w.ProductModelNumber,
                                ProductPrice = w.ProductPrice,
                                ProductQuantity = w.ProductQuantity,
                                UserName = w.UserName,
                                ProductSideViewThumb = k.ProductSideViewThumbnail,
                                ProductFrontViewThumb = k.ProductFrontViewThumbnail,
                                ProductIsNew = k.ProductIsNew
                            }).Distinct().ToList();
            }
            else
            {
                return RedirectToAction("index", "home");
            }
            return View(wishList);
        }
        public ActionResult SubCategory(int id, string name)
        {
            string   nam = "";
            if (name != null)
            {
               nam = name.Replace('-', ' ');
                nam = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nam.ToLower());
                ViewBag.CatName = nam;              
            }

            try
            {
                var catName = "";
                var getSubCatName = (from c in db.SubCategories

                                  select new
                                  {
                                      c.SubCategoryName
                                  }).ToList();
                int count = 7; // pick subcatgory name randomly
                var subCat = getSubCatName.OrderBy(t => Guid.NewGuid())
                        .Take(count);
                foreach (var d in subCat)
                {
                    catName = d.SubCategoryName.ToLower() + "," + catName.ToLower();
                }
                var getit = catName;
                ViewBag.Title =  nam.ToLower();
                ViewBag.MetaKeywords = catName  + nam.ToLower();
                ViewBag.MetaDescription = nam.ToLower() + " product sub category";
            }
            catch (Exception)
            {

            }
            if (id == null)
            {
                return RedirectToAction("index", "home");
            }
            object subCategoryList = null;
            try
            {
                //ViewBag.Current = "Scheduler";
                subCategoryList = (from p in db.Products.Where(x => x.SubCategoryID == id).OrderByDescending(x => x.SubCategoryID)
                                   where p.ProductQuantity > 0
                                   select new
                                      FEDisplayProductsOnProductPageViewModel
                                   {
                                       ProductID = p.ProductID,
                                       ProductSideViewThumbnail = p.ProductSideViewThumbnail,
                                       ProductFrontViewThumbnail = p.ProductFrontViewThumbnail,
                                       ProductDescription = p.ProductshortDesc,
                                       ProductPrice = p.ProductUnitPrice,
                                       ProductLocation = p.ProductLocation,
                                       ProductName = p.ProductName,
                                       ProductQuantity = p.ProductQuantity,
                                       ProductIsNew = p.ProductIsNew
                                   }).Take(80);
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return View(subCategoryList);

        }


        public ActionResult Category(int id, string name)
        {
            string nam = "";
            if(name!= null)
            {
                nam = name.Replace('-',' ');
                nam = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nam.ToLower());
                ViewBag.CatName = nam;              
            }

            try
            {
               var catName = "";
                var getcatName = (from c in db.Categories select c).ToList();
                foreach(var d in getcatName)
                {
                    catName = d.CategoryName.ToLower() + "," + catName.ToLower();
                }

                ViewBag.Title = nam.ToLower();
                ViewBag.MetaKeywords = catName + nam.ToLower();
                ViewBag.MetaDescription = "product categories for our online store";
            }
            catch(Exception)
            {

            }
            if (id == null)
            {
                return RedirectToAction("index", "home");
            }
            object CategoryList = null;
            try
            {
                //ViewBag.Current = "Scheduler";
                CategoryList = (from p in db.Products.Where(x => x.CategoryID == id).OrderByDescending(x => x.CategoryID)
                                where p.ProductQuantity > 0
                                   select new
                                      FEDisplayProductsOnProductPageViewModel
                                   {
                                       ProductID = p.ProductID,
                                       ProductSideViewThumbnail = p.ProductSideViewThumbnail,
                                       ProductFrontViewThumbnail = p.ProductFrontViewThumbnail,
                                       ProductDescription = p.ProductshortDesc,
                                       ProductPrice = p.ProductUnitPrice,
                                       ProductLocation = p.ProductLocation,
                                       ProductName = p.ProductName,
                                       ProductQuantity = p.ProductQuantity,
                                       ProductIsNew = p.ProductIsNew
                                   }).Take(80);
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return View(CategoryList);

        }

        //Custom Error page
        public ActionResult Error()
        {
            return View();
        }
       

       
        //Customer Dashboard
        public ActionResult Dashboard()
        {
            object getOrder = null;
            if (Session["Username"] != null)
            {
               
                string username = Session["username"].ToString();
                try
                {
                    getOrder = (from o in db.Orders.Where(x => x.UserName == username)
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
        }

       


        public ActionResult ChangePassword()
        {
            if (Session["Username"] != null)
            {
               
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(FEChangeOldPasswordViewModel cp)
        {
            var PwdHashing = new PasswordHashing();
            string password = PwdHashing.Encrypt(cp.UserOldPassword);
            var chkUser = (from l in db.Users
                           where l.UserPassword == password
                           select l).FirstOrDefault();
            if (chkUser != null)
            {
                try
                {
                    var decriptPwd = PwdHashing.Encrypt(cp.UserPassword);
                    chkUser.UserPassword = decriptPwd;
                    chkUser.UserConfirmPassword = decriptPwd;
                    db.SaveChanges();
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Password successfully changed!");
                }
                catch (Exception)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "password change was not successful, please try again!");
                }
            }
            else
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Old password does not exist!");
            }
            return View();
        }

        public ActionResult UpdateProfile()
        {
           FEUpdateProfileViewModel model = null;
            if (Session["Username"] != null)
            {
                var emailAddress = Session["Username"].ToString();
                var getRecord = (from u in db.Users where (u.UserEmail == emailAddress) select u).FirstOrDefault();
                if(getRecord !=null)
                {
                    model = new FEUpdateProfileViewModel
                    {
                        UserID = getRecord.UserID,
                        Firstname = getRecord.UserFirstName,
                        Lastname = getRecord.UserLastName,
                        BillingAddress = getRecord.UserBillingAddress,
                        ContactAddress = getRecord.UserAddress1,
                        OfficeAddress = getRecord.UserAddress2,                       
                        PhoneNumber = getRecord.UserPhone
                    };
                    //LoadDropDownList();                    
                }
                populateDropdownListDefaultValue(emailAddress, getRecord.UserCity);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateProfile(FEUpdateProfileViewModel udp, string gender, string country, string State, string lga, string txtCity, string txtState)
        {
            var emailAddress = Session["Username"].ToString();
            string state = "";
                var lga3 = (txtCity != "") ? txtCity : lga;
                int lgaP = int.Parse(lga3);
            var getCityName = (from s in db.Lgas.Where(x => x.LgaID == lgaP) select s).FirstOrDefault();
            try
            {
                if (txtState != "")
                {
                    state = txtState;
                }
                else
                {
                    int enteredStateID = Convert.ToInt32(State);
                    var stateID = (from st in db.States where (st.StateID == enteredStateID) select st).FirstOrDefault();
                    state = stateID.StateName;
                }
                
                var update = (from u in db.Users.Where(x => x.UserEmail == emailAddress) select u).FirstOrDefault();
                if (update != null)
                {
                    update.UserLastName = udp.Lastname;
                    update.UserFirstName = udp.Firstname;
                    update.UserPhone = udp.PhoneNumber;
                    update.UserState = state;
                    update.UserCity = getCityName.LgaName;
                    update.CountryID = int.Parse(country);
                    update.UserBillingAddress = udp.BillingAddress;
                    update.UserAddress1 = udp.ContactAddress;
                    update.UserAddress2 = udp.OfficeAddress;
                    update.GenderID = int.Parse(gender);
                    db.SaveChanges();
                    
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Record Updated successfully");
                }
            }
            catch(Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Error: "+ex.Message);
            }
            populateDropdownListDefaultValue(emailAddress, getCityName.LgaName);
            return View();
        }

        public void populateDropdownListDefaultValue(string username, string lgaName)
        {
            //new to populate ddl for edit starts here               
            var getCountryAndGenderID = (from u in db.Users.Where(x => x.UserEmail.Equals(username))
                                         select u).FirstOrDefault();
            if (getCountryAndGenderID.CountryID == 155)//Nigeria
            {
                var getSateID = (from s in db.States.Where(x => x.StateName.Equals(getCountryAndGenderID.UserState)) select s).FirstOrDefault();
                var getCityId = (from s in db.Lgas.Where(x => x.LgaName == lgaName) select s).FirstOrDefault();

                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", getCountryAndGenderID.CountryID);
                ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", getCountryAndGenderID.GenderID);
                ViewBag.StateId = new SelectList(db.States, "StateID", "StateName", getSateID.StateID);
                ViewBag.CityID = new SelectList(db.Lgas, "LgaID", "LgaName", getCityId.LgaID);
            }
            else
            {

            }

            //new ends here
        }

        public ActionResult Register()
        {
            ViewBag.Title = "customer registration form";
            ViewBag.MetaKeywords = "register new customers, new registration, registration form";
            ViewBag.MetaDescription = "glorious evidence online store registration for new customers";
            LoadDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult Register([Bind(Include = "UserID,UserFirstName,UserLastName,BillingAddress,UserAddress1,UserAddress2,CountryID,UserState,UserCity,UserEmail, " +
           " UserPassword,UserConfirmPassword,SecurityQuestionID,UserPhone,CreatedOn,GenderID,IsDeleted,UserRole,ActivationID,IsActivated")]
            FEUserRegistrationViewModel suvm, string lga, string State, string terms, string txtState, string txtCity, string gender, string Country)
        {
            try
            {

                ViewBag.StateId = new SelectList(db.States, "StateID", "StateName");
                string termsAndCondition = (terms == "yes") ? "Agreed" : "Not Agreed";

                //var state = (txtState != "") ? txtState : State;
                string state = "";
                int genderID = int.Parse(gender.ToString());
                int countryID = int.Parse(Country.ToString());
                var lga3 = (txtCity != "") ? txtCity : lga;

                if (txtState != "")
                {
                    state = txtState;
                }
                else
                {
                    int enteredStateID = Convert.ToInt32(State);
                    var stateID = (from st in db.States where (st.StateID == enteredStateID) select st).FirstOrDefault();
                    state = stateID.StateName;
                }

                var activationID = Guid.NewGuid();
                var encriptPwd = new PasswordHashing();
                if (termsAndCondition == "Agreed")
                {

                    var itemCollections = new User
                    {
                        UserLastName = suvm.UserLastName,
                        UserFirstName = suvm.UserFirstName,
                        UserAddress1 = suvm.UserAddress1,
                        UserAddress2 = suvm.UserAddress2,
                        UserState = state.ToString(),
                        UserCity = lga3,
                        UserEmail = suvm.UserEmail,
                        UserPassword = encriptPwd.Encrypt(suvm.UserPassword),
                        UserConfirmPassword = encriptPwd.Encrypt(suvm.UserConfirmPassword),
                        SecurityQuestionID = 2,
                        UserBillingAddress = suvm.BillingAddress,
                        UserPhone = suvm.UserPhone,
                        CreatedOn = DateTime.Now,
                        GenderID = genderID,
                        IsDeleted = false,
                        CountryID = countryID,
                        UserRole = "4",
                        ActivationID = activationID,
                        IsActivated = false,

                    };


                    LoadDropDownList();
                    var chkExistingEmail = (from l in db.Users
                                            where l.UserEmail == suvm.UserEmail
                                            select l).FirstOrDefault();
                    if (chkExistingEmail == null)
                    {
                        try
                        {
                            var uir = new UsersInRole
                            {
                                UserID = suvm.UserID,
                                RoleID = Convert.ToInt32(4)
                            };
                            string newActivationID = activationID.ToString();
                            itemCollections.UsersInRoles.Add(uir);
                            db.Users.Add(itemCollections);
                            db.SaveChanges();
                            EmailNotification.ProcessEmailForAccountActivation(suvm.UserEmail, suvm.UserFirstName, suvm.UserLastName, newActivationID, suvm.UserEmail);
                            LoadDropDownList();
                            ViewBag.DisplayMessage = "success";
                            ModelState.AddModelError("", "Registration Successful, an activation link has been sent to your inbox, kindly activate your account so you will be able to login!");
                        }
                        catch (DbEntityValidationException ex)
                        {
                            ViewBag.DisplayMessage = "Info";
                            ModelState.AddModelError("", ex.Message);
                        }
                    }
                    else
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", "This email address has already been used, enter a different email address!");
                        LoadDropDownList();
                        return View();
                    }

                }


                if (termsAndCondition == "Not Agreed")
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "You must agree to our terms and conditions before submitting the form");

                    LoadDropDownList();
                    return View();
                }


            }

            catch (DbUpdateException ex)
            {
                UpdateException updateException = (UpdateException)ex.InnerException;
                SqlException sqlException = (SqlException)updateException.InnerException;

                foreach (SqlError error in sqlException.Errors)
                {
                    // TODO: Do something with your errors
                }
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Error: " + ex.Message);
                //return View();
            }
            return View();
        }


        public JsonResult LgaList(int id)
        {
            var state = from s in db.Lgas
                        where s.StateID == id
                        select s;
            return Json(new SelectList(state.ToArray(), "LgaID", "LgaName"), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadLgaByStateId(string stateName)
        {
            var stateList = GetLga(Convert.ToInt32(stateName));
            var stateData = stateList.Select(m => new SelectListItem()
            {
                Text = m.LgaName,
                Value = m.LgaID.ToString(),
                // Value = m.LgaName
            });
            return Json(stateData, JsonRequestBehavior.AllowGet);
        }

        public IList<Lga> GetLga(int stateId)
        {
            return db.Lgas.Where(m => m.StateID == stateId).ToList();
        }

        private void LoadDropDownList()
        {
            try
            {
                ViewBag.StateId = new SelectList(db.States, "StateID", "StateName");
                ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName");
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName"); 
            }
            catch (Exception ex)
            {
                ViewBag.DispMsg = false;
                ModelState.AddModelError("", ex.Message);
            }

        }

        public ActionResult ForgotPassword()
        {
            ViewBag.Title = "forget password";
            ViewBag.MetaKeywords = "gorget password, forget password, retrieve password";
            ViewBag.MetaDescription = "glorious evidence online store user retrieve password";

            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string emailAddress)
        {
            var PwdHashing = new PasswordHashing();

            var chkUser = (from l in db.Users
                           where l.UserEmail == emailAddress
                           select l).FirstOrDefault();

            if (chkUser != null)
            {
                StringBuilder buffer;

                try
                {
                    String pin = System.Guid.NewGuid().ToString();
                    buffer = new StringBuilder(pin);
                    buffer.Length = 10;
                    var newPassword = buffer.ToString().ToUpper();

                    var decriptPwd = PwdHashing.Encrypt(newPassword);
                    chkUser.UserPassword = decriptPwd;
                    chkUser.UserConfirmPassword = decriptPwd;
                    db.SaveChanges();
                    EmailNotification.ProcessEmailForPasswordReset(newPassword, chkUser.UserEmail, chkUser.UserFirstName, chkUser.UserLastName);
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Password reset was successful, a new password has been sent to your email address!");
                }
                catch (Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Password reset not successful! " + ex.Message);
                }
            }
            else
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Email address does not exist!");
            }
            return View();
        }

        
        public ActionResult CustomerLogin()
        {
            if(Session["cart"] != null)
            {
                if(Session["username"] != null)
                {
                    return RedirectToAction("BillingAddress", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult CustomerLogin(UserLoginViewModel login)
        {           
            try
            {
                var PwdHashing = new PasswordHashing();
                string username = login.UserEmail;
                string password = login.UserPassword;
                //int role = 0;
                var chkUser = (from l in db.Users
                               where l.UserEmail == username && l.IsDeleted == false && l.IsActivated == true
                               select l).FirstOrDefault();
                if (chkUser != null)
                {
                    try
                    {
                        var decriptPwd = PwdHashing.Decrypt(chkUser.UserPassword);
                        if (chkUser.UserEmail == username && decriptPwd == password)
                        {
                            Session["username"] = chkUser.UserEmail;
                           // Session["password"] = chkUser.UserPassword;
                            Session["firstName"] = chkUser.UserFirstName;
                            Session["userID"] = chkUser.UserID;                           
                            return RedirectToAction("BillingAddress", "Home");
                        }
                        else
                        {
                            ViewBag.DisplayMessage = "Info";
                            ModelState.AddModelError("", "Email or Password not valid!");
                        }
                    }
                    catch (Exception)
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", "Database Password not encripted!");                      
                    }

                }
                else
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "User does not exist!");                  
                }
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public decimal GetTotalPriceForProductDeliveryPrice()
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
                    total = (from prod in db.Products
                             join s in db.ShoppingCarts on prod.ProductID equals s.ProductID
                             where s.CartSessionID == anonymousID.Value.ToString()
                             select (int?)s.ProductQuantity * prod.ShippingFee).Sum();
                }
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return total ?? decimal.Zero;
        }
        
        public decimal GetTotalPrice()
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
                    total = (from cartItems in db.ShoppingCarts
                             where cartItems.CartSessionID == anonymousID.Value.ToString()
                             select (int?)cartItems.ProductQuantity * cartItems.ProductPrice).Sum();
                }
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }            
            return total ?? decimal.Zero;
        }

        public decimal GetTotalPriceForAProduct(long productID, string productSku, int quantity)
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
                    total = (from cartItems in db.ShoppingCarts
                             where cartItems.ProductSku == productSku && cartItems.ProductID == productID
                             select quantity * cartItems.ProductPrice).Sum();
                }     
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
                   
            return total ?? decimal.Zero;
        }

        public int GetTotalQuantity()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            int? total = 0;
            try
            {
                HttpCookie anonymousID = Request.Cookies["CartSessionID"];
                if (anonymousID != null)
                {
                    total = (from cartItems in db.ShoppingCarts
                             where cartItems.CartSessionID == anonymousID.Value.ToString()
                             select (int?)cartItems.ProductQuantity).Sum();
                }   
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
                    
            return total ?? 0;
        }

        public State GetShippingAndHandlingAmount()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
           State ShippingAndHandlingAmount = null;
            ShippingAndHandlingAmount = (from sh in db.States select sh).FirstOrDefault();

            return ShippingAndHandlingAmount;
        }

        public object LoadCart()
        {
            HttpCookie anonymousID = Request.Cookies["CartSessionID"];
            object cart = null;
            if (anonymousID != null)
            {
                cart = (from c in db.ShoppingCarts.Where(x => x.CartSessionID == anonymousID.Value.ToString())
                        
                        select new FEShoppingCartViewModel
                        {
                            ProductID = c.ProductID,
                            CartSessionID = c.CartSessionID,
                            ProductName = c.ProductName,
                            ProductPrice = c.ProductPrice,
                            ProductSku = c.ProductSku,
                            ProductTotalPrice = c.ProductQuantity * c.ProductPrice,
                            ProductQuantity = c.ProductQuantity,
                            FrontViewThumbNail = c.FrontViewThumbNail.Substring(1),

                        }).ToList();
            }

            return cart;
        }

        private void LoadOrderSummary(int? id)
        {            
                HttpCookie anonymousID = Request.Cookies["CartSessionID"];
             var username = Session["username"].ToString();
                if(anonymousID !=null)
                {
                   
                    var del = (from d in db.ShoppingCarts.Where(x => x.ProductID == id && x.CartSessionID == anonymousID.Value.ToString()) select d).FirstOrDefault();
                    if (del != null)
                    {
                        
                        db.ShoppingCarts.Remove(del);
                        db.SaveChanges();
                        LoadCart();
                        GetTotalPrice();
                        GetTotalPrice();
                        var cartItems = (ShoppingCart)Session["cart"];
                        cartItems.ProductQuantity = GetTotalQuantity();
                        ViewBag.DisplayMessage = "success";
                        ModelState.AddModelError("", "Item(s) successfully removed from cart!");
                    }                               
            }

            ViewBag.CartTotal = GetTotalPrice();
            ViewBag.CartItems = GetTotalQuantity();
            if (ViewBag.CartTotal > 0)
            {
                var sh = (from s in db.States
                          join u in db.Users on s.StateName equals u.UserBillingState
                          where u.UserEmail.Equals(username)
                          select new
                              {
                                  s.ShippingFee
                              }).FirstOrDefault();
                var getcount = GetTotalQuantity();
                var sfee = GetTotalPriceForProductDeliveryPrice();
                var shippingFee = (sfee / getcount);

                ViewBag.ShippingAndHandling = sh.ShippingFee + shippingFee;

                ViewBag.Tax = 0.00m;
                ViewBag.Discount = 0.00m;
                Session["grandTotal"] = ViewBag.CartTotal + ViewBag.ShippingAndHandling + ViewBag.Tax;
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

        private void LoadOrderSummaryForViewCart(int? id)
        {
            HttpCookie anonymousID = Request.Cookies["CartSessionID"];
            if (anonymousID != null)
            {
                var del = (from d in db.ShoppingCarts.Where(x => x.ProductID == id && x.CartSessionID == anonymousID.Value.ToString()) select d).FirstOrDefault();
                if (del != null)
                {
                    //int getQuant = int.Parse(del.ProductQuantity.ToString());
                    db.ShoppingCarts.Remove(del);
                    db.SaveChanges();
                    LoadCart();
                    GetTotalPrice();
                    GetTotalPrice();
                    var cartItems = (ShoppingCart)Session["cart"];
                    cartItems.ProductQuantity = GetTotalQuantity();
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Item(s) successfully removed from cart!");
                }
            }

            ViewBag.CartTotal = GetTotalPrice();
            ViewBag.CartItems = GetTotalQuantity();
            if (ViewBag.CartTotal > 0)
            {
                //ShippingAndHandling sh = GetShippingAndHandlingAmount();
                var getcount = GetTotalQuantity();
                var sfee = GetTotalPriceForProductDeliveryPrice();
                var shippingFee = (sfee / getcount);
                ViewBag.ShippingAndHandling = shippingFee;

                ViewBag.Tax = 0.00m;
                ViewBag.Discount = 0.00m;
                Session["grandTotal"] = ViewBag.CartTotal + ViewBag.ShippingAndHandling + ViewBag.Tax;
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
        public ActionResult ViewCart(int? id)
        {
            ViewBag.Title = "shopping cart";
            ViewBag.MetaKeywords = "shopping cart, cart, product shopping cart, online shopping cart";
            ViewBag.MetaDescription = "customers shopping cart with cart details and order summary";

            LoadCart();
            LoadOrderSummaryForViewCart(id);
            return View(LoadCart());
        }

        [HttpPost]
        public ActionResult ViewCart(List<FEShoppingCartViewModel> cartQuantity, string command, int? id)
        {
            if (Session["cart"] != null)
            {
                if (command == "UpdateCart")
                {
                    foreach (var pquan in cartQuantity)
                    {
                        var updateQuantityInProductTable = (from u in db.Products.Where(x => x.ProductID.Equals(pquan.ProductID)) select u).FirstOrDefault();
                        if (updateQuantityInProductTable != null)
                        {
                            if(updateQuantityInProductTable.ProductQuantity < pquan.ProductQuantity)
                            {
                                ViewBag.DisplayMessage = "Info";
                                ModelState.AddModelError("", "Sorry, we currently have " + updateQuantityInProductTable.ProductQuantity + " quantity(ies) available for: " + updateQuantityInProductTable.ProductName);

                                LoadCart();
                                LoadOrderSummaryForViewCart(id);
                                var cartItemList = (ShoppingCart)Session["cart"];
                                cartItemList.ProductQuantity = GetTotalQuantity();
                                return View(LoadCart());

                            }
                            else
                            {
                                var getRecordForUpdate = (from u in db.ShoppingCarts.Where(x => x.ProductID.Equals(pquan.ProductID)) select u).FirstOrDefault();
                                if (getRecordForUpdate != null)
                                {
                                    var quantity = pquan.ProductQuantity;
                                    getRecordForUpdate.ProductQuantity = pquan.ProductQuantity;
                                    getRecordForUpdate.ProductTotalPrice = GetTotalPriceForAProduct(pquan.ProductID, pquan.ProductSku, quantity);
                                }
                            }
                        }
                    }
                   
                    db.SaveChanges();
                    
                    LoadCart();
                    LoadOrderSummaryForViewCart(id);
                    var cartItems = (ShoppingCart)Session["cart"];
                    cartItems.ProductQuantity = GetTotalQuantity();
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Cart Updated successfully!");
                }
                else if (command == "Proceedtocheckout")
                {
                    HttpCookie anonymousID = Request.Cookies["CartSessionID"];
                    if (Session["Cart"] != null)
                    {
                        var getGetIDAndQuantityFromCart = (from g in db.ShoppingCarts.Where(x => x.CartSessionID.Equals(anonymousID.Value))
                                                           select new 
                                                               {
                                                                   g.CartSessionID,
                                                                   g.ProductQuantity,
                                                                   g.ProductID
                                                               }).ToList();
                        if (getGetIDAndQuantityFromCart != null)
                        {
                            foreach (var pquanr in getGetIDAndQuantityFromCart)
                            {
                                var updateQuantityInProductTable = (from u in db.Products.Where(x => x.ProductID.Equals(pquanr.ProductID)) select u).FirstOrDefault();
                                if (updateQuantityInProductTable != null)
                                {
                                    if (updateQuantityInProductTable.ProductQuantity >= pquanr.ProductQuantity)
                                    {
                                        LoadCart();
                                        LoadOrderSummaryForViewCart(id);
                                        var cartItemList = (ShoppingCart)Session["cart"];
                                        cartItemList.ProductQuantity = GetTotalQuantity();
                                        var vare =  ViewBag.CartTotal + ViewBag.ShippingAndHandling + ViewBag.Tax;
                                        //Session["TotalAmount"] = vare;
                                        return RedirectToAction("CustomerLogin", "Home");                                      
                                    }
                                    else
                                    {
                                        ViewBag.DisplayMessage = "Info";
                                        ModelState.AddModelError("", "Sorry, we currently have " + updateQuantityInProductTable.ProductQuantity + " quantity(ies) available for " + updateQuantityInProductTable.ProductName + " enter quantity(ies) as " + updateQuantityInProductTable.ProductQuantity + " and click on update cart button to update quantity in cart.");
                                        LoadCart();
                                        LoadOrderSummaryForViewCart(id);
                                        var cartItemList = (ShoppingCart)Session["cart"];
                                        cartItemList.ProductQuantity = GetTotalQuantity();
                                        return View(LoadCart());
                                    }
                                }
                            }
                        }
                        
                    }
                }
                ViewBag.CartTotal = GetTotalPrice();
                ViewBag.CartItems = GetTotalQuantity();
                if (ViewBag.CartTotal > 0)
                {
                    var getcount = GetTotalQuantity();
                    var sfee = GetTotalPriceForProductDeliveryPrice();
                    var shippingFee = (sfee / getcount);
                    //State sh = GetShippingAndHandlingAmount();
                    ViewBag.ShippingAndHandling = shippingFee;
                    //ViewBag.ShippingAndHandling = 1000m;
                    ViewBag.Tax = 0.00m;
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
            else
            {
               return RedirectToAction("Index", "Home");
            }

            return View(LoadCart());
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            //Session["username"] = null;
            Session.Abandon();
            Session.RemoveAll();
            Session.Clear();
            return View();
        }

        private void LoadDropDownBoxForEdit()
        {

        }
        public ActionResult BillingAddress(int? id)
        {
            FEBillingAddressViewModel getRegDetails = null;
            if (Session["cart"] != null && Session["username"] != null)
            {
                //LoadDropDownList();
                LoadOrderSummaryForViewCart(id);
                string username = Session["username"].ToString();

                //new to populate ddl for edit starts here               
                var getCountryAndGenderID = (from u in db.Users.Where(x => x.UserEmail.Equals(username))
                                             select u).FirstOrDefault();
                if(getCountryAndGenderID.CountryID == 155)//Nigeria
                {
                    var getSateID = (from s in db.States.Where(x => x.StateName.Equals(getCountryAndGenderID.UserState)) select s).FirstOrDefault();
                    var getCityId = (from s in db.Lgas.Where(x => x.LgaName.Equals(getCountryAndGenderID.UserCity)) select s).FirstOrDefault();

                    ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", getCountryAndGenderID.CountryID);
                    ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", getCountryAndGenderID.GenderID);
                    ViewBag.StateId = new SelectList(db.States, "StateID", "StateName", getSateID.StateID);
                    ViewBag.CityID = new SelectList(db.Lgas, "LgaID", "LgaName", getCityId.LgaID);
                }
                else
                {

                }
               
                //new ends here

              getRegDetails = (from r in db.Users
                                 join g in db.Genders on r.GenderID equals g.GenderID
                                 join c in db.Countries on r.CountryID equals c.CountryID
                                 where (r.UserEmail.Equals(username))
                                 select new FEBillingAddressViewModel
                    {
                        UserEmail = r.UserEmail,
                        UserCity = r.UserCity,
                        UserLastName = r.UserLastName,
                        UserFirstName = r.UserFirstName,
                        UserState = r.UserState,
                        UserPhone = r.UserPhone,
                        UserAddress1 = r.UserAddress1,
                        UserAddress2 = r.UserAddress2,
                        BillingAddress = r.UserBillingAddress,
                        CountryID = c.CountryName,
                        GenderID = g.GenderName,
                        UserID = r.UserID
                    }).FirstOrDefault();

             // model.PopulateDefaultItem = getRegDetails;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(getRegDetails);
        }


        [HttpPost]
        public ActionResult BillingAddress(FEBillingAddressViewModel billing, int? id, string lga, string gender, string State, string terms, string txtState, string txtCity, string Country)
        {
            //LoadDropDownList();
            LoadOrderSummaryForViewCart(id);
            string state = "";
            var genderID = int.Parse(gender.ToString());
            var lga3 = (txtCity != "") ? txtCity : lga;
            string username = Session["username"].ToString();
            if (txtState != "")
            {
                state = txtState;
            }
            else
            {
                int enteredStateID = Convert.ToInt32(State);
                var stateID = (from st in db.States where (st.StateID == enteredStateID) select st).FirstOrDefault();
                state = stateID.StateName;
            }

            if (lga3 != "---Select LGA---")
            {
                
                int countryID = int.Parse(Country.ToString());
                var getRecord = (from r in db.Users.Where(x => x.UserEmail.Equals(username)) select r).FirstOrDefault();
                if (getRecord != null)
                {
                    try
                    {
                        getRecord.UserBillingCity = lga3;
                        getRecord.UserBillingState = state;
                        getRecord.UserBillingCountry = Country;
                        getRecord.UserBillingPhone = billing.UserPhone;
                        getRecord.UserAddress1 = billing.UserAddress1;
                        getRecord.UserAddress2 = billing.UserAddress2;
                        getRecord.UserBillingAddress = billing.BillingAddress;
                        getRecord.UserBillingFirstName = billing.UserFirstName;
                        getRecord.UserBillingLastName = billing.UserLastName;
                        getRecord.GenderID = genderID;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", ex.Message);
                    }
                }
                //Start updating shopping and whislist with username
                var chkUser = (from l in db.Users
                               where l.UserEmail == username && l.IsDeleted == false && l.IsActivated == true
                               select l).FirstOrDefault();
                if (chkUser != null)
                {
                    try
                    {
                        HttpCookie anonymousID = Request.Cookies["CartSessionID"];
                        if (anonymousID.Value != null)
                        {
                            var updateShoppinngCartUsername = (from u in db.ShoppingCarts.Where(x => x.CartSessionID.Equals(anonymousID.Value.ToString())) select u).ToList();
                            if (updateShoppinngCartUsername != null)
                            {
                                foreach (var u in updateShoppinngCartUsername)
                                {
                                    u.UserName = chkUser.UserEmail;
                                }
                                db.SaveChanges();
                            }

                            var updateWhishListUsername = (from u in db.WishLists.Where(x => x.WishListSessionID.Equals(anonymousID.Value.ToString())) select u).ToList();
                            if (updateWhishListUsername != null)
                            {
                                foreach (var w in updateWhishListUsername)
                                {
                                    w.UserName = chkUser.UserEmail;
                                }
                                db.SaveChanges();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", ex.Message);
                    }
                    //End update shopping and whistlist with username
                    return RedirectToAction("DeliveryMethod", "Home");
                }
                
            }
            else
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Please select city!");
            }
            
            //new to populate ddl for edit starts here               
            var getCountryAndGenderID = (from u in db.Users.Where(x => x.UserEmail.Equals(username))
                                         select u).FirstOrDefault();
            var getSateID = (from s in db.States.Where(x => x.StateName.Equals(getCountryAndGenderID.UserState)) select s).FirstOrDefault();
            var getCityId = (from s in db.Lgas.Where(x => x.LgaName.Equals(getCountryAndGenderID.UserCity)) select s).FirstOrDefault();

            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", getCountryAndGenderID.CountryID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", getCountryAndGenderID.GenderID);
            ViewBag.StateId = new SelectList(db.States, "StateID", "StateName", getSateID.StateID);
            ViewBag.CityID = new SelectList(db.Lgas, "LgaID", "LgaName", getCityId.LgaID);
            //new ends here
            return View();
        }

        public ActionResult DeliveryMethod(int? id)
        {
            if (Session["cart"] != null && Session["username"] != null)
            {
                LoadCart();
                LoadOrderSummary(id);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(LoadCart());
        }

        [HttpPost]
        public ActionResult DeliveryMethod(string command, int? id)
        {
            var deliveryOption = command;
            if (deliveryOption != null)
            {
                try
                {
                    Session["deliveryOption"] = deliveryOption;
                    //LoadCart();
                    //LoadOrderSummary(id);
                    string username = Session["username"].ToString();
                    HttpCookie anonymousID = Request.Cookies["CartSessionID"];
                    LoadOrderSummary(id);
                    var chkUser = (from l in db.Users
                                   where l.UserEmail == username && l.IsDeleted == false && l.IsActivated == true
                                   select l).FirstOrDefault();
                    var updateShoppinngCartUsername = (from u in db.ShoppingCarts.Where(x => x.CartSessionID.Equals(anonymousID.Value.ToString())) select u).ToList();
                    if (updateShoppinngCartUsername != null)
                    {
                        foreach (var u in updateShoppinngCartUsername)
                        {
                            u.DeliveryOption = deliveryOption;
                        }
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", ex.Message);
                }                
            }
            else
            {               
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Please choose your delivery option!");
                return View();
            }
            LoadCart();
            LoadOrderSummary(id);
            return RedirectToAction("PaymentMethod", "Home");            
        }

        public ActionResult PaymentMethod(int? id)
        {
            if (Session["cart"] != null && Session["username"] != null)
            {
                LoadCart();
                LoadOrderSummary(id);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(LoadCart());
        }

        [HttpPost]
        public ActionResult PaymentMethod(string payment, int? id)
        {
            try
            {
                var paymentOption = payment;
                if (paymentOption != null)
                {

                    Session["paymentOption"] = paymentOption;
                    LoadCart();
                    LoadOrderSummary(id);
                    string username = Session["username"].ToString();
                    HttpCookie anonymousID = Request.Cookies["CartSessionID"];
                    LoadOrderSummary(id);
                    var chkUser = (from l in db.Users
                                   where l.UserEmail == username && l.IsDeleted == false && l.IsActivated == true
                                   select l).FirstOrDefault();
                    var updateShoppinngCartUsername = (from u in db.ShoppingCarts.Where(x => x.CartSessionID.Equals(anonymousID.Value.ToString())) select u).ToList();
                    if (updateShoppinngCartUsername != null)
                    {
                        foreach (var u in updateShoppinngCartUsername)
                        {
                            u.PaymentOption = paymentOption;
                        }
                        db.SaveChanges();
                    }
                }
                else
                {
                    LoadCart();
                    LoadOrderSummary(id);
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Please choose your delivery option!");
                    return View();
                }              
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return RedirectToAction("OrderReview", "Home");
        }

        public ActionResult OrderReview(int? id)
        {
            if (Session["cart"] != null && Session["username"] != null)
            {
                var username = Session["username"].ToString();
                try
                {
                    LoadCart();
                    if (id != null)
                    {
                        var del = (from d in db.ShoppingCarts.Where(x => x.ProductID == id && x.CartSessionID == Request.AnonymousID) select d).FirstOrDefault();
                        if (del != null)
                        {
                            //int getQuant = int.Parse(del.ProductQuantity.ToString());
                            db.ShoppingCarts.Remove(del);
                            db.SaveChanges();
                            LoadCart();
                            GetTotalPrice();
                            GetTotalPrice();
                            var cartItems = (ShoppingCart)Session["cart"];
                            cartItems.ProductQuantity = GetTotalQuantity();
                            ViewBag.DisplayMessage = "success";
                            ModelState.AddModelError("", "Item(s) successfully removed from cart!");
                        }
                    }

                    ViewBag.CartTotal = GetTotalPrice();
                    ViewBag.CartItems = GetTotalQuantity();
                    if (ViewBag.CartTotal > 0)
                    {
                        var sh = (from s in db.States
                                  join u in db.Users on s.StateName equals u.UserBillingState
                                  where u.UserEmail.Equals(username)
                                  select new
                                  {
                                      s.ShippingFee
                                  }).FirstOrDefault();

                        var getcount = GetTotalQuantity();
                        var sfee = GetTotalPriceForProductDeliveryPrice();
                        var shippingFee = (sfee / getcount);

                        ViewBag.ShippingAndHandling = sh.ShippingFee + shippingFee;
                        ViewBag.Tax = 0.00m;
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
                
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(LoadCart());
        }

        [HttpPost]
        public ActionResult OrderReview(int? id, Order order)
        {
            var paymentOption = Session["paymentOption"].ToString();
            var deliveryOption = Session["deliveryOption"].ToString();
            var userName = Session["username"].ToString();
            LoadCart();
            LoadOrderSummary(id);

            // store total price for api final output starts here
            var cartTotals = GetTotalPrice();
            var sh = (from s in db.States
                      join u in db.Users on s.StateName equals u.UserBillingState
                      where u.UserEmail.Equals(userName)
                      select new
                      {
                          s.ShippingFee
                      }).FirstOrDefault();
            var getcount = GetTotalQuantity();
            var sfee = GetTotalPriceForProductDeliveryPrice();
            var shippingFee = (sfee / getcount);

            var shippingAndHandling = sh.ShippingFee + shippingFee;
            var tax = 0.00m;
            var discount = 0.00m;
            decimal orderTotal = 0.00m;
            orderTotal = cartTotals + shippingAndHandling + tax + discount;
            Session["TotalAmount"] = orderTotal;
            // store total price for api final output ends here
            if (paymentOption == "payOnDelivery")
            {
                try
                {
                    var saveDeliveryOption = "";
                    if(deliveryOption=="deliveryTeam")
                    {
                        saveDeliveryOption = "Delivery Team";
                    }
                    else
                    {
                        saveDeliveryOption = "Self Pick Up";
                    }
                    var transNum = Guid.NewGuid();
                    HttpCookie anonymousID = Request.Cookies["CartSessionID"];
                    if (anonymousID != null)
                    {
                        var cartItems = (from c in db.ShoppingCarts.Where(x => x.CartSessionID == anonymousID.Value.ToString())
                                         select new FEShoppingCartViewModel
                                         {
                                             ProductID = c.ProductID,
                                             CartSessionID = c.CartSessionID,
                                             ProductName = c.ProductName,
                                             ProductPrice = c.ProductPrice,
                                             ProductSku = c.ProductSku,
                                             ProductTotalPrice = c.ProductQuantity * c.ProductPrice,
                                             ProductQuantity = c.ProductQuantity,
                                             FrontViewThumbNail = c.FrontViewThumbNail.Substring(1),
                                             DateCreated = c.DateCreated,

                                         }).ToList();
                        var userRecord = (from u in db.Users.Where(x => x.UserEmail.Equals(userName)) select u).FirstOrDefault();
                        //decimal orderTotal = Convert.ToDecimal(Session["TotalAmount"].ToString());
                        
                        foreach (var items in cartItems)
                        {
                            //orderTotal = items.ProductQuantity * items.ProductPrice;
                            var addOrder = new Order
                            {
                                ProductID = items.ProductID,
                                ProductModelNumber = items.ProductSku,
                                UserName = userName,
                                OrderAmount = items.ProductTotalPrice,
                                OrderUnitPrice = items.ProductPrice,
                                OrderDate = Convert.ToDateTime(items.DateCreated),
                                OrderQuantity = items.ProductQuantity,
                                OrderDeliveryOption = saveDeliveryOption,
                                OrderDeliveryStatus = "Pending",
                                OrderPaymentOption = "Pay On Delivery",
                                OrderTransactionNumber = transNum.ToString(),
                                OrderTransactionStatus = "Successful",
                                IsDeleted = false,
                                OrderType = "Online",
                                GrandTotalAmount = orderTotal,
                                UserID = userRecord.UserID,
                                
                            };
                             
                            var getProductName = (from g in db.Products.Where(x => x.ProductID.Equals(addOrder.ProductID)) select g).FirstOrDefault();
                            var od = new OrderDetail
                            {
                                OrderID = addOrder.OrderID,
                                ProductID = addOrder.ProductID,
                                ProductPrice = addOrder.OrderUnitPrice,
                                ProductQuantity = addOrder.OrderQuantity,
                                ProductSKU = addOrder.ProductModelNumber,
                                CreatedOn = addOrder.OrderDate,
                                IsDeleted = false,
                                ProductName = getProductName.ProductName
                            };
                            addOrder.OrderDetails.Add(od);
                            db.Orders.Add(addOrder);
                            db.SaveChanges();                            
                        }

                        EmailNotification.SendEmailAttachmentUsingItextSharp(userName, transNum.ToString());
                        //var userPhone = userRecord.UserPhone;
                        string smsNotificationNumbers = ConfigurationManager.AppSettings["SmsNotificationNumbers"].ToString();
                        SMSNotification.SendSMSAlertToCustomerOnSuccessfulTransaction(smsNotificationNumbers);
                        //SMSNotification.SendSms(anonymousID.Value.ToString(), userRecord.UserLastName, orderTotal, smsNotificationNumbers, "Successful");
                       
                        var updateQuantity = (from c in db.ShoppingCarts where (c.CartSessionID == anonymousID.Value.ToString()) select c).ToList();

                        if (updateQuantity != null)
                        {
                            try
                            {
                                foreach (var upQuan in updateQuantity)
                                {
                                    var getProduct = (from p in db.Products.Where(x => x.ProductID.Equals(upQuan.ProductID)) select p).FirstOrDefault();
                                    if (getProduct != null)
                                    {
                                        getProduct.ProductQuantity = getProduct.ProductQuantity - upQuan.ProductQuantity;
                                        if (getProduct.ProductQuantity == 0)
                                        {
                                            getProduct.ProductAvailability = "Unavailable";
                                        }
                                    }
                                }
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                ViewBag.DisplayMessage = "Info";
                                ModelState.AddModelError("", ex.Message);
                            }
                        }

                        return RedirectToAction("OrderCompleted", "Home");
                    }
                }
                catch(Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", ex.Message);
                }
               
            }
            else if (paymentOption == "payWithCreditCard")
            {
                //Session["TotalPrice"] = GetTotalPrice();
                return RedirectToAction("ProcessPayment", "PaymentGateway");
            }
            return View();
        }

        public ActionResult OrderCompleted()
        {
            if (Session["cart"] != null && Session["username"] != null)
            {
                try
                {
                    Session["cart"] = null;
                    var username = Session["username"].ToString();
                    db.ShoppingCarts.RemoveRange(db.ShoppingCarts.Where(c => c.UserName == username));
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", ex.Message);
                }
                
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult AccountVerification()
        {
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["activationCode"]) ? Request.QueryString["activationCode"] : Guid.Empty.ToString();
            if (!string.IsNullOrEmpty(activationCode))
            {
                try
                {
                    var getRecord = (from u in db.Users.Where(x => x.ActivationID.ToString().Equals(activationCode)) select u).FirstOrDefault();
                    if (getRecord != null && getRecord.IsActivated == false)
                    {
                        getRecord.IsActivated = true;
                        db.SaveChanges();
                        ViewBag.DisplayMessage = "success";
                        ModelState.AddModelError("", "Your account has been successfully activated.");
                    }
                    else if (getRecord != null && getRecord.IsActivated == true)
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", "Your account has already been activated, kindly login with your username and password!");
                    }
                    else
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", "Invalid Activation code!");
                    }
                }
                catch(Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", ex.Message);
                }
               
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult ViewOrderDetails(int id, string name)
        {
            //var transID4 = Request.QueryString["id"].ToString();
            FECombineOrderDetailsViewModel model = new FECombineOrderDetailsViewModel();
            if (Session["username"] != null)
            {
                var username = Session["username"].ToString();
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
                                                     UserBillingFirstName =u.UserBillingFirstName,
                                                     UserBillingLastName = u.UserBillingLastName,
                                                     UserBillingPhone= u.UserBillingPhone
                                                 }).FirstOrDefault();
                        model.OrderItems = orderDetailsItems;
                       
                        ViewBag.CartTotal = GetTotalPriceForOrderSummary(name);                       
                        if (ViewBag.CartTotal > 0)
                        {
                            //ViewBag.CartTotals = GetTotalPriceForOrderSummary(name);
                            var sh = (from s in db.States
                                      join u in db.Users on s.StateName equals u.UserBillingState
                                      where u.UserEmail.Equals(username)
                                      select new
                                      {
                                          s.ShippingFee                                          
                                      }).FirstOrDefault();

                            var getcount = GetTotalQuantity();
                            var sfee = GetTotalPriceForProductDeliveryPrice();
                            var shippingFee = (sfee / getcount);

                            ViewBag.ShippingAndHandling = sh.ShippingFee + shippingFee;
                            ViewBag.Tax =0.00m;
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