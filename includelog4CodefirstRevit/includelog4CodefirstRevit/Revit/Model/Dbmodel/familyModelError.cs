
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
        public string familyһ�������� { get; set; }
        [StringLength(255)]
        public string family���������� { get; set; }
      

        [StringLength(255)]
        public string familysymbolname���������� { get; set; }
        [StringLength(255)]
        public string familyinstancename��ʵ��ID { get; set; }

        //[StringLength(255)]
        //public string familyrealname { get; set; }
        [StringLength(255)]
        public string errorinfo { get; set; }
        [StringLength(255)]
        public string correctinfo { get; set; }

    }
}
