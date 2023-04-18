using dnlib.DotNet;
using dnlib.DotNet.Writer;
using Gourmet.Core;
using System;

namespace Gourmet
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Environment.Exit(0);
            }

            AssemblyDef assembly = AssemblyDef.Load(args[0]);
            Context ctx = new Context(assembly);

            Pipeline.Metadata.Remover.AntiDe4dot.Remove(ctx);

            var Options = new ModuleWriterOptions(assembly.ManifestModule);
            Options.Logger = DummyLogger.NoThrowInstance;
            assembly.Write(args[0].Replace(".exe", "-cleaned.exe"), Options);

            Console.ReadLine();
        }
    }
}