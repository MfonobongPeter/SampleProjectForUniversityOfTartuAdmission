using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineStore;
using System.Web.Configuration;
using System.Net.Mail;
using System.Text;

namespace OnlineStore.Controllers
{
    public class SecurityQuestionsController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();
        PasswordHashing PwdHashing = new PasswordHashing();
        // GET: SecurityQuestions
        public ActionResult Show()
        {
            object sq = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    sq = db.SecurityQuestions.ToList();
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
            return View(sq);
        }

        // GET: SecurityQuestions/Details/5
        public ActionResult Details(int? id)
        {
            SecurityQuestion securityQuestion = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    securityQuestion = db.SecurityQuestions.Find(id);
                    if (securityQuestion == null)
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
            
            return View(securityQuestion);
        }

        // GET: SecurityQuestions/Create
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

        // POST: SecurityQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "SecurityQuestionID,Question")] SecurityQuestion securityQuestion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isExist = (from l in db.SecurityQuestions.Where(x => x.Question == securityQuestion.Question) select l).FirstOrDefault();
                     if (isExist == null)
                     {
                         db.SecurityQuestions.Add(securityQuestion);
                         db.SaveChanges();
                         // return RedirectToAction("Show");
                         ViewBag.DisplayMessage = "success";
                         ModelState.AddModelError("", "Question saved successfully!");
                     }
                     else
                     {
                         ViewBag.DisplayMessage = "Info";
                         ModelState.AddModelError("", "Security question " + isExist.Question + " already exist, Please enter a new question!");
                         //LoadDropDownList();
                     }
                }
                catch(Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Error!" + ex.Message);
                }
                
            }

            return View(securityQuestion);
        }

        // GET: SecurityQuestions/Edit/5
        public ActionResult Edit(int? id)
        {
            SecurityQuestion securityQuestion = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    securityQuestion = db.SecurityQuestions.Find(id);
                    if (securityQuestion == null)
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
            
            return View(securityQuestion);
        }

        // POST: SecurityQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SecurityQuestionID,Question")] SecurityQuestion securityQuestion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(securityQuestion).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Question updated successfully!");
                }
                catch(Exception ex)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Error!" + ex.Message);
                }
            }
            return View(securityQuestion);
        }

        // GET: SecurityQuestions/Delete/5
        public ActionResult Delete(int? id)
        {
            SecurityQuestion securityQuestion = null;
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 3)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    securityQuestion = db.SecurityQuestions.Find(id);
                    if (securityQuestion == null)
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
            
            return View(securityQuestion);
        }

        // POST: SecurityQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                SecurityQuestion securityQuestion = db.SecurityQuestions.Find(id);
                db.SecurityQuestions.Remove(securityQuestion);
                db.SaveChanges();
                ViewBag.DisplayMessage = "success";
                ModelState.AddModelError("", "Security Question deleted successfully!");
                //return RedirectToAction("Show");
            }
           catch(Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Error!" + ex.Message);
            }
            return View();
        }

        


        public ActionResult SecurityQuestion()
        {
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 2 || role == 3)
                {
                    var sqtn = Session["username"].ToString();
                    try
                    {

                        var chkUser = (from l in db.Users
                                       join k in db.SecurityQuestions
                                           on l.SecurityQuestionID
                                           equals k.SecurityQuestionID
                                       where l.UserEmail == sqtn
                                       select new
                                       {
                                           l.UserLastName,
                                           l.UserFirstName,
                                           l.SecurityAnswer,
                                           k.Question
                                           //l.SecurityAnswer
                                       }).FirstOrDefault();
                        if (chkUser != null)
                        {
                            Session["firstname"] = chkUser.UserFirstName;
                            Session["SQ"] = chkUser.Question;
                            Session["SA"] = chkUser.SecurityAnswer;

                        }
                        //else
                        //{
                        //    ModelState.AddModelError("", "Security Answer not valid!");
                        //}
                    }
                    catch (Exception ex)
                    {
                        ViewBag.DisplayMessage = "Info";
                        ModelState.AddModelError("", "Security Answer not valid!" + ex.Message);
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
        // [ValidateAntiForgeryToken()]
        public ActionResult SecurityQuestion(string securityAnswer)
        {
            try
            {
               
                var sqtn = Session["username"].ToString();
                var sas = Session["SA"].ToString();

                var seDecript = PwdHashing.Decrypt(sas);
                if (seDecript.ToLower() == securityAnswer.ToLower())
                {
                    var role = Convert.ToInt16(Session["userrole"].ToString());
                    if (role == 1)
                    {
                        return RedirectToAction("DashBoard", "Admindefault");
                    }
                    else if (role == 2)
                    {
                        return RedirectToAction("DashBoard", "Staff");
                    }
                    else if (role == 3)
                    {
                        return RedirectToAction("DashBoard", "Developer");
                    }
                   
                }
                else
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Security Answer not valid!");
                }
            }
            catch (Exception)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "The entered security answer is invalid.");
            }

            return View();
        }

        public ActionResult ForgetSecurityAnswer()
        {
            if (Session["Username"] != null)
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
        public ActionResult ForgetSecurityAnswer(string emailAddress)
        {
            
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
                    buffer.Length = 8;
                    var newSecurityAnswer = buffer.ToString().ToUpper();

                    var decriptPwd = PwdHashing.Encrypt(newSecurityAnswer);
                    chkUser.SecurityAnswer = decriptPwd;
                    //chkUser.UserConfirmPassword = decriptPwd;
                    db.SaveChanges();
                    ProcessEmail(newSecurityAnswer, chkUser.UserEmail, chkUser.UserFirstName, chkUser.UserLastName);
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Security answer reset was successful, a new security answer has been sent to your email address!");
                }
                catch (Exception)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Security answer was not successful, please contact the admin!");
                }
            }
            else
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", "Email address does not exist!");
            }
            return View();
        }

        public ActionResult ChangeSecurityQuestion()
        {
            if (Session["Username"] != null)
            {
                var role = Convert.ToInt16(Session["userrole"].ToString());
                if (role == 1 || role == 2 || role == 3)
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
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Info";
                ModelState.AddModelError("", ex.Message);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeSecurityQuestion([Bind(Include = "SecurityAnswer,SecurityQuestion")] SecurityQuestionViewModel suvm)
        {
            string emailAddress = Session["Username"].ToString();
            var chkUser = (from l in db.Users
                           where l.UserEmail == emailAddress
                           select l).FirstOrDefault();
            if(chkUser != null)
            {
                try
                {
                    
                    var decriptPwd = PwdHashing.Encrypt(suvm.SecurityAnswer);
                    chkUser.SecurityAnswer = decriptPwd;
                    chkUser.SecurityQuestionID = suvm.SecurityQuestion.Value;                                      
                    db.SaveChanges();                   
                    ViewBag.DisplayMessage = "success";
                    ModelState.AddModelError("", "Your security answer reset was successful!");
                    LoadDropDownList();
                }
                catch (Exception)
                {
                    ViewBag.DisplayMessage = "Info";
                    ModelState.AddModelError("", "Security answer was not successful, please contact the admin!");
                    LoadDropDownList();
                }
            }            
            return View();
        }


       


        private string GetBody(string caption, string newSecurityAnswer)
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
            body.Append("New Security Answer:");
            body.Append("</td>");
            body.Append("<td style = 'border-left:2px;border-right: 2px;border-top: 3px;border-bottom: 3px;background: #e3eee4;bgcolor: #e3eee4'>");
            body.Append(newSecurityAnswer);
            body.Append("</td>");
            body.Append("</tr>");

            body.Append("<tr>");
            body.Append("<td style='border-left: solid 1px #FF7F50; background: #FF7F50';bgcolor: #FF7F50'>");
            body.Append("");
            body.Append("</td>");
            body.Append("<td style = 'border-left:2px;border-right: 2px;border-top: 3px;border-bottom: 3px;background: #e3eee4;bgcolor: #e3eee4'>");
            body.Append("Click <a href = 'http://www.gloriousevidence.com/Admin'>here<a/> to login.");
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

        public bool ProcessEmail(string newPassword, string emailAddress, string firstname, string surname)
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
                string caption = "Dear " + firstname + " " + surname + ", <br/>Your security answer has been reset, your new security answer is given below,<br/> kindly change your new security answer upon first login to answer your secutiy question or to something meaningful. <br/><br/>";

                var message = new MailMessage();
                message.To.Add(new MailAddress(emailAddress));

                message.From = new MailAddress(emailFrom, "Security Answer Reset On Glorious Evidence");
                message.Subject = "Successful Security Answer Reset On Glorious Evidence Nig Ltd";
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
                    Timeout = 100000//For Godaddy
                };

                smtp.Send(message);
                message = null;
                smtp = null;
                return true;

            }
            catch (SmtpException)
            {
                //JUtility.ErrorLog.LogApplicationError(ex);
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
