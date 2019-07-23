namespace includelog4CodefirstRevit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("revitnamerule")]
    public partial class revitnamerule
    {
        [Required]
        [StringLength(255)]
        public string familytype { get; set; }

        [Key]
        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string numbe { get; set; }
    }
}
