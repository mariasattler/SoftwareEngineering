using SEUebung.Interfaces;
using SEUebung.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            ((List<IPlugin>)Plugins).Add(new NavigationPlugin());
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
            Assembly testAssembly = Assembly.LoadFile(@"C:\Users\sattler\Documents\GitHub\SoftwareEngineering\SEUebung\SEUebung\Plugin\ClassLibrary1.dll");
            Type calcType = testAssembly.GetType("SEUebung.Plugin.IPlugin", true);
            Type[] pluginTypes = testAssembly.GetTypes();

            foreach (Type pluginType in pluginTypes)
            {
                if (pluginType.GetInterface("SEUebung.Interfaces.IPlugin") == null)
                {
                    continue;
                }
                //IPlugin s = (IPlugin) Activator.CreateInstance(pluginType);
                
               // Add((obj) newd);
            }
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
