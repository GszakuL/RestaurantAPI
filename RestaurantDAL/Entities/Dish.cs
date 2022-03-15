using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDAL.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }   
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public int RestaturantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
