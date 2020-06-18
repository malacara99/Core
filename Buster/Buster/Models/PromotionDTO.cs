using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buster.Models
{
    public class PromotionDTO
    {
        public int PromotionId { get; set; }
        public string Description { get; set; }
        public string CvePromotion { get; set; }
        public bool Active { get; set; }
    }
}
