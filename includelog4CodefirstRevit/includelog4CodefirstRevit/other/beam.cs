using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace includelog4CodefirstRevit
{
    public  class GetProfileOfBeam
    {
        public double Heigth { get; set; }
        public double Width { get; set; }
        public GetProfileOfBeam(FamilyInstance familyInstance)
        {
            //求出梁的基线的方向
            Line line = ((familyInstance.Location) as LocationCurve).Curve as Line;
            XYZ dir = line.Direction;
            //根据梁的几何信息，得到solid,face
            GeometryElement geometryElement = familyInstance.get_Geometry(new Options());
            foreach (GeometryObject geoOb in geometryElement)
            {
                GeometryInstance gIn = geoOb as GeometryInstance;
                if (gIn != null)
                {
                    GeometryElement ge = gIn.GetInstanceGeometry();
                    foreach (GeometryObject go in ge)
                    {
                        Solid solid = go as Solid;
                        if (solid != null && solid.Volume > 0)
                        {
                            foreach (Face face in solid.Faces)
                            {
                                XYZ faceNormal = face.ComputeNormal(new UV());
                                if (faceNormal.IsAlmostEqualTo(dir) || faceNormal.IsAlmostEqualTo(-dir))
                                {
                                    BoundingBoxUV uvBox = face.GetBoundingBox();
                                    XYZ min = face.Evaluate(uvBox.Min);
                                    XYZ max = face.Evaluate(uvBox.Max);
                                    Heigth = Math.Abs(max.Z - min.Z) * 304.8;
                                    Heigth = Math.Round(Heigth);
                                    //h好求，但是宽度b不一定是x还是y，所以用了下面的方式
                                    double l = max.DistanceTo(min) * 304.8;
                                    Width = Math.Sqrt(l * l - Heigth * Heigth);
                                    Width = Math.Round(Width);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}
