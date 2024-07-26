using MVCIntegrador2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIntegrador2.Controllers
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor
        public ActionResult Index()
        {
            /*Pasar entre es-AR/en-US para ver cambios de cultura*/
            //System.Globalization.CultureInfo cultura = new System.Globalization.CultureInfo("en-US");
            System.Globalization.CultureInfo cultura = new System.Globalization.CultureInfo("es-AR");
            System.Threading.Thread.CurrentThread.CurrentCulture = cultura;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultura;

            IList<ProveedorModel> proveedores;
            if (TempData.ContainsKey("proveedoresFiltrados"))
            {
                proveedores = (IList<ProveedorModel>)TempData["proveedoresFiltrados"];
            }
            else
            {
                proveedores = getProveedores();
            }
            return View(proveedores);
        }

        public ActionResult Edit(int idProveedor)
        {
            /*Pasar entre es-AR/en-US para ver cambios de cultura*/
            //System.Globalization.CultureInfo cultura = new System.Globalization.CultureInfo("en-US");
            System.Globalization.CultureInfo cultura = new System.Globalization.CultureInfo("es-AR");
            System.Threading.Thread.CurrentThread.CurrentCulture = cultura;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultura;
            if (idProveedor == 0)
            {
                ViewBag.Provincias = ObtenerListaProvincias();
                ViewBag.Localidades = ObtenerListaLocalidades();
                return View("Edit", new ProveedorModel());
            }
            else
            {
                IList<ProveedorModel> proveedores = getProveedorById(idProveedor);
                if (proveedores == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Provincias = ObtenerListaProvincias();
                ViewBag.Localidades = ObtenerListaLocalidades();
                return View("Edit", proveedores[0]);
            }
        }

        public ActionResult Delete(int idProveedor)
        {
           bool borrado = BorrarProveedorById(idProveedor);

            if (borrado) return RedirectToAction("Index");
            else return HttpNotFound();
        }

        public ActionResult Provincias()
        {

            var provinciasSelectList = ObtenerProvincias();

            return Json(provinciasSelectList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Localidades(int idProvincia)
        {
           
            var localidadesSelectList = ObtenerLocalidadesByIdProvincia(idProvincia);

            return Json(localidadesSelectList, JsonRequestBehavior.AllowGet);
        }


        private bool BorrarProveedorById(int idProveedor)
        {
            try
            {
                string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("DELETE FROM Proveedor WHERE Id = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Id", idProveedor);
                        command.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ActionResult FilterProveedores(string nombre, string provincia, string localidad)
        {
            IList<ProveedorModel> proveedores = getProveedoresFiltrados(nombre, provincia, localidad);
            TempData["proveedoresFiltrados"] = proveedores;
            return RedirectToAction("Index");
        }

        

        [HttpPost]
        public ActionResult CreateProveedor(ProveedorModel proveedor)
        {
            if (ModelState.IsValid)
            {
                InsertarProveedor(proveedor);

                return RedirectToAction("Index");
            }
            ViewBag.Provincias = ObtenerListaProvincias();
            ViewBag.Localidades = ObtenerListaLocalidades();
            return View("Edit", proveedor);
        }

        [HttpPost]
        public ActionResult EditProveedor(ProveedorModel proveedor)
        {
            if (ModelState.IsValid)
            {
                ActualizarProveedor(proveedor);

                return RedirectToAction("Index");
            }

            return Edit(proveedor.Id);
        }
        private List<SelectListItem> ObtenerListaProvincias()
        {
            List<ProvinciaModel> provincias =  ObtenerProvincias(); ;

            List<SelectListItem> selectListItems = provincias.Select(prov => new SelectListItem
            {
                Value = prov.Id.ToString(),
                Text = prov.Provincia
            }).ToList();

            return selectListItems;
        }

        private List<SelectListItem> ObtenerListaLocalidades()
        {
            List<LocalidadModel> localidades = ObtenerLocalidades(); ;

            List<SelectListItem> selectListItems = localidades.Select(loc => new SelectListItem
            {
                Value = loc.Id.ToString(),
                Text = loc.Localidad
            }).ToList();

            return selectListItems;
        }

        private List<SelectListItem> ObtenerListaLocalidadesByIdProvincia(int idProvincia)
        {
            List<LocalidadModel> localidades = ObtenerLocalidadesByIdProvincia(idProvincia); ;

            List<SelectListItem> selectListItems = localidades.Select(loc => new SelectListItem
            {
                Value = loc.Id.ToString(),
                Text = loc.Localidad
            }).ToList();

            return selectListItems;
        }


        private void ActualizarProveedor(ProveedorModel proveedor)
        {
            string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Proveedor " +
                       "SET Nombre = @Nombre, Domicilio = @Domicilio, IdLocalidad = @IdLocalidad, IdProvincia = @IdProvincia " +
                       "WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                    command.Parameters.AddWithValue("@Domicilio", proveedor.Domicilio);
                    command.Parameters.AddWithValue("@IdLocalidad", proveedor.IdLocalidad);
                    command.Parameters.AddWithValue("@IdProvincia", proveedor.IdProvincia);
                    command.Parameters.AddWithValue("@Id", proveedor.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void InsertarProveedor(ProveedorModel proveedor)
        {
            string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Proveedor (Nombre, Domicilio, IdLocalidad, IdProvincia) " +
                               "VALUES (@Nombre, @Domicilio, @IdLocalidad, @IdProvincia)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                    command.Parameters.AddWithValue("@Domicilio", proveedor.Domicilio);
                    command.Parameters.AddWithValue("@IdLocalidad", proveedor.IdLocalidad);
                    command.Parameters.AddWithValue("@IdProvincia", proveedor.IdProvincia);

                    command.ExecuteNonQuery();
                }
            }
        }

        private dynamic ObtenerLocalidades()
        {
            List<LocalidadModel> localidades = new List<LocalidadModel>();
            string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Localidad";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            localidades.Add(new LocalidadModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Localidad = reader["Localidad"].ToString(),
                            });
                        }
                    }
                }
            }
            return localidades;
        }

        private dynamic ObtenerLocalidadesByIdProvincia(int idProvincia)
        {
            List<LocalidadModel> localidades = new List<LocalidadModel>();
            string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Localidad WHERE IdProvincia = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", idProvincia);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            localidades.Add(new LocalidadModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Localidad = reader["Localidad"].ToString(),
                            });
                        }
                    }
                }
            }
            return localidades;
        }

        private dynamic ObtenerProvincias()
        {
            List<ProvinciaModel> provincias = new List<ProvinciaModel>();
            string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Provincia";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            provincias.Add(new ProvinciaModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Provincia = reader["Provincia"].ToString(),
                            });
                        }
                    }
                }
            }
            return provincias;
        }

        private IList<ProveedorModel> getProveedorById(int idProveedor)
        {
            List<ProveedorModel> proveedores = new List<ProveedorModel>();
            string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT p.*, provi.Provincia, l.Localidad FROM Proveedor p " +
                       "INNER JOIN Localidad l ON p.IdLocalidad = l.Id INNER JOIN Provincia provi ON p.IdProvincia = provi.Id " +
                       "WHERE p.Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", idProveedor);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            proveedores.Add(new ProveedorModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Domicilio = reader["Domicilio"].ToString(),
                                IdLocalidad = Convert.ToInt32(reader["IdLocalidad"]),                               
                                Localidad = new LocalidadModel
                                {
                                    Localidad = reader["Localidad"].ToString(),
                                },
                                Provincia = new ProvinciaModel
                                {
                                    Provincia = reader["Provincia"].ToString(),
                                }
                            });
                        }
                    }
                }
            }
            return proveedores;
        }

        


        private IList<ProveedorModel> getProveedoresFiltrados(string nombre, string provincia, string localidad)
        {
            List<ProveedorModel> proveedores = new List<ProveedorModel>();
            string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT p.*, provi.Provincia, l.Localidad FROM Proveedor p " +
                       "INNER JOIN Localidad l ON p.IdLocalidad = l.Id INNER JOIN Provincia provi ON l.IdProvincia = provi.Id " +
                       "WHERE p.Nombre LIKE @Nombre AND provi.Provincia LIKE @Provincia AND l.Localidad LIKE @Localidad";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (nombre != "")
                    {
                        command.Parameters.AddWithValue("@Nombre", "%" + nombre + "%");
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Nombre", "%%");
                    }
                    if (provincia != "")
                    {
                        command.Parameters.AddWithValue("@Provincia", "%" + provincia + "%");
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Provincia", "%%");
                    }
                    if (localidad != "")
                    {
                        command.Parameters.AddWithValue("@Localidad", "%" + localidad + "%");
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Localidad", "%%");
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            proveedores.Add(new ProveedorModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Domicilio = reader["Domicilio"].ToString(),
                                IdLocalidad = Convert.ToInt32(reader["IdLocalidad"]),
                                IdProvincia = Convert.ToInt32(reader["IdProvincia"]),
                                Localidad = new LocalidadModel
                                {
                                    Localidad = reader["Localidad"].ToString(),
                                },
                                Provincia = new ProvinciaModel
                                {
                                    Provincia = reader["Provincia"].ToString(),
                                }
                            });
                        }
                    }
                }
            }

            return proveedores;
        }
        private IList<ProveedorModel> getProveedores()
        {
            List<ProveedorModel> proveedores = new List<ProveedorModel>();
            string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT p.*, provi.Provincia, l.Localidad FROM Proveedor p " +
                       "INNER JOIN Localidad l ON p.IdLocalidad = l.Id INNER JOIN Provincia provi ON p.IdProvincia = provi.Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            proveedores.Add(new ProveedorModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Domicilio = reader["Domicilio"].ToString(),
                                IdLocalidad = Convert.ToInt32(reader["IdLocalidad"]),
                                IdProvincia = Convert.ToInt32(reader["IdProvincia"]),
                                Localidad = new LocalidadModel
                                {
                                    Localidad = reader["Localidad"].ToString(),
                                },
                                 Provincia = new ProvinciaModel
                                 {
                                     Provincia = reader["Provincia"].ToString(),
                                 }
                            });
                        }
                    }
                }
            }

            return proveedores;
        }
    }
}