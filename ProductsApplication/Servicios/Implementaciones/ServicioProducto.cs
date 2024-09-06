using Datos.Implementaciones;
using Entidades;
using Entidades.Filtros;
using Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Implementaciones
{
    public class ServicioProducto
    {
        DatosProducto _datos;
        public ServicioProducto()
        {
            _datos = new DatosProducto();
        }

        public void Insertar(Producto p)
        {
            _datos.Insertar(p);
        }

        public void Actualizar(Producto p)
        {
            _datos.Actualizar(p);
        }
        
        public void Borrar(int id)
        {
            _datos.Borrar(id);
        }

        public List<Producto> ObtenerPorFiltro(ProductoFiltro pFiltro)
        {
            return _datos.ObtenerPorFiltro(pFiltro);
        }

        public List<Producto> ObtenerPaginado(string descripcion, string categoria, string sidx, string sord, int page, int rows, out int total)
        {
            return _datos.ObtenerPaginado(descripcion, categoria, sidx, sord, page, rows, out total);
        }

        public List<Categoria> ObtenerCategorias()
        {
            return _datos.ObtenerCategorias();
        }
    }
}
