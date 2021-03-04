using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Models.ViewModel
{
    public class DetailsVM
    {
        public Product Product { get; set; }
        public bool ExistsInCart{ get; set; }
    }
}
