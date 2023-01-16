using Autodesk.Revit.DB;
using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Entities
{
    public class SolidComparer : IGeometryObjectComparer
    {
        private readonly double compareEps = 1e-3;
        public GeomAndCoordsCompareResult Compare(object x, object y)
        {
            if (x == null || y == null)
                throw new CompareTypeException("One of rhe compare objects is null");
            if (x is not Solid || y is not Solid)
                throw new CompareTypeException($"In SolidComparer args is not solids: " +
                    $"x type = {x.GetType().Name}, y type = {y.GetType().Name}.");
            else
            {
                try
                {
                    Solid xSolid = x as Solid;
                    Solid ySolid = y as Solid;
                    BoundingBoxXYZ xBound = xSolid.GetBoundingBox();
                    BoundingBoxXYZ yBound = ySolid.GetBoundingBox();

                    XYZ rootCenter = (xBound.Max + xBound.Min) / 2;
                    XYZ compareCenter = (yBound.Max + yBound.Min) / 2;
                    bool isCoordsEqual = rootCenter.EqualsTo(compareCenter, 1e-7);

                    bool isGeomEquals = xSolid.Volume - ySolid.Volume <= compareEps
                        //&& xBound.Min.EqualsTo(yBound.Min, 1e-7) && xBound.Max.EqualsTo(yBound.Max, 1e-7)
                        && xBound.Transform.AlmostEqual(yBound.Transform);

                    if (isGeomEquals && isCoordsEqual)
                        return GeomAndCoordsCompareResult.Match;
                    else if (isGeomEquals && !isCoordsEqual)
                        return GeomAndCoordsCompareResult.GeometryEqualButCoordsNot;
                    else if (!isGeomEquals && isCoordsEqual)
                        return GeomAndCoordsCompareResult.CoordsEqualButGeometryNot;
                    else return GeomAndCoordsCompareResult.NotFoundEqual;
                }
                catch (Exception ex)
                {
                    throw new CompareGeometryException("", ex);
                }

            }
        }
    }
}
