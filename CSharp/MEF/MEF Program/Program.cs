using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace PZone.Samples
{
    class Program
    {
        [ImportMany(typeof(IPlugin))]
        public IEnumerable<IPlugin> Plugins;


        static void Main()
        {
            var program = new Program();
            program.Run();
            Console.ReadKey();
        }


        private Program()
        {
            var catalog = new AggregateCatalog();
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyDirectory = Path.GetDirectoryName(assembly.Location);
            catalog.Catalogs.Add(
                new DirectoryCatalog(assemblyDirectory)
            );
            var container = new CompositionContainer(catalog);
            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }


        public void Run()
        {
            foreach (var plugin in Plugins)
                plugin.Execute();
        }
    }
}