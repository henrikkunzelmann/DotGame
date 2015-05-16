using DotGame.EntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Test
{
    public class RotateComponent : Component
    {
        protected override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.TotalTime.TotalMilliseconds / 1000f;
            Entity.Transform.LocalRotation = Quaternion.CreateFromYawPitchRoll(50 * (float)Math.Sin(t * 0.01f) * (float)Math.Sin(t), 0, 0.2f * (float)Math.Sin(t));
        }
    }
}
