using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineStore;

namespace OnlineStore.Controllers
{
    public class CountriesController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();

        // GET: Countries
        public  ActionResult Show()
        {
            object countryList = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    countryList =
                (from u in db.Countries

                 select new CountryViewModel
                 {
                     CountryID = u.CountryID,
                     CountryName = u.CountryName

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
            
            return View(countryList);
        }

        // GET: Countries/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            object country = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    country = await db.Countries.FindAsync(id);
                    if (country == null)
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
            
            return View(country);
        }

        // GET: Countries/Create
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

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([Bind(Include = "CountryID,CountryName")] CountryViewModel country)
        {
            if (ModelState.IsValid)
            {
                var isExist = (from l in db.Countries.Where(x => x.CountryName == country.CountryName) select l).FirstOrDefault();
                if (isExist == null)
                {
                    var saveCountry = new Country
                    {
                        CountryName = country.CountryName
                    };
                    try
                    {
                        db.Countries.Add(saveCountry);
                        await db.SaveChangesAsync();
                        ViewBag.DisplayMessage = "success";
                        ModelState.AddModelError("",  " Saved Successfully!");
                    }
                    catch (Exception ex)
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", "Error: " + ex.Message);
                    }
                }
                else
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Country Name " + isExist.CountryName + " already exist, Please enter a new country!");
                   // LoadDropDownList();
                }
            }

            return View(country);
        }

        // GET: Countries/Edit/5
        public ActionResult Edit(int? id)
        {
            Country country = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    country = db.Countries.Find(id);
                    if (country == null)
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
            
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CountryID,CountryName")] Country country)
        {
            if (country.CountryName != null)
            {
                var countryList = (from d in db.Countries.Where(x => x.CountryID == country.CountryID) select d).FirstOrDefault();
                if (countryList != null)
                {
                    try
                    {
                        countryList.CountryName = country.CountryName;
                        db.SaveChanges();
                        ViewBag.DisplayMessage = "success";
                        ModelState.AddModelError("", "Updated Successfully!");

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
                    ModelState.AddModelError("", "Info: Record does not exist!" );
                }
             // return RedirectToAction("Show");
            }
            else
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Enter Category Name!");
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            Country country = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    country = await db.Countries.FindAsync(id);
                    if (country == null)
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
            
            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Country country = await db.Countries.FindAsync(id);
                db.Countries.Remove(country);
                await db.SaveChangesAsync();
                //return RedirectToAction("Show");
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
