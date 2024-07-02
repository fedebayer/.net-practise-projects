using MVCIntegrador1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace MVCIntegrador1.Controllers
{
    public class PersonaController : Controller
    {
        // GET: Persona
        public ActionResult Index()
        {
            IList<PersonaModel> personas = getPersonas();

            return View(personas);
        }


        public ActionResult Edit(int idPersona)
        {
            IList<PersonaModel> personas = getPersonaById(idPersona);
            if (personas[0] == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoDocumentos = ObtenerTiposDocumentos();

            return View("Edit", personas[0]);
        }

        [HttpPost]
        public ActionResult EditPersona(PersonaModel persona)
        {
            if (ModelState.IsValid)
            {
                ActualizarPersona(persona);

                return RedirectToAction("Index");
            }

            return Edit(persona.Id);
        }

        public ActionResult Create()
        {
            ViewBag.TipoDocumentos = ObtenerTiposDocumentos();
            return View();
        }

        [HttpPost]
        public ActionResult Create(PersonaModel persona)
        {
            if (ModelState.IsValid)
            {
                InsertarPersona(persona);

                return RedirectToAction("Index");
            }

            return Create();
        }

        private List<SelectListItem> ObtenerTiposDocumentos()
        {
            List<TipoDocumentoModel> tiposDocumento = ObtenerTiposDocumentoDesdeBaseDeDatos();

            List<SelectListItem> selectListItems = tiposDocumento.Select(tipoDoc => new SelectListItem
            {
                Value = tipoDoc.Id.ToString(),
                Text = tipoDoc.TipoDoc
            }).ToList();

            return selectListItems;
        }

        private List<TipoDocumentoModel> ObtenerTiposDocumentoDesdeBaseDeDatos()
        {
            List<TipoDocumentoModel> tipoDocumentos = new List<TipoDocumentoModel>();
            string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM TipoDocumento";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tipoDocumentos.Add(new TipoDocumentoModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                TipoDoc = reader["TipoDoc"].ToString(),
                            });
                        }
                    }
                }
            }
            return tipoDocumentos;
        }

        private void ActualizarPersona(PersonaModel persona)
        {
            string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Persona " +
                       "SET Nombre = @Nombre, Apellido = @Apellido, Edad = @Edad, Id_tipo_doc = @Id_tipo_doc, Nro_doc = @Nro_doc, Email = @Email " +
                       "WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", persona.Nombre);
                    command.Parameters.AddWithValue("@Apellido", persona.Apellido);
                    command.Parameters.AddWithValue("@Edad", persona.Edad);
                    command.Parameters.AddWithValue("@Id_tipo_doc", persona.Id_tipo_doc);
                    command.Parameters.AddWithValue("@Nro_doc", persona.Nro_doc);
                    command.Parameters.AddWithValue("@Email", persona.Email); ;
                    command.Parameters.AddWithValue("@Id", persona.Id);

                    command.ExecuteNonQuery();
                }
            }
        }
        private void InsertarPersona(PersonaModel persona)
        {
            string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Persona (Nombre, Apellido, Edad, Id_tipo_doc, Nro_doc, Email) " +
                               "VALUES (@Nombre, @Apellido, @Edad, @Id_tipo_doc, @Nro_doc, @Email)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", persona.Nombre);
                    command.Parameters.AddWithValue("@Apellido", persona.Apellido);
                    command.Parameters.AddWithValue("@Edad", persona.Edad);
                    command.Parameters.AddWithValue("@Id_tipo_doc", persona.Id_tipo_doc);
                    command.Parameters.AddWithValue("@Nro_doc", persona.Nro_doc);
                    command.Parameters.AddWithValue("@Email", persona.Email); ;

                    command.ExecuteNonQuery();
                }
            }
        }

        private IList<PersonaModel> getPersonas()
        {
            List<PersonaModel> personas = new List<PersonaModel>();
            string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT p.*, t.TipoDoc FROM Persona p " +
                       "INNER JOIN TipoDocumento t ON p.Id_tipo_doc = t.Id"; ;
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            personas.Add(new PersonaModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Edad = Convert.ToInt32(reader["Edad"]),
                                Id_tipo_doc = Convert.ToInt32(reader["Id_tipo_doc"]),
                                Nro_doc = reader["Nro_doc"].ToString(),
                                Email = reader["email"].ToString(),
                                TipoDocumento = new TipoDocumentoModel
                                {
                                    TipoDoc = reader["TipoDoc"].ToString(),
                                }
                            });
                        }
                    }
                }
            }

            return personas;
        }

        private List<PersonaModel> getPersonaById(int personaId)
        {
            List<PersonaModel> personas = new List<PersonaModel>();
            string connectionString = @"Server=SVR-SQLCAP01\SQLEXPRESS;Database=FB_Practica_1;User Id=sa;Password=codes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Persona WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", personaId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            personas.Add(new PersonaModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Edad = Convert.ToInt32(reader["Edad"]),
                                Id_tipo_doc = Convert.ToInt32(reader["Id_tipo_doc"]),
                                Nro_doc = reader["Nro_doc"].ToString(),
                                Email = reader["email"].ToString()
                            });
                        }
                    }
                }
            }

            return personas;
        }
    }
}