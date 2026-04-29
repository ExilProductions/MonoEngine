using System;
using Microsoft.Xna.Framework;

namespace MonoGameTemplate.Engine.Components
{
    public class Transform
    {
        private Vector3 _position = Vector3.Zero;
        private Quaternion _rotation = Quaternion.Identity;
        private Vector3 _scale = Vector3.One;

        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public Quaternion Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        public Vector3 EulerAnglesDegrees
        {
            get
            {
                var matrix = Matrix.CreateFromQuaternion(_rotation);
                float pitch = (float)Math.Asin(-matrix.M23);
                float yaw = (float)Math.Atan2(matrix.M13, matrix.M33);
                float roll = (float)Math.Atan2(matrix.M21, matrix.M22);
                return new Vector3(
                    MathHelper.ToDegrees(pitch),
                    MathHelper.ToDegrees(yaw),
                    MathHelper.ToDegrees(roll));
            }
            set
            {
                float pitch = MathHelper.ToRadians(value.X);
                float yaw = MathHelper.ToRadians(value.Y);
                float roll = MathHelper.ToRadians(value.Z);
                _rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
            }
        }

        public Vector3 Scale
        {
            get => _scale;
            set => _scale = value;
        }

        public Matrix LocalMatrix =>
            Matrix.CreateScale(_scale) *
            Matrix.CreateFromQuaternion(_rotation) *
            Matrix.CreateTranslation(_position);
    }
}
