using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GastroByte.Utilities
{
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private readonly int[] allowedRoles;

        public AuthorizeRoleAttribute(params int[] roles)
        {
            this.allowedRoles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userRole = (int?)filterContext.HttpContext.Session["UserRole"];

            // Verificar si el usuario no ha iniciado sesión
            if (userRole == null)
            {
                filterContext.Result = new RedirectResult("~/Usuario/Login");
                return;
            }

            // Si el usuario está autenticado pero no tiene el rol adecuado
            if (!allowedRoles.Contains(userRole.Value))
            {
                filterContext.Result = new RedirectResult("~/Home/AccessDenied");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
