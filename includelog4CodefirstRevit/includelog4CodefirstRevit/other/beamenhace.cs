namespace includelog4CodefirstRevitEnhance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Autodesk.Revit.UI;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Structure;

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class beamwaenhance : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document RevitDoc = commandData.Application.ActiveUIDocument.Document;

            //log4net.LogManager.GetLogger() 然后是调用了Common.utility 的Logmanager

            includelog4CodefirstRevit.Logger.Setup();


            var pipes = new FilteredElementCollector(RevitDoc).OfClass(typeof(Autodesk.Revit.DB.Plumbing.Pipe)).Cast<Autodesk.Revit.DB.Plumbing.Pipe>().ToList();

            //得到水管
            ////============代码片段3-2 过滤所有外墙============
            //// 获取风管类型
            //var ductTypeFilter = new ElementClassFilter(typeof(Autodesk.Revit.DB.Mechanical.Duct));
            //FilteredElementCollector ductTypes = new FilteredElementCollector(RevitDoc);
            //var result = ductTypes.WherePasses(ductTypeFilter).ToList();
            //foreach (DuctType element in result)
            //{
            //    ductTypeId = element.Id;
            //    break;
            //}

            //============代码片段3-2 过滤所有外墙============
            FilteredElementCollector filteredElements = new FilteredElementCollector(RevitDoc);
            ElementClassFilter classFilter = new ElementClassFilter(typeof(Wall));
            filteredElements = filteredElements.WherePasses(classFilter);

            Wall wa = filteredElements.FirstOrDefault() as Wall;

            //也可以采用wa.getPara方式来获取参数
            Transaction trans = new Transaction(RevitDoc, "修改参数");
            trans.Start();

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
                        compute(each, wa);
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
            //FamilyInstance beam = RevitDoc.GetElement(new ElementId(461762)) as FamilyInstance;

            // LocationCurve lc = beam.Location as LocationCurve;
            //Line beamline = lc.Curve as Line;

            trans.Commit();

            return Result.Succeeded;
        }
        private void compute(FamilyInstance beam, Wall wa)
        {
            LocationCurve lc = beam.Location as LocationCurve;
            Line beamline = lc.Curve as Line;

            LocationCurve wallLine = wa.Location as LocationCurve;
            Line wline = wallLine.Curve as Line;
            XYZ startpoint = wline.GetEndPoint(0);
            XYZ endpoint = wline.GetEndPoint(1);

            Parameter wallpare1 = wa.get_Parameter(BuiltInParameter.WALL_TOP_OFFSET);
            Parameter wall无连接高度 = wa.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM);


            double distance = Convert.ToDouble(beamline.GetEndPoint(1).Z.ToString()) - Convert.ToDouble(wline.GetEndPoint(1).Z.ToString());
            includelog4CodefirstRevit.GetProfileOfBeam get = new includelog4CodefirstRevit.GetProfileOfBeam(beam);

            double beam一半的高度 = Convert.ToDouble(get.Heigth.ToString());
            distance = distance - beam一半的高度;
            double wall无连接高度一半高度 = wall无连接高度.AsDouble() / 2;
            distance = distance - wall无连接高度一半高度;

            //double wall墙最终的高度 = wall无连接高度.AsDouble() + distance;
            double wall墙最终的高度 = Convert.ToDouble( wall无连接高度.AsValueString()) + distance;
            Parameter walloffset = wa.get_Parameter(BuiltInParameter.WALL_TOP_OFFSET);
            if (!walloffset.IsReadOnly)
                walloffset.Set(3);//故意设置的高度为绝对3，但是这个计算值为什么是3 还需要重新从程序中计算得到

            System.Diagnostics.Trace.WriteLine("wall墙最终的高度： " + wall墙最终的高度);

        }
        private void findNearestLine()
        { }
        private void xx(Document RevitDoc)
        {
            // 首先找到线形的墙
            ElementFilter wallFilter = new ElementClassFilter(typeof(Wall));
            FilteredElementCollector filteredElements = new FilteredElementCollector(RevitDoc);
            filteredElements = filteredElements.WherePasses(wallFilter);
            Wall wall = null;
            Line line = null;
            foreach (Wall element in filteredElements)
            {
                LocationCurve locationCurve = element.Location as LocationCurve;
                if (locationCurve != null)
                {
                    line = locationCurve.Curve as Line;
                    if (line != null)
                    {
                        wall = element;
                        break;
                    }
                }
            }
        }
        //private void te()
        //{
        //    var beamBottomFaces = FaceUtils.GetBottomFaces(beam); //这个方法是自己封装的
        //    if (null != beamBottomFaces && beamBottomFaces.Any())
        //    {
        //        var beamLocationCurve = beam.Location as LocationCurve;
        //        var beamCurve = beamLocationCurve.Curve;
        //        if (beamCurve != null)
        //        {
        //            if (beamCurve is Line)
        //            {
        //                beamCurve = GetExtLocationCurve(beamCurve); //如果LocationCurve是Line，最好做个延伸算法来延长，不然有些梁因为扣减的话，locationCurve容易缺少一部分
        //            }
        //            var beamPoints = GetPoints(beamCurve, pointRange); //pointRange是取点间隔
        //            if (beamPoints != null && beamPoints.Any())
        //            {
        //                var floorDataList = GetBeamFloorsPairCore(beamBottomFaces, beamPoints, floors); //floors为建筑板                               
        //            }
        //        }

        //    }
        //}

        ////LocationCurve的延伸：
        ////
        //private Curve GetExtLocationCurve(Curve curve)
        //{
        //    XYZ dir0 = XYZ.Zero;
        //    XYZ dir1 = XYZ.Zero;
        //    if (curve is Line)
        //    {
        //        dir0 = (curve as Line).Direction.Negate();
        //        dir1 = (curve as Line).Direction;
        //    }
        //    Curve extCurve = Line.CreateBound(curve.GetEndPoint(0) + 1E3 * dir0, curve.GetEndPoint(1) + 1E3 * dir1);
        //    return extCurve;
        //}//在Curve上按PointRange选取点位：

        //private List<XYZ> GetPoints(Curve curve, double pointRange)
        //{
        //    var points = new List<XYZ>();
        //    var beamLength = curve.Length;
        //    var pointsNumber = beamLength % pointRange == 0 ? ((beamLength / pointRange) - 1) : Math.Floor((beamLength / pointRange));

        //    for (var i = 1; i <= pointsNumber; i++)
        //    {
        //        var point = curve.Evaluate(pointRange * i, false);
        //        points.Add(point);
        //    }
        //    return points;
        //}
        ////获取距离：

        //private List<KeyValuePair<Element, List<KeyValuePair<XYZ, double>>>> GetBeamFloorsPairCore(List<PlanarFace> beamBottomFaces, IEnumerable<XYZ> beamPoints, List<Element> floors)
        //{
        //    var floorDataList = new List<KeyValuePair<Element, List<KeyValuePair<XYZ, double>>>>();
        //    //寻找每一块结构梁下的板
        //    foreach (var floor in constructionFloors)
        //    {
        //        //获取该板的最上点坐标
        //        var floorTopFaces = FaceUtils.GetTopFaces(floor);
        //        if (null != floorTopFaces && floorTopFaces.Any())
        //        {
        //            var defaultFloorOriginZ = floorTopFaces.FirstOrDefault().Origin.Z;
        //            foreach (var tf in floorTopFaces)
        //            {
        //                var originZ = tf.Origin.Z;
        //                if (defaultFloorOriginZ <= originZ)
        //                {
        //                    defaultFloorOriginZ = originZ;
        //                }
        //            }


        //            var defaultBeamOriginZ = beamBottomFaces.FirstOrDefault().Origin.Z;
        //            foreach (var bf in beamBottomFaces)
        //            {
        //                var originZ = bf.Origin.Z;
        //                if (defaultBeamOriginZ >= originZ)
        //                {
        //                    defaultBeamOriginZ = originZ;
        //                }
        //            }
        //            //板在梁下面
        //            var isLower = defaultFloorOriginZ < defaultBeamOriginZ;

        //            if (isLower)
        //            {
        //                var datalist = new List<KeyValuePair<XYZ, double>>();
        //                //梁上一点能投影到板上
        //                foreach (var point in beamPoints)
        //                {
        //                    foreach (var tf in floorTopFaces)
        //                    {
        //                        var isProject = tf.Project(point);
        //                        if (null != isProject)
        //                        {
        //                            //投影到板上点的坐标
        //                            var projectPoint = isProject.XYZPoint;

        //                            //投影点到梁上点的距离
        //                            foreach (var bf in beamBottomFaces)
        //                            {
        //                                var bp = bf.Project(projectPoint);
        //                                if (null != bp)
        //                                {
        //                                    var distance = bp.Distance;
        //                                    distance = UnitUtils.ConvertFromInternalUnits(distance, DisplayUnitType.DUT_MILLIMETERS);
        //                                    distance = Math.Floor(distance);

        //                                    var pointAndDistance = new KeyValuePair<XYZ, double>(projectPoint, distance);
        //                                    datalist.Add(pointAndDistance);
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                if (datalist != null && datalist.Any())
        //                {
        //                    var floorAndData = new KeyValuePair<Element, List<KeyValuePair<XYZ, double>>>(floor, datalist);
        //                    floorDataList.Add(floorAndData);
        //                }
        //            }
        //        }
        //    }
        //    return floorDataList;
        //}
    }
    //    #region Header
    //    //
    //    // CmdParamValuesForCats.cs - retrieve all parameter values for all elements of the given categories
    //    //
    //    // Copyright (C) 2018 Jeremy Tammik, Autodesk Inc. All rights reserved.
    //    //
    //    // Keywords: The Building Coder Revit API C# .NET add-in.
    //    //
    //    #endregion // Header
    //    public Autodesk.Revit.UI.Result testAngelline(ExternalCommandData commandData, ref string message, ElementSet elements)
    //    {

    //        Line line1 = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(10, 0, 0));
    //    Line line2 = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(0, 10, 0));    // L
    //    Line line3 = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(-10, 10, 0));  // \-
    //    Line line4 = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(-10, -10, 0)); // /-
    //    Line line5 = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(-10, 0, 0));   // --
    //    Line line6 = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(0, 10, 0));    // |-
    //    Line line7 = Line.CreateBound(new XYZ(0, 10, 0), new XYZ(10, 10, 0));  // =
    //    double angle;
    //            angle = line1.Direction.AngleTo(line2.Direction);
    //            System.Diagnostics.Trace.WriteLine(angle);
    //                angle = line1.Direction.AngleTo(line3.Direction);
    //                System.Diagnostics.Trace.WriteLine(angle);
    //                angle = line1.Direction.AngleTo(line4.Direction);
    //                System.Diagnostics.Trace.WriteLine(angle);
    //                angle = line1.Direction.AngleTo(line5.Direction);
    //                System.Diagnostics.Trace.WriteLine(angle);
    //                angle = line1.Direction.AngleTo(line6.Direction);
    //                System.Diagnostics.Trace.WriteLine(angle);
    //                angle = line1.Direction.AngleTo(line7.Direction);

    //        return Result.Succeeded;
    //}
}

    namespace BuildingCoderEnhance
    {
        #region Namespaces
        using Autodesk.Revit.Attributes;
        using Autodesk.Revit.DB;
        using Autodesk.Revit.UI;
        #endregion // Namespaces
        [Transaction(TransactionMode.ReadOnly)]
        class CmdParamValuesForCats : IExternalCommand
        {
            /// <summary>
            /// List all built-in categories of interest
            /// </summary>
            static BuiltInCategory[] _categories =
            {
              BuiltInCategory.OST_Doors,
              BuiltInCategory.OST_Rooms,
              BuiltInCategory.OST_Windows
            };

            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                #region Obsolete inline code
#if NEED_ALL_THE_INLINE_CODE
              // Parameter names are not guaranteed to be 
              // unique! Therefore, it may be impossible to
              // include all parameter values in a dictionary
              // using the parameter name as a key.

#if PARAMETER_NAMES_ARE_UNIQUE
                    Dictionary<string,Dictionary<string,Dictionary<string, string>>>  map_cat_to_uid_to_param_values;
#else // unfortunately, parameter names are not unique:
                    Dictionary<string,Dictionary<string,List<string>>> map_cat_to_uid_to_param_values;
#endif // PARAMETER_NAMES_ARE_UNIQUE

                map_cat_to_uid_to_param_values     = GetParamValuesForCats( doc, _cats );

#if DEBUG
            List<string> keys = new List<string>( map_cat_to_uid_to_param_values.Keys );
            keys.Sort();

              foreach( string key in keys )
              {
                Dictionary<string, List<string>> els 
                  = map_cat_to_uid_to_param_values[key];

                int n = els.Count;

                Debug.Print( "{0} ({1} element{2}){3}",
                  key, n, Util.PluralSuffix( n ), 
                  Util.DotOrColon( n ) );

                if( 0 < n )
                {
                  List<string> uids = new List<string>( els.Keys );
                  string uid = uids[0];

                  List<string> param_values = els[uid];
                  param_values.Sort();

                  n = param_values.Count;

                  Debug.Print( "  first element {0} has {1} parameter{2}{3}",
                    uid, n, Util.PluralSuffix( n ),
                    Util.DotOrColon( n ) );

                  param_values.ForEach( pv => Debug.Print( "    " + pv ) );
        }
      }
#endif // DEBUG
#endif
                #endregion // Obsolete inline code

                //  JtParamValuesForCats data  = new JtParamValuesForCats(doc, _categories);

#if DEBUG
                //  data.DebugPrint();
#endif // DEBUG

                return Result.Succeeded;
            }
        }
    }




