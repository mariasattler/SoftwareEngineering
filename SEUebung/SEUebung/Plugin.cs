using SEUebung.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung
{
    public class Plugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            throw new NotImplementedException();
        }

        public IResponse Handle(IRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
