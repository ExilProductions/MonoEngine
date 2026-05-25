using System;
using Microsoft.Xna.Framework;
using MonoGameTemplate.Engine.Components;

namespace MonoGameTemplate.GameCode.Components
{
    public class OrbitDance : Component
    {
        public float OrbitRadius { get; set; } = 3f;
        public float OrbitSpeed { get; set; } = 1f;

        public float RotationSpeedX { get; set; } = 45f;
        public float RotationSpeedY { get; set; } = 90f;
        public float RotationSpeedZ { get; set; } = 30f;

        public float ScaleBase { get; set; } = 1f;
        public float ScaleAmplitude { get; set; } = 0.4f;
        public float ScalePulseSpeed { get; set; } = 2f;

        private float _time;
        private Vector3 _eulerDegrees;

        public override void OnUpdate(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _time += dt;

            float x = MathF.Cos(_time * OrbitSpeed) * OrbitRadius;
            float z = MathF.Sin(_time * OrbitSpeed) * OrbitRadius;
            Transform.Position = new Vector3(x, 0f, z);

            _eulerDegrees.X += RotationSpeedX * dt;
            _eulerDegrees.Y += RotationSpeedY * dt;
            _eulerDegrees.Z += RotationSpeedZ * dt;
            Transform.EulerAnglesDegrees = _eulerDegrees;

            float scale = ScaleBase + MathF.Sin(_time * ScalePulseSpeed) * ScaleAmplitude;
            Transform.Scale = new Vector3(scale);
        }
    }
}
