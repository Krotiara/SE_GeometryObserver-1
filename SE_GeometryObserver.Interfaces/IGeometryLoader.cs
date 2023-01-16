using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Interfaces
{
    public interface IGeometryLoader
    {
        public IList<IGeometryElementModel> LoadGeometries(Document document, IEnumerable<BuiltInCategory> categories = null, Transform geomTran = null);
    }
}
