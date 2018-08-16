using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Eblog.Admin.CustomFilter
{
    public class LoginEditor : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) // method çalıştırıldıktan sonra devreye giriyor
        {

            HttpContextWrapper wrapper = new HttpContextWrapper(HttpContext.Current);
            var SessionControl = context.HttpContext.Session["Rol"];

            if (Convert.ToInt32(SessionControl) != 2 && Convert.ToInt32(SessionControl) != 1)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Home" }, { "action", "Login" } });
            }
        }

        public void OnActionExecuting(ActionExecutingContext context) // method tetiklendiği anda devreye giriyor
        {

        }
    }
}