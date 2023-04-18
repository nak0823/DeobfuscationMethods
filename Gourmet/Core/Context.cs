using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Gourmet.Core
{
    internal class Context
    {
        public AssemblyDef assemblyDef;
        public ModuleDef moduleDef;
        public TypeDef typeDef;
        public Importer importer;
        public MethodDef methodDef;
        public ModuleDefMD ModfuleDefMd;

        public Context(AssemblyDef assembly)
        {
            assemblyDef = assembly;
            moduleDef = assembly.ManifestModule;
            typeDef = moduleDef.GlobalType;
            importer = new Importer(moduleDef);
            methodDef = typeDef.FindOrCreateStaticConstructor();
        }
    }
}