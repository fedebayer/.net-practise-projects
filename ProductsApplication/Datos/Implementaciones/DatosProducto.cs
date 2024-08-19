using Entidades;
using Entidades.Filtros;
using Framework.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Implementaciones
{
    public class DatosProducto
    {
        SqlConnection connection;
        string connstr;

        public DatosProducto()
        {
            connstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            connection = new SqlConnection(connstr);
        }

        public void Insertar(Producto p)
        {
            using (SqlCommand command = new SqlCommand(Constantes.SP_PRODUCTO_INSERT, connection))
            {
                connection.Open();
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("descripcion", p.Descripcion);

                command.Parameters.AddWithValue("precio", p.Precio);

                command.Parameters.AddWithValue("id_categoria", p.IdCategoria);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Actualizar(Producto p)
        {
            using (SqlCommand command = new SqlCommand(Constantes.SP_PRODUCTO_ACTUALIZAR, connection))
            {
                connection.Open();
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("id", p.Id);

                command.Parameters.AddWithValue("descripcion", p.Descripcion);

                command.Parameters.AddWithValue("precio", p.Precio);

                command.Parameters.AddWithValue("id_categoria", p.IdCategoria);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Borrar(int id)
        {
            using (SqlCommand command = new SqlCommand(Constantes.SP_PRODUCTO_DELETE, connection))
            {
                connection.Open();
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("id", id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public List<Producto> ObtenerPorFiltro(ProductoFiltro pFiltro)
        {
            List<Producto> registrosEncontrados = new List<Producto>();

            using (SqlCommand command = new SqlCommand(Constantes.SP_PRODUCTO_OBTENER_POR_FILTRO, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;

                if (pFiltro.Id.HasValue)
                    command.Parameters.AddWithValue("id", pFiltro.Id.Value);

                if (!string.IsNullOrEmpty(pFiltro.Descripcion))
                    command.Parameters.AddWithValue("descripcion", pFiltro.Descripcion);

                if (!string.IsNullOrEmpty(pFiltro.Categoria))
                    command.Parameters.AddWithValue("categoria", pFiltro.Categoria);

                using (SqlDataAdapter da = new SqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt != null)
                    {
                        Producto producto = null;

                        foreach (DataRow dataRow in dt.Rows)
                        {
                            producto = new Producto();
                            registrosEncontrados.Add(producto.FromDataRow(dataRow));
                        }
                    }
                }
            }

            return registrosEncontrados;
        }

        public List<Producto> ObtenerPaginado(string descripcion, string categoria, string sidx, string sord, int page, int rows, out int total)
        {
            List<Producto> registrosEncontrados = new List<Producto>();

            using (SqlCommand command = new SqlCommand(Constantes.SP_PRODUCTO_OBTENER_PAGINADO, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("intPagNumero", page);
                command.Parameters.AddWithValue("intPagTamano", rows);

                if (!string.IsNullOrEmpty(descripcion))
                    command.Parameters.AddWithValue("descripcion", descripcion);

                if (!string.IsNullOrEmpty(categoria))
                    command.Parameters.AddWithValue("categoria", categoria);

                using (SqlDataAdapter da = new SqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt != null)
                    {
                        Producto producto = null;

                        foreach (DataRow dataRow in dt.Rows)
                        {
                            producto = new Producto();
                            registrosEncontrados.Add(producto.FromDataRow(dataRow));
                        }
                    }
                }

                total = registrosEncontrados.Count;
            }

            return registrosEncontrados;
        }


        public List<Categoria> ObtenerCategorias()
        {
            List<Categoria> registrosEncontrados = new List<Categoria>();

            using (SqlCommand command = new SqlCommand(Constantes.SP_PRODUCTO_OBTENER_CATEGORIAS, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (SqlDataAdapter da = new SqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt != null)
                    {
                        Categoria categoria = null;

                        foreach (DataRow dataRow in dt.Rows)
                        {
                            categoria = new Categoria();
                            registrosEncontrados.Add(categoria.FromDataRow(dataRow));
                        }
                    }
                }
            }

            return registrosEncontrados;
        }
    }
}