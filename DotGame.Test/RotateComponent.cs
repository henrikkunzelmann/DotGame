using DotGame.EntitySystem;
using System;
using System.Numerics;

namespace DotGame.Test
{
    public class RotateComponent : Component
    {
        public override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.TotalTime.TotalMilliseconds / 1000f;
            Entity.Transform.LocalRotation = Quaternion.CreateFromYawPitchRoll(50 * (float)Math.Sin(t * 0.01f) * (float)Math.Sin(t), 0, 0.2f * (float)Math.Sin(t));
        }
    }
}
