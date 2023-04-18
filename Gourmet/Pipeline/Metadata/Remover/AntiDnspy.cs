using dnlib.DotNet.Emit;
using Gourmet.Core;
using System.Collections.Generic;
using System.Drawing;

namespace Gourmet.Pipeline.Metadata.Remover
{
    internal class AntiDnspy
    {
        private static bool Detected = false;
        private static int RemovedNops = 0;

        public static void Remove(Context ctx)
        {
            foreach (var typeDef in ctx.moduleDef.Types)
            {
                foreach (var methodDef in typeDef.Methods)
                {
                    List<Instruction> InvalidNops = new List<Instruction>();

                    for (int i = 0; i < methodDef.Body.Instructions.Count; i++)
                    {
                        Instruction Instr = methodDef.Body.Instructions[i];

                        if (Instr.OpCode == OpCodes.Nop)
                        {
                            InvalidNops.Add(Instr);
                        }
                        else
                        {
                            break; // Break at the first instruction that is not a NOP.
                        }
                    }

                    if (InvalidNops.Count >= 12) // Remove the Nops when bigger than 12.
                    {
                        Detected = true;
                        for (int i = InvalidNops.Count - 1; i >= 0; i--)
                        {
                            RemovedNops++;
                            methodDef.Body.Instructions.RemoveAt(i);
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
                    Utilities.Logger.Log("!", $"Successfully removed {RemovedNops} useless NOP instructions!", Color.Green);
                    break;
            }
        }
    }
}