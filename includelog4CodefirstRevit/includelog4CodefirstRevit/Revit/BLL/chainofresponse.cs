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
using System.Xml;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using includelog4CodefirstRevit.Revit.Model.Uimodel;
using includelog4CodefirstRevit.Revit.Model.Dbmodel;
using Autodesk.Revit.DB.Plumbing;
namespace includelog4CodefirstRevit
{
    public class parent
    {
        public string type;
       // public familyModelError fmd = null;

        public List<revitnamerule> fnb = new List<revitnamerule>();
        //输入单独的一个文件，输入一个Model的文件，作为参数传递



        public virtual familyModelError parse(Element ele, List<revitnamerule> rnr)
        {
            //由子类进行覆盖实现
            return null;
        }
       
        
    }
        public class systemfamily : parent
        {
            //public HostObject ho;
            public override familyModelError parse(Element ele, List<revitnamerule> rnr)
            {
            //if (ele.GetType().BaseType == typeof(HostObject))//系统族
            //{
            //    userdefine ud = new userdefine();
            //    HostObject hi = ele as HostObject;

            //    set族参数(ud, hi);
            //    ud.family族类型名称 = ele.Name;
            //    Common.utility.WriteErrorLog(string.Format("解析之前，获取到的系统族参数，系统族的ID {0},family一级族名称{1},family二级族名称{2}，family族类型名称 {3}，family类别Category名称{4}\r\n", ud.family族实例ID,ud.family一级族名称,ud.family二级族名称,ud.family族类型名称,ud.family类别Category名称));

            //    if (type == hi.Category.Name)
            //    {
            //        personal个性化设置Bywall(ele, ud, rnr);
            //    }
            //    else
            //    {
            //        //下面要进行解析
            //        //List<revitnamerule> rnrfilter = rnr.Where(the => the.familytype == type).ToList();
            //        personal个性化设置(ele, ud, rnr);
            //    }
            return null;
                }

                
            
            protected void set族参数(userdefine ud, HostObject hi)
            {
                //根据传递过来的hostobject，对wall类型的参数进行赋值
                ud.family族实例ID = hi.Id.ToString();
                ud.family一级族名称 = hi.Category.Name;
                ud.family二级族名称 = "";
                ud.family类别Category名称 = "";

            }

        }

       public class Wallstep : systemfamily
        {
            public Wallstep()
            {
                type = "墙";
            }
        public override familyModelError parse(Element ele, List<revitnamerule> rnr)
        {
            familyModelError fme = null;
            userdefine ud = new userdefine();
                HostObject hi = ele as HostObject;

            if (ele.GetType().BaseType == typeof(HostObject) && type == hi.Category.Name)//系统族
            {
                set族参数(ud, hi);
                ud.family族类型名称 = ele.Name;
                Common.utility.WriteDebugLog(string.Format("解析之前，获取到的系统族参数，系统族的ID {0},family一级族名称{1},family二级族名称{2}，family族类型名称 {3}，family类别Category名称{4}\r\n", ud.family族实例ID, ud.family一级族名称, ud.family二级族名称, ud.family族类型名称, ud.family类别Category名称));

               
             return   personal个性化设置Bywall(ele, ud, rnr);
               
             }
            return fme;
            }


        
        private  familyModelError personal个性化设置Bywall(Element ele, userdefine ud, List<revitnamerule> rnr)
            {
                Wall wall = ele as Wall;
                walluserdefine wud = new Revit.Model.Uimodel.walluserdefine();
                wud.wall墙的厚度 = Convert.ToInt32(wall.Width * 304.5);
                wud.family一级族名称 = ud.family一级族名称;
                wud.family二级族名称 = ud.family二级族名称;
                wud.family族实例ID = ud.family族实例ID;
                wud.family族类型名称 = ud.family族类型名称;
                wud.family类别Category名称 = ud.family类别Category名称;

                Common.utility.WriteDebugLog(string.Format("解析之前，获取到的系统族中墙的参数，系统族的ID {0},family一级族名称{1},family二级族名称{2}，family族类型名称{3}，family类别Category名称{4}，family墙的厚度{5}\r\n", ud.family族实例ID, ud.family一级族名称, ud.family二级族名称, ud.family族类型名称, ud.family类别Category名称,wud.wall墙的厚度));
                includelog4CodefirstRevit.service.parsenameFromWall pf = new service.parsenameFromWall();
                return pf.parsename(wud, rnr);

                //针对墙的特殊属性

                // List<revitnamerule> rnrfilter = rnr.Where(the => the.familytype == type).ToList();
            }


        }
       public class nonwall : systemfamily
        {
            public nonwall()
            {
                type = "非墙";
            }

