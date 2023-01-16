using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Entities
{
    public static class Extensions
    {

        public static bool EqualsTo(this XYZ p1, XYZ p2, double tolerance)
        {
            return (Math.Abs(p1.X - p2.X) <= tolerance 
                && Math.Abs(p1.Y - p2.Y) <= tolerance 
                && Math.Abs(p1.Z - p2.Z) <= tolerance);
        }

    }
}
