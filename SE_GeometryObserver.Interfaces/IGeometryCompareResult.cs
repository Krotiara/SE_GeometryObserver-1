using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Interfaces
{
    public interface IGeometryCompareResult
    {
        public int Id { get; set; }

        public string CurrentElementName { get; }

        public string CurrentElementId { get;} 

        public string FileElementName { get;  }

        public string FileElementId { get;  }


        public IGeometryElementModel FileGeometryModel { get; set; }

        public IGeometryElementModel CurrentGeometryModel { get; set; }

        public GeomAndCoordsCompareResult CompareResult { get; set; }

    }

}
