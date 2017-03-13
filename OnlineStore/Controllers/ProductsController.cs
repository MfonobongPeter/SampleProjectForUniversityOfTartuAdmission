using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineStore;
using System.IO;
using System.Configuration;
using System.Text;
using System.Data.Entity.Validation;

namespace OnlineStore.Controllers
{
    public class ProductsController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();

        // GET: Products
        public ActionResult Show()
        {
            object product = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 2 || role == 3)
                {
                    // var products = db.Products.Include(p => p.SubCategory);
                    product = (from p in db.Products
                               select new ProductViewEditViewModel
                               {
                                   ProductID = p.ProductID,
                                   ProductSKU = p.ProductSKU,
                                   Quantity = p.ProductQuantity,
                                   Price = p.ProductUnitPrice,
                                   Name = p.ProductName,
                                   ShortDescription = p.ProductshortDesc,
                                   LongDescription = p.ProductLongDesc,
                                   //CartDescription = p.ProductCartDesc,
                                   Location = p.ProductLocation,
                                   Weight = p.ProductWeight,
                                   Availability = p.ProductAvailability,
                                   Colour = p.ProductColour
                               }).ToList();
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
           
            return View(product);
        }

        // GET: Products/Details/5
        public ActionResult Details(long? id)
        {
            object productList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 2 || role == 3)
                {
                    try
                    {
                        productList =
                                        (from u in db.Products
                                         where (u.ProductID == id)
                                         select new ProductViewEditViewModel
                                         {
                                             ProductID = u.ProductID,
                                             ProductSKU = u.ProductSKU,
                                             Quantity = u.ProductQuantity,
                                             Price = u.ProductUnitPrice,
                                             Name = u.ProductName,
                                             ShortDescription = u.ProductshortDesc,
                                             LongDescription = u.ProductLongDesc,
                                             //CartDescription = u.ProductCartDesc,
                                             Location = u.ProductLocation,
                                             Weight = u.ProductWeight,
                                             Availability = u.ProductAvailability,
                                             Colour = u.ProductColour,
                                             ProductCreatedOn = u.ProductCreatedOn,
                                             CreatedBy = u.ProductCreatedBy
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
            return View(productList);
        }

        // GET: Products/Create
        public ActionResult Add()
        {

            ProductViewModel pr = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 2 || role == 3)
                {
                    StringBuilder buffer;


                    String sku = System.Guid.NewGuid().ToString();
                    buffer = new StringBuilder(sku);
                    buffer.Length = 8;
                    LoadDropDownList();
                    pr = new ProductViewModel()
                    {
                        ProductSKU = "GE-" + buffer.ToString().ToUpper()
                    };
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
            //ViewBag.SubCategoryID = new SelectList(db.SubCategories, "SubCategoryID", "CreatedBy");           
            return View(pr);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include =
            "ProductID,CategoryID,ProductSKU,ProductQuantity,ProductUnitPrice,ProductName,ProductshortDesc,ProductLongDesc,ProductLocation,ProductWeight,ProductCreatedBy,ProductCreatedOn,ProductDiscount,ProductAvailability,ProductColour,ShippingFee")] ProductViewModel product, HttpPostedFileBase ProductFrontView, HttpPostedFileBase ProductFrontViewThumbnail, HttpPostedFileBase ProductSideView, HttpPostedFileBase ProductSideViewThumbnail, string SubCategory)
        {
            //if (ModelState.IsValid)
            //{
            var quantity = product.ProductQuantity;
            var productAvailability = "";
            int subCatID = int.Parse(SubCategory.ToString());
            var prodSku = (from p in db.Products.Where(p=>p.ProductSKU == product.ProductSKU) select p).FirstOrDefault();

            

            if (quantity != null)
            {
                productAvailability = "Available";
            }
            try
            {
                Int32 frontViewUploadImageSize = Convert.ToInt32(ConfigurationManager.AppSettings["FrontViewUploadImageSize"]); //70kb max
                Int32 sidetViewUploadImageSize = Convert.ToInt32(ConfigurationManager.AppSettings["SideViewUploadImageSize"]); //70kb max
                Int32 frontViewThumbNailUploadImageSize = Convert.ToInt32(ConfigurationManager.AppSettings["FrontViewThumbNailUploadImageSize"]); //15kb max
                Int32 sideViewThumbNailUploadImageSize = Convert.ToInt32(ConfigurationManager.AppSettings["SideViewThumbNailUploadImageSize"]); //15kb max

                Int32 requiredNormalViewHeight = Convert.ToInt32(ConfigurationManager.AppSettings["RequiredNormalViewHeight"]); //700px max
                Int32 requiredNormalViewWidth = Convert.ToInt32(ConfigurationManager.AppSettings["RequiredNormalViewWidth"]); //700px max
                Int32 requiredThumbViewHeight = Convert.ToInt32(ConfigurationManager.AppSettings["RequiredThumbViewHeight"]); //250px max
                Int32 requiredThumbViewWidth = Convert.ToInt32(ConfigurationManager.AppSettings["RequiredThumbViewWidth"]); //250px max

                bool requiredFrontViewDimension = ValidateFileDimensions(ProductFrontView, requiredNormalViewHeight, requiredNormalViewWidth);
                bool requiredSideViewDimension = ValidateFileDimensions(ProductSideView, requiredNormalViewHeight, requiredNormalViewWidth);
                bool requiredFrontViewThumbDimension = ValidateFileDimensions(ProductFrontViewThumbnail, requiredThumbViewHeight, requiredThumbViewWidth);
                bool requiredSideViewThumbDimension = ValidateFileDimensions(ProductSideViewThumbnail, requiredThumbViewWidth, requiredThumbViewWidth);

                if(!requiredFrontViewDimension)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Front View image dimension not valid, required dimension is: (height = 700px, width = 700px)");
                    LoadDropDownList();
                    return View();
                }

                if(!requiredSideViewDimension)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Side View image dimension not valid, required dimension is: (height = 700px, width = 700px)");
                    LoadDropDownList();
                    return View();
                }

