using Microsoft.Xna.Framework;
using MonoGameTemplate.Engine.Components;
using MonoGameTemplate.Engine.SceneManagment;

namespace MonoGameTemplate.GameCode.Scenes
{
    public class DefaultScene : SceneBase
    {
        public override void OnSceneLoaded(Entity rootEntity)
        {
            var cameraEntity = new Entity("Main Camera");
            cameraEntity.Transform.Position = new Vector3(0f, 0f, 6f);
            rootEntity.AddChild(cameraEntity);

            var cubeObject = new Entity("Cube");
            cubeObject.Transform.Position = Vector3.Zero;
            cubeObject.Transform.Scale = new Vector3(2f, 2f, 2f);
            rootEntity.AddChild(cubeObject);
        }
    }
}
