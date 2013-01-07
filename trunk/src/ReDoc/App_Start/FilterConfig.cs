using System.Web;
using System.Web.Mvc;
using ReDoc.Controllers;
using Simple;

namespace ReDoc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogErrorAttribute());
        }
    }

    public class LogErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            SystemMonitor.Error(filterContext.Exception, "Error");
            base.OnException(filterContext);
        }

     
    }
}