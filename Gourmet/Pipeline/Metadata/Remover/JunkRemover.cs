using dnlib.DotNet.Emit;
using Gourmet.Core;
using System.Drawing;
using System.Linq;

namespace Gourmet.Pipeline.Metadata.Remover
{
    internal class JunkRemover
    {
        private static bool Detected = false;
        private static int Junk = 0;

        public static void Remover(Context ctx)
        {
            foreach (var typeDef in ctx.moduleDef.Types.ToList())
            {
                foreach (var methodDef in typeDef.Methods.ToList())
                {

                    if (methodDef.Body.Instructions.Count <= 1)
                    { 
                        Instruction instr = methodDef.Body.Instructions[0];

                        if (instr.OpCode.Code == Code.Ldfld)
                        {
                            Detected = true;
                            Junk++;
                            typeDef.Methods.Remove(methodDef);
                        }

                        if (!methodDef.HasBody)
                        {
                            Detected = true;
                            Junk++;
                            typeDef.Methods.Remove(methodDef);
                        }
                    }
                }
            }

            switch (Detected)
            {
                case false:
                    Utilities.Logger.Log("!", $"AntiDnspy protection has not been found!", Color.Red);
                    break;

                case true:
                    Utilities.Logger.Log("!", $"Successfully removed {Junk} junk methods and classes!", Color.Green);
                    break;
            }
        }
    }
}