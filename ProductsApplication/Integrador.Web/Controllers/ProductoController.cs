/*FB: Se cambian las variables de tipo N, U Y D a New, Update y Delete para ser mas representativo y evitar confusion
      Ademas se agregan en constantes para mas facil representacion

      Se termina la funcionalidad que estaba incompleta de verificar que no exista y si es asi devolver un error explicando la situacion

      Se agregan en RESX variables y se utilizan para los mensajes de eliminado/actulizado/agregado correctamente y no se pudo encontrar producto*/

using Entidades;
using Entidades.Filtros;
using Framework.Common;
using Integrador.Web.ViewModels;
using Integrador.Web.ViewModels.Producto;
using Resources;
using Servicios.Implementaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Integrador.Controllers
{
    public class ProductoController : Controller
    {
        private ServicioProducto servicioProducto;

        public ProductoController()
        {
            servicioProducto = new ServicioProducto();
        }

        public ActionResult Index()
        {
            try
            {
                ProductoViewModel model = new ProductoViewModel();

                ViewBag.Categorias = ObtenerListaCategorias();
                ViewBag.CategoriasDescripcion = ObtenerListaCategoriasDescripcion();

                return View(model);
            }
            catch (Exception)
            {
                ErrorViewModel errorModel = new ErrorViewModel(Global.ErrorPantalla);
                return View(Constantes.NAME_VIEW_ERROR, errorModel);
            }
        }

        public JsonResult BuscarProducto(ProductoViewModel listaVM)
        {
            JsonGridData jsonGridData = new JsonGridData();

            try
            {
                #region Validaciones

                if (string.IsNullOrEmpty(listaVM.Descripcion) || listaVM.Descripcion.Equals(Global.Null) || listaVM.Descripcion.Equals("Todos"))
                    listaVM.Descripcion = string.Empty;

                if (string.IsNullOrEmpty(listaVM.Categoria) || listaVM.Categoria.Equals(Global.Null) || listaVM.Categoria.Equals("Todos"))
                    listaVM.Categoria = string.Empty;

                #endregion Validaciones

                int total = 0;
                GrillaProductoViewModel grillaProductoViewModel = new GrillaProductoViewModel();

                grillaProductoViewModel.Items = servicioProducto.ObtenerPaginado(listaVM.Descripcion,
                                                                                     listaVM.Categoria,
                                                                                     listaVM.sidx,
                                                                                     listaVM.sord,
                                                                                     listaVM.page,
                                                                                     listaVM.rows,
                                                                                     out total).Select(producto => new GrillaProductoViewModel.ItemViewModel()
                                                                                     {
                                                                                         ID = producto.Id.ToString(),
                                                                                         Descripcion = producto.Descripcion.ToString(),
                                                                                         Precio = (int)producto.Precio,
                                                                                         IdCategoria = (int)producto.IdCategoria,
                                                                                         DescripcionCategoria = producto.DescripcionCategoria.ToString()
                                                                                     }).ToList();

                jsonGridData.rows = grillaProductoViewModel.Items;
                jsonGridData.total = (total / listaVM.rows) + (total % listaVM.rows > 0 ? 1 : 0);
                jsonGridData.records = total;
                jsonGridData.page = listaVM.page;
                jsonGridData.rowNum = listaVM.rows;
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = e.Message });
            }

            return Json(jsonGridData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GestionProducto(string tipo, string descripcion, int IdCategoria, string id)
        {
            JsonData jsonData = new JsonData();

            try
            {
                bool result = false;
                Producto prod;
                if (tipo == Constantes.NEW)
                {
                    //SE VALIDA QUE EL PRODUCTO NO EXISTA EN LA BASE DE DATOS
                    IList<Producto> productos = servicioProducto.ObtenerPorFiltro(new ProductoFiltro() { Descripcion = descripcion });


                    if (productos.Count > 0)
                    {
                        jsonData.content = Global.ProductoYaExiste;
                        jsonData.result = JsonData.Result.Error;
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }

                    prod = new Producto();
                    prod.Descripcion = descripcion;
                    prod.IdCategoria = IdCategoria;

                    servicioProducto.Insertar(prod);

                    result = true;
                }
                else
                {
                    IList<Producto> productos = servicioProducto.ObtenerPorFiltro(new ProductoFiltro() { Id = int.Parse(id) });
                    if (productos.Count > 0)
                    {
                        prod = productos.FirstOrDefault();

                        if (tipo == Constantes.UPDATE)
                        {
                            prod.Id = int.Parse(id);
                            prod.Descripcion = descripcion;
                            prod.IdCategoria = IdCategoria;

                            servicioProducto.Actualizar(prod);
                            jsonData.content = Global.ProductoActualizado;
                            jsonData.result = JsonData.Result.Ok;
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                        else if (tipo == Constantes.DELETE)
                        {
                            servicioProducto.Borrar(prod.Id);
                            jsonData.content = Global.ProductoEliminado;
                            jsonData.result = JsonData.Result.Ok;
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        jsonData.content = Global.ProductoNoEncontrado;
                        jsonData.result = JsonData.Result.Error;
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }

                if (result)
                {
                    jsonData.content = Global.ProductoIngresado;
                    jsonData.result = JsonData.Result.Ok;
                }
                else
                {
                    jsonData.content = Global.NoSePudoRealizarLaOperacion;
                    jsonData.result = JsonData.Result.Error;
                }
            }
            catch (Exception)
            {
                jsonData.content = Global.ErrorGenerico;
                jsonData.result = JsonData.Result.Error;
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private List<SelectListItem> ObtenerListaCategorias()
        {
            List<Categoria> categorias = ObtenerCategorias(); ;

            List<SelectListItem> selectListItems = categorias.Select(loc => new SelectListItem
            {
                Value = loc.Id.ToString(),
                Text = loc.Descripcion
            }).ToList();

            return selectListItems;
        }

        private List<SelectListItem> ObtenerListaCategoriasDescripcion()
        {
            List<Categoria> categorias = ObtenerCategorias(); ;

            List<SelectListItem> selectListItems = categorias.Select(loc => new SelectListItem
            {
                Value = loc.Descripcion,
                Text = loc.Descripcion
            }).ToList();

            return selectListItems;
        }

        private dynamic ObtenerCategorias()
        {
            var categorias = servicioProducto.ObtenerCategorias();

            return categorias;
        }
    }
}