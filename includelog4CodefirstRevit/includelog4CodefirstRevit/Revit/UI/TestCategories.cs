using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using includelog4CodefirstRevit;
using includelog4CodefirstRevit.Revit.Model.Dbmodel;

namespace HelloWorld
{
    public class TestCategories
    {
        [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
        //此处的TransactionMode比如由Automatic改为Manual，不然在调试时会出现“revit无法运行外部程序”

        public class Categories : IExternalCommand
        {
            public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {//类别的章节中
                includelog4CodefirstRevit.Logger.Setup();
                Document document = commandData.Application.ActiveUIDocument.Document;
                //从当前文档对象中取到Setting对象
                Settings documentSettings = document.Settings;
                String prompt = "Number of all categories in current Revit document: " + documentSettings.Categories.Size + "\n";

                foreach (Category category in documentSettings.Categories)
                    
                Common.utility.WriteDebugLog(string.Format("category.ParentName:{0},category.Name:{1},category.SubCategories.Size:{2}\r\n", category.Parent==null?"父节点为空": category.Parent.Name, category.Name,category.SubCategories.Size));

                //族类型、族实例
                //FamilylnstanceFilter familylnstanceFilter = new FamilylnstanceFilter(RevitDoc, symbol.Id)；
                FilteredElementCollector collector = new FilteredElementCollector(document);
                ElementClassFilter familyinstanceFilter = new ElementClassFilter(typeof(FamilyInstance));
                
                //都是从过滤器中，进行过滤，然后得到的是元素。然后针对元素进行处理。是否可以向下转型为FamliyInstance等？

                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);

                FilteredElementCollector coll = new FilteredElementCollector(document);
                List<Element> elems = coll.OfClass(typeof(FamilyInstance)).ToList();

                ICollection<Element> founds= collector.WherePasses(filter).ToElements();


                foreach (FamilyInstance element in elems)
                {
                    
                    Common.utility.WriteDebugLog(string.Format("element.GetType():{0}\r\n",element.Category.Name,element.GetType().Name));
                    //Element是所有的父类，FamilySymbol是子类，FamilyInstance也是子类，Family也是子类
                    // if (element.GetType().Equals(typeof(FamilyInstance)))
                    // {
                    //  FamilyInstance fi = element as FamilyInstance;
                    //http://blog.sina.com.cn/s/blog_c51368a50102wkyk.html
                    //可以分析一下RevitLookup中是否可以去到类型的属性？
                    FamilySymbol symbol= element.Symbol; 
                        Family family = symbol.Family;
                    //问题，Family 在load的时候，文件名。在一个Family里面，应该有多个symbol
                    //问题，FamilyInstance仅仅获得结构等。但是没有墙，那么在系统中，一共有多少个系统族，FamilyInstance
                    //如果是wall的族，那么过滤实例的时候，是否能过滤到？
                    //已经试验确定，通过FamilyInstance过滤取数的时候，只能获得结构混凝土 - 矩形梁，混凝土 - 矩形 - 柱
                    Common.utility.WriteDebugLog(string.Format("elementname:{0},familyinstancename:{1},Family:{2}\r\n", element.Name, symbol.Name,family.Name));

                   // }
                   // else
                  //  {
                       // Common.utility.WriteErrorLog(string.Format("这个是元素但不是FamilyInstance:{0}\r\n", element.Name));
                   // }
                }
                //问：FamilyInstance与Element有什么区别？
                //FamilyInstanceFilter familyInstanceFilter=new FamilyInstanceFilter()

                //3.1.7 从族创建族实例
                //============代码片段3-12 放置类型为"0762 x 2032 mm"的门============
                string doorTypeName = "0762 x 2032 mm";
                FamilySymbol doorType = null;

                // 在文档中找到名字为"0762 x 2032 mm"的门类型
               // ElementFilter doorCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);
                ElementFilter familySymbolFilter = new ElementClassFilter(typeof(FamilySymbol));
               // LogicalAndFilter andFilter = new LogicalAndFilter(doorCategoryFilter, familySymbolFilter);
                FilteredElementCollector doorSymbols = new FilteredElementCollector(document);
              //  doorSymbols = doorSymbols.WherePasses(andFilter);

                doorSymbols = doorSymbols.WherePasses(familySymbolFilter);
                //可以根据这FamilySymbol，来获取所有的类型属性，这样就可以不区分
                bool symbolFound = false;
                foreach (FamilySymbol symbol in doorSymbols)
                {
                    if (symbol.Name == doorTypeName)
                    {
                        symbolFound = true;
                        doorType = symbol;
                        break;
                    }
                    

          Common.utility.WriteDebugLog(string.Format("FamilySymbolName:{0},symbol.Category:{1},symbol.FamilyName:{2},symbol.GetType().Name:{3} \r\n", symbol.Name, symbol.Category, symbol.FamilyName,symbol.GetType().Name));
                    foreach (Parameter p in symbol.Parameters)
                    {
                        Common.utility.WriteDebugLog(string.Format("p.Definition.Name:{0},Name,p.AsValueString():{1},p.Definition.UnitType:{2} \r\n", p.Definition.Name,p.AsValueString(),p.Definition.UnitType));
                    }
                }

                // 如果没有找到，就加载一个族文件
                //if (!symbolFound)
                //{
                //    string file = @"C:\ProgramData\Autodesk\RVT 2018\Libraries\China\机电\安防\被动红外入侵探测器 - 壁装式.rfa";
                //    Family family;
                //    bool loadSuccess = document.LoadFamily(file, out family);
                //    if (loadSuccess)
                //    {
                //        foreach (ElementId doorTypeId in family.GetValidTypes())
                //        {
                //            doorType = document.GetElement(doorTypeId) as FamilySymbol;
                //            if (doorType != null)
                //            {
                //                if (doorType.Name == doorTypeName)
                //                {
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        Autodesk.Revit.UI.TaskDialog.Show("Load family failed", "Could not load family file '" + file + "'");
                //    }
                //}


                return Autodesk.Revit.UI.Result.Succeeded;
            }
        }
    }
}
