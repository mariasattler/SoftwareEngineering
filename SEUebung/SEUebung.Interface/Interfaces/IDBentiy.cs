using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung
{
    public interface IDBentiy
    {
        int id { get;}
        int day { get; }
        int month { get; }
        int year { get; }
        string time { get; }
        int temp { get; }
        
    }
}
