using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buster.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
