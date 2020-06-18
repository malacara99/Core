﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Buster.Models
{
    public class ProductCreationDTO
    {
        [Required]
        public string Sku { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}