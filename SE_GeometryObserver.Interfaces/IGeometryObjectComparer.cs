using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Interfaces
{
    public interface IGeometryObjectComparer
    {
        public GeomAndCoordsCompareResult Compare(object x, object y);
    }
}
