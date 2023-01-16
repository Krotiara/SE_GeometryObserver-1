using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Interfaces
{
    public interface IGeometryElementModel
    {
        public ElementId ElementId { get; set; }

        public string ElementName { get; set; }

        public string DocumentName { get; set; }

        public GeometryElement GeometryElement { get; set; }

        public IList<GeometryObject> GeometryObjects { get; }
    }
}
