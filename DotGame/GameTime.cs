using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    /// <summary>
    /// Gibt die Zeit an, wie lange das Spiel schon läuft und wie viel Zeit seit der letzten Aktualisierung vergangen ist.
    /// </summary>
    public class GameTime
    {
        /// <summary>
        /// Gibt die Zeit an, wie lange das Spiel schon läuft.
        /// </summary>
        public TimeSpan TotalTime { get; private set; }

        /// <summary>
        /// Die Zeit die seit der letzten Aktualisierung vergangen ist.
        /// </summary>
        public TimeSpan LastFrameTime { get; private set; }

        public GameTime()
        {
            this.TotalTime = new TimeSpan();
            this.LastFrameTime = new TimeSpan();
        }

        public GameTime(TimeSpan totalTime, TimeSpan lastFrameTime)
        {
            this.TotalTime = totalTime;
            this.LastFrameTime = lastFrameTime;
        }
    }
}
