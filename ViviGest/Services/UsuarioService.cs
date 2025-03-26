using GastroByte.Dtos;
using GastroByte.Repositories;
using GastroByte.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GastroByte.Services
{
    public class UsuarioService
    {


        public UsuarioDto CreateUser(UsuarioDto userModel)
        {
            UsuarioDto responseUserDto = new UsuarioDto();
            UsuarioReposiyoty userReposiyoty = new UsuarioReposiyoty();
            try
            {
                // Primero, verifica si la contraseña es válida (no vacía)
                if (string.IsNullOrEmpty(userModel.contrasena))
                {
                    responseUserDto.Response = 0;

                    return responseUserDto;
                }

                userModel.contrasena = EncryptUtility.HashPassword(userModel.contrasena);

                if (userReposiyoty.BuscarUsuario(userModel.nombre))
                {
                    responseUserDto.Response = 0;
                    responseUserDto.Message = "Usuario ya existe";
                }
                else
                {


                    if (userReposiyoty.CreateUser(userModel) != 0)
                    {
                        responseUserDto.Response = 1;
                        responseUserDto.Message = "Creacion exitosa";
                    }
                    else
                    {
                        responseUserDto.Response = 0;
                        responseUserDto.Message = "Algo paso";
                    }
                }

                return responseUserDto;
            }
            catch (Exception e)
            {
                responseUserDto.Response = 0;
                responseUserDto.Message = e.InnerException.ToString();
                return responseUserDto;
            }
        }

        public IEnumerable<UsuarioDto> GetAllUsuario()
        {
            UsuarioReposiyoty userReposiyoty = new UsuarioReposiyoty();
            return userReposiyoty.GetAllUsuarios(); // Esto ahora debería funcionar correctamente
        }
        
        public UsuarioDto LoginUser(UsuarioDto loginUser)
        {
            
                UsuarioDto responseUserDto = new UsuarioDto();
                UsuarioReposiyoty userReposiyoty = new UsuarioReposiyoty();

                try
                {
                    var storedUser = userReposiyoty.BuscarUsuarioPorNumeroDocumento(loginUser.numero_documento);

                    if (storedUser == null)
                    {
                        responseUserDto.Response = 0;
                        responseUserDto.Message = "Usuario no encontrado.";
                        return responseUserDto;
                    }

                    // Verifica la contraseña con bcrypt
                    if (EncryptUtility.VerifyPassword(loginUser.contrasena, storedUser.contrasena))
                    {
                        // Asigna los valores necesarios al DTO de respuesta
                        responseUserDto.Response = 1; // Login exitoso
                        responseUserDto.id_usuario = storedUser.id_usuario; // Asigna el ID
                        responseUserDto.nombre = storedUser.nombre; // Asigna el nombre
                        responseUserDto.id_rol = storedUser.id_rol; // Asigna el rol
                        responseUserDto.numero_documento = storedUser.numero_documento; 
                        responseUserDto.telefono = storedUser.telefono; 
                        responseUserDto.correo_electronico = storedUser.correo_electronico; 
                    }
                    else
                    {
                        responseUserDto.Response = 0;
                        responseUserDto.Message = "Contraseña incorrecta.";
                    }

                    return responseUserDto;
                }
                catch (Exception e)
                {
                    responseUserDto.Response = 0;
                    responseUserDto.Message = e.InnerException?.ToString() ?? e.Message;
                    return responseUserDto;
                }
            }




     
        }


    }

