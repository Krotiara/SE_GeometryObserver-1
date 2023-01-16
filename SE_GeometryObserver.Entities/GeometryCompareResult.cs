using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Entities
{
    public class GeometryCompareResult : IGeometryCompareResult
    {
        public int Id { get; set; }

        public IGeometryElementModel FileGeometryModel { get; set; }
        public IGeometryElementModel CurrentGeometryModel { get; set; }
        public GeomAndCoordsCompareResult CompareResult { get; set; }

        public string CurrentElementName => CurrentGeometryModel == null? "" : 
            CurrentGeometryModel.ElementName;

        public string CurrentElementId => CurrentGeometryModel == null ? "" :
            CurrentGeometryModel.ElementId.IntegerValue.ToString();

        public string FileElementName => FileGeometryModel == null? "" :
            FileGeometryModel.ElementName;

        public string FileElementId => FileGeometryModel == null ? "" : 
            FileGeometryModel.ElementId.IntegerValue.ToString();
    }
}
