using SEUebung.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung
{
    /// <summary>
    /// Plugin Manager
    /// </summary>
    public class PluginManager : IPluginManager
    {
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="plugins"></param>
        public PluginManager(IEnumerable<IPlugin> plugins)
        {
            Plugins = plugins;
        }

        /// <summary>
        /// returns a list of all Plugins
        /// </summary>
        public IEnumerable<IPlugin> Plugins { get; }
        /// <summary>
        /// Adds a Plugin to the PluginManager when given a Typ Plugin
        /// </summary>
        /// <param name="plugin"></param>
        public void Add(IPlugin plugin)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Adds a Plugin to the PluginManager when given a string
        /// </summary>
        /// <param name="plugin"></param>
        public void Add(string plugin)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// clears the Plugin
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
