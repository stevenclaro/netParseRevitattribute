using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace includelog4CodefirstRevit.Revit.Model.Uimodel
{
  public  class userdefine
    {
        public string family族实例ID { get; set; }
        public string family一级族名称 { get; set; }//墙           门
        public string family二级族名称 { get; set; }//基本墙，叠层墙，幕墙 选择之一     单扇-与墙齐 选择之一
        public string family族类型名称 { get; set; }//常规-200mm，内部-砌块墙190 等选择之一； 750*2000mm ，先用这个进行判断

        public string family类别Category名称 { get; set; }//在族设计的时候，会用到
    }
}
