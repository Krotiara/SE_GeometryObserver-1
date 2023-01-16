using SE_GeometryObserver.Entities;
using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Service.Service
{
    public class CompareResultsLoader : ICompareResultsLoader<GeometryCompareResult>
    {
        public ICommonCompareResults<GeometryCompareResult> Load(string path)
        {
            throw new NotImplementedException();
        }

        public void Save(ICommonCompareResults<GeometryCompareResult> compareResults)
        {
            throw new NotImplementedException();
        }
    }
}
