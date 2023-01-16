using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Service.Service
{
    public interface ICompareResultsLoader<T> where T : IGeometryCompareResult
    {
        public void Save(ICommonCompareResults<T> compareResults);

        public ICommonCompareResults<T> Load(string path);
    }
}
