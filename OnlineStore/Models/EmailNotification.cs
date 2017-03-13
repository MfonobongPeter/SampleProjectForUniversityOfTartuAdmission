using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using System.Configuration;



namespace OnlineStore
{
    public static class EmailNotification
    {
        private static OnlineStoreEntities db = new OnlineStoreEntities();
        public static string Msg = "";
        public static bool ProcessEmailForPasswordReset(string newPassword, string emailAddress, string firstname, string surname)
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
                message.Body = GetBodyForPasswordReset(caption, newPassword);
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
                //ViewBag.DisplayMessage = "Info";
                //ModelState.AddModelError("", "Error: " + ex.Message);
                return false;
            }
        }


        private static string GetBodyForPasswordReset(string caption, string newPassword)
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



        public static bool ProcessEmailForAccountActivation(string emailAddress, string firstname, string surname, string activationCode, string username)
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
                string caption = "Dear " + firstname + " " + surname + ", <br/><br/>Thank you for registering at Glorious Evidence Online Store. To complete your registration, please click on the link below. <br/>If your email system does not allow linking, please copy and paste link into your browser. <br/><br/>";

                var message = new MailMessage();
                message.To.Add(new MailAddress(emailAddress));

                message.From = new MailAddress(emailFrom, "Glorious Evidence Online Store");
                message.Subject = "Account Activation Required";
                string ActivationUrl = caption +"<br/> "+ "http://www.gloriousevidence.com/Home/AccountVerification/?activationCode="+activationCode.ToString();
                //message.Body = GetBodyForAccountActivation(caption, firstname, surname, activationCode, username);
                message.Body = ActivationUrl;
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
            catch (SmtpException)
            {
                //ViewBag.DisplayMessage = "Info";
                //ModelState.AddModelError("", "Error: " + ex.Message);
                return false;
            }
        }

        public static bool ProcessEmailForContactForm(string emailAddress, string firstname, string sentMessage, string lastname, string subject)
        {
            try
            {
               var emailTo = WebConfigurationManager.AppSettings["SendingEmailAddress"];
                bool UseSsl = bool.Parse(WebConfigurationManager.AppSettings["SMTPRequireSSL"]); //false;
                string Username = WebConfigurationManager.AppSettings["SMTPUserName"];//"info@shodsystems.com";
                string Password = WebConfigurationManager.AppSettings["SMTPPassword"]; //"Shod@2015";
                // string ServerName = "smtpout.europe.secureserver.net";
                string ServerName = WebConfigurationManager.AppSettings["SMTPClient"]; //"relay-hosting.secureserver.net";     
                int Port = int.Parse(WebConfigurationManager.AppSettings["SMTPPortNumber"]); //25;

                MailAddress MailFromAddress = new MailAddress(emailAddress, firstname + " " + lastname);
                string caption = "Feedback from: " + firstname + " " + lastname + ", <br/><br/>" + sentMessage + " <br/><br/>";

                var message = new MailMessage();
                message.To.Add(new MailAddress(emailTo));

                message.From = new MailAddress(emailAddress, firstname + " " + lastname);
                message.Subject = subject;
                //string ActivationUrl = caption + "<br/> " + "http://www.gloriousevidence.com/Home/AccountVerification/?activationCode=" + activationCode.ToString();
                //message.Body = GetBodyForAccountActivation(caption, firstname, surname, activationCode, username);
                message.Body = caption;
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
                Msg = "Sent successfully!";
                message = null;
                smtp = null;
                return true;

            }
            catch (SmtpException ex)
            {
                //ViewBag.DisplayMessage = "Info";
                //ModelState.AddModelError("", "Error: " + ex.Message);
                Msg = ex.Message;
                return false;
            }
        }
       


        public static string SendEmailAttachmentUsingItextSharp(string userName,string transactionID)
        {
            
            var getItems = (from u in db.Users
                                        join o in db.Orders on u.UserID equals o.UserID
                                        join cr in db.ShoppingCarts on u.UserEmail equals cr.UserName
                                        join p in db.Products on o.ProductID equals p.ProductID
                                        join l in db.Lgas on u.UserBillingCity equals l.LgaID.ToString()
                                        join c in db.Countries on u.UserBillingCountry equals c.CountryID.ToString()
                                        where (o.OrderTransactionNumber == transactionID)
                                        select new
                                        {
                                            p.ProductName,
                                            p.ProductSKU,
                                            u.UserFirstName,
                                            u.UserLastName,
                                            u.UserBillingAddress,                                                                                        
                                            u.UserBillingFirstName,
                                            u.UserBillingLastName,
                                            u.UserBillingPhone,
                                            u.UserBillingState,
                                            u.UserEmail,
                                            c.CountryName,
                                            l.LgaName,
                                            cr.DateCreated,
                                            o.OrderPaymentOption,
                                            o.OrderDeliveryOption,
                                            o.OrderTransactionNumber,
                                            o.OrderTransactionStatus,
                                            cr.ProductPrice,
                                            cr.ProductTotalPrice
                                        }).FirstOrDefault();
            var gInvoice = (from u in db.Users
                                   
                                   join cr in db.ShoppingCarts on u.UserEmail equals cr.UserName
                                   join p in db.Products on cr.ProductID equals p.ProductID
                                   where (cr.UserName == userName)
                                   select new
                                   {
                                       cr.ProductPrice,
                                       cr.ProductTotalPrice,
                                       cr.ProductQuantity,                                       
                                       cr.ProductSku,
                                       cr.ProductName
                                   }).ToList();

            var shp = (from s in db.States
                      join u in db.Users on s.StateName equals u.UserBillingState
                      where u.UserEmail.Equals(userName)
                      select new
                      {
                          s.ShippingFee
                      }).FirstOrDefault();

            //var getSH = (from sh in db.States select new).FirstOrDefault();

            var getcount = GetTotalQuantity(userName);
            var sfee = GetTotalPriceForProductDeliveryPrice(userName);
            var shippingFee = (sfee / getcount);

            decimal tax = 0.00m;
            decimal shipping = Convert.ToDecimal(shp.ShippingFee + shippingFee);
            decimal subTotal = GetTotalPriceForOrderSummary(userName);
            decimal grandTotal = (subTotal + tax + shipping);

            StringBuilder sb = new StringBuilder();
            sb.Append("<br/>");
            sb.Append("<header class='clearfix'>");
            sb.Append("<img src = 'http://www.gloriousevidence.com/Images/Logo.png' />");
            sb.Append("<div style = 'text-align:right; font-size:small; color:#003418; line-height:0.4cm; font-weight:bold;'>");
            sb.Append("<div>48, Bode Thomas Street,<br /> Surulere, Lagos State.</div>");
            sb.Append("<div>Nigeria.</div>");
            sb.Append("<div>(+234) 8030603005</div>");
            sb.Append("<div>billing@gloriousevidence.com</div>");
            sb.Append("</div>");           
            sb.Append("<br/>");
            sb.Append("<h4><b>ORDER INVOICE</b></h4>");
            sb.Append("=============================================================================");
            sb.Append("=======================================================================================");
            sb.Append("<br/>");
            sb.Append("<div style = 'font-size:small; font-family:Calibri;text-align:center;'>");
            sb.Append("<div><span>Transaction with ID: #</span>" + getItems.OrderTransactionNumber.ToString().ToUpper() + " was placed on the " + getItems.DateCreated.Value.ToShortDateString() + " and it's being prepared. Note: All prices are in naira currency.</div>");
            sb.Append("<br/>");
            sb.Append("<div><span>Transaction Status:</span> " + getItems.OrderTransactionStatus + "</div>");
            sb.Append("<div><span>Delivery Option:</span> " + getItems.OrderDeliveryOption + "</div>");
            sb.Append("<div><span>Payment Method:</span> " + getItems.OrderPaymentOption + "</div>");
            sb.Append("<div><span>Customer Name:</span> " + getItems.UserFirstName + " " + getItems.UserLastName + "</div>");            
            sb.Append("</div>");
            sb.Append("<br/>");
            sb.Append("</header>");
            sb.Append("<main>");
            sb.Append("<table border = '1'>");
            sb.Append("<thead>");
            sb.Append("<div style = 'font-size:medium; font-family:Calibri; font-weight:bold;'>");
            sb.Append("<tr style='text-align:center; background-color:#e3eee4;'>");
            sb.Append("<th class='service'>Product</th>");
            sb.Append("<th class='desc'>Quantity</th>");
            sb.Append("<th>Unit Price</th>");
            sb.Append("<th>Total</th>");
            sb.Append("</tr>");
            sb.Append("</div>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            sb.Append("<div style = 'font-size:small; font-family:Calibri;'>");
            sb.Append("<tr style='text-align:center; background-color:#f1f1f1; 'font-size:small; font-family:Calibri;'>");
            foreach (var rec in gInvoice)
            {
                sb.Append("<td>" + rec.ProductName + "-" + rec.ProductSku + "</td>");
                sb.Append("<td>" + rec.ProductQuantity + "</td>");
                sb.Append("<td>" + rec.ProductPrice.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("ig-NG")) + "</td>");
                sb.Append("<td>" + rec.ProductTotalPrice.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("ig-NG")) + "</td>");                
            }
            sb.Append("</tr>");
            sb.Append("</div>");
            sb.Append("</tbody>");          
            sb.Append("</table>");
            sb.Append("<br/>");
            sb.Append("<div style = 'text-align:right;font-size:small; font-family:Calibri; line-height:0.4cm; font-weight:bold;'>");
            sb.Append("<div><h4><b>Order Summary</b></h4></div>");
            sb.Append("<div>   SubTotal: " + subTotal.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("ig-NG")) + " <br/></div>");
            sb.Append("<div>   TotalTax: " + tax.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("ig-NG")) + " <br/></div>");
            sb.Append("<div>   Shipping: " + shipping.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("ig-NG")) + " <br/></div>");
            sb.Append("<div> GrandTotal: " + grandTotal.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("ig-NG")) + " <br/></div>");
            sb.Append("</div>");
            sb.Append("<br/>");
            sb.Append("<div style = 'text-align:left; display:inline-block; font-size:small; line-height:0.4cm; font-family:Calibri;'>");           
            sb.Append("<div><h4><b>Shipping Details:</b></h4></div>");
            sb.Append("<div>" + getItems.UserBillingFirstName + " " + getItems.UserBillingLastName + " <br/></div>");
            sb.Append("<div>" + getItems.UserBillingAddress + " <br/></div>");
            sb.Append("<div>" + getItems.LgaName + " <br/></div>");
            sb.Append("<div>" + getItems.UserBillingState + " <br/></div>");
            sb.Append("<div>" + getItems.CountryName + " <br/></div>");
            sb.Append("<div>" + getItems.UserBillingPhone + " <br/></div>");
            sb.Append("</div>");            
            sb.Append("</main>");
            sb.Append("<br/>");
            sb.Append("<div style = 'text-align:center;font-size:x-small; font-family:Calibri;'>");
            sb.Append("<div>You may review your order history at any time by logging in to your account.</div>");
            sb.Append("</div>");


            StringReader sr = new StringReader(sb.ToString());
            string ServerName = WebConfigurationManager.AppSettings["SMTPClient"];
            string smtpName = WebConfigurationManager.AppSettings["SMTPUserName"];
            string smtpPassword = WebConfigurationManager.AppSettings["SMTPPassword"];
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfDoc.Open();

                    htmlparser.Parse(sr);
                    pdfDoc.Close();

                    byte[] bytes = memoryStream.ToArray();
                    memoryStream.Close();

                    //string emailTo = "dorissweet4love@yahoo.com,mfon2k2@gmail.com";
                    //string[] multi = emailTo.Split(',');
                    string fromEmail = "billing@gloriousevidence.com";
                    MailMessage mm = new MailMessage();
                    mm.To.Add(new MailAddress(getItems.UserEmail, getItems.UserFirstName + " " + getItems.UserLastName));
                    //mm.To.Add(new MailAddress("gloriousevidencenig@gmail.com", "Glorious Evidence Ltd"));
                    mm.CC.Add(new MailAddress("billing@gloriousevidence.com", "Glorious Evidence Ltd"));
                    mm.Bcc.Add(new MailAddress("mfon2k2@gmail.com", "Glorious Evidence Ltd"));
                    //foreach(var multiEmailId in multi)
                    //{
                    //    mm.CC.Add(new MailAddress(multiEmailId));
                    //}
                    
                    mm.From = new MailAddress(fromEmail, "Glorious Online Store");
                    mm.Subject = "New Order: Transaction#" + getItems.OrderTransactionNumber.ToString().ToUpper();
                    mm.Body = "Thanks for your patronage, Please find attached invoice";
                    mm.Attachments.Add(new Attachment(new MemoryStream(bytes), getItems.OrderTransactionNumber.ToString() + "-Order-Invoice.pdf"));
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ServerName;
                    smtp.EnableSsl = false;
                    NetworkCredential NetworkCred = new NetworkCredential();
                    NetworkCred.UserName = smtpName;
                    NetworkCred.Password = smtpPassword;
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 25;
                    smtp.Timeout = 500000;
                    smtp.Send(mm);
                }
                catch(Exception)
                {

                }
               
            }
            return sb.ToString();
        }

        public static decimal GetTotalPriceForProductDeliveryPrice(string username)
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = 0.00m;
            try
            {
                var getCartSessionId = (from c in db.ShoppingCarts.Where(x => x.UserName.Equals(username))
                                        select new
                                        {
                                            c.CartSessionID
                                        }).FirstOrDefault();
                var anonymousID = getCartSessionId.CartSessionID;
                if (anonymousID != null)
                {
                    total = (from prod in db.Products
                             join s in db.ShoppingCarts on prod.ProductID equals s.ProductID
                             where s.CartSessionID == anonymousID.ToString()
                             select (int?)s.ProductQuantity * prod.ShippingFee).Sum();
                }
            }
            catch (Exception ex)
            {
                //ViewBag.DisplayMessage = "Info";
                //ModelState.AddModelError("", ex.Message);
            }
            return total ?? decimal.Zero;
        }

        public static int GetTotalQuantity(string username)
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            int? total = 0;
            try
            {
                var getCartSessionId = (from c in db.ShoppingCarts.Where(x => x.UserName.Equals(username))
                                        select new
                                            {
                                                c.CartSessionID
                                            }).FirstOrDefault();
                var anonymousID = getCartSessionId.CartSessionID;
                if (anonymousID != null)
                {
                    total = (from cartItems in db.ShoppingCarts
                             where cartItems.CartSessionID == anonymousID.ToString()
                             select (int?)cartItems.ProductQuantity).Sum();
                }
            }
            catch (Exception ex)
            {
                //ViewBag.DisplayMessage = "Info";
                //ModelState.AddModelError("", ex.Message);
            }

            return total ?? 0;
        }

        public static decimal GetTotalPriceForOrderSummary(string username)
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = 0.00m;
            try
            {               
                    total = (from orderItems in db.ShoppingCarts
                             where orderItems.UserName == username
                             select (int?)orderItems.ProductQuantity * orderItems.ProductPrice).Sum();               
            }
            catch (Exception)
            {
                
            }
            return total ?? decimal.Zero;
        }
    }
}