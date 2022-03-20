using System;
using System.Collections.Generic;

#nullable disable

namespace Module14
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
        public int? Length { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
