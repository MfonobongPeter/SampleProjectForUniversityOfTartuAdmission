using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           
            routes.MapRoute(
                name: "ProductDetailsDisplay",
                url: "details/{id}/{name}",    /* "soal" is only decor */
                defaults: new
                {
                    controller = "Home",
                    action = "ProductDetails",
                    name = UrlParameter.Optional
                }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

                );

            routes.MapRoute(
                name: "Home",
                url: "subcat/{id}/{name}",    /* "soal" is only decor */
                defaults: new
                {
                    controller = "Home",
                    action = "SubCategory",
                    name = UrlParameter.Optional
                }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

                );

            routes.MapRoute(
               name: "Category",
               url: "category/{id}/{name}",    /* "soal" is only decor */
               defaults: new
               {
                   controller = "Home",
                   action = "Category",
                   name = UrlParameter.Optional
               }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

               );

            routes.MapRoute(
              name: "Cart",
              url: "shopping-cart/",    /* "soal" is only decor */
              defaults: new
              {
                  controller = "Home",
                  action = "ViewCart",
                  name = UrlParameter.Optional
              }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

              );

            routes.MapRoute(
              name: "About",
              url: "about/",    /* "soal" is only decor */
              defaults: new
              {
                  controller = "Home",
                  action = "About",
                  name = UrlParameter.Optional
              }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

              );

            routes.MapRoute(
              name: "Contact",
              url: "contact/",    /* "soal" is only decor */
              defaults: new
              {
                  controller = "Home",
                  action = "Contact",
                  name = UrlParameter.Optional
              }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

              );

            routes.MapRoute(
              name: "Index",
              url: "index/",    /* "soal" is only decor */
              defaults: new
              {
                  controller = "Home",
                  action = "Index",
                  name = UrlParameter.Optional
              }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

              );

            routes.MapRoute(
              name: "AccountVerification",
              url: "account-verification/",    /* "soal" is only decor */
              defaults: new
              {
                  controller = "Home",
                  action = "AccountVerification",
                  name = UrlParameter.Optional
              }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

              );

            routes.MapRoute(
              name: "BillingAddress",
              url: "billing-address/",    /* "soal" is only decor */
              defaults: new
              {
                  controller = "Home",
                  action = "BillingAddress",
                  name = UrlParameter.Optional
              }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

              );

            routes.MapRoute(
              name: "ChangePassword",
              url: "change-password/",    /* "soal" is only decor */
              defaults: new
              {
                  controller = "Home",
                  action = "ChangePassword",
                  name = UrlParameter.Optional
              }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

              );

            routes.MapRoute(
             name: "CustomerLogin",
             url: "customer-login/",    /* "soal" is only decor */
             defaults: new
             {
                 controller = "Home",
                 action = "CustomerLogin",
                 name = UrlParameter.Optional
             }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

             );

            routes.MapRoute(
             name: "DashBoard",
             url: "dash-board/",    /* "soal" is only decor */
             defaults: new
             {
                 controller = "Home",
                 action = "DashBoard",
                 name = UrlParameter.Optional
             }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

             );

            routes.MapRoute(
            name: "DeliveryMethod",
            url: "delivery-method/",    /* "soal" is only decor */
            defaults: new
            {
                controller = "Home",
                action = "DeliveryMethod",
                name = UrlParameter.Optional
            }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

            );

            routes.MapRoute(
            name: "ForgotPassword",
            url: "forgot-password/",    /* "soal" is only decor */
            defaults: new
            {
                controller = "Home",
                action = "ForgotPassword",
                name = UrlParameter.Optional
            }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

            );


            routes.MapRoute(
            name: "Logout",
            url: "logout/",    /* "soal" is only decor */
            defaults: new
            {
                controller = "Home",
                action = "Logout",
                name = UrlParameter.Optional
            }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

            );

            routes.MapRoute(
            name: "OrderCompleted",
            url: "order-completed/",    /* "soal" is only decor */
            defaults: new
            {
                controller = "Home",
                action = "OrderCompleted",
                name = UrlParameter.Optional
            }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

            );

            routes.MapRoute(
            name: "OrderReview",
            url: "order-review/",    /* "soal" is only decor */
            defaults: new
            {
                controller = "Home",
                action = "OrderReview",
                name = UrlParameter.Optional
            }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

            );

            routes.MapRoute(
            name: "PaymentMethod",
            url: "payment-method/",    /* "soal" is only decor */
            defaults: new
            {
                controller = "Home",
                action = "PaymentMethod",
                name = UrlParameter.Optional
            }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

            );


            routes.MapRoute(
            name: "Products",
            url: "products/",    /* "soal" is only decor */
            defaults: new
            {
                controller = "Home",
                action = "Products",
                name = UrlParameter.Optional
            }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

            );

            routes.MapRoute(
            name: "Register",
            url: "register/",    /* "soal" is only decor */
            defaults: new
            {
                controller = "Home",
                action = "Register",
                name = UrlParameter.Optional
            }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

            );

            routes.MapRoute(
           name: "UpdateProfile",
           url: "update-profile/",    /* "soal" is only decor */
           defaults: new
           {
               controller = "Home",
               action = "UpdateProfile",
               name = UrlParameter.Optional
           }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

           );

            routes.MapRoute(
           name: "ViewOrderDetails",
           url: "view-order-details/",    /* "soal" is only decor */
           defaults: new
           {
               controller = "Home",
               action = "UpdateProfile",
               name = UrlParameter.Optional
           }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

           );

            routes.MapRoute(
          name: "ViewWishList",
          url: "view-wishlist/",    /* "soal" is only decor */
          defaults: new
          {
              controller = "Home",
              action = "ViewWishList",
              name = UrlParameter.Optional
          }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
                /* slug only explains content of the webpage*/

          );

        //    routes.MapRoute(
        // name: "FailedTransaction",
        // url: "failed-transaction/",    /* "soal" is only decor */
        // defaults: new
        // {
        //     controller = "PaymentGateway",
        //     action = "FailedTransaction",
        //     name = UrlParameter.Optional
        // }    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
        //        /* slug only explains content of the webpage*/

        // );

        //    routes.MapRoute(
        //name: "PaymentSuccessful",
        //url: "payment-successful/",    /* "soal" is only decor */
        //defaults: new
        //{
        //    controller = "PaymentGateway",
        //    action = "PaymentSuccessful",
        //    name = UrlParameter.Optional
        //}    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
        //        /* slug only explains content of the webpage*/

        //);

        //    routes.MapRoute(
        //name: "ProcessPayment",
        //url: "process-payment/",    /* "soal" is only decor */
        //defaults: new
        //{
        //    controller = "PaymentGateway",
        //    action = "ProcessPayment",
        //    name = UrlParameter.Optional
        //}    /*I made "slug" optional because I don't need this part to retrieve conternt from database */
        //        /* slug only explains content of the webpage*/

        //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{name}",
                defaults: new { controller = "Home", action = "Index", id = "", name = "" }
            );
        }
    }
}
