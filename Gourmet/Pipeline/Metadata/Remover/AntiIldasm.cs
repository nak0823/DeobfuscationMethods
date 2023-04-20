using dnlib.DotNet.Emit;
using Gourmet.Core;
using System.Collections.Generic;
using System.Drawing;

namespace Gourmet.Pipeline.Metadata.Remover
{
    internal class AntiIldasm
    {
        private static bool Detected = false;

        public static void Remover(Context ctx)
        {
            foreach (var typeDef in ctx.moduleDef.Types)
            {
                // TODO: Find a working Anti Ildasm
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