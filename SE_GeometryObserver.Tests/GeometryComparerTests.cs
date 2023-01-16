using Autodesk.Revit.DB;
using SE_GeometryObserver.Entities;
using SE_GeometryObserver.Interfaces;
using SE_GeometryObserver.Service;
using SE_GeometryObserver.Service.Service;
using Xunit;
using xUnitRevitUtils;

namespace SE_GeometryObserver.Tests
{
 
    public class GeometryComparerTests
    {
        private readonly string pathToFiles = Path.Combine(Directory
               .GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Files");

        public GeometryComparerTests()
        {

        }


        [Fact]
        public void CompareEqualDocGeometriesMustReturnMatchTest()
        {
            string docPath = Path.Combine(pathToFiles, "SingleRootDoc.rvt");
            string sourceDocPath = Path.Combine(pathToFiles, "SingleRootDoc.rvt");
            Document doc = xru.OpenDoc(docPath);
            Document sourceDoc = xru.OpenDoc(sourceDocPath);

            IGeometryLoader  geometryLoader = new GeometryLoader();
            IEnumerable<BuiltInCategory> cats = new List<BuiltInCategory> { BuiltInCategory.OST_Walls };
            IList<IGeometryElementModel> docGeometryElements = geometryLoader.LoadGeometries(doc, cats);
            IList<IGeometryElementModel> sourceDocGeometryElements = geometryLoader.LoadGeometries(sourceDoc,cats);

            IGeometryComparer comparer = new GeometryComparer(new CompareGeometryElementsService());
            IList< IGeometryCompareResult> ress =  comparer.Compare(docGeometryElements, sourceDocGeometryElements);
            foreach (IGeometryCompareResult res in ress)
                Assert.Equal(GeomAndCoordsCompareResult.Match, res.CompareResult);

            doc.Close(false);
            sourceDoc.Close(false);
        }

    }
}