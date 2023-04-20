using Gourmet.Core;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Gourmet.Pipeline.Metadata.Remover
{
    internal class AttributeRemover
    {
        private static bool Detected = false;
        private static int Attributes = 0;

        private static List<string> FakeAttribs = new List<string>()
        {
            "Borland.Vcl.Types",
            "Borland.Eco.Interfaces",
            "();\t",
            "BabelObfuscatorAttribute",
            "ZYXDNGuarder",
            "DotfuscatorAttribute",
            "YanoAttribute",
            "Reactor",
            "EMyPID_8234_",
            "ObfuscatedByAgileDotNetAttribute",
            "ObfuscatedByGoliath",
            "CheckRuntime",
            "ObfuscatedByCliSecureAttribute",
            "____KILL",
            "CodeWallTrialVersion",
            "Sixxpack",
            "Microsoft.VisualBasic",
            "nsnet",
            "ConfusedByAttribute",
            "Macrobject.Obfuscator",
            "Protected_By_Attribute'00'NETSpider.Attribute",
            "CryptoObfuscator.ProtectedWithCryptoObfuscatorAttribute",
            "Xenocode.Client.Attributes.AssemblyAttributes.ProcessedByXenocode",
            "NineRays.Obfuscator.Evaluation",
            "SecureTeam.Attributes.ObfuscatedByAgileDotNetAttribute",
            "SmartAssembly.Attributes.PoweredByAttribute",
            "Sugary",
        };

        public static void Remover(Context ctx)
        {
            try
            {
                foreach (var typeDef in ctx.moduleDef.Types.ToList())
                {
                    bool hasAttribute =
                        typeDef.CustomAttributes.Any(attrib => attrib.AttributeType.FullName == "System.Attribute");
                    bool hasInheritedAttribute =
                        typeDef.BaseType != null && typeDef.BaseType.FullName == "System.Attribute";

                    if (FakeAttribs.Contains(typeDef.Name) && hasAttribute || hasInheritedAttribute)
                    {
                        Detected = true;
                        Attributes++;
                        ctx.moduleDef.Types.Remove(typeDef);
                    }
                }
            }
            catch
            {

            }


            switch (Detected)
            {
                case false:
                    Utilities.Logger.Log("!", $"Fake Attributes protection has not been found!", Color.Red);
                    break;

                case true:
                    Utilities.Logger.Log("!", $"Successfully removed {Attributes} fake attributes!", Color.Green);
                    break;
            }
        }
    }
}