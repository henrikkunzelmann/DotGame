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
            Entity.Transform.LocalRotation = Quaternion.CreateFromYawPitchRoll((float)Math.Sin(t * 0.7f) * 0.1f, (float)Math.Cos(t * 0.1f) * 0.1f, (float)Math.Sin(t * 0.5) * 0.1f);
        }
    }
}
