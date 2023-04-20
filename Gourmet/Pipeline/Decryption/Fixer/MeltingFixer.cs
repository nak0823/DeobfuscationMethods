using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Gourmet.Core;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Gourmet.Pipeline.Decryption.Fixer
{
    internal class MeltFixer
    {
        private static bool Detected = false;
        private static int Replaced = 0;
        public static void Remover(Context ctx)
        {
            foreach (var typeDef in ctx.moduleDef.GetTypes().ToList())
            {
                foreach (MethodDef methodDef in typeDef.Methods.ToList())
                {
                    if (!methodDef.HasBody) continue;
                    
                    for (int i = 0; i < methodDef.Body.Instructions.Count; i++)
                    {
                        if (methodDef.Body.Instructions[i].OpCode == OpCodes.Call &&
                            methodDef.Body.Instructions[i].Operand is MethodDef meltCall &&
                            meltCall.Parameters.Count == 0)
                        {
                            if (meltCall.Body.Instructions.Count == 2)
                            {
                                if (meltCall.Body.Instructions[0].OpCode.Code == Code.Ldc_I4 ||
                                    meltCall.Body.Instructions[0].OpCode.Code == Code.Ldstr &&
                                    meltCall.Body.Instructions[1].OpCode.Code == Code.Ret)
                                {
                                    Replaced++;
                                    Detected = true;
                                    switch (meltCall.Body.Instructions[0].OpCode.Code)
                                    {
                                        case Code.Ldstr:
                                            var meltString = (string)meltCall.Body.Instructions[0].Operand;
                                            methodDef.Body.Instructions[i] = Instruction.Create(OpCodes.Ldstr, meltString);
                                            typeDef.Methods.Remove(meltCall);
                                            break;

                                        case Code.Ldc_I4:
                                            var meltInt = (int)meltCall.Body.Instructions[0].Operand;
                                            methodDef.Body.Instructions[i] = Instruction.Create(OpCodes.Ldc_I4, meltInt);
                                            typeDef.Methods.Remove(meltCall);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            switch (Detected)
            {
                case false:
                    Utilities.Logger.Log("!", $"Const Melting protection has not been found!", Color.Red);
                    break;

                case true:
                    Utilities.Logger.Log("!", $"Successfully replaced {Replaced} strings/ints!", Color.Green);
                    break;
            }
        }

        private static bool CanReplace(MethodDef methodDef)
        {
            List<Instruction> instructions = new List<Instruction>(methodDef.Body.Instructions);

            if (instructions[0].OpCode == OpCodes.Ldstr && instructions[1].OpCode == OpCodes.Ret &&
                instructions.Count == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}