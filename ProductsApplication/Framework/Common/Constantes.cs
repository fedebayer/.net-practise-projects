/*FB: Se agregan en constantes a New, Update y Delete para ser mas representativo y evitar confusion*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common
{
    public class Constantes
    {
        public const string NAME_VIEW_ERROR = "Error";
        public const string TODOS = "Todos";
        public const string SORT_ORDER_ASC = "asc";
        public const string SORT_ORDER_DESC = "desc";

        public const string NEW = "New";
        public const string UPDATE = "Update";
        public const string DELETE = "Delete";


        #region PRODUCTO - STORED PROCEDURE

        public const string SP_PRODUCTO_INSERT = "sp_insert_productos_fb";
        public const string SP_PRODUCTO_ACTUALIZAR = "sp_update_productos_fb";
        public const string SP_PRODUCTO_DELETE = "sp_delete_productos_fb";
        public const string SP_PRODUCTO_OBTENER_POR_FILTRO = "sp_obtener_por_filtro_productos_fb";
        public const string SP_PRODUCTO_OBTENER_PAGINADO = "sp_obtener_productos_fb";

        public const string SP_PRODUCTO_OBTENER_CATEGORIAS = "sp_obtener_categorias_fb";

        #endregion
    }
}
