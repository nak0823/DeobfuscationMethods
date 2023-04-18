using dnlib.DotNet.Emit;
using Gourmet.Core;
using System.Drawing;
using dnlib.DotNet;
using Console = Colorful.Console;

namespace Gourmet.Pipeline.Metadata.Remover
{
    internal class InvalidOpcodes
    {
        private static bool Detected = false;
        private static int RemovedOpcodes = 0;

        public static void Remover(Context ctx)
        {
            foreach (var typeDef in ctx.moduleDef.Types)
            {
                foreach (var methodDef in typeDef.Methods)
                {
                    if (methodDef.Body.Instructions.Count > 0)
                    {
                        Instruction instr = methodDef.Body.Instructions[0];

                        if (instr.OpCode.Code == Code.Box && instr.Operand is TypeRef opTypeRef && opTypeRef.FullName == "System.Math")
                        {
                            Detected = true;
                            RemovedOpcodes++;
                            methodDef.Body.Instructions.RemoveAt(0);
                        }
                    }
                }
            }

            switch (Detected)
            {
                case false:
                    Utilities.Logger.Log("!", $"Fake Opcodes protection has not been found!", Color.Red);
                    break;

                case true:
                    Utilities.Logger.Log("!", $"Successfully removed {RemovedOpcodes} invalid OP instructions!", Color.Green);
                    break;
            }
        }
    }
}