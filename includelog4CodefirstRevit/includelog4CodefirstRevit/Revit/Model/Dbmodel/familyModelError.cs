
namespace includelog4CodefirstRevit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("dbo.familyModel")]
    //[Table("familyModel", Schema = "dbo")]
    public partial class familyModelError
    {
        [Key]
        [StringLength(255)]
        public string guidid { get; set; }

        [StringLength(255)]
        public string family一级族名称 { get; set; }
        [StringLength(255)]
        public string family二级族名称 { get; set; }
      

        [StringLength(255)]
        public string familysymbolname族类型名称 { get; set; }
        [StringLength(255)]
        public string familyinstancename族实例ID { get; set; }

        //[StringLength(255)]
        //public string familyrealname { get; set; }
        [StringLength(255)]
        public string errorinfo { get; set; }
        [StringLength(255)]
        public string correctinfo { get; set; }

    }
}
