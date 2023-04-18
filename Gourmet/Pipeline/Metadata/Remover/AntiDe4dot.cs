using Gourmet.Core;
using System.Drawing;
using System.Linq;

namespace Gourmet.Pipeline.Metadata.Remover
{
    internal class AntiDe4dot
    {
        private static bool Detected = false;
        private static int RemovedInterfaces = 0;

        public static void Remove(Context ctx)
        {
            foreach (var typeDef in ctx.moduleDef.Types.ToList())
            {
                bool hasAttribute = typeDef.CustomAttributes.Any(attrib => attrib.AttributeType.FullName == "System.Attribute");
                bool hasInheritedAttribute = typeDef.BaseType != null && typeDef.BaseType.FullName == "System.Attribute";
                bool isEmptyClass = !typeDef.Fields.Any() && !typeDef.HasEvents && !typeDef.Methods.Any();

                if (hasAttribute || hasInheritedAttribute && isEmptyClass)
                {
                    Detected = true;
                    RemovedInterfaces++;
                    ctx.moduleDef.Types.Remove(typeDef);
                    Utilities.Logger.Log("!", $"Removed invalid interface {typeDef.Name}", Color.Aqua);
                }
            }

            switch (Detected)
            {
                case false:
                    Utilities.Logger.Log("!", $"AntiDe4dot protection has not been found!", Color.Red);
                    break;

                case true:
                    Utilities.Logger.Log("!", $"Successfully removed {RemovedInterfaces} invalid interfaces!", Color.Green);
                    break;
            }
        }
    }
}