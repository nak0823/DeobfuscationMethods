using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Gourmet.Core;
using System.Drawing;
using System.Linq;

namespace Gourmet.Pipeline.Metadata.Remover
{
    internal class AntiDump
    {
        private static bool Detected = false;

        public static void Remover(Context ctx) // This is useless to remove when you think about it. Still included.
        {
            try
            {
                foreach (var typeDef in ctx.moduleDef.Types.ToList())
                {
                    foreach (var methodDef in typeDef.Methods.ToList())
                    {
                        if (methodDef.Body != null)
                        {
                            foreach (var instruction in methodDef.Body.Instructions)
                            {
                                if (instruction.OpCode == OpCodes.Call &&
                                    instruction.Operand is MethodDef targetMethod &&
                                    targetMethod.Name == "VirtualProtect" && methodDef.Body.Instructions.Count > 2)
                                {
                                    Detected = true;
                                    typeDef.Methods.Remove(methodDef);

                                    // Now remove the call from cctor
                                    var methodName = methodDef.Name;
                                    var cctor = typeDef.FindOrCreateStaticConstructor();

                                    for (int i = 0; i < cctor.Body.Instructions.Count; i++)
                                    {
                                        var instr = cctor.Body.Instructions[i];

                                        if (instr.OpCode == OpCodes.Call &&
                                            instr.Operand is MethodDef cctorCallMethod &&
                                            cctorCallMethod.Name == methodName)
                                        {
                                            cctor.Body.Instructions.RemoveAt(i);
                                            i--;
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            switch (Detected)
            {
                case false:
                    Utilities.Logger.Log("!", $"Anti Dump protection has not been found!", Color.Red);
                    break;

                case true:
                    Utilities.Logger.Log("!", $"Successfully removed the anti dump Class!", Color.Green);
                    break;
            }
        }
    }
}