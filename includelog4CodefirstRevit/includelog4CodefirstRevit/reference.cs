//阿里云  >  教程中心   >  net教程  >  Revit API射线法读取空间中相交的元素
//Revit API射线法读取空间中相交的元素
//发布时间：2018-03-01 来源：网络 上传者：用户

//关键字: 空间 元素

//发表文章
//摘要：RevitAPI提供根据射线来寻找经过的元素。方法是固定模式,没什么好说。关键代码:doc.FindReferencesWithContextByDirection(ptStart, (ptEnd - ptStart), view3d)//射线法寻找穿过的对象[TransactionAttribute(Autodesk.Revit.Attributes.TransactionMode.Manual)]public class&
//Revit API提供根据射线来寻找经过的元素。方法是固定模式,没什么好说。 
//关键代码:doc.FindReferencesWithContextByDirection(ptStart, (ptEnd - ptStart), view3d)//射线法寻找穿过的对象 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Mechanical;

[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
public class FindSupporting : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
    {

        UIApplication app = commandData.Application;
        Document doc = app.ActiveUIDocument.Document;
        Transaction trans = new Transaction(doc, "ExComm");
        trans.Start();

        Selection sel = app.ActiveUIDocument.Selection;
        //Reference ref1 = sel.PickObject(ObjectType.Element, "Please pick a beam"); 
        //FamilyInstance beam = doc.GetElement(ref1) as FamilyInstance; 
        Reference ref1 = sel.PickObject(ObjectType.Element, "Please pick a duct");
        Duct duct = doc.GetElement(ref1) as Duct;

        //Read the beam's location line 
        //LocationCurve lc = beam.Location as LocationCurve; 
        LocationCurve lc = duct.Location as LocationCurve;
        Curve curve = lc.Curve;
        //取得线端点的方法 
        XYZ ptStart = curve.GetEndPoint(0);
        XYZ ptEnd = curve.GetEndPoint(1);

        //move the two point a little bit lower, so the ray can go through the wall 
        XYZ offset = new XYZ(0, 0, 0.01);//向量偏移的方法,这里向下偏移。 
        ptStart = ptStart - offset;
        ptEnd = ptEnd - offset;

        View3D view3d = null;
        view3d = doc.ActiveView as View3D;
        if (view3d == null)
        {
            TaskDialog.Show("3D view", "current view should be 3D view");
            return Result.Failed;
        }

        double beamLen = curve.Length;
        //终点-起点就是线的方向。这里是射线法的关键代码。必须在三维视图下。 
        IList<ReferenceWithContext> references = doc.FindReferencesWithContextByDirection(ptStart, (ptEnd - ptStart), view3d);
        

        //ElementSet wallSet = app.Application.Create.NewElementSet(); 

        sel.Elements.Clear();
        double tolerate = 0.00001;
        foreach (ReferenceWithContext reference in references)
        {
            Reference ref2 = reference.GetReference();//取得引用 
            ElementId id = ref2.ElementId;
            Element elem = doc.GetElement(id);

            if (elem is Wall)
            {
                if (reference.Proximity < (beamLen + tolerate))//Proximity接近,即与射线原点的距离。 
                {
                    sel.Elements.Add(elem);
                }
            }
        }


        trans.Commit();

        return Result.Succeeded;
    } 