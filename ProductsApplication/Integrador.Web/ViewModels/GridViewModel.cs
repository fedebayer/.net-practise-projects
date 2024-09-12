using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Integrador.Web.ViewModels
{
    public class GridViewModel
    {
        public string sidx { get; set; }
        public string sord { get; set; }
        public int page { get; set; }
        public int rows { get; set; }

        public GridViewModel()
        {
            this.sidx = "";
            this.sord = "";
            this.page = 0;
            this.rows = 0;
        }
    }
}