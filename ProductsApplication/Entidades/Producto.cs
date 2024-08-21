/*FB: Se soluciona error de que todos los precios sean 0 cuando no es asi, habia un error de escritura en la linea 22 decia 'PRCIO' y era 'PRECIO'*/

using System.Data;

namespace Entidades
{
    public class Producto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int Precio { get; set; }

        public int IdCategoria { get; set; }

        public string DescripcionCategoria { get; set; }

        public Producto FromDataRow(DataRow dataRow)
        {
            Producto entidad = new Producto();

            if (dataRow.Table.Columns.Contains("ID_PRODUCTO"))
                entidad.Id = (int)dataRow["ID_PRODUCTO"];

            if (dataRow.Table.Columns.Contains("DESCRIPCION"))
                entidad.Descripcion = dataRow["DESCRIPCION"].ToString();

            if (dataRow.Table.Columns.Contains("PRECIO"))
                entidad.Precio = (int)dataRow["PRECIO"];

            if (dataRow.Table.Columns.Contains("ID_CATEGORIA")) { 
                entidad.IdCategoria = (int)dataRow["ID_CATEGORIA"];

            }
            if (dataRow.Table.Columns.Contains("DESCRIPCION_CATEGORIA")) { 
                entidad.DescripcionCategoria = dataRow["DESCRIPCION_CATEGORIA"].ToString();
            }

            return entidad;
        }
    }
}