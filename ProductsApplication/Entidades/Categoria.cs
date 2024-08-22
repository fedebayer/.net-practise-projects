using System.Data;

namespace Entidades
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public Categoria FromDataRow(DataRow dataRow)
        {
            Categoria entidad = new Categoria();

            if (dataRow.Table.Columns.Contains("ID_CATEGORIA"))
                entidad.Id = (int)dataRow["ID_CATEGORIA"];

            if (dataRow.Table.Columns.Contains("DESCRIPCION"))
                entidad.Descripcion = dataRow["DESCRIPCION"].ToString();

            return entidad;
        }
    }
}
