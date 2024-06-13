using MVCApplication.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCApplication.Controllers
{
    public class ProvinciaController : Controller
    {
        //
        // GET: /Provincia/

        public ActionResult Index()
        {
            IList<ProvinciaModel> provincias = new List<ProvinciaModel>();

            provincias.Add(new ProvinciaModel() { Id = 1, Descripcion = "Buenos Aires" });
            provincias.Add(new ProvinciaModel() { Id = 2, Descripcion = "Córdoba" });
            provincias.Add(new ProvinciaModel() { Id = 3, Descripcion = "Entre Ríos" });

            return View("Index", provincias);
        }

        public ActionResult Edit(int idProvincia)
        {
            IList<ProvinciaModel> provincias = ObtenerProvincias();

            //OJO! Esto se resolvería con la consulta al correspondiente
            //negocio. Es para poder ver en el ejemplo el ActionResult
            ProvinciaModel provincia =
                (from prov in provincias
                 where prov.Id == idProvincia
                 select prov).First();

            return View("Edit", provincia);
        }

        private IList<ProvinciaModel> ObtenerProvincias()
        {
            ProvinciaModel provincia1 = new ProvinciaModel() { Id = 1, Descripcion = "Buenos Aires" };
            ProvinciaModel provincia2 = new ProvinciaModel() { Id = 2, Descripcion = "Córdoba" };
            ProvinciaModel provincia3 = new ProvinciaModel() { Id = 3, Descripcion = "Entre Ríos" };
            return new List<ProvinciaModel>
            {
                provincia1,
                provincia2,
                provincia3
            };
        }

        public ActionResult Edit(ProvinciaModel provincia)
        {
            if (ModelState.IsValid)
            {
                //... Aquí va el código
                //... para almacenar los cambios

                return RedirectToAction("Index");
            }
            return View("Edit", provincia);
        }

        public string CargarPopup()
        {
            DatosPopup datos = new DatosPopup();
            datos.InSitu = true;
            datos.Tercerizado = false;
            datos.TiposTrabajo = new List<TipoTrabajo>();
            datos.TiposTrabajo.Add(new TipoTrabajo() { Id = 1, Descripcion = "Tipo1" });
            datos.TiposTrabajo.Add(new TipoTrabajo() { Id = 2, Descripcion = "Tipo2" });
            datos.TiposTrabajo.Add(new TipoTrabajo() { Id = 3, Descripcion = "Tipo3" });

            System.IO.StringWriter sw = new System.IO.StringWriter();
            ViewEngineResult ver = ViewEngines.Engines.FindPartialView(this.ControllerContext, "DatosPopup");
            this.ViewData.Model = datos;
            ViewContext vc = new ViewContext(this.ControllerContext, ver.View, this.ViewData, this.TempData, sw);

            ver.View.Render(vc, sw);
            return sw.GetStringBuilder().ToString();
        }

        public string GuardarDatosPopup(DatosPopup datosPopup)
        {
            string respuesta = "";
            try
            {
                if (ModelState.IsValid)
                {
                    respuesta = "TODO OK!!!";//TODO: realizar las operaciones necesarias de la ViewModel
                }
            }
            catch (Exception ex) { respuesta = ex.Message; }
            return respuesta;
        }
    }
}