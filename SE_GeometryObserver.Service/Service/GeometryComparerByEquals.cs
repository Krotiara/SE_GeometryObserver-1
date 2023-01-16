using SE_GeometryObserver.Entities;
using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Service.Service
{
    public class GeometryComparerByEquals : IGeometryComparer
    {

        private readonly ICalculateNearestGeomModelsService calcNearestGeomModelsService;
        private readonly ICompareGeometryElementsService compareGeometryElementsService;

        public GeometryComparerByEquals(ICalculateNearestGeomModelsService calcNearestGeomModelsService, ICompareGeometryElementsService compareGeometryElementsService) 
        {
            this.calcNearestGeomModelsService = calcNearestGeomModelsService;
            this.compareGeometryElementsService = compareGeometryElementsService;
        }

        public IList<IGeometryCompareResult> Compare(IList<IGeometryElementModel> rootList, IList<IGeometryElementModel> compareList)
        {
            IList<IGeometryCompareResult> result = new List<IGeometryCompareResult>();
            foreach (IGeometryElementModel model in rootList)
            {
                IList<IGeometryElementModel> nearestModels = 
                    calcNearestGeomModelsService.GetNearest(model, compareList);

                List<GeomAndCoordsCompareResult> nearestCompareResults = new List<GeomAndCoordsCompareResult>();
                foreach(IGeometryElementModel compareModel in nearestModels)
                {
                    GeomAndCoordsCompareResult type = 
                        compareGeometryElementsService.CompareGeometryBetween(model, compareModel);
                    nearestCompareResults.Add(type);  
                }

                int matchIndex = nearestCompareResults.IndexOf(GeomAndCoordsCompareResult.Match);
                int coordEqualButGeomNotIndex = 
                    nearestCompareResults.IndexOf(GeomAndCoordsCompareResult.CoordsEqualButGeometryNot);
                int geomEqualsButCoordsNotIndex = nearestCompareResults.IndexOf(GeomAndCoordsCompareResult.GeometryEqualButCoordsNot);
                
                if(matchIndex != -1)
                    result.Add(new GeometryCompareResult()
                    {
                        FileGeometryModel = nearestModels[matchIndex],
                        CurrentGeometryModel = model,
                        CompareResult = nearestCompareResults[matchIndex]
                    });
                else if(coordEqualButGeomNotIndex != -1)
                    result.Add(new GeometryCompareResult()
                    {
                        FileGeometryModel = nearestModels[coordEqualButGeomNotIndex],
                        CurrentGeometryModel = model,
                        CompareResult = nearestCompareResults[coordEqualButGeomNotIndex]
                    });
                else if (geomEqualsButCoordsNotIndex != -1)
                    result.Add(new GeometryCompareResult()
                    {
                        FileGeometryModel = nearestModels[geomEqualsButCoordsNotIndex],
                        CurrentGeometryModel = model,
                        CompareResult = nearestCompareResults[geomEqualsButCoordsNotIndex]
                    });
                else
                    result.Add(new GeometryCompareResult()
                    {
                        FileGeometryModel = null,
                        CurrentGeometryModel = model,
                        CompareResult = GeomAndCoordsCompareResult.NotFoundEqual
                    });
            }

            int curId = 1;
            foreach (GeometryCompareResult compareResult in result)
                if (compareResult.CompareResult != GeomAndCoordsCompareResult.Match)
                    compareResult.Id = curId++;

            return result;
        }
    }
}
