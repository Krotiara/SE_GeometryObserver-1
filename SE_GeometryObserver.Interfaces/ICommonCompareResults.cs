using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Interfaces
{
    public interface ICommonCompareResults<T> where T : IGeometryCompareResult
    {
        public string DocumentName { get; set; }

        public string FileDocumentName { get; set; }

        public IList<BuiltInCategory> BuiltInCategories { get; set; }

        public string CategoriesDescription { get; }

        public IList<T> CompareResults { get; set; }
    }
}
