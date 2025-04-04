using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;

namespace ViviGest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // Inicializar el logger de log4net
        private static readonly ILog log = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            // Configurar log4net
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Log de inicio
            log.Info("Aplicación iniciada.");
        }
    }
}