        public override familyModelError parse(Element ele, List<revitnamerule> rnr)
        {
            familyModelError fme = null;
            userdefine ud = new userdefine();
            HostObject hi = ele as HostObject;
            if (ele.GetType().BaseType == typeof(HostObject)&&hi.Category.Name!="墙")//系统族
            {
               

                set族参数(ud, hi);
                ud.family族类型名称 = ele.Name;
                //ParameterSet pe= ele.Parameters;
                
                Common.utility.WriteErrorLog(string.Format("解析之前，获取到的系统族参数，系统族的ID {0},family一级族名称{1},family二级族名称{2}，family族类型名称 {3}，family类别Category名称{4}\r\n", ud.family族实例ID, ud.family一级族名称, ud.family二级族名称, ud.family族类型名称, ud.family类别Category名称));

                    //下面要进行解析
                    //List<revitnamerule> rnrfilter = rnr.Where(the => the.familytype == type).ToList();
                 return   personal个性化设置(ele, ud, rnr);
                }
            return fme;
            }
        protected familyModelError personal个性化设置(Element ele, userdefine ud, List<revitnamerule> rnr)
        {
            parsestring ps = new parsestring();
            return ps.parsename(ud, rnr);
        }

    }

        public class nonsystemfamily : parent
        {
            //public FamilyInstance fi;
            public override familyModelError parse(Element ele, List<revitnamerule> rnr)
            {
                if (ele.GetType() == typeof(FamilyInstance))//非系统族
                {
                    door ud = new door();
                    FamilyInstance fi = ele as FamilyInstance;

                  //  if (type == fi.Category.Name)
                    //{
                        //根据传递过来的hostobject，对wall类型的参数进行赋值
                        ud.family族实例ID = fi.Id.ToString();
                        ud.family一级族名称 = fi.Symbol.FamilyName;
                        ud.family二级族名称 = "";
                        ud.family族类型名称 = fi.Symbol.Name;
                        ud.family类别Category名称 = "";
                    //}
                //下面要进行解析,解析的时候，把Model的类传过去
                Common.utility.WriteErrorLog(string.Format("解析之前，获取到的非系统族中的参数，系统族的ID {0},family一级族名称{1},family二级族名称{2}，family族类型名称{3}，family类别Category名称{4}\r\n", ud.family族实例ID, ud.family一级族名称, ud.family二级族名称, ud.family族类型名称, ud.family类别Category名称));
                parsestring p = new includelog4CodefirstRevit.parsestring();
                return p.parsename(ud, rnr);

                }
            return null;

            }
        


    }
       public class doorstep : nonsystemfamily
        {
            public doorstep()
            {
                type = "门 and other";

            }

