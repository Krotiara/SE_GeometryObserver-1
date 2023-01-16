using Autodesk.Revit.DB;
using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Service.Service
{
    public class CalculateNearestGeomModelsService : ICalculateNearestGeomModelsService
    {

        private readonly int nearestGetCount = 20;

        public IList<IGeometryElementModel> GetNearest(IGeometryElementModel model, 
            IList<IGeometryElementModel> source)
        {
            return source
                .Select(x => new { model = x, distance = GetDistanceBetween(model, x) })
                .OrderBy(x => x.distance)
                .Take(nearestGetCount)
                .Select(x => x.model)
                .ToList();        
        }


        private double GetDistanceBetween(IGeometryElementModel first, IGeometryElementModel second)
        {
            BoundingBoxXYZ firstBB = first.GeometryElement.GetBoundingBox();
            BoundingBoxXYZ secondBB = second.GeometryElement.GetBoundingBox();

            XYZ firstCenter = firstBB.Max - firstBB.Min;
            XYZ secondCenter = secondBB.Max - secondBB.Min;
            double dist = firstCenter.DistanceTo(secondCenter);

            return dist;
        }
    }
}
