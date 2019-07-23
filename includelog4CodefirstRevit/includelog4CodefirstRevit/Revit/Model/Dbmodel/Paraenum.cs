using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace includelog4CodefirstRevit.Revit.Model.Dbmodel
{
   public class ParaEnum
    {
        [Key]
        [StringLength(255)]
        public string guidid { get; set; }
        [StringLength(255)]
        public string ParaEnumID { get; set; }
        [StringLength(255)]
        public string ParaEnum名称 { get; set; }
        

    }
}
