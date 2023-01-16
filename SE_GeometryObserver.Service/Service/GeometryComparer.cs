using Autodesk.Revit.DB;
using SE_GeometryObserver.Entities;
using SE_GeometryObserver.Interfaces;

namespace SE_GeometryObserver.Service.Service
{
    public class GeometryComparer : IGeometryComparer
    {
        readonly ICompareGeometryElementsService compareGeometryElementsService;

        public GeometryComparer(ICompareGeometryElementsService compareGeometryElementsService)
        {
            this.compareGeometryElementsService = compareGeometryElementsService;
        }


        //TODO add try catch and log
        public IList<IGeometryCompareResult> Compare(IList<IGeometryElementModel> rootList,
            IList<IGeometryElementModel> compareList)
        {
            try
            {
                IList<IGeometryCompareResult> result = new List<IGeometryCompareResult>();

                IList<bool> compareListUseMask = new bool[compareList.Count];
                var compareListWithIndexes = compareList.Select((v, i) => new { value = v, index = i });
                foreach (GeometryElementModel model in rootList)
                {
                    try
                    {
                        var compareModelPair = compareListWithIndexes.FirstOrDefault(x => x.value.ElementId == model.ElementId);

                        if (compareModelPair == null)
                            result.Add(new GeometryCompareResult()
                            {
                                FileGeometryModel = null,
                                CurrentGeometryModel = model,
                                CompareResult = GeomAndCoordsCompareResult.NotFoundEqual
                            });
                        else
                        {
                            IGeometryElementModel compareModel = compareModelPair.value;
                            compareListUseMask[compareModelPair.index] = true;
                            GeomAndCoordsCompareResult type =
                                compareGeometryElementsService.CompareGeometryBetween(model, compareModel);
                            if (type == GeomAndCoordsCompareResult.NotFoundEqual)
                                result.Add(new GeometryCompareResult()
                                {
                                    FileGeometryModel = null,
                                    CurrentGeometryModel = model,
                                    CompareResult = type
                                });
                            else
                                result.Add(new GeometryCompareResult()
                                {
                                    FileGeometryModel = compareModel,
                                    CurrentGeometryModel = model,
                                    CompareResult = type
                                });
                        }
                    }
                    catch (CompareGeometryException ex)
                    {
                        //TODO log
                        continue;
                    }
                    catch (Exception ex)
                    {
                        //TODO log
#warning TODO обработка ошибок
                        throw new NotImplementedException(); //unexpected error
                    }
                }

                IEnumerable<IGeometryElementModel> notFoundedElements = compareList
                    .Zip(compareListUseMask, (e, m) => new { element = e, isMask = m })
                    .Where(x => !x.isMask)
                    .Select(x => x.element);

                foreach (GeometryElementModel model in notFoundedElements)
                    result.Add(new GeometryCompareResult()
                    {
                        FileGeometryModel = model,
                        CurrentGeometryModel = null,
                        CompareResult = GeomAndCoordsCompareResult.NotFoundEqual
                    });

                return result;
            }
            catch(Exception ex)
            {
#warning TODO обработка ошибок
                throw new NotImplementedException(); //unexpected error
            }
        }
    }
}
