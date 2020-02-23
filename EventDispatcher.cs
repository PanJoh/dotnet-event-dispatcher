using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HdcDst.Utils
{
    public class EventDispatcher
    {
        private readonly IDictionary<Type, object> eventActionMap;

        public EventDispatcher()
        {
            eventActionMap = new Dictionary<Type, object>();
        }

        public void AddEventHandler<T>(Func<T, Task> action)
        {
            eventActionMap.Add(typeof(T), action);
        }

        public void RemoveEventHandler<T>()
        {
            eventActionMap.Remove(typeof(T));
        }

        public bool HasHandler<T>()
        {
            return eventActionMap.ContainsKey(typeof(T));
        }

        public Task OnEvent<T>(T ev)
        {
            if(!eventActionMap.TryGetValue(typeof(T), out var obj))
            {
                return Task.FromException(new NotSupportedException($"No handler for event type {typeof(T)}"));
            }

            if (!(obj is  Func<T, Task> action))
            {
                return Task.FromException(new ArgumentException($"Handler of event {typeof(T)} is not of type {typeof(Func<T, Task>)}"));
            }

            return action(ev);
        }
    }
}