        //解析所有的参数属性
        public override familyModelError parse(Element ele, List<revitnamerule> rnr)
        {
            //parse( doc,  ele,  rnr);
            return null;
        }
        public List<ParaModel>  parseSys(Document doc, Element ele, List<revitnamerule> rnr)
        {
            List<includelog4CodefirstRevit.Revit.Model.Dbmodel.ParaModel> pmodelist = new List<includelog4CodefirstRevit.Revit.Model.Dbmodel.ParaModel>();
            //取得系统的参数     
            string[] strs = System.Enum.GetNames(typeof(BuiltInParameter));
           
            foreach (string str in strs)
            {

                BuiltInParameter paramEnum = (BuiltInParameter)System.Enum.Parse(typeof(BuiltInParameter), str);// 查看枚举名称对应的BuiltInParameter
                if (ele.Id.ToString() == "462276")
                { 
                    if (paramEnum == BuiltInParameter.CURVE_ELEM_LENGTH)
                    {
                        string xx = "he";
                    }
                }
                Parameter tmpParam = ele.get_Parameter(paramEnum);// 通过BuiltInParameter的到参数
               // Parameter tmpParam1 = ele.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);

                if (tmpParam != null)
                {
                    includelog4CodefirstRevit.Revit.Model.Dbmodel.ParaModel pmodel = new Revit.Model.Dbmodel.ParaModel();
                    pmodel.guidid = Guid.NewGuid().ToString();
                    pmodel.para参数名 = tmpParam.Definition.Name;
                    pmodel.para参数值= GetParamVal( doc, tmpParam);
                    pmodel.para族ID = tmpParam.Element.Id.ToString();
                    pmodel.date数据产生时间 = System.DateTime.Now.ToLocalTime().ToString();
                    pmodel.para参数属于自定义 = "实例参数-系统自动实例参数";
                    pmodelist.Add(pmodel);
                    // sb.Append(e.Name + ":\t" + str + "(" + paraName + ")" + " \t= \t" + val + "\r\n");
                }
               
            }
            //取得用户自定义参数


               
            
            return pmodelist;
            }
        public List<ParaModel> parse用户自定义参数(Document doc, Element ele, List<ParaModel> pmodelist)
        {
            List<ParaModel> pmode用户自定义参数List = new List<ParaModel>();

            //取得用户自定义参数
           
           // Pipe pipe = ele as Pipe;
            //foreach (Parameter p in pipe.PipeType.Parameters)
            //{
                //https://www.cnblogs.com/greatverve/archive/2011/11/07/api-parameter-material.html
                //https://www.cnblogs.com/zhuweisky/p/4209058.html
                //http://blog.oraycn.com/OAUS.aspx
                //http://www.oraycn.com/AboutUs.aspx
            //}
                ParameterSet parameters = ele.Parameters;
            
            foreach (Parameter parameter in parameters)
            {
                bool w = false;
                if (parameter.Definition.Name == "sjktype")
                {
                    w = true;
                    System.Diagnostics.Debug.Assert(w);
                }
                if (pmodelist.Where(x => x.para参数名.Contains(parameter.Definition.Name)).Count() < 1)
                {
                    includelog4CodefirstRevit.Revit.Model.Dbmodel.ParaModel pmodel = new Revit.Model.Dbmodel.ParaModel();
                    pmodel.guidid = Guid.NewGuid().ToString();
                    pmodel.para参数名 = parameter.Definition.Name;
                    pmodel.para族ID = parameter.Element.Id.ToString();
                    pmodel.para参数值 = GetParamVal(doc, parameter);
                    pmodel.date数据产生时间 = System.DateTime.Now.ToLocalTime().ToString();
                    pmodel.para参数属于自定义 = "实例参数-用户自定义实例参数";
                    pmode用户自定义参数List.Add(pmodel);
                }


            }

            return pmode用户自定义参数List;
        }
        public List<ParaModel> parse类型参数(Document doc, Element ele, List<ParaModel> pmodelist)
        {
            List<ParaModel> plist = new List<ParaModel>();
            Type type = ele.GetType();

            //ementType et = ele.GetType();
            if (ele.Id.ToString() == "462276")
            {
                string ss = "";

                RoofBase r;
                
                

                Wall wall = ele as Wall;
                //要根据每一个类型来取得类型参数,如WallType
                foreach (Parameter p in wall.WallType.Parameters)
                {
                    
                    ParaModel pmodel = new ParaModel();
                    pmodel.guidid = Guid.NewGuid().ToString();
                    pmodel.para参数名 = p.Definition.Name;
                    pmodel.para族ID = ele.Id.ToString();
                    pmodel.para参数值 = GetParamVal(doc, p);
                    pmodel.date数据产生时间 = System.DateTime.Now.ToLocalTime().ToString();
                    if (p.Definition.Name == "sjktype")
                        pmodel.para参数属于自定义 = "类型参数-用户自定义类型参数";
                    else
                        pmodel.para参数属于自定义 = "类型参数-系统自定义类型参数";

                    //pmodel.displayUnitType= p.DisplayUnitType.ToString();
                    
                    plist.Add(pmodel);
                }
            }
            //ementType et = ele.GetType();
            if (ele.Id.ToString() == "475195")//是一个管子
            {
                string ss = "";



                Pipe pipe = ele as Pipe;
                //要根据每一个类型来取得类型参数,如WallType
                
                foreach (Parameter p in pipe.PipeType.Parameters)
                {

                    ParaModel pmodel = new ParaModel();
                    pmodel.guidid = Guid.NewGuid().ToString();
                    pmodel.para参数名 = p.Definition.Name;
                    pmodel.para族ID = ele.Id.ToString();
                    pmodel.para参数值 = GetParamVal(doc, p);
                    pmodel.date数据产生时间 = System.DateTime.Now.ToLocalTime().ToString();
                    if (p.Definition.Name == "pipetype")
                        pmodel.para参数属于自定义 = "类型参数-用户自定义类型参数";
                    else
                        pmodel.para参数属于自定义 = "类型参数-系统自定义类型参数";

                   // pmodel.displayUnitType = p.DisplayUnitType.ToString();
                   
                    plist.Add(pmodel);
                }
            }
            return plist;
        }
//  参数的类型string与int取得的方法有所不同,可以封装成一个函数。 
//得到参数的值 
public  string GetParamVal(Document doc, Parameter p)
        {
            string strResult = "";
            switch (p.StorageType)
            {
                case StorageType.Double:
                    strResult = p.AsValueString();
                    break;
                case StorageType.ElementId:
                    if ( doc.GetElement(p.AsElementId()) != null)
                        strResult = doc.GetElement(p.AsElementId()).Name;
                     break;
                case StorageType.String:
                    strResult = p.AsString();
                    break;
                case StorageType.Integer:
                    strResult = p.AsInteger().ToString();
                    break;
            }
            return strResult;
        }


    }

}

