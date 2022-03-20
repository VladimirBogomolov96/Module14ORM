using System;
using System.Collections.Generic;

#nullable disable

namespace Module14
{
    public partial class Order
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
