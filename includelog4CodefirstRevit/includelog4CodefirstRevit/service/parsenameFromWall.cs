using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using includelog4CodefirstRevit.Revit.Model.Uimodel;
namespace includelog4CodefirstRevit.service
{
 public   class parsenameFromWall:parsestring
    {
        public familyModelError parsename(includelog4CodefirstRevit.Revit.Model.Uimodel.userdefine ud, List<revitnamerule> fnfb)
        {
            //这个网站是验证正则表达式非常好的网站https://regexr.com/
            familyModelError fe = new familyModelError();

            fe.guidid = Guid.NewGuid().ToString();
            //把它强制转换为墙的类型
            walluserdefine wud = ud as walluserdefine;
            string input = wud.family族类型名称;
            int width = wud.wall墙的厚度;
            int i = 0;
            fe.familyinstancename族实例ID = wud.family族实例ID;
            fe.familysymbolname族类型名称 = wud.family族类型名称;
            fe.family一级族名称 = wud.family一级族名称;
            fe.family二级族名称 = wud.family二级族名称;
            

            foreach (var x in fnfb)
            {
                i++;
                string pattern = x.name + ".*" + width.ToString();
                if (input == "建筑砌块墙 -200")
                {
                    string tes = "";
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(input, pattern))
                {
                    //如果规则汉字+墙厚度匹配。后面判断剩余部分是否包含汉字
                    fe.correctinfo = "该规则匹配正确，包含规则汉字，且包含墙的厚度";
                    break;
                }
                
               if (i == fnfb.Count)
                    {
                        //说明是最后一条，也没有匹配上
                        fe.errorinfo = "与命名规则不匹配，因为命名中没有规定的汉字或无墙的厚度";
                    }
                }


            
            return fe;
        }
        private bool bool已经明确本条的匹配结果(string input, string rulename, int width, familyModelError fe)
        {
            bool result = false;
            if (input == rulename)
            {
                fe.errorinfo = "不符合要求，命名中只有规定的汉字，但是没有其他的内容";
                result = true;
                return result;
            }
            string[] substrings = Regex.Split(input, rulename);


            if (substrings.Count() > 1)
            {
                //说明第一个汉字是匹配了，下面需要进一步匹配，不允许剩余的还有汉字
                foreach (var sub in substrings)
                {
                    if (CheckStringChineseReg(sub))
                    {
                        fe.errorinfo = "去掉规定的汉字之外，还有其他汉字";
                        result = true;
                        break;
                        //如果有汉字，说明有错误,是优先级最高的错误
                    }

                }
                //
                result = special(substrings, width, fe);
            }
            if (substrings.Count() == 1)
            {
                //说明本条没有找到，需要到下一条看看

                return result;

            }
            return result;

        }
       





      
    }
  


    
}
