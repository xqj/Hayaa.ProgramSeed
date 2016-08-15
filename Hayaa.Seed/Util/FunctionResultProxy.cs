using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hayaa.Seed.Util
{
   internal class FunctionResultProxy
    {
       public static T GetResult<T>(Func<T,T> functionDo)  where T:class,new()
       {
           T r = new T();
           return functionDo(r);
       }
    }
}
