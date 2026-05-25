using Microsoft.Xna.Framework;
using MonoGameTemplate.Engine.Components;
using MonoGameTemplate.Engine.SceneManagment;
using MonoGameTemplate.GameCode.Components;

namespace MonoGameTemplate.GameCode.Scenes
{
    public class DefaultScene : SceneBase
    {
        public override void OnSceneLoaded(Entity rootEntity)
        {
            var cameraEntity = new Entity("Main Camera");
            cameraEntity.Transform.Position = new Vector3(0f, 2f, 8f);
            cameraEntity.Transform.EulerAnglesDegrees = new Vector3(0f, 0f, -10f);
            rootEntity.AddChild(cameraEntity);
            cameraEntity.AddComponent<Camera>();

            var teapotEntity = new Entity("Teapot");
            teapotEntity.Transform.Position = Vector3.Zero;
            teapotEntity.Transform.Scale = Vector3.One;
            rootEntity.AddChild(teapotEntity);
            teapotEntity.AddComponent<OrbitDance>();

            var renderer = teapotEntity.AddComponent<MeshRenderer>();
            renderer.SetModel("Models/Teapot/teapot");
        }
    }
}
