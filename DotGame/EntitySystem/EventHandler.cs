using DotGame.Utils;
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
        private static Dictionary<Type, Dictionary<string, EventCache>> cache = new Dictionary<Type, Dictionary<string, EventCache>>();

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
            var list = new List<EventHandler>();
            CollectChildHandlers(new List<EventHandler>(), list);
            foreach (var child in list)
                child.InvokeInternal(@event, isRequired, args);

            InvokeInternal(@event, isRequired, args);
        }

        private void InvokeInternal(string @event, bool isRequired, params object[] args)
        {
            var e = GetEvent(GetType(), @event);
            if (e == null && isRequired)
                throw new InvalidOperationException(string.Format("EventHandler doesn not define an event called \"{0}\".", @event));
            if (e != null)
                e.Method.Invoke(this, args);
        }

        private EventCache GetEvent(Type type, string @event)
        {
            Dictionary<string, EventCache> d;
            if (!cache.TryGetValue(type, out d))
            {
                d = new Dictionary<string, EventCache>();
                cache[type] = d;
            }

            EventCache e;
            if (!d.TryGetValue(@event, out e))
            {
                var method = GetType().GetMethod(@event, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method == null || !IsEvent(method))
                    return null;
                e = new EventCache(method);
                d[@event] = e;
            }
            return e;
        }

        protected virtual void GetChildHandlers(List<EventHandler> handlers)
        {
        }

        private void CollectChildHandlers(List<EventHandler> tmplist, List<EventHandler> result)
        {
            int start = result.Count;
            tmplist.Clear();
            GetChildHandlers(tmplist);
            result.AddRange(tmplist);
            int end = result.Count;
            for (int i = start; i < end; i++)
                result[i].CollectChildHandlers(tmplist, result);
        }

        private bool IsEvent(MethodInfo method)
        {
            if (method == null)
                return false;

            var attributes = method.GetCustomAttributes(typeof(EventAttribute), true);
            return attributes.Length > 0;
        }

        private class EventCache
        {
            public readonly MethodInfo Method;

            public EventCache(MethodInfo method)
            {
                if (method == null)
                    throw new ArgumentNullException("method");
                if (method.GetCustomAttributes(typeof(EventAttribute), true).Length == 0)
                    throw new InvalidOperationException("Given method is no event.");

                this.Method = method;
            }
        }
    }
}
