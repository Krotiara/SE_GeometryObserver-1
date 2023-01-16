using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Service.Service
{
    public interface ICalculateNearestGeomModelsService
    {
        public IList<IGeometryElementModel> GetNearest
            (IGeometryElementModel model, IList<IGeometryElementModel> source);
    }
}
