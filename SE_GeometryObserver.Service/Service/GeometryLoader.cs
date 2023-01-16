using Autodesk.Revit.DB;
using SE_GeometryObserver.Entities;
using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Service
{
    public class GeometryLoader : IGeometryLoader
    {
        public IList<IGeometryElementModel> LoadGeometries(Document document, IEnumerable<BuiltInCategory> categories = null, Transform geomTran = null)
        {
            
            IList<IGeometryElementModel> result = new List<IGeometryElementModel>();

            FilteredElementCollector finalCollector = new FilteredElementCollector(document);
            IEnumerable<Element> elements;
            if (categories != null)
            {
                ElementMulticategoryFilter catsFilter = new ElementMulticategoryFilter(categories.ToList());
                elements = finalCollector.WherePasses(catsFilter).ToElements();
            }
            else
                elements = finalCollector.WhereElementIsNotElementType().
                    Where(x=>x.GetTypeId() != null); //TODO может нужно не брать все, а как-то сократить?
            foreach (Element element in elements)
            {
                GeometryElement gE = element.get_Geometry(new Options() {DetailLevel = ViewDetailLevel.Fine});
                if (gE == null || IsEmptyGeometry(gE))
                    continue;
                if (geomTran != null)
                    gE = gE.GetTransformed(geomTran);
                result.Add(new GeometryElementModel()
                {
                    ElementName = element.Name,
                    ElementId = element.Id,
                    DocumentName = document.PathName,
                    GeometryElement = gE
                });
            }

            return result;
        }


        private bool IsEmptyGeometry(GeometryElement gE)
        {
            foreach(GeometryObject geometryObject in gE)
            {
                if (geometryObject is Solid && (geometryObject as Solid).Volume != 0)
                    return false;
                if (geometryObject is Line && (geometryObject as Line).Length != 0)
                    return false;
                if (geometryObject is GeometryInstance)
                    return false;
            }
            return true;
        }
    }
}
