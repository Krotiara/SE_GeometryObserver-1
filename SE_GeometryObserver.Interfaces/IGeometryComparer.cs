using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Interfaces
{  
    public interface IGeometryComparer
    {
        public IList<IGeometryCompareResult> Compare(IList<IGeometryElementModel> rootList, IList<IGeometryElementModel> compareList);
    }
}
