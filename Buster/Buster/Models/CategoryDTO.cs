using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Buster.Models
{
    public class CategoryDTO
    {
        [Required]
        public string Name { get; set; }
        public DateTime RegisterDate { get; set; }
        public List<ProductDTO> Product { get; set; }
    }
}
