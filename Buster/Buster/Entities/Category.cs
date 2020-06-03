using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Buster.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime RegisterDate { get; set; }
        public List<Product> Product { get; set; }
    }
}
