using System;
using Gourmet.Core;
using System.Drawing;
using System.Linq;
using dnlib.DotNet;
using System.Collections.Generic;
using dnlib.DotNet.Emit;

namespace Gourmet.Pipeline.Metadata.Fixer
{
    internal class InvalidMD
    {
        private static bool Detected = false;
        private static int FixedMethods = 0;

        public static void Remover(Context ctx)
        {
            foreach (var typeDef in ctx.moduleDef.Types.ToList())
            {
                foreach (MethodDef methodDef in typeDef.Methods)
                {
                    if (!methodDef.HasBody) continue;
                    List<Instruction> instructions = new List<Instruction>(methodDef.Body.Instructions);

                    if (!(instructions.Count >= 13)) continue;

                    if (instructions[0].OpCode.Code == Code.Ldc_I4_0 && instructions[12].OpCode.Code == Code.Calli && instructions[13].OpCode.Code == Code.Sizeof)
                    {
                        FixedMethods++;
                        Detected = true;

                       for (int i = 0; i < 13; i++)
                        {
                            
                            methodDef.Body.Instructions.Remove(instructions[i]);
                        }
                    }

                }
            }

            switch (Detected)
            {
                case false:
                    Utilities.Logger.Log("!", $"AntiDe4dot protection has not been found!", Color.Red);
                    break;

                case true:
                    Utilities.Logger.Log("!", $"Successfully removed {FixedMethods} invalid interfaces!", Color.Green);
                    break;
            }
        }
    }
}