                if (!requiredFrontViewThumbDimension)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Front View image thumbnail dimension not valid, required dimension is: (height = 250px, width = 250px)");
                    LoadDropDownList();
                    return View();
                }

                if (!requiredSideViewThumbDimension)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Side View image thumbnail dimension not valid, required dimension is: (height = 250px, width = 250px)");
                    LoadDropDownList();
                    return View();
                }

                if (prodSku == null)
                {
                    
                    if (ProductFrontView.ContentLength > 0 && ProductFrontView.ContentLength <= frontViewUploadImageSize)
                    {
                        if (ProductSideView.ContentLength > 0 && ProductFrontView.ContentLength <= sidetViewUploadImageSize)
                        {
                            if (ProductFrontViewThumbnail.ContentLength > 0 && ProductFrontViewThumbnail.ContentLength <= frontViewThumbNailUploadImageSize)
                            {
                                if (ProductSideViewThumbnail.ContentLength > 0 && ProductSideViewThumbnail.ContentLength <= sideViewThumbNailUploadImageSize)
                                {
                                    if (CheckFileType(ProductFrontView.FileName))
                                    {
                                        if (CheckFileType(ProductSideView.FileName))
                                        {
                                            if (CheckFileType(ProductFrontViewThumbnail.FileName))
                                            {
                                                if (CheckFileType(ProductSideViewThumbnail.FileName))
                                                {
                                                    var _productFrontView = Path.GetFileName(ProductFrontView.FileName);
                                                    var _productSideView = Path.GetFileName(ProductSideView.FileName);
                                                    var _productFrontViewThumbNail = Path.GetFileName(ProductFrontViewThumbnail.FileName);
                                                    var _productSideViewThumbNail = Path.GetFileName(ProductSideViewThumbnail.FileName);


                                                    var frontViewConcat = "~/images/product-front-view/" + product.ProductSKU.ToLower() + "-" + SeoFriendlyWithDotsForImageExtension.URLFriendly(_productFrontView);
                                                    var sideViewConcat = "~/images/product-side-view/" + product.ProductSKU.ToLower() + "-" + SeoFriendlyWithDotsForImageExtension.URLFriendly(_productSideView);
                                                    var frontViewThumbNailConCat = "~/images/product-front-view-thumb-nail/" + product.ProductSKU.ToLower() + "-" + SeoFriendlyWithDotsForImageExtension.URLFriendly(_productFrontViewThumbNail);
                                                    var sideViewThumbNailConCat = "~/images/product-side-view-thumb-nail/" + product.ProductSKU.ToLower() + "-" + SeoFriendlyWithDotsForImageExtension.URLFriendly(_productSideViewThumbNail);


                                                    var frontViewImagePath = Path.Combine(Server.MapPath("~/images/product-front-view/"), product.ProductSKU.ToLower() + "-" + SeoFriendlyWithDotsForImageExtension.URLFriendly(_productFrontView));
                                                    var sideViewImagePath = Path.Combine(Server.MapPath("~/images/product-side-view/"), product.ProductSKU.ToLower() + "-" + SeoFriendlyWithDotsForImageExtension.URLFriendly(_productSideView));
                                                    var frontViewThumbNailImagePath = Path.Combine(Server.MapPath("~/images/product-front-view-thumb-nail/"), product.ProductSKU.ToLower() + "-" + SeoFriendlyWithDotsForImageExtension.URLFriendly(_productFrontViewThumbNail));
                                                    var sideViewThumbNailImagePath = Path.Combine(Server.MapPath("~/images/product-side-view-thumb-nail/"), product.ProductSKU.ToLower() + "-" + SeoFriendlyWithDotsForImageExtension.URLFriendly(_productSideViewThumbNail));

                                                    var productName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(product.ProductName.ToLower());
                                                    var insertProductDetail = new Product
                                                    {
                                                        SubCategoryID = subCatID,
                                                        CategoryID = product.CategoryID,
                                                        ProductSKU = product.ProductSKU,
                                                        ProductQuantity = quantity,
                                                        ProductUnitPrice = product.ProductUnitPrice,
                                                        ProductWeight = Convert.ToDecimal(product.ProductWeight),
                                                        ProductName = productName,
                                                        ProductAvailability = productAvailability,
                                                        //ProductShortDesc = product.ProductCartDesc,
                                                        ProductColour = product.ProductColour,
                                                        //ProductImagePath = frontViewConcat,
                                                        ProductLocation = product.ProductLocation,
                                                        ProductLongDesc = product.ProductLongDesc,
                                                        ProductshortDesc = product.ProductshortDesc,
                                                        ProductCreatedBy = Session["username"].ToString(),
                                                        ProductCreatedOn = DateTime.Now,
                                                        ProductFrontView = frontViewConcat,
                                                        ProductSideView = sideViewConcat,
                                                        ProductFrontViewThumbnail = frontViewThumbNailConCat,
                                                        ProductSideViewThumbnail = sideViewThumbNailConCat,
                                                        ShippingFee = product.ShippingFee,
                                                        ProductIsNew = true
                                                    };
                                                    db.Products.Add(insertProductDetail);
                                                    db.SaveChanges();
                                                    ProductFrontView.SaveAs(frontViewImagePath);
                                                    ProductSideView.SaveAs(sideViewImagePath);
                                                    ProductFrontViewThumbnail.SaveAs(frontViewThumbNailImagePath);
                                                    ProductSideViewThumbnail.SaveAs(sideViewThumbNailImagePath);
                                                    UpdateProductIsNew();
                                                    ViewBag.DisplayMessage = "success";
                                                    ModelState.AddModelError("", "Product saved successfully!");
                                                }
                                                else
                                                {
                                                    ViewBag.DisplayMessage = "Info";
                                                    ModelState.AddModelError("", "Product Side View Thumbnail file format not allowed! file format must be in any of these formats: Jpg,Jpeg");
                                                }
                                            }
                                            else
                                            {
                                                ViewBag.DisplayMessage = "Info";
                                                ModelState.AddModelError("", "Product Front View Thumbnail file format not allowed! file format must be in any of these formats: Jpg,Jpeg");
                                            }
                                        }
                                        else
                                        {
                                            ViewBag.DisplayMessage = "Info";
                                            ModelState.AddModelError("", "Product Side View file format not allowed! file format must be in any of these formats: Jpg,Jpeg");
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.DisplayMessage = "Info";
                                        ModelState.AddModelError("", "Product Front View file format not allowed! file format must be in any of these formats: Jpg,Jpeg");
                                    }
                                }
                                else
                                {
                                    ViewBag.DisplayMessage = "Info";
                                    ModelState.AddModelError("", "Side View Thumbnail image file too large! file size should not exceed 30kb");
                                }
                            }
                            else
                            {
                                ViewBag.DisplayMessage = "Info";
                                ModelState.AddModelError("", "Font View Thumbnail image file too large! file size should not exceed 30kb");
                            }
                        }
                        else
                        {
                            ViewBag.DisplayMessage = "Info";
                            ModelState.AddModelError("", "Side View image file too large! file size should not exceed 80kb");
                        }
                    }
                    else
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", "Front View image file too large! file size should not exceed 80kb");
                    }
                }
                else
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Product Model Number(ProductSku) already exist, two products can't have same model number.");
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName,
                            validationError.ErrorMessage);
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", validationError.PropertyName + " " + validationError.ErrorMessage);
                    }
                }
            }


            //}
            //else
            //{
            //    ViewBag.DisplayMessage = "Info";
            //    ModelState.AddModelError("", "Null value in some model items");
            //}
            //ViewBag.LoadCategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            LoadDropDownList();
            return View(product);
        }

        private void UpdateProductIsNew()
        {
            DateTime b = DateTime.Now;
            var isNewUpdate = (from p in db.Products
                               //where p.ProductCreatedOn.Equals(b.Date)
                               select p).ToList();
            if (isNewUpdate != null)
            {
                foreach (var prod in isNewUpdate)
                {
                    DateTime? a = prod.ProductCreatedOn;
                    if (a.Value.Date.Equals(b.Date))
                    {
                        prod.ProductIsNew = true;
                    }
                    else
                    {
                        prod.ProductIsNew = false;
                    }
                }
                db.SaveChanges();
            }
        }

        public bool ValidateFileDimensions(HttpPostedFileBase fileUpload,int height,int width)
        {
            using (System.Drawing.Image myImage = System.Drawing.Image.FromStream(fileUpload.InputStream))
            {
                return (myImage.Height == height && myImage.Width == width);
            }
        }
        public JsonResult SubCatList(int id)
        {
            var category = from s in db.SubCategories
                        where s.CategoryID == id
                        select s;
            return Json(new SelectList(category.ToArray(), "SubCategoryID", "SubCategoryName"), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadSubCategoryByCategoryID(string categoryName)
        {
            var CategoryList = GetSubCategory(Convert.ToInt32(categoryName));
            var CategoryData = CategoryList.Select(m => new SelectListItem()
            {
                Text = m.SubCategoryName,
                // Value = m.StateID.ToString(),
                Value = m.SubCategoryID.ToString()
            });
            return Json(CategoryData, JsonRequestBehavior.AllowGet);
        }

        public IList<SubCategory> GetSubCategory(int categoryId)
        {
            return db.SubCategories.Where(m => m.CategoryID == categoryId).ToList();
        }

        public bool CheckFileType(String fileName)
        {
            try
            {
                switch (Path.GetExtension(fileName).ToLower())
                {                    
                    case ".jpg":
                    case ".jpeg":
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(long? id)
        {
            object productList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 2 || role == 3)
                {
                    try
                    {
                        productList =
                                        (from u in db.Products
                                         where (u.ProductID == id)
                                         select new ProductViewEditViewModel
                                         {
                                             ProductID = u.ProductID,
                                             ProductSKU = u.ProductSKU,
                                             Quantity = u.ProductQuantity,
                                             Price = u.ProductUnitPrice,
                                             Name = u.ProductName,
                                             ShortDescription = u.ProductshortDesc,
                                             LongDescription = u.ProductLongDesc,
                                             //CartDescription = u.ProductCartDesc,
                                             Location = u.ProductLocation,
                                             Weight = u.ProductWeight,
                                             Availability = u.ProductAvailability,
                                             Colour = u.ProductColour,
                                             ShippingFee=u.ShippingFee
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
           
            return View(productList);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductSKU,Quantity,Price,Name,ShortDescription,LongDescription,CartDescription,Location,Weight,Availability,ShippingFee")] ProductViewEditViewModel prod)
        {
            if (prod.Name != null)
            {
                var productList = (from d in db.Products.Where(x => x.ProductID == prod.ProductID) select d).FirstOrDefault();
                if (productList != null)
                {
                    productList.ProductSKU = prod.ProductSKU;

                    productList.ProductQuantity = prod.Quantity;
                    productList.ProductUnitPrice = prod.Price;
                    productList.ProductName = prod.Name;
                    productList.ProductshortDesc = prod.ShortDescription;
                    productList.ProductLongDesc = prod.LongDescription;
                   // productList.ProductCartDesc = prod.CartDescription;
                    productList.ProductLocation = prod.Location;
                    productList.ProductWeight = prod.Weight;
                    productList.ProductAvailability = prod.Availability;
                    productList.ProductColour = prod.Colour;
                    productList.ShippingFee = prod.ShippingFee;
                    db.SaveChanges();
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Product Edited Successfully!");
                }
                else
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Product not found!");
                }
               
            }
            else
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Enter Category Name!");
            }
            return View(prod);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(long? id)
        {
            object productList = "";
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 2 || role == 3)
                {
                    try
                    {
                        productList =
                                        (from u in db.Products
                                         where (u.ProductID == id)
                                         select new ProductViewEditViewModel
                                         {
                                             ProductID = u.ProductID,
                                             ProductSKU = u.ProductSKU,
                                             Quantity = u.ProductQuantity,
                                             Price = u.ProductUnitPrice,
                                             Name = u.ProductName,
                                             ShortDescription = u.ProductshortDesc,
                                             LongDescription = u.ProductLongDesc,
                                             // CartDescription = u.ProductCartDesc,
                                             Location = u.ProductLocation,
                                             Weight = u.ProductWeight,
                                             Availability = u.ProductAvailability,
                                             Colour = u.ProductColour,
                                             ProductCreatedOn = u.ProductCreatedOn,
                                             CreatedBy = u.ProductCreatedBy
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
            
            return View(productList);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
          
            try
            {
                var remove = (from d in db.Products.Where(x => x.ProductID == id) select d).FirstOrDefault();
                if (remove !=null)
                {
                    db.Products.Remove(remove);
                    db.SaveChanges();
                    //ViewBag.DisplayMessage = "success";
                    //ModelState.AddModelError("", "Product deleted successfully!");                   
                }               
            }
            catch(Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
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
                ModelState.AddModelError("", ex.Message);
            }

            //for sub category
            try
            {
                //security dropdown starts here
                var fkSubCategory = (from sCtg in db.SubCategories
                                  select new
                                  {
                                      sCtg.SubCategoryID,
                                      sCtg.SubCategoryName
                                  }
                ).ToList();
                var items = fkSubCategory.Select(t => new SelectListItem
                {
                    Text = t.SubCategoryName,
                    Value = t.SubCategoryID.ToString()
                }).ToList();

                ViewBag.fkSubCategoryList = items;
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }

        }
    }
}
