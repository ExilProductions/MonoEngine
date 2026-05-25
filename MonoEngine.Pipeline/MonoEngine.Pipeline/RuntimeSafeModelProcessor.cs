using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace MonoEngine.Pipeline
{
    [ContentProcessor(DisplayName = "RuntimeSafeModelProcessor")]
    public class RuntimeSafeModelProcessor : ModelProcessor
    {
        // Material has an internal setter; we reach it via reflection.
        private static readonly PropertyInfo MaterialProperty =
            typeof(ModelMeshPartContent).GetProperty("Material",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;

        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            ModelContent model = base.Process(input, context);

            // Walk mesh parts to find the first built texture reference.
            string textureAssetName = null;
            foreach (ModelMeshContent mesh in model.Meshes)
            {
                if (textureAssetName != null) break;
                foreach (ModelMeshPartContent part in mesh.MeshParts)
                {
                    if (part.Material is BasicMaterialContent basic && basic.Texture != null)
                    {
                        // basic.Texture.Filename is the absolute path of the built texture XNB.
                        // Convert it to a content asset name relative to the output root.
                        var xnbAbs = basic.Texture.Filename;
                        var rel = Path.GetRelativePath(context.OutputDirectory, xnbAbs);
                        textureAssetName = Path.ChangeExtension(rel, null).Replace('\\', '/');
                        break;
                    }
                }
            }
            model.Tag = textureAssetName;

            // Null every material so ModelWriter emits null for the effect shared-resource
            // slot instead of a ReflectiveWriter<MaterialContent> reference.
            foreach (ModelMeshContent mesh in model.Meshes)
                foreach (ModelMeshPartContent part in mesh.MeshParts)
                    MaterialProperty.SetValue(part, null);

            return model;
        }
    }
}
