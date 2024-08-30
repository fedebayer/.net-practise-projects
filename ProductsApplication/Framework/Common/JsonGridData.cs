using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Common
{
    public class JsonGridData
    {
        public dynamic rows { get; set; }
        public int total { get; set; }
        public int records { get; set; }
        public int page { get; set; }
        public int rowNum { get; set; }
    }
}
