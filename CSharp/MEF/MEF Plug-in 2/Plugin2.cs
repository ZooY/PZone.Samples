using System;
using System.ComponentModel.Composition;

namespace PZone.Samples
{
    [Export(typeof(IPlugin))]
    public class Plugin2 : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("I am Plug-in 2!");
        }
    }
}