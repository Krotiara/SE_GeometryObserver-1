using Autodesk.Revit.DB;
using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Entities
{
    public class CommonCompareResults : ICommonCompareResults<GeometryCompareResult>
    {
        public CommonCompareResults(string docName, string fileDocName, IEnumerable<Category> cats, IList<GeometryCompareResult> res)
        {
            DocumentName = docName;
            FileDocumentName = fileDocName;
            BuiltInCategories = cats.Select(x => (BuiltInCategory)x.Id.IntegerValue).ToList();
            CategoriesDescription = string.Join(",", cats.Select(x => x.Name));
            CompareResults = res;
        }

        public string DocumentName { get; set; }
        public string FileDocumentName { get; set; }
        public IList<BuiltInCategory> BuiltInCategories { get; set; }
        public IList<GeometryCompareResult> CompareResults { get; set; }

        public string CategoriesDescription { get; }
    }
}
