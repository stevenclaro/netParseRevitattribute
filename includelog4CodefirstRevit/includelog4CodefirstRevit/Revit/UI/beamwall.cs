using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using includelog4CodefirstRevit;
using Autodesk.Revit.DB.Structure;

namespace HelloWorld
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    //此处的TransactionMode比如由Automatic改为Manual，不然在调试时会出现“revit无法运行外部程序”

    public class beamwall : IExternalCommand
    {
        List<string> list1 = new List<string>();
        Autodesk.Revit.ApplicationServices.Application application; 
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            List<FamilyInstance> beam所有的梁;
            includelog4CodefirstRevit.Logger.Setup();

            UIApplication uiApplication = commandData.Application;
            application = uiApplication.Application;

            UIDocument uiDocument = uiApplication.ActiveUIDocument;
            Document document = uiDocument.Document;

            beam所有的梁=getBeams得到Revit中的所有的梁(document);

            //includelog4CodefirstRevit.Model1 m = new includelog4CodefirstRevit.Model1();
            includelog4CodefirstRevit.ModelCodeFirst m = new includelog4CodefirstRevit.ModelCodeFirst();
             

            includelog4CodefirstRevit.ModelNameRuleDb mnrb = new ModelNameRuleDb();
            List<revitnamerule> rnr = mnrb.revitnamerule.ToList();

            Document doc = commandData.Application.ActiveUIDocument.Document;
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ElementClassFilter instanceFitler = new ElementClassFilter(typeof(FamilyInstance));
            ElementClassFilter hostFilter = new ElementClassFilter(typeof(HostObject));
            LogicalOrFilter andFilter = new LogicalOrFilter(instanceFitler, hostFilter);

            
            collector.WherePasses(andFilter);
           // collector.OfClass(typeof(FamilyInstance));//过滤获取到当前文件中所有的族实例
            IList<Element> CollectorList = collector.ToElements();
            string famliyName = "";

            Common.utility.WriteDebugLog(string.Format("该Revit文档中元素数量为{0}\r\n", CollectorList.Count));
            //Common.utility.WriteErrorLog(string.Format("文件数量总结为{0},目前处理的进度为{1},当前处理的文件名称为{2} \r\n", 1, 2, 3));


            includelog4CodefirstRevit.Revit.BLL.buildchain bc = new includelog4CodefirstRevit.Revit.BLL.buildchain();
            bc.getAllSubclass();
            List<parent> par = bc.chainlist;

            Common.utility.WriteDebugLog(string.Format("获取所有的子类元素数量为{0}\r\n", par.Count));

            List<familyModelError> fmeList = new List<includelog4CodefirstRevit.familyModelError>();

            for (int i = 0; i < CollectorList.Count; i++)
            {
                familyModelError fme;
                foreach (var x in par)
                {
                    fme = x.parse(CollectorList[i], rnr);
                    if (fme != null)
                    {
                        //只要找到第一个满足条件，就进行处理。并进行返还
                        
                        fmeList.Add(fme);
                       // break;
                    }
                }
            }

            m.familyModelErrors.AddRange(fmeList);

            includelog4CodefirstRevit.tools.npoiexcel npoi = new includelog4CodefirstRevit.tools.npoiexcel(fmeList);
            npoi.generateexcel();
            //Common.utility.WriteDebugLog(string.Format("Revit文件中的族实例数量为{0} \r\n", fmlist.Count));
            m.SaveChanges();
            

            return Autodesk.Revit.UI.Result.Succeeded;
        }
        //得到所有的Beams，方法判断的多了一点，不是特别精简，先可以用
        private List<FamilyInstance> getBeams得到Revit中的所有的梁(Document RevitDoc)
        {
            List<FamilyInstance> beamList=new List<FamilyInstance>();
            FilteredElementCollector filteredElementsStructural = new FilteredElementCollector(RevitDoc);
            ElementClassFilter classFilterStructural = new ElementClassFilter(typeof(FamilyInstance));
            filteredElementsStructural = filteredElementsStructural.WherePasses(classFilterStructural);
            System.Diagnostics.Trace.WriteLine("取得梁的个数： " + filteredElementsStructural.Count());

            Common.utility.WriteDebugLog(string.Format("取得梁的个数{0} \r\n", filteredElementsStructural.Count()));

            string messagex = "";
            foreach (FamilyInstance each in filteredElementsStructural)
            {
                switch (each.StructuralType)
                {
                    case StructuralType.Beam: // 结构梁
                        messagex = "FamilyInstance is a beam.";
                        //compute(each, wa);
                        beamList.Add(each);
                        break;
                    case StructuralType.Brace: // 结构支撑
                        messagex = "FamilyInstance is a brace.";
                        break;
                    case StructuralType.Column: // 结构柱
                        messagex = "FamilyInstance is a column.";
                        break;
                    case StructuralType.Footing: // 独立地基
                        messagex = "FamilyInstance is a footing.";
                        break;
                    default:
                        messagex = "FamilyInstance is non-structural or unknown framing.";
                        break;
                }
            }
            return beamList;

        }
        //============代码片段6-1：几何============
        public Options GetGeometryOption()
        {
            Autodesk.Revit.DB.Options option = this.application.Create.NewGeometryOptions();
            option.ComputeReferences = true;   //打开计算几何引用
            option.DetailLevel = ViewDetailLevel.Fine;   //视图详细程度为最好
            return option;
        }

        //============代码片段6-2：几何============
        public void GetWallGeometry()
        {
            Document doc = this.ActiveUIDocument.Document;
            Wall aWall = doc.GetElement(new ElementId(186388)) as Wall;

            Options option = GetGeometryOption();  // 创建几何选项
            Autodesk.Revit.DB.GeometryElement geomElement = aWall.get_Geometry(option);
            foreach (GeometryObject geomObj in geomElement)
            {
                Solid geomSolid = geomObj as Solid;
                if (null != geomSolid)
                {
                    foreach (Face geomFace in geomSolid.Faces)
                    {
                        // 得到墙的面
                    }
                    foreach (Edge geomEdge in geomSolid.Edges)
                    {
                        // 得到墙的边
                    }
                }
            }
        }
        public void GetBeamGeometry()
        {
            Document doc = this.ActiveUIDocument.Document;
            Wall aWall = doc.GetElement(new ElementId(186388)) as Wall;

            Options option = GetGeometryOption();  // 创建几何选项
            Autodesk.Revit.DB.GeometryElement geomElement = aWall.get_Geometry(option);
            foreach (GeometryObject geomObj in geomElement)
            {
                Solid geomSolid = geomObj as Solid;
                if (null != geomSolid)
                {
                    foreach (Face geomFace in geomSolid.Faces)
                    {
                        // 得到墙的面
                    }
                    foreach (Edge geomEdge in geomSolid.Edges)
                    {
                        // 得到墙的边
                    }
                }
            }
        }
    }

}  
   
