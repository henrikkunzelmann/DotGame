using System;

namespace DotGame
{
    /// <summary>
    /// Gibt die Zeit an, wie lange das Spiel schon läuft und wie viel Zeit seit der letzten Aktualisierung vergangen ist.
    /// </summary>
    public struct GameTime
    {
        /// <summary>
        /// Gibt die Zeit an, wie lange das Spiel schon läuft.
        /// </summary>
        public TimeSpan TotalTime { get; private set; }

        /// <summary>
        /// Die Zeit die seit der letzten Aktualisierung vergangen ist.
        /// </summary>
        public TimeSpan LastFrameTime { get; private set; }

        /// <summary>
        /// Die Ticks die seit Spielstart vergangen sind.
        /// </summary>
        public long TickCount { get; private set; }


        public GameTime(TimeSpan totalTime, TimeSpan lastFrameTime, long tickCount)
            : this()
        {
            this.TotalTime = totalTime;
            this.LastFrameTime = lastFrameTime;
            this.TickCount = tickCount;
        }
    }
}
