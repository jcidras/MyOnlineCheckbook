using System;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.Web.Routing;

namespace OnlineCheckbook
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AuthConfig.RegisterAuth();            
        }        
    }
}
