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
            var list = new List<EventHandler>();
            CollectChildHandlers(new List<EventHandler>(), list);
            foreach (var child in list)
                child.InvokeInternal(@event, isRequired, args);

            InvokeInternal(@event, isRequired, args);
        }

        private void InvokeInternal(string @event, bool isRequired, params object[] args)
        {
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
    }
}
