using Autodesk.Revit.DB;
using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Entities
{
    public class GeometryInstanceComparer : IGeometryObjectComparer
    {
        public GeomAndCoordsCompareResult Compare(object x, object y)
        {
            if (x == null || y == null)
                throw new CompareTypeException("One of rhe compare objects is null");
            if (x is not GeometryInstance || y is not GeometryInstance)
                throw new CompareTypeException($"In GeometryInstance args is not geometry Instances: " +
                    $"x type = {x.GetType().Name}, y type = {y.GetType().Name}.");
            else
            {
                try
                {
                    GeometryInstance xInst = x as GeometryInstance;
                    GeometryInstance yInst = y as GeometryInstance;

                    BoundingBoxXYZ xBound = xInst.GetInstanceGeometry().GetBoundingBox();
                    BoundingBoxXYZ yBound = yInst.GetInstanceGeometry().GetBoundingBox();

                    XYZ rootCenter = (xBound.Max + xBound.Min) / 2;
                    XYZ compareCenter = (yBound.Max + yBound.Min) / 2;
                    bool isCoordsEqual = rootCenter.EqualsTo(compareCenter, 1e-7);
                    bool isGeomEquals = xInst.Transform.AlmostEqual(yInst.Transform);

                    if (isGeomEquals && isCoordsEqual)
                        return GeomAndCoordsCompareResult.Match;
                    else if (isGeomEquals && !isCoordsEqual)
                        return GeomAndCoordsCompareResult.GeometryEqualButCoordsNot;
                    else if (!isGeomEquals && isCoordsEqual)
                        return GeomAndCoordsCompareResult.CoordsEqualButGeometryNot;
                    else return GeomAndCoordsCompareResult.NotFoundEqual;
                }
                catch(Exception ex)
                {
                    throw new CompareGeometryException("", ex);
                }
            }

        }
    }
}
