using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public static class ExceptionExtensions
    {
        public static Exception GetInnerMostException(this Exception e)
        {
            if (e.InnerException == null)
                return e;
            return GetInnerMostException(e.InnerException);
        }
    }
}