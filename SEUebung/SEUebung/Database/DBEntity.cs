using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung.Database
{
    public class DBEntity : IDBentiy
    {
        public DBEntity(int id, int day, int month, int year, int temp, string time)
        {
            this.id = id;
            this.day = day;
            this.month = month;
            this.year = year;
            this.temp = temp;
            this.time = time;
        }
        /// <summary>
        /// returns the id
        /// </summary>
        public int id{ get; private set; }
        /// <summary>
        /// returns the day
        /// </summary>
        public int day { get; private set; }
        /// <summary>
        /// returns the month
        /// </summary>
        public int month { get; private set; }
        /// <summary>
        /// returns the year
        /// </summary>
        public int year { get; private set; }
        /// <summary>
        /// returns the time
        /// </summary>
        public string time { get; private set; }
        /// <summary>
        /// returns the temperature
        /// </summary>
        public int temp { get; private set; }
    }
}
