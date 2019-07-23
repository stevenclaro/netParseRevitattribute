using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using includelog4CodefirstRevit;
using Autodesk.Revit.DB.Structure;

namespace includelog4CodefirstRevit.Revit.UI
{
 public   class FloorUI
    {
        
        public PlanarFace getFloor的上表面(HostObject floor)
        {
            //============代码片段4-6 获取楼板的上表面============
            //Floor floor = GetElement<Floor>(185601);
            PlanarFace topFace = null;
            // 获取一个楼板面的引用
           
            IList<Reference> references = HostObjectUtils.GetTopFaces(floor);
        if (references.Count == 1)
        {
           var reference = references[0];

                // 从引用获取面的几何对象，这里是一个PlanarFace
                GeometryObject topFaceGeo = floor.GetGeometryObjectFromReference(reference);
                // 转型成我们想要的对象
                 topFace = topFaceGeo as PlanarFace;
                
          }

            return topFace;
        }
        public PlanarFace getFloor的下表面(Floor floor)
        {
            //============代码片段4-6 获取楼板的上表面============
            //Floor floor = GetElement<Floor>(185601);
            PlanarFace topFace = null;
            // 获取一个楼板面的引用

            IList<Reference> references = HostObjectUtils.GetBottomFaces(floor);
            if (references.Count == 1)
            {
                var reference = references[0];

                // 从引用获取面的几何对象，这里是一个PlanarFace
                GeometryObject topFaceGeo = floor.GetGeometryObjectFromReference(reference);
                // 转型成我们想要的对象
                topFace = topFaceGeo as PlanarFace;

            }

            return topFace;
        }
    }
    public static class face面
    {
        public static string top上表面 = "top上表面";
        public static string bottom下表面 = "bottom下表面";
        public static string side侧表面 = "side侧表面";
    }
}
