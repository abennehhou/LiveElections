using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LiveElections
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ILog _logger = LogManager.GetLogger(typeof(MvcApplication));
        public static ElectionsContext ElectionsContext;

        protected void Application_Start()
        {
            _logger.Info("Initializing MVC Application");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ElectionsContext = new ElectionsContext();
        }
    }
}
