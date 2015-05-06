using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    public static class TimerUtils
    {
        private static Dictionary<string, Stopwatch> watches = new Dictionary<string, Stopwatch>();

        public static void Start(string name)
        {
            watches[name] = Stopwatch.StartNew();
        }

        public static void Restart(string name)
        {
            watches[name].Restart();
        }

        public static void Stop(string name)
        {
            watches[name].Stop();
        }

        public static double GetTimeInMs(string name, int amount, bool stop)
        {
            if (stop)
                watches[name].Stop();
            return watches[name].ElapsedTicks / (double)Stopwatch.Frequency * 1000 / (double)amount;
        }

        public static string DefaultString(string name, int amount, bool stop)
        {
            return string.Format("Timer \"{0}\" took {1}ms.", name, GetTimeInMs(name, amount, stop));
        }

        public static string FormatString(string name, int amount, bool stop, string msg, params object[] args)
        {
            return string.Format(msg, GetTimeInMs(name, amount, stop), args);
        }
    }
}
