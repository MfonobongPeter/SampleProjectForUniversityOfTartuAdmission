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

namespace OnlineStore.Controllers
{
    public class UsersController : Controller
    {
        OnlineStoreEntities db = new OnlineStoreEntities();
        // GET: Users
        

        public ActionResult SignUp()
        {
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    ViewBag.StateId = new SelectList(db.States, "StateID", "StateName");
                    // ViewBag.StateId = new SelectList(db.States, "StateName", "StateName");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "UserID,UserFirstName,UserLastName,UserAddress1,UserAddress2,CountryID,UserStateList,UserCity,UserEmail, "+
           " UserPassword,UserConfirmPassword,SecurityQuestionIDList,SecurityAnswer,UserPhone,CreatedOn,Gender,IsDeleted,UserRole")] SignUpViewModel suvm, string lga, string State, string terms, string txtState, string txtCity)
        {
            try
            {

                ViewBag.StateId = new SelectList(db.States, "StateID", "StateName");
                string termsAndCondition = (terms == "yes") ? "Agreed" : "Not Agreed";               
               
                //var state = (txtState != "") ? txtState : State;
                string state = "";
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
                   // string sexGender = suvm.Gender;
                    int sqs = suvm.SecurityQuestionIDList.Value;
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
                        SecurityQuestionID = Convert.ToInt16(suvm.SecurityQuestionIDList),
                        SecurityAnswer = encriptPwd.Encrypt(suvm.SecurityAnswer),
                        UserPhone = suvm.UserPhone,
                        CreatedOn = DateTime.Now,
                        GenderID = suvm.Gender.Value,
                        IsDeleted = false,
                        CountryID = suvm.CountryID,
                        UserRole = suvm.UserRole,
                        IsActivated= false,
                        ActivationID= activationID,
                    };

                    var uir = new UsersInRole
                    {
                        UserID = suvm.UserID,
                        RoleID = Convert.ToInt32(suvm.UserRole)
                    };
                    LoadDropDownList();
                    var chkExistingEmail = (from l in db.Users
                                   where l.UserEmail == suvm.UserEmail
                                   select l).FirstOrDefault();
                    if(chkExistingEmail == null)
                    {
                        try 
                        {
                            string newActivationID = activationID.ToString();
                            itemCollections.UsersInRoles.Add(uir);
                            db.Users.Add(itemCollections);
                            db.SaveChanges();
                            EmailNotification.ProcessEmailForAccountActivation(suvm.UserEmail, suvm.UserFirstName, suvm.UserLastName, newActivationID, suvm.UserEmail);
                            LoadDropDownList();
                            ViewBag.DisplayMessage = "success";
                            ModelState.AddModelError("", "Record Saved Successfully, an activation link has been sent to your inbox, kindly activate your account so you will be able to login!");
                        }
                        catch(Exception ex)
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
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
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
               Value = m.StateID.ToString(),
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
                //security dropdown starts here
                var sQuestions = (from sq in db.SecurityQuestions
                                  select new
                                  {
                                      sq.SecurityQuestionID,
                                      sq.Question
                                  }
                ).ToList();
                var items = sQuestions.Select(t => new SelectListItem
                {
                    Text = t.Question,
                    Value = t.SecurityQuestionID.ToString()
                }).ToList();

                ViewBag.sQuestionsVB = items;


                //Gender drop down starts here
                var genderList = (from gnd in db.Genders
                                  select new
                                  {
                                      gnd.GenderID,
                                      gnd.GenderName
                                  }
                ).ToList();
                var genderItems = genderList.Select(t => new SelectListItem
                {
                    Text = t.GenderName,
                    Value = t.GenderID.ToString()
                }).ToList();

                ViewBag.cGenderList = genderItems;
                //country dropdown list starts here
                var countryList = (from cty in db.Countries
                                   select new
                                   {
                                       cty.CountryID,
                                       cty.CountryName
                                   }
                        ).ToList();
                var countryItems = countryList.Select(t => new SelectListItem
                {
                    Text = t.CountryName,
                    Value = t.CountryID.ToString()
                    //Value = t.CountryName
                }).ToList();

                ViewBag.cCountryList = countryItems;
                //country dropdown list ends here


                //Role drop downlist starts here
                var rolesList = (from r in db.Roles
                                   select new
                                   {
                                       r.RoleiD,
                                       r.RoleName
                                   }
                        ).ToList();
                var roleItems = rolesList.Select(t => new SelectListItem
                {
                    Text = t.RoleName,
                    Value = t.RoleiD.ToString()
                    //Value = t.CountryName
                }).ToList().Take(2);

                ViewBag.cRoleList = roleItems;
                //role drop list ends
            }
            catch (Exception ex)
            {
                ViewBag.DispMsg = false;
                ModelState.AddModelError("", ex.Message);
            }

        }

        public ActionResult Show()
        {
            object showUsers = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    showUsers = (from s in db.Users
                                 join c in db.Countries
                                 on s.CountryID
                                 equals c.CountryID
                                 where s.IsDeleted == false
                                 select new ShowSignUpViewModel
                                 {
                                     UserID = s.UserID,
                                     FirstName = s.UserFirstName,
                                     LastName = s.UserLastName,
                                     Address1 = s.UserAddress1,
                                     Country = c.CountryName,
                                     State = s.UserState,
                                     City = s.UserCity,
                                     Email = s.UserEmail,
                                     Phone = s.UserPhone,
                                     CreatedOn = s.CreatedOn
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

            return View(showUsers);
        }


        public ActionResult Details(int? id)
        {
            object showUsers = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    showUsers = (from s in db.Users
                                 join c in db.Countries
                                 on s.CountryID
                                 equals c.CountryID
                                 join g in db.Genders on s.GenderID equals g.GenderID
                                 where s.UserID == id
                                 select new ShowSignUpViewModel
                                 {
                                     UserID = s.UserID,
                                     Gender = g.GenderName,
                                     FirstName = s.UserFirstName,
                                     LastName = s.UserLastName,
                                     Address1 = s.UserAddress1,
                                     Country = c.CountryName,
                                     State = s.UserState,
                                     City = s.UserCity,
                                     Email = s.UserEmail,
                                     Phone = s.UserPhone,
                                     BillingCity = s.UserBillingCity,
                                     BillingState = s.UserBillingState,
                                     BillingCountry = s.UserBillingCountry,
                                     BillingAddress = s.UserBillingAddress,
                                     CreatedOn = s.CreatedOn
                                 }).FirstOrDefault();
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
            return View(showUsers);
        }


        public ActionResult Delete(int? id)
        {
            object showUsers = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 3)
                {
                    showUsers = (from s in db.Users
                                     join c in db.Countries
                                     on s.CountryID
                                     equals c.CountryID
                                     where s.UserID == id
                                     select new ShowSignUpViewModel
                                     {
                                         UserID = s.UserID,
                                         FirstName = s.UserFirstName,
                                         LastName = s.UserLastName,
                                         Address1 = s.UserAddress1,
                                         Country = c.CountryName,
                                         State = s.UserState,
                                         City = s.UserCity,
                                         Email = s.UserEmail,
                                         Phone = s.UserPhone,
                                         CreatedOn = s.CreatedOn
                                     }).FirstOrDefault();
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
            
            return View(showUsers);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var resetUserIsDeleted = (from u in db.Users.Where(x => x.UserID == id) select u).FirstOrDefault();
                if (resetUserIsDeleted != null)
                {
                    resetUserIsDeleted.IsDeleted = true;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }
            return RedirectToAction("Show");
        }

        //[Route("Admin/AdminLogin")]
        public ActionResult AdminLogin()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.RemoveAll();
            Session.Clear();
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AdminLogin(string emailAddress,string password)
        {
            try
            {
                var PwdHashing = new PasswordHashing();
                //string username = emailAddress;
                //string passwords = password;
                //int role = 0;
                var chkUser = (from l in db.Users
                               //join r in db.Roles on l.UserRole 
                               //equals Convert.ToInt16(r.RoleiD)
                               where l.UserEmail == emailAddress && l.IsDeleted == false && l.IsActivated == true
                               select l).FirstOrDefault();

                if (chkUser != null)
                {                   
                    try
                    {
                        var decriptPwd = PwdHashing.Decrypt(chkUser.UserPassword);
                        if (chkUser.UserEmail == emailAddress && decriptPwd == password)
                        {
                            Session["username"] = chkUser.UserEmail;
                            Session["password"] = chkUser.UserPassword;
                            Session["userRole"] = chkUser.UserRole;

                            return RedirectToAction("SecurityQuestion", "SecurityQuestions");
                        }
                        else
                        {
                            ViewBag.DisplayMessage = "Info";
                            ModelState.AddModelError("", "Email or Password not valid!");
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", "Database Password not encripted! " + ex.Message);
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
                ModelState.AddModelError("", "login unsuccessful, please check your network connection!" + ex.Message);
               // return View();
            }
            return View();
        }

        [HttpGet]
        public ActionResult ForgetPassword()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(string emailAddress)
        {
            var PwdHashing = new PasswordHashing();
            
            var chkUser = (from l in db.Users
                           where l.UserEmail == emailAddress
                           select l).FirstOrDefault();

            if(chkUser != null)
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
                    ProcessEmail(newPassword, chkUser.UserEmail, chkUser.UserFirstName, chkUser.UserLastName);
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Password reset was successful, a new password has been sent to your email address!");
                }
                catch(Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Password reset not successful!");
                }
            }
            else
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Email address does not exist!");
            }
            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if(Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 2 || role == 3)
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

        [HttpPost]
        public ActionResult ChangePassword(ChangeOldPasswordViewModel suvm)
        {
            var PwdHashing = new PasswordHashing();
            string emailAddress = Session["Username"].ToString();
            var chkUser = (from l in db.Users
                           where l.UserEmail == emailAddress
                           select l).FirstOrDefault();
            if (chkUser != null)
            {
                try
                {
                    var decriptPwd = PwdHashing.Encrypt(suvm.UserPassword);
                    chkUser.UserPassword = decriptPwd;
                    chkUser.UserConfirmPassword = decriptPwd;
                    db.SaveChanges();                   
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Your password reset was successful!");
                }
                catch (Exception)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "password reset was not successful, please try again!");
                }
            }
            else
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Email address does not exist!");
            }
            return View();
        }

       
        private string GetBody(string caption, string newPassword)
        {
            var body = new StringBuilder();
            body.Append("<html>");
            body.Append("<head>");
            body.Append("</head>");
            body.Append("<body>");

            body.Append(caption);

            body.Append("<table>");
            body.Append("<tr>");
            body.Append("<td style='border-left: solid 1px #FF7F50; background: #FF7F50';bgcolor: #FF7F50'>");
            body.Append("New Password:");
            body.Append("</td>");
            body.Append("<td style = 'border-left:2px;border-right: 2px;border-top: 3px;border-bottom: 3px;background: #e3eee4;bgcolor: #e3eee4'>");
            body.Append(newPassword);
            body.Append("</td>");
            body.Append("</tr>");            

            body.Append("<tr>");
            body.Append("<td style='border-left: solid 1px #FF7F50; background: #FF7F50';bgcolor: #FF7F50'>");
            body.Append("");
            body.Append("</td>");
            body.Append("<td style = 'border-left:2px;border-right: 2px;border-top: 3px;border-bottom: 3px;background: #e3eee4;bgcolor: #e3eee4'>");
            body.Append("Click <a href = 'http://www.gloriousevidence.com/'>here<a/> to login.");
            body.Append("</td>");
            body.Append("</tr>");

            body.Append("</table>");
            body.Append("========================================<br/>");
            body.Append("<p>Best Regards<p/> <br/>");
            body.Append("<b>Admin, Glorious Evidence Nig Ltd<b/>");
            body.Append("</body>");
            body.Append("</html>");
            return body.ToString();
        }

        public bool ProcessEmail(string newPassword,string emailAddress, string firstname,string surname)
        {
            try
            {
                var emailFrom = WebConfigurationManager.AppSettings["SendingEmailAddress"];
                bool UseSsl = bool.Parse(WebConfigurationManager.AppSettings["SMTPRequireSSL"]); //false;
                string Username = WebConfigurationManager.AppSettings["SMTPUserName"];//"info@shodsystems.com";
                string Password = WebConfigurationManager.AppSettings["SMTPPassword"]; //"Shod@2015";
                // string ServerName = "smtpout.europe.secureserver.net";
                string ServerName = WebConfigurationManager.AppSettings["SMTPClient"]; //"relay-hosting.secureserver.net";     
                int Port = int.Parse(WebConfigurationManager.AppSettings["SMTPPortNumber"]); //25;

                MailAddress MailFromAddress = new MailAddress(emailFrom, "Glorious Evidence Ltd");
                string caption = "Dear " + firstname + " " + surname + ", <br/>Your password has been reset, your new password is given below, kindly change your new password upon first login <br/><br/>";

                var message = new MailMessage();
                message.To.Add(new MailAddress(emailAddress));

                message.From = new MailAddress(emailFrom, "Password Reset On Glorious Evidence");
                message.Subject = "Successful Password Reset On Glorious Evidence Nig Ltd";
                message.Body = GetBody(caption, newPassword);
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure;


                var smtp = new SmtpClient
                {
                    Host = ServerName,
                    Port = Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = UseSsl,
                    Credentials = new NetworkCredential(Username, Password),
                    Timeout = 500000//For Godaddy
                };

                smtp.Send(message);
                message = null;
                smtp = null;
                return true;

            }
            catch (SmtpException ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Error: " + ex.Message);
                return false;
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
