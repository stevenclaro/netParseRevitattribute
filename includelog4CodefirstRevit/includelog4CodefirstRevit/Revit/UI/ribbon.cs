using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using includelog4CodefirstRevit;
using includelog4CodefirstRevit.Revit.Model.Dbmodel;

namespace includelog4CodefirstRevit.Revit.UI
{
    public class CICDIAddpanel : Autodesk.Revit.UI.IExternalApplication
    { 
    public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
    {
        //添加一个新的Ribbon面板
        RibbonPanel ribbonPanel = application.CreateRibbonPanel("BIM事业部");

        //在新的Ribbon面板上添加一个按钮
        //点击这个按钮，调用本章第四节第一个实例。
        PushButton pushButton = ribbonPanel.AddItem(new PushButtonData("质量检查",
            "质量检查", @"E:\zljc\includelog4CodefirstRevit\includelog4CodefirstRevit\bin\Debug\includelog4CodefirstRevit.dll", "HelloWorld.Cicdiinstance")) as PushButton;
        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        //UI定制不需要特别在OnShutdown方法中做处理。
        return Result.Succeeded;
    }
    }
}
