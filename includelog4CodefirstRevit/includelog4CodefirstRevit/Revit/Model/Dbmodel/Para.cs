using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace includelog4CodefirstRevit.Revit.Model.Dbmodel
{
   public class ParaModel
    {
        [Key]
        [StringLength(255)]
        public string guidid { get; set; }
        [StringLength(255)]
        public string para族ID { get; set; }
        [StringLength(255)]
        public string para参数名 { get; set; }
        [StringLength(255)]
        public string para参数类型 { get; set; }


        [StringLength(255)]
        public string para参数值 { get; set; }
        [StringLength(255)]
        public string para参数属于自定义 { get; set; }
        [StringLength(255)]
        public string date数据产生时间 { get; set; }
        [StringLength(255)]
        public string displayUnitType { get; set; }
        

    }
}
