using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalLib.Database
{
    public interface IIdentifiableMultiKey<TKey1, TKey2>
    {
        (TKey1, TKey2) GetId();
        string GetKeyName1();
        string GetKeyName2();
    }
}
