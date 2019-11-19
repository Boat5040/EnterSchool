using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace EnterSchoolApp
{
    public static class HMTLHelperExtensions
    {
        public static MvcHtmlString IsSelected(this HtmlHelper helper, ViewContext context, string controller, string action = null)
        {
            string incomingController = context.RouteData.Values["controller"].ToString();
            string incomingAction = context.RouteData.Values["action"].ToString();

            if (!string.IsNullOrWhiteSpace(controller) && !string.IsNullOrWhiteSpace(action))
            {
                if (controller.Equals(incomingController) && action.Equals(incomingAction))
                    return new MvcHtmlString("active");
            }
            else if (!string.IsNullOrWhiteSpace(controller))
            {
                if (controller.Equals(incomingController))
                    return new MvcHtmlString("active");
            }

            return new MvcHtmlString("");
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

    }
}
