using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Gourmet.Core;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Gourmet.Pipeline.Decryption.Fixer
{
    internal class StringDecrypt
    {
        private static bool Detected = false;
        private static int DecryptedStrings = 0;
        private static List<Instruction> JunkInstructions = new List<Instruction>();

        public static void Remover(Context ctx)
        {
            foreach (var typeDef in ctx.moduleDef.GetTypes())
            {
                foreach (MethodDef methodDef in typeDef.Methods)
                {
                    if (!methodDef.HasBody) continue;
                    
                    List<Instruction> instructions = new List<Instruction>(methodDef.Body.Instructions);

                    for (int i = instructions.Count - 1; i >= 0; i--)
                    {
                        if (instructions[i].OpCode.Code == Code.Ldstr &&
                            instructions[i + 1].OpCode.Code == Code.Ldc_I4 &&
                            instructions[i + 2].OpCode.Code == Code.Ldc_I4 &&
                            instructions[i + 3].OpCode.Code == Code.Ldc_I4 &&
                            instructions[i + 4].OpCode.Code == Code.Ldc_I4 &&
                            instructions[i + 5].OpCode.Code == Code.Ldc_I4 &&
                            instructions[i + 6].OpCode.Code == Code.Call)
                        {
                            var String = (string)instructions[i].Operand;
                            var Key_1 = (int)instructions[i + 1].Operand;
                            var Key_2 = (int)instructions[i + 2].Operand;
                            var Key_3 = (int)instructions[i + 3].Operand;
                            var Key_4 = (int)instructions[i + 4].Operand;
                            var Key_5 = (int)instructions[i + 5].Operand;

                            var Decrypt = DecryptString(String, Key_1, Key_2, Key_3, Key_4, Key_5);
                            Utilities.Logger.Log("!", $"Decrypted string with outcome: {Decrypt}", Color.Aqua);

                            // Replace Instructions
                            // Remove Instructions
                            DecryptedStrings++;
                            Detected = true;
                            methodDef.Body.Instructions.RemoveAt(i + 6);
                            methodDef.Body.Instructions.RemoveAt(i + 5);
                            methodDef.Body.Instructions.RemoveAt(i + 4);
                            methodDef.Body.Instructions.RemoveAt(i + 3);
                            methodDef.Body.Instructions.RemoveAt(i + 2);
                            methodDef.Body.Instructions.RemoveAt(i + 1);
                            methodDef.Body.Instructions[i].Operand = Decrypt;
                        }
                    }

                }
            }

            switch (Detected)
            {
                case false:
                    Utilities.Logger.Log("!", $"String Encryption protection has not been found!", Color.Red);
                    break;

                case true:
                    Utilities.Logger.Log("!", $"Successfully decrypted {DecryptedStrings} strings!", Color.Green);
                    break;
            }
        }

        public static string DecryptString(string A_0, int A_1, int A_2, int A_3, int A_4, int A_5)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in A_0.ToCharArray())
            {
                stringBuilder.Append((char)((int)c - A_2));
            }
            return stringBuilder.ToString();
        }
    }
}