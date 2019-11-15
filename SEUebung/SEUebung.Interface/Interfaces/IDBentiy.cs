using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung
{
    /// <summary>
    /// Interface DBEntity
    /// </summary>
    public interface IDBentiy
    {/// <summary>
    /// returns id
    /// </summary>
        int id { get;}
        /// <summary>
        /// returns day
        /// </summary>
        int day { get; }
        /// <summary>
        /// returns month
        /// </summary>
        int month { get; }
        /// <summary>
        /// returns year
        /// </summary>
        int year { get; }
        /// <summary>
        /// returns time
        /// </summary>
        string time { get; }
        /// <summary>
        /// return the temperature
        /// </summary>
        int temp { get; }
        
    }
}
