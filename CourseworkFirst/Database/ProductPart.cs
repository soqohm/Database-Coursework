namespace CourseworkFirst
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductPart
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(40)]
        public string ProductId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(40)]
        public string PartId { get; set; }

        public int Count { get; set; }

        public virtual Part Part { get; set; }

        public virtual Product Product { get; set; }
    }
}
