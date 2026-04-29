using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameTemplate.Engine.Components;

namespace MonoGameTemplate.GameCode.Components
{
    public class MoveTransform : Component
    {
        Transform targetTransform;

        public override void OnAdded()
        {
            targetTransform = Transform;
        }

        public override void OnUpdate(GameTime gameTime)
        {
            var oldRotationY = targetTransform.EulerAnglesDegrees.Y;
            oldRotationY += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            targetTransform.EulerAnglesDegrees = new Vector3(targetTransform.EulerAnglesDegrees.X, oldRotationY, targetTransform.EulerAnglesDegrees.Z);
        }
    }
}
