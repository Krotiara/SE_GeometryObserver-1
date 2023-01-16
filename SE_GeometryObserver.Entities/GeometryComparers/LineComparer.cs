using Autodesk.Revit.DB;
using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Entities
{
    public class LineComparer : IGeometryObjectComparer
    {
        public GeomAndCoordsCompareResult Compare(object x, object y)
        {
            if (x == null || y == null)
                throw new CompareTypeException("One of rhe compare objects is null");
            if (x is not Line || y is not Line)
                throw new CompareTypeException($"In LineComparer args is not lines: " +
                    $"x type = {x.GetType().Name}, y type = {y.GetType().Name}.");
            else
            {
                try
                {
                    Line xLine = x as Line;
                    Line yLine = y as Line;

                    bool isCoordsEqual = xLine.Origin.EqualsTo(yLine.Origin, 1e-7);
                    bool isGeomEquals = xLine.Direction.IsAlmostEqualTo(yLine.Direction);

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
