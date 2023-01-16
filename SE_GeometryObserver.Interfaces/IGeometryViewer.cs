using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Interfaces
{
    public interface IGeometryViewer
    {

        public Task ViewGeometryCompareResult(IGeometryCompareResult geometryCompareResult, bool isIsolate);

        public Task ClearGeometryCompareResultsOnViewAsync();

    }
}
