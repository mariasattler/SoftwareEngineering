using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung
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

        public int id{ get; private set; }

        public int day { get; private set; }

        public int month { get; private set; }

        public int year { get; private set; }

        public string time { get; private set; }

        public int temp { get; private set; }
    }
}
