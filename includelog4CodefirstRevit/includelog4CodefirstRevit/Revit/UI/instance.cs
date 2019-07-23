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
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    //此处的TransactionMode比如由Automatic改为Manual，不然在调试时会出现“revit无法运行外部程序”

    public class Cicdiinstance : IExternalCommand
    {
        List<string> list1 = new List<string>();
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            includelog4CodefirstRevit.Logger.Setup();
            //includelog4CodefirstRevit.Model1 m = new includelog4CodefirstRevit.Model1();
            includelog4CodefirstRevit.ModelCodeFirst m = new includelog4CodefirstRevit.ModelCodeFirst();

            //Database.SetInitializer<Models.musicStoreContext>(new DropCreateDatabaseAlways<Models.musicStoreContext>());

            includelog4CodefirstRevit.ModelNameRuleDb mnrb = new ModelNameRuleDb();
            List<revitnamerule> rnr = mnrb.revitnamerule.ToList();

            Document doc = commandData.Application.ActiveUIDocument.Document;
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ElementClassFilter instanceFitler = new ElementClassFilter(typeof(FamilyInstance));
            ElementClassFilter hostFilter = new ElementClassFilter(typeof(HostObject));
            LogicalOrFilter andFilter = new LogicalOrFilter(instanceFitler, hostFilter);

           //sjk20181224 FamilyManager fa = doc.FamilyManager;

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

            string[] strs = System.Enum.GetNames(typeof(BuiltInParameter));
            List<ParaEnum> ParaEnumList = new List<ParaEnum>();
            int j = 0;
            foreach (string x in strs)
            {
                j++;
                ParaEnum pe = new ParaEnum();
                pe.guidid = Guid.NewGuid().ToString();
                pe.ParaEnumID = j.ToString();
                pe.ParaEnum名称 = x;

                ParaEnumList.Add(pe);
            }
            m.ParaEnums.AddRange(ParaEnumList);
              //  Common.utility.WriteErrorLog(string.Format("BuiltInParameter的参数，{0}\r\n", x));

            doorstep ds = new includelog4CodefirstRevit.doorstep();
            List<ParaModel> ParaModelList = new List<includelog4CodefirstRevit.Revit.Model.Dbmodel.ParaModel>();
            //取系统的参数
            for (int i = 0; i < CollectorList.Count; i++)
            {

                ParaModelList.AddRange(ds.parseSys(doc, CollectorList[i], rnr));

            }
            List<ParaModel> ParaModel用户自定义List = new List<ParaModel>(); ;
            //取用户自定义参数
            for (int i = 0; i < CollectorList.Count; i++)
            {
                // if(CollectorList[i].Id.ToString()=="462276")
                List<ParaModel> templist  = ds.parse用户自定义参数(doc, CollectorList[i], ParaModelList);
                ParaModel用户自定义List.AddRange(templist);
            }
            //取得所有的类型参数
            List<ParaModel> typelist=new List<ParaModel>();
            for (int i = 0; i < CollectorList.Count; i++)
            {
                List<ParaModel> xlist = ds.parse类型参数(doc, CollectorList[i], ParaModelList);
                typelist.AddRange(xlist);

            }
            //获得所有的类型参数
          //sjk20181224  FamilyType currentType = fa.CurrentType;

            m.ParaModels.AddRange(typelist);
            m.ParaModels.AddRange(ParaModelList);
            m.ParaModels.AddRange(ParaModel用户自定义List);
            //Common.utility.WriteDebugLog(string.Format("Revit文件中的族实例数量为{0} \r\n", fmlist.Count));
            m.SaveChanges();
            

            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }

}  
   
