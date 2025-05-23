using ViviGest.Dtos;
using ViviGest.Services;
using System;
using System.Web.Mvc;

namespace ViviGest.Controllers
{
    public class UsuarioController : Controller
    {
        // Vista principal de Usuario
        // Esta acción devuelve la vista principal del usuario
        public ActionResult Index()
        {
            return View();
        }

        // Vista de Login (GET)
        // Esta acción se encarga de mostrar el formulario de inicio de sesión
        public ActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl; // Almacena la URL de retorno para redirigir después del login
            return View();
        }

        // Procesamiento del Login (POST)
        // Esta acción maneja la lógica cuando el usuario envía el formulario de login
        [HttpPost]
        public ActionResult Login(usuariosDto loginUser, string returnUrl = null)
        {
            // Verifica si el modelo de usuario está vacío, y si es así, asigna un mensaje de error
            if (loginUser == null)
            {
                loginUser = new usuariosDto { Message = "El modelo de usuario no se envió correctamente." };
                return View(loginUser); // Devuelve la vista con el mensaje de error
            }

            try
            {
                // Llama al servicio de usuario para autenticar al usuario
                var userService = new UsuarioService();
                var userResponse = userService.LoginUser(loginUser);


                // Redirige al returnUrl si está definido, o al home en caso contrario
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl); // Si hay una URL de retorno, redirige a esa
                // Si la respuesta indica que el login fue exitoso (Response == 1)
                if (userResponse.Response == 1)
                {
                    Session["UserID"] = userResponse.id_usuario;
                    Session["UserName"] = userResponse.nombres;
                    Session["UserRole"] = userResponse.id_rol;
                    Session["UserDocumento"] = userResponse.numero_documento;
                    Session["UserTelefono"] = userResponse.telefono;
                    Session["UserCorreo"] = userResponse.correo_electronico;

                    // Redirige a diferentes páginas según el rol del usuario
                    switch (userResponse.id_rol)
                    {
                        case 1:
                            return RedirectToAction("Index", "Administrador");
                        case 2:
                            return RedirectToAction("Index", "Visitante");
                        case 3:
                            return RedirectToAction("Index", "Visitante");
                        default:
                            // Si el rol no coincide, redirige a una página genérica o de error
                            return RedirectToAction("Login", "Usuario");

                    }
                }
                else
                {
                    // Si las credenciales son incorrectas, muestra un mensaje de error
                    loginUser.Message = "Credenciales incorrectas. Inténtalo nuevamente.";
                    return View(loginUser); // Devuelve la vista con el mensaje de error
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, muestra el mensaje de error correspondiente
                loginUser.Message = "Ocurrió un error inesperado: " + ex.Message;
                return View(loginUser); // Devuelve la vista con el mensaje de error
            }
        }




        // GET: Usuario/Logout
        public ActionResult Logout()
        {
            Session.Clear();  // Limpia todas las variables de sesión
            return RedirectToAction("Login", "Usuario");
        }


        public ActionResult Create()
        {
            usuariosDto user = new usuariosDto
            {
                Response = 0, // Inicializa Response en 0 o algún valor por defecto
                Message = string.Empty // Inicializa Message como una cadena vacía
            };
            return View(user);
        }

        // POST: Usuario/Create
        [HttpPost]
        public ActionResult Create(usuariosDto newUser)
        {
            if (newUser == null)
            {
                newUser = new usuariosDto();
                newUser.Message = "El modelo de usuario no se envió correctamente.";
                return View(newUser);
            }

            try
            {
                UsuarioService userService = new UsuarioService();
                usuariosDto userResponse = userService.CreateUser(newUser);

                if (userResponse.Response == 1)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    // Asegúrate de que `Message` tenga un valor
                    if (string.IsNullOrEmpty(userResponse.Message))
                    {
                        userResponse.Message = "Error al crear el usuario. Por favor, inténtalo nuevamente.";
                    }
                    return View(userResponse);
                }
            }
            catch (Exception ex)
            {
                // En caso de excepción, devuelves el modelo con un mensaje de error
                newUser.Message = "Ocurrió un error inesperado: " + ex.Message; // Muestra el mensaje de la excepción
                newUser.Response = 0;
                return View(newUser);
            }
        }
    }
}