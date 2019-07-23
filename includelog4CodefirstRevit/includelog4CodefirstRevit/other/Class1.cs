using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace HelloWorld
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    //此处的TransactionMode比如由Automatic改为Manual，不然在调试时会出现“revit无法运行外部程序”

    public class Class1 : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            //log4net.Config.XmlConfigurator.Configure();

            //log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)];
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.config"));
            includelog4CodefirstRevit.Logger.Setup();
            //log4net.LogManager.GetLogger() 然后是调用了Common.utility 的Logmanager

            TaskDialog.Show("Revit", "Hello Worldsjk1");
            Common.utility.WriteDebugLog(string.Format("文件数量总结为{0},目前处理的进度为{1},当前处理的文件名称为{2} \r\n", 1, 2, 3));
            Common.utility.WriteErrorLog(string.Format("文件数量总结为{0},目前处理的进度为{1},当前处理的文件名称为{2} \r\n", 1, 2, 3));
            includelog4CodefirstRevit.Model1 m = new includelog4CodefirstRevit.Model1();
            try
            {
                var x = m.employee.Count();
                Common.utility.WriteDebugLog(string.Format("数据库表中记录为{0}\r\n", x));
            }
            catch (Exception ex)
            {
                Common.utility.WriteErrorLog(ex.ToString());
            }
            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}

namespace includelog4CodefirstRevit
{
    public class Class1
    {
    }
}
