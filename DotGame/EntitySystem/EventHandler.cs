using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.EntitySystem
{
    /// <summary>
    /// Stellt einen EventHandler dar, der rekursive Events beinhaltet.
    /// </summary>
    public class EventHandler
    {
        /// TODO(Joex3): Cache global machen.
        private Dictionary<string, MethodInfo> eventCache = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// Prüft, ob ein Event mit dem gegebenen Namen existiert.
        /// </summary>
        /// <param name="event">Der Name des Events.</param>
        /// <returns>True, wenn das Event existiert, ansonsten false.</returns>
        public bool Defines(string @event)
        {
            var method = GetType().GetMethod(@event);
            return IsEvent(method);
        }

        /// <summary>
        /// Ruft das gegebene Event mit den angegebenen Parametern auf.
        /// </summary>
        /// <param name="event">Der Name des Events.</param>
        /// <param name="isRequired">Wenn true, wird eine InvalidOperationException geworfen, wenn das Event nicht existiert.</param>
        /// <param name="args">Die Parameterliste.</param>
        public void Invoke(string @event, bool isRequired, params object[] args)
        {
            foreach (var child in GetChildHandlers())
                child.Invoke(@event, isRequired, args);

            MethodInfo method;
            if (eventCache.TryGetValue(@event, out method))
            {
                if (method == null)
                {
                    if (isRequired)
                        throw new InvalidOperationException(string.Format("EventHandler doesn not define an event called \"{0}\".", @event));
                }
                else
                {
                    // TODO(Joex3): Parameterprüfung.
                    method.Invoke(this, args);
                }
            }
            else
            {
                method = GetType().GetMethod(@event, BindingFlags.Instance | BindingFlags.NonPublic);
                eventCache.Add(@event, method);
                if (IsEvent(method))
                    method.Invoke(this, args);
                else if (isRequired)
                    throw new InvalidOperationException(string.Format("EventHandler doesn not define an event called \"{0}\".", @event));
            }
        }

        protected virtual EventHandler[] GetChildHandlers()
        {
            return new EventHandler[0];
        }

        private bool IsEvent(MethodInfo method)
        {
            if (method == null)
                return false;

            var attributes = method.GetCustomAttributes(typeof(EventAttribute), true);
            return attributes.Length > 0;
        }
    }
}
