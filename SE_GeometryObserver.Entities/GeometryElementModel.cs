using Autodesk.Revit.DB;
using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Entities
{
    public class GeometryElementModel: IGeometryElementModel
    {
        public ElementId ElementId { get; set; }

        public string ElementName { get; set; }

        public string DocumentName { get; set; }

        public GeometryElement GeometryElement { get; set; }


        public IList<GeometryObject> GeometryObjects
        {
            get
            {
                IList<GeometryObject> list = new List<GeometryObject>();
                foreach (GeometryObject obj in GeometryElement)
                    list.Add(obj);
                return list;
            }
        }
    }
}
