using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace includelog4CodefirstRevit.Revit.Model.Uimodel
{
  public  class walluserdefine:userdefine
    {
        //名称中的汉字，唯一。并且在人工设置的时候互相不包含。
        //第一个版本，汉字左边不能有任何内容，右边不能包含汉字

        //闸阀-DN 、、150-1.6、、MPa  2端固定，但是中间的数字，可以进行人工变化
        //public string wall族实例ID { get; set; }
        //public string wall一级族名称 { get; set; }//墙           门
        //public string wall二级族名称 { get; set; }//基本墙，叠层墙，幕墙 选择之一     单扇-与墙齐 选择之一
        //public string wall族类型名称 { get; set; }//常规-200mm，内部-砌块墙190 等选择之一； 750*2000mm ，先用这个进行判断

        //public string wall类别Category名称 { get; set; }//在族设计的时候，会用到

        public int wall墙的厚度{ get; set; }


}


}
