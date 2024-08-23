using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Filtros
{
    public class ProductoFiltro
    {
        public virtual string Descripcion { get; set; }

        public virtual string Categoria { get; set; }

        public virtual int? Id { get; set; }
    }
}
