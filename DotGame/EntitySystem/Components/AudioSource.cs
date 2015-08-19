using DotGame.Audio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.EntitySystem.Components
{
    public class AudioSource : Component
    {
        [JsonIgnore]
        // TODO (Joex3): SoundInstance über Json/Bson serialisierbar machen.
        public ISoundInstance Sound { get { return sound; } set { sound = value; ApplySettings(); } }
        private ISoundInstance sound;

        public override void Update(GameTime gameTime)
        {
            ApplySettings();
        }

        public override void Destroy()
        {
            base.Destroy();

            sound.Stop();
        }

        private void ApplySettings()
        {
            if (Sound != null)
            {
                var position = Sound.Position;
                Sound.Position = Entity.Transform.Position;
                Sound.Velocity = Sound.Position - position;
            }
        }
    }
}
