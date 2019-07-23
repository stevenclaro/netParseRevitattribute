namespace includelog4CodefirstRevit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("employee")]
    public partial class employee
    {
        [Key]
        [StringLength(255)]
        public string Empid { get; set; }

        [StringLength(255)]
        public string name { get; set; }
    }
}
