using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Gourmet.Core;
using System;

namespace Gourmet.Pipeline.Decryption.Fixer
{
    internal class EncodeFixer
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

                    for (int i = 0; i < methodDef.Body.Instructions.Count; i++)
                    {
                        Instruction instruction = methodDef.Body.Instructions[i];

                        if (instruction.OpCode == OpCodes.Call &&
                            instruction.Operand is IMethod method &&
                            method.DeclaringType.FullName == typeof(Math).FullName &&
                            method.Name == "Min" &&
                            method.MethodSig.Params.Count == 2)
                        {
                            if (i < 2 || methodDef.Body.Instructions[i - 2].Operand == null || !(methodDef.Body.Instructions[i - 1].Operand is Instruction))
                            {
                                continue;
                            }


                            int operand = (int)methodDef.Body.Instructions[i - 2].Operand;
                            int neg = (int)(methodDef.Body.Instructions[i - 1].Operand as Instruction).Operand;

                            if (operand < int.MaxValue)
                            {
                                operand = Math.Min(operand, int.MaxValue);
                            }

                            for (int j = 0; j < neg; j++)
                            {
                                operand = -operand;
                            }
                            operand = Math.Abs(operand);



                            Console.WriteLine($"Found call to Math.Min({operand}, {int.MaxValue}) in method {methodDef.Name}");
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

        private static int DecodeInt(int encodedValue)
        {
            int absValue = Math.Abs(encodedValue);

            int negCount = 0;

            while ((absValue & 1) == 0)
            {
                absValue >>= 1;
                negCount++;
            }

            int decodedValue = absValue;
            for (int i = 0; i < negCount; i++)
                decodedValue = -decodedValue;

            return decodedValue;
        }
    }
}