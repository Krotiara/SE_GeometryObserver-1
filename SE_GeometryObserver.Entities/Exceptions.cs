using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Entities
{
    public class UnresolveGeometryObjectComparerTypeException: Exception
    {
        public UnresolveGeometryObjectComparerTypeException(string message) : base(message)
        {

        }
    }


    public class UnresolveGetRelativeGeometryTypeException : Exception
    {
        public UnresolveGetRelativeGeometryTypeException(string message) : base(message)
        {

        }
    }


    public class CompareTypeException:Exception
    {
        public CompareTypeException(string message):base(message)
        {

        }

    }


    public class CompareGeometryException : Exception
    {
        public CompareGeometryException(string message) : base(message)
        {

        }

        public CompareGeometryException(string message, Exception ex): base(message,ex)
        {

        }

    }
}
