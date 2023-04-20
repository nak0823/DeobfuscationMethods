using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Gourmet.Core;
using System.Drawing;

namespace Gourmet.Pipeline.Decryption.Fixer
{

    internal class IntDecrypt
    {
        private static bool Detected = false;

        public static void Remover(Context ctx)
        {
            foreach (var typeDef in ctx.moduleDef.GetTypes())
            {
                if (typeDef.IsGlobalModuleType) continue;

                foreach (MethodDef methodDef in typeDef.Methods)
                {
                    if (!methodDef.HasBody) continue;

                    List<Instruction> instructions = new List<Instruction>(methodDef.Body.Instructions);

                    Console.WriteLine(instructions[0].OpCode.Code);
                    Console.WriteLine(instructions[1].OpCode.Code);
                    Console.WriteLine(instructions[2].OpCode.Code);
                    Console.WriteLine(instructions[3].OpCode.Code);
                    Console.WriteLine(instructions[4].OpCode.Code);
                    Console.WriteLine(instructions[5].OpCode.Code);
                    Console.WriteLine(instructions[6].OpCode.Code);

                    if (instructions[0].OpCode.Code == Code.Ldc_I4 &&
                        instructions[1].OpCode.Code == Code.Ldc_I4 &&
                        instructions[2].OpCode.Code == Code.Ldc_I4 &&
                        instructions[3].OpCode.Code == Code.Xor &&
                        instructions[4].OpCode.Code == Code.Ldc_I4 &&
                        instructions[5].OpCode.Code == Code.Bne_Un &&
                        instructions[6].OpCode.Code == Code.Ldc_I4)
                    {
                      
                    }
                }
            }

            switch (Detected)
            {
                case true:
                    break;

                case false:
                    break;
            }
        }   


    }
}
