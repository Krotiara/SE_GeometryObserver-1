using Autodesk.Revit.DB;
using SE_GeometryObserver.Entities;
using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Service.Service
{
    public class CompareGeometryElementsService : ICompareGeometryElementsService
    {
        public GeomAndCoordsCompareResult CompareGeometryBetween(IGeometryElementModel rootModel, IGeometryElementModel compareModel)
        {

            if (IsEmptyGeometry(rootModel.GeometryElement) || IsEmptyGeometry(compareModel.GeometryElement))
                throw new CompareGeometryException("empty geometry was passed into CompareGeometryBetween");
            try
            {                
                IList<GeometryObject> rootGeometry = rootModel.GeometryElement.Select(x => x).ToList();
                IList<GeometryObject> compareGeometry = compareModel.GeometryElement.Select(x => x).ToList();
         
                foreach (GeometryObject geometryObject in rootGeometry)
                {
                    IGeometryObjectComparer comparer = GetGeometryObjectComparer(geometryObject);
                    IList<GeometryObject> relativeGeometries = GetRelativeObjectsTo(geometryObject, compareGeometry);
                    IList<GeomAndCoordsCompareResult> relGeomsCompareResults = new List<GeomAndCoordsCompareResult>();

                    bool isFound = false;
                    foreach (GeometryObject relativeGeom in relativeGeometries)
                    {
                        try
                        {  
                            GeomAndCoordsCompareResult res = comparer.Compare(geometryObject, relativeGeom);
                            relGeomsCompareResults.Add(res);
                            if (res == GeomAndCoordsCompareResult.Match)
                            {
                                isFound = true;
                                break;
                            }
                        }
                        catch (CompareTypeException ex)
                        {
                            continue; //TODO log
                        }
                        catch(CompareGeometryException ex)
                        {
                            throw; //TODO log
                        }
                    }

                    if (!isFound &&
                        relGeomsCompareResults.IndexOf(GeomAndCoordsCompareResult.CoordsEqualButGeometryNot) != -1)
                        return GeomAndCoordsCompareResult.CoordsEqualButGeometryNot;
                    else if (!isFound &&
                        relGeomsCompareResults.IndexOf(GeomAndCoordsCompareResult.GeometryEqualButCoordsNot) != -1)
                        return GeomAndCoordsCompareResult.GeometryEqualButCoordsNot;
                    else if(!isFound)
                        return GeomAndCoordsCompareResult.NotFoundEqual;
                    
                }
#warning Всегда возвращается Match - ERROR
                return GeomAndCoordsCompareResult.Match;
            }
            catch(Exception ex)
            {
                throw new CompareGeometryException("compare error", ex);
            }
        }


        private bool IsEmptyGeometry(GeometryElement gE)
        {
            foreach (GeometryObject geometryObject in gE)
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


        private bool IsPointsAlmostEqual(XYZ point1, XYZ point2)
        {
            //IsAlmostEqualTo не подходит, так как в основном предназначен для векторного сравнения, а не для сравнения точек
            return point1.DistanceTo(point2) <= 1e-9; // 0.33 фута ~ 10 сантиметров
        }


        private IGeometryObjectComparer GetGeometryObjectComparer(GeometryObject obj)
        {
            if (obj is Solid)
                return new SolidComparer();
            else if (obj is GeometryInstance)
                return new GeometryInstanceComparer();
            else if (obj is Line)
                return new LineComparer();
            else
                throw new UnresolveGeometryObjectComparerTypeException
                    ($"Unresolve type for resolve comaprer. Type = {obj.GetType().Name}");
        }


        private IList<GeometryObject> GetRelativeObjectsTo(GeometryObject obj, IList<GeometryObject> sourceList)
        {
            List<Type> types = new List<Type>() { typeof(Solid), typeof(GeometryInstance), typeof(Line) };
            Type objType = obj.GetType();
            foreach (Type type in types)
            {
                if (objType.Equals(type))
                    return sourceList.Where(x => x.GetType().Equals(objType)).ToList();
            }
            throw new UnresolveGetRelativeGeometryTypeException
                ($"Unresolve type for get relative geometry objects. Type = {objType.Name}");
        }
    }
}
