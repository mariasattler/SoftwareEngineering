using SEUebung.Interfaces;
using SEUebung.Plugin;
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
        
        public PluginManager()
        {
            Plugins = new List<IPlugin>();
            ((List<IPlugin>)Plugins).Add(new StartPlugin());
            ((List<IPlugin>)Plugins).Add(new StaticDataPlugin());
            ((List<IPlugin>)Plugins).Add(new ToLowerPlugin());
            ((List<IPlugin>)Plugins).Add(new GetTemperature());

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
            if (!Plugins.Contains(plugin))
            {
                ((List<IPlugin>)Plugins).Add(plugin);
            }
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
            ((List<IPlugin>)Plugins).Clear();
        }
    }
}
