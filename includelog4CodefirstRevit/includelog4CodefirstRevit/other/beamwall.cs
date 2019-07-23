namespace includelog4CodefirstRevit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Autodesk.Revit.UI;
    using Autodesk.Revit.DB;

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class beamwall : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document RevitDoc = commandData.Application.ActiveUIDocument.Document;
            //============代码片段3-2 过滤所有外墙============
            FilteredElementCollector filteredElements = new FilteredElementCollector(RevitDoc);
            ElementClassFilter classFilter = new ElementClassFilter(typeof(Wall));
            filteredElements = filteredElements.WherePasses(classFilter);

            Wall wa = filteredElements.FirstOrDefault() as Wall;
            Parameter wallpare = wa.LookupParameter("顶部延伸距离");
            //也可以采用wa.getPara方式来获取参数
            Transaction trans = new Transaction(RevitDoc, "修改参数");
            trans.Start();
            if (!wallpare.IsReadOnly)
                wallpare.Set(10000);// lvl 为要设置的标高



            //============ 代码片段3 - 16：元素编辑 ============
            // Wall wall = element as Wall;
            if (null != wa)
            {
                //下面的代码已经能成功的运行，为了方便后面的调试，把他先注释掉
                LocationCurve wallLine = wa.Location as LocationCurve;
                XYZ newPlace = new XYZ(-10, -20, 0);
                //wallLine.Move(newPlace);
                Line wline = wallLine.Curve as Line;
                XYZ startpoint = wline.GetEndPoint(0);
                XYZ endpoint = wline.GetEndPoint(1);

                XYZ midpoint = (wline.GetEndPoint(0) + wline.GetEndPoint(1)) / 2;
                System.Diagnostics.Trace.WriteLine("XYZ startpoint： " + startpoint.ToString());
                System.Diagnostics.Trace.WriteLine("XYZ endpoint： " + endpoint.ToString());
                System.Diagnostics.Trace.WriteLine("XYZ midpoint： " + midpoint.ToString());
            

            FamilyInstance beam = RevitDoc.GetElement(new ElementId(461762)) as FamilyInstance;

            LocationCurve lc = beam.Location as LocationCurve;
            Line beamline = lc.Curve as Line;

                double distance = Convert.ToDouble(beamline.GetEndPoint(1).Z.ToString()) - Convert.ToDouble(wline.GetEndPoint(1).Z.ToString());
                System.Diagnostics.Trace.WriteLine("distance： " + distance);

                //Wall wa = filteredElements.FirstOrDefault() as Wall;
                Parameter wallpare1 = wa.get_Parameter(BuiltInParameter.WALL_TOP_OFFSET);
                Parameter wall无连接高度 = wa.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM);

                double startheigh = wallpare1.AsDouble();
                //也可以采用wa.getPara方式来获取参数
                //Transaction trans = new Transaction(RevitDoc, "修改参数");
                if (!wallpare1.IsReadOnly)
                    wallpare1.Set(wall无连接高度.AsDouble()+distance);

            }
            trans.Commit();

            return Result.Succeeded;
        }
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
    #region Header
    //
    // CmdParamValuesForCats.cs - retrieve all parameter values for all elements of the given categories
    //
    // Copyright (C) 2018 Jeremy Tammik, Autodesk Inc. All rights reserved.
    //
    // Keywords: The Building Coder Revit API C# .NET add-in.
    //
    #endregion // Header
}


namespace BuildingCoder
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

            public Result Execute(ExternalCommandData commandData,ref string message,ElementSet elements)
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

    

