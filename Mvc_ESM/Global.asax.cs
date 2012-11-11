using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc_ESM
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        //protected void Application_Error()
        //{
        //    var exception = Server.GetLastError();
        //    var httpException = exception as HttpException;
        //    Response.Clear();
        //    Server.ClearError();
        //    var routeData = new RouteData();
        //    routeData.Values["controller"] = "Errors";
        //    routeData.Values["action"] = "General";
        //    routeData.Values["exception"] = exception;
        //    Response.StatusCode = 500;
        //    if (httpException != null)
        //    {
        //        Response.StatusCode = httpException.GetHttpCode();
        //        switch (Response.StatusCode)
        //        {
        //            case 403:
        //                routeData.Values["action"] = "Http403";
        //                break;
        //            case 404:
        //                routeData.Values["action"] = "Http404";
        //                break;
        //        }
        //    }

        //    IController errorsController = new Mvc_ESM.Controllers.ErrorsController();
        //    var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
        //    errorsController.Execute(rc);
        //}
    }
}