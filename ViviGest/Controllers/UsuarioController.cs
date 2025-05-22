using ViviGest.Dtos;
using ViviGest.Services;
using System;
using System.Web.Mvc;

namespace ViviGest.Controllers
{
    public class UsuarioController : Controller
    {
    
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl; 
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(usuariosDto loginUser, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            // Opción #2: Usuario admin hardcodeado para pruebas
            if (loginUser.numero_documento == "admin" && loginUser.contrasena == "admin123")
            {
                Session["UserID"] = 16;
                Session["UserName"] = "Administrador";
                Session["UserRole"] = 1; // Rol administrador
                Session["UserDocumento"] = "00000000";
                Session["UserTelefono"] = "0000000000";
                Session["UserCorreo"] = "admin@mock.com";

                return RedirectToAction("MisPagos", "Residente");
            }
            if (loginUser.numero_documento == "12345" && loginUser.contrasena == "123")
            {
                Session["UserID"] = 8;
                Session["UserName"] = "Carlos";
                Session["UserRole"] = 3; // Rol administrador
                Session["UserDocumento"] = "00000";
                Session["UserTelefono"] = "0000000000";
                Session["UserCorreo"] = "admin@mock.com";

                return RedirectToAction("Index", "Administrador");
            }

            // 1. Validamos los atributos [Required] de usuariosDto
            if (!ModelState.IsValid)
                return View(loginUser);

            try
            {
                var userService = new UsuarioService();
                var userResponse = userService.LoginUser(loginUser);

                if (userResponse.Response == 1)
                {
                    Session["UserID"] = userResponse.id_usuario;
                    Session["UserName"] = userResponse.nombres;
                    Session["UserRole"] = userResponse.id_rol;
                    Session["UserDocumento"] = userResponse.numero_documento;
                    Session["UserTelefono"] = userResponse.telefono;
                    Session["UserCorreo"] = userResponse.correo_electronico;

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    switch (userResponse.id_rol)
                    {
                        case 1: return RedirectToAction("Index", "Administrador");
                        case 2: return RedirectToAction("Index", "Asistente");
                        case 3: return RedirectToAction("Index", "Cliente");
                        default:
                            ModelState.AddModelError("", "Rol de usuario no válido.");
                            return View(loginUser);
                    }
                }

                ModelState.AddModelError("", userResponse.Message ?? "Usuario o contraseña incorrectos.");
                return View(loginUser);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error interno: " + ex.Message);
                return View(loginUser);
            }
        }
    }
}

